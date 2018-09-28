// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Gameplay.AI.Sensors.LOSSensor
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Data;
using djack.RogueSurvivor.Engine;
using djack.RogueSurvivor.Engine.AI;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace djack.RogueSurvivor.Gameplay.AI.Sensors
{
  [Serializable]
  internal class LOSSensor : Sensor
  {
    private HashSet<Point> m_FOV;
    private LOSSensor.SensingFilter m_Filters;

    public HashSet<Point> FOV
    {
      get
      {
        return this.m_FOV;
      }
    }

    public LOSSensor.SensingFilter Filters
    {
      get
      {
        return this.m_Filters;
      }
      set
      {
        this.m_Filters = value;
      }
    }

    public LOSSensor(LOSSensor.SensingFilter filters)
    {
      this.m_Filters = filters;
    }

    public override List<Percept> Sense(RogueGame game, Actor actor)
    {
      this.m_FOV = LOS.ComputeFOVFor(game.Rules, actor, actor.Location.Map.LocalTime, game.Session.World.Weather);
      int num1 = game.Rules.ActorFOV(actor, actor.Location.Map.LocalTime, game.Session.World.Weather);
      List<Percept> perceptList1 = new List<Percept>();
      if ((this.m_Filters & LOSSensor.SensingFilter.ACTORS) != (LOSSensor.SensingFilter) 0)
      {
        int num2 = num1 * num1;
        Location location1 = actor.Location;
        int countActors = location1.Map.CountActors;
        if (num2 < countActors)
        {
          foreach (Point point in this.m_FOV)
          {
            location1 = actor.Location;
            Actor actorAt = location1.Map.GetActorAt(point.X, point.Y);
            if (actorAt != null && actorAt != actor)
            {
              List<Percept> perceptList2 = perceptList1;
              Actor actor1 = actorAt;
              location1 = actor.Location;
              int turnCounter = location1.Map.LocalTime.TurnCounter;
              Location location2 = actorAt.Location;
              Percept percept = new Percept((object) actor1, turnCounter, location2);
              perceptList2.Add(percept);
            }
          }
        }
        else
        {
          location1 = actor.Location;
          foreach (Actor actor1 in location1.Map.Actors)
          {
            if (actor1 != actor)
            {
              Rules rules = game.Rules;
              location1 = actor.Location;
              Point position1 = location1.Position;
              location1 = actor1.Location;
              Point position2 = location1.Position;
              if ((double) rules.LOSDistance(position1, position2) <= (double) num1)
              {
                HashSet<Point> fov = this.m_FOV;
                location1 = actor1.Location;
                Point position3 = location1.Position;
                if (fov.Contains(position3))
                {
                  List<Percept> perceptList2 = perceptList1;
                  Actor actor2 = actor1;
                  location1 = actor.Location;
                  int turnCounter = location1.Map.LocalTime.TurnCounter;
                  Location location2 = actor1.Location;
                  Percept percept = new Percept((object) actor2, turnCounter, location2);
                  perceptList2.Add(percept);
                }
              }
            }
          }
        }
      }
      if ((this.m_Filters & LOSSensor.SensingFilter.ITEMS) != (LOSSensor.SensingFilter) 0)
      {
        foreach (Point position in this.m_FOV)
        {
          Location location1 = actor.Location;
          Inventory itemsAt = location1.Map.GetItemsAt(position);
          if (itemsAt != null && !itemsAt.IsEmpty)
          {
            List<Percept> perceptList2 = perceptList1;
            Inventory inventory = itemsAt;
            location1 = actor.Location;
            int turnCounter = location1.Map.LocalTime.TurnCounter;
            location1 = actor.Location;
            Location location2 = new Location(location1.Map, position);
            Percept percept = new Percept((object) inventory, turnCounter, location2);
            perceptList2.Add(percept);
          }
        }
      }
      if ((this.m_Filters & LOSSensor.SensingFilter.CORPSES) != (LOSSensor.SensingFilter) 0)
      {
        foreach (Point position in this.m_FOV)
        {
          List<Corpse> corpsesAt = actor.Location.Map.GetCorpsesAt(position.X, position.Y);
          if (corpsesAt != null)
            perceptList1.Add(new Percept((object) corpsesAt, actor.Location.Map.LocalTime.TurnCounter, new Location(actor.Location.Map, position)));
        }
      }
      return perceptList1;
    }

    [System.Flags]
    public enum SensingFilter
    {
      ACTORS = 1,
      ITEMS = 2,
      CORPSES = 4,
    }
  }
}
