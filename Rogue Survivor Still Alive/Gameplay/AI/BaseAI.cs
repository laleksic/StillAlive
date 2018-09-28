﻿// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Gameplay.AI.BaseAI
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Data;
using djack.RogueSurvivor.Engine;
using djack.RogueSurvivor.Engine.Actions;
using djack.RogueSurvivor.Engine.AI;
using djack.RogueSurvivor.Engine.Items;
using djack.RogueSurvivor.Engine.MapObjects;
using djack.RogueSurvivor.Gameplay.AI.Sensors;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace djack.RogueSurvivor.Gameplay.AI
{
  [Serializable]
  internal abstract class BaseAI : AIController
  {
    private const int FLEE_THROUGH_EXIT_CHANCE = 50;
    private const int EMOTE_GRAB_ITEM_CHANCE = 30;
    private const int EMOTE_FLEE_CHANCE = 30;
    private const int EMOTE_FLEE_TRAPPED_CHANCE = 50;
    private const int EMOTE_CHARGE_CHANCE = 30;
    private const float MOVE_DISTANCE_PENALTY = 0.42f;
    private const float LEADER_LOF_PENALTY = 1f;
    private ActorOrder m_Order;
    private ActorDirective m_Directive;
    private Location m_prevLocation;
    private List<Item> m_TabooItems;
    private List<Point> m_TabooTiles;
    private List<Actor> m_TabooTrades;

    public override ActorOrder Order
    {
      get
      {
        return this.m_Order;
      }
    }

    public override ActorDirective Directives
    {
      get
      {
        if (this.m_Directive == null)
          this.m_Directive = new ActorDirective();
        return this.m_Directive;
      }
      set
      {
        this.m_Directive = value;
      }
    }

    protected Location PrevLocation
    {
      get
      {
        return this.m_prevLocation;
      }
    }

    protected List<Item> TabooItems
    {
      get
      {
        return this.m_TabooItems;
      }
    }

    protected List<Point> TabooTiles
    {
      get
      {
        return this.m_TabooTiles;
      }
    }

    protected List<Actor> TabooTrades
    {
      get
      {
        return this.m_TabooTrades;
      }
    }

    public override void TakeControl(Actor actor)
    {
      base.TakeControl(actor);
      this.CreateSensors();
      this.m_TabooItems = (List<Item>) null;
      this.m_TabooTiles = (List<Point>) null;
      this.m_TabooTrades = (List<Actor>) null;
    }

    public override void SetOrder(ActorOrder newOrder)
    {
      this.m_Order = newOrder;
    }

    public override ActorAction GetAction(RogueGame game)
    {
      List<Percept> percepts = this.UpdateSensors(game);
      if (this.m_prevLocation.Map == null)
        this.m_prevLocation = this.m_Actor.Location;
      this.m_Actor.TargetActor = (Actor) null;
      ActorAction actorAction = this.SelectAction(game, percepts);
      this.m_prevLocation = this.m_Actor.Location;
      if (actorAction != null)
        return actorAction;
      this.m_Actor.Activity = Activity.IDLE;
      return (ActorAction) new ActionWait(this.m_Actor, game);
    }

    protected abstract void CreateSensors();

    protected abstract List<Percept> UpdateSensors(RogueGame game);

    protected abstract ActorAction SelectAction(RogueGame game, List<Percept> percepts);

    protected List<Percept> FilterSameMap(RogueGame game, List<Percept> percepts)
    {
      if (percepts == null || percepts.Count == 0)
        return (List<Percept>) null;
      List<Percept> perceptList = (List<Percept>) null;
      Location location = this.m_Actor.Location;
      Map map = location.Map;
      foreach (Percept percept in percepts)
      {
        location = percept.Location;
        if (location.Map == map)
        {
          if (perceptList == null)
            perceptList = new List<Percept>(percepts.Count);
          perceptList.Add(percept);
        }
      }
      return perceptList;
    }

    protected List<Percept> FilterEnemies(RogueGame game, List<Percept> percepts)
    {
      if (percepts == null || percepts.Count == 0)
        return (List<Percept>) null;
      List<Percept> perceptList = (List<Percept>) null;
      foreach (Percept percept in percepts)
      {
        Actor percepted = percept.Percepted as Actor;
        if (percepted != null && percepted != this.m_Actor && game.Rules.IsEnemyOf(this.m_Actor, percepted))
        {
          if (perceptList == null)
            perceptList = new List<Percept>(percepts.Count);
          perceptList.Add(percept);
        }
      }
      return perceptList;
    }

    protected List<Percept> FilterNonEnemies(RogueGame game, List<Percept> percepts)
    {
      if (percepts == null || percepts.Count == 0)
        return (List<Percept>) null;
      List<Percept> perceptList = (List<Percept>) null;
      foreach (Percept percept in percepts)
      {
        Actor percepted = percept.Percepted as Actor;
        if (percepted != null && percepted != this.m_Actor && !game.Rules.IsEnemyOf(this.m_Actor, percepted))
        {
          if (perceptList == null)
            perceptList = new List<Percept>(percepts.Count);
          perceptList.Add(percept);
        }
      }
      return perceptList;
    }

    protected List<Percept> FilterCurrent(RogueGame game, List<Percept> percepts)
    {
      if (percepts == null || percepts.Count == 0)
        return (List<Percept>) null;
      List<Percept> perceptList = (List<Percept>) null;
      int turnCounter = this.m_Actor.Location.Map.LocalTime.TurnCounter;
      foreach (Percept percept in percepts)
      {
        if (percept.Turn == turnCounter)
        {
          if (perceptList == null)
            perceptList = new List<Percept>(percepts.Count);
          perceptList.Add(percept);
        }
      }
      return perceptList;
    }

    protected Percept FilterNearest(RogueGame game, List<Percept> percepts)
    {
      if (percepts == null || percepts.Count == 0)
        return (Percept) null;
      Percept percept1 = percepts[0];
      Rules rules1 = game.Rules;
      Location location = this.m_Actor.Location;
      Point position1 = location.Position;
      location = percepts[0].Location;
      Point position2 = location.Position;
      float num1 = rules1.StdDistance(position1, position2);
      for (int index = 1; index < percepts.Count; ++index)
      {
        Percept percept2 = percepts[index];
        Rules rules2 = game.Rules;
        location = this.m_Actor.Location;
        Point position3 = location.Position;
        location = percept2.Location;
        Point position4 = location.Position;
        float num2 = rules2.StdDistance(position3, position4);
        if ((double) num2 < (double) num1)
        {
          percept1 = percept2;
          num1 = num2;
        }
      }
      return percept1;
    }

    protected Percept FilterStrongestScent(RogueGame game, List<Percept> scents)
    {
      if (scents == null || scents.Count == 0)
        return (Percept) null;
      Percept percept = (Percept) null;
      SmellSensor.AIScent aiScent = (SmellSensor.AIScent) null;
      foreach (Percept scent in scents)
      {
        SmellSensor.AIScent percepted = scent.Percepted as SmellSensor.AIScent;
        if (percepted == null)
          throw new InvalidOperationException("percept not an aiScent");
        if (percept == null || percepted.Strength > aiScent.Strength)
        {
          aiScent = percepted;
          percept = scent;
        }
      }
      return percept;
    }

    protected List<Percept> FilterActorsModel(RogueGame game, List<Percept> percepts, ActorModel model)
    {
      if (percepts == null || percepts.Count == 0)
        return (List<Percept>) null;
      List<Percept> perceptList = (List<Percept>) null;
      foreach (Percept percept in percepts)
      {
        Actor percepted = percept.Percepted as Actor;
        if (percepted != null && percepted.Model == model)
        {
          if (perceptList == null)
            perceptList = new List<Percept>(percepts.Count);
          perceptList.Add(percept);
        }
      }
      return perceptList;
    }

    protected List<Percept> FilterActors(RogueGame game, List<Percept> percepts, Predicate<Actor> predicateFn)
    {
      if (percepts == null || percepts.Count == 0)
        return (List<Percept>) null;
      List<Percept> perceptList = (List<Percept>) null;
      foreach (Percept percept in percepts)
      {
        Actor percepted = percept.Percepted as Actor;
        if (percepted != null && predicateFn(percepted))
        {
          if (perceptList == null)
            perceptList = new List<Percept>(percepts.Count);
          perceptList.Add(percept);
        }
      }
      return perceptList;
    }

    protected List<Percept> FilterFireTargets(RogueGame game, List<Percept> percepts)
    {
      return this.Filter(game, percepts, (Predicate<Percept>) (p =>
      {
        Actor percepted = p.Percepted as Actor;
        if (percepted == null)
          return false;
        return game.Rules.CanActorFireAt(this.m_Actor, percepted);
      }));
    }

    protected List<Percept> FilterStacks(RogueGame game, List<Percept> percepts)
    {
      return this.Filter(game, percepts, (Predicate<Percept>) (p => p.Percepted is Inventory));
    }

    protected List<Percept> FilterCorpses(RogueGame game, List<Percept> percepts)
    {
      return this.Filter(game, percepts, (Predicate<Percept>) (p => p.Percepted is List<Corpse>));
    }

    protected List<Percept> Filter(RogueGame game, List<Percept> percepts, Predicate<Percept> predicateFn)
    {
      if (percepts == null || percepts.Count == 0)
        return (List<Percept>) null;
      List<Percept> perceptList = (List<Percept>) null;
      foreach (Percept percept in percepts)
      {
        if (predicateFn(percept))
        {
          if (perceptList == null)
            perceptList = new List<Percept>(percepts.Count);
          perceptList.Add(percept);
        }
      }
      return perceptList;
    }

    protected Percept FilterFirst(RogueGame game, List<Percept> percepts, Predicate<Percept> predicateFn)
    {
      if (percepts == null || percepts.Count == 0)
        return (Percept) null;
      foreach (Percept percept in percepts)
      {
        if (predicateFn(percept))
          return percept;
      }
      return (Percept) null;
    }

    protected List<Percept> FilterOut(RogueGame game, List<Percept> percepts, Predicate<Percept> rejectPredicateFn)
    {
      return this.Filter(game, percepts, (Predicate<Percept>) (p => !rejectPredicateFn(p)));
    }

    protected List<Percept> SortByDistance(RogueGame game, List<Percept> percepts)
    {
      if (percepts == null || percepts.Count == 0)
        return (List<Percept>) null;
      Point from = this.m_Actor.Location.Position;
      List<Percept> perceptList = new List<Percept>((IEnumerable<Percept>) percepts);
      perceptList.Sort((Comparison<Percept>) ((pA, pB) =>
      {
        float num1 = game.Rules.StdDistance(pA.Location.Position, from);
        float num2 = game.Rules.StdDistance(pB.Location.Position, from);
        if ((double) num1 > (double) num2)
          return 1;
        return (double) num1 >= (double) num2 ? 0 : -1;
      }));
      return perceptList;
    }

    protected List<Percept> SortByDate(RogueGame game, List<Percept> percepts)
    {
      if (percepts == null || percepts.Count == 0)
        return (List<Percept>) null;
      List<Percept> perceptList = new List<Percept>((IEnumerable<Percept>) percepts);
      perceptList.Sort((Comparison<Percept>) ((pA, pB) =>
      {
        if (pA.Turn < pB.Turn)
          return 1;
        return pA.Turn <= pB.Turn ? 0 : -1;
      }));
      return perceptList;
    }

    protected ActorAction BehaviorWander(RogueGame game, Predicate<Location> goodWanderLocFn)
    {
      BaseAI.ChoiceEval<Direction> choiceEval = this.Choose<Direction>(game, Direction.COMPASS_LIST, (Func<Direction, bool>) (dir =>
      {
        Location location = this.m_Actor.Location + dir;
        if (goodWanderLocFn != null && !goodWanderLocFn(location))
          return false;
        return this.isValidWanderAction(game, game.Rules.IsBumpableFor(this.m_Actor, game, location));
      }), (Func<Direction, float>) (dir =>
      {
        int num = game.Rules.Roll(0, 666);
        if (this.m_Actor.Model.Abilities.IsIntelligent && BaseAI.IsAnyActivatedTrapThere(this.m_Actor.Location.Map, (this.m_Actor.Location + dir).Position))
          num -= 1000;
        return (float) num;
      }), (Func<float, float, bool>) ((a, b) => (double) a > (double) b));
      if (choiceEval != null)
        return (ActorAction) new ActionBump(this.m_Actor, game, choiceEval.Choice);
      return (ActorAction) null;
    }

    protected ActorAction BehaviorWander(RogueGame game)
    {
      return this.BehaviorWander(game, (Predicate<Location>) null);
    }

    protected ActorAction BehaviorBumpToward(RogueGame game, Point goal, Func<Point, Point, float> distanceFn)
    {
      return this.ChooseExtended<Direction, ActorAction>(game, Direction.COMPASS_LIST, (Func<Direction, ActorAction>) (dir =>
      {
        Location location = this.m_Actor.Location + dir;
        ActorAction a = game.Rules.IsBumpableFor(this.m_Actor, game, location);
        if (a == null)
        {
          if (this.m_Actor.Model.Abilities.IsUndead && game.Rules.HasActorPushAbility(this.m_Actor))
          {
            MapObject mapObjectAt = this.m_Actor.Location.Map.GetMapObjectAt(location.Position);
            if (mapObjectAt != null && game.Rules.CanActorPush(this.m_Actor, mapObjectAt))
            {
              Direction pushDir = game.Rules.RollDirection();
              if (game.Rules.CanPushObjectTo(mapObjectAt, mapObjectAt.Location.Position + pushDir))
                return (ActorAction) new ActionPush(this.m_Actor, game, mapObjectAt, pushDir);
            }
          }
          return (ActorAction) null;
        }
        if (location.Position == goal || this.IsValidMoveTowardGoalAction(a))
          return a;
        return (ActorAction) null;
      }), (Func<Direction, float>) (dir =>
      {
        Location location = this.m_Actor.Location + dir;
        if (distanceFn != null)
          return distanceFn(location.Position, goal);
        return game.Rules.StdDistance(location.Position, goal);
      }), (Func<float, float, bool>) ((a, b) =>
      {
        if (!float.IsNaN(a))
          return (double) a < (double) b;
        return false;
      }))?.Choice;
    }

    protected ActorAction BehaviorStupidBumpToward(RogueGame game, Point goal)
    {
      return this.BehaviorBumpToward(game, goal, (Func<Point, Point, float>) ((ptA, ptB) =>
      {
        if (ptA == ptB)
          return 0.0f;
        float num = game.Rules.StdDistance(ptA, ptB);
        if (!game.Rules.IsWalkableFor(this.m_Actor, this.m_Actor.Location.Map, ptA.X, ptA.Y))
          num += 0.42f;
        return num;
      }));
    }

    protected ActorAction BehaviorIntelligentBumpToward(RogueGame game, Point goal)
    {
      float currentDistance = game.Rules.StdDistance(this.m_Actor.Location.Position, goal);
      bool imStarvingOrCourageous = game.Rules.IsActorStarving(this.m_Actor) || this.Directives.Courage == ActorCourage.COURAGEOUS;
      return this.BehaviorBumpToward(game, goal, (Func<Point, Point, float>) ((ptA, ptB) =>
      {
        if (ptA == ptB)
          return 0.0f;
        float num = game.Rules.StdDistance(ptA, ptB);
        if ((double) num >= (double) currentDistance)
          return float.NaN;
        if (!imStarvingOrCourageous)
        {
          int trapsMaxDamage = this.ComputeTrapsMaxDamage(this.m_Actor.Location.Map, ptA);
          if (trapsMaxDamage > 0)
          {
            if (trapsMaxDamage >= this.m_Actor.HitPoints)
              return float.NaN;
            num += 0.42f;
          }
        }
        return num;
      }));
    }

    protected ActorAction BehaviorWalkAwayFrom(RogueGame game, Percept goal)
    {
      return this.BehaviorWalkAwayFrom(game, new List<Percept>(1)
      {
        goal
      });
    }

    protected ActorAction BehaviorWalkAwayFrom(RogueGame game, List<Percept> goals)
    {
      Actor leader = this.m_Actor.Leader;
      int num1 = !this.m_Actor.HasLeader ? 0 : (this.m_Actor.GetEquippedWeapon() is ItemRangedWeapon ? 1 : 0);
      Actor actor = (Actor) null;
      if (num1 != 0)
        actor = this.GetNearestTargetFor(game, this.m_Actor.Leader);
      bool checkLeaderLoF = actor != null && actor.Location.Map == this.m_Actor.Location.Map;
      List<Point> leaderLoF = (List<Point>) null;
      if (checkLeaderLoF)
      {
        leaderLoF = new List<Point>(1);
        ItemRangedWeapon equippedWeapon = this.m_Actor.GetEquippedWeapon() as ItemRangedWeapon;
        LOS.CanTraceFireLine(leader.Location, actor.Location.Position, (equippedWeapon.Model as ItemRangedWeaponModel).Attack.Range, leaderLoF);
      }
      BaseAI.ChoiceEval<Direction> choiceEval = this.Choose<Direction>(game, Direction.COMPASS_LIST, (Func<Direction, bool>) (dir => this.IsValidFleeingAction(game.Rules.IsBumpableFor(this.m_Actor, game, this.m_Actor.Location + dir))), (Func<Direction, float>) (dir =>
      {
        Location location = this.m_Actor.Location + dir;
        float num2 = this.SafetyFrom(game.Rules, location.Position, goals);
        if (this.m_Actor.HasLeader)
        {
          num2 -= game.Rules.StdDistance(location.Position, this.m_Actor.Leader.Location.Position);
          if (checkLeaderLoF && leaderLoF.Contains(location.Position))
            --num2;
        }
        return num2;
      }), (Func<float, float, bool>) ((a, b) => (double) a > (double) b));
      if (choiceEval != null)
        return (ActorAction) new ActionBump(this.m_Actor, game, choiceEval.Choice);
      return (ActorAction) null;
    }

    protected ActorAction BehaviorMeleeAttack(RogueGame game, Percept target)
    {
      Actor percepted = target.Percepted as Actor;
      if (percepted == null)
        throw new ArgumentException("percepted is not an actor");
      if (!game.Rules.CanActorMeleeAttack(this.m_Actor, percepted))
        return (ActorAction) null;
      return (ActorAction) new ActionMeleeAttack(this.m_Actor, game, percepted);
    }

    protected ActorAction BehaviorRangedAttack(RogueGame game, Percept target)
    {
      Actor percepted = target.Percepted as Actor;
      if (percepted == null)
        throw new ArgumentException("percepted is not an actor");
      if (!game.Rules.CanActorFireAt(this.m_Actor, percepted))
        return (ActorAction) null;
      return (ActorAction) new ActionRangedAttack(this.m_Actor, game, percepted);
    }

    protected ActorAction BehaviorEquipWeapon(RogueGame game)
    {
      Item equippedWeapon = this.GetEquippedWeapon();
      if (equippedWeapon != null && equippedWeapon is ItemRangedWeapon)
      {
        if (!this.Directives.CanFireWeapons)
          return (ActorAction) new ActionUnequipItem(this.m_Actor, game, equippedWeapon);
        ItemRangedWeapon rw = equippedWeapon as ItemRangedWeapon;
        if (rw.Ammo > 0)
          return (ActorAction) null;
        ItemAmmo compatibleAmmoItem = this.GetCompatibleAmmoItem(game, rw);
        if (compatibleAmmoItem != null)
          return (ActorAction) new ActionUseItem(this.m_Actor, game, (Item) compatibleAmmoItem);
      }
      if (this.Directives.CanFireWeapons)
      {
        Item rangedWeaponWithAmmo = this.GetBestRangedWeaponWithAmmo((Predicate<Item>) (it => !this.IsItemTaboo(it)));
        if (rangedWeaponWithAmmo != null && game.Rules.CanActorEquipItem(this.m_Actor, rangedWeaponWithAmmo))
          return (ActorAction) new ActionEquipItem(this.m_Actor, game, rangedWeaponWithAmmo);
      }
      ItemMeleeWeapon bestMeleeWeapon = this.GetBestMeleeWeapon(game, (Predicate<Item>) (it => !this.IsItemTaboo(it)));
      if (bestMeleeWeapon == null)
        return (ActorAction) null;
      if (equippedWeapon == bestMeleeWeapon)
        return (ActorAction) null;
      if (equippedWeapon == null)
      {
        if (game.Rules.CanActorEquipItem(this.m_Actor, (Item) bestMeleeWeapon))
          return (ActorAction) new ActionEquipItem(this.m_Actor, game, (Item) bestMeleeWeapon);
        return (ActorAction) null;
      }
      if (equippedWeapon == null)
        return (ActorAction) null;
      if (game.Rules.CanActorUnequipItem(this.m_Actor, equippedWeapon))
        return (ActorAction) new ActionUnequipItem(this.m_Actor, game, equippedWeapon);
      return (ActorAction) null;
    }

    protected ActorAction BehaviorEquipBodyArmor(RogueGame game)
    {
      ItemBodyArmor bestBodyArmor = this.GetBestBodyArmor(game, (Predicate<Item>) (it => !this.IsItemTaboo(it)));
      if (bestBodyArmor == null)
        return (ActorAction) null;
      Item equippedBodyArmor = this.GetEquippedBodyArmor();
      if (equippedBodyArmor == bestBodyArmor)
        return (ActorAction) null;
      if (equippedBodyArmor != null)
      {
        if (game.Rules.CanActorUnequipItem(this.m_Actor, equippedBodyArmor))
          return (ActorAction) new ActionUnequipItem(this.m_Actor, game, equippedBodyArmor);
        return (ActorAction) null;
      }
      if (equippedBodyArmor != null)
        return (ActorAction) null;
      if (game.Rules.CanActorEquipItem(this.m_Actor, (Item) bestBodyArmor))
        return (ActorAction) new ActionEquipItem(this.m_Actor, game, (Item) bestBodyArmor);
      return (ActorAction) null;
    }

    protected ActorAction BehaviorEquipCellPhone(RogueGame game)
    {
      bool flag1 = false;
      if (this.m_Actor.CountFollowers > 0)
        flag1 = true;
      else if (this.m_Actor.HasLeader)
      {
        bool flag2 = false;
        ItemTracker equippedItem = this.m_Actor.Leader.GetEquippedItem(DollPart.LEFT_HAND) as ItemTracker;
        if (equippedItem == null)
          flag2 = false;
        else if (equippedItem.CanTrackFollowersOrLeader)
          flag2 = true;
        flag1 = flag2;
      }
      Item equippedCellPhone = this.GetEquippedCellPhone();
      if (equippedCellPhone != null)
      {
        if (!flag1)
          return (ActorAction) new ActionUnequipItem(this.m_Actor, game, equippedCellPhone);
        return (ActorAction) null;
      }
      if (!flag1)
        return (ActorAction) null;
      Item firstTracker = this.GetFirstTracker((Predicate<ItemTracker>) (it =>
      {
        if (it.CanTrackFollowersOrLeader)
          return !this.IsItemTaboo((Item) it);
        return false;
      }));
      if (firstTracker != null && game.Rules.CanActorEquipItem(this.m_Actor, firstTracker))
        return (ActorAction) new ActionEquipItem(this.m_Actor, game, firstTracker);
      return (ActorAction) null;
    }

    protected ActorAction BehaviorUnequipCellPhoneIfLeaderHasNot(RogueGame game)
    {
      ItemTracker equippedItem1 = this.m_Actor.GetEquippedItem(DollPart.LEFT_HAND) as ItemTracker;
      if (equippedItem1 == null)
        return (ActorAction) null;
      if (!equippedItem1.CanTrackFollowersOrLeader)
        return (ActorAction) null;
      ItemTracker equippedItem2 = this.m_Actor.Leader.GetEquippedItem(DollPart.LEFT_HAND) as ItemTracker;
      if ((equippedItem2 == null || !equippedItem2.CanTrackFollowersOrLeader) && game.Rules.CanActorUnequipItem(this.m_Actor, (Item) equippedItem1))
        return (ActorAction) new ActionUnequipItem(this.m_Actor, game, (Item) equippedItem1);
      return (ActorAction) null;
    }

    protected ActorAction BehaviorEquipLight(RogueGame game)
    {
      if (this.GetEquippedLight() != null)
        return (ActorAction) null;
      Item firstLight = this.GetFirstLight((Predicate<Item>) (it => !this.IsItemTaboo(it)));
      if (firstLight != null && game.Rules.CanActorEquipItem(this.m_Actor, firstLight))
        return (ActorAction) new ActionEquipItem(this.m_Actor, game, firstLight);
      return (ActorAction) null;
    }

    protected ActorAction BehaviorEquipStenchKiller(RogueGame game)
    {
      if (this.GetEquippedStenchKiller() != null)
        return (ActorAction) null;
      ItemSprayScent firstStenchKiller = this.GetFirstStenchKiller((Predicate<ItemSprayScent>) (it => !this.IsItemTaboo((Item) it)));
      if (firstStenchKiller != null && game.Rules.CanActorEquipItem(this.m_Actor, (Item) firstStenchKiller))
        return (ActorAction) new ActionEquipItem(this.m_Actor, game, (Item) firstStenchKiller);
      return (ActorAction) null;
    }

    protected ActorAction BehaviorUnequipLeftItem(RogueGame game)
    {
      Item equippedItem = this.m_Actor.GetEquippedItem(DollPart.LEFT_HAND);
      if (equippedItem == null)
        return (ActorAction) null;
      if (game.Rules.CanActorUnequipItem(this.m_Actor, equippedItem))
        return (ActorAction) new ActionUnequipItem(this.m_Actor, game, equippedItem);
      return (ActorAction) null;
    }

    protected ActorAction BehaviorGrabFromStack(RogueGame game, Point position, Inventory stack)
    {
      if (stack == null || stack.IsEmpty)
        return (ActorAction) null;
      MapObject mapObjectAt = this.m_Actor.Location.Map.GetMapObjectAt(position);
      if (mapObjectAt != null)
      {
        Fortification fortification = mapObjectAt as Fortification;
        if (fortification != null && !fortification.IsWalkable)
          return (ActorAction) null;
        DoorWindow doorWindow = mapObjectAt as DoorWindow;
        if (doorWindow != null && doorWindow.IsBarricaded)
          return (ActorAction) null;
      }
      Item obj = (Item) null;
      foreach (Item it in stack.Items)
      {
        if (game.Rules.CanActorGetItem(this.m_Actor, it) && this.IsInterestingItem(game, it))
        {
          obj = it;
          break;
        }
      }
      if (obj == null)
        return (ActorAction) null;
      Item it1 = obj;
      if (game.Rules.RollChance(30))
        game.DoEmote(this.m_Actor, string.Format("{0}! Great!", (object) it1.AName));
      if (position == this.m_Actor.Location.Position)
        return (ActorAction) new ActionTakeItem(this.m_Actor, game, position, it1);
      return this.BehaviorIntelligentBumpToward(game, position);
    }

    protected ActorAction BehaviorDropItem(RogueGame game, Item it)
    {
      if (it == null)
        return (ActorAction) null;
      if (game.Rules.CanActorUnequipItem(this.m_Actor, it))
      {
        this.MarkItemAsTaboo(it);
        return (ActorAction) new ActionUnequipItem(this.m_Actor, game, it);
      }
      if (!game.Rules.CanActorDropItem(this.m_Actor, it))
        return (ActorAction) null;
      this.UnmarkItemAsTaboo(it);
      return (ActorAction) new ActionDropItem(this.m_Actor, game, it);
    }

    protected ActorAction BehaviorDropUselessItem(RogueGame game)
    {
      if (this.m_Actor.Inventory.IsEmpty)
        return (ActorAction) null;
      foreach (Item it in this.m_Actor.Inventory.Items)
      {
        bool flag = false;
        if (it is ItemLight)
          flag = (it as ItemLight).Batteries <= 0;
        else if (it is ItemTracker)
          flag = (it as ItemTracker).Batteries <= 0;
        else if (it is ItemSprayPaint)
          flag = (it as ItemSprayPaint).PaintQuantity <= 0;
        else if (it is ItemSprayScent)
          flag = (it as ItemSprayScent).SprayQuantity <= 0;
        if (flag)
          return this.BehaviorDropItem(game, it);
      }
      return (ActorAction) null;
    }

    protected ActorAction BehaviorRestIfTired(RogueGame game)
    {
      if (this.m_Actor.StaminaPoints >= 10)
        return (ActorAction) null;
      return (ActorAction) new ActionWait(this.m_Actor, game);
    }

    protected ActorAction BehaviorEat(RogueGame game)
    {
      Item bestEdibleItem = this.GetBestEdibleItem(game);
      if (bestEdibleItem == null)
        return (ActorAction) null;
      if (!game.Rules.CanActorUseItem(this.m_Actor, bestEdibleItem))
        return (ActorAction) null;
      return (ActorAction) new ActionUseItem(this.m_Actor, game, bestEdibleItem);
    }

    protected ActorAction BehaviorSleep(RogueGame game, HashSet<Point> FOV)
    {
      if (!game.Rules.CanActorSleep(this.m_Actor))
        return (ActorAction) null;
      Map map = this.m_Actor.Location.Map;
      if (map.HasAnyAdjacentInMap(this.m_Actor.Location.Position, (Predicate<Point>) (pt => map.GetMapObjectAt(pt) is DoorWindow)))
      {
        ActorAction actorAction = this.BehaviorWander(game, (Predicate<Location>) (loc =>
        {
          if (!(map.GetMapObjectAt(loc.Position) is DoorWindow))
            return !map.HasAnyAdjacentInMap(loc.Position, (Predicate<Point>) (pt => loc.Map.GetMapObjectAt(pt) is DoorWindow));
          return false;
        }));
        if (actorAction != null)
          return actorAction;
      }
      if (game.Rules.IsOnCouch(this.m_Actor))
        return (ActorAction) new ActionSleep(this.m_Actor, game);
      Point? nullable = new Point?();
      float num1 = float.MaxValue;
      foreach (Point point in FOV)
      {
        MapObject mapObjectAt = map.GetMapObjectAt(point);
        if (mapObjectAt != null && mapObjectAt.IsCouch && map.GetActorAt(point) == null)
        {
          float num2 = game.Rules.StdDistance(this.m_Actor.Location.Position, point);
          if ((double) num2 < (double) num1)
          {
            num1 = num2;
            nullable = new Point?(point);
          }
        }
      }
      if (nullable.HasValue)
      {
        ActorAction actorAction = this.BehaviorIntelligentBumpToward(game, nullable.Value);
        if (actorAction != null)
          return actorAction;
      }
      return (ActorAction) new ActionSleep(this.m_Actor, game);
    }

    protected int ComputeTrapsMaxDamage(Map map, Point pos)
    {
      Inventory itemsAt = map.GetItemsAt(pos);
      if (itemsAt == null)
        return 0;
      int num = 0;
      foreach (Item obj in itemsAt.Items)
      {
        ItemTrap itemTrap = obj as ItemTrap;
        if (itemTrap != null)
          num += itemTrap.TrapModel.Damage;
      }
      return num;
    }

    protected ActorAction BehaviorBuildTrap(RogueGame game)
    {
      ItemTrap firstByType = this.m_Actor.Inventory.GetFirstByType(typeof (ItemTrap)) as ItemTrap;
      if (firstByType == null)
        return (ActorAction) null;
      string reason;
      if (!this.IsGoodTrapSpot(game, this.m_Actor.Location.Map, this.m_Actor.Location.Position, out reason))
        return (ActorAction) null;
      if (!firstByType.IsActivated && !firstByType.TrapModel.ActivatesWhenDropped)
        return (ActorAction) new ActionUseItem(this.m_Actor, game, (Item) firstByType);
      game.DoEmote(this.m_Actor, string.Format("{0} {1}!", (object) reason, (object) firstByType.AName));
      return (ActorAction) new ActionDropItem(this.m_Actor, game, (Item) firstByType);
    }

    protected bool IsGoodTrapSpot(RogueGame game, Map map, Point pos, out string reason)
    {
      reason = "";
      bool flag = false;
      bool isInside = map.GetTileAt(pos).IsInside;
      if (!isInside && map.GetCorpsesAt(pos) != null)
      {
        reason = "that corpse will serve as a bait for";
        flag = true;
      }
      else if (this.m_prevLocation.Map.GetTileAt(this.m_prevLocation.Position).IsInside != isInside)
      {
        reason = "protecting the building with";
        flag = true;
      }
      else
      {
        MapObject mapObjectAt = map.GetMapObjectAt(pos);
        if (mapObjectAt != null && mapObjectAt is DoorWindow)
        {
          reason = "protecting the doorway with";
          flag = true;
        }
        else if (map.GetExitAt(pos) != null)
        {
          reason = "protecting the exit with";
          flag = true;
        }
      }
      if (!flag)
        return false;
      Inventory itemsAt = map.GetItemsAt(pos);
      return itemsAt == null || itemsAt.CountItemsMatching((Predicate<Item>) (it =>
      {
        ItemTrap itemTrap = it as ItemTrap;
        if (itemTrap == null)
          return false;
        return itemTrap.IsActivated;
      })) <= 3;
    }

    protected ActorAction BehaviorBuildSmallFortification(RogueGame game)
    {
      if (this.m_Actor.Sheet.SkillTable.GetSkillLevel(3) == 0)
        return (ActorAction) null;
      if (game.Rules.CountBarricadingMaterial(this.m_Actor) < game.Rules.ActorBarricadingMaterialNeedForFortification(this.m_Actor, false))
        return (ActorAction) null;
      Map map = this.m_Actor.Location.Map;
      BaseAI.ChoiceEval<Direction> choiceEval = this.Choose<Direction>(game, Direction.COMPASS_LIST, (Func<Direction, bool>) (dir =>
      {
        Point point = this.m_Actor.Location.Position + dir;
        if (!map.IsInBounds(point) || !map.IsWalkable(point) || (map.IsOnMapBorder(point.X, point.Y) || map.GetActorAt(point) != null) || map.GetExitAt(point) != null)
          return false;
        return this.IsDoorwayOrCorridor(game, map, point);
      }), (Func<Direction, float>) (dir => (float) game.Rules.Roll(0, 666)), (Func<float, float, bool>) ((a, b) => (double) a > (double) b));
      if (choiceEval == null)
        return (ActorAction) null;
      Point point1 = this.m_Actor.Location.Position + choiceEval.Choice;
      if (!game.Rules.CanActorBuildFortification(this.m_Actor, point1, false))
        return (ActorAction) null;
      return (ActorAction) new ActionBuildFortification(this.m_Actor, game, point1, false);
    }

    protected ActorAction BehaviorBuildLargeFortification(RogueGame game, int startLineChance)
    {
      if (this.m_Actor.Sheet.SkillTable.GetSkillLevel(3) == 0)
        return (ActorAction) null;
      if (game.Rules.CountBarricadingMaterial(this.m_Actor) < game.Rules.ActorBarricadingMaterialNeedForFortification(this.m_Actor, true))
        return (ActorAction) null;
      Map map = this.m_Actor.Location.Map;
      BaseAI.ChoiceEval<Direction> choiceEval = this.Choose<Direction>(game, Direction.COMPASS_LIST, (Func<Direction, bool>) (dir =>
      {
        Point point = this.m_Actor.Location.Position + dir;
        if (!map.IsInBounds(point) || !map.IsWalkable(point) || (map.IsOnMapBorder(point.X, point.Y) || map.GetActorAt(point) != null) || (map.GetExitAt(point) != null || map.GetTileAt(point.X, point.Y).IsInside))
          return false;
        int num1 = map.CountAdjacentInMap(point, (Predicate<Point>) (ptAdj => !map.GetTileAt(ptAdj).Model.IsWalkable));
        int num2 = map.CountAdjacentInMap(point, (Predicate<Point>) (ptAdj =>
        {
          Fortification mapObjectAt = map.GetMapObjectAt(ptAdj) as Fortification;
          if (mapObjectAt != null)
            return !mapObjectAt.IsTransparent;
          return false;
        }));
        return num1 == 3 && num2 == 0 && game.Rules.RollChance(startLineChance) || num1 == 0 && num2 == 1;
      }), (Func<Direction, float>) (dir => (float) game.Rules.Roll(0, 666)), (Func<float, float, bool>) ((a, b) => (double) a > (double) b));
      if (choiceEval == null)
        return (ActorAction) null;
      Point point1 = this.m_Actor.Location.Position + choiceEval.Choice;
      if (!game.Rules.CanActorBuildFortification(this.m_Actor, point1, true))
        return (ActorAction) null;
      return (ActorAction) new ActionBuildFortification(this.m_Actor, game, point1, true);
    }

    protected ActorAction BehaviorAttackBarricade(RogueGame game)
    {
      Map map = this.m_Actor.Location.Map;
      List<Point> pointList = map.FilterAdjacentInMap(this.m_Actor.Location.Position, (Predicate<Point>) (pt =>
      {
        DoorWindow mapObjectAt = map.GetMapObjectAt(pt) as DoorWindow;
        if (mapObjectAt != null)
          return mapObjectAt.IsBarricaded;
        return false;
      }));
      if (pointList == null)
        return (ActorAction) null;
      DoorWindow mapObjectAt1 = map.GetMapObjectAt(pointList[game.Rules.Roll(0, pointList.Count)]) as DoorWindow;
      ActionBreak actionBreak = new ActionBreak(this.m_Actor, game, (MapObject) mapObjectAt1);
      if (actionBreak.IsLegal())
        return (ActorAction) actionBreak;
      return (ActorAction) null;
    }

    protected ActorAction BehaviorAssaultBreakables(RogueGame game, HashSet<Point> fov)
    {
      Map map = this.m_Actor.Location.Map;
      List<Percept> percepts = (List<Percept>) null;
      foreach (Point position in fov)
      {
        MapObject mapObjectAt = map.GetMapObjectAt(position);
        if (mapObjectAt != null && mapObjectAt.IsBreakable)
        {
          if (percepts == null)
            percepts = new List<Percept>();
          percepts.Add(new Percept((object) mapObjectAt, map.LocalTime.TurnCounter, new Location(map, position)));
        }
      }
      if (percepts == null)
        return (ActorAction) null;
      Percept percept = this.FilterNearest(game, percepts);
      Rules rules = game.Rules;
      Location location = this.m_Actor.Location;
      Point position1 = location.Position;
      location = percept.Location;
      Point position2 = location.Position;
      if (rules.IsAdjacent(position1, position2))
      {
        ActionBreak actionBreak = new ActionBreak(this.m_Actor, game, percept.Percepted as MapObject);
        if (actionBreak.IsLegal())
          return (ActorAction) actionBreak;
        return (ActorAction) null;
      }
      RogueGame game1 = game;
      location = percept.Location;
      Point position3 = location.Position;
      return this.BehaviorIntelligentBumpToward(game1, position3);
    }

    protected ActorAction BehaviorPushNonWalkableObject(RogueGame game)
    {
      if (!game.Rules.HasActorPushAbility(this.m_Actor))
        return (ActorAction) null;
      Map map = this.m_Actor.Location.Map;
      List<Point> pointList = map.FilterAdjacentInMap(this.m_Actor.Location.Position, (Predicate<Point>) (pt =>
      {
        MapObject mapObjectAt = map.GetMapObjectAt(pt);
        if (mapObjectAt == null || mapObjectAt.IsWalkable)
          return false;
        return game.Rules.CanActorPush(this.m_Actor, mapObjectAt);
      }));
      if (pointList == null)
        return (ActorAction) null;
      MapObject mapObjectAt1 = map.GetMapObjectAt(pointList[game.Rules.Roll(0, pointList.Count)]);
      ActionPush actionPush = new ActionPush(this.m_Actor, game, mapObjectAt1, game.Rules.RollDirection());
      if (actionPush.IsLegal())
        return (ActorAction) actionPush;
      return (ActorAction) null;
    }

    protected ActorAction BehaviorUseMedecine(RogueGame game, int factorHealing, int factorStamina, int factorSleep, int factorCure, int factorSan)
    {
      Inventory inventory = this.m_Actor.Inventory;
      if (inventory == null || inventory.IsEmpty)
        return (ActorAction) null;
      bool needHP = this.m_Actor.HitPoints < game.Rules.ActorMaxHPs(this.m_Actor);
      bool needSTA = game.Rules.IsActorTired(this.m_Actor);
      bool needSLP = this.m_Actor.Model.Abilities.HasToSleep && this.WouldLikeToSleep(game, this.m_Actor);
      bool needCure = this.m_Actor.Infection > 0;
      bool needSan = this.m_Actor.Model.Abilities.HasSanity && this.m_Actor.Sanity < (int) (0.75 * (double) game.Rules.ActorMaxSanity(this.m_Actor));
      if (!needHP && !needSTA && (!needSLP && !needCure) && !needSan)
        return (ActorAction) null;
      List<ItemMedicine> itemsByType = inventory.GetItemsByType<ItemMedicine>();
      if (itemsByType == null)
        return (ActorAction) null;
      BaseAI.ChoiceEval<ItemMedicine> choiceEval = this.Choose<ItemMedicine>(game, itemsByType, (Func<ItemMedicine, bool>) (it => true), (Func<ItemMedicine, float>) (it =>
      {
        int num = 0;
        if (needHP)
          num += factorHealing * it.Healing;
        if (needSTA)
          num += factorStamina * it.StaminaBoost;
        if (needSLP)
          num += factorSleep * it.SleepBoost;
        if (needCure)
          num += factorCure * it.InfectionCure;
        if (needSan)
          num += factorSan * it.SanityCure;
        return (float) num;
      }), (Func<float, float, bool>) ((a, b) => (double) a > (double) b));
      if (choiceEval == null || (double) choiceEval.Value <= 0.0)
        return (ActorAction) null;
      return (ActorAction) new ActionUseItem(this.m_Actor, game, (Item) choiceEval.Choice);
    }

    protected ActorAction BehaviorUseEntertainment(RogueGame game)
    {
      Inventory inventory = this.m_Actor.Inventory;
      if (inventory.IsEmpty)
        return (ActorAction) null;
      ItemEntertainment firstByType = (ItemEntertainment) inventory.GetFirstByType(typeof (ItemEntertainment));
      if (firstByType == null)
        return (ActorAction) null;
      if (!game.Rules.CanActorUseItem(this.m_Actor, (Item) firstByType))
        return (ActorAction) null;
      return (ActorAction) new ActionUseItem(this.m_Actor, game, (Item) firstByType);
    }

    protected ActorAction BehaviorDropBoringEntertainment(RogueGame game)
    {
      Inventory inventory = this.m_Actor.Inventory;
      if (inventory.IsEmpty)
        return (ActorAction) null;
      foreach (Item it in inventory.Items)
      {
        if (it is ItemEntertainment && this.m_Actor.IsBoredOf(it))
          return (ActorAction) new ActionDropItem(this.m_Actor, game, it);
      }
      return (ActorAction) null;
    }

    protected ActorAction BehaviorFollowActor(RogueGame game, Actor other, Point otherPosition, bool isVisible, int maxDist)
    {
      if (other == null || other.IsDead)
        return (ActorAction) null;
      Rules rules1 = game.Rules;
      Location location = this.m_Actor.Location;
      Point position1 = location.Position;
      Point pB = otherPosition;
      int num = rules1.GridDistance(position1, pB);
      if (isVisible && num <= maxDist)
        return (ActorAction) new ActionWait(this.m_Actor, game);
      location = other.Location;
      Map map1 = location.Map;
      location = this.m_Actor.Location;
      Map map2 = location.Map;
      if (map1 != map2)
      {
        location = this.m_Actor.Location;
        Map map3 = location.Map;
        location = this.m_Actor.Location;
        Point position2 = location.Position;
        Exit exitAt = map3.GetExitAt(position2);
        if (exitAt != null)
        {
          Map toMap = exitAt.ToMap;
          location = other.Location;
          Map map4 = location.Map;
          if (toMap == map4)
          {
            Rules rules2 = game.Rules;
            Actor actor1 = this.m_Actor;
            location = this.m_Actor.Location;
            Point position3 = location.Position;
            if (rules2.CanActorUseExit(actor1, position3))
            {
              Actor actor2 = this.m_Actor;
              location = this.m_Actor.Location;
              Point position4 = location.Position;
              RogueGame game1 = game;
              return (ActorAction) new ActionUseExit(actor2, position4, game1);
            }
          }
        }
      }
      ActorAction actorAction = this.BehaviorIntelligentBumpToward(game, otherPosition);
      if (actorAction == null || !actorAction.IsLegal())
        return (ActorAction) null;
      if (other.IsRunning)
        this.RunIfPossible(game.Rules);
      return actorAction;
    }

    protected ActorAction BehaviorHangAroundActor(RogueGame game, Actor other, Point otherPosition, int minDist, int maxDist)
    {
      if (other == null || other.IsDead)
        return (ActorAction) null;
      int num = 0;
      Point p;
      do
      {
        p = otherPosition;
        p.X += game.Rules.Roll(minDist, maxDist + 1) - game.Rules.Roll(minDist, maxDist + 1);
        p.Y += game.Rules.Roll(minDist, maxDist + 1) - game.Rules.Roll(minDist, maxDist + 1);
        this.m_Actor.Location.Map.TrimToBounds(ref p);
      }
      while (game.Rules.GridDistance(p, otherPosition) < minDist && ++num < 100);
      ActorAction a = this.BehaviorIntelligentBumpToward(game, p);
      if (a == null || !this.IsValidMoveTowardGoalAction(a) || !a.IsLegal())
        return (ActorAction) null;
      if (other.IsRunning)
        this.RunIfPossible(game.Rules);
      return a;
    }

    protected ActorAction BehaviorTrackScent(RogueGame game, List<Percept> scents)
    {
      if (scents == null || scents.Count == 0)
        return (ActorAction) null;
      Percept percept = this.FilterStrongestScent(game, scents);
      Map map = this.m_Actor.Location.Map;
      if (!(this.m_Actor.Location.Position == percept.Location.Position))
        return this.BehaviorIntelligentBumpToward(game, percept.Location.Position) ?? (ActorAction) null;
      if (map.GetExitAt(this.m_Actor.Location.Position) != null && this.m_Actor.Model.Abilities.AI_CanUseAIExits)
        return this.BehaviorUseExit(game, BaseAI.UseExitFlags.BREAK_BLOCKING_OBJECTS | BaseAI.UseExitFlags.ATTACK_BLOCKING_ENEMIES);
      return (ActorAction) null;
    }

    protected ActorAction BehaviorChargeEnemy(RogueGame game, Percept target)
    {
      ActorAction actorAction1 = this.BehaviorMeleeAttack(game, target);
      if (actorAction1 != null)
        return actorAction1;
      Actor percepted = target.Percepted as Actor;
      if (game.Rules.IsActorTired(this.m_Actor) && game.Rules.IsAdjacent(this.m_Actor.Location, target.Location))
        return this.BehaviorUseMedecine(game, 0, 1, 0, 0, 0) ?? (ActorAction) new ActionWait(this.m_Actor, game);
      ActorAction actorAction2 = this.BehaviorIntelligentBumpToward(game, target.Location.Position);
      if (actorAction2 == null)
        return (ActorAction) null;
      Attack currentRangedAttack = this.m_Actor.CurrentRangedAttack;
      int range1 = currentRangedAttack.Range;
      currentRangedAttack = percepted.CurrentRangedAttack;
      int range2 = currentRangedAttack.Range;
      if (range1 < range2)
        this.RunIfPossible(game.Rules);
      return actorAction2;
    }

    protected ActorAction BehaviorLeadActor(RogueGame game, Percept target)
    {
      Actor percepted = target.Percepted as Actor;
      if (!game.Rules.CanActorTakeLead(this.m_Actor, percepted))
        return (ActorAction) null;
      if (game.Rules.IsAdjacent(this.m_Actor.Location.Position, percepted.Location.Position))
        return (ActorAction) new ActionTakeLead(this.m_Actor, game, percepted);
      return this.BehaviorIntelligentBumpToward(game, percepted.Location.Position) ?? (ActorAction) null;
    }

    protected ActorAction BehaviorDontLeaveFollowersBehind(RogueGame game, int distance, out Actor target)
    {
      target = (Actor) null;
      int num1 = int.MinValue;
      Map map = this.m_Actor.Location.Map;
      Point position = this.m_Actor.Location.Position;
      int num2 = 0;
      int num3 = this.m_Actor.CountFollowers / 2;
      foreach (Actor follower in this.m_Actor.Followers)
      {
        if (follower.Location.Map == map)
        {
          if (game.Rules.GridDistance(follower.Location.Position, position) <= distance && ++num2 >= num3)
            return (ActorAction) null;
          int num4 = game.Rules.GridDistance(follower.Location.Position, position);
          if (target == null || num4 > num1)
          {
            target = follower;
            num1 = num4;
          }
        }
      }
      if (target == null)
        return (ActorAction) null;
      return this.BehaviorIntelligentBumpToward(game, target.Location.Position);
    }

    protected ActorAction BehaviorFightOrFlee(RogueGame game, List<Percept> enemies, bool hasVisibleLeader, bool isLeaderFighting, ActorCourage courage, string[] emotes)
    {
      Percept target = this.FilterNearest(game, enemies);
      bool flag1 = false;
      Actor enemy = target.Percepted as Actor;
      bool flag2;
      if (this.HasEquipedRangedWeapon(enemy))
        flag2 = false;
      else if (this.m_Actor.Model.Abilities.IsLawEnforcer && enemy.MurdersCounter > 0)
        flag2 = false;
      else if (game.Rules.IsActorTired(this.m_Actor) && game.Rules.IsAdjacent(this.m_Actor.Location, enemy.Location))
        flag2 = true;
      else if (this.m_Actor.Leader != null)
      {
        switch (courage)
        {
          case ActorCourage.COWARD:
            flag2 = true;
            flag1 = true;
            break;
          case ActorCourage.CAUTIOUS:
            flag2 = this.WantToEvadeMelee(game, this.m_Actor, courage, enemy);
            flag1 = !this.HasSpeedAdvantage(game, this.m_Actor, enemy);
            break;
          case ActorCourage.COURAGEOUS:
            if (isLeaderFighting)
            {
              flag2 = false;
              break;
            }
            flag2 = this.WantToEvadeMelee(game, this.m_Actor, courage, enemy);
            flag1 = !this.HasSpeedAdvantage(game, this.m_Actor, enemy);
            break;
          default:
            throw new ArgumentOutOfRangeException("unhandled courage");
        }
      }
      else
      {
        switch (courage)
        {
          case ActorCourage.COWARD:
            flag2 = true;
            flag1 = true;
            break;
          case ActorCourage.CAUTIOUS:
          case ActorCourage.COURAGEOUS:
            flag2 = this.WantToEvadeMelee(game, this.m_Actor, courage, enemy);
            flag1 = !this.HasSpeedAdvantage(game, this.m_Actor, enemy);
            break;
          default:
            throw new ArgumentOutOfRangeException("unhandled courage");
        }
      }
      if (flag2)
      {
        if (this.m_Actor.Model.Abilities.CanTalk && game.Rules.RollChance(30))
          game.DoEmote(this.m_Actor, string.Format("{0} {1}!", (object) emotes[0], (object) enemy.Name));
        if (this.m_Actor.Model.Abilities.CanUseMapObjects)
        {
          BaseAI.ChoiceEval<Direction> choiceEval = this.Choose<Direction>(game, Direction.COMPASS_LIST, (Func<Direction, bool>) (dir =>
          {
            Point position1 = this.m_Actor.Location.Position + dir;
            DoorWindow mapObjectAt = this.m_Actor.Location.Map.GetMapObjectAt(position1) as DoorWindow;
            if (mapObjectAt == null)
              return false;
            RogueGame game1 = game;
            Location location = this.m_Actor.Location;
            Point position2 = location.Position;
            Point between = position1;
            location = enemy.Location;
            Point position3 = location.Position;
            if (!this.IsBetween(game1, position2, between, position3) || !game.Rules.IsClosableFor(this.m_Actor, mapObjectAt))
              return false;
            Rules rules = game.Rules;
            Point pA = position1;
            location = enemy.Location;
            Point position4 = location.Position;
            return rules.GridDistance(pA, position4) != 1 || !game.Rules.IsClosableFor(enemy, mapObjectAt);
          }), (Func<Direction, float>) (dir => (float) game.Rules.Roll(0, 666)), (Func<float, float, bool>) ((a, b) => (double) a > (double) b));
          if (choiceEval != null)
          {
            Actor actor = this.m_Actor;
            RogueGame game1 = game;
            Location location = this.m_Actor.Location;
            Map map = location.Map;
            location = this.m_Actor.Location;
            Point position = location.Position + choiceEval.Choice;
            DoorWindow mapObjectAt = map.GetMapObjectAt(position) as DoorWindow;
            return (ActorAction) new ActionCloseDoor(actor, game1, mapObjectAt);
          }
        }
        if (this.m_Actor.Model.Abilities.CanBarricade)
        {
          BaseAI.ChoiceEval<Direction> choiceEval = this.Choose<Direction>(game, Direction.COMPASS_LIST, (Func<Direction, bool>) (dir =>
          {
            Point position1 = this.m_Actor.Location.Position + dir;
            DoorWindow mapObjectAt = this.m_Actor.Location.Map.GetMapObjectAt(position1) as DoorWindow;
            if (mapObjectAt == null)
              return false;
            RogueGame game1 = game;
            Location location = this.m_Actor.Location;
            Point position2 = location.Position;
            Point between = position1;
            location = enemy.Location;
            Point position3 = location.Position;
            return this.IsBetween(game1, position2, between, position3) && game.Rules.CanActorBarricadeDoor(this.m_Actor, mapObjectAt);
          }), (Func<Direction, float>) (dir => (float) game.Rules.Roll(0, 666)), (Func<float, float, bool>) ((a, b) => (double) a > (double) b));
          if (choiceEval != null)
          {
            Actor actor = this.m_Actor;
            RogueGame game1 = game;
            Location location = this.m_Actor.Location;
            Map map = location.Map;
            location = this.m_Actor.Location;
            Point position = location.Position + choiceEval.Choice;
            DoorWindow mapObjectAt = map.GetMapObjectAt(position) as DoorWindow;
            return (ActorAction) new ActionBarricadeDoor(actor, game1, mapObjectAt);
          }
        }
        if (this.m_Actor.Model.Abilities.AI_CanUseAIExits && game.Rules.RollChance(50))
        {
          ActorAction actorAction = this.BehaviorUseExit(game, BaseAI.UseExitFlags.NONE);
          if (actorAction != null)
          {
            bool flag3 = true;
            if (this.m_Actor.HasLeader)
            {
              Location location = this.m_Actor.Location;
              Map map = location.Map;
              location = this.m_Actor.Location;
              Point position = location.Position;
              Exit exitAt = map.GetExitAt(position);
              if (exitAt != null)
              {
                location = this.m_Actor.Leader.Location;
                flag3 = location.Map == exitAt.ToMap;
              }
            }
            if (flag3)
            {
              this.m_Actor.Activity = Activity.FLEEING;
              return actorAction;
            }
          }
        }
        if (!(enemy.GetEquippedWeapon() is ItemRangedWeapon) && !game.Rules.IsAdjacent(this.m_Actor.Location, enemy.Location))
        {
          ActorAction actorAction = this.BehaviorUseMedecine(game, 2, 2, 1, 0, 0);
          if (actorAction != null)
          {
            this.m_Actor.Activity = Activity.FLEEING;
            return actorAction;
          }
        }
        ActorAction actorAction1 = this.BehaviorWalkAwayFrom(game, enemies);
        if (actorAction1 != null)
        {
          if (flag1)
            this.RunIfPossible(game.Rules);
          this.m_Actor.Activity = Activity.FLEEING;
          return actorAction1;
        }
        if (actorAction1 == null && this.IsAdjacentToEnemy(game, enemy))
        {
          if (this.m_Actor.Model.Abilities.CanTalk && game.Rules.RollChance(50))
            game.DoEmote(this.m_Actor, emotes[1]);
          return this.BehaviorMeleeAttack(game, target);
        }
      }
      else
      {
        ActorAction actorAction = this.BehaviorChargeEnemy(game, target);
        if (actorAction != null)
        {
          if (this.m_Actor.Model.Abilities.CanTalk && game.Rules.RollChance(30))
            game.DoEmote(this.m_Actor, string.Format("{0} {1}!", (object) emotes[2], (object) enemy.Name));
          this.m_Actor.Activity = Activity.FIGHTING;
          this.m_Actor.TargetActor = target.Percepted as Actor;
          return actorAction;
        }
      }
      return (ActorAction) null;
    }

    protected ActorAction BehaviorWarnFriends(RogueGame game, List<Percept> friends, Actor nearestEnemy)
    {
      if (game.Rules.IsAdjacent(this.m_Actor.Location, nearestEnemy.Location))
        return (ActorAction) null;
      if (this.m_Actor.HasLeader && this.m_Actor.Leader.IsSleeping)
        return (ActorAction) new ActionShout(this.m_Actor, game);
      foreach (Percept friend in friends)
      {
        Actor percepted = friend.Percepted as Actor;
        if (percepted == null)
          throw new ArgumentException("percept not an actor");
        if (percepted != null && percepted != this.m_Actor && (percepted.IsSleeping && !game.Rules.IsEnemyOf(this.m_Actor, percepted)) && game.Rules.IsEnemyOf(percepted, nearestEnemy))
        {
          string text = nearestEnemy == null ? string.Format("Wake up {0}!", (object) percepted.Name) : string.Format("Wake up {0}! {1} sighted!", (object) percepted.Name, (object) nearestEnemy.Name);
          return (ActorAction) new ActionShout(this.m_Actor, game, text);
        }
      }
      return (ActorAction) null;
    }

    protected ActorAction BehaviorTellFriendAboutPercept(RogueGame game, Percept percept)
    {
      Map map = this.m_Actor.Location.Map;
      List<Point> pointList = map.FilterAdjacentInMap(this.m_Actor.Location.Position, (Predicate<Point>) (pt =>
      {
        Actor actorAt = map.GetActorAt(pt);
        return actorAt != null && !actorAt.IsSleeping && !game.Rules.IsEnemyOf(this.m_Actor, actorAt);
      }));
      if (pointList == null || pointList.Count == 0)
        return (ActorAction) null;
      Actor actorAt1 = map.GetActorAt(pointList[game.Rules.Roll(0, pointList.Count)]);
      string str1 = this.MakeCentricLocationDirection(game, this.m_Actor.Location, percept.Location);
      string format = "{0} ago";
      Location location = this.m_Actor.Location;
      string str2 = WorldTime.MakeTimeDurationMessage(location.Map.LocalTime.TurnCounter - percept.Turn);
      string str3 = string.Format(format, (object) str2);
      string text;
      if (percept.Percepted is Actor)
        text = string.Format("I saw {0} {1} {2}.", (object) (percept.Percepted as Actor).Name, (object) str1, (object) str3);
      else if (percept.Percepted is Inventory)
      {
        Inventory percepted = percept.Percepted as Inventory;
        if (percepted.IsEmpty)
          return (ActorAction) null;
        Item it = percepted[game.Rules.Roll(0, percepted.CountItems)];
        if (!this.IsItemWorthTellingAbout(it))
          return (ActorAction) null;
        int num = game.Rules.ActorFOV(actorAt1, map.LocalTime, game.Session.World.Weather);
        location = percept.Location;
        Map map1 = location.Map;
        location = actorAt1.Location;
        Map map2 = location.Map;
        if (map1 == map2)
        {
          Rules rules = game.Rules;
          location = percept.Location;
          Point position1 = location.Position;
          location = actorAt1.Location;
          Point position2 = location.Position;
          if ((double) rules.StdDistance(position1, position2) <= (double) (2 + num))
            return (ActorAction) null;
        }
        text = string.Format("I saw {0} {1} {2}.", (object) it.AName, (object) str1, (object) str3);
      }
      else
      {
        if (!(percept.Percepted is string))
          throw new InvalidOperationException("unhandled percept.Percepted type");
        text = string.Format("I heard {0} {1} {2}!", (object) (percept.Percepted as string), (object) str1, (object) str3);
      }
      ActionSay actionSay = new ActionSay(this.m_Actor, game, actorAt1, text, RogueGame.Sayflags.NONE);
      if (actionSay.IsLegal())
        return (ActorAction) actionSay;
      return (ActorAction) null;
    }

    protected ActorAction BehaviorExplore(RogueGame game, ExplorationData exploration)
    {
      Point position1 = this.m_Actor.Location.Position;
      int x1 = position1.X;
      position1 = this.m_prevLocation.Position;
      int x2 = position1.X;
      int vx = x1 - x2;
      position1 = this.m_Actor.Location.Position;
      int y1 = position1.Y;
      position1 = this.m_prevLocation.Position;
      int y2 = position1.Y;
      int vy = y1 - y2;
      Direction prevDirection = Direction.FromVector(vx, vy);
      bool imStarvingOrCourageous = game.Rules.IsActorStarving(this.m_Actor) || this.Directives.Courage == ActorCourage.COURAGEOUS;
      BaseAI.ChoiceEval<Direction> choiceEval = this.Choose<Direction>(game, Direction.COMPASS_LIST, (Func<Direction, bool>) (dir =>
      {
        Location location = this.m_Actor.Location + dir;
        if (exploration.HasExplored(location))
          return false;
        return this.IsValidMoveTowardGoalAction(game.Rules.IsBumpableFor(this.m_Actor, game, location));
      }), (Func<Direction, float>) (dir =>
      {
        Location loc = this.m_Actor.Location + dir;
        Map map = loc.Map;
        Point position2 = loc.Position;
        if (this.m_Actor.Model.Abilities.IsIntelligent && !imStarvingOrCourageous && this.ComputeTrapsMaxDamage(map, position2) >= this.m_Actor.HitPoints)
          return float.NaN;
        int num = 0;
        if (!exploration.HasExplored(map.GetZonesAt(position2.X, position2.Y)))
          num += 1000;
        if (!exploration.HasExplored(loc))
          num += 500;
        MapObject mapObjectAt = map.GetMapObjectAt(position2);
        if (mapObjectAt != null && (mapObjectAt.IsMovable || mapObjectAt is DoorWindow))
          num += 100;
        if (BaseAI.IsAnyActivatedTrapThere(map, position2))
          num += -50;
        if (map.GetTileAt(position2.X, position2.Y).IsInside)
        {
          if (map.LocalTime.IsNight)
            num += 50;
        }
        else if (!map.LocalTime.IsNight)
          num += 50;
        if (dir == prevDirection)
          num += 25;
        return (float) (num + game.Rules.Roll(0, 10));
      }), (Func<float, float, bool>) ((a, b) =>
      {
        if (!float.IsNaN(a))
          return (double) a > (double) b;
        return false;
      }));
      if (choiceEval != null)
        return (ActorAction) new ActionBump(this.m_Actor, game, choiceEval.Choice);
      return (ActorAction) null;
    }

    protected ActorAction BehaviorCloseDoorBehindMe(RogueGame game, Location previousLocation)
    {
      DoorWindow mapObjectAt = previousLocation.Map.GetMapObjectAt(previousLocation.Position) as DoorWindow;
      if (mapObjectAt == null)
        return (ActorAction) null;
      if (game.Rules.IsClosableFor(this.m_Actor, mapObjectAt))
        return (ActorAction) new ActionCloseDoor(this.m_Actor, game, mapObjectAt);
      return (ActorAction) null;
    }

    protected ActorAction BehaviorSecurePerimeter(RogueGame game, HashSet<Point> fov)
    {
      Map map = this.m_Actor.Location.Map;
      foreach (Point position in fov)
      {
        MapObject mapObjectAt = map.GetMapObjectAt(position);
        if (mapObjectAt != null)
        {
          DoorWindow door = mapObjectAt as DoorWindow;
          if (door != null)
          {
            if (door.IsOpen && game.Rules.IsClosableFor(this.m_Actor, door))
            {
              if (game.Rules.IsAdjacent(door.Location.Position, this.m_Actor.Location.Position))
                return (ActorAction) new ActionCloseDoor(this.m_Actor, game, door);
              return this.BehaviorIntelligentBumpToward(game, door.Location.Position);
            }
            if (door.IsWindow && !door.IsBarricaded && game.Rules.CanActorBarricadeDoor(this.m_Actor, door))
            {
              if (game.Rules.IsAdjacent(door.Location.Position, this.m_Actor.Location.Position))
                return (ActorAction) new ActionBarricadeDoor(this.m_Actor, game, door);
              return this.BehaviorIntelligentBumpToward(game, door.Location.Position);
            }
          }
        }
      }
      return (ActorAction) null;
    }

    protected ActorAction BehaviorUseExit(RogueGame game, BaseAI.UseExitFlags useFlags)
    {
      Location location = this.m_Actor.Location;
      Map map = location.Map;
      location = this.m_Actor.Location;
      Point position = location.Position;
      Exit exitAt = map.GetExitAt(position);
      if (exitAt == null)
        return (ActorAction) null;
      if (!exitAt.IsAnAIExit)
        return (ActorAction) null;
      if ((useFlags & BaseAI.UseExitFlags.DONT_BACKTRACK) != BaseAI.UseExitFlags.NONE && exitAt.ToMap == this.m_prevLocation.Map && exitAt.ToPosition == this.m_prevLocation.Position)
        return (ActorAction) null;
      if ((useFlags & BaseAI.UseExitFlags.ATTACK_BLOCKING_ENEMIES) != BaseAI.UseExitFlags.NONE)
      {
        Actor actorAt = exitAt.ToMap.GetActorAt(exitAt.ToPosition);
        if (actorAt != null && game.Rules.IsEnemyOf(this.m_Actor, actorAt) && game.Rules.CanActorMeleeAttack(this.m_Actor, actorAt))
          return (ActorAction) new ActionMeleeAttack(this.m_Actor, game, actorAt);
      }
      if ((useFlags & BaseAI.UseExitFlags.BREAK_BLOCKING_OBJECTS) != BaseAI.UseExitFlags.NONE)
      {
        MapObject mapObjectAt = exitAt.ToMap.GetMapObjectAt(exitAt.ToPosition);
        if (mapObjectAt != null && game.Rules.IsBreakableFor(this.m_Actor, mapObjectAt))
          return (ActorAction) new ActionBreak(this.m_Actor, game, mapObjectAt);
      }
      if (!game.Rules.CanActorUseExit(this.m_Actor, this.m_Actor.Location.Position))
        return (ActorAction) null;
      return (ActorAction) new ActionUseExit(this.m_Actor, this.m_Actor.Location.Position, game);
    }

    protected ActorAction BehaviorFleeFromExplosives(RogueGame game, List<Percept> itemStacks)
    {
      if (itemStacks == null || itemStacks.Count == 0)
        return (ActorAction) null;
      List<Percept> goals = this.Filter(game, itemStacks, (Predicate<Percept>) (p =>
      {
        Inventory percepted = p.Percepted as Inventory;
        if (percepted == null || percepted.IsEmpty)
          return false;
        foreach (Item obj in percepted.Items)
        {
          if (obj is ItemPrimedExplosive)
            return true;
        }
        return false;
      }));
      if (goals == null || goals.Count == 0)
        return (ActorAction) null;
      ActorAction actorAction = this.BehaviorWalkAwayFrom(game, goals);
      if (actorAction == null)
        return (ActorAction) null;
      this.RunIfPossible(game.Rules);
      return actorAction;
    }

    protected ActorAction BehaviorThrowGrenade(RogueGame game, HashSet<Point> fov, List<Percept> enemies)
    {
      if (enemies == null || enemies.Count == 0)
        return (ActorAction) null;
      if (enemies.Count < 3)
        return (ActorAction) null;
      Inventory inventory = this.m_Actor.Inventory;
      if (inventory == null || inventory.IsEmpty)
        return (ActorAction) null;
      ItemGrenade firstGrenade = this.GetFirstGrenade((Predicate<Item>) (it => !this.IsItemTaboo(it)));
      if (firstGrenade == null)
        return (ActorAction) null;
      ItemGrenadeModel model = firstGrenade.Model as ItemGrenadeModel;
      int maxRange = game.Rules.ActorMaxThrowRange(this.m_Actor, model.MaxThrowDistance);
      Point? nullable = new Point?();
      int num1 = 0;
      foreach (Point toPosition in fov)
      {
        Rules rules1 = game.Rules;
        Location location = this.m_Actor.Location;
        Point position1 = location.Position;
        Point pB1 = toPosition;
        int num2 = rules1.GridDistance(position1, pB1);
        BlastAttack blastAttack = model.BlastAttack;
        int radius1 = blastAttack.Radius;
        if (num2 > radius1)
        {
          Rules rules2 = game.Rules;
          location = this.m_Actor.Location;
          Point position2 = location.Position;
          Point pB2 = toPosition;
          if (rules2.GridDistance(position2, pB2) <= maxRange && LOS.CanTraceThrowLine(this.m_Actor.Location, toPosition, maxRange, (List<Point>) null))
          {
            int num3 = 0;
            int x1 = toPosition.X;
            blastAttack = model.BlastAttack;
            int radius2 = blastAttack.Radius;
            int x2 = x1 - radius2;
            while (true)
            {
              int num4 = x2;
              int x3 = toPosition.X;
              blastAttack = model.BlastAttack;
              int radius3 = blastAttack.Radius;
              int num5 = x3 + radius3;
              if (num4 <= num5)
              {
                int y1 = toPosition.Y;
                blastAttack = model.BlastAttack;
                int radius4 = blastAttack.Radius;
                int y2 = y1 - radius4;
                while (true)
                {
                  int num6 = y2;
                  int y3 = toPosition.Y;
                  blastAttack = model.BlastAttack;
                  int radius5 = blastAttack.Radius;
                  int num7 = y3 + radius5;
                  if (num6 <= num7)
                  {
                    location = this.m_Actor.Location;
                    if (location.Map.IsInBounds(x2, y2))
                    {
                      location = this.m_Actor.Location;
                      Actor actorAt = location.Map.GetActorAt(x2, y2);
                      if (actorAt != null && actorAt != this.m_Actor)
                      {
                        Rules rules3 = game.Rules;
                        Point pA = toPosition;
                        location = actorAt.Location;
                        Point position3 = location.Position;
                        int distance = rules3.GridDistance(pA, position3);
                        int num8 = distance;
                        blastAttack = model.BlastAttack;
                        int radius6 = blastAttack.Radius;
                        if (num8 <= radius6)
                        {
                          if (game.Rules.IsEnemyOf(this.m_Actor, actorAt))
                          {
                            int num9 = game.Rules.BlastDamage(distance, model.BlastAttack) * game.Rules.ActorMaxHPs(actorAt);
                            num3 += num9;
                          }
                          else
                            break;
                        }
                      }
                    }
                    ++y2;
                  }
                  else
                    goto label_22;
                }
                num3 = -1;
label_22:
                ++x2;
              }
              else
                break;
            }
            if (num3 > 0 && (!nullable.HasValue || num3 > num1))
            {
              nullable = new Point?(toPosition);
              num1 = num3;
            }
          }
        }
      }
      if (!nullable.HasValue)
        return (ActorAction) null;
      if (!firstGrenade.IsEquipped)
      {
        Item equippedWeapon = this.m_Actor.GetEquippedWeapon();
        if (equippedWeapon != null)
          return (ActorAction) new ActionUnequipItem(this.m_Actor, game, equippedWeapon);
        return (ActorAction) new ActionEquipItem(this.m_Actor, game, (Item) firstGrenade);
      }
      ActorAction actorAction = (ActorAction) new ActionThrowGrenade(this.m_Actor, game, nullable.Value);
      if (!actorAction.IsLegal())
        return (ActorAction) null;
      return actorAction;
    }

    protected ActorAction BehaviorMakeRoomForFood(RogueGame game, List<Percept> stacks)
    {
      if (stacks == null || stacks.Count == 0)
        return (ActorAction) null;
      if (this.m_Actor.Inventory.CountItems < game.Rules.ActorMaxInv(this.m_Actor))
        return (ActorAction) null;
      if (this.HasItemOfType(typeof (ItemFood)))
        return (ActorAction) null;
      bool flag = false;
      foreach (Percept stack in stacks)
      {
        Inventory percepted = stack.Percepted as Inventory;
        if (percepted != null && percepted.HasItemOfType(typeof (ItemFood)))
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        return (ActorAction) null;
      Inventory inventory = this.m_Actor.Inventory;
      Item firstMatching1 = inventory.GetFirstMatching((Predicate<Item>) (it => !this.IsInterestingItem(game, it)));
      if (firstMatching1 != null)
        return this.BehaviorDropItem(game, firstMatching1);
      Item firstMatching2 = inventory.GetFirstMatching((Predicate<Item>) (it => it is ItemBarricadeMaterial));
      if (firstMatching2 != null)
        return this.BehaviorDropItem(game, firstMatching2);
      Item firstMatching3 = inventory.GetFirstMatching((Predicate<Item>) (it => it is ItemLight));
      if (firstMatching3 != null)
        return this.BehaviorDropItem(game, firstMatching3);
      Item firstMatching4 = inventory.GetFirstMatching((Predicate<Item>) (it => it is ItemSprayPaint));
      if (firstMatching4 != null)
        return this.BehaviorDropItem(game, firstMatching4);
      Item firstMatching5 = inventory.GetFirstMatching((Predicate<Item>) (it => it is ItemSprayScent));
      if (firstMatching5 != null)
        return this.BehaviorDropItem(game, firstMatching5);
      Item firstMatching6 = inventory.GetFirstMatching((Predicate<Item>) (it => it is ItemAmmo));
      if (firstMatching6 != null)
        return this.BehaviorDropItem(game, firstMatching6);
      Item firstMatching7 = inventory.GetFirstMatching((Predicate<Item>) (it => it is ItemMedicine));
      if (firstMatching7 != null)
        return this.BehaviorDropItem(game, firstMatching7);
      Item it1 = inventory[game.Rules.Roll(0, inventory.CountItems)];
      return this.BehaviorDropItem(game, it1);
    }

    protected ActorAction BehaviorUseStenchKiller(RogueGame game)
    {
      ItemSprayScent equippedItem = this.m_Actor.GetEquippedItem(DollPart.LEFT_HAND) as ItemSprayScent;
      if (equippedItem == null)
        return (ActorAction) null;
      if (equippedItem.SprayQuantity <= 0)
        return (ActorAction) null;
      if ((equippedItem.Model as ItemSprayScentModel).Odor != Odor.PERFUME_LIVING_SUPRESSOR)
        return (ActorAction) null;
      if (!this.IsGoodStenchKillerSpot(game, this.m_Actor.Location.Map, this.m_Actor.Location.Position))
        return (ActorAction) null;
      ActionUseItem actionUseItem = new ActionUseItem(this.m_Actor, game, (Item) equippedItem);
      if (actionUseItem.IsLegal())
        return (ActorAction) actionUseItem;
      return (ActorAction) null;
    }

    protected bool IsGoodStenchKillerSpot(RogueGame game, Map map, Point pos)
    {
      if (map.GetScentByOdorAt(Odor.PERFUME_LIVING_SUPRESSOR, pos) > 0)
        return false;
      if (this.m_prevLocation.Map.GetTileAt(this.m_prevLocation.Position).IsInside != map.GetTileAt(pos).IsInside)
        return true;
      MapObject mapObjectAt = map.GetMapObjectAt(pos);
      return mapObjectAt != null && mapObjectAt is DoorWindow || map.GetExitAt(pos) != null;
    }

    protected ActorAction BehaviorEnforceLaw(RogueGame game, List<Percept> percepts, out Actor target)
    {
      target = (Actor) null;
      if (!this.m_Actor.Model.Abilities.IsLawEnforcer)
        return (ActorAction) null;
      if (percepts == null)
        return (ActorAction) null;
      List<Percept> percepts1 = this.FilterActors(game, percepts, (Predicate<Actor>) (a =>
      {
        if (a.MurdersCounter > 0)
          return !game.Rules.IsEnemyOf(this.m_Actor, a);
        return false;
      }));
      if (percepts1 == null || percepts1.Count == 0)
        return (ActorAction) null;
      Percept percept = this.FilterNearest(game, percepts1);
      target = percept.Percepted as Actor;
      if (game.Rules.RollChance(game.Rules.ActorUnsuspicousChance(this.m_Actor, target)))
      {
        game.DoEmote(target, string.Format("moves unnoticed by {0}.", (object) this.m_Actor.Name));
        return (ActorAction) null;
      }
      game.DoEmote(this.m_Actor, string.Format("takes a closer look at {0}.", (object) target.Name));
      int chance = game.Rules.ActorSpotMurdererChance(this.m_Actor, target);
      if (!game.Rules.RollChance(chance))
        return (ActorAction) null;
      game.DoMakeAggression(this.m_Actor, target);
      return (ActorAction) new ActionSay(this.m_Actor, game, target, string.Format("HEY! YOU ARE WANTED FOR {0} MURDER{1}!", (object) target.MurdersCounter, target.MurdersCounter > 1 ? (object) "s" : (object) ""), RogueGame.Sayflags.IS_IMPORTANT);
    }

    protected ActorAction BehaviorGoEatFoodOnGround(RogueGame game, List<Percept> stacksPercepts)
    {
      if (stacksPercepts == null)
        return (ActorAction) null;
      List<Percept> percepts = this.Filter(game, stacksPercepts, (Predicate<Percept>) (p => (p.Percepted as Inventory).HasItemOfType(typeof (ItemFood))));
      if (percepts == null)
        return (ActorAction) null;
      Location location = this.m_Actor.Location;
      Map map = location.Map;
      location = this.m_Actor.Location;
      Point position1 = location.Position;
      Inventory itemsAt = map.GetItemsAt(position1);
      if (itemsAt != null && itemsAt.HasItemOfType(typeof (ItemFood)))
      {
        Item firstByType = itemsAt.GetFirstByType(typeof (ItemFood));
        return (ActorAction) new ActionEatFoodOnGround(this.m_Actor, game, firstByType);
      }
      Percept percept = this.FilterNearest(game, percepts);
      RogueGame game1 = game;
      location = percept.Location;
      Point position2 = location.Position;
      return this.BehaviorStupidBumpToward(game1, position2);
    }

    protected ActorAction BehaviorGoEatCorpse(RogueGame game, List<Percept> corpsesPercepts)
    {
      if (corpsesPercepts == null)
        return (ActorAction) null;
      if (this.m_Actor.Model.Abilities.IsUndead && this.m_Actor.HitPoints >= game.Rules.ActorMaxHPs(this.m_Actor))
        return (ActorAction) null;
      Location location = this.m_Actor.Location;
      Map map = location.Map;
      location = this.m_Actor.Location;
      Point position1 = location.Position;
      List<Corpse> corpsesAt = map.GetCorpsesAt(position1);
      if (corpsesAt != null)
      {
        Corpse corpse = corpsesAt[0];
        if (game.Rules.CanActorEatCorpse(this.m_Actor, corpse))
          return (ActorAction) new ActionEatCorpse(this.m_Actor, game, corpse);
      }
      Percept percept = this.FilterNearest(game, corpsesPercepts);
      if (!this.m_Actor.Model.Abilities.IsIntelligent)
      {
        RogueGame game1 = game;
        location = percept.Location;
        Point position2 = location.Position;
        return this.BehaviorStupidBumpToward(game1, position2);
      }
      RogueGame game2 = game;
      location = percept.Location;
      Point position3 = location.Position;
      return this.BehaviorIntelligentBumpToward(game2, position3);
    }

    protected ActorAction BehaviorGoReviveCorpse(RogueGame game, List<Percept> corpsesPercepts)
    {
      if (corpsesPercepts == null)
        return (ActorAction) null;
      if (this.m_Actor.Sheet.SkillTable.GetSkillLevel(14) == 0)
        return (ActorAction) null;
      if (!this.HasItemOfModel((ItemModel) game.GameItems.MEDIKIT))
        return (ActorAction) null;
      List<Percept> percepts = this.Filter(game, corpsesPercepts, (Predicate<Percept>) (p =>
      {
        foreach (Corpse corpse in p.Percepted as List<Corpse>)
        {
          if (game.Rules.CanActorReviveCorpse(this.m_Actor, corpse) && !game.Rules.IsEnemyOf(this.m_Actor, corpse.DeadGuy))
            return true;
        }
        return false;
      }));
      if (percepts == null)
        return (ActorAction) null;
      Location location = this.m_Actor.Location;
      Map map = location.Map;
      location = this.m_Actor.Location;
      Point position1 = location.Position;
      List<Corpse> corpsesAt = map.GetCorpsesAt(position1);
      if (corpsesAt != null)
      {
        foreach (Corpse corpse in corpsesAt)
        {
          if (game.Rules.CanActorReviveCorpse(this.m_Actor, corpse) && !game.Rules.IsEnemyOf(this.m_Actor, corpse.DeadGuy))
            return (ActorAction) new ActionReviveCorpse(this.m_Actor, game, corpse);
        }
      }
      Percept percept = this.FilterNearest(game, percepts);
      if (!this.m_Actor.Model.Abilities.IsIntelligent)
      {
        RogueGame game1 = game;
        location = percept.Location;
        Point position2 = location.Position;
        return this.BehaviorStupidBumpToward(game1, position2);
      }
      RogueGame game2 = game;
      location = percept.Location;
      Point position3 = location.Position;
      return this.BehaviorIntelligentBumpToward(game2, position3);
    }

    private string MakeCentricLocationDirection(RogueGame game, Location from, Location to)
    {
      if (from.Map != to.Map)
        return string.Format("in {0}", (object) to.Map.Name);
      Point position1 = from.Position;
      Point position2 = to.Position;
      Point v = new Point(position2.X - position1.X, position2.Y - position1.Y);
      return string.Format("{0} tiles to the {1}", (object) (int) game.Rules.StdDistance(v), (object) Direction.ApproximateFromVector(v));
    }

    protected bool IsItemWorthTellingAbout(Item it)
    {
      return it != null && !(it is ItemBarricadeMaterial) && (this.m_Actor.Inventory == null || this.m_Actor.Inventory.IsEmpty || !this.m_Actor.Inventory.Contains(it));
    }

    protected Item GetEquippedWeapon()
    {
      if (this.m_Actor.Inventory == null || this.m_Actor.Inventory.IsEmpty)
        return (Item) null;
      foreach (Item obj in this.m_Actor.Inventory.Items)
      {
        if (obj.IsEquipped && obj is ItemWeapon)
          return obj;
      }
      return (Item) null;
    }

    protected Item GetBestRangedWeaponWithAmmo(Predicate<Item> fn)
    {
      if (this.m_Actor.Inventory == null || this.m_Actor.Inventory.IsEmpty)
        return (Item) null;
      Item obj1 = (Item) null;
      int num1 = 0;
      foreach (Item obj2 in this.m_Actor.Inventory.Items)
      {
        ItemRangedWeapon w = obj2 as ItemRangedWeapon;
        if (w != null && (fn == null || fn(obj2)))
        {
          bool flag = false;
          if (w.Ammo > 0)
          {
            flag = true;
          }
          else
          {
            foreach (Item obj3 in this.m_Actor.Inventory.Items)
            {
              if (obj3 is ItemAmmo && (fn == null || fn(obj3)) && (obj3 as ItemAmmo).AmmoType == w.AmmoType)
              {
                flag = true;
                break;
              }
            }
          }
          if (flag)
          {
            int num2 = this.ScoreRangedWeapon(w);
            if (obj1 == null || num2 > num1)
            {
              obj1 = (Item) w;
              num1 = num2;
            }
          }
        }
      }
      return obj1;
    }

    protected int ScoreRangedWeapon(ItemRangedWeapon w)
    {
      ItemRangedWeaponModel model = w.Model as ItemRangedWeaponModel;
      return 1000 * model.Attack.Range + model.Attack.DamageValue;
    }

    protected Item GetFirstMeleeWeapon(Predicate<Item> fn)
    {
      if (this.m_Actor.Inventory == null || this.m_Actor.Inventory.IsEmpty)
        return (Item) null;
      foreach (Item obj in this.m_Actor.Inventory.Items)
      {
        if (obj is ItemMeleeWeapon && (fn == null || fn(obj)))
          return obj;
      }
      return (Item) null;
    }

    protected Item GetFirstBodyArmor(Predicate<Item> fn)
    {
      if (this.m_Actor.Inventory == null || this.m_Actor.Inventory.IsEmpty)
        return (Item) null;
      foreach (Item obj in this.m_Actor.Inventory.Items)
      {
        if (obj is ItemBodyArmor && (fn == null || fn(obj)))
          return obj;
      }
      return (Item) null;
    }

    protected ItemGrenade GetFirstGrenade(Predicate<Item> fn)
    {
      if (this.m_Actor.Inventory == null || this.m_Actor.Inventory.IsEmpty)
        return (ItemGrenade) null;
      foreach (Item obj in this.m_Actor.Inventory.Items)
      {
        if (obj is ItemGrenade && (fn == null || fn(obj)))
          return obj as ItemGrenade;
      }
      return (ItemGrenade) null;
    }

    protected Item GetEquippedBodyArmor()
    {
      if (this.m_Actor.Inventory == null || this.m_Actor.Inventory.IsEmpty)
        return (Item) null;
      foreach (Item obj in this.m_Actor.Inventory.Items)
      {
        if (obj.IsEquipped && obj is ItemBodyArmor)
          return obj;
      }
      return (Item) null;
    }

    protected Item GetEquippedCellPhone()
    {
      if (this.m_Actor.Inventory == null || this.m_Actor.Inventory.IsEmpty)
        return (Item) null;
      foreach (Item obj in this.m_Actor.Inventory.Items)
      {
        if (obj.IsEquipped && obj is ItemTracker && (obj as ItemTracker).CanTrackFollowersOrLeader)
          return obj;
      }
      return (Item) null;
    }

    protected Item GetFirstTracker(Predicate<ItemTracker> fn)
    {
      if (this.m_Actor.Inventory == null || this.m_Actor.Inventory.IsEmpty)
        return (Item) null;
      foreach (Item obj in this.m_Actor.Inventory.Items)
      {
        ItemTracker itemTracker = obj as ItemTracker;
        if (itemTracker != null && (fn == null || fn(itemTracker)))
          return obj;
      }
      return (Item) null;
    }

    protected Item GetEquippedLight()
    {
      if (this.m_Actor.Inventory == null || this.m_Actor.Inventory.IsEmpty)
        return (Item) null;
      foreach (Item obj in this.m_Actor.Inventory.Items)
      {
        if (obj.IsEquipped && obj is ItemLight)
          return obj;
      }
      return (Item) null;
    }

    protected Item GetFirstLight(Predicate<Item> fn)
    {
      if (this.m_Actor.Inventory == null || this.m_Actor.Inventory.IsEmpty)
        return (Item) null;
      foreach (Item obj in this.m_Actor.Inventory.Items)
      {
        if (obj is ItemLight && (fn == null || fn(obj)))
          return obj;
      }
      return (Item) null;
    }

    protected ItemSprayScent GetEquippedStenchKiller()
    {
      if (this.m_Actor.Inventory == null || this.m_Actor.Inventory.IsEmpty)
        return (ItemSprayScent) null;
      foreach (Item obj in this.m_Actor.Inventory.Items)
      {
        if (obj.IsEquipped && obj is ItemSprayScent && ((obj as ItemSprayScent).Model as ItemSprayScentModel).Odor == Odor.PERFUME_LIVING_SUPRESSOR)
          return obj as ItemSprayScent;
      }
      return (ItemSprayScent) null;
    }

    protected ItemSprayScent GetFirstStenchKiller(Predicate<ItemSprayScent> fn)
    {
      if (this.m_Actor.Inventory == null || this.m_Actor.Inventory.IsEmpty)
        return (ItemSprayScent) null;
      foreach (Item obj in this.m_Actor.Inventory.Items)
      {
        if (obj is ItemSprayScent && (fn == null || fn(obj as ItemSprayScent)))
          return obj as ItemSprayScent;
      }
      return (ItemSprayScent) null;
    }

    protected bool IsRangedWeaponOutOfAmmo(Item it)
    {
      ItemRangedWeapon itemRangedWeapon = it as ItemRangedWeapon;
      if (itemRangedWeapon == null)
        return false;
      return itemRangedWeapon.Ammo <= 0;
    }

    protected bool IsLightOutOfBatteries(Item it)
    {
      ItemLight itemLight = it as ItemLight;
      if (itemLight == null)
        return false;
      return itemLight.Batteries <= 0;
    }

    protected Item GetBestEdibleItem(RogueGame game)
    {
      if (this.m_Actor.Inventory == null || this.m_Actor.Inventory.IsEmpty)
        return (Item) null;
      int turnCounter = this.m_Actor.Location.Map.LocalTime.TurnCounter;
      int num1 = game.Rules.ActorMaxFood(this.m_Actor) - this.m_Actor.FoodPoints;
      Item obj1 = (Item) null;
      int num2 = int.MinValue;
      foreach (Item obj2 in this.m_Actor.Inventory.Items)
      {
        ItemFood food = obj2 as ItemFood;
        if (food != null)
        {
          int num3 = 0;
          int num4 = game.Rules.FoodItemNutrition(food, turnCounter);
          int num5 = num4 - num1;
          if (num5 > 0)
            num3 -= num5;
          if (!food.IsPerishable)
            num3 -= num4;
          if (obj1 == null || num3 > num2)
          {
            obj1 = (Item) food;
            num2 = num3;
          }
        }
      }
      return obj1;
    }

    public bool IsInterestingItem(RogueGame game, Item it)
    {
      if (this.m_Actor.Inventory.CountItems == game.Rules.ActorMaxInv(this.m_Actor) - 1)
        return it is ItemFood;
      if (it.IsForbiddenToAI || it is ItemSprayPaint || it is ItemTrap && (it as ItemTrap).IsActivated)
        return false;
      if (it is ItemFood)
      {
        if (game.Rules.IsActorHungry(this.m_Actor))
          return true;
        if (!this.HasEnoughFoodFor(game, this.m_Actor.Sheet.BaseFoodPoints / 2))
          return !game.Rules.IsFoodSpoiled(it as ItemFood, this.m_Actor.Location.Map.LocalTime.TurnCounter);
        return false;
      }
      if (it is ItemRangedWeapon)
      {
        if (this.m_Actor.Model.Abilities.AI_NotInterestedInRangedWeapons)
          return false;
        ItemRangedWeapon rw = it as ItemRangedWeapon;
        return (rw.Ammo > 0 || this.GetCompatibleAmmoItem(game, rw) != null) && this.CountItemsOfSameType(typeof (ItemRangedWeapon)) < 1 && (this.m_Actor.Inventory.Contains(it) || !this.HasItemOfModel(it.Model));
      }
      if (it is ItemAmmo)
      {
        ItemAmmo am = it as ItemAmmo;
        if (this.GetCompatibleRangedWeapon(game, am) == null)
          return false;
        return !this.HasAtLeastFullStackOfItemTypeOrModel(it, 2);
      }
      if (it is ItemMeleeWeapon)
      {
        if (this.m_Actor.Sheet.SkillTable.GetSkillLevel(13) > 0)
          return false;
        return this.CountItemQuantityOfType(typeof (ItemMeleeWeapon)) < 2;
      }
      if (it is ItemMedicine)
        return !this.HasAtLeastFullStackOfItemTypeOrModel(it, 2);
      if (this.IsLightOutOfBatteries(it) || it is ItemPrimedExplosive || this.m_Actor.IsBoredOf(it))
        return false;
      return !this.HasAtLeastFullStackOfItemTypeOrModel(it, 1);
    }

    public bool HasAnyInterestingItem(RogueGame game, Inventory inv)
    {
      if (inv == null)
        return false;
      foreach (Item it in inv.Items)
      {
        if (this.IsInterestingItem(game, it))
          return true;
      }
      return false;
    }

    protected Item FirstInterestingItem(RogueGame game, Inventory inv)
    {
      if (inv == null)
        return (Item) null;
      foreach (Item it in inv.Items)
      {
        if (this.IsInterestingItem(game, it))
          return it;
      }
      return (Item) null;
    }

    protected bool HasEnoughFoodFor(RogueGame game, int nutritionNeed)
    {
      if (this.m_Actor.Inventory == null || this.m_Actor.Inventory.IsEmpty)
        return false;
      int turnCounter = this.m_Actor.Location.Map.LocalTime.TurnCounter;
      int num = 0;
      foreach (Item obj in this.m_Actor.Inventory.Items)
      {
        if (obj is ItemFood)
        {
          num += game.Rules.FoodItemNutrition(obj as ItemFood, turnCounter);
          if (num >= nutritionNeed)
            return true;
        }
      }
      return false;
    }

    protected bool HasAtLeastFullStackOfItemTypeOrModel(Item it, int n)
    {
      if (this.m_Actor.Inventory == null || this.m_Actor.Inventory.IsEmpty)
        return false;
      if (it.Model.IsStackable)
        return this.CountItemsQuantityOfModel(it.Model) >= n * it.Model.StackingLimit;
      return this.CountItemsOfSameType(it.GetType()) >= n;
    }

    protected bool HasItemOfModel(ItemModel model)
    {
      if (this.m_Actor.Inventory == null || this.m_Actor.Inventory.IsEmpty)
        return false;
      foreach (Item obj in this.m_Actor.Inventory.Items)
      {
        if (obj.Model == model)
          return true;
      }
      return false;
    }

    protected int CountItemsQuantityOfModel(ItemModel model)
    {
      if (this.m_Actor.Inventory == null || this.m_Actor.Inventory.IsEmpty)
        return 0;
      int num = 0;
      foreach (Item obj in this.m_Actor.Inventory.Items)
      {
        if (obj.Model == model)
          num += obj.Quantity;
      }
      return num;
    }

    protected bool HasItemOfType(Type tt)
    {
      if (this.m_Actor.Inventory == null || this.m_Actor.Inventory.IsEmpty)
        return false;
      return this.m_Actor.Inventory.HasItemOfType(tt);
    }

    protected int CountItemQuantityOfType(Type tt)
    {
      if (this.m_Actor.Inventory == null || this.m_Actor.Inventory.IsEmpty)
        return 0;
      int num = 0;
      foreach (Item obj in this.m_Actor.Inventory.Items)
      {
        if (obj.GetType() == tt)
          num += obj.Quantity;
      }
      return num;
    }

    protected int CountItemsOfSameType(Type tt)
    {
      if (this.m_Actor.Inventory == null || this.m_Actor.Inventory.IsEmpty)
        return 0;
      int num = 0;
      foreach (object obj in this.m_Actor.Inventory.Items)
      {
        if (obj.GetType() == tt)
          ++num;
      }
      return num;
    }

    protected void RunIfPossible(Rules rules)
    {
      if (!rules.CanActorRun(this.m_Actor))
        return;
      this.m_Actor.IsRunning = true;
    }

    protected int GridDistancesSum(Rules rules, Point from, List<Percept> goals)
    {
      int num = 0;
      foreach (Percept goal in goals)
        num += rules.GridDistance(from, goal.Location.Position);
      return num;
    }

    protected float SafetyFrom(Rules rules, Point from, List<Percept> dangers)
    {
      Map map = this.m_Actor.Location.Map;
      float num1 = (float) (this.GridDistancesSum(rules, from, dangers) / (1 + dangers.Count));
      int num2 = 0;
      foreach (Direction direction in Direction.COMPASS)
      {
        Point point = from + direction;
        if (point == this.m_Actor.Location.Position || rules.IsWalkableFor(this.m_Actor, map, point.X, point.Y))
          ++num2;
      }
      float num3 = (float) num2 * 0.1f;
      bool isInside = map.GetTileAt(from).IsInside;
      int num4 = 0;
      foreach (Percept danger in dangers)
      {
        if (map.GetTileAt(danger.Location.Position).IsInside)
          ++num4;
        else
          --num4;
      }
      float num5 = 0.0f;
      if (isInside)
      {
        if (num4 < 0)
          num5 = 1.25f;
      }
      else if (num4 > 0)
        num5 = 1.25f;
      float num6 = 0.0f;
      if (this.m_Actor.Model.Abilities.CanTire && this.m_Actor.Model.Abilities.CanJump)
      {
        MapObject mapObjectAt = map.GetMapObjectAt(from);
        if (mapObjectAt != null && mapObjectAt.IsJumpable)
          num6 = 0.1f;
      }
      float num7 = 1f + num3 + num5 - num6;
      return num1 * num7;
    }

    protected BaseAI.ChoiceEval<_T_> Choose<_T_>(RogueGame game, List<_T_> listOfChoices, Func<_T_, bool> isChoiceValidFn, Func<_T_, float> evalChoiceFn, Func<float, float, bool> isBetterEvalThanFn)
    {
      if (listOfChoices.Count == 0)
        return (BaseAI.ChoiceEval<_T_>) null;
      bool flag = false;
      float num = 0.0f;
      List<BaseAI.ChoiceEval<_T_>> choiceEvalList1 = new List<BaseAI.ChoiceEval<_T_>>(listOfChoices.Count);
      for (int index = 0; index < listOfChoices.Count; ++index)
      {
        if (isChoiceValidFn(listOfChoices[index]))
        {
          float f = evalChoiceFn(listOfChoices[index]);
          if (!float.IsNaN(f))
          {
            choiceEvalList1.Add(new BaseAI.ChoiceEval<_T_>(listOfChoices[index], f));
            if (!flag || isBetterEvalThanFn(f, num))
            {
              flag = true;
              num = f;
            }
          }
        }
      }
      if (choiceEvalList1.Count == 0)
        return (BaseAI.ChoiceEval<_T_>) null;
      if (choiceEvalList1.Count == 1)
        return choiceEvalList1[0];
      List<BaseAI.ChoiceEval<_T_>> choiceEvalList2 = new List<BaseAI.ChoiceEval<_T_>>(choiceEvalList1.Count);
      for (int index = 0; index < choiceEvalList1.Count; ++index)
      {
        if ((double) choiceEvalList1[index].Value == (double) num)
          choiceEvalList2.Add(choiceEvalList1[index]);
      }
      int index1 = game.Rules.Roll(0, choiceEvalList2.Count);
      return choiceEvalList2[index1];
    }

    protected BaseAI.ChoiceEval<_DATA_> ChooseExtended<_T_, _DATA_>(RogueGame game, List<_T_> listOfChoices, Func<_T_, _DATA_> isChoiceValidFn, Func<_T_, float> evalChoiceFn, Func<float, float, bool> isBetterEvalThanFn)
    {
      if (listOfChoices.Count == 0)
        return (BaseAI.ChoiceEval<_DATA_>) null;
      bool flag = false;
      float num = 0.0f;
      List<BaseAI.ChoiceEval<_DATA_>> choiceEvalList1 = new List<BaseAI.ChoiceEval<_DATA_>>(listOfChoices.Count);
      for (int index = 0; index < listOfChoices.Count; ++index)
      {
        _DATA_ choice = isChoiceValidFn(listOfChoices[index]);
        if ((object) choice != null)
        {
          float f = evalChoiceFn(listOfChoices[index]);
          if (!float.IsNaN(f))
          {
            choiceEvalList1.Add(new BaseAI.ChoiceEval<_DATA_>(choice, f));
            if (!flag || isBetterEvalThanFn(f, num))
            {
              flag = true;
              num = f;
            }
          }
        }
      }
      if (choiceEvalList1.Count == 0)
        return (BaseAI.ChoiceEval<_DATA_>) null;
      if (choiceEvalList1.Count == 1)
        return choiceEvalList1[0];
      List<BaseAI.ChoiceEval<_DATA_>> choiceEvalList2 = new List<BaseAI.ChoiceEval<_DATA_>>(choiceEvalList1.Count);
      for (int index = 0; index < choiceEvalList1.Count; ++index)
      {
        if ((double) choiceEvalList1[index].Value == (double) num)
          choiceEvalList2.Add(choiceEvalList1[index]);
      }
      if (choiceEvalList2.Count == 0)
        return (BaseAI.ChoiceEval<_DATA_>) null;
      int index1 = game.Rules.Roll(0, choiceEvalList2.Count);
      return choiceEvalList2[index1];
    }

    protected bool IsValidFleeingAction(ActorAction a)
    {
      if (a == null)
        return false;
      if (!(a is ActionMoveStep) && !(a is ActionOpenDoor))
        return a is ActionSwitchPlace;
      return true;
    }

    protected bool isValidWanderAction(RogueGame game, ActorAction a)
    {
      if (a == null)
        return false;
      if (!(a is ActionMoveStep) && !(a is ActionSwitchPlace) && (!(a is ActionPush) && !(a is ActionOpenDoor)) && (!(a is ActionChat) || !this.Directives.CanTrade && (a as ActionChat).Target != this.m_Actor.Leader) && (!(a is ActionBashDoor) && (!(a is ActionGetFromContainer) || !this.IsInterestingItem(game, (a as ActionGetFromContainer).Item))))
        return a is ActionBarricadeDoor;
      return true;
    }

    protected bool IsValidMoveTowardGoalAction(ActorAction a)
    {
      if (a != null && !(a is ActionChat) && (!(a is ActionGetFromContainer) && !(a is ActionSwitchPowerGenerator)))
        return !(a is ActionRechargeItemBattery);
      return false;
    }

    protected bool HasNoFoodItems(Actor actor)
    {
      Inventory inventory = actor.Inventory;
      if (inventory == null || inventory.IsEmpty)
        return true;
      return !inventory.HasItemOfType(typeof (ItemFood));
    }

    protected bool IsSoldier(Actor actor)
    {
      if (actor != null)
        return actor.Controller is SoldierAI;
      return false;
    }

    protected bool WouldLikeToSleep(RogueGame game, Actor actor)
    {
      if (!game.Rules.IsAlmostSleepy(actor))
        return game.Rules.IsActorSleepy(actor);
      return true;
    }

    protected bool IsOccupiedByOther(Map map, Point position)
    {
      Actor actorAt = map.GetActorAt(position);
      if (actorAt != null)
        return actorAt != this.m_Actor;
      return false;
    }

    protected bool IsAdjacentToEnemy(RogueGame game, Actor actor)
    {
      if (actor == null)
        return false;
      Map map = actor.Location.Map;
      return map.HasAnyAdjacentInMap(actor.Location.Position, (Predicate<Point>) (pt =>
      {
        Actor actorAt = map.GetActorAt(pt);
        if (actorAt == null)
          return false;
        return game.Rules.IsEnemyOf(actor, actorAt);
      }));
    }

    protected bool IsInside(Actor actor)
    {
      if (actor == null)
        return false;
      Location location = actor.Location;
      Map map = location.Map;
      location = actor.Location;
      Point position = location.Position;
      int x = position.X;
      location = actor.Location;
      position = location.Position;
      int y = position.Y;
      return map.GetTileAt(x, y).IsInside;
    }

    protected bool HasEquipedRangedWeapon(Actor actor)
    {
      return actor.GetEquippedWeapon() is ItemRangedWeapon;
    }

    protected ItemAmmo GetCompatibleAmmoItem(RogueGame game, ItemRangedWeapon rw)
    {
      if (this.m_Actor.Inventory == null)
        return (ItemAmmo) null;
      foreach (Item obj in this.m_Actor.Inventory.Items)
      {
        ItemAmmo itemAmmo = obj as ItemAmmo;
        if (itemAmmo != null && itemAmmo.AmmoType == rw.AmmoType && game.Rules.CanActorUseItem(this.m_Actor, (Item) itemAmmo))
          return itemAmmo;
      }
      return (ItemAmmo) null;
    }

    protected ItemRangedWeapon GetCompatibleRangedWeapon(RogueGame game, ItemAmmo am)
    {
      if (this.m_Actor.Inventory == null)
        return (ItemRangedWeapon) null;
      foreach (Item obj in this.m_Actor.Inventory.Items)
      {
        ItemRangedWeapon itemRangedWeapon = obj as ItemRangedWeapon;
        if (itemRangedWeapon != null && itemRangedWeapon.AmmoType == am.AmmoType)
          return itemRangedWeapon;
      }
      return (ItemRangedWeapon) null;
    }

    protected ItemMeleeWeapon GetBestMeleeWeapon(RogueGame game, Predicate<Item> fn)
    {
      if (this.m_Actor.Inventory == null)
        return (ItemMeleeWeapon) null;
      int num1 = 0;
      ItemMeleeWeapon itemMeleeWeapon1 = (ItemMeleeWeapon) null;
      foreach (Item obj in this.m_Actor.Inventory.Items)
      {
        if (fn == null || fn(obj))
        {
          ItemMeleeWeapon itemMeleeWeapon2 = obj as ItemMeleeWeapon;
          if (itemMeleeWeapon2 != null)
          {
            ItemMeleeWeaponModel model = itemMeleeWeapon2.Model as ItemMeleeWeaponModel;
            int num2 = 10000;
            Attack attack = model.Attack;
            int damageValue = attack.DamageValue;
            int num3 = num2 * damageValue;
            int num4 = 100;
            attack = model.Attack;
            int hitValue = attack.HitValue;
            int num5 = num4 * hitValue;
            int num6 = num3 + num5;
            attack = model.Attack;
            int num7 = -attack.StaminaPenalty;
            int num8 = num6 + num7;
            if (num8 > num1)
            {
              num1 = num8;
              itemMeleeWeapon1 = itemMeleeWeapon2;
            }
          }
        }
      }
      return itemMeleeWeapon1;
    }

    protected ItemBodyArmor GetBestBodyArmor(RogueGame game, Predicate<Item> fn)
    {
      if (this.m_Actor.Inventory == null)
        return (ItemBodyArmor) null;
      int num1 = 0;
      ItemBodyArmor itemBodyArmor1 = (ItemBodyArmor) null;
      foreach (Item obj in this.m_Actor.Inventory.Items)
      {
        if (fn == null || fn(obj))
        {
          ItemBodyArmor itemBodyArmor2 = obj as ItemBodyArmor;
          if (itemBodyArmor2 != null)
          {
            int num2 = itemBodyArmor2.Protection_Hit + itemBodyArmor2.Protection_Shot;
            if (num2 > num1)
            {
              num1 = num2;
              itemBodyArmor1 = itemBodyArmor2;
            }
          }
        }
      }
      return itemBodyArmor1;
    }

    protected bool WantToEvadeMelee(RogueGame game, Actor actor, ActorCourage courage, Actor target)
    {
      if (this.WillTireAfterAttack(game, actor))
        return true;
      if (game.Rules.ActorSpeed(actor) > game.Rules.ActorSpeed(target))
      {
        if (game.Rules.WillActorActAgainBefore(actor, target))
          return false;
        if (target.TargetActor == actor)
          return true;
      }
      Actor weakerInMelee = this.FindWeakerInMelee(game, this.m_Actor, target);
      return weakerInMelee != target && (weakerInMelee == this.m_Actor || courage != ActorCourage.COURAGEOUS);
    }

    protected Actor FindWeakerInMelee(RogueGame game, Actor a, Actor b)
    {
      int num1 = a.HitPoints + a.CurrentMeleeAttack.DamageValue;
      int num2 = b.HitPoints + b.CurrentMeleeAttack.DamageValue;
      if (num1 < num2)
        return a;
      if (num1 <= num2)
        return (Actor) null;
      return b;
    }

    protected bool WillTireAfterAttack(RogueGame game, Actor actor)
    {
      if (!actor.Model.Abilities.CanTire)
        return false;
      return actor.StaminaPoints - 8 < 10;
    }

    protected bool WillTireAfterRunning(RogueGame game, Actor actor)
    {
      if (!actor.Model.Abilities.CanTire)
        return false;
      return actor.StaminaPoints - 4 < 10;
    }

    protected bool HasSpeedAdvantage(RogueGame game, Actor actor, Actor target)
    {
      int num1 = game.Rules.ActorSpeed(actor);
      int num2 = game.Rules.ActorSpeed(target);
      return num1 > num2 || game.Rules.CanActorRun(actor) && !game.Rules.CanActorRun(target) && (!this.WillTireAfterRunning(game, actor) && num1 * 2 > num2);
    }

    protected bool NeedsLight(RogueGame game)
    {
      switch (this.m_Actor.Location.Map.Lighting)
      {
        case Lighting._FIRST:
          return true;
        case Lighting.OUTSIDE:
          if (!this.m_Actor.Location.Map.LocalTime.IsNight)
            return false;
          if (game.Session.World.Weather == Weather.HEAVY_RAIN)
            return true;
          Map map = this.m_Actor.Location.Map;
          Location location = this.m_Actor.Location;
          int x = location.Position.X;
          location = this.m_Actor.Location;
          int y = location.Position.Y;
          return !map.GetTileAt(x, y).IsInside;
        case Lighting.LIT:
          return false;
        default:
          throw new ArgumentOutOfRangeException("unhandled lighting");
      }
    }

    protected bool IsBetween(RogueGame game, Point A, Point between, Point B)
    {
      double num1 = (double) game.Rules.StdDistance(A, between);
      float num2 = game.Rules.StdDistance(B, between);
      float num3 = game.Rules.StdDistance(A, B);
      double num4 = (double) num2;
      return num1 + num4 <= (double) num3 + 0.25;
    }

    protected bool IsDoorwayOrCorridor(RogueGame game, Map map, Point pos)
    {
      if (!map.GetTileAt(pos).Model.IsWalkable)
        return false;
      Point p1 = pos + Direction.N;
      bool flag1 = map.IsInBounds(p1) && !map.GetTileAt(p1).Model.IsWalkable;
      Point p2 = pos + Direction.S;
      bool flag2 = map.IsInBounds(p2) && !map.GetTileAt(p2).Model.IsWalkable;
      Point p3 = pos + Direction.E;
      bool flag3 = map.IsInBounds(p3) && !map.GetTileAt(p3).Model.IsWalkable;
      Point p4 = pos + Direction.W;
      bool flag4 = map.IsInBounds(p4) && !map.GetTileAt(p4).Model.IsWalkable;
      Point p5 = pos + Direction.NE;
      int num = !map.IsInBounds(p5) ? 0 : (!map.GetTileAt(p5).Model.IsWalkable ? 1 : 0);
      Point p6 = pos + Direction.NW;
      bool flag5 = map.IsInBounds(p6) && !map.GetTileAt(p6).Model.IsWalkable;
      Point p7 = pos + Direction.SE;
      bool flag6 = map.IsInBounds(p7) && !map.GetTileAt(p7).Model.IsWalkable;
      Point p8 = pos + Direction.SW;
      bool flag7 = map.IsInBounds(p8) && !map.GetTileAt(p8).Model.IsWalkable;
      bool flag8 = num == 0 && !flag6 && !flag5 && !flag7;
      return flag8 & flag1 & flag2 && !flag3 && !flag4 || flag8 & flag3 & flag4 && !flag1 && !flag2;
    }

    protected bool IsFriendOf(RogueGame game, Actor other)
    {
      if (!game.Rules.IsEnemyOf(this.m_Actor, other))
        return this.m_Actor.Faction == other.Faction;
      return false;
    }

    protected Actor GetNearestTargetFor(RogueGame game, Actor actor)
    {
      Map map = actor.Location.Map;
      Actor actor1 = (Actor) null;
      int num1 = int.MaxValue;
      foreach (Actor actor2 in map.Actors)
      {
        if (!actor2.IsDead && actor2 != actor && game.Rules.IsEnemyOf(actor, actor2))
        {
          int num2 = game.Rules.GridDistance(actor2.Location.Position, actor.Location.Position);
          if (num2 < num1 && (num2 == 1 || LOS.CanTraceViewLine(actor.Location, actor2.Location.Position)))
          {
            num1 = num2;
            actor1 = actor2;
          }
        }
      }
      return actor1;
    }

    protected List<Exit> ListAdjacentExits(RogueGame game, Location fromLocation)
    {
      List<Exit> exitList = (List<Exit>) null;
      foreach (Direction direction in Direction.COMPASS)
      {
        Point pos = fromLocation.Position + direction;
        Exit exitAt = fromLocation.Map.GetExitAt(pos);
        if (exitAt != null)
        {
          if (exitList == null)
            exitList = new List<Exit>(8);
          exitList.Add(exitAt);
        }
      }
      return exitList;
    }

    protected Exit PickAnyAdjacentExit(RogueGame game, Location fromLocation)
    {
      List<Exit> exitList = this.ListAdjacentExits(game, fromLocation);
      return exitList?[game.Rules.Roll(0, exitList.Count)];
    }

    public static bool IsAnyActivatedTrapThere(Map map, Point pos)
    {
      Inventory itemsAt = map.GetItemsAt(pos);
      if (itemsAt == null || itemsAt.IsEmpty)
        return false;
      return itemsAt.GetFirstMatching((Predicate<Item>) (it =>
      {
        ItemTrap itemTrap = it as ItemTrap;
        if (itemTrap != null)
          return itemTrap.IsActivated;
        return false;
      })) != null;
    }

    public static bool IsZoneChange(Map map, Point pos)
    {
      List<Zone> zonesHere = map.GetZonesAt(pos.X, pos.Y);
      if (zonesHere == null)
        return false;
      return map.HasAnyAdjacentInMap(pos, (Predicate<Point>) (adj =>
      {
        List<Zone> zonesAt = map.GetZonesAt(adj.X, adj.Y);
        if (zonesAt == null)
          return false;
        if (zonesHere == null)
          return true;
        foreach (Zone zone in zonesAt)
        {
          if (!zonesHere.Contains(zone))
            return true;
        }
        return false;
      }));
    }

    protected Point RandomPositionNear(Rules rules, Map map, Point goal, int range)
    {
      int x = goal.X + rules.Roll(-range, range);
      int y = goal.Y + rules.Roll(-range, range);
      map.TrimToBounds(ref x, ref y);
      return new Point(x, y);
    }

    protected void MarkItemAsTaboo(Item it)
    {
      if (this.m_TabooItems == null)
        this.m_TabooItems = new List<Item>(1);
      else if (this.m_TabooItems.Contains(it))
        return;
      this.m_TabooItems.Add(it);
    }

    protected void UnmarkItemAsTaboo(Item it)
    {
      if (this.m_TabooItems == null)
        return;
      this.m_TabooItems.Remove(it);
      if (this.m_TabooItems.Count != 0)
        return;
      this.m_TabooItems = (List<Item>) null;
    }

    protected bool IsItemTaboo(Item it)
    {
      if (this.m_TabooItems == null)
        return false;
      return this.m_TabooItems.Contains(it);
    }

    protected void MarkTileAsTaboo(Point p)
    {
      if (this.m_TabooTiles == null)
        this.m_TabooTiles = new List<Point>(1);
      else if (this.m_TabooTiles.Contains(p))
        return;
      this.m_TabooTiles.Add(p);
    }

    protected bool IsTileTaboo(Point p)
    {
      if (this.m_TabooTiles == null)
        return false;
      return this.m_TabooTiles.Contains(p);
    }

    protected void ClearTabooTiles()
    {
      this.m_TabooTiles = (List<Point>) null;
    }

    protected void MarkActorAsRecentTrade(Actor other)
    {
      if (this.m_TabooTrades == null)
        this.m_TabooTrades = new List<Actor>(1);
      else if (this.m_TabooTrades.Contains(other))
        return;
      this.m_TabooTrades.Add(other);
    }

    protected bool IsActorTabooTrade(Actor other)
    {
      if (this.m_TabooTrades == null)
        return false;
      return this.m_TabooTrades.Contains(other);
    }

    protected void ClearTabooTrades()
    {
      this.m_TabooTrades = (List<Actor>) null;
    }

    protected class ChoiceEval<_T_>
    {
      public _T_ Choice { get; private set; }

      public float Value { get; private set; }

      public ChoiceEval(_T_ choice, float value)
      {
        this.Choice = choice;
        this.Value = value;
      }

      public override string ToString()
      {
        return string.Format("ChoiceEval({0}; {1:F})", (object) this.Choice == null ? (object) "NULL" : (object) this.Choice.ToString(), (object) this.Value);
      }
    }

    [System.Flags]
    protected enum UseExitFlags
    {
      NONE = 0,
      BREAK_BLOCKING_OBJECTS = 1,
      ATTACK_BLOCKING_ENEMIES = 2,
      DONT_BACKTRACK = 4,
    }
  }
}
