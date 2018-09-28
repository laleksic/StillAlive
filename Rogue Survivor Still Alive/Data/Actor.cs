﻿// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Data.Actor
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using System;
using System.Collections.Generic;

namespace djack.RogueSurvivor.Data
{
  [Serializable]
  internal class Actor
  {
    private Actor.Flags m_Flags;
    private int m_ModelID;
    private int m_FactionID;
    private int m_GangID;
    private string m_Name;
    private ActorController m_Controller;
    private ActorSheet m_Sheet;
    private int m_SpawnTime;
    private Inventory m_Inventory;
    private Doll m_Doll;
    private int m_HitPoints;
    private int m_previousHitPoints;
    private int m_StaminaPoints;
    private int m_previousStamina;
    private int m_FoodPoints;
    private int m_previousFoodPoints;
    private int m_SleepPoints;
    private int m_previousSleepPoints;
    private int m_Sanity;
    private int m_previousSanity;
    private Location m_Location;
    private int m_ActionPoints;
    private int m_LastActionTurn;
    private Activity m_Activity;
    private Actor m_TargetActor;
    private int m_AudioRangeMod;
    private Attack m_CurrentMeleeAttack;
    private Attack m_CurrentRangedAttack;
    private Defence m_CurrentDefence;
    private Actor m_Leader;
    private List<Actor> m_Followers;
    private int m_TrustInLeader;
    private List<TrustRecord> m_TrustList;
    private int m_KillsCount;
    private List<Actor> m_AggressorOf;
    private List<Actor> m_SelfDefenceFrom;
    private int m_MurdersCounter;
    private int m_Infection;
    private Corpse m_DraggedCorpse;
    private List<Item> m_BoringItems;

    public ActorModel Model
    {
      get
      {
        return Models.Actors[this.m_ModelID];
      }
      set
      {
        this.m_ModelID = value.ID;
        this.OnModelSet();
      }
    }

    public bool IsUnique
    {
      get
      {
        return this.GetFlag(Actor.Flags.IS_UNIQUE);
      }
      set
      {
        this.SetFlag(Actor.Flags.IS_UNIQUE, value);
      }
    }

    public Faction Faction
    {
      get
      {
        return Models.Factions[this.m_FactionID];
      }
      set
      {
        this.m_FactionID = value.ID;
      }
    }

    public string Name
    {
      get
      {
        if (!this.IsPlayer)
          return this.m_Name;
        return "(YOU) " + this.m_Name;
      }
      set
      {
        this.m_Name = value;
        if (value == null)
          return;
        this.m_Name.Replace("(YOU) ", "");
      }
    }

    public string UnmodifiedName
    {
      get
      {
        return this.m_Name;
      }
    }

    public bool IsProperName
    {
      get
      {
        return this.GetFlag(Actor.Flags.IS_PROPER_NAME);
      }
      set
      {
        this.SetFlag(Actor.Flags.IS_PROPER_NAME, value);
      }
    }

    public bool IsPluralName
    {
      get
      {
        return this.GetFlag(Actor.Flags.IS_PLURAL_NAME);
      }
      set
      {
        this.SetFlag(Actor.Flags.IS_PLURAL_NAME, value);
      }
    }

    public string TheName
    {
      get
      {
        if (!this.IsProperName && !this.IsPluralName)
          return "the " + this.m_Name;
        return this.Name;
      }
    }

    public ActorController Controller
    {
      get
      {
        return this.m_Controller;
      }
      set
      {
        if (this.m_Controller != null)
          this.m_Controller.LeaveControl();
        this.m_Controller = value;
        if (this.m_Controller == null)
          return;
        this.m_Controller.TakeControl(this);
      }
    }

    public bool IsPlayer
    {
      get
      {
        if (this.m_Controller != null)
          return this.m_Controller is PlayerController;
        return false;
      }
    }

    public int SpawnTime
    {
      get
      {
        return this.m_SpawnTime;
      }
    }

    public int GangID
    {
      get
      {
        return this.m_GangID;
      }
      set
      {
        this.m_GangID = value;
      }
    }

