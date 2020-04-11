﻿//#define WAIT_FOR_DEBUGGER

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using JsonRpc.Client;
using JsonRpc.Contracts;
using JsonRpc.Server;
using JsonRpc.Streams;
using LanguageServer.VsCode;
using Microsoft.Extensions.Logging;

namespace Lextm.ReStructuredText.LanguageServer
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var debugMode = args.Any(a => a.Equals("--debug", StringComparison.OrdinalIgnoreCase));
#if WAIT_FOR_DEBUGGER
            while (!Debugger.IsAttached) Thread.Sleep(5000);
            Debugger.Break();
#endif
            StreamWriter logWriter = null;
            if (debugMode)
            {
                logWriter = File.CreateText("messages-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".log");
                logWriter.AutoFlush = true;
            }

            using (logWriter)
            using (var cin = Console.OpenStandardInput())
            using (var bcin = new BufferedStream(cin))
            using (var cout = Console.OpenStandardOutput())
            using (var reader = new PartwiseStreamMessageReader(bcin))
            using (var writer = new PartwiseStreamMessageWriter(cout))
            {
                var contractResolver = new JsonRpcContractResolver
                {
                    NamingStrategy = new CamelCaseJsonRpcNamingStrategy(),
                    ParameterValueConverter = new CamelCaseJsonValueConverter(),
                };
                var clientHandler = new StreamRpcClientHandler();
                var client = new JsonRpcClient(clientHandler);
                if (debugMode)
                {
                    // We want to capture log all the LSP server-to-client calls as well
                    clientHandler.MessageSending += (_, e) =>
                    {
                        lock (logWriter) logWriter.WriteLine("<C{0}", e.Message);
                    };
                    clientHandler.MessageReceiving += (_, e) =>
                    {
                        lock (logWriter) logWriter.WriteLine(">C{0}", e.Message);
                    };
                }

                // Configure & build service host
                var session = new SessionState(client, contractResolver);
                var host = BuildServiceHost(logWriter, contractResolver, debugMode);
                var serverHandler = new StreamRpcServerHandler(host,
                    StreamRpcServerHandlerOptions.ConsistentResponseSequence |
                    StreamRpcServerHandlerOptions.SupportsRequestCancellation);
                serverHandler.DefaultFeatures.Set(session);
                // If we want server to stop, just stop the "source"
                using (serverHandler.Attach(reader, writer))
                using (clientHandler.Attach(reader, writer))
                {
                    // Wait for the "stop" request.
                    session.CancellationToken.WaitHandle.WaitOne();
                }

                logWriter?.WriteLine("Exited");
            }
        }

        private static IJsonRpcServiceHost BuildServiceHost(TextWriter logWriter,
            IJsonRpcContractResolver contractResolver, bool debugMode)
        {
            var loggerFactory = LoggerFactory.Create(builder => builder.AddDebug());
            if (debugMode)
            {
                loggerFactory.AddFile("logs-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".log");
            }
            
            var builder = new JsonRpcServiceHostBuilder
            {
                ContractResolver = contractResolver,
                LoggerFactory = loggerFactory
            };
            builder.UseCancellationHandling();
            builder.Register(typeof(Program).GetTypeInfo().Assembly);
            if (debugMode)
            {
                // Log all the client-to-server calls.
                builder.Intercept(async (context, next) =>
                {
                    lock (logWriter) logWriter.WriteLine("> {0}", context.Request);
                    await next();
                    lock (logWriter) logWriter.WriteLine("< {0}", context.Response);
                });
            }

            return builder.Build();
        }

    }
}