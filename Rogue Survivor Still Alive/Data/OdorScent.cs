// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Data.OdorScent
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using System;
using System.Drawing;

namespace djack.RogueSurvivor.Data
{
  [Serializable]
  internal class OdorScent
  {
    public const int MIN_STRENGTH = 1;
    public const int MAX_STRENGTH = 270;

    public Odor Odor { get; private set; }

    public int Strength { get; private set; }

    public Point Position { get; private set; }

    public OdorScent(Odor odor, int strength, Point position)
    {
      this.Odor = odor;
      this.Strength = Math.Min(270, strength);
      this.Position = position;
    }

    public void Change(int amount)
    {
      int num = this.Strength + amount;
      if (num < 1)
        num = 0;
      else if (num > 270)
        num = 270;
      this.Strength = num;
    }

    public void Set(int value)
    {
      int num = value;
      if (num < 1)
        num = 0;
      else if (num > 270)
        num = 270;
      this.Strength = num;
    }
  }
}
