// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Gameplay.AI.SewersThingAI
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Data;
using djack.RogueSurvivor.Engine;
using djack.RogueSurvivor.Engine.AI;
using djack.RogueSurvivor.Gameplay.AI.Sensors;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace djack.RogueSurvivor.Gameplay.AI
{
  [Serializable]
  internal class SewersThingAI : BaseAI
  {
    private const int LOS_MEMORY = 20;
    private MemorizedSensor m_LOSSensor;
    private SmellSensor m_LivingSmellSensor;
    private SmellSensor m_MasterSmellSensor;

    protected override void CreateSensors()
    {
      this.m_LOSSensor = new MemorizedSensor((Sensor) new LOSSensor(LOSSensor.SensingFilter.ACTORS), 20);
      this.m_LivingSmellSensor = new SmellSensor(Odor.LIVING);
      this.m_MasterSmellSensor = new SmellSensor(Odor.UNDEAD_MASTER);
    }

    protected override List<Percept> UpdateSensors(RogueGame game)
    {
      List<Percept> perceptList = this.m_LOSSensor.Sense(game, this.m_Actor);
      perceptList.AddRange((IEnumerable<Percept>) this.m_LivingSmellSensor.Sense(game, this.m_Actor));
      perceptList.AddRange((IEnumerable<Percept>) this.m_MasterSmellSensor.Sense(game, this.m_Actor));
      return perceptList;
    }

    protected override ActorAction SelectAction(RogueGame game, List<Percept> percepts)
    {
      HashSet<Point> fov = (this.m_LOSSensor.Sensor as LOSSensor).FOV;
      List<Percept> percepts1 = this.FilterSameMap(game, percepts);
      List<Percept> percepts2 = this.FilterEnemies(game, percepts1);
      if (percepts2 != null)
      {
        List<Percept> perceptList1 = this.Filter(game, percepts2, (Predicate<Percept>) (p => p.Turn == this.m_Actor.Location.Map.LocalTime.TurnCounter));
        if (perceptList1 != null)
        {
          Percept percept1 = (Percept) null;
          ActorAction actorAction1 = (ActorAction) null;
          float num1 = (float) int.MaxValue;
          foreach (Percept percept2 in perceptList1)
          {
            Rules rules = game.Rules;
            Location location = this.m_Actor.Location;
            Point position1 = location.Position;
            location = percept2.Location;
            Point position2 = location.Position;
            float num2 = (float) rules.GridDistance(position1, position2);
            if ((double) num2 < (double) num1)
            {
              RogueGame game1 = game;
              location = percept2.Location;
              Point position3 = location.Position;
              ActorAction actorAction2 = this.BehaviorStupidBumpToward(game1, position3);
              if (actorAction2 != null)
              {
                num1 = num2;
                percept1 = percept2;
                actorAction1 = actorAction2;
              }
            }
          }
          if (actorAction1 != null)
          {
            this.m_Actor.Activity = Activity.CHASING;
            this.m_Actor.TargetActor = percept1.Percepted as Actor;
            return actorAction1;
          }
        }
        List<Percept> perceptList2 = this.Filter(game, percepts2, (Predicate<Percept>) (p => p.Turn != this.m_Actor.Location.Map.LocalTime.TurnCounter));
        if (perceptList2 != null)
        {
          Percept percept1 = (Percept) null;
          ActorAction actorAction1 = (ActorAction) null;
          float num1 = (float) int.MaxValue;
          foreach (Percept percept2 in perceptList2)
          {
            Rules rules = game.Rules;
            Location location = this.m_Actor.Location;
            Point position1 = location.Position;
            location = percept2.Location;
            Point position2 = location.Position;
            float num2 = (float) rules.GridDistance(position1, position2);
            if ((double) num2 < (double) num1)
            {
              RogueGame game1 = game;
              location = percept2.Location;
              Point position3 = location.Position;
              ActorAction actorAction2 = this.BehaviorStupidBumpToward(game1, position3);
              if (actorAction2 != null)
              {
                num1 = num2;
                percept1 = percept2;
                actorAction1 = actorAction2;
              }
            }
          }
          if (actorAction1 != null)
          {
            this.m_Actor.Activity = Activity.CHASING;
            this.m_Actor.TargetActor = percept1.Percepted as Actor;
            return actorAction1;
          }
        }
      }
      ActorAction actorAction = this.BehaviorTrackScent(game, this.m_LivingSmellSensor.Scents);
      if (actorAction != null)
      {
        this.m_Actor.Activity = Activity.TRACKING;
        return actorAction;
      }
      this.m_Actor.Activity = Activity.IDLE;
      return this.BehaviorWander(game);
    }
  }
}
