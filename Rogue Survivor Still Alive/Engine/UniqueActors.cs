// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.UniqueActors
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using System;

namespace djack.RogueSurvivor.Engine
{
  [Serializable]
  internal class UniqueActors
  {
    public UniqueActor BigBear { get; set; }

    public UniqueActor Duckman { get; set; }

    public UniqueActor FamuFataru { get; set; }

    public UniqueActor HansVonHanz { get; set; }

    public UniqueActor JasonMyers { get; set; }

    public UniqueActor PoliceStationPrisonner { get; set; }

    public UniqueActor Roguedjack { get; set; }

    public UniqueActor Santaman { get; set; }

    public UniqueActor TheSewersThing { get; set; }

    public UniqueActor[] ToArray()
    {
      return new UniqueActor[8]
      {
        this.BigBear,
        this.Duckman,
        this.FamuFataru,
        this.HansVonHanz,
        this.PoliceStationPrisonner,
        this.Roguedjack,
        this.Santaman,
        this.TheSewersThing
      };
    }
  }
}
