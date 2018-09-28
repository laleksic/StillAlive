// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.AI.MemorizedSensor
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Data;
using System;
using System.Collections.Generic;

namespace djack.RogueSurvivor.Engine.AI
{
  [Serializable]
  internal class MemorizedSensor : Sensor
  {
    private List<Percept> m_Percepts = new List<Percept>();
    private Sensor m_Sensor;
    private int m_Persistance;

    public Sensor Sensor
    {
      get
      {
        return this.m_Sensor;
      }
    }

    public MemorizedSensor(Sensor noMemorySensor, int persistance)
    {
      if (noMemorySensor == null)
        throw new ArgumentNullException("decoratedSensor");
      this.m_Sensor = noMemorySensor;
      this.m_Persistance = persistance;
    }

    public void Clear()
    {
      this.m_Percepts.Clear();
    }

    public override List<Percept> Sense(RogueGame game, Actor actor)
    {
      int index1 = 0;
      while (index1 < this.m_Percepts.Count)
      {
        if (this.m_Percepts[index1].GetAge(actor.Location.Map.LocalTime.TurnCounter) > this.m_Persistance)
          this.m_Percepts.RemoveAt(index1);
        else
          ++index1;
      }
      int index2 = 0;
      while (index2 < this.m_Percepts.Count)
      {
        Actor percepted = this.m_Percepts[index2].Percepted as Actor;
        if (percepted != null)
        {
          if (!percepted.IsDead)
          {
            Location location = percepted.Location;
            Map map1 = location.Map;
            location = actor.Location;
            Map map2 = location.Map;
            if (map1 == map2)
              goto label_10;
          }
          this.m_Percepts.RemoveAt(index2);
          continue;
        }
label_10:
        ++index2;
      }
      List<Percept> perceptList1 = this.m_Sensor.Sense(game, actor);
      List<Percept> perceptList2 = (List<Percept>) null;
      foreach (Percept percept1 in perceptList1)
      {
        bool flag = false;
        foreach (Percept percept2 in this.m_Percepts)
        {
          if (percept2.Percepted == percept1.Percepted)
          {
            percept2.Location = percept1.Location;
            percept2.Turn = percept1.Turn;
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          if (perceptList2 == null)
            perceptList2 = new List<Percept>(perceptList1.Count);
          perceptList2.Add(percept1);
        }
      }
      if (perceptList2 != null)
      {
        foreach (Percept percept in perceptList2)
          this.m_Percepts.Add(percept);
      }
      return this.m_Percepts;
    }
  }
}
