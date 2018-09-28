// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Program
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using System;
using System.Globalization;
using System.Windows.Forms;

namespace djack.RogueSurvivor
{
  internal static class Program
  {
    [STAThread]
    private static void Main()
    {
      Logger.CreateFile();
      Logger.WriteLine(Logger.Stage.INIT_MAIN, "starting program...");
      Logger.WriteLine(Logger.Stage.INIT_MAIN, string.Format("date : {0}.", (object) DateTime.Now.ToString()));
      Logger.WriteLine(Logger.Stage.INIT_MAIN, string.Format("game version : {0}.", (object) SetupConfig.GAME_VERSION));
      Application.CurrentCulture = CultureInfo.InvariantCulture;
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Logger.WriteLine(Logger.Stage.INIT_MAIN, "loading setup...");
      SetupConfig.Load();
      Logger.WriteLine(Logger.Stage.INIT_MAIN, "setup : " + SetupConfig.toString(SetupConfig.Video) + ", " + SetupConfig.toString(SetupConfig.Sound));
      using (RogueForm rogueForm = new RogueForm())
      {
        try
        {
          Application.Run((Form) rogueForm);
        }
        catch (Exception ex)
        {
          using (Bugreport bugreport = new Bugreport(ex))
          {
            int num = (int) bugreport.ShowDialog();
          }
          Application.Exit();
        }
      }
      Logger.WriteLine(Logger.Stage.CLEAN_MAIN, "exiting program...");
    }
  }
}
