﻿// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.Items.ItemFood
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Data;
using System;

namespace djack.RogueSurvivor.Engine.Items
{
  [Serializable]
  internal class ItemFood : Item
  {
    public int Nutrition { get; private set; }

    public bool IsPerishable { get; private set; }

    public WorldTime BestBefore { get; private set; }

    public ItemFood(ItemModel model)
      : base(model)
    {
      if (!(model is ItemFoodModel))
        throw new ArgumentException("model is not a FoodModel");
      this.Nutrition = (model as ItemFoodModel).Nutrition;
      this.IsPerishable = false;
    }

    public ItemFood(ItemModel model, int bestBefore)
      : base(model)
    {
      if (!(model is ItemFoodModel))
        throw new ArgumentException("model is not a FoodModel");
      this.Nutrition = (model as ItemFoodModel).Nutrition;
      this.BestBefore = new WorldTime(bestBefore);
      this.IsPerishable = true;
    }
  }
}
