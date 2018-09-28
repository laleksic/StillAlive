// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Gameplay.AI.ExplorationData
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Data;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace djack.RogueSurvivor.Gameplay.AI
{
  [Serializable]
  internal class ExplorationData
  {
    private int m_LocationsQueueSize;
    private Queue<Location> m_LocationsQueue;
    private int m_ZonesQueueSize;
    private Queue<Zone> m_ZonesQueue;

    public ExplorationData(int locationsToRemember, int zonesToRemember)
    {
      if (locationsToRemember < 1)
        throw new ArgumentOutOfRangeException("locationsQueueSize < 1");
      if (zonesToRemember < 1)
        throw new ArgumentOutOfRangeException("zonesQueueSize < 1");
      this.m_LocationsQueueSize = locationsToRemember;
      this.m_LocationsQueue = new Queue<Location>(locationsToRemember);
      this.m_ZonesQueueSize = zonesToRemember;
      this.m_ZonesQueue = new Queue<Zone>(zonesToRemember);
    }

    public void Clear()
    {
      this.m_LocationsQueue.Clear();
      this.m_ZonesQueue.Clear();
    }

    public bool HasExplored(Location loc)
    {
      return this.m_LocationsQueue.Contains(loc);
    }

    public void AddExplored(Location loc)
    {
      if (this.m_LocationsQueue.Count >= this.m_LocationsQueueSize)
        this.m_LocationsQueue.Dequeue();
      this.m_LocationsQueue.Enqueue(loc);
    }

    public bool HasExplored(Zone zone)
    {
      return this.m_ZonesQueue.Contains(zone);
    }

    public bool HasExplored(List<Zone> zones)
    {
      if (zones == null || zones.Count == 0)
        return true;
      foreach (Zone zone in zones)
      {
        if (!this.m_ZonesQueue.Contains(zone))
          return false;
      }
      return true;
    }

    public void AddExplored(Zone zone)
    {
      if (this.m_ZonesQueue.Count >= this.m_ZonesQueueSize)
        this.m_ZonesQueue.Dequeue();
      this.m_ZonesQueue.Enqueue(zone);
    }

    public void Update(Location location)
    {
      this.AddExplored(location);
      Map map = location.Map;
      Point position = location.Position;
      int x = position.X;
      position = location.Position;
      int y = position.Y;
      List<Zone> zonesAt = map.GetZonesAt(x, y);
      if (zonesAt == null || zonesAt.Count <= 0)
        return;
      foreach (Zone zone in zonesAt)
      {
        if (!this.HasExplored(zone))
          this.AddExplored(zone);
      }
    }
  }
}
