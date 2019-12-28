using System.Drawing;

using djack.RogueSurvivor.Data;

namespace djack.RogueSurvivor.Gameplay
{
    class GameTiles : TileModelDB
    {
        #region IDs
        public enum IDs
        {
            _FIRST = 0,

            UNDEF = _FIRST,

            FLOOR_ARMY, //@@MP (Release 6-3)
            FLOOR_ASPHALT,
            FLOOR_CONCRETE,
            FLOOR_GRASS,
            FLOOR_OFFICE,
            FLOOR_PLANKS,
            FLOOR_PLANTED, //@@MP (Release 5-5)
            FLOOR_SEWER_WATER,
            FLOOR_TILES,
            FLOOR_WALKWAY,
            FLOOR_WHITE_TILE, //@@MP (Release 7-3)
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
            //@@MP (Release 7-3)
            FLOOR_FOOD_COURT_POOL,
            //is there a better way of doing these?
            #region -Tennis court
            FLOOR_TENNIS_COURT_OUTER,
            FLOOR_TENNIS_COURT_10,
            FLOOR_TENNIS_COURT_11,
            FLOOR_TENNIS_COURT_12,
            FLOOR_TENNIS_COURT_13,
            FLOOR_TENNIS_COURT_14,
            FLOOR_TENNIS_COURT_15,
            FLOOR_TENNIS_COURT_18,
            FLOOR_TENNIS_COURT_19,
            FLOOR_TENNIS_COURT_20,
            FLOOR_TENNIS_COURT_21,
            FLOOR_TENNIS_COURT_22,
            FLOOR_TENNIS_COURT_23,
            FLOOR_TENNIS_COURT_26,
            FLOOR_TENNIS_COURT_27,
            FLOOR_TENNIS_COURT_28,
            FLOOR_TENNIS_COURT_29,
            FLOOR_TENNIS_COURT_30,
            FLOOR_TENNIS_COURT_31,
            FLOOR_TENNIS_COURT_34,
            FLOOR_TENNIS_COURT_35,
            FLOOR_TENNIS_COURT_36,
            FLOOR_TENNIS_COURT_37,
            FLOOR_TENNIS_COURT_38,
            FLOOR_TENNIS_COURT_39,
            FLOOR_TENNIS_COURT_42,
            FLOOR_TENNIS_COURT_43,
            FLOOR_TENNIS_COURT_44,
            FLOOR_TENNIS_COURT_45,
            FLOOR_TENNIS_COURT_46,
            FLOOR_TENNIS_COURT_47,
            FLOOR_TENNIS_COURT_50,
            FLOOR_TENNIS_COURT_51,
            FLOOR_TENNIS_COURT_52,
            FLOOR_TENNIS_COURT_53,
            FLOOR_TENNIS_COURT_54,
            FLOOR_TENNIS_COURT_55,
            FLOOR_TENNIS_COURT_58,
            FLOOR_TENNIS_COURT_59,
            FLOOR_TENNIS_COURT_60,
            FLOOR_TENNIS_COURT_61,
            FLOOR_TENNIS_COURT_62,
            FLOOR_TENNIS_COURT_63,
            FLOOR_TENNIS_COURT_66,
            FLOOR_TENNIS_COURT_67,
            FLOOR_TENNIS_COURT_68,
            FLOOR_TENNIS_COURT_69,
            FLOOR_TENNIS_COURT_70,
            FLOOR_TENNIS_COURT_71,
            #endregion
            #region -Basketball court
            FLOOR_BASKETBALL_COURT_OUTER,
            FLOOR_BASKETBALL_COURT_18,
            FLOOR_BASKETBALL_COURT_19,
            FLOOR_BASKETBALL_COURT_20,
            FLOOR_BASKETBALL_COURT_21,
            FLOOR_BASKETBALL_COURT_22,
            FLOOR_BASKETBALL_COURT_23,
            FLOOR_BASKETBALL_COURT_24,
            FLOOR_BASKETBALL_COURT_25,
            FLOOR_BASKETBALL_COURT_27,
            FLOOR_BASKETBALL_COURT_28,
            FLOOR_BASKETBALL_COURT_29,
            FLOOR_BASKETBALL_COURT_30,
            FLOOR_BASKETBALL_COURT_31,
            FLOOR_BASKETBALL_COURT_32,
            FLOOR_BASKETBALL_COURT_33,
            FLOOR_BASKETBALL_COURT_34,
            FLOOR_BASKETBALL_COURT_36,
            FLOOR_BASKETBALL_COURT_37,
            FLOOR_BASKETBALL_COURT_38,
            FLOOR_BASKETBALL_COURT_39,
            FLOOR_BASKETBALL_COURT_40,
            FLOOR_BASKETBALL_COURT_41,
            FLOOR_BASKETBALL_COURT_42,
            FLOOR_BASKETBALL_COURT_43,
            FLOOR_BASKETBALL_COURT_45,
            FLOOR_BASKETBALL_COURT_46,
            FLOOR_BASKETBALL_COURT_47,
            FLOOR_BASKETBALL_COURT_48,
            FLOOR_BASKETBALL_COURT_49,
            FLOOR_BASKETBALL_COURT_50,
            FLOOR_BASKETBALL_COURT_51,
            FLOOR_BASKETBALL_COURT_52,
            FLOOR_BASKETBALL_COURT_54,
            FLOOR_BASKETBALL_COURT_55,
            FLOOR_BASKETBALL_COURT_56,
            FLOOR_BASKETBALL_COURT_57,
            FLOOR_BASKETBALL_COURT_58,
            FLOOR_BASKETBALL_COURT_59,
            FLOOR_BASKETBALL_COURT_60,
            FLOOR_BASKETBALL_COURT_61,
            FLOOR_BASKETBALL_COURT_63,
            FLOOR_BASKETBALL_COURT_64,
            FLOOR_BASKETBALL_COURT_65,
            FLOOR_BASKETBALL_COURT_66,
            FLOOR_BASKETBALL_COURT_67,
            FLOOR_BASKETBALL_COURT_68,
            FLOOR_BASKETBALL_COURT_69,
            FLOOR_BASKETBALL_COURT_70,
            #endregion

            PARKING_ASPHALT_EW,//@@MP (Release 7-3)
            PARKING_ASPHALT_NS,//@@MP (Release 7-3)
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
            WALL_ARMY_BASE, //@@MP (Release 6-3)
            WALL_FUEL_STATION, //@@MP (Release 7-3)
            WALL_WOOD_PLANKS, //@@MP (Release 7-3)
            WALL_CONCRETE, //@@MP (Release 7-3)
            WALL_PILLAR_CONCRETE, //@@MP (Release 7-3)
            WALL_MALL, //@@MP (Release 7-3)
            WALL_RED_CURTAINS, //@@MP (Release 7-3)

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

