using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using djack.RogueSurvivor.Data;
using djack.RogueSurvivor.Engine;

namespace djack.RogueSurvivor.Gameplay
{
    class GameTiles : TileModelDB
    {
        #region IDs
        public enum IDs
        {
            _FIRST = 0,

            UNDEF = _FIRST,

            FLOOR_ASPHALT,
            FLOOR_CONCRETE,
            FLOOR_GRASS,
            FLOOR_OFFICE,
            FLOOR_PLANKS,
            FLOOR_PLANTED, //@@MP (Release 5-5)
            FLOOR_SEWER_WATER,
            FLOOR_TILES,
            FLOOR_WALKWAY,
            //@@MP (Release 4)
            FLOOR_RED_CARPET,
            FLOOR_BLUE_CARPET,
            FLOOR_DIRT,
            //@@MP (Release 6-1)
            FLOOR_POND_CENTER,
            FLOOR_POND_N_EDGE,
            FLOOR_POND_NE_CORNER,
            FLOOR_POND_E_EDGE,
            FLOOR_POND_SE_CORNER,
            FLOOR_POND_S_EDGE,
            FLOOR_POND_SW_CORNER,
            FLOOR_POND_W_EDGE,
            FLOOR_POND_NW_CORNER,

            ROAD_ASPHALT_EW,
            ROAD_ASPHALT_NS,
            RAIL_EW,

            WALL_BRICK,
            WALL_CHAR_OFFICE,
            WALL_HOSPITAL,
            WALL_POLICE_STATION,
            WALL_SEWER,
            WALL_STONE,
            WALL_SUBWAY,
            WALL_LIGHT_BROWN, //@@MP (Release 4)

            _COUNT
        }
        #endregion

        #region Custom Colors
        static readonly Color DRK_RED = Color.FromArgb(128, 0, 0);
        static readonly Color LIT_GRAY = Color.FromArgb(230, 230, 230);
        #endregion

        #region Fields
        TileModel[] m_Models = new TileModel[(int) IDs._COUNT];
        #endregion

        #region Properties
        public override TileModel this[int id]
        {
            get { return m_Models[id]; }
        }

        public TileModel this[IDs id]
        {
            get { return this[(int)id]; }
            set
            {
                m_Models[(int)id] = value;
                m_Models[(int)id].ID = (int)id;
            }
        }

        public TileModel FLOOR_ASPHALT { get { return this[IDs.FLOOR_ASPHALT]; } }
        public TileModel FLOOR_CONCRETE { get { return this[IDs.FLOOR_CONCRETE]; } }
        public TileModel FLOOR_GRASS { get { return this[IDs.FLOOR_GRASS]; } }
        public TileModel FLOOR_OFFICE { get { return this[IDs.FLOOR_OFFICE]; } }
        public TileModel FLOOR_PLANKS { get { return this[IDs.FLOOR_PLANKS]; } }
        public TileModel FLOOR_PLANTED { get { return this[IDs.FLOOR_PLANTED]; } } //@@MP (Release 5-5)
        public TileModel FLOOR_SEWER_WATER { get { return this[IDs.FLOOR_SEWER_WATER]; } }
        public TileModel FLOOR_TILES { get { return this[IDs.FLOOR_TILES]; } }
        public TileModel FLOOR_WALKWAY { get { return this[IDs.FLOOR_WALKWAY]; } }
        //@@MP (Release 4)
        public TileModel FLOOR_RED_CARPET { get { return this[IDs.FLOOR_RED_CARPET]; } }
        public TileModel FLOOR_BLUE_CARPET { get { return this[IDs.FLOOR_BLUE_CARPET]; } }
        public TileModel FLOOR_DIRT { get { return this[IDs.FLOOR_DIRT]; } }
        //@@MP (Release 6-1)
        public TileModel FLOOR_POND_CENTER { get { return this[IDs.FLOOR_POND_CENTER]; } }
        public TileModel FLOOR_POND_N_EDGE { get { return this[IDs.FLOOR_POND_N_EDGE]; } }
        public TileModel FLOOR_POND_NE_CORNER { get { return this[IDs.FLOOR_POND_NE_CORNER]; } }
        public TileModel FLOOR_POND_E_EDGE { get { return this[IDs.FLOOR_POND_E_EDGE]; } }
        public TileModel FLOOR_POND_SE_CORNER { get { return this[IDs.FLOOR_POND_SE_CORNER]; } }
        public TileModel FLOOR_POND_S_EDGE { get { return this[IDs.FLOOR_POND_S_EDGE]; } }
        public TileModel FLOOR_POND_SW_CORNER { get { return this[IDs.FLOOR_POND_SW_CORNER]; } }
        public TileModel FLOOR_POND_W_EDGE { get { return this[IDs.FLOOR_POND_W_EDGE]; } }
        public TileModel FLOOR_POND_NW_CORNER { get { return this[IDs.FLOOR_POND_NW_CORNER]; } }

