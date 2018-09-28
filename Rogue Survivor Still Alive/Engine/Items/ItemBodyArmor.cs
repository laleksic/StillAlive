// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.Items.ItemBodyArmor
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Data;
using djack.RogueSurvivor.Gameplay;
using System;

namespace djack.RogueSurvivor.Engine.Items
{
  [Serializable]
  internal class ItemBodyArmor : Item
  {
    public int Protection_Hit { get; private set; }

    public int Protection_Shot { get; private set; }

    public int Encumbrance { get; private set; }

    public int Weight { get; private set; }

    public ItemBodyArmor(ItemModel model)
      : base(model)
    {
      if (!(model is ItemBodyArmorModel))
        throw new ArgumentException("model is not a BodyArmorModel");
      ItemBodyArmorModel itemBodyArmorModel = model as ItemBodyArmorModel;
      this.Protection_Hit = itemBodyArmorModel.Protection_Hit;
      this.Protection_Shot = itemBodyArmorModel.Protection_Shot;
      this.Encumbrance = itemBodyArmorModel.Encumbrance;
      this.Weight = itemBodyArmorModel.Weight;
    }

    public bool IsHostileForCops()
    {
      return Array.IndexOf<GameItems.IDs>(GameFactions.BAD_POLICE_OUTFITS, (GameItems.IDs) this.Model.ID) >= 0;
    }

    public bool IsFriendlyForCops()
    {
      return Array.IndexOf<GameItems.IDs>(GameFactions.GOOD_POLICE_OUTFITS, (GameItems.IDs) this.Model.ID) >= 0;
    }

    public bool IsHostileForBiker(GameGangs.IDs gangID)
    {
      return Array.IndexOf<GameItems.IDs>(GameGangs.BAD_GANG_OUTFITS[(int) gangID], (GameItems.IDs) this.Model.ID) >= 0;
    }

    public bool IsFriendlyForBiker(GameGangs.IDs gangID)
    {
      return Array.IndexOf<GameItems.IDs>(GameGangs.GOOD_GANG_OUTFITS[(int) gangID], (GameItems.IDs) this.Model.ID) >= 0;
    }
  }
}
