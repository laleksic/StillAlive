// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Data.Item
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using System;

namespace djack.RogueSurvivor.Data
{
  [Serializable]
  internal class Item
  {
    private int m_ModelID;
    private int m_Quantity;
    private DollPart m_EquipedPart;

    public ItemModel Model
    {
      get
      {
        return Models.Items[this.m_ModelID];
      }
    }

    public virtual string ImageID
    {
      get
      {
        return this.Model.ImageID;
      }
    }

    public string TheName
    {
      get
      {
        ItemModel model = this.Model;
        if (model.IsProper)
          return model.SingleName;
        if (this.m_Quantity > 1 || model.IsPlural)
          return "some " + model.PluralName;
        return "the " + model.SingleName;
      }
    }

    public string AName
    {
      get
      {
        ItemModel model = this.Model;
        if (model.IsProper)
          return model.SingleName;
        if (this.m_Quantity > 1 || model.IsPlural)
          return "some " + model.PluralName;
        if (model.IsAn)
          return "an " + model.SingleName;
        return "a " + model.SingleName;
      }
    }

    public int Quantity
    {
      get
      {
        return this.m_Quantity;
      }
      set
      {
        this.m_Quantity = value;
        if (this.m_Quantity >= 0)
          return;
        this.m_Quantity = 0;
      }
    }

    public bool CanStackMore
    {
      get
      {
        ItemModel model = this.Model;
        if (model.IsStackable)
          return this.m_Quantity < model.StackingLimit;
        return false;
      }
    }

    public DollPart EquippedPart
    {
      get
      {
        return this.m_EquipedPart;
      }
      set
      {
        this.m_EquipedPart = value;
      }
    }

    public bool IsEquipped
    {
      get
      {
        return (uint) this.m_EquipedPart > 0U;
      }
    }

    public bool IsUnique { get; set; }

    public bool IsForbiddenToAI { get; set; }

    public Item(ItemModel model)
    {
      this.m_ModelID = model.ID;
      this.m_Quantity = 1;
      this.m_EquipedPart = DollPart.NONE;
    }
  }
}