        public TileModel FLOOR_ARMY { get { return this[IDs.FLOOR_ARMY]; } } //@@MP (Release 6-3)
        public TileModel FLOOR_ASPHALT { get { return this[IDs.FLOOR_ASPHALT]; } }
        public TileModel FLOOR_CONCRETE { get { return this[IDs.FLOOR_CONCRETE]; } }
        public TileModel FLOOR_GRASS { get { return this[IDs.FLOOR_GRASS]; } }
        public TileModel FLOOR_OFFICE { get { return this[IDs.FLOOR_OFFICE]; } }
        public TileModel FLOOR_PLANKS { get { return this[IDs.FLOOR_PLANKS]; } }
        public TileModel FLOOR_PLANTED { get { return this[IDs.FLOOR_PLANTED]; } } //@@MP (Release 5-5)
        public TileModel FLOOR_SEWER_WATER { get { return this[IDs.FLOOR_SEWER_WATER]; } }
        public TileModel FLOOR_TILES { get { return this[IDs.FLOOR_TILES]; } }
        public TileModel FLOOR_WALKWAY { get { return this[IDs.FLOOR_WALKWAY]; } }
        public TileModel FLOOR_WHITE_TILE { get { return this[IDs.FLOOR_WHITE_TILE]; } } //@@MP (Release 7-3)
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
        //@@MP (Release 7-3)
        public TileModel FLOOR_FOOD_COURT_POOL { get { return this[IDs.FLOOR_FOOD_COURT_POOL]; } }
        // have to be tiles because mapobjects and decorations can be destroyed
        #region -Tennis court
        public TileModel FLOOR_TENNIS_COURT_OUTER { get { return this[IDs.FLOOR_TENNIS_COURT_OUTER]; } }
        public TileModel FLOOR_TENNIS_COURT_10 { get { return this[IDs.FLOOR_TENNIS_COURT_10]; } }
        public TileModel FLOOR_TENNIS_COURT_11 { get { return this[IDs.FLOOR_TENNIS_COURT_11]; } }
        public TileModel FLOOR_TENNIS_COURT_12 { get { return this[IDs.FLOOR_TENNIS_COURT_12]; } }
        public TileModel FLOOR_TENNIS_COURT_13 { get { return this[IDs.FLOOR_TENNIS_COURT_13]; } }
        public TileModel FLOOR_TENNIS_COURT_14 { get { return this[IDs.FLOOR_TENNIS_COURT_14]; } }
        public TileModel FLOOR_TENNIS_COURT_15 { get { return this[IDs.FLOOR_TENNIS_COURT_15]; } }
        public TileModel FLOOR_TENNIS_COURT_18 { get { return this[IDs.FLOOR_TENNIS_COURT_18]; } }
        public TileModel FLOOR_TENNIS_COURT_19 { get { return this[IDs.FLOOR_TENNIS_COURT_19]; } }
        public TileModel FLOOR_TENNIS_COURT_20 { get { return this[IDs.FLOOR_TENNIS_COURT_20]; } }
        public TileModel FLOOR_TENNIS_COURT_21 { get { return this[IDs.FLOOR_TENNIS_COURT_21]; } }
        public TileModel FLOOR_TENNIS_COURT_22 { get { return this[IDs.FLOOR_TENNIS_COURT_22]; } }
        public TileModel FLOOR_TENNIS_COURT_23 { get { return this[IDs.FLOOR_TENNIS_COURT_23]; } }
        public TileModel FLOOR_TENNIS_COURT_26 { get { return this[IDs.FLOOR_TENNIS_COURT_26]; } }
        public TileModel FLOOR_TENNIS_COURT_27 { get { return this[IDs.FLOOR_TENNIS_COURT_27]; } }
        public TileModel FLOOR_TENNIS_COURT_28 { get { return this[IDs.FLOOR_TENNIS_COURT_28]; } }
        public TileModel FLOOR_TENNIS_COURT_29 { get { return this[IDs.FLOOR_TENNIS_COURT_29]; } }
        public TileModel FLOOR_TENNIS_COURT_30 { get { return this[IDs.FLOOR_TENNIS_COURT_30]; } }
        public TileModel FLOOR_TENNIS_COURT_31 { get { return this[IDs.FLOOR_TENNIS_COURT_31]; } }
        public TileModel FLOOR_TENNIS_COURT_34 { get { return this[IDs.FLOOR_TENNIS_COURT_34]; } }
        public TileModel FLOOR_TENNIS_COURT_35 { get { return this[IDs.FLOOR_TENNIS_COURT_35]; } }
        public TileModel FLOOR_TENNIS_COURT_36 { get { return this[IDs.FLOOR_TENNIS_COURT_36]; } }
        public TileModel FLOOR_TENNIS_COURT_37 { get { return this[IDs.FLOOR_TENNIS_COURT_37]; } }
        public TileModel FLOOR_TENNIS_COURT_38 { get { return this[IDs.FLOOR_TENNIS_COURT_38]; } }
        public TileModel FLOOR_TENNIS_COURT_39 { get { return this[IDs.FLOOR_TENNIS_COURT_39]; } }
        public TileModel FLOOR_TENNIS_COURT_42 { get { return this[IDs.FLOOR_TENNIS_COURT_42]; } }
        public TileModel FLOOR_TENNIS_COURT_43 { get { return this[IDs.FLOOR_TENNIS_COURT_43]; } }
        public TileModel FLOOR_TENNIS_COURT_44 { get { return this[IDs.FLOOR_TENNIS_COURT_44]; } }
        public TileModel FLOOR_TENNIS_COURT_45 { get { return this[IDs.FLOOR_TENNIS_COURT_45]; } }
        public TileModel FLOOR_TENNIS_COURT_46 { get { return this[IDs.FLOOR_TENNIS_COURT_46]; } }
        public TileModel FLOOR_TENNIS_COURT_47 { get { return this[IDs.FLOOR_TENNIS_COURT_47]; } }
        public TileModel FLOOR_TENNIS_COURT_50 { get { return this[IDs.FLOOR_TENNIS_COURT_50]; } }
        public TileModel FLOOR_TENNIS_COURT_51 { get { return this[IDs.FLOOR_TENNIS_COURT_51]; } }
        public TileModel FLOOR_TENNIS_COURT_52 { get { return this[IDs.FLOOR_TENNIS_COURT_52]; } }
        public TileModel FLOOR_TENNIS_COURT_53 { get { return this[IDs.FLOOR_TENNIS_COURT_53]; } }
        public TileModel FLOOR_TENNIS_COURT_54 { get { return this[IDs.FLOOR_TENNIS_COURT_54]; } }
        public TileModel FLOOR_TENNIS_COURT_55 { get { return this[IDs.FLOOR_TENNIS_COURT_55]; } }
        public TileModel FLOOR_TENNIS_COURT_58 { get { return this[IDs.FLOOR_TENNIS_COURT_58]; } }
        public TileModel FLOOR_TENNIS_COURT_59 { get { return this[IDs.FLOOR_TENNIS_COURT_59]; } }
        public TileModel FLOOR_TENNIS_COURT_60 { get { return this[IDs.FLOOR_TENNIS_COURT_60]; } }
        public TileModel FLOOR_TENNIS_COURT_61 { get { return this[IDs.FLOOR_TENNIS_COURT_61]; } }
        public TileModel FLOOR_TENNIS_COURT_62 { get { return this[IDs.FLOOR_TENNIS_COURT_62]; } }
        public TileModel FLOOR_TENNIS_COURT_63 { get { return this[IDs.FLOOR_TENNIS_COURT_63]; } }
        public TileModel FLOOR_TENNIS_COURT_66 { get { return this[IDs.FLOOR_TENNIS_COURT_66]; } }
        public TileModel FLOOR_TENNIS_COURT_67 { get { return this[IDs.FLOOR_TENNIS_COURT_67]; } }
        public TileModel FLOOR_TENNIS_COURT_68 { get { return this[IDs.FLOOR_TENNIS_COURT_68]; } }
        public TileModel FLOOR_TENNIS_COURT_69 { get { return this[IDs.FLOOR_TENNIS_COURT_69]; } }
        public TileModel FLOOR_TENNIS_COURT_70 { get { return this[IDs.FLOOR_TENNIS_COURT_70]; } }
        public TileModel FLOOR_TENNIS_COURT_71 { get { return this[IDs.FLOOR_TENNIS_COURT_71]; } }
        #endregion
        #region -Basketball court
        public TileModel FLOOR_BASKETBALL_COURT_OUTER { get { return this[IDs.FLOOR_BASKETBALL_COURT_OUTER]; } }
        public TileModel FLOOR_BASKETBALL_COURT_18 { get { return this[IDs.FLOOR_BASKETBALL_COURT_18]; } }
        public TileModel FLOOR_BASKETBALL_COURT_19 { get { return this[IDs.FLOOR_BASKETBALL_COURT_19]; } }
        public TileModel FLOOR_BASKETBALL_COURT_20 { get { return this[IDs.FLOOR_BASKETBALL_COURT_20]; } }
        public TileModel FLOOR_BASKETBALL_COURT_21 { get { return this[IDs.FLOOR_BASKETBALL_COURT_21]; } }
        public TileModel FLOOR_BASKETBALL_COURT_22 { get { return this[IDs.FLOOR_BASKETBALL_COURT_22]; } }
        public TileModel FLOOR_BASKETBALL_COURT_23 { get { return this[IDs.FLOOR_BASKETBALL_COURT_23]; } }
        public TileModel FLOOR_BASKETBALL_COURT_24 { get { return this[IDs.FLOOR_BASKETBALL_COURT_24]; } }
        public TileModel FLOOR_BASKETBALL_COURT_25 { get { return this[IDs.FLOOR_BASKETBALL_COURT_25]; } }
        public TileModel FLOOR_BASKETBALL_COURT_27 { get { return this[IDs.FLOOR_BASKETBALL_COURT_27]; } }
        public TileModel FLOOR_BASKETBALL_COURT_28 { get { return this[IDs.FLOOR_BASKETBALL_COURT_28]; } }
        public TileModel FLOOR_BASKETBALL_COURT_29 { get { return this[IDs.FLOOR_BASKETBALL_COURT_29]; } }
        public TileModel FLOOR_BASKETBALL_COURT_30 { get { return this[IDs.FLOOR_BASKETBALL_COURT_30]; } }
        public TileModel FLOOR_BASKETBALL_COURT_31 { get { return this[IDs.FLOOR_BASKETBALL_COURT_31]; } }
        public TileModel FLOOR_BASKETBALL_COURT_32 { get { return this[IDs.FLOOR_BASKETBALL_COURT_32]; } }
        public TileModel FLOOR_BASKETBALL_COURT_33 { get { return this[IDs.FLOOR_BASKETBALL_COURT_33]; } }
        public TileModel FLOOR_BASKETBALL_COURT_34 { get { return this[IDs.FLOOR_BASKETBALL_COURT_34]; } }
        public TileModel FLOOR_BASKETBALL_COURT_36 { get { return this[IDs.FLOOR_BASKETBALL_COURT_36]; } }
        public TileModel FLOOR_BASKETBALL_COURT_37 { get { return this[IDs.FLOOR_BASKETBALL_COURT_37]; } }
        public TileModel FLOOR_BASKETBALL_COURT_38 { get { return this[IDs.FLOOR_BASKETBALL_COURT_38]; } }
        public TileModel FLOOR_BASKETBALL_COURT_39 { get { return this[IDs.FLOOR_BASKETBALL_COURT_39]; } }
        public TileModel FLOOR_BASKETBALL_COURT_40 { get { return this[IDs.FLOOR_BASKETBALL_COURT_40]; } }
        public TileModel FLOOR_BASKETBALL_COURT_41 { get { return this[IDs.FLOOR_BASKETBALL_COURT_41]; } }
        public TileModel FLOOR_BASKETBALL_COURT_42 { get { return this[IDs.FLOOR_BASKETBALL_COURT_42]; } }
        public TileModel FLOOR_BASKETBALL_COURT_43 { get { return this[IDs.FLOOR_BASKETBALL_COURT_43]; } }
        public TileModel FLOOR_BASKETBALL_COURT_45 { get { return this[IDs.FLOOR_BASKETBALL_COURT_45]; } }
        public TileModel FLOOR_BASKETBALL_COURT_46 { get { return this[IDs.FLOOR_BASKETBALL_COURT_46]; } }
        public TileModel FLOOR_BASKETBALL_COURT_47 { get { return this[IDs.FLOOR_BASKETBALL_COURT_47]; } }
        public TileModel FLOOR_BASKETBALL_COURT_48 { get { return this[IDs.FLOOR_BASKETBALL_COURT_48]; } }
        public TileModel FLOOR_BASKETBALL_COURT_49 { get { return this[IDs.FLOOR_BASKETBALL_COURT_49]; } }
        public TileModel FLOOR_BASKETBALL_COURT_50 { get { return this[IDs.FLOOR_BASKETBALL_COURT_50]; } }
        public TileModel FLOOR_BASKETBALL_COURT_51 { get { return this[IDs.FLOOR_BASKETBALL_COURT_51]; } }
        public TileModel FLOOR_BASKETBALL_COURT_52 { get { return this[IDs.FLOOR_BASKETBALL_COURT_52]; } }
        public TileModel FLOOR_BASKETBALL_COURT_54 { get { return this[IDs.FLOOR_BASKETBALL_COURT_54]; } }
        public TileModel FLOOR_BASKETBALL_COURT_55 { get { return this[IDs.FLOOR_BASKETBALL_COURT_55]; } }
        public TileModel FLOOR_BASKETBALL_COURT_56 { get { return this[IDs.FLOOR_BASKETBALL_COURT_56]; } }
        public TileModel FLOOR_BASKETBALL_COURT_57 { get { return this[IDs.FLOOR_BASKETBALL_COURT_57]; } }
        public TileModel FLOOR_BASKETBALL_COURT_58 { get { return this[IDs.FLOOR_BASKETBALL_COURT_58]; } }
        public TileModel FLOOR_BASKETBALL_COURT_59 { get { return this[IDs.FLOOR_BASKETBALL_COURT_59]; } }
        public TileModel FLOOR_BASKETBALL_COURT_60 { get { return this[IDs.FLOOR_BASKETBALL_COURT_60]; } }
        public TileModel FLOOR_BASKETBALL_COURT_61 { get { return this[IDs.FLOOR_BASKETBALL_COURT_61]; } }
        public TileModel FLOOR_BASKETBALL_COURT_63 { get { return this[IDs.FLOOR_BASKETBALL_COURT_63]; } }
        public TileModel FLOOR_BASKETBALL_COURT_64 { get { return this[IDs.FLOOR_BASKETBALL_COURT_64]; } }
        public TileModel FLOOR_BASKETBALL_COURT_65 { get { return this[IDs.FLOOR_BASKETBALL_COURT_65]; } }
        public TileModel FLOOR_BASKETBALL_COURT_66 { get { return this[IDs.FLOOR_BASKETBALL_COURT_66]; } }
        public TileModel FLOOR_BASKETBALL_COURT_67 { get { return this[IDs.FLOOR_BASKETBALL_COURT_67]; } }
        public TileModel FLOOR_BASKETBALL_COURT_68 { get { return this[IDs.FLOOR_BASKETBALL_COURT_68]; } }
        public TileModel FLOOR_BASKETBALL_COURT_69 { get { return this[IDs.FLOOR_BASKETBALL_COURT_69]; } }
        public TileModel FLOOR_BASKETBALL_COURT_70 { get { return this[IDs.FLOOR_BASKETBALL_COURT_70]; } }
        #endregion

