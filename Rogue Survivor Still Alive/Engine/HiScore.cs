// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.HiScore
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using System;

namespace djack.RogueSurvivor.Engine
{
  [Serializable]
  internal class HiScore
  {
    public string Name { get; set; }

    public int TotalPoints { get; set; }

    public int DifficultyPercent { get; set; }

    public int SurvivalPoints { get; set; }

    public int KillPoints { get; set; }

    public int AchievementPoints { get; set; }

    public int TurnSurvived { get; set; }

    public TimeSpan PlayingTime { get; set; }

    public string SkillsDescription { get; set; }

    public string Death { get; set; }

    public static HiScore FromScoring(string name, Scoring sc, string skillsDescription)
    {
      if (sc == null)
        throw new ArgumentNullException("scoring");
      return new HiScore()
      {
        AchievementPoints = sc.AchievementPoints,
        Death = sc.DeathReason,
        DifficultyPercent = (int) (100.0 * (double) sc.DifficultyRating),
        KillPoints = sc.KillPoints,
        Name = name,
        PlayingTime = sc.RealLifePlayingTime,
        SkillsDescription = skillsDescription,
        SurvivalPoints = sc.SurvivalPoints,
        TotalPoints = sc.TotalPoints,
        TurnSurvived = sc.TurnsSurvived
      };
    }
  }
}
