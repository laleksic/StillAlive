// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.Items.ItemTrapModel
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Data;

namespace djack.RogueSurvivor.Engine.Items
{
  internal class ItemTrapModel : ItemModel
  {
    private ItemTrapModel.Flags m_Flags;
    private int m_TriggerChance;
    private int m_Damage;
    private int m_BreakChance;
    private int m_BreakChanceWhenEscape;
    private int m_BlockChance;
    private string m_NoiseName;

    public int TriggerChance
    {
      get
      {
        return this.m_TriggerChance;
      }
    }

    public int Damage
    {
      get
      {
        return this.m_Damage;
      }
    }

    public bool UseToActivate
    {
      get
      {
        return (uint) (this.m_Flags & ItemTrapModel.Flags.USE_TO_ACTIVATE) > 0U;
      }
    }

    public bool IsNoisy
    {
      get
      {
        return (uint) (this.m_Flags & ItemTrapModel.Flags.IS_NOISY) > 0U;
      }
    }

    public bool IsOneTimeUse
    {
      get
      {
        return (uint) (this.m_Flags & ItemTrapModel.Flags.IS_ONE_TIME_USE) > 0U;
      }
    }

    public bool IsFlammable
    {
      get
      {
        return (uint) (this.m_Flags & ItemTrapModel.Flags.IS_FLAMMABLE) > 0U;
      }
    }

    public bool ActivatesWhenDropped
    {
      get
      {
        return (uint) (this.m_Flags & ItemTrapModel.Flags.DROP_ACTIVATE) > 0U;
      }
    }

    public int BreakChance
    {
      get
      {
        return this.m_BreakChance;
      }
    }

    public int BlockChance
    {
      get
      {
        return this.m_BlockChance;
      }
    }

    public int BreakChanceWhenEscape
    {
      get
      {
        return this.m_BreakChanceWhenEscape;
      }
    }

    public string NoiseName
    {
      get
      {
        return this.m_NoiseName;
      }
    }

    public ItemTrapModel(string aName, string theNames, string imageID, int stackLimit, int triggerChance, int damage, bool dropActivate, bool useToActivate, bool IsOneTimeUse, int breakChance, int blockChance, int breakChanceWhenEscape, bool IsNoisy, string noiseName, bool IsFlammable)
      : base(aName, theNames, imageID)
    {
      this.DontAutoEquip = true;
      if (stackLimit > 1)
      {
        this.IsStackable = true;
        this.StackingLimit = stackLimit;
      }
      this.m_TriggerChance = triggerChance;
      this.m_Damage = damage;
      this.m_BreakChance = breakChance;
      this.m_BlockChance = blockChance;
      this.m_BreakChanceWhenEscape = breakChanceWhenEscape;
      this.m_Flags = ItemTrapModel.Flags.NONE;
      if (dropActivate)
        this.m_Flags |= ItemTrapModel.Flags.DROP_ACTIVATE;
      if (useToActivate)
        this.m_Flags |= ItemTrapModel.Flags.USE_TO_ACTIVATE;
      if (IsNoisy)
      {
        this.m_Flags |= ItemTrapModel.Flags.IS_NOISY;
        this.m_NoiseName = noiseName;
      }
      if (IsOneTimeUse)
        this.m_Flags |= ItemTrapModel.Flags.IS_ONE_TIME_USE;
      if (!IsFlammable)
        return;
      this.m_Flags |= ItemTrapModel.Flags.IS_FLAMMABLE;
    }

    [System.Flags]
    private enum Flags
    {
      NONE = 0,
      USE_TO_ACTIVATE = 1,
      IS_NOISY = 2,
      IS_ONE_TIME_USE = 4,
      IS_FLAMMABLE = 8,
      DROP_ACTIVATE = 16, // 0x00000010
    }
  }
}
