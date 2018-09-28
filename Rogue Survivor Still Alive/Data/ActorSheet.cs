// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Data.ActorSheet
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using System;

namespace djack.RogueSurvivor.Data
{
  [Serializable]
  internal class ActorSheet
  {
    [NonSerialized]
    public static readonly ActorSheet BLANK = new ActorSheet(0, 0, 0, 0, 0, Attack.BLANK, Defence.BLANK, 0, 0, 0, 0);
    private SkillTable m_SkillTable = new SkillTable();

    public int BaseHitPoints { get; private set; }

    public int BaseStaminaPoints { get; private set; }

    public int BaseFoodPoints { get; private set; }

    public int BaseSleepPoints { get; private set; }

    public int BaseSanity { get; private set; }

    public Attack UnarmedAttack { get; private set; }

    public Defence BaseDefence { get; private set; }

    public int BaseViewRange { get; private set; }

    public int BaseAudioRange { get; private set; }

    public float BaseSmellRating { get; private set; }

    public int BaseInventoryCapacity { get; private set; }

    public SkillTable SkillTable
    {
      get
      {
        return this.m_SkillTable;
      }
      set
      {
        this.m_SkillTable = value;
      }
    }

    public ActorSheet(int baseHitPoints, int baseStaminaPoints, int baseFoodPoints, int baseSleepPoints, int baseSanity, Attack unarmedAttack, Defence baseDefence, int baseViewRange, int baseAudioRange, int smellRating, int inventoryCapacity)
    {
      this.BaseHitPoints = baseHitPoints;
      this.BaseStaminaPoints = baseStaminaPoints;
      this.BaseFoodPoints = baseFoodPoints;
      this.BaseSleepPoints = baseSleepPoints;
      this.BaseSanity = baseSanity;
      this.UnarmedAttack = unarmedAttack;
      this.BaseDefence = baseDefence;
      this.BaseViewRange = baseViewRange;
      this.BaseAudioRange = baseAudioRange;
      this.BaseSmellRating = (float) smellRating / 100f;
      this.BaseInventoryCapacity = inventoryCapacity;
    }

    public ActorSheet(ActorSheet copyFrom)
    {
      if (copyFrom == null)
        throw new ArgumentNullException(nameof (copyFrom));
      this.BaseHitPoints = copyFrom.BaseHitPoints;
      this.BaseStaminaPoints = copyFrom.BaseStaminaPoints;
      this.BaseFoodPoints = copyFrom.BaseFoodPoints;
      this.BaseSleepPoints = copyFrom.BaseSleepPoints;
      this.BaseSanity = copyFrom.BaseSanity;
      this.UnarmedAttack = copyFrom.UnarmedAttack;
      this.BaseDefence = copyFrom.BaseDefence;
      this.BaseViewRange = copyFrom.BaseViewRange;
      this.BaseAudioRange = copyFrom.BaseAudioRange;
      this.BaseSmellRating = copyFrom.BaseSmellRating;
      this.BaseInventoryCapacity = copyFrom.BaseInventoryCapacity;
      if (copyFrom.SkillTable.Skills == null)
        return;
      this.m_SkillTable = new SkillTable(copyFrom.SkillTable.Skills);
    }
  }
}
