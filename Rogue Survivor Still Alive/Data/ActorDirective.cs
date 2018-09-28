// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Data.ActorDirective
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using System;

namespace djack.RogueSurvivor.Data
{
  [Serializable]
  internal class ActorDirective
  {
    public bool CanTakeItems { get; set; }

    public bool CanFireWeapons { get; set; }

    public bool CanThrowGrenades { get; set; }

    public bool CanSleep { get; set; }

    public bool CanTrade { get; set; }

    public ActorCourage Courage { get; set; }

    public ActorDirective()
    {
      this.Reset();
    }

    public void Reset()
    {
      this.CanTakeItems = true;
      this.CanFireWeapons = true;
      this.CanThrowGrenades = true;
      this.CanSleep = true;
      this.CanTrade = true;
      this.Courage = ActorCourage.CAUTIOUS;
    }

    public static string CourageString(ActorCourage c)
    {
      switch (c)
      {
        case ActorCourage.COWARD:
          return "Coward";
        case ActorCourage.CAUTIOUS:
          return "Cautious";
        case ActorCourage.COURAGEOUS:
          return "Courageous";
        default:
          throw new ArgumentOutOfRangeException("unhandled courage");
      }
    }
  }
}