    public bool IsInAGang
    {
      get
      {
        return (uint) this.m_GangID > 0U;
      }
    }

    public Doll Doll
    {
      get
      {
        return this.m_Doll;
      }
    }

    public bool IsDead
    {
      get
      {
        return this.GetFlag(Actor.Flags.IS_DEAD);
      }
      set
      {
        this.SetFlag(Actor.Flags.IS_DEAD, value);
      }
    }

    public bool IsSleeping
    {
      get
      {
        return this.GetFlag(Actor.Flags.IS_SLEEPING);
      }
      set
      {
        this.SetFlag(Actor.Flags.IS_SLEEPING, value);
      }
    }

    public bool IsRunning
    {
      get
      {
        return this.GetFlag(Actor.Flags.IS_RUNNING);
      }
      set
      {
        this.SetFlag(Actor.Flags.IS_RUNNING, value);
      }
    }

    public Inventory Inventory
    {
      get
      {
        return this.m_Inventory;
      }
      set
      {
        this.m_Inventory = value;
      }
    }

    public int HitPoints
    {
      get
      {
        return this.m_HitPoints;
      }
      set
      {
        this.m_HitPoints = value;
      }
    }

    public int PreviousHitPoints
    {
      get
      {
        return this.m_previousHitPoints;
      }
      set
      {
        this.m_previousHitPoints = value;
      }
    }

    public int StaminaPoints
    {
      get
      {
        return this.m_StaminaPoints;
      }
      set
      {
        this.m_StaminaPoints = value;
      }
    }

    public int PreviousStaminaPoints
    {
      get
      {
        return this.m_previousStamina;
      }
      set
      {
        this.m_previousStamina = value;
      }
    }

    public int FoodPoints
    {
      get
      {
        return this.m_FoodPoints;
      }
      set
      {
        this.m_FoodPoints = value;
      }
    }

    public int PreviousFoodPoints
    {
      get
      {
        return this.m_previousFoodPoints;
      }
      set
      {
        this.m_previousFoodPoints = value;
      }
    }

    public int SleepPoints
    {
      get
      {
        return this.m_SleepPoints;
      }
      set
      {
        this.m_SleepPoints = value;
      }
    }

    public int PreviousSleepPoints
    {
      get
      {
        return this.m_previousSleepPoints;
      }
      set
      {
        this.m_previousSleepPoints = value;
      }
    }

    public int Sanity
    {
      get
      {
        return this.m_Sanity;
      }
      set
      {
        this.m_Sanity = value;
      }
    }

    public int PreviousSanity
    {
      get
      {
        return this.m_previousSanity;
      }
      set
      {
        this.m_previousSanity = value;
      }
    }

    public ActorSheet Sheet
    {
      get
      {
        return this.m_Sheet;
      }
    }

    public int ActionPoints
    {
      get
      {
        return this.m_ActionPoints;
      }
      set
      {
        this.m_ActionPoints = value;
      }
    }

    public int LastActionTurn
    {
      get
      {
        return this.m_LastActionTurn;
      }
      set
      {
        this.m_LastActionTurn = value;
      }
    }

    public Location Location
    {
      get
      {
        return this.m_Location;
      }
      set
      {
        this.m_Location = value;
      }
    }

    public Activity Activity
    {
      get
      {
        return this.m_Activity;
      }
      set
      {
        this.m_Activity = value;
      }
    }

    public Actor TargetActor
    {
      get
      {
        return this.m_TargetActor;
      }
      set
      {
        this.m_TargetActor = value;
      }
    }

    public int AudioRange
    {
      get
      {
        return this.m_Sheet.BaseAudioRange + this.m_AudioRangeMod;
      }
    }

    public int AudioRangeMod
    {
      get
      {
        return this.m_AudioRangeMod;
      }
      set
      {
        this.m_AudioRangeMod = value;
      }
    }

    public Attack CurrentMeleeAttack
    {
      get
      {
        return this.m_CurrentMeleeAttack;
      }
      set
      {
        this.m_CurrentMeleeAttack = value;
      }
    }

