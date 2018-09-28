// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.Tasks.TaskRemoveDecoration
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Data;
using System;

namespace djack.RogueSurvivor.Engine.Tasks
{
  [Serializable]
  internal class TaskRemoveDecoration : TimedTask
  {
    private int m_X;
    private int m_Y;
    private string m_imageID;

    public TaskRemoveDecoration(int turns, int x, int y, string imageID)
      : base(turns)
    {
      this.m_X = x;
      this.m_Y = y;
      this.m_imageID = imageID;
    }

    public override void Trigger(Map m)
    {
      m.GetTileAt(this.m_X, this.m_Y).RemoveDecoration(this.m_imageID);
    }
  }
}
