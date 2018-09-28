// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Data.ActorOrder
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using System;
using System.Drawing;

namespace djack.RogueSurvivor.Data
{
  [Serializable]
  internal class ActorOrder
  {
    private ActorTasks m_Task;
    private Location m_Location;

    public ActorTasks Task
    {
      get
      {
        return this.m_Task;
      }
    }

    public Location Location
    {
      get
      {
        return this.m_Location;
      }
    }

    public ActorOrder(ActorTasks task, Location location)
    {
      this.m_Task = task;
      this.m_Location = location;
    }

    public override string ToString()
    {
      switch (this.m_Task)
      {
        case ActorTasks.BARRICADE_ONE:
          string format1 = "barricade one ({0},{1})";
          Point position1 = this.m_Location.Position;
          // ISSUE: variable of a boxed type
          __Boxed<int> x1 = (ValueType) position1.X;
          position1 = this.m_Location.Position;
          // ISSUE: variable of a boxed type
          __Boxed<int> y1 = (ValueType) position1.Y;
          return string.Format(format1, (object) x1, (object) y1);
        case ActorTasks.BARRICADE_MAX:
          string format2 = "barricade max ({0},{1})";
          Point position2 = this.m_Location.Position;
          // ISSUE: variable of a boxed type
          __Boxed<int> x2 = (ValueType) position2.X;
          position2 = this.m_Location.Position;
          // ISSUE: variable of a boxed type
          __Boxed<int> y2 = (ValueType) position2.Y;
          return string.Format(format2, (object) x2, (object) y2);
        case ActorTasks.GUARD:
          string format3 = "guard ({0},{1})";
          Point position3 = this.m_Location.Position;
          // ISSUE: variable of a boxed type
          __Boxed<int> x3 = (ValueType) position3.X;
          position3 = this.m_Location.Position;
          // ISSUE: variable of a boxed type
          __Boxed<int> y3 = (ValueType) position3.Y;
          return string.Format(format3, (object) x3, (object) y3);
        case ActorTasks.PATROL:
          string format4 = "patrol ({0},{1})";
          Point position4 = this.m_Location.Position;
          // ISSUE: variable of a boxed type
          __Boxed<int> x4 = (ValueType) position4.X;
          position4 = this.m_Location.Position;
          // ISSUE: variable of a boxed type
          __Boxed<int> y4 = (ValueType) position4.Y;
          return string.Format(format4, (object) x4, (object) y4);
        case ActorTasks.DROP_ALL_ITEMS:
          return "drop all items";
        case ActorTasks.BUILD_SMALL_FORTIFICATION:
          string format5 = "build small fortification ({0},{1})";
          Point position5 = this.m_Location.Position;
          // ISSUE: variable of a boxed type
          __Boxed<int> x5 = (ValueType) position5.X;
          position5 = this.m_Location.Position;
          // ISSUE: variable of a boxed type
          __Boxed<int> y5 = (ValueType) position5.Y;
          return string.Format(format5, (object) x5, (object) y5);
        case ActorTasks.BUILD_LARGE_FORTIFICATION:
          string format6 = "build large fortification ({0},{1})";
          Point position6 = this.m_Location.Position;
          // ISSUE: variable of a boxed type
          __Boxed<int> x6 = (ValueType) position6.X;
          position6 = this.m_Location.Position;
          // ISSUE: variable of a boxed type
          __Boxed<int> y6 = (ValueType) position6.Y;
          return string.Format(format6, (object) x6, (object) y6);
        case ActorTasks.REPORT_EVENTS:
          return "reporting events to leader";
        case ActorTasks.SLEEP_NOW:
          return "sleep there";
        case ActorTasks.FOLLOW_TOGGLE:
          return "stop/start following";
        case ActorTasks.WHERE_ARE_YOU:
          return "reporting position";
        default:
          throw new NotImplementedException("unhandled task");
      }
    }
  }
}
