// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Gameplay.AI.Sensors.SmellSensor
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
  internal class SmellSensor : Sensor
  {
    private Odor m_OdorToSmell;
    private List<Percept> m_List;

    public List<Percept> Scents
    {
      get
      {
        return this.m_List;
      }
    }

    public SmellSensor(Odor odorToSmell)
    {
      this.m_OdorToSmell = odorToSmell;
      this.m_List = new List<Percept>(9);
    }

    public override List<Percept> Sense(RogueGame game, Actor actor)
    {
      this.m_List.Clear();
      int num = game.Rules.ActorSmellThreshold(actor);
      int x1 = actor.Location.Position.X - 1;
      int x2 = actor.Location.Position.X + 1;
      int y1 = actor.Location.Position.Y - 1;
      int y2 = actor.Location.Position.Y + 1;
      actor.Location.Map.TrimToBounds(ref x1, ref y1);
      actor.Location.Map.TrimToBounds(ref x2, ref y2);
      int turnCounter = actor.Location.Map.LocalTime.TurnCounter;
      Point position = new Point();
      for (int index1 = x1; index1 <= x2; ++index1)
      {
        position.X = index1;
        for (int index2 = y1; index2 <= y2; ++index2)
        {
          position.Y = index2;
          Location location1 = actor.Location;
          int scentByOdorAt = location1.Map.GetScentByOdorAt(this.m_OdorToSmell, position);
          if (scentByOdorAt >= 0 && scentByOdorAt >= num)
          {
            List<Percept> list = this.m_List;
            SmellSensor.AIScent aiScent = new SmellSensor.AIScent(this.m_OdorToSmell, scentByOdorAt);
            int turn = turnCounter;
            location1 = actor.Location;
            Location location2 = new Location(location1.Map, position);
            Percept percept = new Percept((object) aiScent, turn, location2);
            list.Add(percept);
          }
        }
      }
      return this.m_List;
    }

    [Serializable]
    public class AIScent
    {
      public Odor Odor { get; private set; }

      public int Strength { get; private set; }

      public AIScent(Odor odor, int strength)
      {
        this.Odor = odor;
        this.Strength = strength;
      }
    }
  }
}
