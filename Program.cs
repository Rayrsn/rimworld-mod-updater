using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using Serilog;

namespace RimworldModUpdater
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (File.Exists("log.txt"))
                File.Delete("log.txt");

            Log.Logger = new LoggerConfiguration().MinimumLevel.Verbose().WriteTo.File("log.txt").CreateLogger();

            Log.Information("Starting RimworldModUpdater v{0}", Settings.Version);

            Log.Information($"Initializing Cef {Cef.CefSharpVersion} ({Cef.CefVersion}) chromium {Cef.ChromiumVersion}");

            Cef.EnableHighDPISupport();

            var settings = new CefSettings();
            settings.DisableGpuAcceleration();
            Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);

            CefSharpSettings.LegacyJavascriptBindingEnabled = true;

            Application.ApplicationExit += delegate(object sender, EventArgs args)
            {
                Log.Information("Closing steamcmd instances before quit.");
                foreach (var process in Process.GetProcessesByName("steamcmd"))
                {
                    process.Kill();
                }
            };

            // Cleanup steamapps folder.
            if (Directory.Exists("steamcmd/steamapps"))
            {
                try
                {
                    Directory.Delete("steamcmd/steamapps", true);
                    Log.Information("Deleted old steamapps folder.");
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Failed to cleanup steamapps folder (You can ignore this error).");
                }
            }

            Log.Information("Creating UpdaterForm");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new UpdaterForm());
        }
    }
}