        public TileModel ROAD_ASPHALT_EW { get { return this[IDs.ROAD_ASPHALT_EW]; } }
        public TileModel ROAD_ASPHALT_NS { get { return this[IDs.ROAD_ASPHALT_NS]; } }
        public TileModel RAIL_EW { get { return this[IDs.RAIL_EW]; } }

        public TileModel WALL_BRICK { get { return this[IDs.WALL_BRICK]; } }
        public TileModel WALL_CHAR_OFFICE { get { return this[IDs.WALL_CHAR_OFFICE]; } }
        public TileModel WALL_POLICE_STATION { get { return this[IDs.WALL_POLICE_STATION]; } }
        public TileModel WALL_HOSPITAL { get { return this[IDs.WALL_HOSPITAL]; } }
        public TileModel WALL_SEWER { get { return this[IDs.WALL_SEWER]; } }
        public TileModel WALL_STONE { get { return this[IDs.WALL_STONE]; } }
        public TileModel WALL_SUBWAY { get { return this[IDs.WALL_SUBWAY]; } }
        public TileModel WALL_LIGHT_BROWN { get { return this[IDs.WALL_LIGHT_BROWN]; } } //@@MP (Release 4)
        #endregion

        #region Init
        public GameTiles()
        {
            // bind.
            Models.Tiles = this;

            this[IDs.UNDEF] = TileModel.UNDEF;

            #region Floors
            //@MP if you add another also add it to IsFloorModel()
            this[IDs.FLOOR_ASPHALT] = new TileModel(GameImages.TILE_FLOOR_ASPHALT, Color.Gray, true, true);
            this[IDs.FLOOR_CONCRETE] = new TileModel(GameImages.TILE_FLOOR_CONCRETE, Color.LightGray, true, true);
            this[IDs.FLOOR_GRASS] = new TileModel(GameImages.TILE_FLOOR_GRASS, Color.Green, true, true);
            this[IDs.FLOOR_OFFICE] = new TileModel(GameImages.TILE_FLOOR_OFFICE, Color.IndianRed, true, true); //@@MP - changed to red minimap colour (Release 4)
            this[IDs.FLOOR_PLANKS] = new TileModel(GameImages.TILE_FLOOR_PLANKS, Color.Chocolate, true, true); //@@MP - changed from LIT_BROWN to Chocolate (Release 4)
            this[IDs.FLOOR_PLANTED] = new TileModel(GameImages.TILE_FLOOR_PLANTED, Color.Green, true, true); //@@MP (Release 5-5)
            this[IDs.FLOOR_SEWER_WATER] = new TileModel(GameImages.TILE_FLOOR_SEWER_WATER, Color.Blue, true, true) { IsWater = true, WaterCoverImageID = GameImages.TILE_FLOOR_SEWER_WATER_COVER };
            this[IDs.FLOOR_TILES] = new TileModel(GameImages.TILE_FLOOR_TILES, Color.LightGray, true, true);
            this[IDs.FLOOR_WALKWAY] = new TileModel(GameImages.TILE_FLOOR_WALKWAY, Color.LightGray, true, true);
            this[IDs.FLOOR_RED_CARPET] = new TileModel(GameImages.TILE_FLOOR_RED_CARPET, DRK_RED, true, true); //@@MP (Release 4)
            this[IDs.FLOOR_BLUE_CARPET] = new TileModel(GameImages.TILE_FLOOR_BLUE_CARPET, Color.SteelBlue, true, true); //@@MP (Release 4)
            this[IDs.FLOOR_DIRT] = new TileModel(GameImages.TILE_FLOOR_DIRT, Color.Sienna, true, true); //@@MP (Release 4)
            this[IDs.ROAD_ASPHALT_EW] = new TileModel(GameImages.TILE_ROAD_ASPHALT_EW, Color.Gray, true, true);
            this[IDs.ROAD_ASPHALT_NS] = new TileModel(GameImages.TILE_ROAD_ASPHALT_NS, Color.Gray, true, true);
            this[IDs.RAIL_EW] = new TileModel(GameImages.TILE_RAIL_ES, Color.Gray, true, true);
            //@@MP (Release 6-1)
            this[IDs.FLOOR_POND_CENTER] = new TileModel(GameImages.TILE_FLOOR_POND_CENTER, Color.Blue, true, true) { IsWater = true, WaterCoverImageID = GameImages.TILE_FLOOR_POND_WATER_COVER } ;
            this[IDs.FLOOR_POND_NE_CORNER] = new TileModel(GameImages.TILE_FLOOR_POND_NE_CORNER, Color.Blue, true, true) { IsWater = true, WaterCoverImageID = GameImages.TILE_FLOOR_POND_WATER_COVER };
            this[IDs.FLOOR_POND_E_EDGE] = new TileModel(GameImages.TILE_FLOOR_POND_E_EDGE, Color.Blue, true, true) { IsWater = true, WaterCoverImageID = GameImages.TILE_FLOOR_POND_WATER_COVER };
            this[IDs.FLOOR_POND_SE_CORNER] = new TileModel(GameImages.TILE_FLOOR_POND_SE_CORNER, Color.Blue, true, true) { IsWater = true, WaterCoverImageID = GameImages.TILE_FLOOR_POND_WATER_COVER };
            this[IDs.FLOOR_POND_S_EDGE] = new TileModel(GameImages.TILE_FLOOR_POND_S_EDGE, Color.Blue, true, true) { IsWater = true, WaterCoverImageID = GameImages.TILE_FLOOR_POND_WATER_COVER };
            this[IDs.FLOOR_POND_SW_CORNER] = new TileModel(GameImages.TILE_FLOOR_POND_SW_CORNER, Color.Blue, true, true) { IsWater = true, WaterCoverImageID = GameImages.TILE_FLOOR_POND_WATER_COVER };
            this[IDs.FLOOR_POND_W_EDGE] = new TileModel(GameImages.TILE_FLOOR_POND_W_EDGE, Color.Blue, true, true) { IsWater = true, WaterCoverImageID = GameImages.TILE_FLOOR_POND_WATER_COVER };
            this[IDs.FLOOR_POND_NW_CORNER] = new TileModel(GameImages.TILE_FLOOR_POND_NW_CORNER, Color.Blue, true, true) { IsWater = true, WaterCoverImageID = GameImages.TILE_FLOOR_POND_WATER_COVER };
            this[IDs.FLOOR_POND_N_EDGE] = new TileModel(GameImages.TILE_FLOOR_POND_N_EDGE, Color.Blue, true, true) { IsWater = true, WaterCoverImageID = GameImages.TILE_FLOOR_POND_WATER_COVER };
            #endregion

            #region Walls
            //@MP if you add another also add it to IsWallModel()
            this[IDs.WALL_BRICK] = new TileModel(GameImages.TILE_WALL_BRICK, Color.DimGray, false, false);
            this[IDs.WALL_CHAR_OFFICE] = new TileModel(GameImages.TILE_WALL_CHAR_OFFICE, DRK_RED, false, false);
            this[IDs.WALL_HOSPITAL] = new TileModel(GameImages.TILE_WALL_HOSPITAL, Color.White, false, false);
            this[IDs.WALL_POLICE_STATION] = new TileModel(GameImages.TILE_WALL_STONE, Color.CadetBlue, false, false);
            this[IDs.WALL_SEWER] = new TileModel(GameImages.TILE_WALL_SEWER, Color.Olive, false, false); //@@MP - changed from DarkGreen to Olive (Release 4)
            this[IDs.WALL_STONE] = new TileModel(GameImages.TILE_WALL_STONE, Color.DimGray, false, false);
            this[IDs.WALL_SUBWAY] = new TileModel(GameImages.TILE_WALL_STONE, Color.Blue, false, false);
            this[IDs.WALL_LIGHT_BROWN] = new TileModel(GameImages.TILE_WALL_LIGHT_BROWN, Color.BurlyWood, false, false); //@@MP (Release 4)
            #endregion
        }

