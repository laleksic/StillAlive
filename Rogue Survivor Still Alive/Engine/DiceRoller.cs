// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.DiceRoller
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using System;

namespace djack.RogueSurvivor.Engine
{
  [Serializable]
  internal class DiceRoller
  {
    private Random m_Rng;

    public DiceRoller(int seed)
    {
      this.m_Rng = new Random(seed);
    }

    public DiceRoller()
      : this((int) DateTime.UtcNow.Ticks)
    {
    }

    public int Roll(int min, int max)
    {
      if (max <= min)
        return min;
      int num;
      lock (this.m_Rng)
        num = this.m_Rng.Next(min, max);
      if (num >= max)
        num = max - 1;
      return num;
    }

    public float RollFloat()
    {
      lock (this.m_Rng)
        return (float) this.m_Rng.NextDouble();
    }

    public bool RollChance(int chance)
    {
      return this.Roll(0, 100) < chance;
    }
  }
}
