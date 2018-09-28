﻿// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.Rules
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Data;
using djack.RogueSurvivor.Engine.Actions;
using djack.RogueSurvivor.Engine.Items;
using djack.RogueSurvivor.Engine.MapObjects;
using djack.RogueSurvivor.Gameplay;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace djack.RogueSurvivor.Engine
{
  internal class Rules
  {
    public static int INFECTION_LEVEL_1_WEAK = 10;
    public static int INFECTION_LEVEL_2_TIRED = 30;
    public static int INFECTION_LEVEL_3_VOMIT = 50;
    public static int INFECTION_LEVEL_4_BLEED = 75;
    public static int INFECTION_LEVEL_5_DEATH = 100;
    public static int INFECTION_LEVEL_1_WEAK_STA = 24;
    public static int INFECTION_LEVEL_2_TIRED_STA = 24;
    public static int INFECTION_LEVEL_2_TIRED_SLP = 90;
    public static int INFECTION_LEVEL_4_BLEED_HP = 6;
    public static int INFECTION_EFFECT_TRIGGER_CHANCE_1000 = 2;
    public static int UPGRADE_SKILLS_TO_CHOOSE_FROM = 5;
    public static int UNDEAD_UPGRADE_SKILLS_TO_CHOOSE_FROM = 2;
    public static int SKILL_AGILE_ATK_BONUS = 2;
    public static int SKILL_AGILE_DEF_BONUS = 2;
    public static float SKILL_AWAKE_SLEEP_BONUS = 0.15f;
    public static float SKILL_AWAKE_SLEEP_REGEN_BONUS = 0.2f;
    public static int SKILL_BOWS_ATK_BONUS = 5;
    public static int SKILL_BOWS_DMG_BONUS = 2;
    public static float SKILL_CARPENTRY_BARRICADING_BONUS = 0.2f;
    public static int SKILL_CARPENTRY_LEVEL3_BUILD_BONUS = 1;
    public static int SKILL_CHARISMATIC_TRUST_BONUS = 1;
    public static int SKILL_CHARISMATIC_TRADE_BONUS = 10;
    public static int SKILL_FIREARMS_ATK_BONUS = 5;
    public static int SKILL_FIREARMS_DMG_BONUS = 2;
    public static int SKILL_HARDY_HEAL_CHANCE_BONUS = 1;
    public static int SKILL_HAULER_INV_BONUS = 1;
    public static int SKILL_HIGH_STAMINA_STA_BONUS = 5;
    public static int SKILL_LEADERSHIP_FOLLOWER_BONUS = 1;
    public static float SKILL_LIGHT_EATER_FOOD_BONUS = 0.2f;
    public static float SKILL_LIGHT_EATER_MAXFOOD_BONUS = 0.15f;
    public static int SKILL_LIGHT_FEET_TRAP_BONUS = 5;
    public static int SKILL_LIGHT_SLEEPER_WAKEUP_CHANCE_BONUS = 10;
    public static int SKILL_MARTIAL_ARTS_ATK_BONUS = 3;
    public static int SKILL_MARTIAL_ARTS_DMG_BONUS = 1;
    public static float SKILL_MEDIC_BONUS = 0.2f;
    public static int SKILL_MEDIC_REVIVE_BONUS = 10;
    public static int SKILL_MEDIC_LEVEL_FOR_REVIVE_EST = 1;
    public static int SKILL_NECROLOGY_UNDEAD_BONUS = 2;
    public static int SKILL_NECROLOGY_CORPSE_BONUS = 4;
    public static int SKILL_NECROLOGY_LEVEL_FOR_INFECTION = 3;
    public static int SKILL_NECROLOGY_LEVEL_FOR_RISE = 5;
    public static float SKILL_STRONG_PSYCHE_LEVEL_BONUS = 0.15f;
    public static float SKILL_STRONG_PSYCHE_ENT_BONUS = 0.15f;
    public static int SKILL_STRONG_DMG_BONUS = 2;
    public static int SKILL_STRONG_THROW_BONUS = 1;
    public static int SKILL_TOUGH_HP_BONUS = 3;
    public static int SKILL_UNSUSPICIOUS_BONUS = 25;
    public static int UNSUSPICIOUS_BAD_OUTFIT_PENALTY = 50;
    public static int UNSUSPICIOUS_GOOD_OUTFIT_BONUS = 50;
    public static int SKILL_ZAGILE_ATK_BONUS = 1;
    public static int SKILL_ZAGILE_DEF_BONUS = 2;
    public static int SKILL_ZSTRONG_DMG_BONUS = 2;
    public static int SKILL_ZTOUGH_HP_BONUS = 4;
    public static float SKILL_ZEATER_REGEN_BONUS = 0.2f;
    public static float SKILL_ZTRACKER_SMELL_BONUS = 0.1f;
    public static int SKILL_ZLIGHT_FEET_TRAP_BONUS = 3;
    public static int SKILL_ZGRAB_CHANCE = 2;
    public static float SKILL_ZINFECTOR_BONUS = 0.1f;
    public static float SKILL_ZLIGHT_EATER_FOOD_BONUS = 0.1f;
    public static float SKILL_ZLIGHT_EATER_MAXFOOD_BONUS = 0.15f;
    public const int BASE_ACTION_COST = 100;
    public const int BASE_SPEED = 100;
    public const int STAMINA_INFINITE = 99;
    public const int STAMINA_MIN_FOR_ACTIVITY = 10;
    public const int STAMINA_COST_RUNNING = 4;
    public const int STAMINA_REGEN_WAIT = 2;
    public const int STAMINA_REGEN_PER_TURN = 2;
    public const int STAMINA_COST_JUMP = 8;
    public const int STAMINA_COST_MELEE_ATTACK = 8;
    public const int STAMINA_COST_MOVE_DRAGGED_CORPSE = 8;
    public const int JUMP_STUMBLE_CHANCE = 25;
    public const int JUMP_STUMBLE_ACTION_COST = 100;
    public const int LIVING_SCENT_DROP = 270;
    public const int UNDEAD_MASTER_SCENT_DROP = 270;
    public const int BARRICADING_MAX = 80;
    private const int MINIMAL_FOV = 2;
    public const int FOV_PENALTY_SUNSET = 1;
    public const int FOV_PENALTY_EVENING = 2;
    public const int FOV_PENALTY_MIDNIGHT = 3;
    public const int FOV_PENALTY_DEEP_NIGHT = 4;
    public const int FOV_PENALTY_SUNRISE = 2;
    public const int NIGHT_STA_PENALTY = 2;
    public const int FOV_PENALTY_RAIN = 1;
    public const int FOV_PENALTY_HEAVY_RAIN = 2;
    public const int MELEE_WEAPON_BREAK_CHANCE = 1;
    public const int MELEE_WEAPON_FRAGILE_BREAK_CHANCE = 3;
    public const int FIREARM_JAM_CHANCE_NO_RAIN = 1;
    public const int FIREARM_JAM_CHANCE_RAIN = 3;
    private const int FIRE_DISTANCE_VS_RANGE_MODIFIER = 2;
    private const float FIRING_WHEN_STA_TIRED = 0.75f;
    private const float FIRING_WHEN_STA_NOT_FULL = 0.9f;
    public const int BODY_ARMOR_BREAK_CHANCE = 2;
    public const int FOOD_BASE_POINTS = 1440;
    public const int FOOD_HUNGRY_LEVEL = 720;
    public const int ROT_BASE_POINTS = 2880;
    public const int ROT_HUNGRY_LEVEL = 1440;
    public const int SLEEP_BASE_POINTS = 1800;
    public const int SLEEP_SLEEPY_LEVEL = 900;
    public const int SANITY_BASE_POINTS = 2880;
    public const int SANITY_UNSTABLE_LEVEL = 1440;
    public const int SANITY_NIGHTMARE_CHANCE = 2;
    public const int SANITY_NIGHTMARE_SLP_LOSS = 60;
    public const int SANITY_NIGHTMARE_SAN_LOSS = 30;
    public const int SANITY_INSANE_ACTION_CHANCE = 5;
    public const int SANITY_HIT_BUTCHERING_CORPSE = 30;
    public const int SANITY_HIT_UNDEAD_EATING_CORPSE = 60;
    public const int SANITY_HIT_LIVING_EATING_CORPSE = 120;
    public const int SANITY_HIT_EATEN_ALIVE = 120;
    public const int SANITY_HIT_ZOMBIFY = 60;
    public const int SANITY_HIT_BOND_DEATH = 240;
    public const int SANITY_RECOVER_KILL_UNDEAD = 90;
    public const int SANITY_RECOVER_BOND_CHANCE = 5;
    public const int SANITY_RECOVER_BOND = 30;
    public const int FOOD_STARVING_DEATH_CHANCE = 5;
    public const int FOOD_EXPIRED_VOMIT_CHANCE = 25;
    public const int FOOD_VOMIT_STA_COST = 100;
    public const int ROT_STARVING_HP_CHANCE = 5;
    public const int ROT_HUNGRY_SKILL_CHANCE = 5;
    public const int SLEEP_EXHAUSTION_COLLAPSE_CHANCE = 5;
    private const int SLEEP_COUCH_SLEEPING_REGEN = 6;
    public const int SLEEP_NOCOUCH_SLEEPING_REGEN = 4;
    public const int SLEEP_ON_COUCH_HEAL_CHANCE = 5;
    public const int SLEEP_HEAL_HITPOINTS = 2;
    public const int LOUD_NOISE_RADIUS = 5;
    private const int LOUD_NOISE_BASE_WAKEUP_CHANCE = 10;
    private const int LOUD_NOISE_DISTANCE_BONUS = 10;
    public const int VICTIM_DROP_GENERIC_ITEM_CHANCE = 100;
    public const int VICTIM_DROP_AMMOFOOD_ITEM_CHANCE = 100;
    public const int IMPROVED_WEAPONS_FROM_BROKEN_WOOD_CHANCE = 25;
    public const float RAPID_FIRE_FIRST_SHOT_ACCURACY = 0.5f;
    public const float RAPID_FIRE_SECOND_SHOT_ACCURACY = 0.3f;
    public const int ZTRACKINGRADIUS = 6;
    public const int DEFAULT_ACTOR_WEIGHT = 10;
    public const int FIRE_RAIN_TEST_CHANCE = 1;
    public const int FIRE_RAIN_PUT_OUT_CHANCE = 10;
    public const int TRUST_NEUTRAL = 0;
    public const int TRUST_TRUSTING_THRESHOLD = 360;
    public const int TRUST_MIN = -360;
    public const int TRUST_MAX = 1440;
    public const int TRUST_BOND_THRESHOLD = 1440;
    public const int TRUST_BASE_INCREASE = 1;
    public const int TRUST_GOOD_GIFT_INCREASE = 90;
    public const int TRUST_MISC_GIFT_INCREASE = 10;
    public const int TRUST_GIVE_ITEM_ORDER_PENALTY = -30;
    public const int TRUST_LEADER_KILL_ENEMY = 90;
    public const int TRUST_REVIVE_BONUS = 360;
    public const int MURDERER_SPOTTING_BASE_CHANCE = 5;
    public const int MURDERER_SPOTTING_DISTANCE_PENALTY = 1;
    public const int MURDER_SPOTTING_MURDERCOUNTER_BONUS = 5;
    private const float INFECTION_BASE_FACTOR = 1f;
    private const int CORPSE_ZOMBIFY_BASE_CHANCE = 0;
    private const int CORPSE_ZOMBIFY_DELAY = 180;
    private const float CORPSE_ZOMBIFY_INFECTIONP_FACTOR = 1f;
    private const float CORPSE_ZOMBIFY_NIGHT_FACTOR = 2f;
    private const float CORPSE_ZOMBIFY_DAY_FACTOR = 0.01f;
    private const float CORPSE_ZOMBIFY_TIME_FACTOR = 0.001388889f;
    private const float CORPSE_EATING_NUTRITION_FACTOR = 10f;
    private const float CORPSE_EATING_INFECTION_FACTOR = 0.1f;
    private const float CORPSE_DECAY_PER_TURN = 0.005555556f;
    public const int GIVE_RARE_ITEM_DAY = 7;
    public const int GIVE_RARE_ITEM_CHANCE = 5;
    private readonly DiceRoller m_DiceRoller;

    public DiceRoller DiceRoller
    {
      get
      {
        return this.m_DiceRoller;
      }
    }

    public Rules(DiceRoller diceRoller)
    {
      if (diceRoller == null)
        throw new ArgumentNullException(nameof (diceRoller));
      this.m_DiceRoller = diceRoller;
    }

    public int Roll(int min, int max)
    {
      return this.m_DiceRoller.Roll(min, max);
    }

    public bool RollChance(int chance)
    {
      return this.m_DiceRoller.RollChance(chance);
    }

    public float RollFloat()
    {
      return this.m_DiceRoller.RollFloat();
    }

    public float Randomize(float value, float deviation)
    {
      float num = deviation / 2f;
      return (float) ((double) value - (double) num * (double) this.m_DiceRoller.RollFloat() + (double) num * (double) this.m_DiceRoller.RollFloat());
    }

    public int RollX(Map map)
    {
      if (map == null)
        throw new ArgumentNullException(nameof (map));
      return this.m_DiceRoller.Roll(0, map.Width);
    }

    public int RollY(Map map)
    {
      if (map == null)
        throw new ArgumentNullException(nameof (map));
      return this.m_DiceRoller.Roll(0, map.Height);
    }

    public Direction RollDirection()
    {
      return Direction.COMPASS[this.m_DiceRoller.Roll(0, 8)];
    }

    public int RollSkill(int skillValue)
    {
      if (skillValue <= 0)
        return 0;
      return (this.m_DiceRoller.Roll(0, skillValue + 1) + this.m_DiceRoller.Roll(0, skillValue + 1)) / 2;
    }

    public int RollDamage(int damageValue)
    {
      if (damageValue <= 0)
        return 0;
      return this.m_DiceRoller.Roll(damageValue / 2, damageValue + 1);
    }

    public Location RollNeighbourInMap(Location from)
    {
      Location location;
      Map map;
      int x;
      int y;
      do
      {
        do
        {
          location = from + this.RollDirection();
        }
        while (location.Map != from.Map);
        map = from.Map;
        Point position = location.Position;
        x = position.X;
        position = location.Position;
        y = position.Y;
      }
      while (!map.IsInBounds(x, y));
      return location;
    }

    public bool RollValidMoveInMap(Actor actor, int maxTries, out Location nextLocation)
    {
      if (actor == null)
        throw new ArgumentNullException(nameof (actor));
      for (int index = 0; index < maxTries; ++index)
      {
        nextLocation = this.RollNeighbourInMap(actor.Location);
        Actor actor1 = actor;
        Map map = nextLocation.Map;
        Point position = nextLocation.Position;
        int x = position.X;
        position = nextLocation.Position;
        int y = position.Y;
        if (this.IsWalkableFor(actor1, map, x, y))
          return true;
      }
      nextLocation = actor.Location;
      return false;
    }

    public bool RollValidMoveDirection(Actor actor, int maxTries, out Direction direction)
    {
      if (actor == null)
        throw new ArgumentNullException(nameof (actor));
      for (int index = 0; index < maxTries; ++index)
      {
        direction = this.RollDirection();
        Location location = actor.Location + direction;
        if (location.Map == actor.Location.Map && this.IsWalkableFor(actor, location))
          return true;
      }
      direction = (Direction) null;
      return false;
    }

    public bool RollValidBumpDirection(Actor actor, int maxTries, out Direction direction)
    {
      if (actor == null)
        throw new ArgumentNullException(nameof (actor));
      for (int index = 0; index < maxTries; ++index)
      {
        direction = this.RollDirection();
        if ((actor.Location + direction).Map == actor.Location.Map)
          return true;
      }
      direction = (Direction) null;
      return false;
    }

    public bool CanActorGetItemFromContainer(Actor actor, Point position)
    {
      string reason;
      return this.CanActorGetItemFromContainer(actor, position, out reason);
    }

    public bool CanActorGetItemFromContainer(Actor actor, Point position, out string reason)
    {
      if (actor == null)
        throw new ArgumentNullException(nameof (actor));
      MapObject mapObjectAt = actor.Location.Map.GetMapObjectAt(position);
      if (mapObjectAt == null || !mapObjectAt.IsContainer)
      {
        reason = "object is not a container";
        return false;
      }
      Inventory itemsAt = actor.Location.Map.GetItemsAt(position);
      if (itemsAt == null || itemsAt.IsEmpty)
      {
        reason = "nothing to take there";
        return false;
      }
      if (!actor.Model.Abilities.HasInventory || !actor.Model.Abilities.CanUseMapObjects || (actor.Inventory == null || !this.CanActorGetItem(actor, itemsAt.TopItem)))
      {
        reason = "cannot take an item";
        return false;
      }
      reason = "";
      return true;
    }

    public bool CanActorGetItem(Actor actor, Item it)
    {
      string reason;
      return this.CanActorGetItem(actor, it, out reason);
    }

    public bool CanActorGetItem(Actor actor, Item it, out string reason)
    {
      if (actor == null)
        throw new ArgumentNullException(nameof (actor));
      if (it == null)
        throw new ArgumentNullException("item");
      if (!actor.Model.Abilities.HasInventory || !actor.Model.Abilities.CanUseMapObjects || actor.Inventory == null)
      {
        reason = "no inventory";
        return false;
      }
      if (actor.Inventory.IsFull && !actor.Inventory.CanAddAtLeastOne(it))
      {
        reason = "inventory is full";
        return false;
      }
      if (it is ItemTrap && (it as ItemTrap).IsTriggered)
      {
        reason = "triggered trap";
        return false;
      }
      reason = "";
      return true;
    }

    public bool CanActorEquipItem(Actor actor, Item it)
    {
      string reason;
      return this.CanActorEquipItem(actor, it, out reason);
    }

    public bool CanActorEquipItem(Actor actor, Item it, out string reason)
    {
      if (actor == null)
        throw new ArgumentNullException(nameof (actor));
      if (it == null)
        throw new ArgumentNullException("item");
      if (!actor.Model.Abilities.CanUseItems)
      {
        reason = "no ability to use items";
        return false;
      }
      if (!it.Model.IsEquipable)
      {
        reason = "this item cannot be equipped";
        return false;
      }
      reason = "";
      return true;
    }

    public bool CanActorUnequipItem(Actor actor, Item it)
    {
      string reason;
      return this.CanActorUnequipItem(actor, it, out reason);
    }

    public bool CanActorUnequipItem(Actor actor, Item it, out string reason)
    {
      if (actor == null)
        throw new ArgumentNullException(nameof (actor));
      if (it == null)
        throw new ArgumentNullException("item");
      if (!it.IsEquipped)
      {
        reason = "not equipped";
        return false;
      }
      Inventory inventory = actor.Inventory;
      if (inventory == null || !inventory.Contains(it))
      {
        reason = "not in inventory";
        return false;
      }
      reason = "";
      return true;
    }

    public bool CanActorDropItem(Actor actor, Item it)
    {
      string reason;
      return this.CanActorDropItem(actor, it, out reason);
    }

    public bool CanActorDropItem(Actor actor, Item it, out string reason)
    {
      if (actor == null)
        throw new ArgumentNullException(nameof (actor));
      if (it == null)
        throw new ArgumentNullException("item");
      if (it.IsEquipped)
      {
        reason = "unequip first";
        return false;
      }
      Inventory inventory = actor.Inventory;
      if (inventory == null || !inventory.Contains(it))
      {
        reason = "not in inventory";
        return false;
      }
      reason = "";
      return true;
    }

    public bool CanActorUseItem(Actor actor, Item it)
    {
      string reason;
      return this.CanActorUseItem(actor, it, out reason);
    }

    public bool CanActorUseItem(Actor actor, Item it, out string reason)
    {
      if (actor == null)
        throw new ArgumentNullException(nameof (actor));
      if (it == null)
        throw new ArgumentNullException("item");
      if (!actor.Model.Abilities.CanUseItems)
      {
        reason = "no ability to use items";
        return false;
      }
      if (it is ItemWeapon)
      {
        reason = "to use a weapon, equip it";
        return false;
      }
      if (it is ItemFood && !actor.Model.Abilities.HasToEat)
      {
        reason = "no ability to eat";
        return false;
      }
      if (it is ItemMedicine && actor.Model.Abilities.IsUndead)
      {
        reason = "undeads cannot use medecine";
        return false;
      }
      if (it is ItemBarricadeMaterial)
      {
        reason = "to use material, build a barricade";
        return false;
      }
      if (it is ItemAmmo)
      {
        ItemAmmo itemAmmo = it as ItemAmmo;
        ItemRangedWeapon equippedWeapon = actor.GetEquippedWeapon() as ItemRangedWeapon;
        if (equippedWeapon == null || equippedWeapon.AmmoType != itemAmmo.AmmoType)
        {
          reason = "no compatible ranged weapon equipped";
          return false;
        }
        if (equippedWeapon.Ammo >= (equippedWeapon.Model as ItemRangedWeaponModel).MaxAmmo)
        {
          reason = "weapon already fully loaded";
          return false;
        }
      }
      else if (it is ItemSprayScent)
      {
        if ((it as ItemSprayScent).SprayQuantity <= 0)
        {
          reason = "no spray left.";
          return false;
        }
      }
      else if (it is ItemTrap)
      {
        if (!(it as ItemTrap).TrapModel.UseToActivate)
        {
          reason = "does not activate manually";
          return false;
        }
      }
      else if (it is ItemEntertainment)
      {
        if (!actor.Model.Abilities.IsIntelligent)
        {
          reason = "not intelligent";
          return false;
        }
        if (actor.IsBoredOf(it))
        {
          reason = "bored by this";
          return false;
        }
      }
      Inventory inventory = actor.Inventory;
      if (inventory == null || !inventory.Contains(it))
      {
        reason = "not in inventory";
        return false;
      }
      reason = "";
      return true;
    }

    public bool CanActorEatFoodOnGround(Actor actor, Item it, out string reason)
    {
      if (actor == null)
        throw new ArgumentNullException(nameof (actor));
      if (it == null)
        throw new ArgumentNullException("item");
      if (!(it is ItemFood))
      {
        reason = "not food";
        return false;
      }
      Inventory itemsAt = actor.Location.Map.GetItemsAt(actor.Location.Position);
      if (itemsAt == null || !itemsAt.Contains(it))
      {
        reason = "item not here";
        return false;
      }
      reason = "";
      return true;
    }

    public bool CanActorRechargeItemBattery(Actor actor, Item it, out string reason)
    {
      if (actor == null)
        throw new ArgumentNullException(nameof (actor));
      if (it == null)
        throw new ArgumentNullException("item");
      if (!actor.Model.Abilities.CanUseItems)
      {
        reason = "no ability to use items";
        return false;
      }
      if (!it.IsEquipped || !actor.Inventory.Contains(it))
      {
        reason = "item not equipped";
        return false;
      }
      if (!this.IsItemBatteryPowered(it))
      {
        reason = "not a battery powered item";
        return false;
      }
      reason = "";
      return true;
    }

    public bool IsItemBatteryPowered(Item it)
    {
      if (it == null)
        return false;
      if (!(it is ItemLight))
        return it is ItemTracker;
      return true;
    }

    public bool IsItemBatteryFull(Item it)
    {
      if (it == null)
        return true;
      ItemLight itemLight = it as ItemLight;
      if (itemLight != null && itemLight.IsFullyCharged)
        return true;
      ItemTracker itemTracker = it as ItemTracker;
      return itemTracker != null && itemTracker.IsFullyCharged;
    }

    public bool CanActorGiveItemTo(Actor actor, Actor target, Item gift, out string reason)
    {
      if (actor == null)
        throw new ArgumentNullException(nameof (actor));
      if (target == null)
        throw new ArgumentNullException(nameof (target));
      if (gift == null)
        throw new ArgumentNullException(nameof (gift));
      if (this.IsEnemyOf(actor, target))
      {
        reason = "enemy";
        return false;
      }
      if (gift.IsEquipped)
      {
        reason = "equipped";
        return false;
      }
      if (target.IsSleeping)
      {
        reason = "sleeping";
        return false;
      }
      return this.CanActorGetItem(target, gift, out reason);
    }

    public bool CanActorRun(Actor actor)
    {
      string reason;
      return this.CanActorRun(actor, out reason);
    }

    public bool CanActorRun(Actor actor, out string reason)
    {
      if (actor == null)
        throw new ArgumentNullException(nameof (actor));
      if (!actor.Model.Abilities.CanRun)
      {
        reason = "no ability to run";
        return false;
      }
      if (actor.StaminaPoints < 10)
      {
        reason = "not enough stamina to run";
        return false;
      }
      reason = "";
      return true;
    }

    public bool IsActorTired(Actor actor)
    {
      if (actor == null)
        throw new ArgumentNullException(nameof (actor));
      if (actor.Model.Abilities.CanTire)
        return actor.StaminaPoints < 10;
      return false;
    }

    public bool CanActorMeleeAttack(Actor actor, Actor target)
    {
      string reason;
      return this.CanActorMeleeAttack(actor, target, out reason);
    }

    public bool CanActorMeleeAttack(Actor actor, Actor target, out string reason)
    {
      if (actor == null)
        throw new ArgumentNullException(nameof (actor));
      if (target == null)
        throw new ArgumentNullException(nameof (target));
      if (actor.Location.Map == target.Location.Map)
      {
        Location location = actor.Location;
        Point position1 = location.Position;
        location = target.Location;
        Point position2 = location.Position;
        if (!this.IsAdjacent(position1, position2))
        {
          reason = "not adjacent";
          return false;
        }
      }
      else
      {
        Exit exitAt = actor.Location.Map.GetExitAt(actor.Location.Position);
        if (exitAt == null)
        {
          reason = "not reachable";
          return false;
        }
        if (target.Location.Map.GetExitAt(target.Location.Position) == null)
        {
          reason = "not reachable";
          return false;
        }
        if (exitAt.ToMap != target.Location.Map || exitAt.ToPosition != target.Location.Position)
        {
          reason = "not reachable";
          return false;
        }
      }
      if (actor.StaminaPoints < 10)
      {
        reason = "not enough stamina to attack";
        return false;
      }
      if (target.IsDead)
      {
        reason = "already dead!";
        return false;
      }
      reason = "";
      return true;
    }

    public bool HasActorJumpAbility(Actor actor)
    {
      if (!actor.Model.Abilities.CanJump && actor.Sheet.SkillTable.GetSkillLevel(0) <= 0)
        return actor.Sheet.SkillTable.GetSkillLevel(20) > 0;
      return true;
    }

    public bool IsWalkableFor(Actor actor, Map map, int x, int y)
    {
      string reason;
      return this.IsWalkableFor(actor, map, x, y, out reason);
    }

    public bool IsWalkableFor(Actor actor, Map map, int x, int y, out string reason)
    {
      if (map == null)
        throw new ArgumentNullException(nameof (map));
      if (actor == null)
        throw new ArgumentNullException(nameof (actor));
      if (!map.IsInBounds(x, y))
      {
        reason = "out of map";
        return false;
      }
      if (!map.GetTileAt(x, y).Model.IsWalkable)
      {
        reason = "blocked";
        return false;
      }
      MapObject mapObjectAt = map.GetMapObjectAt(x, y);
      if (mapObjectAt != null && !mapObjectAt.IsWalkable)
      {
        if (mapObjectAt.IsJumpable)
        {
          if (!this.HasActorJumpAbility(actor))
          {
            reason = "cannot jump";
            return false;
          }
          if (actor.StaminaPoints < 8)
          {
            reason = "not enough stamina to jump";
            return false;
          }
        }
        else if (actor.Model.Abilities.IsSmall)
        {
          DoorWindow doorWindow = mapObjectAt as DoorWindow;
          if (doorWindow != null && doorWindow.IsClosed)
          {
            reason = "cannot slip through closed door";
            return false;
          }
        }
        else
        {
          reason = "blocked by object";
          return false;
        }
      }
      if (map.GetActorAt(x, y) != null)
      {
        reason = "someone is there";
        return false;
      }
      if (actor.DraggedCorpse != null && this.IsActorTired(actor))
      {
        reason = "dragging a corpse when tired";
        return false;
      }
      reason = "";
      return true;
    }

    public bool IsWalkableFor(Actor actor, Location location)
    {
      string reason;
      return this.IsWalkableFor(actor, location, out reason);
    }

    public bool IsWalkableFor(Actor actor, Location location, out string reason)
    {
      Actor actor1 = actor;
      Map map = location.Map;
      Point position = location.Position;
      int x = position.X;
      position = location.Position;
      int y = position.Y;
      ref string local = ref reason;
      return this.IsWalkableFor(actor1, map, x, y, out local);
    }

    public ActorAction IsBumpableFor(Actor actor, RogueGame game, Map map, int x, int y, out string reason)
    {
      if (map == null)
        throw new ArgumentNullException(nameof (map));
      if (actor == null)
        throw new ArgumentNullException(nameof (actor));
      reason = "";
      if (!map.IsInBounds(x, y))
      {
        if (!this.CanActorLeaveMap(actor, out reason))
          return (ActorAction) null;
        reason = "";
        return (ActorAction) new ActionLeaveMap(actor, game, new Point(x, y));
      }
      Point point = new Point(x, y);
      ActionMoveStep actionMoveStep = new ActionMoveStep(actor, game, point);
      if (actionMoveStep.IsLegal())
      {
        reason = "";
        return (ActorAction) actionMoveStep;
      }
      reason = actionMoveStep.FailReason;
      Actor actorAt = map.GetActorAt(point);
      if (actorAt != null)
      {
        if (this.IsEnemyOf(actor, actorAt))
        {
          if (this.CanActorMeleeAttack(actor, actorAt, out reason))
            return (ActorAction) new ActionMeleeAttack(actor, game, actorAt);
          return (ActorAction) null;
        }
        if (!actor.IsPlayer && !actorAt.IsPlayer && this.CanActorSwitchPlaceWith(actor, actorAt, out reason))
          return (ActorAction) new ActionSwitchPlace(actor, game, actorAt);
        if (this.CanActorChatWith(actor, actorAt, out reason))
          return (ActorAction) new ActionChat(actor, game, actorAt);
        return (ActorAction) null;
      }
      MapObject mapObjectAt = map.GetMapObjectAt(point);
      if (mapObjectAt != null)
      {
        DoorWindow door = mapObjectAt as DoorWindow;
        if (door != null)
        {
          if (door.IsClosed)
          {
            if (this.IsOpenableFor(actor, door, out reason))
              return (ActorAction) new ActionOpenDoor(actor, game, door);
            if (this.IsBashableFor(actor, door, out reason))
              return (ActorAction) new ActionBashDoor(actor, game, door);
            return (ActorAction) null;
          }
          if (door.BarricadePoints > 0)
          {
            if (this.IsBashableFor(actor, door, out reason))
              return (ActorAction) new ActionBashDoor(actor, game, door);
            reason = "cannot bash the barricade";
            return (ActorAction) null;
          }
        }
        if (this.CanActorGetItemFromContainer(actor, point, out reason))
          return (ActorAction) new ActionGetFromContainer(actor, game, point);
        if (actor.Model.Abilities.CanBashDoors && this.IsBreakableFor(actor, mapObjectAt, out reason))
          return (ActorAction) new ActionBreak(actor, game, mapObjectAt);
        PowerGenerator powGen = mapObjectAt as PowerGenerator;
        if (powGen != null)
        {
          if (powGen.IsOn)
          {
            Item equippedItem1 = actor.GetEquippedItem(DollPart.LEFT_HAND);
            if (equippedItem1 != null && this.CanActorRechargeItemBattery(actor, equippedItem1, out reason))
              return (ActorAction) new ActionRechargeItemBattery(actor, game, equippedItem1);
            Item equippedItem2 = actor.GetEquippedItem(DollPart._FIRST);
            if (equippedItem2 != null && this.CanActorRechargeItemBattery(actor, equippedItem2, out reason))
              return (ActorAction) new ActionRechargeItemBattery(actor, game, equippedItem2);
          }
          if (this.IsSwitchableFor(actor, powGen, out reason))
            return (ActorAction) new ActionSwitchPowerGenerator(actor, game, powGen);
          return (ActorAction) null;
        }
      }
      return (ActorAction) null;
    }

    public ActorAction IsBumpableFor(Actor actor, RogueGame game, Location location)
    {
      string reason;
      return this.IsBumpableFor(actor, game, location, out reason);
    }

    public ActorAction IsBumpableFor(Actor actor, RogueGame game, Location location, out string reason)
    {
      Actor actor1 = actor;
      RogueGame game1 = game;
      Map map = location.Map;
      Point position = location.Position;
      int x = position.X;
      position = location.Position;
      int y = position.Y;
      ref string local = ref reason;
      return this.IsBumpableFor(actor1, game1, map, x, y, out local);
    }

    public bool IsSwitchableFor(Actor actor, PowerGenerator powGen, out string reason)
    {
      if (actor == null)
        throw new ArgumentNullException(nameof (actor));
      if (powGen == null)
        throw new ArgumentNullException(nameof (powGen));
      if (!actor.Model.Abilities.CanUseMapObjects)
      {
        reason = "cannot use map objects";
        return false;
      }
      if (actor.IsSleeping)
      {
        reason = "is sleeping";
        return false;
      }
      reason = "";
      return true;
    }

    public bool CanActorLeaveMap(Actor actor, out string reason)
    {
      if (actor == null)
        throw new ArgumentNullException(nameof (actor));
      if (!actor.IsPlayer)
      {
        reason = "can't leave maps";
        return false;
      }
      reason = "";
      return true;
    }

    public bool CanActorUseExit(Actor actor, Point exitPoint)
    {
      string reason;
      return this.CanActorUseExit(actor, exitPoint, out reason);
    }

    public bool CanActorUseExit(Actor actor, Point exitPoint, out string reason)
    {
      if (actor == null)
        throw new ArgumentNullException(nameof (actor));
      if (actor.Location.Map.GetExitAt(exitPoint) == null)
      {
        reason = "no exit there";
        return false;
      }
      if (!actor.IsPlayer && !actor.Model.Abilities.AI_CanUseAIExits)
      {
        reason = "this AI can't use exits";
        return false;
      }
      if (actor.IsSleeping)
      {
        reason = "is sleeping";
        return false;
      }
      reason = "";
      return true;
    }

    public bool CanActorChatWith(Actor speaker, Actor target, out string reason)
    {
      if (speaker == null)
        throw new ArgumentNullException(nameof (speaker));
      if (target == null)
        throw new ArgumentNullException(nameof (target));
      if (!speaker.Model.Abilities.CanTalk)
      {
        reason = "can't talk";
        return false;
      }
      if (!target.Model.Abilities.CanTalk)
      {
        reason = string.Format("{0} can't talk", (object) target.TheName);
        return false;
      }
      if (speaker.IsSleeping)
      {
        reason = "sleeping";
        return false;
      }
      if (target.IsSleeping)
      {
        reason = string.Format("{0} is sleeping", (object) target.TheName);
        return false;
      }
      reason = "";
      return true;
    }

    public bool CanActorInitiateTradeWith(Actor speaker, Actor target)
    {
      string reason;
      return this.CanActorInitiateTradeWith(speaker, target, out reason);
    }

    public bool CanActorInitiateTradeWith(Actor speaker, Actor target, out string reason)
    {
      if (speaker == null)
        throw new ArgumentNullException(nameof (speaker));
      if (target == null)
        throw new ArgumentNullException(nameof (target));
      if (target.IsPlayer)
      {
        reason = "target is player";
        return false;
      }
      if (!speaker.Model.Abilities.CanTrade && target.Leader != speaker)
      {
        reason = "can't trade";
        return false;
      }
      if (!target.Model.Abilities.CanTrade && target.Leader != speaker)
      {
        reason = "target can't trade";
        return false;
      }
      if (this.IsEnemyOf(speaker, target))
      {
        reason = "is an enemy";
        return false;
      }
      if (target.IsSleeping)
      {
        reason = "is sleeping";
        return false;
      }
      if (speaker.Inventory == null || speaker.Inventory.IsEmpty)
      {
        reason = "nothing to offer";
        return false;
      }
      if (target.Inventory == null || target.Inventory.IsEmpty)
      {
        reason = "has nothing to trade";
        return false;
      }
      reason = "";
      return true;
    }

    public bool CanActorShout(Actor speaker)
    {
      string reason;
      return this.CanActorShout(speaker, out reason);
    }

    public bool CanActorShout(Actor speaker, out string reason)
    {
      if (speaker == null)
        throw new ArgumentNullException(nameof (speaker));
      if (speaker.IsSleeping)
      {
        reason = "sleeping";
        return false;
      }
      if (!speaker.Model.Abilities.CanTalk)
      {
        reason = "can't talk";
        return false;
      }
      reason = "";
      return true;
    }

    public bool IsOpenableFor(Actor actor, DoorWindow door)
    {
      string reason;
      return this.IsOpenableFor(actor, door, out reason);
    }

    public bool IsOpenableFor(Actor actor, DoorWindow door, out string reason)
    {
      if (actor == null)
        throw new ArgumentNullException(nameof (actor));
      if (door == null)
        throw new ArgumentNullException(nameof (door));
      if (!actor.Model.Abilities.CanUseMapObjects)
      {
        reason = "no ability to open";
        return false;
      }
      if (!door.IsClosed || door.BarricadePoints > 0)
      {
        reason = "not closed nor barricaded";
        return false;
      }
      reason = "";
      return true;
    }

    public bool IsClosableFor(Actor actor, DoorWindow door)
    {
      string reason;
      return this.IsClosableFor(actor, door, out reason);
    }

    public bool IsClosableFor(Actor actor, DoorWindow door, out string reason)
    {
      if (actor == null)
        throw new ArgumentNullException(nameof (actor));
      if (door == null)
        throw new ArgumentNullException(nameof (door));
      if (!actor.Model.Abilities.CanUseMapObjects)
      {
        reason = "can't use objects";
        return false;
      }
      if (!door.IsOpen)
      {
        reason = "not open";
        return false;
      }
      if (door.Location.Map.GetActorAt(door.Location.Position) != null)
      {
        reason = "someone is there";
        return false;
      }
      reason = "";
      return true;
    }

    public bool CanActorBarricadeDoor(Actor actor, DoorWindow door)
    {
      string reason;
      return this.CanActorBarricadeDoor(actor, door, out reason);
    }

    public bool CanActorBarricadeDoor(Actor actor, DoorWindow door, out string reason)
    {
      if (actor == null)
        throw new ArgumentNullException(nameof (actor));
      if (door == null)
        throw new ArgumentNullException(nameof (door));
      if (!actor.Model.Abilities.CanBarricade)
      {
        reason = "no ability to barricade";
        return false;
      }
      if (door.State != 1 && door.State != 3)
      {
        reason = "not open or broken";
        return false;
      }
      if (door.BarricadePoints >= 80)
      {
        reason = "barricade limit reached";
        return false;
      }
      if (door.Location.Map.GetActorAt(door.Location.Position) != null)
      {
        reason = "someone is there";
        return false;
      }
      if (actor.Inventory == null || actor.Inventory.IsEmpty)
      {
        reason = "no items";
        return false;
      }
      if (!actor.Inventory.HasItemOfType(typeof (ItemBarricadeMaterial)))
      {
        reason = "no barricading material";
        return false;
      }
      reason = "";
      return true;
    }

    public bool IsBashableFor(Actor actor, DoorWindow door)
    {
      string reason;
      return this.IsBashableFor(actor, door, out reason);
    }

    public bool IsBashableFor(Actor actor, DoorWindow door, out string reason)
    {
      if (actor == null)
        throw new ArgumentNullException(nameof (actor));
      if (door == null)
        throw new ArgumentNullException(nameof (door));
      if (!actor.Model.Abilities.CanBashDoors)
      {
        reason = "can't bash doors";
        return false;
      }
      if (this.IsActorTired(actor))
      {
        reason = "tired";
        return false;
      }
      if (door.BreakState != MapObject.Break.BREAKABLE && !door.IsBarricaded)
      {
        reason = "can't break this object";
        return false;
      }
      reason = "";
      return true;
    }

    public bool IsBreakableFor(Actor actor, MapObject mapObj)
    {
      string reason;
      return this.IsBreakableFor(actor, mapObj, out reason);
    }

    public bool IsBreakableFor(Actor actor, MapObject mapObj, out string reason)
    {
      if (actor == null)
        throw new ArgumentNullException(nameof (actor));
      if (mapObj == null)
        throw new ArgumentNullException(nameof (mapObj));
      if (!actor.Model.Abilities.CanBreakObjects)
      {
        reason = "cannot break objects";
        return false;
      }
      if (this.IsActorTired(actor))
      {
        reason = "tired";
        return false;
      }
      DoorWindow doorWindow = mapObj as DoorWindow;
      bool flag = doorWindow != null && doorWindow.IsBarricaded;
      if (mapObj.BreakState != MapObject.Break.BREAKABLE && !flag)
      {
        reason = "can't break this object";
        return false;
      }
      if (mapObj.Location.Map.GetActorAt(mapObj.Location.Position) != null)
      {
        reason = "someone is there";
        return false;
      }
      reason = "";
      return true;
    }

    public bool HasActorPushAbility(Actor actor)
    {
      if (!actor.Model.Abilities.CanPush && actor.Sheet.SkillTable.GetSkillLevel(16) <= 0)
        return actor.Sheet.SkillTable.GetSkillLevel(26) > 0;
      return true;
    }

    public bool CanActorPush(Actor actor, MapObject mapObj)
    {
      string reason;
      return this.CanActorPush(actor, mapObj, out reason);
    }

    public bool CanActorPush(Actor actor, MapObject mapObj, out string reason)
    {
      if (actor == null)
        throw new ArgumentNullException(nameof (actor));
      if (mapObj == null)
        throw new ArgumentNullException(nameof (mapObj));
      if (!this.HasActorPushAbility(actor))
      {
        reason = "cannot push objects";
        return false;
      }
      if (this.IsActorTired(actor))
      {
        reason = "tired";
        return false;
      }
      if (!mapObj.IsMovable)
      {
        reason = "cannot be moved";
        return false;
      }
      if (mapObj.Location.Map.GetActorAt(mapObj.Location.Position) != null)
      {
        reason = "someone is there";
        return false;
      }
      if (mapObj.IsOnFire)
      {
        reason = "on fire";
        return false;
      }
      reason = "";
      return true;
    }

    public bool CanPushObjectTo(MapObject mapObj, Point toPos)
    {
      string reason;
      return this.CanPushObjectTo(mapObj, toPos, out reason);
    }

    public bool CanPushObjectTo(MapObject mapObj, Point toPos, out string reason)
    {
      if (mapObj == null)
        throw new ArgumentNullException(nameof (mapObj));
      if (!mapObj.Location.Map.IsInBounds(toPos))
      {
        reason = "out of map";
        return false;
      }
      if (!mapObj.Location.Map.GetTileAt(toPos.X, toPos.Y).Model.IsWalkable)
      {
        reason = "blocked by an obstacle";
        return false;
      }
      if (mapObj.Location.Map.GetMapObjectAt(toPos) != null)
      {
        reason = "blocked by an object";
        return false;
      }
      if (mapObj.Location.Map.GetActorAt(toPos) != null)
      {
        reason = "blocked by someone";
        return false;
      }
      reason = "";
      return true;
    }

    public bool CanActorShove(Actor actor, Actor other, out string reason)
    {
      if (actor == null)
        throw new ArgumentNullException(nameof (actor));
      if (other == null)
        throw new ArgumentNullException(nameof (other));
      if (!this.HasActorPushAbility(actor))
      {
        reason = "cannot shove people";
        return false;
      }
      if (this.IsActorTired(actor))
      {
        reason = "tired";
        return false;
      }
      reason = "";
      return true;
    }

    public bool CanShoveActorTo(Actor actor, Point toPos, out string reason)
    {
      if (actor == null)
        throw new ArgumentNullException(nameof (actor));
      Map map = actor.Location.Map;
      if (!map.IsInBounds(toPos))
      {
        reason = "out of map";
        return false;
      }
      if (!map.GetTileAt(toPos.X, toPos.Y).Model.IsWalkable)
      {
        reason = "blocked";
        return false;
      }
      MapObject mapObjectAt = map.GetMapObjectAt(toPos);
      if (mapObjectAt != null && !mapObjectAt.IsWalkable)
      {
        reason = "blocked by an object";
        return false;
      }
      if (map.GetActorAt(toPos) != null)
      {
        reason = "blocked by someone";
        return false;
      }
      reason = "";
      return true;
    }

    public List<Actor> GetEnemiesInFov(Actor actor, HashSet<Point> fov)
    {
      if (actor == null)
        throw new ArgumentNullException(nameof (actor));
      if (fov == null)
        throw new ArgumentNullException(nameof (fov));
      List<Actor> actorList = (List<Actor>) null;
      foreach (Point position in fov)
      {
        Actor actorAt = actor.Location.Map.GetActorAt(position);
        if (actorAt != null && actorAt != actor && this.IsEnemyOf(actor, actorAt))
        {
          if (actorList == null)
            actorList = new List<Actor>(3);
          actorList.Add(actorAt);
        }
      }
      actorList?.Sort((Comparison<Actor>) ((a, b) =>
      {
        Location location1 = a.Location;
        Point position1 = location1.Position;
        location1 = actor.Location;
        Point position2 = location1.Position;
        float num1 = this.StdDistance(position1, position2);
        Location location2 = b.Location;
        Point position3 = location2.Position;
        location2 = actor.Location;
        Point position4 = location2.Position;
        float num2 = this.StdDistance(position3, position4);
        if ((double) num1 < (double) num2)
          return -1;
        return (double) num1 <= (double) num2 ? 0 : 1;
      }));
      return actorList;
    }

    public bool CanActorFireAt(Actor actor, Actor target)
    {
      string reason;
      return this.CanActorFireAt(actor, target, out reason);
    }

    public bool CanActorFireAt(Actor actor, Actor target, out string reason)
    {
      return this.CanActorFireAt(actor, target, (List<Point>) null, out reason);
    }

    public bool CanActorFireAt(Actor actor, Actor target, List<Point> LoF)
    {
      string reason;
      return this.CanActorFireAt(actor, target, LoF, out reason);
    }

    public bool CanActorFireAt(Actor actor, Actor target, List<Point> LoF, out string reason)
    {
      if (actor == null)
        throw new ArgumentNullException(nameof (actor));
      if (target == null)
        throw new ArgumentNullException(nameof (target));
      LoF?.Clear();
      ItemRangedWeapon equippedWeapon = actor.GetEquippedWeapon() as ItemRangedWeapon;
      if (equippedWeapon == null)
      {
        reason = "no ranged weapon equipped";
        return false;
      }
      int range1 = actor.CurrentRangedAttack.Range;
      Location location1 = actor.Location;
      Point position1 = location1.Position;
      location1 = target.Location;
      Point position2 = location1.Position;
      int num = this.GridDistance(position1, position2);
      if (range1 < num)
      {
        reason = "out of range";
        return false;
      }
      if (equippedWeapon.Ammo <= 0)
      {
        reason = "no ammo left";
        return false;
      }
      Location location2 = actor.Location;
      location1 = target.Location;
      Point position3 = location1.Position;
      int range2 = actor.CurrentRangedAttack.Range;
      List<Point> line = LoF;
      if (!LOS.CanTraceFireLine(location2, position3, range2, line))
      {
        reason = "no line of fire";
        return false;
      }
      if (target.IsDead)
      {
        reason = "already dead!";
        return false;
      }
      reason = "";
      return true;
    }

    public bool CanActorThrowTo(Actor actor, Point pos, List<Point> LoF)
    {
      string reason;
      return this.CanActorThrowTo(actor, pos, LoF, out reason);
    }

    public bool CanActorThrowTo(Actor actor, Point pos, List<Point> LoF, out string reason)
    {
      if (actor == null)
        throw new ArgumentNullException(nameof (actor));
      LoF?.Clear();
      ItemGrenade equippedWeapon1 = actor.GetEquippedWeapon() as ItemGrenade;
      ItemGrenadePrimed equippedWeapon2 = actor.GetEquippedWeapon() as ItemGrenadePrimed;
      if (equippedWeapon1 == null && equippedWeapon2 == null)
      {
        reason = "no grenade equiped";
        return false;
      }
      ItemGrenadeModel itemGrenadeModel = equippedWeapon1 == null ? (equippedWeapon2.Model as ItemGrenadePrimedModel).GrenadeModel : equippedWeapon1.Model as ItemGrenadeModel;
      int maxRange = this.ActorMaxThrowRange(actor, itemGrenadeModel.MaxThrowDistance);
      if (this.GridDistance(actor.Location.Position, pos) > maxRange)
      {
        reason = "out of throwing range";
        return false;
      }
      if (!LOS.CanTraceThrowLine(actor.Location, pos, maxRange, LoF))
      {
        reason = "no line of throwing";
        return false;
      }
      reason = "";
      return true;
    }

    public bool IsActorHungry(Actor a)
    {
      if (a.Model.Abilities.HasToEat)
        return a.FoodPoints <= 720;
      return false;
    }

    public bool IsActorStarving(Actor a)
    {
      if (a.Model.Abilities.HasToEat)
        return a.FoodPoints <= 0;
      return false;
    }

    public bool IsRottingActorHungry(Actor a)
    {
      if (a.Model.Abilities.IsRotting)
        return a.FoodPoints <= 1440;
      return false;
    }

    public bool IsRottingActorStarving(Actor a)
    {
      if (a.Model.Abilities.IsRotting)
        return a.FoodPoints <= 0;
      return false;
    }

    public bool IsFoodStillFresh(ItemFood food, int turnCounter)
    {
      if (!food.IsPerishable)
        return true;
      return turnCounter < food.BestBefore.TurnCounter;
    }

    public bool IsFoodExpired(ItemFood food, int turnCounter)
    {
      if (food.IsPerishable && turnCounter >= food.BestBefore.TurnCounter)
        return turnCounter < 2 * food.BestBefore.TurnCounter;
      return false;
    }

    public bool IsFoodSpoiled(ItemFood food, int turnCounter)
    {
      if (food.IsPerishable)
        return turnCounter >= 2 * food.BestBefore.TurnCounter;
      return false;
    }

    public int FoodItemNutrition(ItemFood food, int turnCounter)
    {
      if (this.IsFoodStillFresh(food, turnCounter))
        return food.Nutrition;
      if (!this.IsFoodExpired(food, turnCounter))
        return food.Nutrition / 3;
      return 2 * food.Nutrition / 3;
    }

    public bool IsActorSleepy(Actor a)
    {
      if (a.Model.Abilities.HasToSleep)
        return a.SleepPoints <= 900;
      return false;
    }

    public bool IsActorExhausted(Actor a)
    {
      if (a.Model.Abilities.HasToSleep)
        return a.SleepPoints <= 0;
      return false;
    }

    public int SleepToHoursUntilSleepy(int sleep, bool isNight)
    {
      int num = sleep - 900;
      if (isNight)
        num /= 2;
      if (num <= 0)
        return 0;
      return num / 30;
    }

    public bool IsAlmostSleepy(Actor actor)
    {
      if (!actor.Model.Abilities.HasToSleep)
        return false;
      return this.SleepToHoursUntilSleepy(actor.SleepPoints, actor.Location.Map.LocalTime.IsNight) <= 3;
    }

    public bool CanActorSleep(Actor actor)
    {
      string reason;
      return this.CanActorSleep(actor, out reason);
    }

    public bool CanActorSleep(Actor actor, out string reason)
    {
      if (actor == null)
        throw new ArgumentNullException(nameof (actor));
      if (actor.IsSleeping)
      {
        reason = "already sleeping";
        return false;
      }
      if (!actor.Model.Abilities.HasToSleep)
      {
        reason = "no ability to sleep";
        return false;
      }
      if (this.IsActorHungry(actor) || this.IsActorStarving(actor))
      {
        reason = "hungry";
        return false;
      }
      if (actor.SleepPoints >= this.ActorMaxSleep(actor) - 30)
      {
        reason = "not sleepy at all";
        return false;
      }
      reason = "";
      return true;
    }

    public bool IsOnCouch(Actor actor)
    {
      if (actor == null)
        throw new ArgumentNullException(nameof (actor));
      MapObject mapObjectAt = actor.Location.Map.GetMapObjectAt(actor.Location.Position);
      if (mapObjectAt == null)
        return false;
      return mapObjectAt.IsCouch;
    }

    public bool IsActorDisturbed(Actor a)
    {
      if (a.Model.Abilities.HasSanity)
        return a.Sanity <= this.ActorDisturbedLevel(a);
      return false;
    }

    public bool IsActorInsane(Actor a)
    {
      if (a.Model.Abilities.HasSanity)
        return a.Sanity <= 0;
      return false;
    }

    public int SanityToHoursUntilUnstable(Actor a)
    {
      int num = a.Sanity - this.ActorDisturbedLevel(a);
      if (num <= 0)
        return 0;
      return num / 30;
    }

    public bool CanActorTakeLead(Actor actor, Actor target)
    {
      string reason;
      return this.CanActorTakeLead(actor, target, out reason);
    }

    public bool CanActorTakeLead(Actor actor, Actor target, out string reason)
    {
      if (actor == null)
        throw new ArgumentNullException(nameof (actor));
      if (target == null)
        throw new ArgumentNullException(nameof (target));
      if (target.Model.Abilities.IsUndead)
      {
        reason = "undead";
        return false;
      }
      if (this.IsEnemyOf(actor, target))
      {
        reason = "enemy";
        return false;
      }
      if (target.IsSleeping)
      {
        reason = "sleeping";
        return false;
      }
      if (target.HasLeader)
      {
        reason = "has already a leader";
        return false;
      }
      if (target.CountFollowers > 0)
      {
        reason = "is a leader";
        return false;
      }
      int num = this.ActorMaxFollowers(actor);
      if (num == 0)
      {
        reason = "can't lead";
        return false;
      }
      if (actor.CountFollowers >= num)
      {
        reason = "too many followers";
        return false;
      }
      if (target.IsPlayer)
      {
        reason = "is player";
        return false;
      }
      if (actor.Faction != target.Faction && target.Faction.LeadOnlyBySameFaction)
      {
        reason = string.Format("{0} can't lead {1}", (object) actor.Faction.Name, (object) target.Faction.Name);
        return false;
      }
      reason = "";
      return true;
    }

    public bool CanActorCancelLead(Actor actor, Actor target, out string reason)
    {
      if (actor == null)
        throw new ArgumentNullException(nameof (actor));
      if (target == null)
        throw new ArgumentNullException(nameof (target));
      if (target.Leader != actor)
      {
        reason = "not your follower";
        return false;
      }
      if (target.IsSleeping)
      {
        reason = "sleeping";
        return false;
      }
      reason = "";
      return true;
    }

    public bool CanActorSwitchPlaceWith(Actor actor, Actor target)
    {
      string reason;
      return this.CanActorSwitchPlaceWith(actor, target, out reason);
    }

    public bool CanActorSwitchPlaceWith(Actor actor, Actor target, out string reason)
    {
      if (actor == null)
        throw new ArgumentNullException(nameof (actor));
      if (target == null)
        throw new ArgumentNullException(nameof (target));
      if (target.Leader != actor)
      {
        reason = "not your follower";
        return false;
      }
      if (target.IsSleeping)
      {
        reason = "sleeping";
        return false;
      }
      reason = "";
      return true;
    }

    public bool IsActorTrustingLeader(Actor actor)
    {
      if (actor == null)
        throw new ArgumentNullException(nameof (actor));
      if (!actor.HasLeader)
        return false;
      return actor.TrustInLeader >= 360;
    }

    public bool HasActorBondWith(Actor actor, Actor target)
    {
      if (actor.Leader == target)
        return actor.TrustInLeader >= 1440;
      if (target.Leader == actor)
        return target.TrustInLeader >= 1440;
      return false;
    }

    public int CountBarricadingMaterial(Actor actor)
    {
      if (actor.Inventory == null || actor.Inventory.IsEmpty)
        return 0;
      int num = 0;
      foreach (Item obj in actor.Inventory.Items)
      {
        if (obj is ItemBarricadeMaterial)
          num += obj.Quantity;
      }
      return num;
    }

    public bool CanActorBuildFortification(Actor actor, Point pos, bool isLarge)
    {
      string reason;
      return this.CanActorBuildFortification(actor, pos, isLarge, out reason);
    }

    public bool CanActorBuildFortification(Actor actor, Point pos, bool isLarge, out string reason)
    {
      if (actor == null)
        throw new ArgumentNullException(nameof (actor));
      if (actor.Sheet.SkillTable.GetSkillLevel(3) == 0)
      {
        reason = "no skill in carpentry";
        return false;
      }
      Map map = actor.Location.Map;
      if (!map.GetTileAt(pos).Model.IsWalkable)
      {
        reason = "cannot build on walls";
        return false;
      }
      int num = this.ActorBarricadingMaterialNeedForFortification(actor, isLarge);
      if (this.CountBarricadingMaterial(actor) < num)
      {
        reason = string.Format("not enough barricading material, need {0}.", (object) num);
        return false;
      }
      if (map.GetMapObjectAt(pos) != null || map.GetActorAt(pos) != null)
      {
        reason = "blocked";
        return false;
      }
      reason = "";
      return true;
    }

    public bool CanActorRepairFortification(Actor actor, Fortification fort, out string reason)
    {
      if (actor == null)
        throw new ArgumentNullException(nameof (actor));
      if (!actor.Model.Abilities.CanUseMapObjects)
      {
        reason = "cannot use map objects";
        return false;
      }
      if (this.CountBarricadingMaterial(actor) <= 0)
      {
        reason = "no barricading material";
        return false;
      }
      reason = "";
      return true;
    }

    public int ActorDamageVsCorpses(Actor a)
    {
      return a.CurrentMeleeAttack.DamageValue / 2 + Rules.SKILL_NECROLOGY_CORPSE_BONUS * a.Sheet.SkillTable.GetSkillLevel(15);
    }

    public bool CanActorEatCorpse(Actor actor, Corpse corpse)
    {
      string reason;
      return this.CanActorEatCorpse(actor, corpse, out reason);
    }

    public bool CanActorEatCorpse(Actor actor, Corpse corpse, out string reason)
    {
      if (actor == null)
        throw new ArgumentNullException(nameof (actor));
      if (corpse == null)
        throw new ArgumentNullException(nameof (corpse));
      if (!actor.Model.Abilities.IsUndead && !this.IsActorStarving(actor) && !this.IsActorInsane(actor))
      {
        reason = "not starving or insane";
        return false;
      }
      reason = "";
      return true;
    }

    public bool CanActorButcherCorpse(Actor actor, Corpse corpse)
    {
      string reason;
      return this.CanActorButcherCorpse(actor, corpse, out reason);
    }

    public bool CanActorButcherCorpse(Actor actor, Corpse corpse, out string reason)
    {
      if (actor == null)
        throw new ArgumentNullException(nameof (actor));
      if (corpse == null)
        throw new ArgumentNullException(nameof (corpse));
      if (this.IsActorTired(actor))
      {
        reason = "tired";
        return false;
      }
      if (corpse.Position != actor.Location.Position || !actor.Location.Map.HasCorpse(corpse))
      {
        reason = "not in same location";
        return false;
      }
      reason = "";
      return true;
    }

    public bool CanActorStartDragCorpse(Actor actor, Corpse corpse)
    {
      string reason;
      return this.CanActorStartDragCorpse(actor, corpse, out reason);
    }

    public bool CanActorStartDragCorpse(Actor actor, Corpse corpse, out string reason)
    {
      if (actor == null)
        throw new ArgumentNullException(nameof (actor));
      if (corpse == null)
        throw new ArgumentNullException(nameof (corpse));
      if (corpse.IsDragged)
      {
        reason = "corpse is already being dragged";
        return false;
      }
      if (this.IsActorTired(actor))
      {
        reason = "tired";
        return false;
      }
      if (corpse.Position != actor.Location.Position || !actor.Location.Map.HasCorpse(corpse))
      {
        reason = "not in same location";
        return false;
      }
      if (actor.DraggedCorpse != null)
      {
        reason = "already dragging a corpse";
        return false;
      }
      reason = "";
      return true;
    }

    public bool CanActorStopDragCorpse(Actor actor, Corpse corpse)
    {
      string reason;
      return this.CanActorStopDragCorpse(actor, corpse, out reason);
    }

    public bool CanActorStopDragCorpse(Actor actor, Corpse corpse, out string reason)
    {
      if (actor == null)
        throw new ArgumentNullException(nameof (actor));
      if (corpse == null)
        throw new ArgumentNullException(nameof (corpse));
      if (corpse.DraggedBy != actor)
      {
        reason = "not dragging this corpse";
        return false;
      }
      reason = "";
      return true;
    }

    public bool CanActorReviveCorpse(Actor actor, Corpse corpse)
    {
      string reason;
      return this.CanActorReviveCorpse(actor, corpse, out reason);
    }

    public bool CanActorReviveCorpse(Actor actor, Corpse corpse, out string reason)
    {
      if (actor == null)
        throw new ArgumentNullException(nameof (actor));
      if (corpse == null)
        throw new ArgumentNullException(nameof (corpse));
      if (actor.Sheet.SkillTable.GetSkillLevel(14) == 0)
      {
        reason = "lack medic skill";
        return false;
      }
      if (corpse.Position != actor.Location.Position)
      {
        reason = "not there";
        return false;
      }
      if (this.CorpseRotLevel(corpse) > 0)
      {
        reason = "corpse not fresh";
        return false;
      }
      if (corpse.IsDragged)
      {
        reason = "dragged corpse";
        return false;
      }
      if (!actor.Inventory.HasItemMatching((Predicate<Item>) (it => it.Model.ID == 1)))
      {
        reason = "no medikit";
        return false;
      }
      reason = "";
      return true;
    }

    public bool IsAdjacent(Location a, Location b)
    {
      if (a.Map != b.Map)
        return false;
      return this.IsAdjacent(a.Position, b.Position);
    }

    public bool IsAdjacent(Point pA, Point pB)
    {
      if (Math.Abs(pA.X - pB.X) < 2)
        return Math.Abs(pA.Y - pB.Y) < 2;
      return false;
    }

    public int GridDistance(Point pA, int bX, int bY)
    {
      return Math.Max(Math.Abs(pA.X - bX), Math.Abs(pA.Y - bY));
    }

    public int GridDistance(Point pA, Point pB)
    {
      return Math.Max(Math.Abs(pA.X - pB.X), Math.Abs(pA.Y - pB.Y));
    }

    public float StdDistance(Point from, Point to)
    {
      int num1 = to.X - from.X;
      int num2 = to.Y - from.Y;
      return (float) Math.Sqrt((double) (num1 * num1 + num2 * num2));
    }

    public float StdDistance(Point v)
    {
      return (float) Math.Sqrt((double) (v.X * v.X + v.Y * v.Y));
    }

    public float LOSDistance(Point from, Point to)
    {
      int num1 = to.X - from.X;
      int num2 = to.Y - from.Y;
      return (float) Math.Sqrt(0.75 * (double) (num1 * num1 + num2 * num2));
    }

    public Actor GetNextActorToAct(Map map, int turnCounter)
    {
      if (map == null)
        return (Actor) null;
      int countActors = map.CountActors;
      for (int checkNextActorIndex = map.CheckNextActorIndex; checkNextActorIndex < countActors; ++checkNextActorIndex)
      {
        Actor actor = map.GetActor(checkNextActorIndex);
        if (actor.ActionPoints > 0 && !actor.IsSleeping)
        {
          map.CheckNextActorIndex = checkNextActorIndex;
          return actor;
        }
      }
      return (Actor) null;
    }

    public bool IsActorBeforeInMapList(Map map, Actor actor, Actor other)
    {
      foreach (Actor actor1 in map.Actors)
      {
        if (actor1 == actor)
          return true;
        if (actor1 == other)
          return false;
      }
      return true;
    }

    public bool CanActorActThisTurn(Actor actor)
    {
      if (actor == null)
        return false;
      return actor.ActionPoints > 0;
    }

    public bool CanActorActNextTurn(Actor actor)
    {
      if (actor == null)
        return false;
      return actor.ActionPoints + this.ActorSpeed(actor) > 0;
    }

    public bool WillActorActAgainBefore(Actor actor, Actor other)
    {
      return other.ActionPoints <= 0 && (other.ActionPoints + this.ActorSpeed(other) <= 0 || !this.IsActorBeforeInMapList(actor.Location.Map, other, actor));
    }

    public bool WillOtherActTwiceBefore(Actor actor, Actor other)
    {
      if (this.IsActorBeforeInMapList(actor.Location.Map, actor, other))
        return other.ActionPoints > 100;
      return other.ActionPoints + this.ActorSpeed(other) > 100;
    }

    public bool IsEnemyOf(Actor actor, Actor target)
    {
      return actor != null && target != null && (actor.Faction.IsEnemyOf(target.Faction) || actor.Faction == target.Faction && actor.IsInAGang && (target.IsInAGang && actor.GangID != target.GangID) || (actor.AreDirectEnemies(target) || actor.AreIndirectEnemies(target)));
    }

    public bool IsMurder(Actor killer, Actor victim)
    {
      if (killer == null || victim == null || victim.Model.Abilities.IsUndead || (killer.Model.Abilities.IsLawEnforcer && victim.MurdersCounter > 0 || (killer.Faction.IsEnemyOf(victim.Faction) || killer.IsSelfDefenceFrom(victim))) || killer.HasLeader && killer.Leader.IsSelfDefenceFrom(victim))
        return false;
      if (killer.CountFollowers > 0)
      {
        foreach (Actor follower in killer.Followers)
        {
          if (follower.IsSelfDefenceFrom(victim))
            return false;
        }
      }
      return true;
    }

    public int ActorSpeed(Actor actor)
    {
      float speed = (float) actor.Doll.Body.Speed;
      if (this.IsActorTired(actor))
        speed *= 0.6666667f;
      if (this.IsActorExhausted(actor))
        speed /= 2f;
      else if (this.IsActorSleepy(actor))
        speed *= 0.6666667f;
      ItemBodyArmor equippedItem = actor.GetEquippedItem(DollPart.TORSO) as ItemBodyArmor;
      if (equippedItem != null)
        speed -= (float) equippedItem.Weight;
      if (actor.DraggedCorpse != null)
        speed /= 2f;
      return Math.Max((int) speed, 0);
    }

    public int ActorMaxHPs(Actor actor)
    {
      int num = Rules.SKILL_TOUGH_HP_BONUS * actor.Sheet.SkillTable.GetSkillLevel(18) + Rules.SKILL_ZTOUGH_HP_BONUS * actor.Sheet.SkillTable.GetSkillLevel(27);
      return actor.Sheet.BaseHitPoints + num;
    }

    public int ActorMaxSTA(Actor actor)
    {
      int num = Rules.SKILL_HIGH_STAMINA_STA_BONUS * actor.Sheet.SkillTable.GetSkillLevel(8);
      return actor.Sheet.BaseStaminaPoints + num;
    }

    public int ActorItemNutritionValue(Actor actor, int baseValue)
    {
      int num = (int) ((double) baseValue * (double) Rules.SKILL_LIGHT_EATER_FOOD_BONUS * (double) actor.Sheet.SkillTable.GetSkillLevel(10));
      return baseValue + num;
    }

    public int ActorSanRegenValue(Actor actor, int baseValue)
    {
      int num = (int) ((double) baseValue * (double) Rules.SKILL_STRONG_PSYCHE_ENT_BONUS * (double) actor.Sheet.SkillTable.GetSkillLevel(17));
      return baseValue + num;
    }

    public int ActorMaxFood(Actor actor)
    {
      int num = (int) ((double) actor.Sheet.BaseFoodPoints * (double) Rules.SKILL_LIGHT_EATER_MAXFOOD_BONUS * (double) actor.Sheet.SkillTable.GetSkillLevel(10));
      return actor.Sheet.BaseFoodPoints + num;
    }

    public int ActorMaxRot(Actor actor)
    {
      int num = (int) ((double) actor.Sheet.BaseFoodPoints * (double) Rules.SKILL_ZLIGHT_EATER_MAXFOOD_BONUS * (double) actor.Sheet.SkillTable.GetSkillLevel(24));
      return actor.Sheet.BaseFoodPoints + num;
    }

    public int ActorMaxSleep(Actor actor)
    {
      int num = (int) ((double) actor.Sheet.BaseSleepPoints * (double) Rules.SKILL_AWAKE_SLEEP_BONUS * (double) actor.Sheet.SkillTable.GetSkillLevel(1));
      return actor.Sheet.BaseSleepPoints + num;
    }

    public int ActorSleepRegen(Actor actor, bool isOnCouch)
    {
      int num = isOnCouch ? 6 : 4;
      return num + (int) ((double) num * (double) Rules.SKILL_AWAKE_SLEEP_REGEN_BONUS * (double) actor.Sheet.SkillTable.GetSkillLevel(1));
    }

    public int ActorMaxSanity(Actor actor)
    {
      return actor.Sheet.BaseSanity;
    }

    public int ActorDisturbedLevel(Actor actor)
    {
      return (int) (1440.0 * (1.0 - (double) Rules.SKILL_STRONG_PSYCHE_LEVEL_BONUS * (double) actor.Sheet.SkillTable.GetSkillLevel(17)));
    }

    public int ActorMaxInv(Actor actor)
    {
      int num = Rules.SKILL_HAULER_INV_BONUS * actor.Sheet.SkillTable.GetSkillLevel(7);
      return actor.Sheet.BaseInventoryCapacity + num;
    }

    public int ActorDamageBonusVsUndeads(Actor actor)
    {
      return Rules.SKILL_NECROLOGY_UNDEAD_BONUS * actor.Sheet.SkillTable.GetSkillLevel(15);
    }

    public Attack ActorMeleeAttack(Actor actor, Attack baseAttack, Actor target)
    {
      float hitValue = (float) baseAttack.HitValue;
      float damageValue = (float) baseAttack.DamageValue;
      int num1 = Rules.SKILL_AGILE_ATK_BONUS * actor.Sheet.SkillTable.GetSkillLevel(0) + Rules.SKILL_ZAGILE_ATK_BONUS * actor.Sheet.SkillTable.GetSkillLevel(20);
      int num2 = Rules.SKILL_STRONG_DMG_BONUS * actor.Sheet.SkillTable.GetSkillLevel(16) + Rules.SKILL_ZSTRONG_DMG_BONUS * actor.Sheet.SkillTable.GetSkillLevel(26);
      if (actor.GetEquippedWeapon() == null)
      {
        num1 += Rules.SKILL_MARTIAL_ARTS_ATK_BONUS * actor.Sheet.SkillTable.GetSkillLevel(13);
        num2 += Rules.SKILL_MARTIAL_ARTS_DMG_BONUS * actor.Sheet.SkillTable.GetSkillLevel(13);
      }
      if (target != null && target.Model.Abilities.IsUndead)
        num2 += this.ActorDamageBonusVsUndeads(actor);
      float num3 = hitValue + (float) num1;
      float num4 = damageValue + (float) num2;
      if (this.IsActorExhausted(actor))
        num3 /= 2f;
      else if (this.IsActorSleepy(actor))
        num3 *= 0.75f;
      return new Attack(baseAttack.Kind, baseAttack.Verb, (int) num3, (int) num4, baseAttack.StaminaPenalty);
    }

    public Attack ActorRangedAttack(Actor actor, Attack baseAttack, int distance, Actor target)
    {
      int num1 = 0;
      int num2 = 0;
      switch (baseAttack.Kind)
      {
        case AttackKind.FIREARM:
          num1 = Rules.SKILL_FIREARMS_ATK_BONUS * actor.Sheet.SkillTable.GetSkillLevel(5);
          num2 = Rules.SKILL_FIREARMS_DMG_BONUS * actor.Sheet.SkillTable.GetSkillLevel(5);
          break;
        case AttackKind.BOW:
          num1 = Rules.SKILL_BOWS_ATK_BONUS * actor.Sheet.SkillTable.GetSkillLevel(2);
          num2 = Rules.SKILL_BOWS_DMG_BONUS * actor.Sheet.SkillTable.GetSkillLevel(2);
          break;
      }
      if (target != null && target.Model.Abilities.IsUndead)
        num2 += this.ActorDamageBonusVsUndeads(actor);
      int efficientRange = baseAttack.EfficientRange;
      if (distance != efficientRange)
      {
        int num3 = efficientRange - distance;
        num1 += num3 * 2;
      }
      float num4 = (float) (baseAttack.HitValue + num1);
      float num5 = (float) (baseAttack.DamageValue + num2);
      if (this.IsActorExhausted(actor))
        num4 /= 2f;
      else if (this.IsActorSleepy(actor))
        num4 *= 0.75f;
      if (this.IsActorTired(actor))
        num4 *= 0.75f;
      else if (actor.StaminaPoints < this.ActorMaxSTA(actor))
        num4 *= 0.9f;
      return new Attack(baseAttack.Kind, baseAttack.Verb, (int) num4, (int) num5, baseAttack.StaminaPenalty, baseAttack.Range);
    }

    public int ActorMaxThrowRange(Actor actor, int baseRange)
    {
      int num = Rules.SKILL_STRONG_THROW_BONUS * actor.Sheet.SkillTable.GetSkillLevel(16);
      return baseRange + num;
    }

    public Defence ActorDefence(Actor actor, Defence baseDefence)
    {
      if (actor.IsSleeping)
        return new Defence(0, 0, 0);
      int num1 = Rules.SKILL_AGILE_DEF_BONUS * actor.Sheet.SkillTable.GetSkillLevel(0) + Rules.SKILL_ZAGILE_DEF_BONUS * actor.Sheet.SkillTable.GetSkillLevel(20);
      float num2 = (float) (baseDefence.Value + num1);
      if (this.IsActorExhausted(actor))
        num2 /= 2f;
      else if (this.IsActorSleepy(actor))
        num2 *= 0.75f;
      return new Defence((int) num2, baseDefence.Protection_Hit, baseDefence.Protection_Shot);
    }

    public int ActorMedicineEffect(Actor actor, int baseEffect)
    {
      int num = (int) Math.Ceiling((double) Rules.SKILL_MEDIC_BONUS * (double) actor.Sheet.SkillTable.GetSkillLevel(14) * (double) baseEffect);
      return baseEffect + num;
    }

    public int ActorHealChanceBonus(Actor actor)
    {
      return Rules.SKILL_HARDY_HEAL_CHANCE_BONUS * actor.Sheet.SkillTable.GetSkillLevel(6);
    }

    public int ActorBarricadingPoints(Actor actor, int baseBarricadingPoints)
    {
      int num = (int) ((double) baseBarricadingPoints * (double) Rules.SKILL_CARPENTRY_BARRICADING_BONUS * (double) actor.Sheet.SkillTable.GetSkillLevel(3));
      return baseBarricadingPoints + num;
    }

    public int ActorMaxFollowers(Actor actor)
    {
      return Rules.SKILL_LEADERSHIP_FOLLOWER_BONUS * actor.Sheet.SkillTable.GetSkillLevel(9);
    }

    public int ActorFOV(Actor actor, WorldTime time, Weather weather)
    {
      if (actor.IsSleeping)
        return 0;
      int val2 = actor.Sheet.BaseViewRange;
      Lighting lighting = actor.Location.Map.Lighting;
      switch (lighting)
      {
        case Lighting._FIRST:
          val2 = this.DarknessFov(actor);
          goto case Lighting.LIT;
        case Lighting.OUTSIDE:
          val2 = val2 - this.NightFovPenalty(actor, time) - this.WeatherFovPenalty(actor, weather);
          goto case Lighting.LIT;
        case Lighting.LIT:
          if (this.IsActorExhausted(actor))
            val2 -= 2;
          else if (this.IsActorSleepy(actor))
            --val2;
          Location location;
          switch (lighting)
          {
            case Lighting._FIRST:
              int num = this.GetLightBonusEquipped(actor);
              if (num == 0)
              {
                Map map = actor.Location.Map;
                Map map1 = map;
                location = actor.Location;
                Point position = location.Position;
                Predicate<Point> predicateFn = (Predicate<Point>) (pt =>
                {
                  Actor actorAt = map.GetActorAt(pt);
                  if (actorAt == null)
                    return false;
                  return this.HasLightOnEquipped(actorAt);
                });
                if (map1.HasAnyAdjacentInMap(position, predicateFn))
                  num = 1;
              }
              val2 += num;
              break;
            case Lighting.OUTSIDE:
              if (!time.IsNight)
                break;
              goto case Lighting._FIRST;
          }
          location = actor.Location;
          Map map2 = location.Map;
          location = actor.Location;
          Point position1 = location.Position;
          MapObject mapObjectAt = map2.GetMapObjectAt(position1);
          if (mapObjectAt != null && mapObjectAt.StandOnFovBonus)
            ++val2;
          return Math.Max(2, val2);
        default:
          throw new ArgumentOutOfRangeException("unhandled lighting");
      }
    }

    public float ActorSmell(Actor actor)
    {
      return (float) (1.0 + (double) Rules.SKILL_ZTRACKER_SMELL_BONUS * (double) actor.Sheet.SkillTable.GetSkillLevel(28)) * actor.Model.StartingSheet.BaseSmellRating;
    }

    public int ActorSmellThreshold(Actor actor)
    {
      if (actor.IsSleeping)
        return -1;
      return 271 - (int) ((double) this.ActorSmell(actor) * 270.0);
    }

    private bool HasLightOnEquipped(Actor actor)
    {
      ItemLight equippedItem = actor.GetEquippedItem(DollPart.LEFT_HAND) as ItemLight;
      if (equippedItem != null)
        return equippedItem.Batteries > 0;
      return false;
    }

    private int GetLightBonusEquipped(Actor actor)
    {
      ItemLight equippedItem = actor.GetEquippedItem(DollPart.LEFT_HAND) as ItemLight;
      if (equippedItem != null && equippedItem.Batteries > 0)
        return equippedItem.FovBonus;
      return 0;
    }

    public int ActorLoudNoiseWakeupChance(Actor actor, int noiseDistance)
    {
      int num1 = 10;
      int num2 = Rules.SKILL_LIGHT_SLEEPER_WAKEUP_CHANCE_BONUS * actor.Sheet.SkillTable.GetSkillLevel(12);
      int num3 = Math.Max(0, (5 - noiseDistance) * 10);
      int num4 = num2;
      return num1 + num4 + num3;
    }

    public int ActorBarricadingMaterialNeedForFortification(Actor builder, bool isLarge)
    {
      return Math.Max(1, (isLarge ? 4 : 2) - (builder.Sheet.SkillTable.GetSkillLevel(3) >= 3 ? Rules.SKILL_CARPENTRY_LEVEL3_BUILD_BONUS : 0));
    }

    public int ActorTrustIncrease(Actor actor)
    {
      return 1 + Rules.SKILL_CHARISMATIC_TRUST_BONUS * actor.Sheet.SkillTable.GetSkillLevel(4);
    }

    public int ActorCharismaticTradeChance(Actor actor)
    {
      return Rules.SKILL_CHARISMATIC_TRADE_BONUS * actor.Sheet.SkillTable.GetSkillLevel(4);
    }

    public int ActorUnsuspicousChance(Actor observer, Actor actor)
    {
      int num1 = Rules.SKILL_UNSUSPICIOUS_BONUS * actor.Sheet.SkillTable.GetSkillLevel(19);
      int num2 = 0;
      ItemBodyArmor equippedItem = actor.GetEquippedItem(DollPart.TORSO) as ItemBodyArmor;
      if (equippedItem != null)
      {
        if (observer.Faction.ID == 6)
        {
          if (equippedItem.IsHostileForCops())
            num2 -= Rules.UNSUSPICIOUS_BAD_OUTFIT_PENALTY;
          else if (equippedItem.IsFriendlyForCops())
            num2 += Rules.UNSUSPICIOUS_GOOD_OUTFIT_BONUS;
        }
        else if (observer.Faction.ID == 4)
        {
          if (equippedItem.IsHostileForBiker((GameGangs.IDs) observer.GangID))
            num2 -= Rules.UNSUSPICIOUS_BAD_OUTFIT_PENALTY;
          else if (equippedItem.IsFriendlyForBiker((GameGangs.IDs) observer.GangID))
            num2 += Rules.UNSUSPICIOUS_GOOD_OUTFIT_BONUS;
        }
      }
      int num3 = num2;
      return num1 + num3;
    }

    public int ActorSpotMurdererChance(Actor spotter, Actor murderer)
    {
      int num1 = 5 * murderer.MurdersCounter;
      Location location = spotter.Location;
      Point position1 = location.Position;
      location = murderer.Location;
      Point position2 = location.Position;
      int num2 = this.GridDistance(position1, position2);
      return 5 + num1 - num2;
    }

    private int NightFovPenalty(Actor actor, WorldTime time)
    {
      if (actor.Model.Abilities.IsUndead)
        return 0;
      switch (time.Phase)
      {
        case DayPhase.SUNSET:
          return 1;
        case DayPhase.EVENING:
          return 2;
        case DayPhase.MIDNIGHT:
          return 3;
        case DayPhase.DEEP_NIGHT:
          return 4;
        case DayPhase.SUNRISE:
          return 2;
        default:
          return 0;
      }
    }

    public int NightStaminaPenalty(Actor actor)
    {
      return actor.Model.Abilities.IsUndead ? 0 : 2;
    }

    private int WeatherFovPenalty(Actor actor, Weather weather)
    {
      if (actor.Model.Abilities.IsUndead)
        return 0;
      if (weather == Weather.RAIN)
        return 1;
      return weather == Weather.HEAVY_RAIN ? 2 : 0;
    }

    public bool IsWeatherRain(Weather weather)
    {
      switch (weather)
      {
        case Weather._FIRST:
        case Weather.CLOUDY:
          return false;
        case Weather.RAIN:
        case Weather.HEAVY_RAIN:
          return true;
        default:
          throw new ArgumentOutOfRangeException("unhandled weather");
      }
    }

    private int DarknessFov(Actor actor)
    {
      if (actor.Model.Abilities.IsUndead)
        return actor.Sheet.BaseViewRange;
      return 2;
    }

    public float ComputeMapPowerRatio(Map map)
    {
      int num1;
      int num2 = num1 = 0;
      foreach (MapObject mapObject in map.MapObjects)
      {
        PowerGenerator powerGenerator = mapObject as PowerGenerator;
        if (powerGenerator != null)
        {
          if (powerGenerator.IsOn)
            ++num1;
          else
            ++num2;
        }
      }
      int num3 = num1 + num2;
      if (num3 == 0)
        return 0.0f;
      return (float) num1 / (float) num3;
    }

    public int BlastDamage(int distance, BlastAttack attack)
    {
      if (distance < 0 || distance > attack.Radius)
        throw new ArgumentOutOfRangeException(string.Format("blast distance {0} out of range", (object) distance));
      return attack.Damage[distance];
    }

    public int ActorBiteHpRegen(Actor a, int dmg)
    {
      int num = (int) ((double) Rules.SKILL_ZEATER_REGEN_BONUS * (double) a.Sheet.SkillTable.GetSkillLevel(21) * (double) dmg);
      return dmg + num;
    }

    public int ActorBiteNutritionValue(Actor actor, int baseValue)
    {
      return (int) (10.0 + (double) (Rules.SKILL_ZLIGHT_EATER_FOOD_BONUS * (float) actor.Sheet.SkillTable.GetSkillLevel(24)) + (double) (Rules.SKILL_LIGHT_EATER_FOOD_BONUS * (float) actor.Sheet.SkillTable.GetSkillLevel(10))) * baseValue;
    }

    public int CorpseEeatingInfectionTransmission(int infection)
    {
      return (int) (0.100000001490116 * (double) infection);
    }

    public int ActorInfectionHPs(Actor a)
    {
      return this.ActorMaxHPs(a) + this.ActorMaxSTA(a);
    }

    public static int InfectionForDamage(Actor infector, int dmg)
    {
      return (int) ((1.0 + (double) infector.Sheet.SkillTable.GetSkillLevel(23) * (double) Rules.SKILL_ZINFECTOR_BONUS) * (double) dmg);
    }

    public int ActorInfectionPercent(Actor a)
    {
      return 100 * a.Infection / this.ActorInfectionHPs(a);
    }

    public int InfectionEffectTriggerChance1000(int infectionPercent)
    {
      return Rules.INFECTION_EFFECT_TRIGGER_CHANCE_1000 + infectionPercent / 5;
    }

    public int CorpseFreshnessPercent(Corpse c)
    {
      return (int) (100.0 * (double) c.HitPoints / (double) this.ActorMaxHPs(c.DeadGuy));
    }

    public int CorpseRotLevel(Corpse c)
    {
      int num = this.CorpseFreshnessPercent(c);
      if (num < 5)
        return 5;
      if (num < 25)
        return 4;
      if (num < 50)
        return 3;
      if (num < 75)
        return 2;
      return num < 90 ? 1 : 0;
    }

    public static float CorpseDecayPerTurn(Corpse c)
    {
      return 0.005555556f;
    }

    public int CorpseZombifyChance(Corpse c, WorldTime timeNow, bool checkDelay = true)
    {
      int num1 = timeNow.TurnCounter - c.Turn;
      if (checkDelay && num1 < 180)
        return 0;
      int num2 = this.ActorInfectionPercent(c.DeadGuy);
      if (checkDelay)
      {
        int num3 = num2 >= 100 ? 1 : 100 / (1 + num2);
        if (timeNow.TurnCounter % num3 != 0)
          return 0;
      }
      float num4 = 0.0f + 1f * (float) num2 - (float) (int) (1.0 / 720.0 * (double) num1);
      return Math.Max(0, Math.Min(100, !timeNow.IsNight ? (int) (num4 * 0.01f) : (int) (num4 * 2f)));
    }

    public int CorpseReviveChance(Actor actor, Corpse corpse)
    {
      if (!this.CanActorReviveCorpse(actor, corpse))
        return 0;
      return this.CorpseFreshnessPercent(corpse) / 2 + actor.Sheet.SkillTable.GetSkillLevel(14) * Rules.SKILL_MEDIC_REVIVE_BONUS;
    }

    public int CorpseReviveHPs(Actor actor, Corpse corpse)
    {
      return 5 + actor.Sheet.SkillTable.GetSkillLevel(14);
    }

    public bool CheckTrapTriggers(ItemTrap trap, Actor a)
    {
      if (a.Model.Abilities.IsSmall && this.RollChance(90))
        return false;
      int num = 0 + (a.Sheet.SkillTable.GetSkillLevel(11) * Rules.SKILL_LIGHT_FEET_TRAP_BONUS + a.Sheet.SkillTable.GetSkillLevel(25) * Rules.SKILL_ZLIGHT_FEET_TRAP_BONUS);
      return this.RollChance(trap.TrapModel.TriggerChance * trap.Quantity - num);
    }

    public bool CheckTrapTriggers(ItemTrap trap, MapObject mobj)
    {
      return this.RollChance(trap.TrapModel.TriggerChance * mobj.Weight);
    }

    public bool CheckTrapStepOnBreaks(ItemTrap trap, MapObject mobj = null)
    {
      int breakChance = trap.TrapModel.BreakChance;
      if (mobj != null)
        breakChance *= mobj.Weight;
      return this.RollChance(breakChance);
    }

    public bool CheckTrapEscapeBreaks(ItemTrap trap, Actor a)
    {
      return this.RollChance(trap.TrapModel.BreakChanceWhenEscape);
    }

    public bool CheckTrapEscape(ItemTrap trap, Actor a)
    {
      return this.RollChance(0 + (a.Sheet.SkillTable.GetSkillLevel(11) * Rules.SKILL_LIGHT_FEET_TRAP_BONUS + a.Sheet.SkillTable.GetSkillLevel(25) * Rules.SKILL_ZLIGHT_FEET_TRAP_BONUS) + (100 - trap.TrapModel.BlockChance * trap.Quantity));
    }

    public bool IsTrapCoveringMapObjectThere(Map map, Point pos)
    {
      MapObject mapObjectAt = map.GetMapObjectAt(pos);
      if (mapObjectAt == null)
        return false;
      if (mapObjectAt.IsJumpable)
        return true;
      if (mapObjectAt.IsWalkable)
        return !(mapObjectAt is DoorWindow);
      return false;
    }

    public bool IsTrapTriggeringMapObjectThere(Map map, Point pos)
    {
      MapObject mapObjectAt = map.GetMapObjectAt(pos);
      if (mapObjectAt == null || mapObjectAt.IsWalkable || mapObjectAt.IsJumpable)
        return false;
      return !(mapObjectAt is DoorWindow);
    }

    public int ZGrabChance(Actor grabber, Actor victim)
    {
      return grabber.Sheet.SkillTable.GetSkillLevel(22) * Rules.SKILL_ZGRAB_CHANCE;
    }

    public static bool HasImmediateZombification(GameMode mode)
    {
      return mode == GameMode.GM_STANDARD;
    }

    public static bool HasInfection(GameMode mode)
    {
      return (uint) mode > 0U;
    }

    public static bool HasCorpses(GameMode mode)
    {
      return (uint) mode > 0U;
    }

    public static bool HasEvolution(GameMode mode)
    {
      return mode != GameMode.GM_VINTAGE;
    }

    public static bool HasAllZombies(GameMode mode)
    {
      return mode != GameMode.GM_VINTAGE;
    }

    public static bool HasZombiesInBasements(GameMode mode)
    {
      return mode != GameMode.GM_VINTAGE;
    }

    public static bool HasZombiesInSewers(GameMode mode)
    {
      return mode != GameMode.GM_VINTAGE;
    }
  }
}
