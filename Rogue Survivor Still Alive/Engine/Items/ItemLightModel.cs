// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.Items.ItemLightModel
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Data;

namespace djack.RogueSurvivor.Engine.Items
{
  internal class ItemLightModel : ItemModel
  {
    private int m_MaxBatteries;
    private int m_FovBonus;
    private string m_OutOfBatteriesImageID;

    public int MaxBatteries
    {
      get
      {
        return this.m_MaxBatteries;
      }
    }

    public int FovBonus
    {
      get
      {
        return this.m_FovBonus;
      }
    }

    public string OutOfBatteriesImageID
    {
      get
      {
        return this.m_OutOfBatteriesImageID;
      }
    }

    public ItemLightModel(string aName, string theNames, string imageID, int fovBonus, int maxBatteries, string outOfBatteriesImageID)
      : base(aName, theNames, imageID)
    {
      this.m_FovBonus = fovBonus;
      this.m_MaxBatteries = maxBatteries;
      this.m_OutOfBatteriesImageID = outOfBatteriesImageID;
      this.DontAutoEquip = true;
    }
  }
}
