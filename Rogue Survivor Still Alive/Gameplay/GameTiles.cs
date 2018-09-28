﻿// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Gameplay.GameTiles
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Data;
using System.Drawing;

namespace djack.RogueSurvivor.Gameplay
{
  internal class GameTiles : TileModelDB
  {
    private static readonly Color DRK_GRAY1 = Color.DimGray;
    private static readonly Color DRK_GRAY2 = Color.DarkGray;
    private static readonly Color DRK_RED = Color.FromArgb(128, 0, 0);
    private static readonly Color LIT_GRAY1 = Color.Gray;
    private static readonly Color LIT_GRAY2 = Color.LightGray;
    private static readonly Color LIT_GRAY3 = Color.FromArgb(230, 230, 230);
    private static readonly Color LIT_BROWN = Color.BurlyWood;
    private TileModel[] m_Models = new TileModel[19];

    public override TileModel this[int id]
    {
      get
      {
        return this.m_Models[id];
      }
    }

    public TileModel this[GameTiles.IDs id]
    {
      get
      {
        return this[(int) id];
      }
      set
      {
        this.m_Models[(int) id] = value;
        this.m_Models[(int) id].ID = (int) id;
      }
    }

    public TileModel FLOOR_ASPHALT
    {
      get
      {
        return this[GameTiles.IDs.FLOOR_ASPHALT];
      }
    }

    public TileModel FLOOR_CONCRETE
    {
      get
      {
        return this[GameTiles.IDs.FLOOR_CONCRETE];
      }
    }

    public TileModel FLOOR_GRASS
    {
      get
      {
        return this[GameTiles.IDs.FLOOR_GRASS];
      }
    }

    public TileModel FLOOR_OFFICE
    {
      get
      {
        return this[GameTiles.IDs.FLOOR_OFFICE];
      }
    }

    public TileModel FLOOR_PLANKS
    {
      get
      {
        return this[GameTiles.IDs.FLOOR_PLANKS];
      }
    }

    public TileModel FLOOR_SEWER_WATER
    {
      get
      {
        return this[GameTiles.IDs.FLOOR_SEWER_WATER];
      }
    }

    public TileModel FLOOR_TILES
    {
      get
      {
        return this[GameTiles.IDs.FLOOR_TILES];
      }
    }

    public TileModel FLOOR_WALKWAY
    {
      get
      {
        return this[GameTiles.IDs.FLOOR_WALKWAY];
      }
    }

    public TileModel ROAD_ASPHALT_EW
    {
      get
      {
        return this[GameTiles.IDs.ROAD_ASPHALT_EW];
      }
    }

    public TileModel ROAD_ASPHALT_NS
    {
      get
      {
        return this[GameTiles.IDs.ROAD_ASPHALT_NS];
      }
    }

    public TileModel RAIL_EW
    {
      get
      {
        return this[GameTiles.IDs.RAIL_EW];
      }
    }

    public TileModel WALL_BRICK
    {
      get
      {
        return this[GameTiles.IDs.WALL_BRICK];
      }
    }

    public TileModel WALL_CHAR_OFFICE
    {
      get
      {
        return this[GameTiles.IDs.WALL_CHAR_OFFICE];
      }
    }

    public TileModel WALL_POLICE_STATION
    {
      get
      {
        return this[GameTiles.IDs.WALL_POLICE_STATION];
      }
    }

    public TileModel WALL_HOSPITAL
    {
      get
      {
        return this[GameTiles.IDs.WALL_HOSPITAL];
      }
    }

    public TileModel WALL_SEWER
    {
      get
      {
        return this[GameTiles.IDs.WALL_SEWER];
      }
    }

    public TileModel WALL_STONE
    {
      get
      {
        return this[GameTiles.IDs.WALL_STONE];
      }
    }

    public TileModel WALL_SUBWAY
    {
      get
      {
        return this[GameTiles.IDs.WALL_SUBWAY];
      }
    }

