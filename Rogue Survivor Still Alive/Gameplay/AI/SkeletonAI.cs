// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Gameplay.AI.SkeletonAI
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

namespace djack.RogueSurvivor.Gameplay.AI
{
  [Serializable]
  internal class SkeletonAI : BaseAI
  {
    private const int IDLE_CHANCE = 80;
    private LOSSensor m_LOSSensor;

    protected override void CreateSensors()
    {
      this.m_LOSSensor = new LOSSensor(LOSSensor.SensingFilter.ACTORS);
    }

    protected override List<Percept> UpdateSensors(RogueGame game)
    {
      return this.m_LOSSensor.Sense(game, this.m_Actor);
    }

    protected override ActorAction SelectAction(RogueGame game, List<Percept> percepts)
    {
      List<Percept> percepts1 = this.FilterSameMap(game, percepts);
      Percept percept = this.FilterNearest(game, this.FilterEnemies(game, percepts1));
      if (percept != null)
      {
        ActorAction actorAction = this.BehaviorStupidBumpToward(game, percept.Location.Position);
        if (actorAction != null)
        {
          this.m_Actor.Activity = Activity.CHASING;
          this.m_Actor.TargetActor = percept.Percepted as Actor;
          return actorAction;
        }
      }
      if (game.Rules.RollChance(80))
      {
        this.m_Actor.Activity = Activity.IDLE;
        return (ActorAction) new ActionWait(this.m_Actor, game);
      }
      this.m_Actor.Activity = Activity.IDLE;
      return this.BehaviorWander(game);
    }
  }
}
