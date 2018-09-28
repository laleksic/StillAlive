// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Data.TimedTask
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using System;

namespace djack.RogueSurvivor.Data
{
  [Serializable]
  internal abstract class TimedTask
  {
    public int TurnsLeft { get; set; }

    public bool IsCompleted
    {
      get
      {
        return this.TurnsLeft <= 0;
      }
    }

    protected TimedTask(int turnsLeft)
    {
      this.TurnsLeft = turnsLeft;
    }

    public void Tick(Map m)
    {
      if (--this.TurnsLeft > 0)
        return;
      this.Trigger(m);
    }

    public abstract void Trigger(Map m);
  }
}
