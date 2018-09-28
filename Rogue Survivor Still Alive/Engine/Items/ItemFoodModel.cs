// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.Items.ItemFoodModel
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Data;

namespace djack.RogueSurvivor.Engine.Items
{
  internal class ItemFoodModel : ItemModel
  {
    private int m_Nutrition;
    private bool m_IsPerishable;
    private int m_BestBeforeDays;

    public int Nutrition
    {
      get
      {
        return this.m_Nutrition;
      }
    }

    public bool IsPerishable
    {
      get
      {
        return this.m_IsPerishable;
      }
    }

    public int BestBeforeDays
    {
      get
      {
        return this.m_BestBeforeDays;
      }
    }

    public ItemFoodModel(string aName, string theNames, string imageID, int nutrition, int bestBeforeDays)
      : base(aName, theNames, imageID)
    {
      this.m_Nutrition = nutrition;
      if (bestBeforeDays < 0)
      {
        this.m_IsPerishable = false;
      }
      else
      {
        this.m_IsPerishable = true;
        this.m_BestBeforeDays = bestBeforeDays;
      }
    }
  }
}