        #endregion

        #region Helpers
        public bool IsRoadModel(TileModel model)
        {
            return model == this[IDs.ROAD_ASPHALT_EW] || model == this[IDs.ROAD_ASPHALT_NS];
        }

        public bool IsWallModel(TileModel model) //@MP (Release 6-3)
        {
            return model == this[IDs.WALL_BRICK] || model == this[IDs.WALL_CHAR_OFFICE] || model == this[IDs.WALL_HOSPITAL] || model == this[IDs.WALL_LIGHT_BROWN] ||
                model == this[IDs.WALL_POLICE_STATION] || model == this[IDs.WALL_STONE] || model == this[IDs.WALL_SUBWAY];
        }

        public bool IsFloorModel(TileModel model) //@MP (Release 6-3)
        {
            return model == this[IDs.FLOOR_ASPHALT] || model == this[IDs.FLOOR_BLUE_CARPET] || model == this[IDs.FLOOR_CONCRETE] || model == this[IDs.FLOOR_DIRT] ||
                model == this[IDs.FLOOR_GRASS] || model == this[IDs.FLOOR_OFFICE] || model == this[IDs.FLOOR_PLANKS] || model == this[IDs.FLOOR_PLANTED] ||
                model == this[IDs.FLOOR_RED_CARPET] || model == this[IDs.FLOOR_TILES] || model == this[IDs.FLOOR_WALKWAY] || model == this[IDs.RAIL_EW] ||
                model == this[IDs.ROAD_ASPHALT_EW] || model == this[IDs.ROAD_ASPHALT_NS];
        }
        #endregion
    }
}

