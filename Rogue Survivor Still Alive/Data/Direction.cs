// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Data.Direction
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using System;
using System.Collections.Generic;
using System.Drawing;

namespace djack.RogueSurvivor.Data
{
  [Serializable]
  internal sealed class Direction
  {
    public static readonly Direction NEUTRAL = new Direction(-1, "neutral", new Point(0, 0));
    public static readonly Direction N = new Direction(0, nameof (N), new Point(0, -1));
    public static readonly Direction NE = new Direction(1, nameof (NE), new Point(1, -1));
    public static readonly Direction E = new Direction(2, nameof (E), new Point(1, 0));
    public static readonly Direction SE = new Direction(3, nameof (SE), new Point(1, 1));
    public static readonly Direction S = new Direction(4, nameof (S), new Point(0, 1));
    public static readonly Direction SW = new Direction(5, nameof (SW), new Point(-1, 1));
    public static readonly Direction W = new Direction(6, nameof (W), new Point(-1, 0));
    public static readonly Direction NW = new Direction(7, nameof (NW), new Point(-1, -1));
    public static readonly Direction[] COMPASS = new Direction[8]
    {
      Direction.N,
      Direction.NE,
      Direction.E,
      Direction.SE,
      Direction.S,
      Direction.SW,
      Direction.W,
      Direction.NW
    };
    public static readonly List<Direction> COMPASS_LIST = new List<Direction>()
    {
      Direction.N,
      Direction.NE,
      Direction.E,
      Direction.SE,
      Direction.S,
      Direction.SW,
      Direction.W,
      Direction.NW
    };
    public static readonly Direction[] COMPASS_4 = new Direction[4]
    {
      Direction.N,
      Direction.E,
      Direction.S,
      Direction.W
    };
    private int m_Index;
    private string m_Name;
    private Point m_Vector;
    private PointF m_NormalizedVector;

    public static Direction FromVector(Point v)
    {
      foreach (Direction direction in Direction.COMPASS)
      {
        if (direction.Vector == v)
          return direction;
      }
      return (Direction) null;
    }

    public static Direction FromVector(int vx, int vy)
    {
      foreach (Direction direction in Direction.COMPASS)
      {
        if (direction.Vector.X == vx & direction.Vector.Y == vy)
          return direction;
      }
      return (Direction) null;
    }

    public static Direction ApproximateFromVector(Point v)
    {
      PointF pointF = (PointF) v;
      float num1 = (float) Math.Sqrt((double) pointF.X * (double) pointF.X + (double) pointF.Y * (double) pointF.Y);
      if ((double) num1 == 0.0)
        return Direction.N;
      pointF.X /= num1;
      pointF.Y /= num1;
      float num2 = float.MaxValue;
      Direction direction1 = Direction.N;
      foreach (Direction direction2 in Direction.COMPASS)
      {
        float num3 = Math.Abs(pointF.X - direction2.NormalizedVector.X) + Math.Abs(pointF.Y - direction2.NormalizedVector.Y);
        if ((double) num3 < (double) num2)
        {
          direction1 = direction2;
          num2 = num3;
        }
      }
      return direction1;
    }

    public static Direction Right(Direction d)
    {
      return Direction.COMPASS[(d.m_Index + 1) % 8];
    }

    public static Direction Left(Direction d)
    {
      return Direction.COMPASS[(d.m_Index - 1) % 8];
    }

    public static Direction Opposite(Direction d)
    {
      return Direction.COMPASS[(d.m_Index + 4) % 8];
    }

    public static Point operator +(Point lhs, Direction rhs)
    {
      return new Point(lhs.X + rhs.Vector.X, lhs.Y + rhs.Vector.Y);
    }

    public int Index
    {
      get
      {
        return this.m_Index;
      }
    }

    public string Name
    {
      get
      {
        return this.m_Name;
      }
    }

    public Point Vector
    {
      get
      {
        return this.m_Vector;
      }
    }

    public PointF NormalizedVector
    {
      get
      {
        return this.m_NormalizedVector;
      }
    }

    private Direction(int index, string name, Point vector)
    {
      this.m_Index = index;
      this.m_Name = name;
      this.m_Vector = vector;
      float num = (float) Math.Sqrt((double) (vector.X * vector.X + vector.Y * vector.Y));
      if ((double) num != 0.0)
        this.m_NormalizedVector = new PointF((float) vector.X / num, (float) vector.Y / num);
      else
        this.m_NormalizedVector = PointF.Empty;
    }

    public override string ToString()
    {
      return this.m_Name;
    }
  }
}