        public TileModel PARKING_ASPHALT_EW { get { return this[IDs.PARKING_ASPHALT_EW]; } }//@@MP (Release 7-3)
        public TileModel PARKING_ASPHALT_NS { get { return this[IDs.PARKING_ASPHALT_NS]; } }//@@MP (Release 7-3)
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
        public TileModel WALL_ARMY_BASE { get { return this[IDs.WALL_ARMY_BASE]; } } //@@MP (Release 6-3)
        public TileModel WALL_FUEL_STATION { get { return this[IDs.WALL_FUEL_STATION]; } } //@@MP (Release 7-3)
        public TileModel WALL_WOOD_PLANKS { get { return this[IDs.WALL_WOOD_PLANKS]; } } //@@MP (Release 7-3)
        public TileModel WALL_CONCRETE { get { return this[IDs.WALL_CONCRETE]; } } //@@MP (Release 7-3)
        public TileModel WALL_PILLAR_CONCRETE { get { return this[IDs.WALL_PILLAR_CONCRETE]; } } //@@MP (Release 7-3)
        public TileModel WALL_MALL { get { return this[IDs.WALL_MALL]; } } //@@MP (Release 7-3)
        public TileModel WALL_RED_CURTAINS { get { return this[IDs.WALL_RED_CURTAINS]; } } //@@MP (Release 7-3)
        #endregion

