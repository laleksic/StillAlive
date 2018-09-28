// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Data.Corpse
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using System;
using System.Drawing;

namespace djack.RogueSurvivor.Data
{
  [Serializable]
  internal class Corpse
  {
    private Actor m_DeadGuy;
    private int m_Turn;
    private Point m_Position;
    private float m_HitPoints;
    private int m_MaxHitPoints;
    private float m_Rotation;
    private float m_Scale;
    private Actor m_DraggedBy;

    public Actor DeadGuy
    {
      get
      {
        return this.m_DeadGuy;
      }
    }

    public int Turn
    {
      get
      {
        return this.m_Turn;
      }
    }

    public Point Position
    {
      get
      {
        return this.m_Position;
      }
      set
      {
        this.m_Position = value;
      }
    }

    public float HitPoints
    {
      get
      {
        return this.m_HitPoints;
      }
      set
      {
        this.m_HitPoints = value;
      }
    }

    public int MaxHitPoints
    {
      get
      {
        return this.m_MaxHitPoints;
      }
    }

    public float Rotation
    {
      get
      {
        return this.m_Rotation;
      }
      set
      {
        this.m_Rotation = value;
      }
    }

    public float Scale
    {
      get
      {
        return this.m_Scale;
      }
      set
      {
        this.m_Scale = Math.Max(0.0f, Math.Min(1f, value));
      }
    }

    public bool IsDragged
    {
      get
      {
        if (this.m_DraggedBy != null)
          return !this.m_DraggedBy.IsDead;
        return false;
      }
    }

    public Actor DraggedBy
    {
      get
      {
        return this.m_DraggedBy;
      }
      set
      {
        this.m_DraggedBy = value;
      }
    }

    public Corpse(Actor deadGuy, int hitPoints, int maxHitPoints, int corpseTurn, float rotation, float scale)
    {
      this.m_DeadGuy = deadGuy;
      this.m_Turn = corpseTurn;
      this.m_HitPoints = (float) hitPoints;
      this.m_MaxHitPoints = maxHitPoints;
      this.m_Rotation = rotation;
      this.m_Scale = scale;
      this.m_DraggedBy = (Actor) null;
    }
  }
}
