using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JsonRpc.DynamicProxy.Client;
using JsonRpc.Client;
using JsonRpc.Contracts;
using LanguageServer.VsCode.Contracts;
using LanguageServer.VsCode.Contracts.Client;
using LanguageServer.VsCode.Server;
using Microsoft.Extensions.Logging;

namespace Lextm.ReStructuredText.LanguageServer
{
    public class SessionState
    {
        private readonly CancellationTokenSource cts = new CancellationTokenSource();
        private readonly ILoggerFactory loggerFactory;

        private readonly ILogger logger;

        private int needInferPageInfo = 1;

        public SessionState(JsonRpcClient rpcClient, JsonRpcContractResolver contractResolver)
        {
            RpcClient = rpcClient ?? throw new ArgumentNullException(nameof(rpcClient));
            var builder = new JsonRpcProxyBuilder { ContractResolver = contractResolver };
            Client = new ClientProxy(builder, rpcClient);
            DocumentStates = new ConcurrentDictionary<Uri, DocumentState>();
            Project = new SphinxProject();
        }

        public CancellationToken CancellationToken => cts.Token;

        public JsonRpcClient RpcClient { get; }

        public ClientProxy Client { get; }

        public ConcurrentDictionary<Uri, DocumentState> DocumentStates { get; } = new ConcurrentDictionary<Uri, DocumentState>();

        public ReStructuredTextSettings Settings { get; set; } = new ReStructuredTextSettings();

        //public PageInfoStore PageInfoStore { get; } //= new PageInfoStore();

        public ReStructuredTextParser WiktextParser { get; }
        public SphinxProject Project { get; private set; }

        public DocumentState AddOrUpdateDocument(TextDocumentItem doc)
        {
            var ds = new DocumentState(TextDocument.Load<FullTextDocument>(doc), WiktextParser, loggerFactory);
            DocumentStates[doc.Uri] = ds;
            Attach(ds);
            return ds;
        }

        private void Attach(DocumentState doc)
        {
            doc.DocumentChanged += DocumentState_DocumentChanged;
            doc.DocumentLinted += DocumentState_DocumentLinted;
        }

        public bool RemoveDocument(Uri documentUri)
        {
            if (DocumentStates.TryRemove(documentUri, out var doc))
            {
                Detach(doc);
                return true;
            }
            return false;
        }

        public void Detach(DocumentState doc)
        {
            doc.DocumentChanged -= DocumentState_DocumentChanged;
            doc.DocumentLinted -= DocumentState_DocumentLinted;
        }

        private void DocumentState_DocumentChanged(object sender, EventArgs e)
        {
            var d = (DocumentState)sender;
            d.RequestAnalysis();
        }

        private void DocumentState_DocumentLinted(object sender, EventArgs args)
        {
            var d = (DocumentState)sender;
            //ClientProxy.TextDocument.PublishDiagnostics(d.TextDocument.Uri, d.LintedDocument.Diagnostics);
            // Infer pages upon document open.
            if (Interlocked.Exchange(ref needInferPageInfo, 0) != 0)
            {
                //                var inferredCount = d.LintedDocument.InferTemplateInformation(PageInfoStore);
                //                logger.LogDebug("Inferred {count} templates from {document}.", inferredCount,
                //                    d.TextDocument.Uri);
            }
        }

        public void StopServer()
        {
            cts.Cancel();
        }
    }

    /// <summary>
    /// Mutable document state.
    /// </summary>
    public class DocumentState
    {

        public DocumentState(TextDocument doc, ReStructuredTextParser parser, ILoggerFactory loggerFactory)
        {
            //if (loggerFactory == null) throw new ArgumentNullException(nameof(loggerFactory));
            //Logger = loggerFactory.CreateLogger<DocumentState>();
            TextDocument = doc ?? throw new ArgumentNullException(nameof(doc));
            WikitextLinter = parser;
            // DocumentLinter = new TextDocumentLinter(this);
            Synchronizer = new TextDocumentSynchronizer(this);
            // Caller's responsibility.
            // DocumentLinter.RequestLint();
        }

        public event EventHandler DocumentChanged;

        public event EventHandler DocumentLinted;

        public Document LintedDocument { get; private set; }

        public TextDocument TextDocument { get; private set; }

        //private readonly ILogger Logger;

        private readonly TextDocumentSynchronizer Synchronizer;

        //private readonly TextDocumentLinter DocumentLinter;     // A WikitextLinter with delay

        private readonly ReStructuredTextParser WikitextLinter;         // Not thread-safe

        public void NotifyChanges(IEnumerable<TextDocumentContentChangeEvent> changes)
        {
            if (changes == null) return;
            Synchronizer.NotifyChanges(changes);
        }

        public void RequestAnalysis()
        {
            //DocumentLinter.RequestAnalyze();
        }

        /// <summary>
        /// Apply impending changes to <see cref="TextDocument"/> right now.
        /// </summary>
        public Task ApplyChagesAsync()
        {
            return Synchronizer.ApplyChangesAsync();
        }

        /// <summary>
        /// Apply impending changes to <see cref="LintedDocument"/> right now.
        /// </summary>
        public void AnalyzeAsync()
        {            
            //await Synchronizer.ApplyChangesAsync();
            //await DocumentLinter.AnalyzeAsync(ct);
            //if (LintedDocument == null || TextDocument.)
            {
                LintedDocument = ReStructuredTextParser.ParseContent(TextDocument.Content);
            }
        }

        protected virtual void OnDocumentChanged()
        {
            DocumentChanged?.Invoke(this, EventArgs.Empty);
            // Update the AST, btw.
            //DocumentLinter.RequestAnalyze();
        }

