using System;
using System.Windows.Forms;
using System.Globalization;

namespace djack.RogueSurvivor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        [STAThread]
        static void Main()
        {
#if !DEBUG
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException); //@@MP
#endif
            Logger.CreateFile();
            Logger.WriteLine(Logger.Stage.INIT_MAIN, "starting program...");
            Logger.WriteLine(Logger.Stage.INIT_MAIN, String.Format("date : {0}.", DateTime.Now.ToString()));
            Logger.WriteLine(Logger.Stage.INIT_MAIN, String.Format("game version : {0}.", SetupConfig.GAME_VERSION));

            Logger.WriteLine(Logger.Stage.INIT_MAIN, "loading setup...");
            SetupConfig.Load();

            // Debug mode : don't catch exceptions, I want to debug them (optionally catch them)
            // Release mode : catch exceptions cleanly and report.
#if DEBUG
            var form = new RogueForm();
            form.Run();
#else
            try
            {
                var form = new RogueForm();
                form.Run();
            }
            catch (Exception e)
            {
                using (Bugreport report = new Bugreport(e))
                {
                    report.ShowDialog();
                }
            }
#endif
            Logger.WriteLine(Logger.Stage.CLEAN_MAIN, "exiting program...");
        }
    }
}
