// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Data.DollBody
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using System;

namespace djack.RogueSurvivor.Data
{
  [Serializable]
  internal class DollBody
  {
    [NonSerialized]
    public static readonly DollBody UNDEF = new DollBody(true, 0);
    private readonly bool m_IsMale;
    private readonly int m_Speed;

    public bool IsMale
    {
      get
      {
        return this.m_IsMale;
      }
    }

    public int Speed
    {
      get
      {
        return this.m_Speed;
      }
    }

    public DollBody(bool isMale, int speed)
    {
      this.m_IsMale = isMale;
      this.m_Speed = speed;
    }
  }
}
