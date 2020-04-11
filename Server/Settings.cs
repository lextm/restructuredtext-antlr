namespace Lextm.ReStructuredText.LanguageServer
{

    public class SettingsRoot
    {
        public ReStructuredTextSettings ReStructuredText { get; set; }
    }

    public class ReStructuredTextSettings
    {
        public string ConfPath { get; set; }
        public LanguageServerSettings LanguageServer { get; set; } = new LanguageServerSettings();
    }

    public class LanguageServerSettings
    {
        public int MaxNumberOfProblems { get; set; } = 10;

        public LanguageServerTraceSettings Trace { get; } = new LanguageServerTraceSettings();
    }

    public class LanguageServerTraceSettings
    {
        public string Server { get; set; }
    }
}
