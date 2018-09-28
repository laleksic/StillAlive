// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.Items.ItemPrimedExplosive
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Data;
using System;

namespace djack.RogueSurvivor.Engine.Items
{
  [Serializable]
  internal class ItemPrimedExplosive : ItemExplosive
  {
    public int FuseTimeLeft { get; set; }

    public ItemPrimedExplosive(ItemModel model)
      : base(model, model)
    {
      if (!(model is ItemExplosiveModel))
        throw new ArgumentException("model is not ItemExplosiveModel");
      this.FuseTimeLeft = (model as ItemExplosiveModel).FuseDelay;
    }
  }
}
