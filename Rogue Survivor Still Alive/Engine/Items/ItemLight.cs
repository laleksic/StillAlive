// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.Items.ItemLight
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Data;
using System;

namespace djack.RogueSurvivor.Engine.Items
{
  [Serializable]
  internal class ItemLight : Item
  {
    private int m_Batteries;

    public int Batteries
    {
      get
      {
        return this.m_Batteries;
      }
      set
      {
        if (value < 0)
          value = 0;
        this.m_Batteries = Math.Min(value, (this.Model as ItemLightModel).MaxBatteries);
      }
    }

    public int FovBonus
    {
      get
      {
        return (this.Model as ItemLightModel).FovBonus;
      }
    }

    public bool IsFullyCharged
    {
      get
      {
        return this.m_Batteries >= (this.Model as ItemLightModel).MaxBatteries;
      }
    }

    public override string ImageID
    {
      get
      {
        if (this.IsEquipped && this.Batteries > 0)
          return base.ImageID;
        return (this.Model as ItemLightModel).OutOfBatteriesImageID;
      }
    }

    public ItemLight(ItemModel model)
      : base(model)
    {
      if (!(model is ItemLightModel))
        throw new ArgumentException("model is not a LightModel");
      this.Batteries = (model as ItemLightModel).MaxBatteries;
    }
  }
}
