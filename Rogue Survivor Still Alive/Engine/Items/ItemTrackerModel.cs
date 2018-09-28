// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.Items.ItemTrackerModel
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Data;

namespace djack.RogueSurvivor.Engine.Items
{
  internal class ItemTrackerModel : ItemModel
  {
    private ItemTrackerModel.TrackingFlags m_Tracking;
    private int m_MaxBatteries;

    public ItemTrackerModel.TrackingFlags Tracking
    {
      get
      {
        return this.m_Tracking;
      }
    }

    public int MaxBatteries
    {
      get
      {
        return this.m_MaxBatteries;
      }
    }

    public ItemTrackerModel(string aName, string theNames, string imageID, ItemTrackerModel.TrackingFlags tracking, int maxBatteries)
      : base(aName, theNames, imageID)
    {
      this.m_Tracking = tracking;
      this.m_MaxBatteries = maxBatteries;
      this.DontAutoEquip = true;
    }

    [System.Flags]
    public enum TrackingFlags
    {
      FOLLOWER_AND_LEADER = 1,
      UNDEADS = 2,
      BLACKOPS_FACTION = 4,
      POLICE_FACTION = 8,
    }
  }
}
