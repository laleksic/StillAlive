// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Data.StateMapObject
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using System;

namespace djack.RogueSurvivor.Data
{
  [Serializable]
  internal class StateMapObject : MapObject
  {
    private int m_State;

    public int State
    {
      get
      {
        return this.m_State;
      }
    }

    public StateMapObject(string name, string hiddenImageID)
      : base(name, hiddenImageID)
    {
    }

    public StateMapObject(string name, string hiddenImageID, MapObject.Break breakable, MapObject.Fire burnable, int hitPoints)
      : base(name, hiddenImageID, breakable, burnable, hitPoints)
    {
    }

    public virtual void SetState(int newState)
    {
      this.m_State = newState;
    }
  }
}