        #region Init
        public GameTiles()
        {
            // bind.
            Models.Tiles = this;

            this[IDs.UNDEF] = TileModel.UNDEF;

            #region Floors
            //@MP if you add another also add it to IsFloorModel()
            this[IDs.FLOOR_ARMY] = new TileModel(GameImages.TILE_FLOOR_OFFICE, Color.Khaki, true, true); //@@MP - (Release 6-3)
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
            this[IDs.FLOOR_WHITE_TILE] = new TileModel(GameImages.TILE_FLOOR_WHITE_TILE, Color.Cornsilk, true, true); //@@MP (Release 7-3)
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
            //@@MP (Release 7-3)
            this[IDs.FLOOR_FOOD_COURT_POOL] = new TileModel(GameImages.TILE_FLOOR_FOOD_COURT_POOL, Color.LightBlue, true, true) { IsWater = true, WaterCoverImageID = GameImages.TILE_FLOOR_POOL_WATER_COVER };
            this[IDs.PARKING_ASPHALT_EW] = new TileModel(GameImages.TILE_PARKING_ASPHALT_EW, Color.Gray, true, true);
            this[IDs.PARKING_ASPHALT_NS] = new TileModel(GameImages.TILE_PARKING_ASPHALT_NS, Color.Gray, true, true);
            #region -Tennis court
            //refer to Resources\Images\Tiles\tennis_court\floor_tennis_court_slices-map-for-devs.png for the layout
            this[IDs.FLOOR_TENNIS_COURT_OUTER] = new TileModel(GameImages.TILE_FLOOR_TENNIS_COURT_OUTER, Color.SeaGreen, true, true);
            this[IDs.FLOOR_TENNIS_COURT_10] = new TileModel(GameImages.TILE_FLOOR_TENNIS_COURT_10, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_TENNIS_COURT_11] = new TileModel(GameImages.TILE_FLOOR_TENNIS_COURT_11, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_TENNIS_COURT_12] = new TileModel(GameImages.TILE_FLOOR_TENNIS_COURT_12, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_TENNIS_COURT_13] = new TileModel(GameImages.TILE_FLOOR_TENNIS_COURT_13, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_TENNIS_COURT_14] = new TileModel(GameImages.TILE_FLOOR_TENNIS_COURT_14, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_TENNIS_COURT_15] = new TileModel(GameImages.TILE_FLOOR_TENNIS_COURT_15, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_TENNIS_COURT_18] = new TileModel(GameImages.TILE_FLOOR_TENNIS_COURT_18, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_TENNIS_COURT_19] = new TileModel(GameImages.TILE_FLOOR_TENNIS_COURT_19, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_TENNIS_COURT_20] = new TileModel(GameImages.TILE_FLOOR_TENNIS_COURT_20, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_TENNIS_COURT_21] = new TileModel(GameImages.TILE_FLOOR_TENNIS_COURT_21, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_TENNIS_COURT_22] = new TileModel(GameImages.TILE_FLOOR_TENNIS_COURT_22, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_TENNIS_COURT_23] = new TileModel(GameImages.TILE_FLOOR_TENNIS_COURT_23, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_TENNIS_COURT_26] = new TileModel(GameImages.TILE_FLOOR_TENNIS_COURT_26, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_TENNIS_COURT_27] = new TileModel(GameImages.TILE_FLOOR_TENNIS_COURT_27, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_TENNIS_COURT_28] = new TileModel(GameImages.TILE_FLOOR_TENNIS_COURT_28, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_TENNIS_COURT_29] = new TileModel(GameImages.TILE_FLOOR_TENNIS_COURT_29, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_TENNIS_COURT_30] = new TileModel(GameImages.TILE_FLOOR_TENNIS_COURT_30, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_TENNIS_COURT_31] = new TileModel(GameImages.TILE_FLOOR_TENNIS_COURT_31, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_TENNIS_COURT_34] = new TileModel(GameImages.TILE_FLOOR_TENNIS_COURT_34, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_TENNIS_COURT_35] = new TileModel(GameImages.TILE_FLOOR_TENNIS_COURT_35, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_TENNIS_COURT_36] = new TileModel(GameImages.TILE_FLOOR_TENNIS_COURT_36, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_TENNIS_COURT_37] = new TileModel(GameImages.TILE_FLOOR_TENNIS_COURT_37, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_TENNIS_COURT_38] = new TileModel(GameImages.TILE_FLOOR_TENNIS_COURT_38, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_TENNIS_COURT_39] = new TileModel(GameImages.TILE_FLOOR_TENNIS_COURT_39, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_TENNIS_COURT_42] = new TileModel(GameImages.TILE_FLOOR_TENNIS_COURT_42, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_TENNIS_COURT_43] = new TileModel(GameImages.TILE_FLOOR_TENNIS_COURT_43, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_TENNIS_COURT_44] = new TileModel(GameImages.TILE_FLOOR_TENNIS_COURT_44, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_TENNIS_COURT_45] = new TileModel(GameImages.TILE_FLOOR_TENNIS_COURT_45, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_TENNIS_COURT_46] = new TileModel(GameImages.TILE_FLOOR_TENNIS_COURT_46, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_TENNIS_COURT_47] = new TileModel(GameImages.TILE_FLOOR_TENNIS_COURT_47, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_TENNIS_COURT_50] = new TileModel(GameImages.TILE_FLOOR_TENNIS_COURT_50, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_TENNIS_COURT_51] = new TileModel(GameImages.TILE_FLOOR_TENNIS_COURT_51, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_TENNIS_COURT_52] = new TileModel(GameImages.TILE_FLOOR_TENNIS_COURT_52, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_TENNIS_COURT_53] = new TileModel(GameImages.TILE_FLOOR_TENNIS_COURT_53, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_TENNIS_COURT_54] = new TileModel(GameImages.TILE_FLOOR_TENNIS_COURT_54, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_TENNIS_COURT_55] = new TileModel(GameImages.TILE_FLOOR_TENNIS_COURT_55, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_TENNIS_COURT_58] = new TileModel(GameImages.TILE_FLOOR_TENNIS_COURT_58, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_TENNIS_COURT_59] = new TileModel(GameImages.TILE_FLOOR_TENNIS_COURT_59, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_TENNIS_COURT_60] = new TileModel(GameImages.TILE_FLOOR_TENNIS_COURT_60, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_TENNIS_COURT_61] = new TileModel(GameImages.TILE_FLOOR_TENNIS_COURT_61, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_TENNIS_COURT_62] = new TileModel(GameImages.TILE_FLOOR_TENNIS_COURT_62, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_TENNIS_COURT_63] = new TileModel(GameImages.TILE_FLOOR_TENNIS_COURT_63, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_TENNIS_COURT_66] = new TileModel(GameImages.TILE_FLOOR_TENNIS_COURT_66, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_TENNIS_COURT_67] = new TileModel(GameImages.TILE_FLOOR_TENNIS_COURT_67, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_TENNIS_COURT_68] = new TileModel(GameImages.TILE_FLOOR_TENNIS_COURT_68, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_TENNIS_COURT_69] = new TileModel(GameImages.TILE_FLOOR_TENNIS_COURT_69, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_TENNIS_COURT_70] = new TileModel(GameImages.TILE_FLOOR_TENNIS_COURT_70, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_TENNIS_COURT_71] = new TileModel(GameImages.TILE_FLOOR_TENNIS_COURT_71, Color.CornflowerBlue, true, true);
            #endregion
            #region -Basketball court
            //refer to Resources\Images\Tiles\basketball_court\floor_basketball_court_slices-map-for-devs.png for the layout
            this[IDs.FLOOR_BASKETBALL_COURT_OUTER] = new TileModel(GameImages.TILE_FLOOR_BASKETBALL_COURT_OUTER, Color.Gray, true, true);
            this[IDs.FLOOR_BASKETBALL_COURT_18] = new TileModel(GameImages.TILE_FLOOR_BASKETBALL_COURT_18, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_BASKETBALL_COURT_19] = new TileModel(GameImages.TILE_FLOOR_BASKETBALL_COURT_19, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_BASKETBALL_COURT_20] = new TileModel(GameImages.TILE_FLOOR_BASKETBALL_COURT_20, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_BASKETBALL_COURT_21] = new TileModel(GameImages.TILE_FLOOR_BASKETBALL_COURT_21, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_BASKETBALL_COURT_22] = new TileModel(GameImages.TILE_FLOOR_BASKETBALL_COURT_22, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_BASKETBALL_COURT_23] = new TileModel(GameImages.TILE_FLOOR_BASKETBALL_COURT_23, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_BASKETBALL_COURT_24] = new TileModel(GameImages.TILE_FLOOR_BASKETBALL_COURT_24, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_BASKETBALL_COURT_25] = new TileModel(GameImages.TILE_FLOOR_BASKETBALL_COURT_25, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_BASKETBALL_COURT_27] = new TileModel(GameImages.TILE_FLOOR_BASKETBALL_COURT_27, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_BASKETBALL_COURT_28] = new TileModel(GameImages.TILE_FLOOR_BASKETBALL_COURT_28, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_BASKETBALL_COURT_29] = new TileModel(GameImages.TILE_FLOOR_BASKETBALL_COURT_29, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_BASKETBALL_COURT_30] = new TileModel(GameImages.TILE_FLOOR_BASKETBALL_COURT_30, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_BASKETBALL_COURT_31] = new TileModel(GameImages.TILE_FLOOR_BASKETBALL_COURT_31, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_BASKETBALL_COURT_32] = new TileModel(GameImages.TILE_FLOOR_BASKETBALL_COURT_32, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_BASKETBALL_COURT_33] = new TileModel(GameImages.TILE_FLOOR_BASKETBALL_COURT_33, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_BASKETBALL_COURT_34] = new TileModel(GameImages.TILE_FLOOR_BASKETBALL_COURT_34, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_BASKETBALL_COURT_36] = new TileModel(GameImages.TILE_FLOOR_BASKETBALL_COURT_36, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_BASKETBALL_COURT_37] = new TileModel(GameImages.TILE_FLOOR_BASKETBALL_COURT_37, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_BASKETBALL_COURT_38] = new TileModel(GameImages.TILE_FLOOR_BASKETBALL_COURT_38, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_BASKETBALL_COURT_39] = new TileModel(GameImages.TILE_FLOOR_BASKETBALL_COURT_39, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_BASKETBALL_COURT_40] = new TileModel(GameImages.TILE_FLOOR_BASKETBALL_COURT_40, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_BASKETBALL_COURT_41] = new TileModel(GameImages.TILE_FLOOR_BASKETBALL_COURT_41, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_BASKETBALL_COURT_42] = new TileModel(GameImages.TILE_FLOOR_BASKETBALL_COURT_42, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_BASKETBALL_COURT_43] = new TileModel(GameImages.TILE_FLOOR_BASKETBALL_COURT_43, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_BASKETBALL_COURT_45] = new TileModel(GameImages.TILE_FLOOR_BASKETBALL_COURT_45, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_BASKETBALL_COURT_46] = new TileModel(GameImages.TILE_FLOOR_BASKETBALL_COURT_46, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_BASKETBALL_COURT_47] = new TileModel(GameImages.TILE_FLOOR_BASKETBALL_COURT_47, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_BASKETBALL_COURT_48] = new TileModel(GameImages.TILE_FLOOR_BASKETBALL_COURT_48, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_BASKETBALL_COURT_49] = new TileModel(GameImages.TILE_FLOOR_BASKETBALL_COURT_49, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_BASKETBALL_COURT_50] = new TileModel(GameImages.TILE_FLOOR_BASKETBALL_COURT_50, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_BASKETBALL_COURT_51] = new TileModel(GameImages.TILE_FLOOR_BASKETBALL_COURT_51, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_BASKETBALL_COURT_52] = new TileModel(GameImages.TILE_FLOOR_BASKETBALL_COURT_52, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_BASKETBALL_COURT_54] = new TileModel(GameImages.TILE_FLOOR_BASKETBALL_COURT_54, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_BASKETBALL_COURT_55] = new TileModel(GameImages.TILE_FLOOR_BASKETBALL_COURT_55, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_BASKETBALL_COURT_56] = new TileModel(GameImages.TILE_FLOOR_BASKETBALL_COURT_56, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_BASKETBALL_COURT_57] = new TileModel(GameImages.TILE_FLOOR_BASKETBALL_COURT_57, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_BASKETBALL_COURT_58] = new TileModel(GameImages.TILE_FLOOR_BASKETBALL_COURT_58, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_BASKETBALL_COURT_59] = new TileModel(GameImages.TILE_FLOOR_BASKETBALL_COURT_59, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_BASKETBALL_COURT_60] = new TileModel(GameImages.TILE_FLOOR_BASKETBALL_COURT_60, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_BASKETBALL_COURT_61] = new TileModel(GameImages.TILE_FLOOR_BASKETBALL_COURT_61, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_BASKETBALL_COURT_63] = new TileModel(GameImages.TILE_FLOOR_BASKETBALL_COURT_63, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_BASKETBALL_COURT_64] = new TileModel(GameImages.TILE_FLOOR_BASKETBALL_COURT_64, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_BASKETBALL_COURT_65] = new TileModel(GameImages.TILE_FLOOR_BASKETBALL_COURT_65, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_BASKETBALL_COURT_66] = new TileModel(GameImages.TILE_FLOOR_BASKETBALL_COURT_66, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_BASKETBALL_COURT_67] = new TileModel(GameImages.TILE_FLOOR_BASKETBALL_COURT_67, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_BASKETBALL_COURT_68] = new TileModel(GameImages.TILE_FLOOR_BASKETBALL_COURT_68, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_BASKETBALL_COURT_69] = new TileModel(GameImages.TILE_FLOOR_BASKETBALL_COURT_69, Color.CornflowerBlue, true, true);
            this[IDs.FLOOR_BASKETBALL_COURT_70] = new TileModel(GameImages.TILE_FLOOR_BASKETBALL_COURT_70, Color.CornflowerBlue, true, true);
            #endregion
            #endregion

            #region Walls
            //@MP if you add another also add it to IsWallModel(), IsDestructibleWallModel() and RogueGame.ReplaceDestroyedWall()
            this[IDs.WALL_BRICK] = new TileModel(GameImages.TILE_WALL_BRICK, Color.DimGray, false, false);
            this[IDs.WALL_CHAR_OFFICE] = new TileModel(GameImages.TILE_WALL_CHAR_OFFICE, DRK_RED, false, false);
            this[IDs.WALL_HOSPITAL] = new TileModel(GameImages.TILE_WALL_HOSPITAL, Color.White, false, false);
            this[IDs.WALL_POLICE_STATION] = new TileModel(GameImages.TILE_WALL_STONE, Color.CadetBlue, false, false);
            this[IDs.WALL_SEWER] = new TileModel(GameImages.TILE_WALL_SEWER, Color.Olive, false, false); //@@MP - changed from DarkGreen to Olive (Release 4)
            this[IDs.WALL_STONE] = new TileModel(GameImages.TILE_WALL_STONE, Color.DimGray, false, false);
            this[IDs.WALL_SUBWAY] = new TileModel(GameImages.TILE_WALL_STONE, Color.Blue, false, false);
            this[IDs.WALL_LIGHT_BROWN] = new TileModel(GameImages.TILE_WALL_LIGHT_BROWN, Color.BurlyWood, false, false); //@@MP (Release 4)
            this[IDs.WALL_ARMY_BASE] = new TileModel(GameImages.TILE_WALL_ARMY_BASE, Color.OliveDrab, false, false); //@@MP (Release 6-3)
            this[IDs.WALL_FUEL_STATION] = new TileModel(GameImages.TILE_WALL_FUEL_STATION, Color.MediumPurple, false, false); //@@MP (Release 7-3)
            this[IDs.WALL_WOOD_PLANKS] = new TileModel(GameImages.TILE_WALL_WOOD_PLANKS, Color.Sienna, false, false); //@@MP (Release 7-3)
            this[IDs.WALL_CONCRETE] = new TileModel(GameImages.TILE_FLOOR_CONCRETE, Color.LightGray, false, false); //@@MP (Release 7-3)
            this[IDs.WALL_PILLAR_CONCRETE] = new TileModel(GameImages.TILE_WALL_PILLAR_CONCRETE, Color.LightGray, false, false); //@@MP (Release 7-3)
            this[IDs.WALL_MALL] = new TileModel(GameImages.TILE_WALL_MALL, Color.BlanchedAlmond, false, false); //@@MP (Release 7-3)
            this[IDs.WALL_RED_CURTAINS] = new TileModel(GameImages.TILE_WALL_RED_CURTAINS, DRK_RED, false, false); //@@MP (Release 7-3)
            #endregion
        }

