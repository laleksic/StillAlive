// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Data.Skill
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using System;

namespace djack.RogueSurvivor.Data
{
  [Serializable]
  internal class Skill
  {
    private int m_ID;
    private int m_Level;

    public int ID
    {
      get
      {
        return this.m_ID;
      }
    }

    public int Level
    {
      get
      {
        return this.m_Level;
      }
      set
      {
        this.m_Level = value;
      }
    }

    public Skill(int id)
    {
      this.m_ID = id;
    }
  }
}
