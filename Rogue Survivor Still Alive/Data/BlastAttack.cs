// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Data.BlastAttack
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using System;

namespace djack.RogueSurvivor.Data
{
  [Serializable]
  internal struct BlastAttack
  {
    public int Radius { get; private set; }

    public int[] Damage { get; private set; }

    public bool CanDamageObjects { get; private set; }

    public bool CanDestroyWalls { get; private set; }

    public BlastAttack(int radius, int[] damage, bool canDamageObjects, bool canDestroyWalls)
    {
      this = new BlastAttack();
      if (damage.Length != radius + 1)
        throw new ArgumentException("damage.Length != radius + 1");
      this.Radius = radius;
      this.Damage = damage;
      this.CanDamageObjects = canDamageObjects;
      this.CanDestroyWalls = canDestroyWalls;
    }
  }
}