        #endregion

        #region Helpers
        public bool IsRoadModel(TileModel model)
        {
            return model == this[IDs.ROAD_ASPHALT_EW] || model == this[IDs.ROAD_ASPHALT_NS] || model == this[IDs.PARKING_ASPHALT_EW] || model == this[IDs.PARKING_ASPHALT_NS];
        }

        public bool IsWallModel(TileModel model) //@MP (Release 6-3)
        {
            return model == this[IDs.WALL_BRICK] || model == this[IDs.WALL_CHAR_OFFICE] || model == this[IDs.WALL_HOSPITAL] || model == this[IDs.WALL_LIGHT_BROWN] ||
                model == this[IDs.WALL_POLICE_STATION] || model == this[IDs.WALL_STONE] || model == this[IDs.WALL_SUBWAY] || model == this[IDs.WALL_ARMY_BASE] ||
                model == this[IDs.WALL_FUEL_STATION] || model == this[IDs.WALL_WOOD_PLANKS] || model == this[IDs.WALL_CONCRETE] || model == this[IDs.WALL_PILLAR_CONCRETE] ||
                model == this[IDs.WALL_RED_CURTAINS] || model == this[IDs.WALL_MALL];
        }

        public bool IsFloorModel(TileModel model) //@MP (Release 6-3)
        {
            return model == this[IDs.FLOOR_ASPHALT] || model == this[IDs.FLOOR_BLUE_CARPET] || model == this[IDs.FLOOR_CONCRETE] || model == this[IDs.FLOOR_DIRT] ||
                model == this[IDs.FLOOR_GRASS] || model == this[IDs.FLOOR_OFFICE] || model == this[IDs.FLOOR_PLANKS] || model == this[IDs.FLOOR_PLANTED] ||
                model == this[IDs.FLOOR_RED_CARPET] || model == this[IDs.FLOOR_TILES] || model == this[IDs.FLOOR_WALKWAY] || model == this[IDs.RAIL_EW] ||
                model == this[IDs.ROAD_ASPHALT_EW] || model == this[IDs.ROAD_ASPHALT_NS] || model == this[IDs.FLOOR_ARMY] || model == this[IDs.FLOOR_TENNIS_COURT_OUTER] || 
                model == this[IDs.FLOOR_WHITE_TILE] || model == this[IDs.PARKING_ASPHALT_EW] || model == this[IDs.PARKING_ASPHALT_NS] || 
            #region -tennis court
                model == this[IDs.FLOOR_TENNIS_COURT_11] || model == this[IDs.FLOOR_TENNIS_COURT_12] || model == this[IDs.FLOOR_TENNIS_COURT_13] || model == this[IDs.FLOOR_TENNIS_COURT_14] ||
                model == this[IDs.FLOOR_TENNIS_COURT_15] || model == this[IDs.FLOOR_TENNIS_COURT_18] || model == this[IDs.FLOOR_TENNIS_COURT_19] || model == this[IDs.FLOOR_TENNIS_COURT_20] ||
                model == this[IDs.FLOOR_TENNIS_COURT_21] || model == this[IDs.FLOOR_TENNIS_COURT_22] || model == this[IDs.FLOOR_TENNIS_COURT_23] || model == this[IDs.FLOOR_TENNIS_COURT_26] ||
                model == this[IDs.FLOOR_TENNIS_COURT_27] || model == this[IDs.FLOOR_TENNIS_COURT_28] || model == this[IDs.FLOOR_TENNIS_COURT_29] || model == this[IDs.FLOOR_TENNIS_COURT_30] ||
                model == this[IDs.FLOOR_TENNIS_COURT_31] || model == this[IDs.FLOOR_TENNIS_COURT_34] || model == this[IDs.FLOOR_TENNIS_COURT_35] || model == this[IDs.FLOOR_TENNIS_COURT_36] ||
                model == this[IDs.FLOOR_TENNIS_COURT_37] || model == this[IDs.FLOOR_TENNIS_COURT_38] || model == this[IDs.FLOOR_TENNIS_COURT_39] || model == this[IDs.FLOOR_TENNIS_COURT_42] ||
                model == this[IDs.FLOOR_TENNIS_COURT_43] || model == this[IDs.FLOOR_TENNIS_COURT_44] || model == this[IDs.FLOOR_TENNIS_COURT_45] || model == this[IDs.FLOOR_TENNIS_COURT_46] ||
                model == this[IDs.FLOOR_TENNIS_COURT_47] || model == this[IDs.FLOOR_TENNIS_COURT_50] || model == this[IDs.FLOOR_TENNIS_COURT_51] || model == this[IDs.FLOOR_TENNIS_COURT_52] ||
                model == this[IDs.FLOOR_TENNIS_COURT_53] || model == this[IDs.FLOOR_TENNIS_COURT_54] || model == this[IDs.FLOOR_TENNIS_COURT_55] || model == this[IDs.FLOOR_TENNIS_COURT_58] ||
                model == this[IDs.FLOOR_TENNIS_COURT_59] || model == this[IDs.FLOOR_TENNIS_COURT_60] || model == this[IDs.FLOOR_TENNIS_COURT_61] || model == this[IDs.FLOOR_TENNIS_COURT_62] ||
                model == this[IDs.FLOOR_TENNIS_COURT_63] || model == this[IDs.FLOOR_TENNIS_COURT_66] || model == this[IDs.FLOOR_TENNIS_COURT_67] || model == this[IDs.FLOOR_TENNIS_COURT_68] ||
                model == this[IDs.FLOOR_TENNIS_COURT_69] || model == this[IDs.FLOOR_TENNIS_COURT_70] || model == this[IDs.FLOOR_TENNIS_COURT_71] || model == this[IDs.FLOOR_TENNIS_COURT_10] ||
            #endregion
            #region -basketball court
                model == this[IDs.FLOOR_BASKETBALL_COURT_18] || model == this[IDs.FLOOR_BASKETBALL_COURT_19] || model == this[IDs.FLOOR_BASKETBALL_COURT_20] || model == this[IDs.FLOOR_BASKETBALL_COURT_21] ||
                model == this[IDs.FLOOR_BASKETBALL_COURT_22] || model == this[IDs.FLOOR_BASKETBALL_COURT_23] || model == this[IDs.FLOOR_BASKETBALL_COURT_24] || model == this[IDs.FLOOR_BASKETBALL_COURT_25] ||
                model == this[IDs.FLOOR_BASKETBALL_COURT_27] || model == this[IDs.FLOOR_BASKETBALL_COURT_28] || model == this[IDs.FLOOR_BASKETBALL_COURT_29] || model == this[IDs.FLOOR_BASKETBALL_COURT_30] ||
                model == this[IDs.FLOOR_BASKETBALL_COURT_31] || model == this[IDs.FLOOR_BASKETBALL_COURT_32] || model == this[IDs.FLOOR_BASKETBALL_COURT_33] || model == this[IDs.FLOOR_BASKETBALL_COURT_34] ||
                model == this[IDs.FLOOR_BASKETBALL_COURT_36] || model == this[IDs.FLOOR_BASKETBALL_COURT_37] || model == this[IDs.FLOOR_BASKETBALL_COURT_38] || model == this[IDs.FLOOR_BASKETBALL_COURT_39] ||
                model == this[IDs.FLOOR_BASKETBALL_COURT_40] || model == this[IDs.FLOOR_BASKETBALL_COURT_41] || model == this[IDs.FLOOR_BASKETBALL_COURT_42] || model == this[IDs.FLOOR_BASKETBALL_COURT_43] ||
                model == this[IDs.FLOOR_BASKETBALL_COURT_45] || model == this[IDs.FLOOR_BASKETBALL_COURT_46] || model == this[IDs.FLOOR_BASKETBALL_COURT_47] || model == this[IDs.FLOOR_BASKETBALL_COURT_48] ||
                model == this[IDs.FLOOR_BASKETBALL_COURT_49] || model == this[IDs.FLOOR_BASKETBALL_COURT_50] || model == this[IDs.FLOOR_BASKETBALL_COURT_51] || model == this[IDs.FLOOR_BASKETBALL_COURT_52] ||
                model == this[IDs.FLOOR_BASKETBALL_COURT_54] || model == this[IDs.FLOOR_BASKETBALL_COURT_55] || model == this[IDs.FLOOR_BASKETBALL_COURT_56] || model == this[IDs.FLOOR_BASKETBALL_COURT_57] || 
                model == this[IDs.FLOOR_BASKETBALL_COURT_58] || model == this[IDs.FLOOR_BASKETBALL_COURT_59] || model == this[IDs.FLOOR_BASKETBALL_COURT_60] || model == this[IDs.FLOOR_BASKETBALL_COURT_61] ||
                model == this[IDs.FLOOR_BASKETBALL_COURT_63] || model == this[IDs.FLOOR_BASKETBALL_COURT_64] || model == this[IDs.FLOOR_BASKETBALL_COURT_65] || model == this[IDs.FLOOR_BASKETBALL_COURT_66] ||
                model == this[IDs.FLOOR_BASKETBALL_COURT_67] || model == this[IDs.FLOOR_BASKETBALL_COURT_68] || model == this[IDs.FLOOR_BASKETBALL_COURT_69] || model == this[IDs.FLOOR_BASKETBALL_COURT_70]
            #endregion
                ;
        }

