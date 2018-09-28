﻿// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.Items.ItemMeleeWeapon
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Data;
using System;

namespace djack.RogueSurvivor.Engine.Items
{
  [Serializable]
  internal class ItemMeleeWeapon : ItemWeapon
  {
    public bool IsFragile
    {
      get
      {
        return (this.Model as ItemMeleeWeaponModel).IsFragile;
      }
    }

    public ItemMeleeWeapon(ItemModel model)
      : base(model)
    {
      if (!(model is ItemMeleeWeaponModel))
        throw new ArgumentException("model is not a MeleeWeaponModel");
    }
  }
}
