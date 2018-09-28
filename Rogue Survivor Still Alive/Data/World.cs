// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Data.World
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using System;

namespace djack.RogueSurvivor.Data
{
  [Serializable]
  internal class World
  {
    private District[,] m_DistrictsGrid;
    private int m_Size;
    private Weather m_Weather;

    public int Size
    {
      get
      {
        return this.m_Size;
      }
    }

    public District this[int x, int y]
    {
      get
      {
        if (x < 0 || x >= this.m_Size)
          throw new ArgumentOutOfRangeException(nameof (x));
        if (y < 0 || y >= this.m_Size)
          throw new ArgumentOutOfRangeException(nameof (y));
        return this.m_DistrictsGrid[x, y];
      }
      set
      {
        if (x < 0 || x >= this.m_Size)
          throw new ArgumentOutOfRangeException(nameof (x));
        if (y < 0 || y >= this.m_Size)
          throw new ArgumentOutOfRangeException(nameof (y));
        this.m_DistrictsGrid[x, y] = value;
      }
    }

    public Weather Weather
    {
      get
      {
        return this.m_Weather;
      }
      set
      {
        this.m_Weather = value;
      }
    }

    public World(int size)
    {
      if (size <= 0)
        throw new ArgumentOutOfRangeException("size <=0");
      this.m_DistrictsGrid = new District[size, size];
      this.m_Size = size;
      this.m_Weather = Weather._FIRST;
    }

    public void TrimToBounds(ref int x, ref int y)
    {
      if (x < 0)
        x = 0;
      if (x >= this.m_Size)
        x = this.m_Size - 1;
      if (y < 0)
        y = 0;
      if (y < this.m_Size)
        return;
      y = this.m_Size - 1;
    }

    public static string CoordToString(int x, int y)
    {
      return string.Format("{0}{1}", (object) (char) (65 + x), (object) y);
    }

    public void OptimizeBeforeSaving()
    {
      for (int index1 = 0; index1 < this.m_Size; ++index1)
      {
        for (int index2 = 0; index2 < this.m_Size; ++index2)
          this.m_DistrictsGrid[index1, index2].OptimizeBeforeSaving();
      }
    }
  }
}
