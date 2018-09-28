// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.GameHintsStatus
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace djack.RogueSurvivor.Engine
{
  [Serializable]
  internal class GameHintsStatus
  {
    private bool[] m_AdvisorHints = new bool[46];

    public void ResetAllHints()
    {
      for (int index = 0; index < 46; ++index)
        this.m_AdvisorHints[index] = false;
    }

    public bool IsAdvisorHintGiven(AdvisorHint hint)
    {
      return this.m_AdvisorHints[(int) hint];
    }

    public void SetAdvisorHintAsGiven(AdvisorHint hint)
    {
      this.m_AdvisorHints[(int) hint] = true;
    }

    public int CountAdvisorHintsGiven()
    {
      int num = 0;
      for (int index = 0; index < 46; ++index)
      {
        if (this.m_AdvisorHints[index])
          ++num;
      }
      return num;
    }

    public bool HasAdvisorGivenAllHints()
    {
      return this.CountAdvisorHintsGiven() >= 46;
    }

    public static void Save(GameHintsStatus hints, string filepath)
    {
      if (filepath == null)
        throw new ArgumentNullException(nameof (filepath));
      Logger.WriteLine(Logger.Stage.RUN_MAIN, "saving hints...");
      IFormatter formatter = GameHintsStatus.CreateFormatter();
      Stream stream = GameHintsStatus.CreateStream(filepath, true);
      Stream serializationStream = stream;
      GameHintsStatus gameHintsStatus = hints;
      formatter.Serialize(serializationStream, (object) gameHintsStatus);
      stream.Flush();
      stream.Close();
      Logger.WriteLine(Logger.Stage.RUN_MAIN, "saving hints... done!");
    }

    public static GameHintsStatus Load(string filepath)
    {
      if (filepath == null)
        throw new ArgumentNullException(nameof (filepath));
      Logger.WriteLine(Logger.Stage.RUN_MAIN, "loading hints...");
      GameHintsStatus gameHintsStatus;
      try
      {
        IFormatter formatter = GameHintsStatus.CreateFormatter();
        Stream stream = GameHintsStatus.CreateStream(filepath, false);
        Stream serializationStream = stream;
        gameHintsStatus = (GameHintsStatus) formatter.Deserialize(serializationStream);
        stream.Close();
      }
      catch (Exception ex)
      {
        Logger.WriteLine(Logger.Stage.RUN_MAIN, "failed to load hints (first run?).");
        Logger.WriteLine(Logger.Stage.RUN_MAIN, string.Format("load exception : {0}.", (object) ex.ToString()));
        Logger.WriteLine(Logger.Stage.RUN_MAIN, "resetting.");
        gameHintsStatus = new GameHintsStatus();
        gameHintsStatus.ResetAllHints();
      }
      Logger.WriteLine(Logger.Stage.RUN_MAIN, "loading options... done!");
      return gameHintsStatus;
    }

    private static IFormatter CreateFormatter()
    {
      return (IFormatter) new BinaryFormatter();
    }

    private static Stream CreateStream(string saveFileName, bool save)
    {
      return (Stream) new FileStream(saveFileName, save ? FileMode.Create : FileMode.Open, save ? FileAccess.Write : FileAccess.Read, FileShare.None);
    }
  }
}
