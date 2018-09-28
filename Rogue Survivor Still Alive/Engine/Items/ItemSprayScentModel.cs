// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.Items.ItemSprayScentModel
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Data;

namespace djack.RogueSurvivor.Engine.Items
{
  internal class ItemSprayScentModel : ItemModel
  {
    private int m_MaxSprayQuantity;
    private Odor m_Odor;
    private int m_Strength;

    public int MaxSprayQuantity
    {
      get
      {
        return this.m_MaxSprayQuantity;
      }
    }

    public int Strength
    {
      get
      {
        return this.m_Strength;
      }
    }

    public Odor Odor
    {
      get
      {
        return this.m_Odor;
      }
    }

    public ItemSprayScentModel(string aName, string theNames, string imageID, int sprayQuantity, Odor odor, int strength)
      : base(aName, theNames, imageID)
    {
      this.m_MaxSprayQuantity = sprayQuantity;
      this.m_Odor = odor;
      this.m_Strength = strength;
    }
  }
}