    public Attack CurrentRangedAttack
    {
      get
      {
        return this.m_CurrentRangedAttack;
      }
      set
      {
        this.m_CurrentRangedAttack = value;
      }
    }

    public Defence CurrentDefence
    {
      get
      {
        return this.m_CurrentDefence;
      }
      set
      {
        this.m_CurrentDefence = value;
      }
    }

    public Actor Leader
    {
      get
      {
        return this.m_Leader;
      }
    }

    public bool HasLeader
    {
      get
      {
        if (this.m_Leader != null)
          return !this.m_Leader.IsDead;
        return false;
      }
    }

    public int TrustInLeader
    {
      get
      {
        return this.m_TrustInLeader;
      }
      set
      {
        this.m_TrustInLeader = value;
      }
    }

    public IEnumerable<Actor> Followers
    {
      get
      {
        return (IEnumerable<Actor>) this.m_Followers;
      }
    }

    public int CountFollowers
    {
      get
      {
        if (this.m_Followers == null)
          return 0;
        return this.m_Followers.Count;
      }
    }

    public int KillsCount
    {
      get
      {
        return this.m_KillsCount;
      }
      set
      {
        this.m_KillsCount = value;
      }
    }

    public IEnumerable<Actor> AggressorOf
    {
      get
      {
        return (IEnumerable<Actor>) this.m_AggressorOf;
      }
    }

    public int CountAggressorOf
    {
      get
      {
        if (this.m_AggressorOf == null)
          return 0;
        return this.m_AggressorOf.Count;
      }
    }

    public IEnumerable<Actor> SelfDefenceFrom
    {
      get
      {
        return (IEnumerable<Actor>) this.m_SelfDefenceFrom;
      }
    }

    public int CountSelfDefenceFrom
    {
      get
      {
        if (this.m_SelfDefenceFrom == null)
          return 0;
        return this.m_SelfDefenceFrom.Count;
      }
    }

    public int MurdersCounter
    {
      get
      {
        return this.m_MurdersCounter;
      }
      set
      {
        this.m_MurdersCounter = value;
      }
    }

    public int Infection
    {
      get
      {
        return this.m_Infection;
      }
      set
      {
        this.m_Infection = value;
      }
    }

    public Corpse DraggedCorpse
    {
      get
      {
        return this.m_DraggedCorpse;
      }
      set
      {
        this.m_DraggedCorpse = value;
      }
    }

    public Actor(ActorModel model, Faction faction, string name, bool isProperName, bool isPluralName, int spawnTime)
    {
      if (model == null)
        throw new ArgumentNullException(nameof (model));
      if (faction == null)
        throw new ArgumentNullException(nameof (faction));
      if (name == null)
        throw new ArgumentNullException(nameof (name));
      this.m_ModelID = model.ID;
      this.m_FactionID = faction.ID;
      this.m_GangID = 0;
      this.m_Name = name;
      this.IsProperName = isProperName;
      this.IsPluralName = isPluralName;
      this.m_Location = new Location();
      this.m_SpawnTime = spawnTime;
      this.IsUnique = false;
      this.IsDead = false;
      this.OnModelSet();
    }

    public Actor(ActorModel model, Faction faction, int spawnTime)
      : this(model, faction, model.Name, false, false, spawnTime)
    {
    }

    private void OnModelSet()
    {
      ActorModel model = this.Model;
      this.m_Doll = new Doll(model.DollBody);
      this.m_Sheet = new ActorSheet(model.StartingSheet);
      this.m_ActionPoints = this.m_Doll.Body.Speed;
      this.m_HitPoints = this.m_previousHitPoints = this.m_Sheet.BaseHitPoints;
      this.m_StaminaPoints = this.m_previousStamina = this.m_Sheet.BaseStaminaPoints;
      this.m_FoodPoints = this.m_previousFoodPoints = this.m_Sheet.BaseFoodPoints;
      this.m_SleepPoints = this.m_previousSleepPoints = this.m_Sheet.BaseSleepPoints;
      this.m_Sanity = this.m_previousSanity = this.m_Sheet.BaseSanity;
      if (model.Abilities.HasInventory)
        this.m_Inventory = new Inventory(model.StartingSheet.BaseInventoryCapacity);
      this.m_CurrentMeleeAttack = model.StartingSheet.UnarmedAttack;
      this.m_CurrentDefence = model.StartingSheet.BaseDefence;
      this.m_CurrentRangedAttack = Attack.BLANK;
    }

