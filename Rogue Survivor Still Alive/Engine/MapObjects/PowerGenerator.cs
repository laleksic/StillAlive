// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.MapObjects.PowerGenerator
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Data;
using System;

namespace djack.RogueSurvivor.Engine.MapObjects
{
  [Serializable]
  internal class PowerGenerator : StateMapObject
  {
    public const int STATE_OFF = 0;
    public const int STATE_ON = 1;
    private string m_OffImageID;
    private string m_OnImageID;

    public bool IsOn
    {
      get
      {
        return this.State == 1;
      }
    }

    public PowerGenerator(string name, string offImageID, string onImageID)
      : base(name, offImageID)
    {
      this.m_OffImageID = offImageID;
      this.m_OnImageID = onImageID;
    }

    public override void SetState(int newState)
    {
      base.SetState(newState);
      if (newState != 0)
      {
        if (newState != 1)
          throw new ArgumentOutOfRangeException("unhandled state");
        this.ImageID = this.m_OnImageID;
      }
      else
        this.ImageID = this.m_OffImageID;
    }

    public void TogglePower()
    {
      this.SetState(this.State == 0 ? 1 : 0);
    }
  }
}
