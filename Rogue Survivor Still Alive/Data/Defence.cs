// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Data.Defence
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using System;

namespace djack.RogueSurvivor.Data
{
  [Serializable]
  internal struct Defence
  {
    [NonSerialized]
    public static readonly Defence BLANK = new Defence(0, 0, 0);

    public int Value { get; private set; }

    public int Protection_Hit { get; private set; }

    public int Protection_Shot { get; private set; }

    public Defence(int value, int protection_hit, int protection_shot)
    {
      this = new Defence();
      this.Value = value;
      this.Protection_Hit = protection_hit;
      this.Protection_Shot = protection_shot;
    }

    public static Defence operator +(Defence lhs, Defence rhs)
    {
      return new Defence(lhs.Value + rhs.Value, lhs.Protection_Hit + rhs.Protection_Hit, lhs.Protection_Shot + rhs.Protection_Shot);
    }

    public static Defence operator -(Defence lhs, Defence rhs)
    {
      return new Defence(lhs.Value - rhs.Value, lhs.Protection_Hit - rhs.Protection_Hit, lhs.Protection_Shot - rhs.Protection_Shot);
    }
  }
}