    public void AddFollower(Actor other)
    {
      if (other == null)
        throw new ArgumentNullException(nameof (other));
      if (this.m_Followers != null && this.m_Followers.Contains(other))
        throw new ArgumentException("other is already a follower");
      if (this.m_Followers == null)
        this.m_Followers = new List<Actor>(1);
      this.m_Followers.Add(other);
      if (other.Leader != null)
        other.Leader.RemoveFollower(other);
      other.m_Leader = this;
    }

    public void RemoveFollower(Actor other)
    {
      if (other == null)
        throw new ArgumentNullException(nameof (other));
      if (this.m_Followers == null)
        throw new InvalidOperationException("no followers");
      this.m_Followers.Remove(other);
      if (this.m_Followers.Count == 0)
        this.m_Followers = (List<Actor>) null;
      other.m_Leader = (Actor) null;
      AIController controller = other.Controller as AIController;
      if (controller == null)
        return;
      controller.Directives.Reset();
      controller.SetOrder((ActorOrder) null);
    }

    public void RemoveAllFollowers()
    {
      while (this.m_Followers != null && this.m_Followers.Count > 0)
        this.RemoveFollower(this.m_Followers[0]);
    }

    public void SetTrustIn(Actor other, int trust)
    {
      if (this.m_TrustList == null)
      {
        this.m_TrustList = new List<TrustRecord>(1)
        {
          new TrustRecord() { Actor = other, Trust = trust }
        };
      }
      else
      {
        foreach (TrustRecord trust1 in this.m_TrustList)
        {
          if (trust1.Actor == other)
          {
            trust1.Trust = trust;
            return;
          }
        }
        this.m_TrustList.Add(new TrustRecord()
        {
          Actor = other,
          Trust = trust
        });
      }
    }

    public void AddTrustIn(Actor other, int amount)
    {
      this.SetTrustIn(other, this.GetTrustIn(other) + amount);
    }

    public int GetTrustIn(Actor other)
    {
      if (this.m_TrustList == null)
        return 0;
      foreach (TrustRecord trust in this.m_TrustList)
      {
        if (trust.Actor == other)
          return trust.Trust;
      }
      return 0;
    }

    public void MarkAsAgressorOf(Actor other)
    {
      if (other == null || other.IsDead)
        return;
      if (this.m_AggressorOf == null)
        this.m_AggressorOf = new List<Actor>(1);
      else if (this.m_AggressorOf.Contains(other))
        return;
      this.m_AggressorOf.Add(other);
    }

    public void MarkAsSelfDefenceFrom(Actor other)
    {
      if (other == null || other.IsDead)
        return;
      if (this.m_SelfDefenceFrom == null)
        this.m_SelfDefenceFrom = new List<Actor>(1);
      else if (this.m_SelfDefenceFrom.Contains(other))
        return;
      this.m_SelfDefenceFrom.Add(other);
    }

    public bool IsAggressorOf(Actor other)
    {
      if (this.m_AggressorOf == null)
        return false;
      return this.m_AggressorOf.Contains(other);
    }

    public bool IsSelfDefenceFrom(Actor other)
    {
      if (this.m_SelfDefenceFrom == null)
        return false;
      return this.m_SelfDefenceFrom.Contains(other);
    }

    public void RemoveAggressorOf(Actor other)
    {
      if (this.m_AggressorOf == null)
        return;
      this.m_AggressorOf.Remove(other);
      if (this.m_AggressorOf.Count != 0)
        return;
      this.m_AggressorOf = (List<Actor>) null;
    }

