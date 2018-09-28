// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Gameplay.Generators.StdTownGenerator
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Data;
using djack.RogueSurvivor.Engine;
using System;
using System.Drawing;

namespace djack.RogueSurvivor.Gameplay.Generators
{
  internal class StdTownGenerator : BaseTownGenerator
  {
    public StdTownGenerator(RogueGame game, BaseTownGenerator.Parameters parameters)
      : base(game, parameters)
    {
    }

    public override Map Generate(int seed)
    {
      Map map = base.Generate(seed);
      map.Name = "Std City";
      int maxTries = 10 * map.Width * map.Height;
      int num1 = 0;
      GameOptions options;
      while (true)
      {
        int num2 = num1;
        options = RogueGame.Options;
        int maxCivilians = options.MaxCivilians;
        if (num2 < maxCivilians)
        {
          if (this.m_DiceRoller.RollChance(this.Params.PolicemanChance))
          {
            Actor newPoliceman = this.CreateNewPoliceman(0);
            this.ActorPlace(this.m_DiceRoller, maxTries, map, newPoliceman, (Predicate<Point>) (pt => !map.GetTileAt(pt.X, pt.Y).IsInside));
          }
          else
          {
            Actor newCivilian = this.CreateNewCivilian(0, 0, 1);
            this.ActorPlace(this.m_DiceRoller, maxTries, map, newCivilian, (Predicate<Point>) (pt => map.GetTileAt(pt.X, pt.Y).IsInside));
          }
          ++num1;
        }
        else
          break;
      }
      int num3 = 0;
      while (true)
      {
        int num2 = num3;
        options = RogueGame.Options;
        int maxDogs = options.MaxDogs;
        if (num2 < maxDogs)
        {
          Actor newFeralDog = this.CreateNewFeralDog(0);
          this.ActorPlace(this.m_DiceRoller, maxTries, map, newFeralDog, (Predicate<Point>) (pt => !map.GetTileAt(pt.X, pt.Y).IsInside));
          ++num3;
        }
        else
          break;
      }
      options = RogueGame.Options;
      int maxUndeads = options.MaxUndeads;
      options = RogueGame.Options;
      int zeroUndeadsPercent = options.DayZeroUndeadsPercent;
      int num4 = maxUndeads * zeroUndeadsPercent / 100;
      for (int index = 0; index < num4; ++index)
      {
        Actor newUndead = this.CreateNewUndead(0);
        this.ActorPlace(this.m_DiceRoller, maxTries, map, newUndead, (Predicate<Point>) (pt => !map.GetTileAt(pt.X, pt.Y).IsInside));
      }
      return map;
    }

    public override Map GenerateSewersMap(int seed, District district)
    {
      Map sewersMap = base.GenerateSewersMap(seed, district);
      if (Rules.HasZombiesInSewers(this.m_Game.Session.GameMode))
      {
        int maxTries = 10 * sewersMap.Width * sewersMap.Height;
        double num1 = 0.5;
        GameOptions options = RogueGame.Options;
        int maxUndeads = options.MaxUndeads;
        options = RogueGame.Options;
        int zeroUndeadsPercent = options.DayZeroUndeadsPercent;
        double num2 = (double) (maxUndeads * zeroUndeadsPercent);
        int num3 = (int) (num1 * num2 / 100.0);
        for (int index = 0; index < num3; ++index)
        {
          Actor newSewersUndead = this.CreateNewSewersUndead(0);
          this.ActorPlace(this.m_DiceRoller, maxTries, sewersMap, newSewersUndead);
        }
      }
      return sewersMap;
    }

    public override Map GenerateSubwayMap(int seed, District district)
    {
      return base.GenerateSubwayMap(seed, district);
    }
  }
}
