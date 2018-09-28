// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Data.TileModel
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using System.Drawing;

namespace djack.RogueSurvivor.Data
{
  internal class TileModel
  {
    public static readonly TileModel UNDEF = new TileModel("", Color.Pink, false, true);
    private int m_ID;
    private string m_ImageID;
    private bool m_IsWalkable;
    private bool m_IsTransparent;
    private Color m_MinimapColor;

    public int ID
    {
      get
      {
        return this.m_ID;
      }
      set
      {
        this.m_ID = value;
      }
    }

    public string ImageID
    {
      get
      {
        return this.m_ImageID;
      }
    }

    public bool IsWalkable
    {
      get
      {
        return this.m_IsWalkable;
      }
    }

    public bool IsTransparent
    {
      get
      {
        return this.m_IsTransparent;
      }
    }

    public Color MinimapColor
    {
      get
      {
        return this.m_MinimapColor;
      }
    }

    public bool IsWater { get; set; }

    public string WaterCoverImageID { get; set; }

    public TileModel(string imageID, Color minimapColor, bool IsWalkable, bool IsTransparent)
    {
      this.m_ImageID = imageID;
      this.m_IsWalkable = IsWalkable;
      this.m_IsTransparent = IsTransparent;
      this.m_MinimapColor = minimapColor;
    }
  }
}
