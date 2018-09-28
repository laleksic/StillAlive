﻿// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.Items.ItemTrap
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Data;
using System;

namespace djack.RogueSurvivor.Engine.Items
{
  [Serializable]
  internal class ItemTrap : Item
  {
    private bool m_IsActivated;
    private bool m_IsTriggered;

    public bool IsActivated
    {
      get
      {
        return this.m_IsActivated;
      }
      set
      {
        this.m_IsActivated = value;
      }
    }

    public bool IsTriggered
    {
      get
      {
        return this.m_IsTriggered;
      }
      set
      {
        this.m_IsTriggered = value;
      }
    }

    public ItemTrapModel TrapModel
    {
      get
      {
        return this.Model as ItemTrapModel;
      }
    }

    public ItemTrap(ItemModel model)
      : base(model)
    {
      if (!(model is ItemTrapModel))
        throw new ArgumentException("model is not a TrapModel");
    }

    public ItemTrap Clone()
    {
      return new ItemTrap((ItemModel) this.TrapModel);
    }
  }
}
