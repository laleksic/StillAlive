// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.Items.ItemEntertainmentModel
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Data;

namespace djack.RogueSurvivor.Engine.Items
{
  internal class ItemEntertainmentModel : ItemModel
  {
    private int m_Value;
    private int m_BoreChance;

    public int Value
    {
      get
      {
        return this.m_Value;
      }
      set
      {
        this.m_Value = value;
      }
    }

    public int BoreChance
    {
      get
      {
        return this.m_BoreChance;
      }
      set
      {
        this.m_BoreChance = value;
      }
    }

    public ItemEntertainmentModel(string aName, string theNames, string imageID, int value, int boreChance)
      : base(aName, theNames, imageID)
    {
      this.m_Value = value;
      this.m_BoreChance = boreChance;
    }
  }
}
