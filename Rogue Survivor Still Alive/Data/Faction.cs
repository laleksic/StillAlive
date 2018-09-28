// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Data.Faction
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using System;
using System.Collections.Generic;

namespace djack.RogueSurvivor.Data
{
  [Serializable]
  internal class Faction
  {
    private List<Faction> m_Enemies = new List<Faction>(1);
    private int m_ID;
    private string m_Name;
    private string m_MemberName;

    public int ID
    {
      get
      {
        return this.m_ID;
      }
      set
      {
        this.m_ID = value;
      }
    }

    public string Name
    {
      get
      {
        return this.m_Name;
      }
    }

    public string MemberName
    {
      get
      {
        return this.m_MemberName;
      }
    }

    public bool LeadOnlyBySameFaction { get; set; }

    public IEnumerable<Faction> Enemies
    {
      get
      {
        return (IEnumerable<Faction>) this.m_Enemies;
      }
    }

    public Faction(string name, string memberName)
    {
      if (name == null)
        throw new ArgumentNullException(nameof (name));
      if (memberName == null)
        throw new ArgumentNullException(nameof (memberName));
      this.m_Name = name;
      this.m_MemberName = memberName;
    }

    public void AddEnemy(Faction other)
    {
      this.m_Enemies.Add(other);
    }

    public virtual bool IsEnemyOf(Faction other)
    {
      if (other != this)
        return this.m_Enemies.Contains(other);
      return false;
    }
  }
}
