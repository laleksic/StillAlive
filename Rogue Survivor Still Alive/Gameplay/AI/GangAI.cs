﻿// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Gameplay.AI.GangAI
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Data;
using djack.RogueSurvivor.Engine;
using djack.RogueSurvivor.Engine.Actions;
using djack.RogueSurvivor.Engine.AI;
using djack.RogueSurvivor.Gameplay.AI.Sensors;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace djack.RogueSurvivor.Gameplay.AI
{
  [Serializable]
  internal class GangAI : OrderableAI
  {
    private static string[] FIGHT_EMOTES = new string[3]
    {
      "Fuck you",
      "Fuck it I'm trapped!",
      "Come on"
    };
    private const int FOLLOW_NPCLEADER_MAXDIST = 1;
    private const int FOLLOW_PLAYERLEADER_MAXDIST = 1;
    private const int LOS_MEMORY = 10;
    private const int EXPLORATION_LOCATIONS = 30;
    private const int EXPLORATION_ZONES = 3;
    private const int DONT_LEAVE_BEHIND_EMOTE_CHANCE = 50;
    private LOSSensor m_LOSSensor;
    private MemorizedSensor m_MemorizedSensor;
    private ExplorationData m_Exploration;

    public override void TakeControl(Actor actor)
    {
      base.TakeControl(actor);
      this.m_Exploration = new ExplorationData(30, 3);
    }

    protected override void CreateSensors()
    {
      this.m_LOSSensor = new LOSSensor(LOSSensor.SensingFilter.ACTORS | LOSSensor.SensingFilter.ITEMS);
      this.m_MemorizedSensor = new MemorizedSensor((Sensor) this.m_LOSSensor, 10);
    }

    protected override List<Percept> UpdateSensors(RogueGame game)
    {
      return this.m_MemorizedSensor.Sense(game, this.m_Actor);
    }

    protected override ActorAction SelectAction(RogueGame game, List<Percept> percepts)
    {
      HashSet<Point> fov1 = this.m_LOSSensor.FOV;
      List<Percept> percepts1 = this.FilterSameMap(game, percepts);
      if (this.Order != null)
      {
        ActorAction actorAction = this.ExecuteOrder(game, this.Order, percepts1);
        if (actorAction == null)
        {
          this.SetOrder((ActorOrder) null);
        }
        else
        {
          this.m_Actor.Activity = Activity.FOLLOWING_ORDER;
          return actorAction;
        }
      }
      this.m_Actor.IsRunning = false;
      List<Percept> percepts2 = this.FilterEnemies(game, percepts1);
      List<Percept> perceptList = this.FilterCurrent(game, percepts2);
      bool flag1 = perceptList != null;
      bool flag2 = percepts2 != null;
      bool flag3 = this.m_Actor.HasLeader && !this.DontFollowLeader;
      bool hasVisibleLeader = flag3 && fov1.Contains(this.m_Actor.Leader.Location.Position);
      bool isLeaderFighting = flag3 && this.IsAdjacentToEnemy(game, this.m_Actor.Leader);
      bool flag4 = !game.Rules.IsActorTired(this.m_Actor);
      this.m_Exploration.Update(this.m_Actor.Location);
      ActorAction actorAction1 = this.BehaviorEquipWeapon(game);
      if (actorAction1 != null)
      {
        this.m_Actor.Activity = Activity.IDLE;
        return actorAction1;
      }
      ActorAction actorAction2 = this.BehaviorEquipBodyArmor(game);
      if (actorAction2 != null)
      {
        this.m_Actor.Activity = Activity.IDLE;
        return actorAction2;
      }
      if (flag1 && (flag3 || game.Rules.RollChance(50)))
      {
        List<Percept> percepts3 = this.FilterFireTargets(game, perceptList);
        if (percepts3 != null)
        {
          Percept target = this.FilterNearest(game, percepts3);
          ActorAction actorAction3 = this.BehaviorRangedAttack(game, target);
          if (actorAction3 != null)
          {
            this.m_Actor.Activity = Activity.FIGHTING;
            this.m_Actor.TargetActor = target.Percepted as Actor;
            return actorAction3;
          }
        }
      }
      if (flag1)
      {
        if (game.Rules.RollChance(50))
        {
          List<Percept> friends = this.FilterNonEnemies(game, percepts1);
          if (friends != null)
          {
            ActorAction actorAction3 = this.BehaviorWarnFriends(game, friends, this.FilterNearest(game, perceptList).Percepted as Actor);
            if (actorAction3 != null)
            {
              this.m_Actor.Activity = Activity.IDLE;
              return actorAction3;
            }
          }
        }
        ActorAction actorAction4 = this.BehaviorFightOrFlee(game, perceptList, hasVisibleLeader, isLeaderFighting, ActorCourage.COURAGEOUS, GangAI.FIGHT_EMOTES);
        if (actorAction4 != null)
          return actorAction4;
      }
      ActorAction actorAction5 = this.BehaviorUseMedecine(game, 2, 1, 2, 4, 2);
      if (actorAction5 != null)
      {
        this.m_Actor.Activity = Activity.IDLE;
        return actorAction5;
      }
      if (this.BehaviorRestIfTired(game) != null)
      {
        this.m_Actor.Activity = Activity.IDLE;
        return (ActorAction) new ActionWait(this.m_Actor, game);
      }
      if (flag1 & flag4)
      {
        Percept target = this.FilterNearest(game, perceptList);
        ActorAction actorAction3 = this.BehaviorChargeEnemy(game, target);
        if (actorAction3 != null)
        {
          this.m_Actor.Activity = Activity.FIGHTING;
          this.m_Actor.TargetActor = target.Percepted as Actor;
          return actorAction3;
        }
      }
      if (game.Rules.IsActorHungry(this.m_Actor))
      {
        ActorAction actorAction3 = this.BehaviorEat(game);
        if (actorAction3 != null)
        {
          this.m_Actor.Activity = Activity.IDLE;
          return actorAction3;
        }
        if (game.Rules.IsActorStarving(this.m_Actor) || game.Rules.IsActorInsane(this.m_Actor))
        {
          ActorAction actorAction4 = this.BehaviorGoEatCorpse(game, this.FilterCorpses(game, percepts1));
          if (actorAction4 != null)
          {
            this.m_Actor.Activity = Activity.IDLE;
            return actorAction4;
          }
        }
      }
      if (!flag2 && this.WouldLikeToSleep(game, this.m_Actor) && (this.IsInside(this.m_Actor) && game.Rules.CanActorSleep(this.m_Actor)))
      {
        ActorAction actorAction3 = this.BehaviorSecurePerimeter(game, this.m_LOSSensor.FOV);
        if (actorAction3 != null)
        {
          this.m_Actor.Activity = Activity.IDLE;
          return actorAction3;
        }
        ActorAction actorAction4 = this.BehaviorSleep(game, this.m_LOSSensor.FOV);
        if (actorAction4 != null)
        {
          if (actorAction4 is ActionSleep)
            this.m_Actor.Activity = Activity.SLEEPING;
          return actorAction4;
        }
      }
      ActorAction actorAction6 = this.BehaviorDropUselessItem(game);
      if (actorAction6 != null)
      {
        this.m_Actor.Activity = Activity.IDLE;
        return actorAction6;
      }
      bool flag5 = flag3 || this.m_Actor.CountFollowers > 0;
      bool flag6 = this.NeedsLight(game);
      if (!flag5 && !flag6)
      {
        ActorAction actorAction3 = this.BehaviorUnequipLeftItem(game);
        if (actorAction3 != null)
        {
          this.m_Actor.Activity = Activity.IDLE;
          return actorAction3;
        }
      }
      if (flag5)
      {
        ActorAction actorAction3 = this.BehaviorEquipCellPhone(game);
        if (actorAction3 != null)
        {
          this.m_Actor.Activity = Activity.IDLE;
          return actorAction3;
        }
      }
      else if (flag6)
      {
        ActorAction actorAction3 = this.BehaviorEquipLight(game);
        if (actorAction3 != null)
        {
          this.m_Actor.Activity = Activity.IDLE;
          return actorAction3;
        }
      }
      if (!flag1)
      {
        Map map = this.m_Actor.Location.Map;
        List<Percept> percepts3 = this.FilterOut(game, this.FilterStacks(game, percepts1), (Predicate<Percept>) (p =>
        {
          if (p.Turn == map.LocalTime.TurnCounter)
            return this.IsOccupiedByOther(map, p.Location.Position);
          return true;
        }));
        if (percepts3 != null)
        {
          Percept percept = this.FilterNearest(game, percepts3);
          ActorAction actorAction3 = this.BehaviorGrabFromStack(game, percept.Location.Position, percept.Percepted as Inventory);
          if (actorAction3 != null)
          {
            this.m_Actor.Activity = Activity.IDLE;
            return actorAction3;
          }
        }
      }
      Location location;
      if (!flag1)
      {
        location = this.m_Actor.Location;
        Map map = location.Map;
        List<Percept> percepts3 = this.FilterActors(game, this.FilterCurrent(game, percepts1), (Predicate<Actor>) (a =>
        {
          if (a.Inventory == null || a.Inventory.CountItems == 0 || this.IsFriendOf(game, a))
            return false;
          if (!game.Rules.RollChance(game.Rules.ActorUnsuspicousChance(this.m_Actor, a)))
            return this.HasAnyInterestingItem(game, a.Inventory);
          game.DoEmote(a, string.Format("moves unnoticed by {0}.", (object) this.m_Actor.Name));
          return false;
        }));
        if (percepts3 != null)
        {
          Actor percepted = this.FilterNearest(game, percepts3).Percepted as Actor;
          Item obj = this.FirstInterestingItem(game, percepted.Inventory);
          game.DoMakeAggression(this.m_Actor, percepted);
          this.m_Actor.Activity = Activity.CHASING;
          this.m_Actor.TargetActor = percepted;
          return (ActorAction) new ActionSay(this.m_Actor, game, percepted, string.Format("Hey! That's some nice {0} you have here!", (object) obj.Model.SingleName), RogueGame.Sayflags.IS_IMPORTANT);
        }
      }
      ActorAction actorAction7 = this.BehaviorAttackBarricade(game);
      if (actorAction7 != null)
      {
        this.m_Actor.Activity = Activity.IDLE;
        return actorAction7;
      }
      if (flag3)
      {
        location = this.m_Actor.Leader.Location;
        Point position1 = location.Position;
        HashSet<Point> pointSet = fov1;
        location = this.m_Actor.Leader.Location;
        Point position2 = location.Position;
        bool isVisible = pointSet.Contains(position2);
        int maxDist = this.m_Actor.Leader.IsPlayer ? 1 : 1;
        ActorAction actorAction3 = this.BehaviorFollowActor(game, this.m_Actor.Leader, position1, isVisible, maxDist);
        if (actorAction3 != null)
        {
          this.m_Actor.Activity = Activity.FOLLOWING;
          this.m_Actor.TargetActor = this.m_Actor.Leader;
          return actorAction3;
        }
      }
      bool flag7 = this.m_Actor.Sheet.SkillTable.GetSkillLevel(9) >= 1;
      if ((!(!flag3 & flag7) ? 0 : (this.m_Actor.CountFollowers < game.Rules.ActorMaxFollowers(this.m_Actor) ? 1 : 0)) != 0)
      {
        Percept target = this.FilterNearest(game, this.FilterNonEnemies(game, percepts1));
        if (target != null)
        {
          ActorAction actorAction3 = this.BehaviorLeadActor(game, target);
          if (actorAction3 != null)
          {
            this.m_Actor.Activity = Activity.IDLE;
            this.m_Actor.TargetActor = target.Percepted as Actor;
            return actorAction3;
          }
        }
      }
      if (this.m_Actor.CountFollowers > 0)
      {
        Actor target;
        ActorAction actorAction3 = this.BehaviorDontLeaveFollowersBehind(game, 3, out target);
        if (actorAction3 != null)
        {
          if (game.Rules.RollChance(50))
          {
            if (target.IsSleeping)
            {
              game.DoEmote(this.m_Actor, string.Format("patiently waits for {0} to wake up.", (object) target.Name));
            }
            else
            {
              HashSet<Point> fov2 = this.m_LOSSensor.FOV;
              location = target.Location;
              Point position = location.Position;
              if (fov2.Contains(position))
                game.DoEmote(this.m_Actor, string.Format("Hey {0}! Fucking move!", (object) target.Name));
              else
                game.DoEmote(this.m_Actor, string.Format("Where is that {0} retard?", (object) target.Name));
            }
          }
          this.m_Actor.Activity = Activity.IDLE;
          return actorAction3;
        }
      }
      ActorAction actorAction8 = this.BehaviorExplore(game, this.m_Exploration);
      if (actorAction8 != null)
      {
        this.m_Actor.Activity = Activity.IDLE;
        return actorAction8;
      }
      this.m_Actor.Activity = Activity.IDLE;
      return this.BehaviorWander(game);
    }
  }
}
