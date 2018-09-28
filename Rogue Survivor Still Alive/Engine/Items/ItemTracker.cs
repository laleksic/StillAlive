// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.Items.ItemTracker
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Data;
using System;

namespace djack.RogueSurvivor.Engine.Items
{
  [Serializable]
  internal class ItemTracker : Item
  {
    private int m_Batteries;

    public ItemTrackerModel.TrackingFlags Tracking { get; private set; }

    public bool CanTrackFollowersOrLeader
    {
      get
      {
        return (uint) (this.Tracking & ItemTrackerModel.TrackingFlags.FOLLOWER_AND_LEADER) > 0U;
      }
    }

    public bool CanTrackUndeads
    {
      get
      {
        return (uint) (this.Tracking & ItemTrackerModel.TrackingFlags.UNDEADS) > 0U;
      }
    }

    public bool CanTrackBlackOps
    {
      get
      {
        return (uint) (this.Tracking & ItemTrackerModel.TrackingFlags.BLACKOPS_FACTION) > 0U;
      }
    }

    public bool CanTrackPolice
    {
      get
      {
        return (uint) (this.Tracking & ItemTrackerModel.TrackingFlags.POLICE_FACTION) > 0U;
      }
    }

    public int Batteries
    {
      get
      {
        return this.m_Batteries;
      }
      set
      {
        if (value < 0)
          value = 0;
        this.m_Batteries = Math.Min(value, (this.Model as ItemTrackerModel).MaxBatteries);
      }
    }

    public bool IsFullyCharged
    {
      get
      {
        return this.m_Batteries >= (this.Model as ItemTrackerModel).MaxBatteries;
      }
    }

    public ItemTracker(ItemModel model)
      : base(model)
    {
      if (!(model is ItemTrackerModel))
        throw new ArgumentException("model is not a TrackerModel");
      ItemTrackerModel itemTrackerModel = model as ItemTrackerModel;
      this.Tracking = itemTrackerModel.Tracking;
      this.Batteries = itemTrackerModel.MaxBatteries;
    }
  }
}