        public bool IsDestructibleWallModel(TileModel model) //@MP (Release 6-3)
        {
            return model == this[IDs.WALL_BRICK] || model == this[IDs.WALL_CHAR_OFFICE] || model == this[IDs.WALL_HOSPITAL] || model == this[IDs.WALL_LIGHT_BROWN] ||
                model == this[IDs.WALL_POLICE_STATION] || model == this[IDs.WALL_STONE] || model == this[IDs.WALL_SUBWAY] || model == this[IDs.WALL_ARMY_BASE] ||
                model == this[IDs.WALL_FUEL_STATION] || model == this[IDs.WALL_WOOD_PLANKS] || model == this[IDs.WALL_MALL] || model == this[IDs.WALL_RED_CURTAINS];
        }

        /// <summary>
        /// Tennis and basketball courts
        /// </summary>
        public bool IsSportsCourtTile(TileModel model) //@MP (Release 7-3)
        {
            return model == this[IDs.FLOOR_TENNIS_COURT_11] || model == this[IDs.FLOOR_TENNIS_COURT_12] || model == this[IDs.FLOOR_TENNIS_COURT_13] || model == this[IDs.FLOOR_TENNIS_COURT_14] ||
                model == this[IDs.FLOOR_TENNIS_COURT_15] || model == this[IDs.FLOOR_TENNIS_COURT_18] || model == this[IDs.FLOOR_TENNIS_COURT_19] || model == this[IDs.FLOOR_TENNIS_COURT_20] ||
                model == this[IDs.FLOOR_TENNIS_COURT_21] || model == this[IDs.FLOOR_TENNIS_COURT_22] || model == this[IDs.FLOOR_TENNIS_COURT_23] || model == this[IDs.FLOOR_TENNIS_COURT_26] ||
                model == this[IDs.FLOOR_TENNIS_COURT_27] || model == this[IDs.FLOOR_TENNIS_COURT_28] || model == this[IDs.FLOOR_TENNIS_COURT_29] || model == this[IDs.FLOOR_TENNIS_COURT_30] ||
                model == this[IDs.FLOOR_TENNIS_COURT_31] || model == this[IDs.FLOOR_TENNIS_COURT_34] || model == this[IDs.FLOOR_TENNIS_COURT_35] || model == this[IDs.FLOOR_TENNIS_COURT_36] ||
                model == this[IDs.FLOOR_TENNIS_COURT_37] || model == this[IDs.FLOOR_TENNIS_COURT_38] || model == this[IDs.FLOOR_TENNIS_COURT_39] || model == this[IDs.FLOOR_TENNIS_COURT_42] ||
                model == this[IDs.FLOOR_TENNIS_COURT_43] || model == this[IDs.FLOOR_TENNIS_COURT_44] || model == this[IDs.FLOOR_TENNIS_COURT_45] || model == this[IDs.FLOOR_TENNIS_COURT_46] ||
                model == this[IDs.FLOOR_TENNIS_COURT_47] || model == this[IDs.FLOOR_TENNIS_COURT_50] || model == this[IDs.FLOOR_TENNIS_COURT_51] || model == this[IDs.FLOOR_TENNIS_COURT_52] ||
                model == this[IDs.FLOOR_TENNIS_COURT_53] || model == this[IDs.FLOOR_TENNIS_COURT_54] || model == this[IDs.FLOOR_TENNIS_COURT_55] || model == this[IDs.FLOOR_TENNIS_COURT_58] ||
                model == this[IDs.FLOOR_TENNIS_COURT_59] || model == this[IDs.FLOOR_TENNIS_COURT_60] || model == this[IDs.FLOOR_TENNIS_COURT_61] || model == this[IDs.FLOOR_TENNIS_COURT_62] ||
                model == this[IDs.FLOOR_TENNIS_COURT_63] || model == this[IDs.FLOOR_TENNIS_COURT_66] || model == this[IDs.FLOOR_TENNIS_COURT_67] || model == this[IDs.FLOOR_TENNIS_COURT_68] ||
                model == this[IDs.FLOOR_TENNIS_COURT_69] || model == this[IDs.FLOOR_TENNIS_COURT_70] || model == this[IDs.FLOOR_TENNIS_COURT_71] || model == this[IDs.FLOOR_TENNIS_COURT_10] ||
                model == this[IDs.FLOOR_BASKETBALL_COURT_18] || model == this[IDs.FLOOR_BASKETBALL_COURT_19] || model == this[IDs.FLOOR_BASKETBALL_COURT_20] || model == this[IDs.FLOOR_BASKETBALL_COURT_21] ||
                model == this[IDs.FLOOR_BASKETBALL_COURT_22] || model == this[IDs.FLOOR_BASKETBALL_COURT_23] || model == this[IDs.FLOOR_BASKETBALL_COURT_24] || model == this[IDs.FLOOR_BASKETBALL_COURT_25] ||
                model == this[IDs.FLOOR_BASKETBALL_COURT_27] || model == this[IDs.FLOOR_BASKETBALL_COURT_28] || model == this[IDs.FLOOR_BASKETBALL_COURT_29] || model == this[IDs.FLOOR_BASKETBALL_COURT_30] ||
                model == this[IDs.FLOOR_BASKETBALL_COURT_31] || model == this[IDs.FLOOR_BASKETBALL_COURT_32] || model == this[IDs.FLOOR_BASKETBALL_COURT_33] || model == this[IDs.FLOOR_BASKETBALL_COURT_34] ||
                model == this[IDs.FLOOR_BASKETBALL_COURT_36] || model == this[IDs.FLOOR_BASKETBALL_COURT_37] || model == this[IDs.FLOOR_BASKETBALL_COURT_38] || model == this[IDs.FLOOR_BASKETBALL_COURT_39] ||
                model == this[IDs.FLOOR_BASKETBALL_COURT_40] || model == this[IDs.FLOOR_BASKETBALL_COURT_41] || model == this[IDs.FLOOR_BASKETBALL_COURT_42] || model == this[IDs.FLOOR_BASKETBALL_COURT_43] ||
                model == this[IDs.FLOOR_BASKETBALL_COURT_45] || model == this[IDs.FLOOR_BASKETBALL_COURT_46] || model == this[IDs.FLOOR_BASKETBALL_COURT_47] || model == this[IDs.FLOOR_BASKETBALL_COURT_48] ||
                model == this[IDs.FLOOR_BASKETBALL_COURT_49] || model == this[IDs.FLOOR_BASKETBALL_COURT_50] || model == this[IDs.FLOOR_BASKETBALL_COURT_51] || model == this[IDs.FLOOR_BASKETBALL_COURT_52] ||
                model == this[IDs.FLOOR_BASKETBALL_COURT_54] || model == this[IDs.FLOOR_BASKETBALL_COURT_55] || model == this[IDs.FLOOR_BASKETBALL_COURT_56] || model == this[IDs.FLOOR_BASKETBALL_COURT_57] ||
                model == this[IDs.FLOOR_BASKETBALL_COURT_58] || model == this[IDs.FLOOR_BASKETBALL_COURT_59] || model == this[IDs.FLOOR_BASKETBALL_COURT_60] || model == this[IDs.FLOOR_BASKETBALL_COURT_61] ||
                model == this[IDs.FLOOR_BASKETBALL_COURT_63] || model == this[IDs.FLOOR_BASKETBALL_COURT_64] || model == this[IDs.FLOOR_BASKETBALL_COURT_65] || model == this[IDs.FLOOR_BASKETBALL_COURT_66] ||
                model == this[IDs.FLOOR_BASKETBALL_COURT_67] || model == this[IDs.FLOOR_BASKETBALL_COURT_68] || model == this[IDs.FLOOR_BASKETBALL_COURT_69] || model == this[IDs.FLOOR_BASKETBALL_COURT_70];
        }
        #endregion
    }
}

