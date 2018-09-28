// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.AI.Percept
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Data;
using System;

namespace djack.RogueSurvivor.Engine.AI
{
  [Serializable]
  internal class Percept
  {
    private int m_Turn;
    private Location m_Location;
    private object m_Percepted;

    public int Turn
    {
      get
      {
        return this.m_Turn;
      }
      set
      {
        this.m_Turn = value;
      }
    }

    public object Percepted
    {
      get
      {
        return this.m_Percepted;
      }
    }

    public Location Location
    {
      get
      {
        return this.m_Location;
      }
      set
      {
        this.m_Location = value;
      }
    }

    public Percept(object percepted, int turn, Location location)
    {
      if (percepted == null)
        throw new ArgumentNullException(nameof (percepted));
      this.m_Percepted = percepted;
      this.m_Turn = turn;
      this.m_Location = location;
    }

    public int GetAge(int currentGameTurn)
    {
      return Math.Max(0, currentGameTurn - this.m_Turn);
    }
  }
}
