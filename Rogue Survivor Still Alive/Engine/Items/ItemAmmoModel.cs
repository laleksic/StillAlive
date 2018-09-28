// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.Items.ItemAmmoModel
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Data;

namespace djack.RogueSurvivor.Engine.Items
{
  internal class ItemAmmoModel : ItemModel
  {
    private AmmoType m_AmmoType;

    public AmmoType AmmoType
    {
      get
      {
        return this.m_AmmoType;
      }
    }

    public int MaxQuantity
    {
      get
      {
        return this.StackingLimit;
      }
    }

    public ItemAmmoModel(string aName, string theNames, string imageID, AmmoType ammoType, int maxQuantity)
      : base(aName, theNames, imageID)
    {
      this.m_AmmoType = ammoType;
      this.IsStackable = true;
      this.StackingLimit = maxQuantity;
    }
  }
}
