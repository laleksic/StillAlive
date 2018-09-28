// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.Achievement
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using System;

namespace djack.RogueSurvivor.Engine
{
  [Serializable]
  internal class Achievement
  {
    public Achievement.IDs ID { get; private set; }

    public string Name { get; private set; }

    public string TeaseName { get; private set; }

    public string[] Text { get; private set; }

    public string MusicID { get; private set; }

    public int ScoreValue { get; private set; }

    public bool IsDone { get; set; }

    public Achievement(Achievement.IDs id, string name, string teaseName, string[] text, string musicID, int scoreValue)
    {
      this.ID = id;
      this.Name = name;
      this.TeaseName = teaseName;
      this.Text = text;
      this.MusicID = musicID;
      this.ScoreValue = scoreValue;
      this.IsDone = false;
    }

    [Serializable]
    public enum IDs
    {
      REACHED_DAY_07 = 0,
      _FIRST = 0,
      REACHED_DAY_14 = 1,
      REACHED_DAY_21 = 2,
      REACHED_DAY_28 = 3,
      CHAR_BROKE_INTO_OFFICE = 4,
      CHAR_FOUND_UNDERGROUND_FACILITY = 5,
      CHAR_POWER_UNDERGROUND_FACILITY = 6,
      KILLED_THE_SEWERS_THING = 7,
      _COUNT = 8,
    }
  }
}