    public void RemoveSelfDefenceFrom(Actor other)
    {
      if (this.m_SelfDefenceFrom == null)
        return;
      this.m_SelfDefenceFrom.Remove(other);
      if (this.m_SelfDefenceFrom.Count != 0)
        return;
      this.m_SelfDefenceFrom = (List<Actor>) null;
    }

    public void RemoveAllAgressorSelfDefenceRelations()
    {
      while (this.m_AggressorOf != null)
      {
        Actor other = this.m_AggressorOf[0];
        this.RemoveAggressorOf(other);
        other.RemoveSelfDefenceFrom(this);
      }
      while (this.m_SelfDefenceFrom != null)
      {
        Actor other = this.m_SelfDefenceFrom[0];
        this.RemoveSelfDefenceFrom(other);
        other.RemoveAggressorOf(this);
      }
    }

    public bool AreDirectEnemies(Actor other)
    {
      return other != null && !other.IsDead && (this.m_AggressorOf != null && this.m_AggressorOf.Contains(other) || this.m_SelfDefenceFrom != null && this.m_SelfDefenceFrom.Contains(other) || (other.IsAggressorOf(this) || other.IsSelfDefenceFrom(this)));
    }

    public bool AreIndirectEnemies(Actor other)
    {
      if (other == null || other.IsDead)
        return false;
      if (this.HasLeader)
      {
        if (this.m_Leader.AreDirectEnemies(other) || other.HasLeader && this.m_Leader.AreDirectEnemies(other.Leader))
          return true;
        foreach (Actor follower in this.m_Leader.Followers)
        {
          if (follower != this && follower.AreDirectEnemies(other))
            return true;
        }
      }
      if (this.CountFollowers > 0)
      {
        foreach (Actor follower in this.m_Followers)
        {
          if (follower.AreDirectEnemies(other))
            return true;
        }
      }
      if (other.HasLeader)
      {
        if (other.Leader.AreDirectEnemies(this) || this.HasLeader && other.Leader.AreDirectEnemies(this.m_Leader))
          return true;
        foreach (Actor follower in other.Leader.Followers)
        {
          if (follower != other && follower.AreDirectEnemies(this))
            return true;
        }
      }
      return false;
    }

    public void AddBoringItem(Item it)
    {
      if (this.m_BoringItems == null)
        this.m_BoringItems = new List<Item>(1);
      if (this.m_BoringItems.Contains(it))
        return;
      this.m_BoringItems.Add(it);
    }

    public bool IsBoredOf(Item it)
    {
      if (this.m_BoringItems == null)
        return false;
      return this.m_BoringItems.Contains(it);
    }

    public Item GetEquippedItem(DollPart part)
    {
      if (this.m_Inventory == null || part == DollPart.NONE)
        return (Item) null;
      foreach (Item obj in this.m_Inventory.Items)
      {
        if (obj.EquippedPart == part)
          return obj;
      }
      return (Item) null;
    }

    public Item GetEquippedWeapon()
    {
      return this.GetEquippedItem(DollPart._FIRST);
    }

    private bool GetFlag(Actor.Flags f)
    {
      return (uint) (this.m_Flags & f) > 0U;
    }

    private void SetFlag(Actor.Flags f, bool value)
    {
      if (value)
        this.m_Flags |= f;
      else
        this.m_Flags &= ~f;
    }

    private void OneFlag(Actor.Flags f)
    {
      this.m_Flags |= f;
    }

    private void ZeroFlag(Actor.Flags f)
    {
      this.m_Flags &= ~f;
    }

    public void OptimizeBeforeSaving()
    {
      if (this.m_TargetActor != null && this.m_TargetActor.IsDead)
        this.m_TargetActor = (Actor) null;
      if (this.m_BoringItems == null)
        return;
      this.m_BoringItems.TrimExcess();
    }

    [System.Flags]
    private enum Flags
    {
      NONE = 0,
      IS_UNIQUE = 1,
      IS_PROPER_NAME = 2,
      IS_PLURAL_NAME = 4,
      IS_DEAD = 8,
      IS_RUNNING = 16, // 0x00000010
      IS_SLEEPING = 32, // 0x00000020
    }
  }
}