        protected virtual void OnDocumentLinted()
        {
            DocumentLinted?.Invoke(this, EventArgs.Empty);
        }

        private class TextDocumentSynchronizer
        {
            public TextDocumentSynchronizer(DocumentState owner)
            {
                if (owner == null) throw new ArgumentNullException(nameof(owner));
                Owner = owner;
            }

            public DocumentState Owner { get; }

            private List<TextDocumentContentChangeEvent> impendingChanges;

            private readonly object syncLock = new object();

            private Task applyChangesTask;      // Running task

            private int willApplyChanges;

            public void NotifyChanges(IEnumerable<TextDocumentContentChangeEvent> changes)
            {
                if (changes == null) throw new ArgumentNullException(nameof(changes));
                if (changes is ICollection<TextDocumentContentChangeEvent> col && col.Count == 0) return;
                lock (syncLock)
                {
                    if (impendingChanges == null)
                        impendingChanges = changes.ToList();
                    else
                        impendingChanges.AddRange(changes);
                }
                if (Interlocked.Exchange(ref willApplyChanges, 1) == 0)
                {
                    // Note: If we're currently in TryMakeChanges, willMakeChanges should be 0
                    var delay = 100;
                    var doc = Owner.TextDocument;
                    // Wait for 1 sec for ever 50k-character content.
                    if (doc != null) delay = Math.Max(delay, doc.Content.Length / 50);
                    Task.Delay(delay).ContinueWith(t => ApplyChangesAsync());
                }
            }

            public Task ApplyChangesAsync()
            {
                lock (syncLock)
                {
                    if (applyChangesTask == null)
                    {
                        applyChangesTask = Task.Run((Action)ApplyChangesCore);
                    }
                    return applyChangesTask;
                }
            }

            private void ApplyChangesCore()
            {
                try
                {
                    Interlocked.Exchange(ref willApplyChanges, 0);
                    while (true)
                    {
                        List<TextDocumentContentChangeEvent> localChanges;
                        // Pick up the changes.
                        lock (syncLock)
                        {
                            localChanges = impendingChanges;
                            if (localChanges == null || localChanges.Count == 0) return;
                            impendingChanges = null;
                        }
                        //Owner.Logger.LogDebug(0, "Making changes to {document}.", Owner.TextDocument.Uri);
                        // Make the changes.
                        var doc = Owner.TextDocument.ApplyChanges(localChanges);
                        Owner.TextDocument = doc;
                        // We have done the changes.
                        if (impendingChanges == null)
                        {
                            localChanges.Clear();
                            lock (syncLock)
                            {
                                if (impendingChanges == null)
                                    impendingChanges = localChanges;
                            }
                        }
                        Owner.OnDocumentChanged();
                    }
                }
                catch (Exception ex)
                {
                    //Owner.Logger.LogError(0, ex, "Error making changes to {document}.", Owner.TextDocument.Uri);
                }
                finally
                {
                    lock (syncLock) applyChangesTask = null;
                    //Owner.Logger.LogDebug(0, "Finished making changes to {document}.", Owner.TextDocument.Uri);
                }
            }
        }

        private class TextDocumentLinter
        {
            /// <summary>
            /// Actually lint the document per this milliseconds.
            /// </summary>
            private const int RenderChangesDelay = 100;

            public TextDocumentLinter(DocumentState owner)
            {
                if (owner == null) throw new ArgumentNullException(nameof(owner));
                Owner = owner;
            }

            public DocumentState Owner { get; }

            private readonly object syncLock = new object();

            private Task analyzeTask;

            private int willLint = 0;

            private int impendingRequests = 0;

            /// <summary>
            /// Request for linting the document, without any condition.
            /// </summary>
            public void RequestAnalyze()
            {
                Interlocked.Exchange(ref impendingRequests, 1);
                if (Interlocked.Exchange(ref willLint, 1) == 0)
                {
                    Task.Delay(RenderChangesDelay).ContinueWith(t => AnalyzeAsync(CancellationToken.None));
                }
            }

            public Task AnalyzeAsync(CancellationToken ct)
            {
                if (ct.IsCancellationRequested) return Task.FromCanceled(ct);
                lock (syncLock)
                {
                    if (analyzeTask == null)
                    {
                        analyzeTask = Task.Factory.StartNew(o => AnalyzeCore((CancellationToken)o), ct, ct);
                    }
                    return analyzeTask;
                }
            }

            private void AnalyzeCore(CancellationToken ct)
            {
                try
                {
                    Interlocked.Exchange(ref willLint, 0);
                    while (Interlocked.Exchange(ref impendingRequests, 0) > 0)
                    {
                        ct.ThrowIfCancellationRequested();
                        //Owner.Logger.LogDebug(0, "Start analyzing {document}.", Owner.TextDocument.Uri);
                        var doc = Owner.TextDocument;
                        //var linted = Owner.WikitextLinter.Lint(doc, ct);
                        var linted = ReStructuredTextParser.ParseContent(doc.Content);
                        // document has been changed!
                        // then just wait for another RequestLint()
                        if (doc != Owner.TextDocument) continue;
                        Owner.LintedDocument = linted;
                        Owner.OnDocumentLinted();
                    }
                }
                catch (Exception ex)
                {
                    //Owner.Logger.LogError(0, ex, "Error analyzing {document}.", Owner.TextDocument.Uri);
                }
                finally
                {
                    lock (syncLock) analyzeTask = null;
                    //Owner.Logger.LogDebug(0, "Finished analyzing {document}.", Owner.TextDocument.Uri);
                }
            }
        }
    }
}