    public GameTiles()
    {
      Models.Tiles = (TileModelDB) this;
      this[GameTiles.IDs._FIRST] = TileModel.UNDEF;
      this[GameTiles.IDs.FLOOR_ASPHALT] = new TileModel("Tiles\\floor_asphalt", GameTiles.LIT_GRAY1, true, true);
      this[GameTiles.IDs.FLOOR_CONCRETE] = new TileModel("Tiles\\floor_concrete", GameTiles.LIT_GRAY2, true, true);
      this[GameTiles.IDs.FLOOR_GRASS] = new TileModel("Tiles\\floor_grass", Color.Green, true, true);
      this[GameTiles.IDs.FLOOR_OFFICE] = new TileModel("Tiles\\floor_office", GameTiles.LIT_GRAY3, true, true);
      this[GameTiles.IDs.FLOOR_PLANKS] = new TileModel("Tiles\\floor_planks", GameTiles.LIT_BROWN, true, true);
      this[GameTiles.IDs.FLOOR_SEWER_WATER] = new TileModel("Tiles\\floor_sewer_water", Color.Blue, true, true)
      {
        IsWater = true,
        WaterCoverImageID = "Tiles\\floor_sewer_water_cover"
      };
      this[GameTiles.IDs.FLOOR_TILES] = new TileModel("Tiles\\floor_tiles", GameTiles.LIT_GRAY2, true, true);
      this[GameTiles.IDs.FLOOR_WALKWAY] = new TileModel("Tiles\\floor_walkway", GameTiles.LIT_GRAY2, true, true);
      this[GameTiles.IDs.ROAD_ASPHALT_EW] = new TileModel("Tiles\\road_asphalt_ew", GameTiles.LIT_GRAY1, true, true);
      this[GameTiles.IDs.ROAD_ASPHALT_NS] = new TileModel("Tiles\\road_asphalt_ns", GameTiles.LIT_GRAY1, true, true);
      this[GameTiles.IDs.RAIL_EW] = new TileModel("Tiles\\rail_ew", GameTiles.LIT_GRAY1, true, true);
      this[GameTiles.IDs.WALL_BRICK] = new TileModel("Tiles\\wall_brick", GameTiles.DRK_GRAY1, false, false);
      this[GameTiles.IDs.WALL_CHAR_OFFICE] = new TileModel("Tiles\\wall_char_office", GameTiles.DRK_RED, false, false);
      this[GameTiles.IDs.WALL_HOSPITAL] = new TileModel("Tiles\\wall_hospital", Color.White, false, false);
      this[GameTiles.IDs.WALL_POLICE_STATION] = new TileModel("Tiles\\wall_stone", Color.CadetBlue, false, false);
      this[GameTiles.IDs.WALL_SEWER] = new TileModel("Tiles\\wall_sewer", Color.DarkGreen, false, false);
      this[GameTiles.IDs.WALL_STONE] = new TileModel("Tiles\\wall_stone", GameTiles.DRK_GRAY1, false, false);
      this[GameTiles.IDs.WALL_SUBWAY] = new TileModel("Tiles\\wall_stone", Color.Blue, false, false);
    }

    public bool IsRoadModel(TileModel model)
    {
      if (model != this[GameTiles.IDs.ROAD_ASPHALT_EW])
        return model == this[GameTiles.IDs.ROAD_ASPHALT_NS];
      return true;
    }

    public enum IDs
    {
      UNDEF = 0,
      _FIRST = 0,
      FLOOR_ASPHALT = 1,
      FLOOR_CONCRETE = 2,
      FLOOR_GRASS = 3,
      FLOOR_OFFICE = 4,
      FLOOR_PLANKS = 5,
      FLOOR_SEWER_WATER = 6,
      FLOOR_TILES = 7,
      FLOOR_WALKWAY = 8,
      ROAD_ASPHALT_EW = 9,
      ROAD_ASPHALT_NS = 10, // 0x0000000A
      RAIL_EW = 11, // 0x0000000B
      WALL_BRICK = 12, // 0x0000000C
      WALL_CHAR_OFFICE = 13, // 0x0000000D
      WALL_HOSPITAL = 14, // 0x0000000E
      WALL_POLICE_STATION = 15, // 0x0000000F
      WALL_SEWER = 16, // 0x00000010
      WALL_STONE = 17, // 0x00000011
      WALL_SUBWAY = 18, // 0x00000012
      _COUNT = 19, // 0x00000013
    }
  }
}
