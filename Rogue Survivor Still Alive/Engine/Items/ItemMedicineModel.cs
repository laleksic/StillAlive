// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.Items.ItemMedicineModel
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Data;

namespace djack.RogueSurvivor.Engine.Items
{
  internal class ItemMedicineModel : ItemModel
  {
    private int m_Healing;
    private int m_StaminaBoost;
    private int m_SleepBoost;
    private int m_InfectionCure;
    private int m_SanityCure;

    public int Healing
    {
      get
      {
        return this.m_Healing;
      }
    }

    public int StaminaBoost
    {
      get
      {
        return this.m_StaminaBoost;
      }
    }

    public int SleepBoost
    {
      get
      {
        return this.m_SleepBoost;
      }
    }

    public int InfectionCure
    {
      get
      {
        return this.m_InfectionCure;
      }
    }

    public int SanityCure
    {
      get
      {
        return this.m_SanityCure;
      }
    }

    public ItemMedicineModel(string aName, string theNames, string imageID, int healing, int staminaBoost, int sleepBoost, int infectionCure, int sanityCure)
      : base(aName, theNames, imageID)
    {
      this.m_Healing = healing;
      this.m_StaminaBoost = staminaBoost;
      this.m_SleepBoost = sleepBoost;
      this.m_InfectionCure = infectionCure;
      this.m_SanityCure = sanityCure;
    }
  }
}
