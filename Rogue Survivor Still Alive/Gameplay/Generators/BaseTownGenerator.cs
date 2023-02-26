﻿using System;
using System.Collections.Generic;
using System.Drawing;

using djack.RogueSurvivor.Data;
using djack.RogueSurvivor.Engine;
using djack.RogueSurvivor.Engine.Items;
using djack.RogueSurvivor.Engine.MapObjects;
using djack.RogueSurvivor.Gameplay.AI;

namespace djack.RogueSurvivor.Gameplay.Generators
{
    class BaseTownGenerator : BaseMapGenerator
    {
        #region Types
        public struct Parameters
        {
            #region Fields
            int m_MapWidth;
            int m_MapHeight;
            int m_MinBlockSize;
            int m_WreckedCarChance;
            int m_CHARBuildingChance;
            int m_ShopBuildingChance;
            int m_ParkBuildingChance;
            int m_PostersChance;
            int m_TagsChance;
            int m_ItemInShopShelfChance;
            int m_PolicemanChance;
            #endregion

            #region Properties
            /// <summary>
            /// District the map is currently generated in.
            /// </summary>
            public District District
            {
                get;
                set;
            }

            /// <summary>
            /// Do we need to generate the Police Station in this district?
            /// </summary>
            public bool GeneratePoliceStation
            {
                get;
                set;
            }

            /// <summary>
            /// Do we need to generate the Hospital in this district?
            /// </summary>
            public bool GenerateHospital
            {
                get;
                set;
            }

            /// <summary>
            /// Do we need to generate the Shopping Mall in this district?
            /// </summary>
            public bool GenerateShoppingMall //@@MP (Release 7-3)
            {
                get;
                set;
            }

            public int MapWidth
            {
                get { return m_MapWidth; }
                set
                {
                    if (value <= 0 || value > RogueGame.MAP_MAX_WIDTH) throw new InvalidOperationException("invalid MapWidth");
                    m_MapWidth = value;
                }
            }

            public int MapHeight
            {
                get { return m_MapHeight; }
                set
                {
                    if (value <= 0 || value > RogueGame.MAP_MAX_WIDTH) throw new InvalidOperationException("invalid MapHeight");
                    m_MapHeight = value;
                }
            }

            public int MinBlockSize
            {
                get { return m_MinBlockSize; }
                set
                {
                    if (value < 4 || value > 32) throw new InvalidOperationException("invalid MinBlockSize must be [4..32]");
                    m_MinBlockSize = value;
                }
            }

            public int WreckedCarChance
            {
                get { return m_WreckedCarChance; }
                set
                {
                    if (value < 0 || value > 100) throw new InvalidOperationException("WreckedCarChance must be [0..100]");
                    m_WreckedCarChance = value;
                }
            }

            public int ShopBuildingChance
            {
                get { return m_ShopBuildingChance; }
                set
                {
                    if (value < 0 || value > 100) throw new InvalidOperationException("ShopBuildingChance must be [0..100]");
                    m_ShopBuildingChance = value;
                }
            }

            public int ParkBuildingChance
            {
                get { return m_ParkBuildingChance; }
                set
                {
                    if (value < 0 || value > 100) throw new InvalidOperationException("ParkBuildingChance must be [0..100]");
                    m_ParkBuildingChance = value;
 
                }
            }

            public int CHARBuildingChance
            {
                get { return m_CHARBuildingChance; }
                set
                {
                    if (value < 0 || value > 100) throw new InvalidOperationException("CHARBuildingChance must be [0..100]");
                    m_CHARBuildingChance = value;

                }
            }

            public int PostersChance
            {
                get { return m_PostersChance; }
                set
                {
                    if (value < 0 || value > 100) throw new InvalidOperationException("PostersChance must be [0..100]");
                    m_PostersChance = value;

                }
            }

            public int TagsChance
            {
                get { return m_TagsChance; }
                set
                {
                    if (value < 0 || value > 100) throw new InvalidOperationException("TagsChance must be [0..100]");
                    m_TagsChance = value;

                }
            }

            public int ItemInShopShelfChance
            {
                get { return m_ItemInShopShelfChance; }
                set
                {
                    if (value < 0 || value > 100) throw new InvalidOperationException("ItemInShopShelfChance must be [0..100]");
                    m_ItemInShopShelfChance = value;

                }
            }

            public int PolicemanChance
            {
                get { return m_PolicemanChance; }
                set
                {
                    if (value < 0 || value > 100) throw new InvalidOperationException("PolicemanChance must be [0..100]");
                    m_PolicemanChance = value;

                }
            }
            #endregion
        }

        public class Block
        {
            /// <summary>
            /// Rectangle enclosing the whole block.
            /// </summary>
            public Rectangle Rectangle { get; set; }

            /// <summary>
            /// Rectangle enclosing the building : the blocks minus the walkway ring.
            /// </summary>
            public Rectangle BuildingRect { get; set; }

            /// <summary>
            /// Rectangle enclosing the inside of the building : the building minus the walls ring.
            /// </summary>
            public Rectangle InsideRect { get; set; }


            public Block(Rectangle rect)
            {
                ResetRectangle(rect);
            }

            public Block(Block copyFrom)
            {
                this.Rectangle = copyFrom.Rectangle;
                this.BuildingRect = copyFrom.BuildingRect;
                this.InsideRect = copyFrom.InsideRect;
            }

            public void ResetRectangle(Rectangle rect)
            {
                this.Rectangle = rect;
                this.BuildingRect = new Rectangle(rect.Left + 1, rect.Top + 1, rect.Width - 2, rect.Height - 2);
                this.InsideRect = new Rectangle(this.BuildingRect.Left + 1, this.BuildingRect.Top + 1, this.BuildingRect.Width - 2, this.BuildingRect.Height - 2);
            }
        }

        protected enum ShopType : byte
        {
            _FIRST,

            GENERAL_STORE = _FIRST,
            GROCERY,
            SPORTSWEAR,
            PHARMACY,
            CONSTRUCTION,
            GUNSHOP,
            HUNTING,
            LIQUOR, //@@MP (Release 4)

            _COUNT
        }
        
        protected enum CHARBuildingType : byte
        {
            NONE,
            AGENCY,
            OFFICE
        }

        protected enum ArmyBuildingType : byte //@@MP (Release 6-3)
        {
            NONE,
            OFFICE
        }

        protected enum HouseOutsideRoomType : byte // alpha10
        {
            _FIRST,

            GARDEN = _FIRST,
            PARKING_LOT,

            _COUNT
        }

        protected enum MallShopType : byte //@@MP (Release 7-3)
        {
            BARBER, //@@MP (Release 7-6)
            BOOKSTORE,
            LIQUOR,
            DEALERSHIP,
            CLOTHING,
            MOBILES,
            ELECTRONICS,
            SPORTING_GOODS,
            GROCERY,
            PHARMACY
        }

        /// <summary>
        /// KVPs don't allow duplicate keys. This class lets you, which is useful for iterating over lots of X,Y coords
        /// </summary>
        public class KeyValuePairWithDuplicates : List<KeyValuePair<int, int>> //@@MP (Release 7-3)
        {
            public void Add(int key, int value)
            {
                var element = new KeyValuePair<int, int>(key, value);
                this.Add(element);
            }
        }
        #endregion

        #region Constants
        public static readonly Parameters DEFAULT_PARAMS = new Parameters()
        {
            MapWidth = RogueGame.MAP_MAX_WIDTH,
            MapHeight = RogueGame.MAP_MAX_HEIGHT,
            MinBlockSize = 11, // 12 for 75x75 map size; 10 gives too many small buildings.
            WreckedCarChance = 10,
            ShopBuildingChance = 10,
            ParkBuildingChance = 10,
            CHARBuildingChance = 10,
            PostersChance = 2,
            TagsChance = 2,
            ItemInShopShelfChance = 100,
            PolicemanChance = 15
        };

        const int PARK_TREE_CHANCE = 25;
        const int PARK_BENCH_CHANCE = 10;
        const int PARK_ITEM_CHANCE = 5;
        const int PARK_GRAVE_OR_TREE_CHANCE = 33; //@@MP (Release 4)
        const int PARK_POND_CHANCE = 1000;  // //@@MP - based on alpha 10 (Release 6-1)
        const int PARK_POND_WIDTH = 5;  // //@@MP (Release 6-1)
        const int PARK_POND_HEIGHT = 5;  // //@@MP (Release 6-1)

        const int MAX_CHAR_GUARDS_PER_OFFICE = 3;

        const int SEWERS_ITEM_CHANCE = 1;
        const int SEWERS_JUNK_CHANCE = 10;
        const int SEWERS_TAG_CHANCE = 10;
        const int SEWERS_IRON_FENCE_PER_BLOCK_CHANCE = 50; // 8 fences average on std maps size 75x75.
        const int SEWERS_ROOM_CHANCE = 20;

        const int SUBWAY_TAGS_POSTERS_CHANCE = 20;

        const int HOUSE_LIVINGROOM_ITEMS_ON_TABLE = 2;
        const int HOUSE_KITCHEN_ITEMS_ON_TABLE = 2;
        const int HOUSE_KITCHEN_ITEMS_IN_FRIDGE = 3;
        const int HOUSE_BASEMENT_CHANCE = 30;
        const int HOUSE_BASEMENT_OBJECT_CHANCE_PER_TILE = 10;
        const int HOUSE_BASEMENT_PILAR_CHANCE = 20;
        const int HOUSE_BASEMENT_WEAPONS_CACHE_CHANCE = 20;
        const int HOUSE_BASEMENT_ZOMBIE_RAT_CHANCE = 5; // per tile.
        // alpha10 new house stuff
        const int HOUSE_OUTSIDE_ROOM_NEED_MIN_ROOMS = 4;
        const int HOUSE_OUTSIDE_ROOM_CHANCE = 75;
        const int HOUSE_GARDEN_TREE_CHANCE = 10;  // per tile
        const int HOUSE_PARKING_LOT_CAR_CHANCE = 10;  // per tile
        // alpha10.1 new house floorplan: apartements
        const int HOUSE_IS_APARTMENTS_CHANCE = 50;

        const int SHOP_BASEMENT_CHANCE = 30;
        const int SHOP_BASEMENT_SHELF_CHANCE_PER_TILE = 5;
        const int SHOP_BASEMENT_ITEM_CHANCE_PER_SHELF = 33;
        const int SHOP_WINDOW_CHANCE = 30;
        const int SHOP_BASEMENT_ZOMBIE_RAT_CHANCE = 5; // per tile.
        #endregion

        #region Fields
        Parameters m_Params = DEFAULT_PARAMS;
        protected DiceRoller m_DiceRoller;

        /// <summary>
        /// Blocks on surface map since during current generation.
        /// </summary>
        List<Block> m_SurfaceBlocks;
        #endregion

        #region Properties
        public Parameters Params
        {
            get { return m_Params; }
            set { m_Params = value; }
        }
        #endregion

        public BaseTownGenerator(RogueGame game, Parameters parameters)
            : base(game)
        {
            m_Params = parameters;
            m_DiceRoller = new DiceRoller();
        }

        #region Entry Map (Surface)
        public override Map Generate(int seed)
        {
            m_DiceRoller = new DiceRoller(seed);
            Map map = new Map(seed, "Base City", m_Params.MapWidth, m_Params.MapHeight);

            ///////////////////
            // Init with grass
            ///////////////////
            TileFill(map, m_Game.GameTiles.FLOOR_GRASS);

            ///////////////
            // Cut blocks
            ///////////////
            List<Block> blocks = new List<Block>();
            Rectangle cityRectangle = new Rectangle(0, 0, map.Width, map.Height);
            
            if (m_Params.GenerateShoppingMall) //@@MP - mall must have a 46x46 block (Release 7-3)
                MakeMallBlocks(map, ref blocks, cityRectangle);
            else
                MakeBlocks(map, true, ref blocks, cityRectangle);

            ///////////////////////////////////////
            // Make concrete buildings from blocks
            ///////////////////////////////////////
            List<Block> emptyBlocks = new List<Block>(blocks);
            List<Block> completedBlocks = new List<Block>(emptyBlocks.Count);

            // remember blocks.
            m_SurfaceBlocks = new List<Block>(blocks.Count);
            foreach (Block b in blocks)
                m_SurfaceBlocks.Add(new Block(b));

            // Single-block Unique buildings.
            #region
            //Shopping mall    //@@MP (Release 7-3)
            if (m_Params.GenerateShoppingMall)
            {
                Block mallBlock;
                MakeShoppingMall(map, blocks, out mallBlock);
                emptyBlocks.Remove(mallBlock);
            }
            // Police Station?
            if (m_Params.GeneratePoliceStation)
            {
                Block policeBlock;
                MakePoliceStation(map, blocks, out policeBlock);
                emptyBlocks.Remove(policeBlock);
            }
            // Hospital?
            if (m_Params.GenerateHospital)
            {
                Block hospitalBlock;
                MakeHospital(map, blocks, out hospitalBlock);
                emptyBlocks.Remove(hospitalBlock);
            }
            // Army Base //@@MP (Release 6-3)
            int armyOfficesCount = 0;
            foreach (Block b in emptyBlocks)
            {
                if (m_Params.District.Kind == DistrictKind.GREEN)// || m_Params.District.Kind == DistrictKind.GENERAL)
                {
                    if (armyOfficesCount == 0) //we only want one per district
                    {
                        ArmyBuildingType btype = MakeArmyOffice(map, b);
                        if (btype != ArmyBuildingType.NONE)
                        {
                            if (btype == ArmyBuildingType.OFFICE) //it will be because we don't have any others (yet)
                            {
                                ++armyOfficesCount;
                                PopulateArmyOfficeBuilding(map, b);
                            }
                            completedBlocks.Add(b);
                            continue;
                        }
                    }
                }
            }
            //Logger.WriteLine(Logger.Stage.RUN_MAIN, "Army offices generated: " + armyOfficesCount.ToString()); //for troubleshooting - we need at least one office to turn into the headquarters

            foreach (Block b in completedBlocks)
                emptyBlocks.Remove(b);
            #endregion

            // shops.
            completedBlocks.Clear();
            foreach (Block b in emptyBlocks)
            {
                if (m_DiceRoller.RollChance(m_Params.ShopBuildingChance) && MakeShopBuilding(map, b))
                    completedBlocks.Add(b);
            }
            foreach (Block b in completedBlocks)
                emptyBlocks.Remove(b);

            #region Business District buildings.
            completedBlocks.Clear();
            int charOfficesCount = 0;
            bool hasLibrary = false; //@@MP (Release 5-3)
            int mechanicsCount = 0; int banksCount = 0; int clinicsCount = 0; int barsCount = 0; int storesCount = 0; //@@MP - limited (Release 7-3)
            foreach (Block b in emptyBlocks)
            {
                //if (m_Params.District.Kind == DistrictKind.BUSINESS) && charOfficesCount == 0) || m_DiceRoller.RollChance(m_Params.CHARBuildingChance)) //@@MP (Release 4)
                if (m_Params.District.Kind == DistrictKind.BUSINESS || m_DiceRoller.RollChance(m_Params.CHARBuildingChance))
                {
                    bool NoCHARBuildingMade = false;
                    int rolled = m_DiceRoller.Roll(0, 99);
                    if (rolled < 33 || charOfficesCount == 0) //@@MP - reduced the number of CHAR buildings (Release 6-3)
                    {
                        CHARBuildingType btype = MakeCHARBuilding(map, b);
                        if (btype != CHARBuildingType.NONE)
                        {
                            if (btype == CHARBuildingType.OFFICE)
                            {
                                ++charOfficesCount;
                                PopulateCHAROfficeBuilding(map, b);
                            }
                            completedBlocks.Add(b);
                            continue;
                        }
                        else
                            NoCHARBuildingMade = true; //@@MP - tracks if an Agency or no building at all was created, as we then have a block to fill with something else
                    }

                    // non-CHAR buildings
                    bool placed = false; //@@MP -  (Release 7-3)
                    if (rolled >= 33 || NoCHARBuildingMade == true) //@@MP (Release 4)
                    {
                        //@@MP - the first 2 need specific sizes and limited to 1 each, the other 3 are good for whatever
                        if (!hasLibrary && MakeLibraryBuilding(map, b)) //@@MP - added a check to ensure only 1 library per district (Release 5-3)
                        {
                            hasLibrary = true; //@@MP - (Release 5-3)
                            placed = true;
                        }
                        else
                        {
                            int roll2 = m_DiceRoller.Roll(0, 4);
                            switch (roll2)
                            {
                                case 0: placed = MakeBarBuilding(map, b, ref barsCount); break;
                                case 1: placed = MakeBankBuilding(map, b, ref banksCount); break;
                                case 2: placed = MakeClinicBuilding(map, b, ref clinicsCount); break;
                                case 3: placed = MakeMechanicWorkshop(map, b, ref mechanicsCount); break;
                            }

                            //we've got enough of the standard biz types   //@@MP (Release 7-3)
                            //fill in a couple of gaps with General stores before we resort to generic offices
                            if (!placed && (storesCount < Math.Round(((double)(map.Width / 10)) / 3))) //cap the number per map based on its dimensions
                            {
                                if (MakeShopBuilding(map, b, ShopType.GENERAL_STORE))
                                {
                                    ++storesCount;
                                    placed = true;
                                }
                            }
                        }

                        if (!placed) //if all else fails, make another generic office
                            MakeOrdinaryOffice(map, b);

                        completedBlocks.Add(b);
                    }
                }
            }
            foreach (Block b in completedBlocks)
                emptyBlocks.Remove(b);
            #endregion

            #region Parks
            bool fireStationPlaced = true; //@@MP (Release 7-3)
            int fuelStationsPlaced = 0; //@@MP (Release 7-3)
            completedBlocks.Clear();
            foreach (Block b in emptyBlocks)
            {
                if (m_DiceRoller.RollChance(m_Params.ParkBuildingChance)) //@@MP (Release 4)
                {
                    bool greenSuccess = true; //@@MP (Release 7-3)
                    if (!MakeTennisCourt(map, b) && !MakeBasketballCourt(map, b)) //@@MP - these must be limited to specific dimensions (Release 7-3)
                    {
                        if (MakeFuelStation(map, b, fuelStationsPlaced)) //@@MP - must be limited to specific dimensions and quantities (Release 7-3)
                        {
                            ++fuelStationsPlaced;
                            goto Completed;
                        }

                        if (!fireStationPlaced && MakeFireStation(map, b)) //only one per district
                        {
                            fireStationPlaced = true;
                            greenSuccess = true;
                            goto Completed;
                        }

                        //now the other types that can be of looser dimensions
                        int rolled = m_DiceRoller.Roll(0, 99);
                        if (rolled >= 65)
                            greenSuccess = MakeParkBuilding(map, b, false); //@@MP - ordinary park, 35%
                        else if (rolled >= 30 && rolled < 64)
                            greenSuccess = MakeFarmBuilding(map, b); //@@MP - farm, 35% (Release 7-3)
                        else if (rolled >= 20 && rolled < 29)
                            greenSuccess = MakeAnimalShelterBuilding(map, b); //@@MP - dog pound, 10% (Release 7-3)
                        else if (rolled >= 10 && rolled < 19)
                            greenSuccess = MakeParkBuilding(map, b, true); //@@MP - graveyard, 10%
                        else
                            greenSuccess = MakeJunkyard(map, b); //10% junkyard
                    }

                    Completed:
                    if (greenSuccess)
                        completedBlocks.Add(b);
                }
            }
            foreach (Block b in completedBlocks)
                emptyBlocks.Remove(b);
            #endregion

            // all the rest is housings.
            completedBlocks.Clear();
            foreach (Block b in emptyBlocks)
            {
                bool completed = false; //@@MP - jsut in case, any tiny blocks will be made parks (Release 7-3)
                int rolled = m_DiceRoller.Roll(0, 99);
                if (rolled >= 89) //@@MP (Release 4)
                    completed = MakeChurchBuilding(map, b); //10%
                else
                    completed = MakeHousingBuilding(map, b);

                if (!completed)
                    MakeNarrowPark(map, b);

                completedBlocks.Add(b);
            }
            foreach (Block b in completedBlocks)
                emptyBlocks.Remove(b);

            ////////////
            // Decorate
            ////////////
            AddWreckedCarsOutside(map, cityRectangle);
            DecorateOutsideWallsWithPosters(map, cityRectangle, m_Params.PostersChance);
            DecorateOutsideWallsWithTags(map, cityRectangle, m_Params.TagsChance);

            ////////
            // Done
            ////////
            return map;
        }
        #endregion

        #region Sewers Map
        public virtual Map GenerateSewersMap(int seed, District district)
        {
            // Create.
            m_DiceRoller = new DiceRoller(seed);
            Map sewers = new Map(seed, "sewers", district.EntryMap.Width, district.EntryMap.Height)
            {
                Lighting = Lighting.DARKNESS
            };
            sewers.AddZone(MakeUniqueZone("sewers", sewers.Rect));
            TileFill(sewers, m_Game.GameTiles.WALL_SEWER);

            ///////////////////////////////////////////////////
            // 1. Make blocks.
            // 2. Make tunnels.
            // 3. Link with surface.
            // 4. Additional jobs.
            // 5. Sewers Maintenance Room & Building(surface).
            // 6. Some rooms.
            // 7. Objects.
            // 8. Items.
            // 9. Tags.
            // 10. Music. //alpha 10
            ///////////////////////////////////////////////////
            Map surface = district.EntryMap;

            // 1. Make blocks.
            List<Block> blocks = new List<Block>(m_SurfaceBlocks.Count);
            MakeBlocks(sewers, false, ref blocks, new Rectangle(0, 0, sewers.Width, sewers.Height));

            // 2. Make tunnels.
            #region
            // Carve tunnels.
            foreach (Block b in blocks)
            {
                TileRectangle(sewers, m_Game.GameTiles.FLOOR_SEWER_WATER, b.Rectangle);
            }
            // Iron Fences blocking some tunnels.
            foreach (Block b in blocks)
            {
                // chance?
                if (!m_DiceRoller.RollChance(SEWERS_IRON_FENCE_PER_BLOCK_CHANCE))
                    continue;

                // fences on a side.
                int fx1, fy1, fx2, fy2;
                bool goodFencePos = false;
                do
                {
                    // roll side.
                    int sideRoll = m_DiceRoller.Roll(0, 4);
                    switch (sideRoll)
                    {
                        case 0: // north.
                        case 1: // south.
                            fx1 = m_DiceRoller.Roll(b.Rectangle.Left, b.Rectangle.Right - 1);
                            fy1 = (sideRoll == 0 ? b.Rectangle.Top : b.Rectangle.Bottom - 1);

                            fx2 = fx1;
                            fy2 = (sideRoll == 0 ? fy1 - 1 : fy1 + 1);
                            break;
                        case 2: // east.
                        case 3: // west.
                            fx1 = (sideRoll == 2 ? b.Rectangle.Left : b.Rectangle.Right - 1);
                            fy1 = m_DiceRoller.Roll(b.Rectangle.Top, b.Rectangle.Bottom - 1);

                            fx2 = (sideRoll == 2 ? fx1 - 1 : fx1 + 1);
                            fy2 = fy1;
                            break;
                        default:
                            throw new InvalidOperationException("unhandled roll");
                    }

                    // never on border.
                    if (sewers.IsOnMapBorder(fx1, fy1) || sewers.IsOnMapBorder(fx2, fy2))
                        continue;

                    // must have walls.
                    if (CountAdjWalls(sewers, fx1, fy1) != 3)
                        continue;
                    if (CountAdjWalls(sewers, fx2, fy2) != 3)
                        continue;

                    // found!
                    goodFencePos = true;
                }
                while (!goodFencePos);

                // add (both of them)
                MapObjectPlace(sewers, fx1, fy1, MakeObjIronFence(GameImages.OBJ_IRON_FENCE));
                MapObjectPlace(sewers, fx2, fy2, MakeObjIronFence(GameImages.OBJ_IRON_FENCE));
            }
            #endregion

            // 3. Link with surface.
            #region
            // loop until we got at least one link.
            int countLinks = 0;
            do
            {
                for (int x = 0; x < sewers.Width; x++)
                {
                    for (int y = 0; y < sewers.Height; y++)
                    {
                        // link? roll chance. 3%
                        bool doLink = m_DiceRoller.RollChance(3);
                        if (!doLink)
                            continue;

                        // both surface and sewer tile must be walkable.
                        Tile tileSewer = sewers.GetTileAt(x, y);
                        if (!tileSewer.Model.IsWalkable)
                            continue;
                        Tile tileSurface = surface.GetTileAt(x, y);
                        if (!tileSurface.Model.IsWalkable)
                            continue;

                        // no blocking object.
                        if (sewers.GetMapObjectAt(x, y) != null)
                            continue;

                        // surface tile must be outside.
                        if (tileSurface.IsInside)
                            continue;
                        // surface tile must be walkway or grass.
                        if (tileSurface.Model != m_Game.GameTiles.FLOOR_WALKWAY && tileSurface.Model != m_Game.GameTiles.FLOOR_GRASS)
                            continue;
                        // surface tile must not be obstructed by an object.
                        if (surface.GetMapObjectAt(x, y) != null)
                            continue;

                        // must not be adjacent to another exit.
                        Point pt = new Point(x, y);
                        if (sewers.HasAnyAdjacentInMap(pt, (p) => sewers.GetExitAt(p) != null))
                            continue;
                        if (surface.HasAnyAdjacentInMap(pt, (p) => surface.GetExitAt(p) != null))
                            continue;

                        // link with ladder and sewer hole.
                        AddExit(sewers, pt, surface, pt, GameImages.DECO_SEWER_LADDER, true);
                        AddExit(surface, pt, sewers, pt, GameImages.DECO_SEWER_HOLE, true);

                        // - one more link.
                        ++countLinks;
                    }
                }
            }
            while (countLinks < 1);
            #endregion

            // 4. Additional jobs.
            #region
            // Mark all the map as inside.
            for (int x = 0; x < sewers.Width; x++)
                for (int y = 0; y < sewers.Height; y++)
                    sewers.GetTileAt(x, y).IsInside = true; 
            #endregion

            // 5. Sewers Maintenance Room & Building(surface).
            #region
            // search a suitable surface blocks.
            List<Block> goodBlocks = null;
            foreach (Block b in m_SurfaceBlocks)
            {
                // surface building must be of minimal size.
                if (b.BuildingRect.Width > m_Params.MinBlockSize + 2 || b.BuildingRect.Height > m_Params.MinBlockSize + 2)
                    continue;

                // must not be a special building or have an exit (eg: houses with basements)
                if (IsThereASpecialBuilding(surface, b.InsideRect))
                    continue;

                // we must carve a room in the sewers.
                bool hasRoom = true;
                for (int x = b.Rectangle.Left; x < b.Rectangle.Right && hasRoom; x++)
                    for (int y = b.Rectangle.Top; y < b.Rectangle.Bottom && hasRoom; y++)
                    {
                        if (sewers.GetTileAt(x, y).Model.IsWalkable)
                            hasRoom = false;
                    }
                if (!hasRoom)
                    continue;

                // found one.
                if (goodBlocks == null)
                    goodBlocks = new List<Block>(m_SurfaceBlocks.Count);
                goodBlocks.Add(b);
                break;
            }

            // if found, make maintenance room in sewers and building on surface.
            if (goodBlocks != null)
            {
                // pick one at random.
                Block surfaceBlock = goodBlocks[m_DiceRoller.Roll(0, goodBlocks.Count)];

                // clear surface building.
                ClearRectangle(surface, surfaceBlock.BuildingRect);
                TileFill(surface, m_Game.GameTiles.FLOOR_CONCRETE, surfaceBlock.BuildingRect);
                m_SurfaceBlocks.Remove(surfaceBlock);

                // make maintenance building on the surface & room in the sewers.
                Block newSurfaceBlock = new Block(surfaceBlock.Rectangle);
                Point ladderHolePos = new Point(newSurfaceBlock.BuildingRect.Left + newSurfaceBlock.BuildingRect.Width / 2, newSurfaceBlock.BuildingRect.Top + newSurfaceBlock.BuildingRect.Height / 2);
                MakeSewersMaintenanceBuilding(surface, true, newSurfaceBlock, sewers, ladderHolePos);
                Block sewersRoom = new Block(surfaceBlock.Rectangle);
                MakeSewersMaintenanceBuilding(sewers, false, sewersRoom, surface, ladderHolePos);
            }
            #endregion

            // 6. Some rooms.
            #region
            foreach (Block b in blocks)
            {
                // chance?
                if (!m_DiceRoller.RollChance(SEWERS_ROOM_CHANCE))
                    continue;

                // must be all walls = not already assigned as a room.
                if (!CheckForEachTile(b.BuildingRect, (pt) => !sewers.GetTileAt(pt).Model.IsWalkable)) //@@MP - unused parameter (Release 5-7)
                    continue;

                // carve a room.
                TileFill(sewers, m_Game.GameTiles.FLOOR_CONCRETE, b.InsideRect);

                // 4 entries.
                sewers.SetTileModelAt(b.BuildingRect.Left + b.BuildingRect.Width / 2, b.BuildingRect.Top, m_Game.GameTiles.FLOOR_CONCRETE);
                sewers.SetTileModelAt(b.BuildingRect.Left + b.BuildingRect.Width / 2, b.BuildingRect.Bottom - 1, m_Game.GameTiles.FLOOR_CONCRETE);
                sewers.SetTileModelAt(b.BuildingRect.Left, b.BuildingRect.Top + b.BuildingRect.Height / 2, m_Game.GameTiles.FLOOR_CONCRETE);
                sewers.SetTileModelAt(b.BuildingRect.Right - 1, b.BuildingRect.Top + b.BuildingRect.Height / 2, m_Game.GameTiles.FLOOR_CONCRETE);

                // zone.
                sewers.AddZone(MakeUniqueZone("room", b.InsideRect));
            }
            #endregion

            // 7. Objects.
            #region
            // junk.
            MapObjectFill(sewers, new Rectangle(0, 0, sewers.Width, sewers.Height),
                (pt) =>
                {
                    if (!m_DiceRoller.RollChance(SEWERS_JUNK_CHANCE))
                        return null;
                    if (!sewers.IsWalkable(pt.X, pt.Y))
                        return null;

                    return MakeObjJunk(GameImages.OBJ_JUNK);
                });
            #endregion

            // 8. Items.
            #region
            for (int x = 0; x < sewers.Width;x++)
                for (int y = 0; y < sewers.Height; y++)
                {
                    if (!m_DiceRoller.RollChance(SEWERS_ITEM_CHANCE))
                        continue;
                    if (!sewers.IsWalkable(x, y))
                        continue;

                    // drop item.
                    Item it;
                    int roll = m_DiceRoller.Roll(0, 3);
                    switch (roll)
                    {
                        case 0: it = MakeItemBigFlashlight(); break;
                        case 1: it = MakeItemCrowbar(); break;
                        case 2: it = MakeItemSprayPaint(); break;
                        default:
                            throw new InvalidOperationException("unhandled roll");
                    }
                    sewers.DropItemAt(it, x, y);
                }
            #endregion

            // 9. Tags.
            #region
            for (int x = 0; x < sewers.Width; x++)
                for (int y = 0; y < sewers.Height; y++)
                {
                    if (m_DiceRoller.RollChance(SEWERS_TAG_CHANCE))
                    {
                        // must be a wall with walkables around.
                        Tile t = sewers.GetTileAt(x, y);
                        if (t.Model.IsWalkable)
                            continue;
                        if (CountAdjWalkables(sewers, x, y) < 2)
                            continue;

                        // tag.
                        t.AddDecoration(TAGS[m_DiceRoller.Roll(0, TAGS.Length)]);
                    }
                }
            #endregion

            // 10. Music.  // alpha10
            sewers.BgMusic = GameMusics.SEWERS;

            // Done.
            return sewers;
        }
        #endregion

        #region Subway Map
        public virtual Map GenerateSubwayMap(int seed, District district)
        {
            // Create.
            m_DiceRoller = new DiceRoller(seed);
            Map subway = new Map(seed, "subway", district.EntryMap.Width, district.EntryMap.Height)
            {
                Lighting = Lighting.DARKNESS
            };
            TileFill(subway, m_Game.GameTiles.WALL_BRICK);

            /////////////////////////////////////
            // 1. Trace rail line.
            // 2. Make station linked to surface?
            // 3. Small tools room.
            // 4. Tags & Posters almost everywhere.
            // 5. Additional jobs.
            // 6. Music. //alpha 10
            /////////////////////////////////////
            Map surface = district.EntryMap;

            // 1. Trace rail line.
            #region
            int railStartX = 0;
            int railEndX = subway.Width - 1;
            int railY = subway.Width / 2 - 1;
            int railSize = 4;

            for (int x = railStartX; x <= railEndX; x++)
            {
                for (int y = railY; y < railY + railSize; y++)
                    subway.SetTileModelAt(x, y, m_Game.GameTiles.RAIL_EW);
            }
            subway.AddZone(MakeUniqueZone(RogueGame.NAME_SUBWAY_RAILS, new Rectangle(railStartX, railY, railEndX - railStartX + 1, railSize)));
            #endregion

            // 2. Make station linked to surface.
            #region
            // search a suitable surface blocks.
            List<Block> goodBlocks = null;
            foreach (Block b in m_SurfaceBlocks)
            {
                // surface building must be of minimal size.
                if (b.BuildingRect.Width > m_Params.MinBlockSize + 2 || b.BuildingRect.Height > m_Params.MinBlockSize + 2)
                    continue;

                // must not be a special building or have an exit (eg: houses with basements)
                if (IsThereASpecialBuilding(surface, b.InsideRect))
                    continue;

                // we must carve a room in the subway and must not be to close to rails.
                bool hasRoom = true;
                int minDistToRails = 8;
                for (int x = b.Rectangle.Left - minDistToRails; x < b.Rectangle.Right + minDistToRails && hasRoom; x++)
                    for (int y = b.Rectangle.Top - minDistToRails; y < b.Rectangle.Bottom + minDistToRails && hasRoom; y++)
                    {
                        if (!subway.IsInBounds(x, y))
                            continue;
                        if (subway.GetTileAt(x, y).Model.IsWalkable)
                            hasRoom = false;
                    }
                if (!hasRoom)
                    continue;

                // found one.
                if (goodBlocks == null)
                    goodBlocks = new List<Block>(m_SurfaceBlocks.Count);
                goodBlocks.Add(b);
                break;
            }

            // if found, make station room and building.
            if (goodBlocks != null)
            {
                // pick one at random.
                Block surfaceBlock = goodBlocks[m_DiceRoller.Roll(0, goodBlocks.Count)];

                // clear surface building.
                ClearRectangle(surface, surfaceBlock.BuildingRect);
                TileFill(surface, m_Game.GameTiles.FLOOR_CONCRETE, surfaceBlock.BuildingRect);
                m_SurfaceBlocks.Remove(surfaceBlock);

                // make station building on the surface & room in the subway.
                Block newSurfaceBlock = new Block(surfaceBlock.Rectangle);
                Point stairsPos = new Point(newSurfaceBlock.BuildingRect.Left + newSurfaceBlock.BuildingRect.Width / 2, newSurfaceBlock.InsideRect.Top);
                MakeSubwayStationBuilding(surface, true, newSurfaceBlock, subway, stairsPos);
                Block subwayRoom = new Block(surfaceBlock.Rectangle);
                MakeSubwayStationBuilding(subway, false, subwayRoom, surface, stairsPos);
            }
            #endregion

            // 3.  Small tools room.
            #region
            const int toolsRoomWidth = 5;
            const int toolsRoomHeight = 5;
            Direction toolsRoomDir = m_DiceRoller.RollChance(50) ? Direction.N : Direction.S;
            Rectangle toolsRoom = Rectangle.Empty;
            bool foundToolsRoom = false;
            int toolsRoomAttempt = 0;
            do
            {
                int x = m_DiceRoller.Roll(10, subway.Width - 10);
                int y = (toolsRoomDir == Direction.N ? railY - 1  : railY + railSize);

                if (!subway.GetTileAt(x, y).Model.IsWalkable)
                {
                    // make room rectangle.
                    if (toolsRoomDir == Direction.N)
                        toolsRoom = new Rectangle(x, y - toolsRoomHeight + 1, toolsRoomWidth, toolsRoomHeight);
                    else
                        toolsRoom = new Rectangle(x, y, toolsRoomWidth, toolsRoomHeight);
                    // check room rect is all walls (do not overlap with platform or other rooms)
                    foundToolsRoom = CheckForEachTile(toolsRoom, (pt) => !subway.GetTileAt(pt).Model.IsWalkable); //@@MP - unused parameter (Release 5-7)
                }
                ++toolsRoomAttempt;
            }
            while (toolsRoomAttempt < subway.Width * subway.Height && !foundToolsRoom);

            if (foundToolsRoom)
            {
                // room.
                TileFill(subway, m_Game.GameTiles.FLOOR_CONCRETE, toolsRoom);
                TileRectangle(subway, m_Game.GameTiles.WALL_BRICK, toolsRoom);
                PlaceDoor(subway, toolsRoom.Left + toolsRoomWidth / 2, (toolsRoomDir == Direction.N ? toolsRoom.Bottom - 1 : toolsRoom.Top), m_Game.GameTiles.FLOOR_CONCRETE, MakeObjIronDoor(DoorWindow.STATE_CLOSED));
                subway.AddZone(MakeUniqueZone("tools room", toolsRoom));

                // shelves on walls with construction items.
                DoForEachTile(toolsRoom, //@@MP - unused parameter (Release 5-7)
                    (pt) =>
                    {
                        if (!subway.IsWalkable(pt.X, pt.Y))
                            return;
                        if (CountAdjWalls(subway, pt.X, pt.Y) == 0 || CountAdjDoors(subway, pt.X, pt.Y) > 0)
                            return;

                        subway.PlaceMapObjectAt(MakeObjShelf(GameImages.OBJ_SHOP_SHELF), pt);
                        subway.DropItemAt(MakeShopConstructionItem(), pt);
                    });
            }
            #endregion

            // 4. Tags & Posters almost everywhere.
            #region
            for (int x = 0; x < subway.Width; x++)
                for (int y = 0; y < subway.Height; y++)
                {
                    if (m_DiceRoller.RollChance(SUBWAY_TAGS_POSTERS_CHANCE))
                    {
                        // must be a wall with walkables around.
                        Tile t = subway.GetTileAt(x, y);
                        if (t.Model.IsWalkable)
                            continue;
                        if (CountAdjWalkables(subway, x, y) < 2)
                            continue;

                        // poster?
                        if (m_DiceRoller.RollChance(50))
                            t.AddDecoration(POSTERS[m_DiceRoller.Roll(0, POSTERS.Length)]);

                        // tag?
                        if (m_DiceRoller.RollChance(50))
                            t.AddDecoration(TAGS[m_DiceRoller.Roll(0, TAGS.Length)]);
                    }
                }
            #endregion

            // 5. Additional jobs.
            #region
            // Mark all the map as inside.
            for (int x = 0; x < subway.Width; x++)
                for (int y = 0; y < subway.Height; y++)
                    subway.GetTileAt(x, y).IsInside = true;
            #endregion

            // 6. Music.  // alpha10
            subway.BgMusic = GameMusics.SUBWAY;

            // Done.
            return subway;
        }
        #endregion

        #region Blocks generation

        void QuadSplit(Rectangle rect, int minWidth, int minHeight, out int splitX, out int splitY, out Rectangle topLeft, out Rectangle topRight, out Rectangle bottomLeft, out Rectangle bottomRight)
        {
            // Choose a random split point.
            int leftWidthSplit = m_DiceRoller.Roll(rect.Width / 3, (2 * rect.Width) / 3);
            int topHeightSplit = m_DiceRoller.Roll(rect.Height / 3, (2 * rect.Height) / 3);

            // Ensure splitting does not produce rects below minima.
            if(leftWidthSplit < minWidth)
                leftWidthSplit = minWidth;
            if(topHeightSplit < minHeight)
                topHeightSplit = minHeight;

            int rightWidthSplit = rect.Width - leftWidthSplit;
            int bottomHeightSplit = rect.Height - topHeightSplit;

            bool doSplitX , doSplitY;
            doSplitX = doSplitY = true;

            if (rightWidthSplit < minWidth)
            {
                leftWidthSplit = rect.Width;
                rightWidthSplit = 0;
                doSplitX = false;
            }
            if (bottomHeightSplit < minHeight)
            {
                topHeightSplit = rect.Height;
                bottomHeightSplit = 0;
                doSplitY = false;
            }
            
            // Split point.
            splitX = rect.Left + leftWidthSplit;
            splitY = rect.Top + topHeightSplit;            

            // Make the quads.
            topLeft = new Rectangle(rect.Left, rect.Top, leftWidthSplit, topHeightSplit);

            if (doSplitX)
                topRight = new Rectangle(splitX, rect.Top, rightWidthSplit, topHeightSplit);
            else
                topRight = Rectangle.Empty;

            if (doSplitY)
                bottomLeft = new Rectangle(rect.Left, splitY, leftWidthSplit, bottomHeightSplit);
            else
                bottomLeft = Rectangle.Empty;

            if (doSplitX && doSplitY)
                bottomRight = new Rectangle(splitX, splitY, rightWidthSplit, bottomHeightSplit);
            else
                bottomRight = Rectangle.Empty;
        }

        void MakeBlocks(Map map, bool makeRoads, ref List<Block> list, Rectangle rect)
        {
            const int ring = 1; // dont change, keep to 1 (0=no roads, >1 = out of map)

            ////////////
            // 1. Split
            ////////////
            int splitX, splitY;
            Rectangle topLeft, topRight, bottomLeft, bottomRight;
            // +N to account for the road ring.
            QuadSplit(rect, m_Params.MinBlockSize + ring, m_Params.MinBlockSize + ring, out splitX, out splitY, out topLeft, out topRight, out bottomLeft, out bottomRight);

            ///////////////////
            // 2. Termination?
            ///////////////////
            if (topRight.IsEmpty && bottomLeft.IsEmpty && bottomRight.IsEmpty)
            {
                // Make road ring?
                if (makeRoads)
                {
                    MakeRoad(map, m_Game.GameTiles[GameTiles.IDs.ROAD_ASPHALT_EW], new Rectangle(rect.Left, rect.Top, rect.Width, ring));        // north side
                    MakeRoad(map, m_Game.GameTiles[GameTiles.IDs.ROAD_ASPHALT_EW], new Rectangle(rect.Left, rect.Bottom - 1, rect.Width, ring)); // south side
                    MakeRoad(map, m_Game.GameTiles[GameTiles.IDs.ROAD_ASPHALT_NS], new Rectangle(rect.Left, rect.Top, ring, rect.Height));       // west side
                    MakeRoad(map, m_Game.GameTiles[GameTiles.IDs.ROAD_ASPHALT_NS], new Rectangle(rect.Right - 1, rect.Top, ring, rect.Height));       // east side

                    // Adjust rect.
                    topLeft.Width -= 2 * ring;
                    topLeft.Height -= 2 * ring;
                    topLeft.Offset(ring, ring);
                }

                // Add block.
                list.Add(new Block(topLeft));
                return;
            }

            //////////////
            // 3. Recurse
            //////////////
            // always top left.
            MakeBlocks(map, makeRoads, ref list, topLeft);
            // then recurse in non empty quads.
            if (!topRight.IsEmpty)
            {
                MakeBlocks(map, makeRoads, ref list, topRight);
            }
            if (!bottomLeft.IsEmpty)
            {
                MakeBlocks(map, makeRoads, ref list, bottomLeft);
            }
            if (!bottomRight.IsEmpty)
            {
                MakeBlocks(map, makeRoads, ref list, bottomRight);
            }
        }

        private static void MallQuadSplit(Rectangle rect, int minWidth, int minHeight, out int splitX, out int splitY, out Rectangle topLeft, out Rectangle topRight, out Rectangle bottomLeft, out Rectangle bottomRight) //@@MP (Release 7-3)
        {
            // static split point.
            int leftWidthSplit = 50;
            int topHeightSplit = 50;

            // Ensure splitting does not produce rects below minima.
            if (leftWidthSplit < minWidth)
                leftWidthSplit = minWidth;
            if (topHeightSplit < minHeight)
                topHeightSplit = minHeight;
            
            int rightWidthSplit = rect.Width - leftWidthSplit;
            int bottomHeightSplit = rect.Height - topHeightSplit;

            // Split point.
            splitX = rect.Left + leftWidthSplit;
            splitY = rect.Top + topHeightSplit;

            // Make the quads.
            topLeft = new Rectangle(rect.Left, rect.Top, leftWidthSplit, topHeightSplit);
            topRight = new Rectangle(splitX, rect.Top, rightWidthSplit, topHeightSplit);
            bottomLeft = new Rectangle(rect.Left, splitY, leftWidthSplit, bottomHeightSplit);
            bottomRight = new Rectangle(splitX, splitY, rightWidthSplit, bottomHeightSplit);
        }

        void MakeMallBlocks(Map map, ref List<Block> list, Rectangle rect) //@@MP (Release 7-3)
        {
            const int ring = 1; // dont change, keep to 1 (0=no roads, >1 = out of map)

            ////////////
            // 1. Split
            ////////////
            int splitX, splitY;
            Rectangle topLeft, topRight, bottomLeft, bottomRight;
            // +N to account for the road ring.
            int minBlockSize, maxBlockSize;
            minBlockSize = maxBlockSize = (48 + ring);
            MallQuadSplit(rect, minBlockSize, maxBlockSize, out splitX, out splitY, out topLeft, out topRight, out bottomLeft, out bottomRight);


            ///////////////
            // 2. Roads
            ///////////////
            //Outer ring road
            MakeRoad(map, m_Game.GameTiles[GameTiles.IDs.ROAD_ASPHALT_EW], new Rectangle(rect.Left, rect.Top, rect.Width, ring));        // north side
            MakeRoad(map, m_Game.GameTiles[GameTiles.IDs.ROAD_ASPHALT_EW], new Rectangle(rect.Left, rect.Bottom - 1, rect.Width, ring)); // south side
            MakeRoad(map, m_Game.GameTiles[GameTiles.IDs.ROAD_ASPHALT_NS], new Rectangle(rect.Left, rect.Top, ring, rect.Height));       // west side
            MakeRoad(map, m_Game.GameTiles[GameTiles.IDs.ROAD_ASPHALT_NS], new Rectangle(rect.Right - 1, rect.Top, ring, rect.Height));  // east side
            //Complete the bordering of the mall
            if (map.Width > 50) //not for 50x50, as that would double-up
            {
                MakeRoad(map, m_Game.GameTiles[GameTiles.IDs.ROAD_ASPHALT_NS], new Rectangle(topLeft.Right - 1, rect.Top, ring, topLeft.Height));  // east side
                MakeRoad(map, m_Game.GameTiles[GameTiles.IDs.ROAD_ASPHALT_EW], new Rectangle(rect.Left, topLeft.Bottom - 1, topLeft.Width, ring)); // south side
            }

            // Adjust rect.
            topLeft.Width -= 2 * ring;
            topLeft.Height -= 2 * ring;
            topLeft.Offset(ring, ring);

            // Add mall block.
            list.Add(new Block(topLeft));

            //////////////
            // 3. Recurse in non-mall quads
            //////////////
            if (topRight.Width >=7 && topRight.Height >= 7) //nothing too small, otherwise we get deformed buildings
                MakeBlocks(map, true, ref list, topRight);
            else
                MakeNarrowPark(map, new Block(topRight));

            if (bottomLeft.Width >= 7 && bottomLeft.Height >= 7)
                MakeBlocks(map, true, ref list, bottomLeft);
            else
                MakeNarrowPark(map, new Block(bottomLeft));

            if (bottomRight.Width >= 7 && bottomRight.Height >= 7)
                MakeBlocks(map, true, ref list, bottomRight);
            else
                MakeNarrowPark(map, new Block(bottomRight));
        }

        protected virtual void MakeRoad(Map map, TileModel roadModel, Rectangle rect)
        {
            TileFill(map, roadModel, rect,
                (tile, prevmodel, x, y) =>
                {
                    // don't overwrite roads!
                    if (m_Game.GameTiles.IsRoadModel(prevmodel))
                        map.SetTileModelAt(x, y, prevmodel);
                });
            map.AddZone(MakeUniqueZone("road", rect));
        }
        #endregion

        #region Door/Window placement
        protected virtual void PlaceDoor(Map map, int x, int y, TileModel floor, DoorWindow door)
        {
            map.SetTileModelAt(x, y, floor);
            MapObjectPlace(map, x, y, door);
        }

        protected virtual void PlaceDoorIfNoObject(Map map, int x, int y, TileModel floor, DoorWindow door)
        {
            if (map.GetMapObjectAt(x, y) != null)
                return;
            PlaceDoor(map, x, y, floor, door);
        }

        protected virtual bool PlaceDoorIfAccessible(Map map, int x, int y, TileModel floor, int minAccessibility, DoorWindow door)
        {
            int countWalkable = 0;

            Point p = new Point(x, y);
            foreach (Direction d in Direction.COMPASS)
            {
                Point next = p + d;
                if (map.IsWalkable(next.X,next.Y))
                    ++countWalkable;
            }

            if (countWalkable >= minAccessibility)
            {
                PlaceDoorIfNoObject(map, x, y, floor, door);
                return true;
            }
            else
                return false;
        }

        protected virtual bool PlaceDoorIfAccessibleAndNotAdjacent(Map map, int x, int y, TileModel floor, int minAccessibility, DoorWindow door)
        {
            int countWalkable = 0;

            Point p = new Point(x, y);
            foreach (Direction d in Direction.COMPASS)
            {
                Point next = p + d;
                if (map.IsWalkable(next.X, next.Y))
                    ++countWalkable;
                if (map.GetMapObjectAt(next.X, next.Y) is DoorWindow)
                    return false;
            }

            if (countWalkable >= minAccessibility)
            {
                PlaceDoorIfNoObject(map, x, y, floor, door);
                return true;
            }
            else
                return false;
        }
        #endregion

        #region Cars
        protected virtual void AddWreckedCarsOutside(Map map, Rectangle rect)
        {
            //////////////////////////////////////
            // Add random cars (+ on fire effect)
            //////////////////////////////////////
            MapObjectFill(map, rect,
                (pt) =>
                {
                    if (m_DiceRoller.RollChance(m_Params.WreckedCarChance))
                    {
                        Tile tile = map.GetTileAt(pt.X, pt.Y);
                        if (!tile.IsInside && tile.Model.IsWalkable && tile.Model != m_Game.GameTiles.FLOOR_GRASS && !tile.Model.IsWater) //@@MP - don't put cars in ponds (Release 6-1)
                        {
                            if (m_Game.GameTiles.IsSportsCourtTile(tile.Model)) //@@MP - checks against all the various tennis and basketball court tiles (Release 7-3)
                                return null;

                            MapObject car = MakeObjWreckedCar(m_DiceRoller);
                            if (m_DiceRoller.RollChance(50))
                            {
                                m_Game.ApplyOnFire(car);
                            }
                            return car;
                        }
                    }
                    return null;
                });
        }
        #endregion

        #region Concrete buildings
        protected static bool IsThereASpecialBuilding(Map map, Rectangle rect) //@@MP - made static (Release 5-7)
        {
            // must not be a special building.
            List<Zone> zonesUpThere = map.GetZonesAt(rect.Left, rect.Top);
            if (zonesUpThere != null)
            {
                bool special = false;
                foreach (Zone z in zonesUpThere)
                    if (z.Name.Contains(RogueGame.NAME_SEWERS_MAINTENANCE) || z.Name.Contains(RogueGame.NAME_SUBWAY_STATION) || z.Name.Contains("office") || z.Name.Contains("shop") ||
                        z.Name.Contains("station") || z.Name.Contains("Farm") || z.Name.Contains("court") || z.Name.Contains("Animal") || z.Name.Contains("Mall")) //@@MP - additions (Release 7-3)
                    {
                        special = true;
                        break;
                    }
                if (special)
                    return true;
            }

            // must not have an exit.
            if (map.HasAnExitIn(rect))
                return true;

            // all clear.
            return false;
        }

        protected virtual bool MakeShopBuilding(Map map, Block b, ShopType? desiredShopType = null)
        {
            ////////////////////////
            // 0. Check suitability
            ////////////////////////
            if (b.InsideRect.Width < 5 || b.InsideRect.Height < 5)
                return false;

            /////////////////////////////
            // 1. Walkway, floor & walls
            /////////////////////////////
            TileRectangle(map, m_Game.GameTiles.FLOOR_WALKWAY, b.Rectangle);
            TileRectangle(map, m_Game.GameTiles.WALL_STONE, b.BuildingRect);
            TileFill(map, m_Game.GameTiles.FLOOR_TILES, b.InsideRect, (tile, prevmodel, x, y) => tile.IsInside = true);

            ///////////////////////
            // 2. Decide shop type
            ///////////////////////
            ShopType shopType;
            if (desiredShopType == null) //@@MP - added as a parameter so that we can generate specific shops on demand (Release 7-3)
                shopType = (ShopType)m_DiceRoller.Roll((int)ShopType._FIRST, (int)ShopType._COUNT);
            else
                shopType = (ShopType)desiredShopType;

            //////////////////////////////////////////
            // 3. Make sections alleys with displays.
            //////////////////////////////////////////            
            #region
            int alleysStartX = b.InsideRect.Left;
            int alleysStartY = b.InsideRect.Top;
            int alleysEndX = b.InsideRect.Right;
            int alleysEndY = b.InsideRect.Bottom;
            bool horizontalAlleys = b.Rectangle.Width >= b.Rectangle.Height;
            int centralAlley;

            if (horizontalAlleys)
            {
                ++alleysStartX;
                --alleysEndX;
                centralAlley = b.InsideRect.Left + b.InsideRect.Width / 2;
            }
            else
            {
                ++alleysStartY;
                --alleysEndY;
                centralAlley = b.InsideRect.Top + b.InsideRect.Height / 2;
            }
            Rectangle alleysRect = Rectangle.FromLTRB(alleysStartX, alleysStartY, alleysEndX, alleysEndY);

            MapObjectFill(map, alleysRect,
                (pt) =>
                {
                    bool addShelf;

                    if (horizontalAlleys)
                        addShelf = ((pt.Y - alleysRect.Top) % 2 == 1) && pt.X != centralAlley;
                    else
                        addShelf = ((pt.X - alleysRect.Left) % 2 == 1) && pt.Y != centralAlley;

                    if (addShelf)
                        return MakeObjShelf(GameImages.OBJ_SHOP_SHELF);
                    else
                        return null;
                });
            #endregion

            ///////////////////////////////
            // 4. Entry door with shop ids
            //    Might add window(s).
            ///////////////////////////////
            #region
            int midX = b.Rectangle.Left + b.Rectangle.Width / 2;
            int midY = b.Rectangle.Top + b.Rectangle.Height / 2;
            string strDoorSide; //@@MP - hold the side of the door for use when creating a cash register at step 8 (Release 3)

            // make doors on one side.
            if (horizontalAlleys)
            {
                bool west = m_DiceRoller.RollChance(50);

                if (west)
                {
                    // west
                    strDoorSide = "west";
                    PlaceDoor(map, b.BuildingRect.Left, midY, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                    if (b.InsideRect.Height >= 8) //@@MP adds up to two more doors depending on how big the shop is
                    {
                        PlaceDoor(map, b.BuildingRect.Left, midY - 1, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                        if (b.InsideRect.Height >= 12)
                            PlaceDoor(map, b.BuildingRect.Left, midY + 1, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                    }
                    //@@MP - cash register by the door. abandoned it for in a nearby corner; see section 8 (Release 3)
                    /*int intOffset = 1;
                    if (CountAdjDoors(map, b.BuildingRect.Left + 1, midY - 1) > 1)
                        intOffset = 2;
                    Point pt = new Point(b.BuildingRect.Left + 1, midY - intOffset);
                    map.PlaceMapObjectAt(MakeObjTelevision(GameImages.OBJ_TELEVISION), pt);*/

                }
                else
                {
                    // east
                    strDoorSide = "east";
                    PlaceDoor(map, b.BuildingRect.Right - 1, midY, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                    if (b.InsideRect.Height >= 8)
                    {
                        PlaceDoor(map, b.BuildingRect.Right - 1, midY - 1, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                        if (b.InsideRect.Height >= 12)
                            PlaceDoor(map, b.BuildingRect.Right - 1, midY + 1, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                    }
                }
            }
            else
            {
                bool north = m_DiceRoller.RollChance(50);

                if (north)
                {
                    // north
                    strDoorSide = "north";
                    PlaceDoor(map, midX, b.BuildingRect.Top, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                    if (b.InsideRect.Width >= 8)
                    {
                        PlaceDoor(map, midX - 1, b.BuildingRect.Top, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                        if (b.InsideRect.Width >= 12)
                            PlaceDoor(map, midX + 1, b.BuildingRect.Top, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                    }
                }
                else
                {
                    // south
                    strDoorSide = "south";
                    PlaceDoor(map, midX, b.BuildingRect.Bottom - 1, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                    if (b.InsideRect.Width >= 8)
                    {
                        PlaceDoor(map, midX - 1, b.BuildingRect.Bottom - 1, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                        if (b.InsideRect.Width >= 12)
                            PlaceDoor(map, midX + 1, b.BuildingRect.Bottom - 1, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                    }
                }
            }

            // add shop image next to doors.
            string shopImage;
            string shopName;
            switch (shopType)
            {
                case ShopType.CONSTRUCTION:
                    shopImage = GameImages.DECO_SHOP_CONSTRUCTION;
                    shopName = "Construction";
                    break;
                case ShopType.GENERAL_STORE:
                    shopImage = GameImages.DECO_SHOP_GENERAL_STORE;
                    shopName = "GeneralStore";
                    break;
                case ShopType.GROCERY:
                    shopImage = GameImages.DECO_SHOP_GROCERY;
                    shopName = "Grocery";
                    break;
                case ShopType.GUNSHOP:
                    shopImage = GameImages.DECO_SHOP_GUNSHOP;
                    shopName = "Gunshop";
                    break;
                case ShopType.PHARMACY:
                    shopImage = GameImages.DECO_SHOP_PHARMACY;
                    shopName = "Pharmacy";
                    break;
                case ShopType.SPORTSWEAR:
                    shopImage = GameImages.DECO_SHOP_SPORTSWEAR;
                    shopName = "Sportswear";
                    break;
                case ShopType.HUNTING:
                    shopImage = GameImages.DECO_SHOP_HUNTING;
                    shopName = "Hunting Shop";
                    break;
                case ShopType.LIQUOR: //@@MP (Release 4)
                    shopImage = GameImages.DECO_SHOP_LIQUOR;
                    shopName = "Liquor Store";
                    break;
                default:
                    throw new InvalidOperationException("unhandled shoptype");
            }
            DecorateOutsideWalls(map, b.BuildingRect, (x, y) => map.GetMapObjectAt(x, y) == null && CountAdjDoors(map, x, y) >= 1 ? shopImage : null);

            // window?
            if (m_DiceRoller.RollChance(SHOP_WINDOW_CHANCE))
            {
                // pick a random side.
                int side = m_DiceRoller.Roll(0, 4);
                int wx, wy;
                switch (side)
                {
                    case 0: wx = b.BuildingRect.Left + b.BuildingRect.Width / 2; wy = b.BuildingRect.Top; break;
                    case 1: wx = b.BuildingRect.Left + b.BuildingRect.Width / 2; wy = b.BuildingRect.Bottom - 1; break;
                    case 2: wx = b.BuildingRect.Left; wy = b.BuildingRect.Top + b.BuildingRect.Height / 2; break;
                    case 3: wx = b.BuildingRect.Right - 1; wy = b.BuildingRect.Top + b.BuildingRect.Height / 2; break;
                    default: throw new InvalidOperationException("unhandled side");
                }
                // check it is ok to make a window there.
                bool isGoodWindowPos = true;
                if (map.GetTileAt(wx, wy).Model.IsWalkable) isGoodWindowPos = false;
                // do it?
                if (isGoodWindowPos)
                {
                    PlaceDoor(map, wx, wy, m_Game.GameTiles.FLOOR_TILES, MakeObjWindow());
                }
            }

            // barricade certain shops types.
            if (shopType == ShopType.GUNSHOP)
            {
                BarricadeDoors(map, b.BuildingRect, Rules.BARRICADING_MAX);
            }

            #endregion

            ///////////////////////////
            // 5. Add items to shelves.
            ///////////////////////////
            #region
            ItemsDrop(map, b.InsideRect,
                (pt) =>
                {
                    MapObject mapObj = map.GetMapObjectAt(pt);
                    if (mapObj == null)
                        return false;
                    return mapObj.ImageID == GameImages.OBJ_SHOP_SHELF &&
                        m_DiceRoller.RollChance(m_Params.ItemInShopShelfChance);
                },
                (pt) => MakeRandomShopItem(shopType));
            #endregion

            ///////////
            // 6. Zone
            ///////////
            // shop building.
            map.AddZone(MakeUniqueZone(shopName, b.BuildingRect));
            // walkway zones.
            MakeWalkwayZones(map, b);

            ////////////////
            // 7. Basement?
            ////////////////
            #region
            if (m_DiceRoller.RollChance(SHOP_BASEMENT_CHANCE))
            {
                // shop basement map:                
                // - a single dark room.
                // - some shop items.

                // - a single dark room.
                Map shopBasement = new Map((map.Seed << 1) ^ shopName.GetHashCode(), "basement-" + shopName, b.BuildingRect.Width, b.BuildingRect.Height)
                {
                    Lighting = Lighting.DARKNESS
                };
                DoForEachTile(shopBasement.Rect, (pt) => shopBasement.GetTileAt(pt).IsInside = true); //@@MP - unused parameter (Release 5-7)
                TileFill(shopBasement, m_Game.GameTiles.FLOOR_CONCRETE);
                TileRectangle(shopBasement, m_Game.GameTiles.WALL_BRICK, shopBasement.Rect);
                shopBasement.AddZone(MakeUniqueZone("basement", shopBasement.Rect));

                // - some shelves with shop items.
                // - some rats.
                DoForEachTile(shopBasement.Rect, //@@MP - unused parameter (Release 5-7)
                    (pt) =>
                    {
                        if (!shopBasement.IsWalkable(pt.X, pt.Y))
                            return;
                        if (shopBasement.GetExitAt(pt) != null)
                            return;

                        if (m_DiceRoller.RollChance(SHOP_BASEMENT_SHELF_CHANCE_PER_TILE))
                        {
                            shopBasement.PlaceMapObjectAt(MakeObjShelf(GameImages.OBJ_SHOP_SHELF), pt);
                            if (m_DiceRoller.RollChance(SHOP_BASEMENT_ITEM_CHANCE_PER_SHELF))
                            {
                                Item it = MakeRandomShopItem(shopType);
                                if (it != null)
                                    shopBasement.DropItemAt(it, pt);
                            }
                        }

                        if (Rules.HasZombiesInBasements(m_Game.Session.GameMode))
                        {
                            if (m_DiceRoller.RollChance(SHOP_BASEMENT_ZOMBIE_RAT_CHANCE))
                                shopBasement.PlaceActorAt(CreateNewBasementRatZombie(0), pt);
                        }
                    });

                // alpha10 music
                shopBasement.BgMusic = null; //@@MP - was Sewers (Release 7-4)

                // link maps, stairs in one corner.
                Point basementCorner = new Point();
                basementCorner.X = m_DiceRoller.RollChance(50) ? 1 : shopBasement.Width - 2;
                basementCorner.Y = m_DiceRoller.RollChance(50) ? 1 : shopBasement.Height - 2;
                Point shopCorner = new Point(basementCorner.X - 1 + b.InsideRect.Left, basementCorner.Y - 1 + b.InsideRect.Top);
                AddExit(shopBasement, basementCorner, map, shopCorner, GameImages.DECO_STAIRS_UP, true);
                AddExit(map, shopCorner, shopBasement, basementCorner, GameImages.DECO_STAIRS_DOWN, true);

                // remove any blocking object in the shop.
                MapObject blocker = map.GetMapObjectAt(shopCorner);
                if (blocker != null)
                    map.RemoveMapObjectAt(shopCorner.X, shopCorner.Y);

                // add map.
                m_Params.District.AddUniqueMap(shopBasement);

            }
            #endregion

            ///////////////////
            // 8. Cash register
            // @@MP - add a cash register in an corner near the door (Release 3)
            ///////////////////
            #region

            Point cornerpt = new Point();
            int cornersFreeCode = 0;
            bool skipThisRegister = false;
            switch (strDoorSide)
            {
                case "west":
                    cornerpt.X = b.InsideRect.Left;
                    cornersFreeCode = TopOrBottomCornersFree(map, cornerpt.X, b.InsideRect.Top, b.InsideRect.Bottom - 1); //the door is on the west wall, so we must check the corners directly north and south of it for a place to put the cash register
                    switch (cornersFreeCode)
                    {
                        case 1:
                            cornerpt.Y = b.InsideRect.Top;
                            break;
                        case 2:
                            cornerpt.Y = b.InsideRect.Bottom - 1;
                            break;
                        case 3: //both free, pick one at random
                            cornerpt.Y = m_DiceRoller.RollChance(50) ? b.InsideRect.Top : b.InsideRect.Bottom - 1;
                            break;
                        default:
                            skipThisRegister = true;  //don't create a cash register because the corners are not free
                            break;
                    }                    
                    break;
                case "east":
                    cornerpt.X = b.InsideRect.Right - 1;
                    cornersFreeCode = TopOrBottomCornersFree(map, cornerpt.X, b.InsideRect.Top, b.InsideRect.Bottom - 1); //the door is on the east wall, so we must check the corners directly north and south of it for a place to put the cash register
                    switch (cornersFreeCode)
                    {
                        case 1:
                            cornerpt.Y = b.InsideRect.Top;
                            break;
                        case 2:
                            cornerpt.Y = b.InsideRect.Bottom - 1;
                            break;
                        case 3: //both free, pick one at random
                            cornerpt.Y = m_DiceRoller.RollChance(50) ? b.InsideRect.Top : b.InsideRect.Bottom - 1;
                            break;
                        default:
                            skipThisRegister = true;  //don't create a cash register because the corners are not free
                            break;
                    }
                    break;
                case "north":
                    cornerpt.Y = b.InsideRect.Top;
                    cornersFreeCode = LeftOrRightCornersFree(map, cornerpt.Y, b.InsideRect.Left, b.InsideRect.Right - 1); //the door is on the north wall, so we must check the corners directly east and west of it for a place to put the cash register
                    switch (cornersFreeCode)
                    {
                        case 1:
                            cornerpt.X = b.InsideRect.Left;
                            break;
                        case 2:
                            cornerpt.X = b.InsideRect.Right - 1;
                            break;
                        case 3: //both free, pick one at random
                            cornerpt.X = m_DiceRoller.RollChance(50) ? b.InsideRect.Left : b.InsideRect.Right - 1;
                            break;
                        default:
                            skipThisRegister = true;  //don't create a cash register because the corners are not free
                            break;
                    }
                    break;
                case "south":
                    cornerpt.Y = b.InsideRect.Bottom - 1;
                    cornersFreeCode = LeftOrRightCornersFree(map, cornerpt.Y, b.InsideRect.Left, b.InsideRect.Right - 1); //the door is on the south wall, so we must check the corners directly east and west of it for a place to put the cash register
                    switch (cornersFreeCode)
                    {
                        case 1:
                            cornerpt.X = b.InsideRect.Left;
                            break;
                        case 2:
                            cornerpt.X = b.InsideRect.Right - 1;
                            break;
                        case 3: //both free, pick one at random
                            cornerpt.X = m_DiceRoller.RollChance(50) ? b.InsideRect.Left : b.InsideRect.Right - 1;
                            break;
                        default:
                            skipThisRegister = true;  //don't create a cash register because the corners are not free
                            break;
                    }
                    break;
            }
            if (!skipThisRegister) //there wasn't a free spot for it
            {
                MapObject blocker = map.GetMapObjectAt(cornerpt);
                if (blocker != null)
                    map.RemoveMapObjectAt(cornerpt.X, cornerpt.Y); // remove any blocking object in the shop. it should only ever be a shelf if anything, as we checked for exits already
                if (map.IsWalkable(cornerpt.X, cornerpt.Y)) // make sure we haven't somehow got a wall or other inaccessible spot
                {
                    map.PlaceMapObjectAt(MakeObjCheckout(GameImages.OBJ_CASH_REGISTER), cornerpt);
                    if (shopType == ShopType.CONSTRUCTION) //@@MP - add dynamite if it's a Construction store (Release 4)
                    {
                        //@@MP - added roll on Resources Availability difficulty option (Release 7-4)
                        if (RogueGame.Options.ResourcesAvailability != GameOptions.Resources.LOW) //no dynamite when using Low resource availability
                        {
                            int chanceToSpawn = GameOptions.ResourcesAvailabilityToInt(RogueGame.Options.ResourcesAvailability);
                            chanceToSpawn = (int)chanceToSpawn / 5;  //@@MP - heavily reduced the chance for it to spawn (Release 7-6)
                            //rollCHance += RogueGame.Options.HiddenRescueDay;   //this made it too easy, but was a cool idea
                            if (m_DiceRoller.RollChance(chanceToSpawn))
                            {
                                map.DropItemAt(MakeItemDynamite(), cornerpt);
                                //Logger.WriteLine(Logger.Stage.RUN_MAIN, "dynamite spawned +1"); //DELETETHIS
                            }
                        }
                    }
                        
                }
            }
            #endregion

            // Done
            return true;
        }

        /// <summary>
        /// Given two different Y coordinates along the same vertical, which has no exit on it?
        /// </summary>
        /// <returns>1=y1 only is free, 2=y2 only is free, 3=both are free</returns>
        private static int TopOrBottomCornersFree(Map map, int x, int y1, int y2) //@@MP (Release 3)
        {
            Point cornerpt = new Point();
            int cornersFreeCode = 0;
            cornerpt.X = x;

            cornerpt.Y = y1;
            if (map.GetExitAt(cornerpt) == null)
                cornersFreeCode += 1;

            cornerpt.Y = y2;
            if (map.GetExitAt(cornerpt) == null)
                cornersFreeCode += 2;

            return cornersFreeCode;
        }

        /// <summary>
        /// Given two different X coordinates along the same horizontal, which has no exit on it?
        /// </summary>
        /// <returns>1= x1 only is free, 2= x2 only is free, 3= both are free</returns>
        private static int LeftOrRightCornersFree(Map map, int y, int x1, int x2) //@@MP (Release 3), made static (Release 5-7)
        {
            Point cornerpt = new Point();
            int cornersFreeCode = 0;
            cornerpt.Y = y;

            cornerpt.X = x1;
            if (map.GetExitAt(cornerpt) == null)
                cornersFreeCode += 1;

            cornerpt.X = x2;
            if (map.GetExitAt(cornerpt) == null)
                cornersFreeCode += 2;

            return cornersFreeCode;
        }

        protected virtual bool MakeLibraryBuilding(Map map, Block b) //@@MP (Release 4)
        {
            ////////////////////////
            // 0. Check suitability
            ////////////////////////
            if (b.InsideRect.Width < 10 || b.InsideRect.Height < 10)
                return false;

            /////////////////////////////
            // 1. Walkway, floor & walls
            /////////////////////////////
            TileRectangle(map, m_Game.GameTiles.FLOOR_WALKWAY, b.Rectangle);
            TileRectangle(map, m_Game.GameTiles.WALL_LIGHT_BROWN, b.BuildingRect);
            TileFill(map, m_Game.GameTiles.FLOOR_BLUE_CARPET, b.InsideRect, (tile, prevmodel, x, y) => tile.IsInside = true);

            //////////////////////////////////////////
            // 2. Make sections alleys with displays.
            //////////////////////////////////////////            
            #region
            int alleysStartX = b.InsideRect.Left;
            int alleysStartY = b.InsideRect.Top;
            int alleysEndX = b.InsideRect.Right;
            int alleysEndY = b.InsideRect.Bottom;
            bool horizontalAlleys = b.Rectangle.Width >= b.Rectangle.Height;
            int centralAlley;

            if (horizontalAlleys)
            {
                ++alleysStartX;
                --alleysEndX;
                centralAlley = b.InsideRect.Left + b.InsideRect.Width / 2;
            }
            else
            {
                ++alleysStartY;
                --alleysEndY;
                centralAlley = b.InsideRect.Top + b.InsideRect.Height / 2;
            }
            Rectangle alleysRect = Rectangle.FromLTRB(alleysStartX, alleysStartY, alleysEndX, alleysEndY);

            MapObjectFill(map, alleysRect,
                (pt) =>
                {
                    bool addShelf;

                    if (horizontalAlleys)
                        addShelf = ((pt.Y - alleysRect.Top) % 2 == 1) && pt.X != centralAlley;
                    else
                        addShelf = ((pt.X - alleysRect.Left) % 2 == 1) && pt.Y != centralAlley;

                    if (addShelf)
                        return MakeObjShelf(GameImages.OBJ_BOOK_SHELVES);
                    else
                        return null;
                });
            #endregion

            ///////////////////////////////
            // 3. Entry door with shop ids
            //    Might add window(s).
            ///////////////////////////////
            #region
            int midX = b.Rectangle.Left + b.Rectangle.Width / 2;
            int midY = b.Rectangle.Top + b.Rectangle.Height / 2;
            string strDoorSide; //@@MP - hold the side of the door for use when creating a counter at step 6

            // make doors on one side.
            if (horizontalAlleys)
            {
                bool west = m_DiceRoller.RollChance(50);

                if (west)
                {
                    // west
                    strDoorSide = "west";
                    PlaceDoor(map, b.BuildingRect.Left, midY, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                    if (b.InsideRect.Height >= 8) //@@MP adds up to two more doors depending on how big the shop is
                    {
                        PlaceDoor(map, b.BuildingRect.Left, midY - 1, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                        if (b.InsideRect.Height >= 12)
                            PlaceDoor(map, b.BuildingRect.Left, midY + 1, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                    }
                }
                else
                {
                    // east
                    strDoorSide = "east";
                    PlaceDoor(map, b.BuildingRect.Right - 1, midY, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                    if (b.InsideRect.Height >= 8)
                    {
                        PlaceDoor(map, b.BuildingRect.Right - 1, midY - 1, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                        if (b.InsideRect.Height >= 12)
                            PlaceDoor(map, b.BuildingRect.Right - 1, midY + 1, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                    }
                }
            }
            else
            {
                bool north = m_DiceRoller.RollChance(50);

                if (north)
                {
                    // north
                    strDoorSide = "north";
                    PlaceDoor(map, midX, b.BuildingRect.Top, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                    if (b.InsideRect.Width >= 8)
                    {
                        PlaceDoor(map, midX - 1, b.BuildingRect.Top, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                        if (b.InsideRect.Width >= 12)
                            PlaceDoor(map, midX + 1, b.BuildingRect.Top, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                    }
                }
                else
                {
                    // south
                    strDoorSide = "south";
                    PlaceDoor(map, midX, b.BuildingRect.Bottom - 1, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                    if (b.InsideRect.Width >= 8)
                    {
                        PlaceDoor(map, midX - 1, b.BuildingRect.Bottom - 1, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                        if (b.InsideRect.Width >= 12)
                            PlaceDoor(map, midX + 1, b.BuildingRect.Bottom - 1, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                    }
                }
            }

            // add shop image next to doors.
            string shopName = "Library";
            DecorateOutsideWalls(map, b.BuildingRect, (x, y) => map.GetMapObjectAt(x, y) == null && CountAdjDoors(map, x, y) >= 1 ? GameImages.DECO_LIBRARY : null);

            // window
            // pick a random side.
            int side = m_DiceRoller.Roll(0, 4);
            int wx, wy;
            switch (side)
            {
                case 0: wx = b.BuildingRect.Left + b.BuildingRect.Width / 2; wy = b.BuildingRect.Top; break;
                case 1: wx = b.BuildingRect.Left + b.BuildingRect.Width / 2; wy = b.BuildingRect.Bottom - 1; break;
                case 2: wx = b.BuildingRect.Left; wy = b.BuildingRect.Top + b.BuildingRect.Height / 2; break;
                case 3: wx = b.BuildingRect.Right - 1; wy = b.BuildingRect.Top + b.BuildingRect.Height / 2; break;
                default: throw new InvalidOperationException("unhandled side");
            }
            // check it is ok to make a window there.
            bool isGoodWindowPos = true;
            if (map.GetTileAt(wx, wy).Model.IsWalkable) isGoodWindowPos = false;
            // do it?
            if (isGoodWindowPos)
            {
                PlaceDoor(map, wx, wy, m_Game.GameTiles.FLOOR_BLUE_CARPET, MakeObjGlassDoor());
            }
            #endregion

            ///////////////////////////
            // 4. Add items to shelves.
            ///////////////////////////
            #region
            if (RogueGame.Options.IsSanityEnabled) //@@MP - added check (Release 7-6)
            {
                ItemsDrop(map, b.InsideRect,
                (pt) =>
                {
                    MapObject mapObj = map.GetMapObjectAt(pt);
                    if (mapObj == null)
                        return false;
                    return mapObj.ImageID == GameImages.OBJ_BOOK_SHELVES;
                },
                (pt) => MakeItemBook(m_DiceRoller));
            }
            #endregion

            ///////////////////
            // 5. Counter in an corner near the door
            ///////////////////
            #region
            Point cornerpt = new Point();
            int cornersFreeCode = 0;
            bool skipThisRegister = false;
            switch (strDoorSide)
            {
                case "west":
                    cornerpt.X = b.InsideRect.Left;
                    cornersFreeCode = TopOrBottomCornersFree(map, cornerpt.X, b.InsideRect.Top, b.InsideRect.Bottom - 1); //the door is on the west wall, so we must check the corners directly north and south of it for a place to put the cash register
                    switch (cornersFreeCode)
                    {
                        case 1:
                            cornerpt.Y = b.InsideRect.Top;
                            break;
                        case 2:
                            cornerpt.Y = b.InsideRect.Bottom - 1;
                            break;
                        case 3: //both free, pick one at random
                            cornerpt.Y = m_DiceRoller.RollChance(50) ? b.InsideRect.Top : b.InsideRect.Bottom - 1;
                            break;
                        default:
                            skipThisRegister = true;  //don't create a cash register because the corners are not free
                            break;
                    }
                    break;
                case "east":
                    cornerpt.X = b.InsideRect.Right - 1;
                    cornersFreeCode = TopOrBottomCornersFree(map, cornerpt.X, b.InsideRect.Top, b.InsideRect.Bottom - 1); //the door is on the east wall, so we must check the corners directly north and south of it for a place to put the cash register
                    switch (cornersFreeCode)
                    {
                        case 1:
                            cornerpt.Y = b.InsideRect.Top;
                            break;
                        case 2:
                            cornerpt.Y = b.InsideRect.Bottom - 1;
                            break;
                        case 3: //both free, pick one at random
                            cornerpt.Y = m_DiceRoller.RollChance(50) ? b.InsideRect.Top : b.InsideRect.Bottom - 1;
                            break;
                        default:
                            skipThisRegister = true;  //don't create a cash register because the corners are not free
                            break;
                    }
                    break;
                case "north":
                    cornerpt.Y = b.InsideRect.Top;
                    cornersFreeCode = LeftOrRightCornersFree(map, cornerpt.Y, b.InsideRect.Left, b.InsideRect.Right - 1); //the door is on the north wall, so we must check the corners directly east and west of it for a place to put the cash register
                    switch (cornersFreeCode)
                    {
                        case 1:
                            cornerpt.X = b.InsideRect.Left;
                            break;
                        case 2:
                            cornerpt.X = b.InsideRect.Right - 1;
                            break;
                        case 3: //both free, pick one at random
                            cornerpt.X = m_DiceRoller.RollChance(50) ? b.InsideRect.Left : b.InsideRect.Right - 1;
                            break;
                        default:
                            skipThisRegister = true;  //don't create a cash register because the corners are not free
                            break;
                    }
                    break;
                case "south":
                    cornerpt.Y = b.InsideRect.Bottom - 1;
                    cornersFreeCode = LeftOrRightCornersFree(map, cornerpt.Y, b.InsideRect.Left, b.InsideRect.Right - 1); //the door is on the south wall, so we must check the corners directly east and west of it for a place to put the cash register
                    switch (cornersFreeCode)
                    {
                        case 1:
                            cornerpt.X = b.InsideRect.Left;
                            break;
                        case 2:
                            cornerpt.X = b.InsideRect.Right - 1;
                            break;
                        case 3: //both free, pick one at random
                            cornerpt.X = m_DiceRoller.RollChance(50) ? b.InsideRect.Left : b.InsideRect.Right - 1;
                            break;
                        default:
                            skipThisRegister = true;  //don't create a cash register because the corners are not free
                            break;
                    }
                    break;
            }
            if (!skipThisRegister) //there wasn't a free spot for it
            {
                MapObject blocker = map.GetMapObjectAt(cornerpt);
                if (blocker != null)
                    map.RemoveMapObjectAt(cornerpt.X, cornerpt.Y); // remove any blocking object in the shop. it should only ever be a shelf if anything, as we checked for exits already
                if (map.IsWalkable(cornerpt.X, cornerpt.Y)) // make sure we haven't somehow got a wall or other inaccessible spot
                    map.PlaceMapObjectAt(MakeObjCheckout(GameImages.OBJ_CASH_REGISTER), cornerpt);
            }
            #endregion

            ///////////
            // 6. Zone
            ///////////
            // shop building.
            map.AddZone(MakeUniqueZone(shopName, b.BuildingRect));
            // walkway zones.
            MakeWalkwayZones(map, b);


            // Done
            return true;
        }

        protected virtual bool MakeChurchBuilding(Map map, Block b) //@@MP (Release 4)
        {
            ////////////////////////
            // 0. Check suitability
            ////////////////////////
            if (b.InsideRect.Width < 5 || b.InsideRect.Height < 5)
                return false;

            /////////////////////////////
            // 1. Walkway, floor & walls
            /////////////////////////////
            TileRectangle(map, m_Game.GameTiles.FLOOR_WALKWAY, b.Rectangle);
            TileRectangle(map, m_Game.GameTiles.WALL_LIGHT_BROWN, b.BuildingRect);
            TileFill(map, m_Game.GameTiles.FLOOR_PLANKS, b.InsideRect, (tile, prevmodel, x, y) => tile.IsInside = true);

            //////////////////////////////////////////
            // 2. Make rows of pews.
            //////////////////////////////////////////            
            #region
            int alleysStartX = b.InsideRect.Left;
            int alleysStartY = b.InsideRect.Top;
            int alleysEndX = b.InsideRect.Right;
            int alleysEndY = b.InsideRect.Bottom;
            bool horizontalAlleys = b.Rectangle.Width >= b.Rectangle.Height;
            int centralAlley;

            if (horizontalAlleys)
            {
                ++alleysStartX;
                --alleysEndX;
                centralAlley = b.InsideRect.Left + b.InsideRect.Width / 2;
            }
            else
            {
                ++alleysStartY;
                --alleysEndY;
                centralAlley = b.InsideRect.Top + b.InsideRect.Height / 2;
            }
            Rectangle alleysRect = Rectangle.FromLTRB(alleysStartX, alleysStartY, alleysEndX, alleysEndY);

            MapObjectFill(map, alleysRect,
                (pt) =>
                {
                    bool addShelf;

                    if (horizontalAlleys)
                    {
                        addShelf = ((pt.Y - alleysRect.Top) % 2 == 1) && pt.X != centralAlley;
                        if (pt.X == centralAlley)
                            map.SetTileModelAt(pt.X, pt.Y, m_Game.GameTiles.FLOOR_RED_CARPET);
                    }
                    else
                    {
                        addShelf = ((pt.X - alleysRect.Left) % 2 == 1) && pt.Y != centralAlley;
                        if (pt.Y == centralAlley)
                            map.SetTileModelAt(pt.X, pt.Y, m_Game.GameTiles.FLOOR_RED_CARPET);
                    }

                    if (addShelf && CountAdjWalls(map, pt.X, pt.Y) == 0)
                        return MakeObjBench(GameImages.OBJ_CHURCH_PEW);
                    else
                        return null;
                });
            #endregion

            ///////////////////////////////
            // 3. Entry door with shop ids
            //    Add lectern and hangings.
            ///////////////////////////////
            #region
            int midX = b.Rectangle.Left + b.Rectangle.Width / 2;
            int midY = b.Rectangle.Top + b.Rectangle.Height / 2;
            string[] CHURCH_HANGINGS = { GameImages.DECO_CHURCH_HANGING1, GameImages.DECO_CHURCH_HANGING2, GameImages.DECO_CHURCH_HANGING3, GameImages.DECO_CHURCH_HANGING4 };
            string chosenHanging = CHURCH_HANGINGS[m_DiceRoller.Roll(0, CHURCH_HANGINGS.Length)];

            // make doors on one side.
            if (!horizontalAlleys)
            {
                bool west = m_DiceRoller.RollChance(50);

                if (west)
                {
                    // west
                    PlaceDoor(map, b.BuildingRect.Left, midY, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjWoodenDoor());

                    //@@MP - lectern opposite to the door (east)
                    Point pt = new Point(b.BuildingRect.Right - 2, midY);
                    map.PlaceMapObjectAt(MakeObjDrawer(GameImages.OBJ_LECTERN), pt);
                    map.DropItemAt(MakeItemCHARBook(), pt); //@@MP (Release 7-6)

                    //@@MP - display cases (with antique melee weapons) flanking lecturn, wall hangings on each side
                    #region south side
                    map.PlaceMapObjectAt(MakeObjDisplayCase(GameImages.OBJ_DISPLAY_CASE), new Point(b.BuildingRect.Right - 2, midY - 1));
                    map.DropItemAt(MakeItemRandomAntiqueWeapon(), new Point(b.BuildingRect.Right - 2, midY - 1));
                    Tile tile = map.GetTileAt(b.BuildingRect.Right - 2, midY - 2);
                    tile.AddDecoration(chosenHanging);
                    #endregion
                    #region north side
                    map.PlaceMapObjectAt(MakeObjDisplayCase(GameImages.OBJ_DISPLAY_CASE), new Point(b.BuildingRect.Right - 2, midY + 1));
                    map.DropItemAt(MakeItemRandomAntiqueWeapon(), new Point(b.BuildingRect.Right - 2, midY + 1));
                    tile = map.GetTileAt(b.BuildingRect.Right - 2, midY + 2);
                    tile.AddDecoration(chosenHanging);
                    #endregion
                }
                else
                {
                    // east
                    PlaceDoor(map, b.BuildingRect.Right - 1, midY, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjWoodenDoor());

                    //@@MP - lectern opposite to the door (west)
                    Point pt = new Point(b.BuildingRect.Left + 1, midY);
                    map.PlaceMapObjectAt(MakeObjDrawer(GameImages.OBJ_LECTERN), pt);
                    map.DropItemAt(MakeItemCHARBook(), pt); //@@MP (Release 7-6)

                    //@@MP - display cases (with antique melee weapons) flanking lecturn, wall hangings on each side
                    #region south side
                    map.PlaceMapObjectAt(MakeObjDisplayCase(GameImages.OBJ_DISPLAY_CASE), new Point(b.BuildingRect.Left + 1, midY - 1));
                    map.DropItemAt(MakeItemRandomAntiqueWeapon(), new Point(b.BuildingRect.Left + 1, midY - 1));
                    Tile tile = map.GetTileAt(b.BuildingRect.Left + 1, midY - 2);
                    tile.AddDecoration(chosenHanging);
                    #endregion
                    #region north side
                    map.PlaceMapObjectAt(MakeObjDisplayCase(GameImages.OBJ_DISPLAY_CASE), new Point(b.BuildingRect.Left + 1, midY + 1));
                    map.DropItemAt(MakeItemRandomAntiqueWeapon(), new Point(b.BuildingRect.Left + 1, midY + 1));
                    tile = map.GetTileAt(b.BuildingRect.Left + 1, midY + 2);
                    tile.AddDecoration(chosenHanging);
                    #endregion
                }
            }
            else
            {
                bool north = m_DiceRoller.RollChance(50);

                if (north)
                {
                    // north
                    PlaceDoor(map, midX, b.BuildingRect.Top, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjWoodenDoor());

                    //@@MP - lectern opposite to the door (south)
                    Point pt = new Point(midX, b.BuildingRect.Bottom - 2);
                    map.PlaceMapObjectAt(MakeObjDrawer(GameImages.OBJ_LECTERN), pt);
                    map.DropItemAt(MakeItemCHARBook(), pt); //@@MP (Release 7-6)

                    //@@MP - display cases (with antique melee weapons) flanking lecturn, wall hangings on each side
                    #region east side
                    map.PlaceMapObjectAt(MakeObjDisplayCase(GameImages.OBJ_DISPLAY_CASE), new Point(midX + 1, b.BuildingRect.Bottom - 2));
                    map.DropItemAt(MakeItemRandomAntiqueWeapon(), new Point(midX + 1, b.BuildingRect.Bottom - 2));
                    Tile tile = map.GetTileAt(midX + 2, b.BuildingRect.Bottom - 2);
                    tile.AddDecoration(chosenHanging);
                    #endregion
                    #region west side
                    map.PlaceMapObjectAt(MakeObjDisplayCase(GameImages.OBJ_DISPLAY_CASE), new Point(midX - 1, b.BuildingRect.Bottom - 2));
                    map.DropItemAt(MakeItemRandomAntiqueWeapon(), new Point(midX - 1, b.BuildingRect.Bottom - 2));
                    tile = map.GetTileAt(midX - 2, b.BuildingRect.Bottom - 2);
                    tile.AddDecoration(chosenHanging);
                    #endregion
                }
                else
                {
                    // south
                    PlaceDoor(map, midX, b.BuildingRect.Bottom - 1, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjWoodenDoor());

                    //@@MP - lectern opposite to the door (north)
                    Point pt = new Point(midX, b.BuildingRect.Top + 1);
                    map.PlaceMapObjectAt(MakeObjDrawer(GameImages.OBJ_LECTERN), pt);
                    map.DropItemAt(MakeItemCHARBook(), pt); //@@MP (Release 7-6)

                    #region east side
                    map.PlaceMapObjectAt(MakeObjDisplayCase(GameImages.OBJ_DISPLAY_CASE), new Point(midX + 1, b.BuildingRect.Top + 1));
                    map.DropItemAt(MakeItemRandomAntiqueWeapon(), new Point(midX + 1, b.BuildingRect.Top + 1));
                    Tile tile = map.GetTileAt(midX + 2, b.BuildingRect.Top + 1);
                    tile.AddDecoration(chosenHanging);
                    #endregion
                    #region west side
                    map.PlaceMapObjectAt(MakeObjDisplayCase(GameImages.OBJ_DISPLAY_CASE), new Point(midX - 1, b.BuildingRect.Top + 1));
                    map.DropItemAt(MakeItemRandomAntiqueWeapon(), new Point(midX - 1, b.BuildingRect.Top + 1));
                    tile = map.GetTileAt(midX - 2, b.BuildingRect.Top + 1);
                    tile.AddDecoration(chosenHanging);
                    #endregion
                }
            }

            // add building image next to doors.
            DecorateOutsideWalls(map, b.BuildingRect, (x, y) => map.GetMapObjectAt(x, y) == null && CountAdjDoors(map, x, y) >= 1 ? GameImages.DECO_CHURCH : null);
            #endregion

            ///////////
            // 4. Zone
            ///////////
            // demark building.
            string buildingName = "Church";
            map.AddZone(MakeUniqueZone(buildingName, b.BuildingRect));
            map.HasChurch = true; //@@MP (Release 6-6)
            // walkway zones.
            MakeWalkwayZones(map, b);
            
            // Done
            return true;
        }

        protected virtual bool MakeBarBuilding(Map map, Block b, ref int barsCount) //@@MP (Release 4)
        {
            ////////////////////////
            // 0. Check suitability
            ////////////////////////
            if (b.InsideRect.Width < 5 || b.InsideRect.Height < 5)
                return false;
            double barsLimit = Math.Round(((double)(map.Width / 10)) / 2.5); //cap the number per map based on its dimensions
            if ((double)barsCount >= barsLimit)
                return false;
            else
                ++barsCount;

            /////////////////////////////
            // 1. Walkway, floor & walls
            /////////////////////////////
            TileRectangle(map, m_Game.GameTiles.FLOOR_WALKWAY, b.Rectangle);
            TileRectangle(map, m_Game.GameTiles.WALL_LIGHT_BROWN, b.BuildingRect);
            TileFill(map, m_Game.GameTiles.FLOOR_PLANKS, b.InsideRect, (tile, prevmodel, x, y) => tile.IsInside = true);

            ///////////////////////////////
            // 2. Entry door with shop ids
            //    Add lectern and hangings.
            ///////////////////////////////
            #region
            int midX = b.Rectangle.Left + b.Rectangle.Width / 2;
            int midY = b.Rectangle.Top + b.Rectangle.Height / 2;

            //hold the area within the bar. after the door position is determined we'll reduce the area held to only in front of the counter.
            //we'll use this held area to place tables later
            int tablesareabottom = b.InsideRect.Bottom;
            int tablesareatop = b.InsideRect.Top;
            int tablesarealeft = b.InsideRect.Left;
            int tablesarearight = b.InsideRect.Right;

            // make doors on one side.
            int doorside = m_DiceRoller.Roll(0, 4);
            switch (doorside)
            {
                case 0:
                    // west
                    PlaceDoor(map, b.BuildingRect.Left, midY, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjWoodenDoor());

                    //place velvet entry rope
                    Point westropept = new Point(b.BuildingRect.Left - 1, midY);
                    map.RemoveMapObjectAt(westropept.X, westropept.Y); //get rid of cars
                    Tile westtile = map.GetTileAt(westropept.X, westropept.Y);
                    westtile.AddDecoration(GameImages.DECO_VELVET_ROPE);

                    //center the sink opposite the door
                    Point eastsinkpt = new Point(b.InsideRect.Right - 1, midY);
                    map.PlaceMapObjectAt(MakeObjKitchenSink(GameImages.OBJ_KITCHEN_SINK), eastsinkpt);

                    //create the shelves and counters
                    tablesarearight -= 4;
                    int eastshelvesX = b.InsideRect.Right - 1;
                    for (int i = 1; i <= b.InsideRect.Height; i++)
                    {
                        int y = b.InsideRect.Bottom - i;

                        //place a shelf with alcohol, but not the middle as we put the sink there
                        Point shelfpt = new Point(eastshelvesX, y);
                        if (map.IsWalkable(shelfpt) && y != midY)
                        {
                            westtile = map.GetTileAt(eastshelvesX, y);
                            map.PlaceMapObjectAt(MakeObjShelf(GameImages.OBJ_BAR_SHELVES), shelfpt);
                            map.DropItemAt(MakeItemAlcohol(), shelfpt);
                        }

                        //place a bar counter another 2 tiles in from the shelves
                        Point counterpt = new Point(eastshelvesX - 2, y);
                        if (map.IsWalkable(counterpt))
                        {
                            westtile = map.GetTileAt(counterpt.X, counterpt.Y);
                            map.PlaceMapObjectAt(MakeObjCounter(GameImages.OBJ_KITCHEN_COUNTER), counterpt); //@@MP - changed to BarCounter to keep them jumpable (Release 5-3)
                            map.DropItemAt(MakeItemAlcohol(), shelfpt);
                        }
                    }
                    break;
                case 1:
                    // east
                    PlaceDoor(map, b.BuildingRect.Right - 1, midY, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjWoodenDoor());

                    //place velvet entry rope
                    Point eastropept = new Point(b.BuildingRect.Right, midY);
                    map.RemoveMapObjectAt(eastropept.X, eastropept.Y); //get rid of cars
                    Tile easttile = map.GetTileAt(eastropept.X, eastropept.Y);
                    easttile.AddDecoration(GameImages.DECO_VELVET_ROPE);

                    //center the sink opposite the door
                    Point westsinkpt = new Point(b.InsideRect.Left, midY);
                    map.PlaceMapObjectAt(MakeObjKitchenSink(GameImages.OBJ_KITCHEN_SINK), westsinkpt);

                    //create the shelves and counters
                    tablesarealeft += 4;
                    int westshelvesX = b.InsideRect.Left;
                    for (int i = 1; i <= b.InsideRect.Height; i++)
                    {
                        int y = b.InsideRect.Bottom - i;

                        //place a shelf with alcohol, but not the middle as we put the sink there
                        Point shelfpt = new Point(westshelvesX, y);
                        if (map.IsWalkable(shelfpt) && y != midY)
                        {
                            easttile = map.GetTileAt(westshelvesX, y);
                            map.PlaceMapObjectAt(MakeObjShelf(GameImages.OBJ_BAR_SHELVES), shelfpt);
                            map.DropItemAt(MakeItemAlcohol(), shelfpt);
                        }

                        //place a bar counter another 2 tiles in from the shelves
                        Point counterpt = new Point(westshelvesX + 2, y);
                        if (map.IsWalkable(counterpt))
                        {
                            westtile = map.GetTileAt(counterpt.X, counterpt.Y);
                            map.PlaceMapObjectAt(MakeObjCounter(GameImages.OBJ_KITCHEN_COUNTER), counterpt); //@@MP - changed to BarCounter to keep them jumpable (Release 5-3)
                            map.DropItemAt(MakeItemAlcohol(), shelfpt);
                        }
                    }
                    break;
                case 2:
                    // north
                    PlaceDoor(map, midX, b.BuildingRect.Top, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjWoodenDoor());

                    //place velvet entry rope
                    Point northropept = new Point(midX, b.BuildingRect.Top - 1);
                    map.RemoveMapObjectAt(northropept.X, northropept.Y); //get rid of cars
                    Tile northtile = map.GetTileAt(northropept.X, northropept.Y);
                    northtile.AddDecoration(GameImages.DECO_VELVET_ROPE);

                    //center the sink opposite the door
                    Point southsinkpt = new Point(midX, b.InsideRect.Bottom - 1);
                    map.PlaceMapObjectAt(MakeObjKitchenSink(GameImages.OBJ_KITCHEN_SINK), southsinkpt);

                    //create the shelves and counters
                    tablesareabottom -= 4;
                    int southshelvesY = b.InsideRect.Bottom - 1;
                    for (int i = 0; i <= b.InsideRect.Width; i++)
                    {
                        int x = b.InsideRect.Left + i;

                        //place a shelf with alcohol, but not the middle as we put the sink there
                        Point shelfpt = new Point(x, southshelvesY);
                        if (map.IsWalkable(shelfpt) && x != midX)
                        {
                            easttile = map.GetTileAt(x, southshelvesY);
                            map.PlaceMapObjectAt(MakeObjShelf(GameImages.OBJ_BAR_SHELVES), shelfpt);
                            map.DropItemAt(MakeItemAlcohol(), shelfpt);
                        }

                        //place a bar counter another 2 tiles in from the shelves
                        Point counterpt = new Point(x, southshelvesY - 2);
                        if (map.IsWalkable(counterpt))
                        {
                            westtile = map.GetTileAt(counterpt.X, counterpt.Y);
                            map.PlaceMapObjectAt(MakeObjCounter(GameImages.OBJ_KITCHEN_COUNTER), counterpt); //@@MP - changed to BarCounter to keep them jumpable (Release 5-3)
                            map.DropItemAt(MakeItemAlcohol(), shelfpt);
                        }
                    }
                    break;
                case 3:
                    // south
                    PlaceDoor(map, midX, b.BuildingRect.Bottom - 1, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjWoodenDoor());

                    //place velvet entry rope
                    Point southropept = new Point(midX, b.BuildingRect.Bottom);
                    map.RemoveMapObjectAt(southropept.X, southropept.Y); //get rid of cars
                    Tile southtile = map.GetTileAt(southropept.X, southropept.Y);
                    southtile.AddDecoration(GameImages.DECO_VELVET_ROPE);

                    //center the sink opposite the door
                    Point northsinkpt = new Point(midX, b.InsideRect.Top);
                    map.PlaceMapObjectAt(MakeObjKitchenSink(GameImages.OBJ_KITCHEN_SINK), northsinkpt);

                    //create the shelves and counters
                    tablesareatop += 4;
                    int northshelvesY = b.InsideRect.Top;
                    for (int i = 0; i <= b.InsideRect.Width; i++)
                    {
                        int x = b.InsideRect.Left + i;

                        //place a shelf with alcohol, but not the middle as we put the sink there
                        Point shelfpt = new Point(x, northshelvesY);
                        if (map.IsWalkable(shelfpt) && x != midX)
                        {
                            easttile = map.GetTileAt(x, northshelvesY);
                            map.PlaceMapObjectAt(MakeObjShelf(GameImages.OBJ_BAR_SHELVES), shelfpt);
                            map.DropItemAt(MakeItemAlcohol(), shelfpt);
                        }

                        //place a bar counter another 2 tiles in from the shelves
                        Point counterpt = new Point(x, northshelvesY + 2);
                        if (map.IsWalkable(counterpt))
                        {
                            westtile = map.GetTileAt(counterpt.X, counterpt.Y);
                            map.PlaceMapObjectAt(MakeObjCounter(GameImages.OBJ_KITCHEN_COUNTER), counterpt); //@@MP - changed to BarCounter to keep them jumpable (Release 5-3)
                            map.DropItemAt(MakeItemAlcohol(), shelfpt);
                        }
                    }
                    break;
            }

            // add building image next to doors.
            DecorateOutsideWalls(map, b.BuildingRect, (x, y) => map.GetMapObjectAt(x, y) == null && CountAdjDoors(map, x, y) >= 1 ? GameImages.DECO_BAR : null);
            #endregion

            ///////////
            // 3. Add tables and chairs
            ///////////
            #region
            int nbTables = 0;
            switch (doorside)
            {
                case 0:
                case 1:
                    nbTables = m_DiceRoller.Roll(Math.Max((tablesarealeft - tablesarearight), b.InsideRect.Height), Math.Max((tablesarealeft - tablesarearight), b.InsideRect.Height)); //@@MP - removed the multiplier (Release 6-1)
                    break;
                case 2:
                case 3:
                    nbTables = m_DiceRoller.Roll(Math.Max(b.InsideRect.Width, (tablesareabottom - tablesareatop)), Math.Max(b.InsideRect.Width, (tablesareabottom - tablesareatop))); //@@MP - removed the multiplier (Release 6-1)
                    break;
            }

            int tablesareawidth = Math.Abs(tablesarealeft - tablesarearight);
            int tablesareaheight = Math.Abs(tablesareabottom - tablesareatop);
            Rectangle insideRoom = new Rectangle(tablesarealeft, tablesareatop, tablesareawidth, tablesareaheight);
            for (int i = 0; i < nbTables; i++)
            {
                MapObjectPlaceInGoodPosition(map, insideRoom,
                    (pt) => !IsADoorNSEW(map, pt.X, pt.Y),
                    m_DiceRoller,
                    (pt) =>
                    {
                        // three chairs around //@@MP (Release 3)
                        Rectangle adjTableRect = new Rectangle(pt.X - 1, pt.Y - 1, 3, 3);
                        adjTableRect.Intersect(insideRoom);
                        MapObjectPlaceInGoodPosition(map, adjTableRect,
                            (pt2) => pt2 != pt && !IsADoorNSEW(map, pt2.X, pt2.Y),
                            m_DiceRoller,
                            (pt2) => MakeObjChair(GameImages.OBJ_CHAIR));
                        MapObjectPlaceInGoodPosition(map, adjTableRect,
                            (pt2) => pt2 != pt && !IsADoorNSEW(map, pt2.X, pt2.Y),
                            m_DiceRoller,
                            (pt2) => MakeObjChair(GameImages.OBJ_CHAIR));
                        MapObjectPlaceInGoodPosition(map, adjTableRect,
                            (pt2) => pt2 != pt && !IsADoorNSEW(map, pt2.X, pt2.Y),
                            m_DiceRoller,
                            (pt2) => MakeObjChair(GameImages.OBJ_CHAIR));
                        MapObjectPlaceInGoodPosition(map, adjTableRect,
                            (pt2) => pt2 != pt && !IsADoorNSEW(map, pt2.X, pt2.Y),
                            m_DiceRoller,
                            (pt2) => MakeObjChair(GameImages.OBJ_CHAIR));

                        /*// items.
                        if (m_DiceRoller.RollChance(20))
                        {
                            for (int ii = 0; ii < HOUSE_LIVINGROOM_ITEMS_ON_TABLE; ii++)
                            {
                                Item it = MakeItemAlcohol();
                                if (it != null)
                                    map.DropItemAt(it, pt);
                            }
                        }*/

                        // table.
                        MapObject table = MakeObjTable(GameImages.OBJ_TABLE);
                        return table;
                    });
            };
            #endregion

            ///////////
            // 4. Zone
            ///////////
            // demark building.
            string buildingName = "Bar";
            map.AddZone(MakeUniqueZone(buildingName, b.BuildingRect));
            // walkway zones.
            MakeWalkwayZones(map, b);

            // Done
            return true;
        }
        
        protected virtual bool MakeMechanicWorkshop(Map map, Block b, ref int mechanicsCount) //@@MP (Release 4)
        {
            ////////////////////////
            // 0. Check suitability
            ////////////////////////
            if (b.InsideRect.Width > 7 || b.InsideRect.Height > 7)
                return false;
            if (b.InsideRect.Width < 5 || b.InsideRect.Height < 5)
                return false;
            double mechanicsLimit = Math.Round(((double)(map.Width / 10)) / 4); //cap the number per map based on its dimensions
            if ((double)mechanicsCount >= mechanicsLimit)
                return false;
            else
                ++mechanicsCount;

            /////////////////////////////
            // 1. Walkway, floor & walls
            /////////////////////////////
            TileRectangle(map, m_Game.GameTiles.FLOOR_WALKWAY, b.Rectangle);
            TileRectangle(map, m_Game.GameTiles.WALL_STONE, b.BuildingRect);
            TileFill(map, m_Game.GameTiles.FLOOR_CONCRETE, b.InsideRect, (tile, prevmodel, x, y) => tile.IsInside = true);

            ///////////////////////////////
            // 2. Entry door with shop ids
            //    Add driveway
            ///////////////////////////////
            #region
            int midX = b.Rectangle.Left + b.Rectangle.Width / 2;
            int midY = b.Rectangle.Top + b.Rectangle.Height / 2;

            // make doors on one side.
            int doorside = m_DiceRoller.Roll(0, 4);
            switch (doorside)
            {
                case 0:
                    // west
                    PlaceDoor(map, b.BuildingRect.Left, midY, m_Game.GameTiles.FLOOR_CONCRETE, MakeObjRollerDoor());
                    PlaceDoor(map, b.BuildingRect.Left, midY - 1, m_Game.GameTiles.FLOOR_CONCRETE, MakeObjRollerDoor());
                    PlaceDoor(map, b.BuildingRect.Left, midY + 1, m_Game.GameTiles.FLOOR_CONCRETE, MakeObjRollerDoor());

                    //place driveway
                    Point westdrivewaypt = new Point(b.BuildingRect.Left - 1, midY);
                    map.RemoveMapObjectAt(westdrivewaypt.X, westdrivewaypt.Y); //get rid of cars
                    map.SetTileModelAt(westdrivewaypt.X, westdrivewaypt.Y, m_Game.GameTiles.FLOOR_ASPHALT);

                    break;
                case 1:
                    // east
                    PlaceDoor(map, b.BuildingRect.Right - 1, midY, m_Game.GameTiles.FLOOR_CONCRETE, MakeObjRollerDoor());
                    PlaceDoor(map, b.BuildingRect.Right - 1, midY - 1, m_Game.GameTiles.FLOOR_CONCRETE, MakeObjRollerDoor());
                    PlaceDoor(map, b.BuildingRect.Right - 1, midY + 1, m_Game.GameTiles.FLOOR_CONCRETE, MakeObjRollerDoor());

                    //place driveway
                    Point eastdrivewaypt = new Point(b.BuildingRect.Right, midY);
                    map.RemoveMapObjectAt(eastdrivewaypt.X, eastdrivewaypt.Y); //get rid of cars
                    map.SetTileModelAt(eastdrivewaypt.X, eastdrivewaypt.Y, m_Game.GameTiles.FLOOR_ASPHALT);

                    break;
                case 2:
                    // north
                    PlaceDoor(map, midX, b.BuildingRect.Top, m_Game.GameTiles.FLOOR_CONCRETE, MakeObjRollerDoor());
                    PlaceDoor(map, midX - 1, b.BuildingRect.Top, m_Game.GameTiles.FLOOR_CONCRETE, MakeObjRollerDoor());
                    PlaceDoor(map, midX + 1, b.BuildingRect.Top, m_Game.GameTiles.FLOOR_CONCRETE, MakeObjRollerDoor());

                    //place driveway
                    Point northdrivewaypt = new Point(midX, b.BuildingRect.Top - 1);
                    map.RemoveMapObjectAt(northdrivewaypt.X, northdrivewaypt.Y); //get rid of cars
                    map.SetTileModelAt(northdrivewaypt.X, northdrivewaypt.Y, m_Game.GameTiles.FLOOR_ASPHALT);

                    break;
                case 3:
                    // south
                    PlaceDoor(map, midX, b.BuildingRect.Bottom - 1, m_Game.GameTiles.FLOOR_CONCRETE, MakeObjRollerDoor());
                    PlaceDoor(map, midX - 1, b.BuildingRect.Bottom - 1, m_Game.GameTiles.FLOOR_CONCRETE, MakeObjRollerDoor());
                    PlaceDoor(map, midX + 1, b.BuildingRect.Bottom - 1, m_Game.GameTiles.FLOOR_CONCRETE, MakeObjRollerDoor());

                    //place driveway
                    Point southdrivewaypt = new Point(midX, b.BuildingRect.Bottom);
                    map.RemoveMapObjectAt(southdrivewaypt.X, southdrivewaypt.Y); //get rid of cars
                    map.SetTileModelAt(southdrivewaypt.X, southdrivewaypt.Y, m_Game.GameTiles.FLOOR_ASPHALT);

                    break;
            }

            // add building image next to doors.
            DecorateOutsideWalls(map, b.BuildingRect, (x, y) => map.GetMapObjectAt(x, y) == null && CountAdjDoors(map, x, y) >= 1 ? GameImages.DECO_MECHANIC : null);
            #endregion

            ///////////
            // 3. Add workbenches and cars
            ///////////
            #region
            bool placedGenerator = false; //@@MP - we only want to place one generator (Release 6-2)
            MapObjectFill(map, b.InsideRect,
                (pt) =>
                {
                    if (CountAdjWalls(map, pt.X, pt.Y) < 3)
                        return null;
                    else if (m_DiceRoller.RollChance(20))
                        map.DropItemAt(MakeShopConstructionItem(), pt);

                    if (!placedGenerator)
                    {
                        placedGenerator = true;
                        return MakeObjPowerGenerator(GameImages.OBJ_POWERGEN_OFF, GameImages.OBJ_POWERGEN_ON); //@@MP (Release 6-2)
                    }
                    else if (m_DiceRoller.RollChance(10))
                        return MakeObjFireBarrel(GameImages.OBJ_EMPTY_BARREL); //@@MP (Release 7-6)
                    else
                        return MakeObjWorkbench(GameImages.OBJ_WORKBENCH); //@@MP (Release 5-3)
                });

            MapObjectFill(map, b.InsideRect,
                            (pt) =>
                            {
                                if (m_DiceRoller.RollChance(m_Params.WreckedCarChance))
                                {
                                    Tile tile = map.GetTileAt(pt.X, pt.Y);
                                    if (tile.IsInside && tile.Model.IsWalkable)
                                    {
                                        MapObject car = MakeObjWreckedCar(m_DiceRoller);
                                        return car;
                                    }
                                }
                                return null;
                            });
            #endregion

            ///////////
            // 4. Zone
            ///////////
            // demark building.
            string buildingName = "Mechanic";
            map.AddZone(MakeUniqueZone(buildingName, b.BuildingRect));
            // walkway zones.
            MakeWalkwayZones(map, b);

            // Done
            return true;
        }

        protected virtual bool MakeFuelStation(Map map, Block b, int fuelStationsPlaced) //@@MP (Release 7-3)
        {
            ////////////////////////
            // 0. Check suitability
            ////////////////////////
            if (b.InsideRect.Width < 8 || b.InsideRect.Width > 11) //can't be too small
                return false;
            else if (b.InsideRect.Height < 8 || b.InsideRect.Height > 11) //can't be too large
                return false;
            double fuelStationsLimit = Math.Round(((double)(map.Width / 10)) / 2.5); //cap the number of stations per map based on its dimensions
            if ((double)fuelStationsPlaced >= fuelStationsLimit)
                return false;

            /////////////////////////////
            // 1. Walkway, floor & walls
            /////////////////////////////
            TileRectangle(map, m_Game.GameTiles.FLOOR_WALKWAY, b.Rectangle);
            TileRectangle(map, m_Game.GameTiles.WALL_FUEL_STATION, b.BuildingRect);
            TileFill(map, m_Game.GameTiles.FLOOR_TILES, b.InsideRect, (tile, prevmodel, x, y) => tile.IsInside = true);

            ///////////////////////////////
            // 2. Entry door with shop ids, driveway and forecourt
            ///////////////////////////////
            #region
            int midX = b.Rectangle.Left + b.Rectangle.Width / 2;
            int midY = b.Rectangle.Top + b.Rectangle.Height / 2;

            // make doors on one side.
            int doorside = m_DiceRoller.Roll(0, 4);
            Rectangle forecourtRect;
            switch (doorside)
            {
                case 0:  // north
                    #region
                    //place driveways and price board
                    Rectangle drivewayNorthLeft = new Rectangle(b.BuildingRect.Left, b.BuildingRect.Top - 1, 3, 1); //x,y,W,H
                    TileRectangle(map, m_Game.GameTiles.FLOOR_ASPHALT, drivewayNorthLeft);
                    Rectangle drivewayNorthRight = new Rectangle(b.BuildingRect.Right - 3, b.BuildingRect.Top - 1, 3, 1); //x,y,W,H
                    TileRectangle(map, m_Game.GameTiles.FLOOR_ASPHALT, drivewayNorthRight);
                    map.RemoveMapObjectAt(midX, b.BuildingRect.Top - 1); //get rid of car
                    MapObjectPlace(map, midX, b.BuildingRect.Top - 1, MakeObjBoard(GameImages.OBJ_FUEL_PRICE_BOARD, new string[] { "Super: 1.17, Regular: 1.14" }));

                    //forecourt
                    forecourtRect = new Rectangle(b.BuildingRect.Left, b.BuildingRect.Top, b.BuildingRect.Width, 3); //x,y,W,H
                    for (int x = 0; x < forecourtRect.Width; x++)
                    {
                        for (int y = 0; y < forecourtRect.Height; y++)
                        {
                            int g = b.BuildingRect.Left + x;
                            int h = b.BuildingRect.Top + y;

                            map.SetTileModelAt(g, h, m_Game.GameTiles.FLOOR_ASPHALT);
                            map.GetTileAt(g, h).IsInside = false; //don't count the forecourt as inside
                            map.RemoveMapObjectAt(g, h); //get rid of cars
                        }
                    }
                    
                    //front wall
                    Rectangle frontWallRectN = new Rectangle(b.BuildingRect.Left, b.BuildingRect.Top + 3, b.BuildingRect.Width, 1);
                    TileRectangle(map, m_Game.GameTiles.WALL_FUEL_STATION, frontWallRectN);
                    DoForEachTile(frontWallRectN,
                    (pt) =>
                    {
                        map.GetTileAt(pt).IsInside = false;
                    });
                    
                    //place doors and signage
                    map.GetTileAt(midX + 2, b.BuildingRect.Top + 3).AddDecoration(GameImages.DECO_SHOP_FUEL_STATION);
                    PlaceFuelStationDoor(map, midX + 1, b.BuildingRect.Top + 3);
                    PlaceFuelStationDoor(map, midX, b.BuildingRect.Top + 3);
                    PlaceFuelStationDoor(map, midX - 1, b.BuildingRect.Top + 3);
                    map.GetTileAt(midX - 2, b.BuildingRect.Top + 3).AddDecoration(GameImages.DECO_SHOP_FUEL_STATION);
                    
                    //place pumps
                    map.SetTileModelAt(midX - 1, b.BuildingRect.Top + 1, m_Game.GameTiles.FLOOR_CONCRETE);
                    map.SetTileModelAt(midX - 2, b.BuildingRect.Top + 1, m_Game.GameTiles.FLOOR_CONCRETE);
                    PlaceFuelPump(map, midX - 3, b.BuildingRect.Top + 1);
                    PlaceFuelPump(map, midX, b.BuildingRect.Top + 1);
                    map.SetTileModelAt(midX + 1, b.BuildingRect.Top + 1, m_Game.GameTiles.FLOOR_CONCRETE);
                    map.SetTileModelAt(midX + 2, b.BuildingRect.Top + 1, m_Game.GameTiles.FLOOR_CONCRETE);
                    PlaceFuelPump(map, midX + 3, b.BuildingRect.Top + 1);
                    
                    break;
                #endregion
                case 1: // south
                    #region
                    //place driveways and price board
                    Rectangle drivewaySouthLeft = new Rectangle(b.BuildingRect.Left, b.BuildingRect.Bottom, 3, 1); //x,y,W,H
                    TileRectangle(map, m_Game.GameTiles.FLOOR_ASPHALT, drivewaySouthLeft);
                    Rectangle drivewaySouthRight = new Rectangle(b.BuildingRect.Right - 3, b.BuildingRect.Bottom, 3, 1); //x,y,W,H
                    TileRectangle(map, m_Game.GameTiles.FLOOR_ASPHALT, drivewaySouthRight);
                    map.RemoveMapObjectAt(midX, b.BuildingRect.Bottom); //get rid of car
                    MapObjectPlace(map, midX, b.BuildingRect.Bottom, MakeObjBoard(GameImages.OBJ_FUEL_PRICE_BOARD, new string[] { "Super: 1.17, Regular: 1.14" }));

                    //forecourt
                    forecourtRect = new Rectangle(b.BuildingRect.Left, b.BuildingRect.Bottom - 3, b.BuildingRect.Width, 3); //x,y,W,H
                    for (int x = 0; x < forecourtRect.Width; x++)
                    {
                        for (int y = 0; y < forecourtRect.Height; y++)
                        {
                            int g = b.BuildingRect.Left + x;
                            int h = ((b.BuildingRect.Bottom + y) - 3);

                            map.SetTileModelAt(g, h, m_Game.GameTiles.FLOOR_ASPHALT);
                            map.GetTileAt(g, h).IsInside = false; //don't count the forecourt as inside
                            map.RemoveMapObjectAt(g, h); //get rid of cars
                        }
                    }
                    
                    //front wall
                    Rectangle frontWallRectS = new Rectangle(b.BuildingRect.Left, b.BuildingRect.Bottom - 4, b.BuildingRect.Width, 1);
                    TileRectangle(map, m_Game.GameTiles.WALL_FUEL_STATION, frontWallRectS);
                    DoForEachTile(frontWallRectS,
                    (pt) =>
                    {
                        map.GetTileAt(pt).IsInside = false;
                    });
                    
                    //place doors and signage
                    map.GetTileAt(midX - 2, b.BuildingRect.Bottom - 4).AddDecoration(GameImages.DECO_SHOP_FUEL_STATION);
                    PlaceFuelStationDoor(map, midX - 1, b.BuildingRect.Bottom - 4);
                    PlaceFuelStationDoor(map, midX, b.BuildingRect.Bottom - 4);
                    PlaceFuelStationDoor(map, midX + 1, b.BuildingRect.Bottom - 4);
                    map.GetTileAt(midX + 2, b.BuildingRect.Bottom - 4).AddDecoration(GameImages.DECO_SHOP_FUEL_STATION);

                    //place pumps
                    PlaceFuelPump(map, midX - 3, b.BuildingRect.Bottom - 2);
                    map.SetTileModelAt(midX - 2, b.BuildingRect.Bottom - 2, m_Game.GameTiles.FLOOR_CONCRETE);
                    map.SetTileModelAt(midX - 1, b.BuildingRect.Bottom - 2, m_Game.GameTiles.FLOOR_CONCRETE);
                    PlaceFuelPump(map, midX, b.BuildingRect.Bottom - 2);
                    map.SetTileModelAt(midX + 1, b.BuildingRect.Bottom - 2, m_Game.GameTiles.FLOOR_CONCRETE);
                    map.SetTileModelAt(midX + 2, b.BuildingRect.Bottom - 2, m_Game.GameTiles.FLOOR_CONCRETE);
                    PlaceFuelPump(map, midX + 3, b.BuildingRect.Bottom - 2);
                    
                    break;
                #endregion
                case 2: // west
                    #region
                    //place driveways and price board
                    Rectangle drivewayWestLeft = new Rectangle(b.BuildingRect.Left - 1, b.BuildingRect.Top, 1, 3); //x,y,W,H
                    TileRectangle(map, m_Game.GameTiles.FLOOR_ASPHALT, drivewayWestLeft);
                    Rectangle drivewayWestRight = new Rectangle(b.BuildingRect.Left - 1, b.BuildingRect.Bottom - 3, 1, 3); //x,y,W,H
                    TileRectangle(map, m_Game.GameTiles.FLOOR_ASPHALT, drivewayWestRight);
                    map.RemoveMapObjectAt(b.BuildingRect.Left - 1, midY); //get rid of car
                    MapObjectPlace(map, b.BuildingRect.Left - 1, midY, MakeObjBoard(GameImages.OBJ_FUEL_PRICE_BOARD, new string[] { "Super: 1.17, Regular: 1.14" }));

                    //forecourt
                    forecourtRect = new Rectangle(b.BuildingRect.Left, b.BuildingRect.Top, 3, b.BuildingRect.Height); //x,y,W,H
                    for (int x = 0; x < forecourtRect.Width; x++)
                    {
                        for (int y = 0; y < forecourtRect.Height; y++)
                        {
                            int g = b.BuildingRect.Left + x;
                            int h = b.BuildingRect.Top + y;

                            map.SetTileModelAt(g, h, m_Game.GameTiles.FLOOR_ASPHALT);
                            map.GetTileAt(g, h).IsInside = false; //don't count the forecourt as inside
                            map.RemoveMapObjectAt(g, h); //get rid of cars
                        }
                    }
                    
                    //front wall
                    Rectangle frontWallRectW = new Rectangle(b.BuildingRect.Left + 3, b.BuildingRect.Top, 1, b.BuildingRect.Height);
                    TileRectangle(map, m_Game.GameTiles.WALL_FUEL_STATION, frontWallRectW);
                    DoForEachTile(frontWallRectW,
                    (pt) =>
                    {
                        map.GetTileAt(pt).IsInside = false;
                    });
                    
                    //place doors and signage
                    map.GetTileAt(b.BuildingRect.Left + 3, midY - 2).AddDecoration(GameImages.DECO_SHOP_FUEL_STATION);
                    PlaceFuelStationDoor(map, b.BuildingRect.Left + 3, midY - 1);
                    PlaceFuelStationDoor(map, b.BuildingRect.Left + 3, midY);
                    PlaceFuelStationDoor(map, b.BuildingRect.Left + 3, midY + 1);
                    map.GetTileAt(b.BuildingRect.Left + 3, midY + 2).AddDecoration(GameImages.DECO_SHOP_FUEL_STATION);

                    //place pumps
                    PlaceFuelPump(map, b.BuildingRect.Left + 1, midY - 3);
                    map.SetTileModelAt(b.BuildingRect.Left + 1, midY - 2, m_Game.GameTiles.FLOOR_CONCRETE);
                    map.SetTileModelAt(b.BuildingRect.Left + 1, midY - 1, m_Game.GameTiles.FLOOR_CONCRETE);
                    PlaceFuelPump(map, b.BuildingRect.Left + 1, midY);
                    map.SetTileModelAt(b.BuildingRect.Left + 1, midY + 1, m_Game.GameTiles.FLOOR_CONCRETE);
                    map.SetTileModelAt(b.BuildingRect.Left + 1, midY + 2, m_Game.GameTiles.FLOOR_CONCRETE);
                    PlaceFuelPump(map, b.BuildingRect.Left + 1, midY + 3);
                    
                    break;
                #endregion
                case 3: // east
                    #region
                    //place driveways and price board
                    Rectangle drivewayEastLeft = new Rectangle(b.BuildingRect.Right, b.BuildingRect.Top, 1, 3); //x,y,W,H
                    TileRectangle(map, m_Game.GameTiles.FLOOR_ASPHALT, drivewayEastLeft);
                    Rectangle drivewayEastRight = new Rectangle(b.BuildingRect.Right, b.BuildingRect.Bottom - 3, 1, 3); //x,y,W,H
                    TileRectangle(map, m_Game.GameTiles.FLOOR_ASPHALT, drivewayEastRight);
                    map.RemoveMapObjectAt(b.BuildingRect.Right, midY); //get rid of car
                    MapObjectPlace(map, b.BuildingRect.Right, midY, MakeObjBoard(GameImages.OBJ_FUEL_PRICE_BOARD, new string[] { "Super: 1.17, Regular: 1.14" }));

                    //forecourt
                    forecourtRect = new Rectangle(b.BuildingRect.Right - 3, b.BuildingRect.Top, 3, b.BuildingRect.Height); //x,y,W,H
                    for (int x = 0; x < forecourtRect.Width; x++)
                    {
                        for (int y = 0; y < forecourtRect.Height; y++)
                        {
                            int g = ((b.BuildingRect.Right + x) - 3);
                            int h = b.BuildingRect.Top + y;

                            map.SetTileModelAt(g, h, m_Game.GameTiles.FLOOR_ASPHALT);
                            map.GetTileAt(g, h).IsInside = false; //don't count the forecourt as inside
                            map.RemoveMapObjectAt(g, h); //get rid of cars
                        }
                    }
                    
                    //front wall
                    Rectangle frontWallRectE = new Rectangle(b.BuildingRect.Right - 4, b.BuildingRect.Top, 1, b.BuildingRect.Height);
                    TileRectangle(map, m_Game.GameTiles.WALL_FUEL_STATION, frontWallRectE);
                    DoForEachTile(frontWallRectE,
                    (pt) =>
                    {
                        map.GetTileAt(pt).IsInside = false;
                    });
                    
                    //place doors and signage
                    map.GetTileAt(b.BuildingRect.Right - 4, midY - 2).AddDecoration(GameImages.DECO_SHOP_FUEL_STATION);
                    PlaceFuelStationDoor(map, b.BuildingRect.Right - 4, midY - 1);
                    PlaceFuelStationDoor(map, b.BuildingRect.Right - 4, midY);
                    PlaceFuelStationDoor(map, b.BuildingRect.Right - 4, midY + 1);
                    map.GetTileAt(b.BuildingRect.Right - 4, midY + 2).AddDecoration(GameImages.DECO_SHOP_FUEL_STATION);

                    //place pumps
                    PlaceFuelPump(map, b.BuildingRect.Right - 2, midY - 3);
                    map.SetTileModelAt(b.BuildingRect.Right - 2, midY - 2, m_Game.GameTiles.FLOOR_CONCRETE);
                    map.SetTileModelAt(b.BuildingRect.Right - 2, midY - 1, m_Game.GameTiles.FLOOR_CONCRETE);
                    PlaceFuelPump(map, b.BuildingRect.Right - 2, midY);
                    map.SetTileModelAt(b.BuildingRect.Right - 2, midY + 1, m_Game.GameTiles.FLOOR_CONCRETE);
                    map.SetTileModelAt(b.BuildingRect.Right - 2, midY + 2, m_Game.GameTiles.FLOOR_CONCRETE);
                    PlaceFuelPump(map, b.BuildingRect.Right - 2, midY + 3);
                    
                    break;
                    #endregion
            }
            #endregion

            //////////////////////////////////////////
            // 3. Make sections alleys with displays.
            //////////////////////////////////////////            
            #region
            int alleysStartX = b.InsideRect.Left;
            int alleysStartY = b.InsideRect.Top;
            int alleysEndX = b.InsideRect.Right;
            int alleysEndY = b.InsideRect.Bottom;

            bool horizontalAlleys = false;
            switch (doorside)
            {
                case 0: //north
                    horizontalAlleys = true;
                    alleysStartY += 4; //account for the forecourt
                    break;
                case 1: //south
                    horizontalAlleys = true;
                    alleysEndY -= 4; //account for the forecourt
                    break;
                case 2: //west
                    alleysStartX += 4; //account for the forecourt
                    break;
                case 3: //east
                    alleysEndX -= 4; //account for the forecourt
                    break;
            }

            if (horizontalAlleys)
            {
                ++alleysStartX;
                --alleysEndX;
            }
            else
            {
                ++alleysStartY;
                --alleysEndY;
            }
            Rectangle alleysRect = Rectangle.FromLTRB(alleysStartX, alleysStartY, alleysEndX, alleysEndY);

            MapObjectFill(map, alleysRect,
                (pt) =>
                {
                    bool addShelf;

                    if (horizontalAlleys)
                        addShelf = ((pt.Y - alleysRect.Top) % 2 == 1);
                    else
                        addShelf = ((pt.X - alleysRect.Left) % 2 == 1);

                    if (addShelf)
                        return MakeObjShelf(GameImages.OBJ_SHOP_SHELF);
                    else
                        return null;
                });
            #endregion

            ///////////
            // 4. Add register
            ///////////
            #region
            bool placedRegister = false; // we only want to place one register, in a front corner
            MapObjectFill(map, b.InsideRect,
                (pt) =>
                {
                    if (!map.IsWalkable(pt.X, pt.Y))
                        return null;
                    else if (!map.GetTileAt(pt.X, pt.Y).IsInside)
                        return null;
                    else if (CountAdjWalls(map, pt.X, pt.Y) < 5)
                        return null;

                    if (!placedRegister)
                    {
                        map.DropItemAt(MakeItemFireExtinguisher(), pt); //@@MP (Release 7-6)

                        placedRegister = true;
                        return MakeObjCheckout(GameImages.OBJ_CASH_REGISTER);
                    }
                    else
                        return null;
                });
            #endregion

            ///////////////////////////
            // 5. Add items to shelves.
            ///////////////////////////
            #region
            ItemsDrop(map, b.InsideRect,
                (pt) =>
                {
                    MapObject mapObj = map.GetMapObjectAt(pt);
                    if (mapObj == null)
                        return false;
                    return mapObj.ImageID == GameImages.OBJ_SHOP_SHELF;
                },
                (pt) => m_DiceRoller.RollChance(15) ? MakeItemSiphonKit() : MakeShopGeneralItem());
            #endregion

            ///////////
            // 6. Zone
            ///////////
            // demark building.
            string buildingName = "Fuel station";
            map.AddZone(MakeUniqueZone(buildingName, b.BuildingRect));
            // walkway zones.
            MakeWalkwayZones(map, b);

            // Done
            return true;
        }

        private void PlaceFuelStationDoor(Map map, int x, int y) //@@MP (Release 7-3)
        {
            MapObject mapObj = map.GetMapObjectAt(x, y);
            if (mapObj != null)
                map.RemoveMapObjectAt(x, y); //get rid of cars

            PlaceDoor(map, x, y, m_Game.GameTiles.FLOOR_TILES, MakeObjGlassDoor());
        }

        private void PlaceFuelPump(Map map, int x, int y) //@@MP (Release 7-3)
        {
            map.SetTileModelAt(x, y, m_Game.GameTiles.FLOOR_CONCRETE);
            MapObjectPlace(map, x, y, MakeObjFuelPump(GameImages.OBJ_FUEL_PUMP));
        }

        protected virtual bool MakeFireStation(Map map, Block b) //@@MP (Release 7-3)
        {
            ////////////////////////
            // 0. Check suitability
            ////////////////////////
            if (b.InsideRect.Width > 6 || b.InsideRect.Height > 6)
                return false;

            /////////////////////////////
            // 1. Walkway, floor & walls
            /////////////////////////////
            TileRectangle(map, m_Game.GameTiles.FLOOR_WALKWAY, b.Rectangle);
            TileRectangle(map, m_Game.GameTiles.WALL_POLICE_STATION, b.BuildingRect);
            TileFill(map, m_Game.GameTiles.FLOOR_CONCRETE, b.InsideRect, (tile, prevmodel, x, y) => tile.IsInside = true);

            ///////////////////////////////
            // 2. Entry door with shop ids
            //    Add driveway
            ///////////////////////////////
            #region
            int midX = b.Rectangle.Left + b.Rectangle.Width / 2;
            int midY = b.Rectangle.Top + b.Rectangle.Height / 2;

            // make doors on one side.
            int doorside = m_DiceRoller.Roll(0, 4);
            switch (doorside)
            {
                case 0:
                    // west
                    PlaceDoor(map, b.BuildingRect.Left, midY, m_Game.GameTiles.FLOOR_CONCRETE, MakeObjRollerDoor());
                    PlaceDoor(map, b.BuildingRect.Left, midY - 1, m_Game.GameTiles.FLOOR_CONCRETE, MakeObjRollerDoor());
                    PlaceDoor(map, b.BuildingRect.Left, midY + 1, m_Game.GameTiles.FLOOR_CONCRETE, MakeObjRollerDoor());

                    //place driveway
                    Point westdrivewaypt = new Point(b.BuildingRect.Left - 1, midY);
                    map.RemoveMapObjectAt(westdrivewaypt.X, westdrivewaypt.Y); //get rid of cars
                    map.SetTileModelAt(westdrivewaypt.X, westdrivewaypt.Y, m_Game.GameTiles.FLOOR_ASPHALT);

                    break;
                case 1:
                    // east
                    PlaceDoor(map, b.BuildingRect.Right - 1, midY, m_Game.GameTiles.FLOOR_CONCRETE, MakeObjRollerDoor());
                    PlaceDoor(map, b.BuildingRect.Right - 1, midY - 1, m_Game.GameTiles.FLOOR_CONCRETE, MakeObjRollerDoor());
                    PlaceDoor(map, b.BuildingRect.Right - 1, midY + 1, m_Game.GameTiles.FLOOR_CONCRETE, MakeObjRollerDoor());

                    //place driveway
                    Point eastdrivewaypt = new Point(b.BuildingRect.Right, midY);
                    map.RemoveMapObjectAt(eastdrivewaypt.X, eastdrivewaypt.Y); //get rid of cars
                    map.SetTileModelAt(eastdrivewaypt.X, eastdrivewaypt.Y, m_Game.GameTiles.FLOOR_ASPHALT);

                    break;
                case 2:
                    // north
                    PlaceDoor(map, midX, b.BuildingRect.Top, m_Game.GameTiles.FLOOR_CONCRETE, MakeObjRollerDoor());
                    PlaceDoor(map, midX - 1, b.BuildingRect.Top, m_Game.GameTiles.FLOOR_CONCRETE, MakeObjRollerDoor());
                    PlaceDoor(map, midX + 1, b.BuildingRect.Top, m_Game.GameTiles.FLOOR_CONCRETE, MakeObjRollerDoor());

                    //place driveway
                    Point northdrivewaypt = new Point(midX, b.BuildingRect.Top - 1);
                    map.RemoveMapObjectAt(northdrivewaypt.X, northdrivewaypt.Y); //get rid of cars
                    map.SetTileModelAt(northdrivewaypt.X, northdrivewaypt.Y, m_Game.GameTiles.FLOOR_ASPHALT);

                    break;
                case 3:
                    // south
                    PlaceDoor(map, midX, b.BuildingRect.Bottom - 1, m_Game.GameTiles.FLOOR_CONCRETE, MakeObjRollerDoor());
                    PlaceDoor(map, midX - 1, b.BuildingRect.Bottom - 1, m_Game.GameTiles.FLOOR_CONCRETE, MakeObjRollerDoor());
                    PlaceDoor(map, midX + 1, b.BuildingRect.Bottom - 1, m_Game.GameTiles.FLOOR_CONCRETE, MakeObjRollerDoor());

                    //place driveway
                    Point southdrivewaypt = new Point(midX, b.BuildingRect.Bottom);
                    map.RemoveMapObjectAt(southdrivewaypt.X, southdrivewaypt.Y); //get rid of cars
                    map.SetTileModelAt(southdrivewaypt.X, southdrivewaypt.Y, m_Game.GameTiles.FLOOR_ASPHALT);

                    break;
            }

            // add building image next to doors.
            DecorateOutsideWalls(map, b.BuildingRect, (x, y) => map.GetMapObjectAt(x, y) == null && CountAdjDoors(map, x, y) >= 1 ? GameImages.DECO_FIRE_STATION : null);
            #endregion

            ///////////
            // 3. Add workbenches, generators and fire truck
            ///////////
            #region
            bool placedGenerator = false; // we only want to place one generator
            MapObjectFill(map, b.InsideRect,
                (pt) =>
                {
                    if (CountAdjWalls(map, pt.X, pt.Y) < 3)
                        return null;
                    else if (m_DiceRoller.RollChance(33))
                    {
                        int roll = m_DiceRoller.Roll(0, 4);
                        switch (roll)
                        {
                            case 0: map.DropItemAt(MakeItemFireAxe(), pt); break;
                            case 1: map.DropItemAt(MakeItemFireHazardSuit(), pt); break;
                            case 2: if (m_DiceRoller.RollChance(15)) map.DropItemAt(MakeItemFlamethrower(), pt); break;
                            case 3: map.DropItemAt(MakeItemFireExtinguisher(), pt); break; //@@MP (Release 7-6)
                            default:
                                throw new InvalidOperationException("unhandled roll");
                        }
                    }

                    if (!placedGenerator)
                    {
                        placedGenerator = true;
                        return MakeObjPowerGenerator(GameImages.OBJ_POWERGEN_OFF, GameImages.OBJ_POWERGEN_ON);
                    }
                    else if (m_DiceRoller.RollChance(10))
                        return MakeObjWorkbench(GameImages.OBJ_WORKBENCH);
                    else if (m_DiceRoller.RollChance(10))
                        return MakeObjFireBarrel(GameImages.OBJ_EMPTY_BARREL); //@@MP (Release 7-6)
                    else
                        return null;
                });

            bool placedFireTruck = false; // we only want to place one at most
            for (int x = b.InsideRect.Left; x < b.InsideRect.Right; x++)
            {
                for (int y = b.InsideRect.Top; y < b.InsideRect.Bottom; y++)
                {
                    if (!placedFireTruck && m_DiceRoller.RollChance(5)) //5%
                    {
                        if (CountAdjWalls(map, x, y) > 2)
                            continue;

                        Tile tile = map.GetTileAt(x, y);
                        if (tile.IsInside && tile.Model.IsWalkable && map.GetMapObjectAt(x,y) == null)
                        {
                            switch (doorside)
                            {
                                //east-west
                                case 0:
                                case 1:
                                    int xF = (x + 1);
                                    Tile tileToTheRight = map.GetTileAt(xF, y);
                                    if (tileToTheRight.IsInside && tileToTheRight.Model.IsWalkable && map.GetMapObjectAt(xF, y) == null)
                                    {
                                        map.PlaceMapObjectAt(MakeObjFireTruck(GameImages.OBJ_FIRE_TRUCK_EW_BACK), new Point(x, y));
                                        map.PlaceMapObjectAt(MakeObjFireTruck(GameImages.OBJ_FIRE_TRUCK_EW_FRONT), new Point(xF, y));
                                        placedFireTruck = true;
                                    }
                                    break;
                                //north-south
                                case 2:
                                case 3:
                                    int yF = (y + 1);
                                    Tile tileBelow = map.GetTileAt(x, yF);
                                    if (tileBelow.IsInside && tileBelow.Model.IsWalkable && map.GetMapObjectAt(x, yF) == null)
                                    {
                                        map.PlaceMapObjectAt(MakeObjFireTruck(GameImages.OBJ_FIRE_TRUCK_NS_BACK), new Point(x, y));
                                        map.PlaceMapObjectAt(MakeObjFireTruck(GameImages.OBJ_FIRE_TRUCK_NS_FRONT), new Point(x, yF));
                                        placedFireTruck = true;
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
            #endregion

            ///////////
            // 4. Zone
            ///////////
            // demark building.
            string buildingName = "Fire station";
            map.AddZone(MakeUniqueZone(buildingName, b.BuildingRect));
            // walkway zones.
            MakeWalkwayZones(map, b);

            // Done
            return true;
        }

        protected virtual bool MakeClinicBuilding(Map map, Block b, ref int clinicsCount) //@@MP (Release 4)
        {
            ////////////////////////
            // 0. Check suitability
            ////////////////////////
            if (b.InsideRect.Width < 5 || b.InsideRect.Height < 5)
                return false;
            double clinicsLimit = Math.Round(((double)(map.Width / 10)) / 2.5); //cap the number per map based on its dimensions
            if ((double)clinicsCount >= clinicsLimit)
                return false;
            else
                ++clinicsCount;

            /////////////////////////////
            // 1. Walkway, floor & walls
            /////////////////////////////
            TileRectangle(map, m_Game.GameTiles.FLOOR_WALKWAY, b.Rectangle);
            TileRectangle(map, m_Game.GameTiles.WALL_STONE, b.BuildingRect);
            TileFill(map, m_Game.GameTiles.FLOOR_TILES, b.InsideRect, (tile, prevmodel, x, y) => tile.IsInside = true);

            ///////////////////////////////
            // 2. Entry door with shop ids
            //    Add lectern and hangings.
            ///////////////////////////////
            #region
            int midX = b.Rectangle.Left + b.Rectangle.Width / 2;
            int midY = b.Rectangle.Top + b.Rectangle.Height / 2;

            //hold the area within the clinic. after the door position is determined we'll use this held area to place objects
            int tablesareabottom = b.InsideRect.Bottom;
            int tablesareatop = b.InsideRect.Top;
            int tablesarealeft = b.InsideRect.Left;
            int tablesarearight = b.InsideRect.Right;

            // make doors on one side.
            Point receptiondeskpt;
            int doorside = m_DiceRoller.Roll(0, 4);
            switch (doorside)
            {
                case 0:
                    // west
                    PlaceDoor(map, b.BuildingRect.Left, midY, m_Game.GameTiles.FLOOR_CONCRETE, MakeObjHospitalDoor());

                    //place desk by the door
                    receptiondeskpt = new Point(b.BuildingRect.Left + 1, midY - 1);
                    if (map.IsWalkable(receptiondeskpt.X, receptiondeskpt.Y)) // make sure we haven't somehow got a wall or other inaccessible spot
                        map.PlaceMapObjectAt(MakeObjReceptionDesk(GameImages.OBJ_CLINIC_DESK), receptiondeskpt);
                    break;
                case 1:
                    // east
                    PlaceDoor(map, b.BuildingRect.Right - 1, midY, m_Game.GameTiles.FLOOR_CONCRETE, MakeObjHospitalDoor());

                    //place desk by the door
                    receptiondeskpt = new Point(b.BuildingRect.Right - 2, midY + 1);
                    if (map.IsWalkable(receptiondeskpt.X, receptiondeskpt.Y)) // make sure we haven't somehow got a wall or other inaccessible spot
                        map.PlaceMapObjectAt(MakeObjReceptionDesk(GameImages.OBJ_CLINIC_DESK), receptiondeskpt);
                    break;
                case 2:
                    // north
                    PlaceDoor(map, midX, b.BuildingRect.Top, m_Game.GameTiles.FLOOR_CONCRETE, MakeObjHospitalDoor());

                    //place desk by the door
                    receptiondeskpt = new Point(midX - 1, b.BuildingRect.Top + 1);
                    if (map.IsWalkable(receptiondeskpt.X, receptiondeskpt.Y)) // make sure we haven't somehow got a wall or other inaccessible spot
                        map.PlaceMapObjectAt(MakeObjReceptionDesk(GameImages.OBJ_CLINIC_DESK), receptiondeskpt);
                    break;
                case 3:
                    // south
                    PlaceDoor(map, midX, b.BuildingRect.Bottom - 1, m_Game.GameTiles.FLOOR_CONCRETE, MakeObjHospitalDoor());

                    //place desk by the door
                    receptiondeskpt = new Point(midX + 1, b.BuildingRect.Bottom - 2);
                    if (map.IsWalkable(receptiondeskpt.X, receptiondeskpt.Y)) // make sure we haven't somehow got a wall or other inaccessible spot
                        map.PlaceMapObjectAt(MakeObjReceptionDesk(GameImages.OBJ_CLINIC_DESK), receptiondeskpt);
                    break;
            }

            // add building image next to doors.
            DecorateOutsideWalls(map, b.BuildingRect, (x, y) => map.GetMapObjectAt(x, y) == null && CountAdjDoors(map, x, y) >= 1 ? GameImages.DECO_CLINIC_SIGN : null);
            #endregion

            ///////////
            // 3. Work out dimensions for placement
            ///////////
            #region
            int nbTables = 0;
            switch (doorside)
            {
                case 0:
                case 1:
                    nbTables = m_DiceRoller.Roll(Math.Max((tablesarealeft - tablesarearight), b.InsideRect.Height), Math.Max((tablesarealeft - tablesarearight), b.InsideRect.Height)); //@@MP - removed multiplier (Release 6-1)
                    break;
                case 2:
                case 3:
                    nbTables = m_DiceRoller.Roll(Math.Max(b.InsideRect.Width, (tablesareabottom - tablesareatop)), Math.Max(b.InsideRect.Width, (tablesareabottom - tablesareatop))); //@@MP - removed multiplier (Release 6-1)
                    break;
            }

            int tablesareawidth = Math.Abs(tablesarealeft - tablesarearight);
            int tablesareaheight = Math.Abs(tablesareabottom - tablesareatop);
            Rectangle insideRoom = new Rectangle(tablesarealeft, tablesareatop, tablesareawidth, tablesareaheight);
            #endregion

            ///////////
            // 4. Add beds, curtains, cupboards, 
            ///////////
            #region
            //cupboards
            bool placedGenerator = false; //@@MP - we only want to place one generator (Release 6-2)
            MapObjectFill(map, b.InsideRect,
                (pt) =>
                {
                    if (CountAdjWalls(map, pt.X, pt.Y) < 3)
                        return null;
                    else if (!placedGenerator) //@@MP (Release 6-2)
                    {
                        placedGenerator = true;
                        return MakeObjPowerGenerator(GameImages.OBJ_POWERGEN_OFF, GameImages.OBJ_POWERGEN_ON);
                    }
                    else if (m_DiceRoller.RollChance(40))
                    {
                        Item it = MakeShopPharmacyItem();
                        if ((it as ItemSprayScent) != null)
                            it = MakeItemLargeMedikit();
                        map.DropItemAt(it, pt);
                        return MakeObjShelf(GameImages.OBJ_CLINIC_CUPBOARD);
                    }
                    else
                        return null;
                });

            // beds, curtain, machinery
            nbTables = (int)Math.Round(nbTables * 0.80); //@@MP - reduced a tad (Release 7-3)
            for (int i = 0; i < nbTables; i++)
            {
                MapObjectPlaceInGoodPosition(map, insideRoom,
                    (pt) => !IsADoorNSEW(map, pt.X, pt.Y), // && CountAdjWalls(map, pt.X, pt.Y) == 0
                    m_DiceRoller,
                    (pt) =>
                    {
                        // curtain and machinery around, with item on the bed
                        Rectangle adjBedRect = new Rectangle(pt.X - 1, pt.Y - 1, 3, 3);
                        adjBedRect.Intersect(insideRoom);
                        MapObjectPlaceInGoodPosition(map, adjBedRect,
                            (pt2) => pt2 != pt && !IsADoorNSEW(map, pt2.X, pt2.Y),
                            m_DiceRoller,
                            (pt2) => MakeObjCurtain(GameImages.OBJ_CLINIC_CURTAIN));

                        MapObjectPlaceInGoodPosition(map, adjBedRect,
                            (pt2) => pt2 != pt && !IsADoorNSEW(map, pt2.X, pt2.Y),
                            m_DiceRoller,
                            (pt2) => MakeObjMachinery(GameImages.OBJ_CLINIC_MACHINERY));

                        // item.
                        Item it = MakeShopPharmacyItem();
                        if ((it as ItemSprayScent) != null)
                            it = MakeItemLargeMedikit();
                        map.DropItemAt(it, pt);

                        // bed.
                        MapObject bed = MakeObjBed(GameImages.OBJ_CLINIC_BED);
                        return bed;
                    });
            }
            #endregion

            ///////////
            // 5. Zone
            ///////////
            // demark building.
            string buildingName = "Clinic";
            map.AddZone(MakeUniqueZone(buildingName, b.BuildingRect));
            // walkway zones.
            MakeWalkwayZones(map, b);

            // Done
            return true;
        }

        protected virtual bool MakeJunkyard(Map map, Block b) //@@MP (Release 4)
        {
            ////////////////////////
            // 0. Check suitability
            ////////////////////////
            if (b.InsideRect.Width < 5 || b.InsideRect.Height < 5)
                return false;

            /////////////////////////////
            // 1. Walkway, floor & walls
            /////////////////////////////
            TileRectangle(map, m_Game.GameTiles.FLOOR_WALKWAY, b.Rectangle);
            TileFill(map, m_Game.GameTiles.FLOOR_DIRT, b.InsideRect); //@@MP - no longer marked as IsInside (Release 5-3)
            //base.TileFill(map, m_Game.GameTiles.FLOOR_DIRT, b.InsideRect, (tile, prevmodel, x, y) => tile.IsInside = true);
            MapObjectFill(map, b.BuildingRect,
                (pt) =>
                {
                    bool placeFence = (pt.X == b.BuildingRect.Left || pt.X == b.BuildingRect.Right - 1 || pt.Y == b.BuildingRect.Top || pt.Y == b.BuildingRect.Bottom - 1);
                    if (placeFence)
                    {
                        map.SetTileModelAt(pt.X, pt.Y, m_Game.GameTiles.FLOOR_DIRT);
                        return MakeObjFence(GameImages.OBJ_CHAINWIRE_FENCE); //@@MP - standard chain wire fence
                    }
                    else
                        return null;
                    });
            

            ///////////////////////////////
            // 2. Entry door with shop ids
            //    Add lectern and hangings.
            ///////////////////////////////
            #region
            int midX = b.Rectangle.Left + b.Rectangle.Width / 2;
            int midY = b.Rectangle.Top + b.Rectangle.Height / 2;

            // make doors on one side.
            int doorside = m_DiceRoller.Roll(0, 4);
            switch (doorside)
            {
                case 0:
                    // west
                    PlaceDoor(map, b.BuildingRect.Left, midY, m_Game.GameTiles.FLOOR_DIRT, MakeObjRollerDoor());
                    PlaceDoor(map, b.BuildingRect.Left, midY - 1, m_Game.GameTiles.FLOOR_DIRT, MakeObjRollerDoor());
                    PlaceDoor(map, b.BuildingRect.Left, midY + 1, m_Game.GameTiles.FLOOR_DIRT, MakeObjRollerDoor());

                    //place driveway
                    Point westdrivewaypt = new Point(b.BuildingRect.Left - 1, midY);
                    map.RemoveMapObjectAt(westdrivewaypt.X, westdrivewaypt.Y); //get rid of cars
                    map.SetTileModelAt(westdrivewaypt.X, westdrivewaypt.Y, m_Game.GameTiles.FLOOR_ASPHALT);

                    break;
                case 1:
                    // east
                    PlaceDoor(map, b.BuildingRect.Right - 1, midY, m_Game.GameTiles.FLOOR_DIRT, MakeObjRollerDoor());
                    PlaceDoor(map, b.BuildingRect.Right - 1, midY - 1, m_Game.GameTiles.FLOOR_DIRT, MakeObjRollerDoor());
                    PlaceDoor(map, b.BuildingRect.Right - 1, midY + 1, m_Game.GameTiles.FLOOR_DIRT, MakeObjRollerDoor());

                    //place driveway
                    Point eastdrivewaypt = new Point(b.BuildingRect.Right, midY);
                    map.RemoveMapObjectAt(eastdrivewaypt.X, eastdrivewaypt.Y); //get rid of cars
                    map.SetTileModelAt(eastdrivewaypt.X, eastdrivewaypt.Y, m_Game.GameTiles.FLOOR_ASPHALT);

                    break;
                case 2:
                    // north
                    PlaceDoor(map, midX, b.BuildingRect.Top, m_Game.GameTiles.FLOOR_DIRT, MakeObjRollerDoor());
                    PlaceDoor(map, midX - 1, b.BuildingRect.Top, m_Game.GameTiles.FLOOR_DIRT, MakeObjRollerDoor());
                    PlaceDoor(map, midX + 1, b.BuildingRect.Top, m_Game.GameTiles.FLOOR_DIRT, MakeObjRollerDoor());

                    //place driveway
                    Point northdrivewaypt = new Point(midX, b.BuildingRect.Top - 1);
                    map.RemoveMapObjectAt(northdrivewaypt.X, northdrivewaypt.Y); //get rid of cars
                    map.SetTileModelAt(northdrivewaypt.X, northdrivewaypt.Y, m_Game.GameTiles.FLOOR_ASPHALT);

                    break;
                case 3:
                    // south
                    PlaceDoor(map, midX, b.BuildingRect.Bottom - 1, m_Game.GameTiles.FLOOR_DIRT, MakeObjRollerDoor());
                    PlaceDoor(map, midX - 1, b.BuildingRect.Bottom - 1, m_Game.GameTiles.FLOOR_DIRT, MakeObjRollerDoor());
                    PlaceDoor(map, midX + 1, b.BuildingRect.Bottom - 1, m_Game.GameTiles.FLOOR_DIRT, MakeObjRollerDoor());

                    //place driveway
                    Point southdrivewaypt = new Point(midX, b.BuildingRect.Bottom);
                    map.RemoveMapObjectAt(southdrivewaypt.X, southdrivewaypt.Y); //get rid of cars
                    map.SetTileModelAt(southdrivewaypt.X, southdrivewaypt.Y, m_Game.GameTiles.FLOOR_ASPHALT);

                    break;
            }

            // add building image next to doors.
            DecorateOutsideWalls(map, b.BuildingRect, (x, y) => map.GetMapObjectAt(x, y) == null && CountAdjDoors(map, x, y) >= 1 ? GameImages.DECO_JUNKYARD : null);
            #endregion

            ///////////
            // 3. Add junk
            ///////////
            #region
            MapObjectFill(map, b.InsideRect,
                            (pt) =>
                            {
                                if (m_DiceRoller.RollChance(60))
                                {
                                    Tile tile = map.GetTileAt(pt.X, pt.Y);
                                    if (tile.Model.IsWalkable)// && tile.Model != m_Game.GameTiles.WALL_STONE) //@@MP - removed (tile.IsInside && ...) (Release 5-3)
                                    {
                                        MapObject thing;
                                        int rolled = m_DiceRoller.Roll(0, 99);
                                        if (rolled >= 80)  //20%
                                            thing = MakeObjWreckedCar(m_DiceRoller);
                                        else if (rolled >= 40 && rolled < 79)  //40%
                                        {
                                            thing = MakeObjJunk(GameImages.OBJ_JUNK);
                                            int itemRoller = m_DiceRoller.Roll(0, 99); //@@MP (Release 7-6)
                                            if (itemRoller <= 60)
                                                map.DropItemAt(MakeJunkyardItem(), pt);
                                        }
                                        else  //40%
                                        {
                                            if (m_DiceRoller.RollChance(60)) //@@MP (Release 7-6)
                                                thing = MakeObjBarrels(GameImages.OBJ_BARRELS);
                                            else
                                                thing = MakeObjFireBarrel(GameImages.OBJ_EMPTY_BARREL);

                                            int itemRoller = m_DiceRoller.Roll(0, 99); //@@MP (Release 7-6)
                                            if (itemRoller <= 60)
                                                map.DropItemAt(MakeJunkyardItem(), pt);
                                        }

                                        return thing;
                                    }
                                }
                                return null;
                            });
            #endregion

            ///////////
            // 4. Zone
            ///////////
            // demark building.
            map.AddZone(MakeUniqueZone("Junkyard", b.BuildingRect));
            // walkway zones.
            MakeWalkwayZones(map, b);

            // Done
            return true;
        }

        protected virtual bool MakeFarmBuilding(Map map, Block b) //@@MP (Release 7-3)
        {
            ////////////////////////
            // 0. Check suitability
            ////////////////////////
            if (b.InsideRect.Width < 8 || b.InsideRect.Height < 6)
                return false;
            if (b.InsideRect.Width < 6 || b.InsideRect.Height < 8)
                return false;

            /////////////////////////////
            // 1. Grass, walkway & fence
            /////////////////////////////
            TileRectangle(map, m_Game.GameTiles.FLOOR_WALKWAY, b.Rectangle);
            TileFill(map, m_Game.GameTiles.FLOOR_GRASS, b.InsideRect);
            MapObjectFill(map, b.BuildingRect,
                (pt) =>
                {
                    if (pt.X == b.BuildingRect.Left || pt.X == b.BuildingRect.Right - 1 || pt.Y == b.BuildingRect.Top || pt.Y == b.BuildingRect.Bottom - 1)
                    {
                        // place a fence, choosing the appropriate oritentation
                        foreach (Direction dir in Direction.COMPASS_NSEW)
                        {
                            Point next = pt + dir;
                            if (map.IsInBounds(next) && map.GetTileAt(next).Model == m_Game.GameTiles.FLOOR_WALKWAY)
                            {
                                map.SetTileModelAt(pt.X, pt.Y, m_Game.GameTiles.FLOOR_DIRT);
                                if (dir == Direction.N || dir == Direction.S) //if has footpath south or north then use EW
                                    return MakeObjWoodenFence(GameImages.OBJ_FARM_FENCE_EW);
                                else if (dir == Direction.E) //else if has footpath east then use NS_right
                                    return MakeObjWoodenFence(GameImages.OBJ_FARM_FENCE_NS_RIGHT);
                                else if (dir == Direction.W) //else if has footpath west then use NS_left
                                    return MakeObjWoodenFence(GameImages.OBJ_FARM_FENCE_NS_LEFT);
                            }
                        }
                        return null;
                    }
                    else
                        return null;
                });

            ///////////////////////////////
            // 2. Random bushes / crops
            ///////////////////////////////
            int plantType = m_DiceRoller.Roll(0, 3); //berries (1), peanuts (2) or crops (3)
            MapObjectFill(map, b.InsideRect,
                (pt) =>
                {
                    //crops
                    switch (plantType)
                    {
                            case 0: return MakeObjFarmPlant("berry bush", GameImages.OBJ_BERRY_BUSH);
                            case 1: return MakeObjFarmPlant("peanut plant", GameImages.OBJ_PEANUT_PLANT);
                            case 2: return MakeObjFarmPlant("grape vine", GameImages.OBJ_GRAPE_VINE);
                            default: throw new InvalidOperationException("roll for plant type outside of range");
                    }
                    
                });

            ///////////
            // 3. Zone
            ///////////
            Zone farmZone = MakeUniqueZone("Farm", b.BuildingRect);
            map.AddZone(farmZone);
            MakeWalkwayZones(map, b);

            ////////////
            // 4. Shed & entrance
            ////////////
            int SHED_WIDTH = 5;
            int SHED_HEIGHT = 5;
            if (b.InsideRect.Width > SHED_WIDTH + 1 && b.InsideRect.Height > SHED_HEIGHT + 1)
            {
                // roll shed pos
                int shedX = m_DiceRoller.Roll(b.InsideRect.Left, b.InsideRect.Right - SHED_WIDTH);
                int shedY = m_DiceRoller.Roll(b.InsideRect.Top, b.InsideRect.Bottom - SHED_HEIGHT);
                Rectangle shedRect = new Rectangle(shedX, shedY, SHED_WIDTH, SHED_HEIGHT);
                Rectangle shedInsideRect = new Rectangle(shedX + 1, shedY + 1, SHED_WIDTH - 2, SHED_HEIGHT - 2);

                // clear everything but zones in shed location
                ClearRectangle(map, shedRect, false);

                // build it
                MakeFarmShedBuilding(map, "Shed", shedRect, b);
            }

            ///////////
            /// 5. Populate with chickens     //@@MP (Release 7-6)
            /// note: this was disabled as I could never figure out why, even though the eggs were all spawned in the farm, and that animal AI prohibits them from entering
            /// a non-grass/dirt/planted tile, the there would always be at least two chickens that would end up elsewhere on the map, including one usually ending up
            /// *inside* the locked army base...?! I suspect it's due to the fact that the district is yet to be finalised, even though the placement of zones is set
            /// and again, the eggs spawn exactly where they should, indicating that the chickens indeed start where they should at least...
            ///////////
#if false
            int chickens = map.CountActorsBasedOn((a) => a.Faction == m_Game.GameFactions.TheUnintelligentAnimals);
            int chickensToSpawn = Rules.MAX_UNINTELLIGENT_ANIMALS_PER_DISTRICT - chickens; //just top up to the max
            DoForEachTile(b.InsideRect, (pt) =>
            {
                if (chickensToSpawn > 0)
                {
                 /*   for (int i = 0; i < chickensToSpawn; i++)
                    {*/
                        // chickens only spawn at farms.
                        Actor chicken = CreateNewChicken(0);
                        bool spawnedChicken = ActorPlace(m_DiceRoller, 25, map, chicken, (chickenPoint) => map.IsWalkable(pt.X, pt.Y) && map.GetActorAt(pt) == null);
                        if (spawnedChicken)
                        {
                            --chickensToSpawn;
                            map.DropItemAt(MakeItemChickenEgg(), pt);
                            map.DropItemAt(MakeItemChickenEgg(), pt);
                            Tile tile = map.GetTileAt(pt);
                            Logger.WriteLine(Logger.Stage.RUN_MAIN, String.Format("spawned {0}: at {1} on {2}", chicken.Name, chicken.Location.Position.ToString(), tile.Model.ImageID));
                        }
                    //}
                }
            });
#endif

            // Done.
            return true;
        }

        protected virtual void MakeFarmShedBuilding(Map map, string baseZoneName, Rectangle shedBuildingRect, Block block) //@@MP (Release 7-3)
        {
            Rectangle shedInsideRect = new Rectangle(shedBuildingRect.X + 1, shedBuildingRect.Y + 1, shedBuildingRect.Width - 2, shedBuildingRect.Height - 2);

            // build building & zone
            TileRectangle(map, m_Game.GameTiles.WALL_WOOD_PLANKS, shedBuildingRect);
            TileFill(map, m_Game.GameTiles.FLOOR_DIRT, shedInsideRect, (tile, prevTileModel, x, y) => tile.IsInside = true);
            map.AddZone(MakeUniqueZone(baseZoneName, shedBuildingRect));

            /////
            // place shed door and make sure door front is cleared of objects (trees).
            /////
            int doorX = 0, doorY = 0;
            int doorFrontX = 0, doorFrontY = 0;
            int doorDir = 0;
            bool placed = false;
            do
            {
                doorDir = m_DiceRoller.Roll(0, 4);
                switch (doorDir)
                {
                    case 0: // west
                        doorX = shedBuildingRect.Left;
                        doorY = shedBuildingRect.Top + shedBuildingRect.Height / 2;
                        doorFrontX = doorX - 1;
                        doorFrontY = doorY;
                        break;
                    case 1: // east
                        doorX = shedBuildingRect.Right - 1;
                        doorY = shedBuildingRect.Top + shedBuildingRect.Height / 2;
                        doorFrontX = doorX + 1;
                        doorFrontY = doorY;
                        break;
                    case 2: // north
                        doorX = shedBuildingRect.Left + shedBuildingRect.Width / 2;
                        doorY = shedBuildingRect.Top;
                        doorFrontX = doorX;
                        doorFrontY = doorY - 1;
                        break;
                    case 3: // south
                        doorX = shedBuildingRect.Left + shedBuildingRect.Width / 2;
                        doorY = shedBuildingRect.Bottom - 1;
                        doorFrontX = doorX;
                        doorFrontY = doorY + 1;
                        break;
                }
                //make sure the door is not against the fence
                MapObject mapObj = map.GetMapObjectAt(doorFrontX, doorFrontY);
                if (mapObj != null)
                {
                    if (mapObj.ImageID == GameImages.OBJ_BERRY_BUSH || mapObj.ImageID == GameImages.OBJ_PEANUT_PLANT || mapObj.ImageID == GameImages.OBJ_GRAPE_VINE)
                        map.RemoveMapObjectAt(doorFrontX, doorFrontY); //get rid of the plant
                    else //it's a fence, which we dont want a door against, so roll again
                        continue;
                }
                map.SetTileModelAt(doorFrontX, doorFrontY, m_Game.GameTiles.FLOOR_DIRT); //start the driveway
                map.PlaceMapObjectAt(MakeObjTractor(GameImages.OBJ_TRACTOR), new Point(doorFrontX, doorFrontY));
                placed = true;

            } while (!placed);
            PlaceDoor(map, doorX, doorY, m_Game.GameTiles.FLOOR_DIRT, MakeObjRollerDoor());

            ///////////////
            // driveway
            ///////////////
            int ex = doorFrontX, ey = doorFrontY;
            do
            {
                switch (doorDir)
                {
                    case 0: ex -= 1; break; // west
                    case 1: ex += 1; break; // east
                    case 2: ey -= 1; break; // north
                    case 3: ey += 1; break; // south
                    default: throw new InvalidOperationException("roll for driveway direction outside of range");
                }

                TileModel tileModel = map.GetTileAt(ex, ey).Model;
                if (tileModel == m_Game.GameTiles.FLOOR_WALKWAY) //keep paving the driveway until we hit the walkway
                    break;
                else if (tileModel != m_Game.GameTiles.FLOOR_DIRT)
                {
                    map.RemoveMapObjectAt(ex, ey); //get rid of plants
                    map.SetTileModelAt(ex, ey, m_Game.GameTiles.FLOOR_DIRT);
                }

            } while (1 < 2);

            map.RemoveMapObjectAt(ex, ey);
            map.SetTileModelAt(ex, ey, m_Game.GameTiles.FLOOR_ASPHALT);

            /////////
            // mark as inside and add workbench with tools
            /////////
            bool alreadyPlacedGenertor = false;
            DoForEachTile(shedInsideRect, (pt) =>
            {
                if (!map.IsWalkable(pt))
                    return;

                if (CountAdjDoors(map, pt.X, pt.Y) > 0)
                    return;

                if (CountAdjWalls(map, pt.X, pt.Y) == 0)
                    return;

                // generator                   //@@MP - moved out of switch in order to make it guaranteed spawn (Release 7-6)
                if (!alreadyPlacedGenertor)
                {
                    alreadyPlacedGenertor = true;
                    map.PlaceMapObjectAt(MakeObjPowerGenerator(GameImages.OBJ_POWERGEN_OFF, GameImages.OBJ_POWERGEN_ON), pt);
                }
                else
                {
                    // objects
                    int objectRoll = m_DiceRoller.Roll(0, 3);
                    switch (objectRoll)
                    {
                        case 0: map.PlaceMapObjectAt(MakeObjWorkbench(GameImages.OBJ_WORKBENCH), pt); break;
                        case 1: map.PlaceMapObjectAt(MakeObjJunk(GameImages.OBJ_JUNK), pt); break;
                        case 2:
                            if (m_DiceRoller.RollChance(50))
                                map.PlaceMapObjectAt(MakeObjBarrels(GameImages.OBJ_BARRELS), pt);
                            else
                                map.PlaceMapObjectAt(MakeObjFireBarrel(GameImages.OBJ_EMPTY_BARREL), pt); //@@MP (Release 7-6)
                            break;
                        default: break; //nothing
                    }
                }

                // construction item (tools, lights)
                Item it = MakeFarmShedItem();  //@@MP (Release 7-6)
                if (it.Model.IsStackable)
                    it.Quantity = it.Model.StackingLimit;
                map.DropItemAt(it, pt);
            });
        }

        protected virtual bool MakeAnimalShelterBuilding(Map map, Block b) //@@MP (Release 7-3)
        {
            ////////////////////////
            // 0. Check suitability
            ////////////////////////
            if (b.InsideRect.Width < 6 || b.InsideRect.Height < 6)
                return false;

            /////////////////////////////
            // 1. Grass, trees, walkway & fence
            /////////////////////////////
            TileRectangle(map, m_Game.GameTiles.FLOOR_WALKWAY, b.Rectangle);
            TileFill(map, m_Game.GameTiles.FLOOR_GRASS, b.BuildingRect);
            MapObjectFill(map, b.BuildingRect,
                (pt) =>
                {
                    if (pt.X == b.BuildingRect.Left || pt.X == b.BuildingRect.Right - 1 || pt.Y == b.BuildingRect.Top || pt.Y == b.BuildingRect.Bottom - 1)
                        return MakeObjFence(GameImages.OBJ_CHAINWIRE_FENCE);
                    else
                        return null;
                });
            MapObjectFill(map, b.InsideRect,
                (pt) =>
                {

                    if (m_DiceRoller.RollChance(PARK_TREE_CHANCE))
                        return MakeObjTree(PARK_TREES[m_DiceRoller.Roll(0, PARK_TREES.Length)]);
                    else
                        return null;
                });

            ////////////
            // 2. Office & entrance
            ////////////
            Point surfaceStairsPos;
            int OFFICE_WIDTH = 5, OFFICE_HEIGHT = 5;
            // roll office pos
            int officeX = m_DiceRoller.Roll(b.InsideRect.Left, b.InsideRect.Right - OFFICE_WIDTH);
            int officeY = m_DiceRoller.Roll(b.InsideRect.Top, b.InsideRect.Bottom - OFFICE_HEIGHT);
            Rectangle officeRect = new Rectangle(officeX, officeY, OFFICE_WIDTH, OFFICE_HEIGHT);
            Rectangle officeInsideRect = new Rectangle(officeX + 1, officeY + 1, OFFICE_WIDTH - 2, OFFICE_HEIGHT - 2);

            // clear everything but zones in office location
            ClearRectangle(map, officeRect, false);

            // build it
            MakeAnimalShelterOfficeBuilding(map, "Office", officeRect, b, out surfaceStairsPos);

            ///////////
            // 3. Generate Kennels level (-1)
            ///////////
            Map kennelsLevel = GenerateAnimalShelter_KennelsLevel(map);

            // Link maps.
            // surface <-> kennels level
            AddExit(map, surfaceStairsPos, kennelsLevel, new Point(2, 1), GameImages.DECO_STAIRS_DOWN, true);
            AddExit(kennelsLevel, new Point(2, 1), map, surfaceStairsPos, GameImages.DECO_STAIRS_UP, true);

            // Add maps to district.
            m_Params.District.AddUniqueMap(kennelsLevel);

            ///////////////
            // 4. Entrance
            ///////////////
            int entranceFace = m_DiceRoller.Roll(0, 4);
            int ex, ey;
            switch (entranceFace)
            {
                case 0: // west
                    ex = b.BuildingRect.Left;
                    ey = b.BuildingRect.Top + b.BuildingRect.Height / 2;
                    break;
                case 1: // east
                    ex = b.BuildingRect.Right - 1;
                    ey = b.BuildingRect.Top + b.BuildingRect.Height / 2;
                    break;
                case 3: // north
                    ex = b.BuildingRect.Left + b.BuildingRect.Width / 2;
                    ey = b.BuildingRect.Top;
                    break;
                default: // south
                    ex = b.BuildingRect.Left + b.BuildingRect.Width / 2;
                    ey = b.BuildingRect.Bottom - 1;
                    break;
            }
            map.RemoveMapObjectAt(ex, ey);
            map.PlaceMapObjectAt(MakeObjChainFenceGate(DoorWindow.STATE_CLOSED), new Point(ex, ey));

            ///////////
            // 5. Zone
            ///////////
            Zone parkZone = MakeUniqueZone("Animal shelter", b.BuildingRect);
            map.AddZone(parkZone);
            MakeWalkwayZones(map, b);

            // Done.
            return true;
        }

        protected virtual void MakeAnimalShelterOfficeBuilding(Map map, string baseZoneName, Rectangle officeBuildingRect, Block block, out Point stairsToSurface) //@@MP (Release 7-3)
        {
            Rectangle officeInsideRect = new Rectangle(officeBuildingRect.X + 1, officeBuildingRect.Y + 1, officeBuildingRect.Width - 2, officeBuildingRect.Height - 2);

            // build building & zone
            TileRectangle(map, m_Game.GameTiles.WALL_BRICK, officeBuildingRect);
            TileFill(map, m_Game.GameTiles.FLOOR_OFFICE, officeInsideRect, (tile, prevTileModel, x, y) => tile.IsInside = true);
            map.AddZone(MakeUniqueZone(baseZoneName, officeBuildingRect));

            /////
            // place office door and make sure door front is cleared of objects (trees).
            /////
            int doorX = 0, doorY = 0;
            int doorFrontX = 0, doorFrontY = 0;
            int doorDir = 0;
            bool placed = false;
            do
            {
                doorDir = m_DiceRoller.Roll(0, 4);
                switch (doorDir)
                {
                    case 0: // west
                        doorX = officeBuildingRect.Left;
                        doorY = officeBuildingRect.Top + officeBuildingRect.Height / 2;
                        doorFrontX = doorX - 1;
                        doorFrontY = doorY;
                        break;
                    case 1: // east
                        doorX = officeBuildingRect.Right - 1;
                        doorY = officeBuildingRect.Top + officeBuildingRect.Height / 2;
                        doorFrontX = doorX + 1;
                        doorFrontY = doorY;
                        break;
                    case 2: // north
                        doorX = officeBuildingRect.Left + officeBuildingRect.Width / 2;
                        doorY = officeBuildingRect.Top;
                        doorFrontX = doorX;
                        doorFrontY = doorY - 1;
                        break;
                    case 3: // south
                        doorX = officeBuildingRect.Left + officeBuildingRect.Width / 2;
                        doorY = officeBuildingRect.Bottom - 1;
                        doorFrontX = doorX;
                        doorFrontY = doorY + 1;
                        break;
                }
                //make sure the door is not against the fence
                MapObject mapObj = map.GetMapObjectAt(doorFrontX, doorFrontY);
                if (mapObj != null)
                {
                    if (mapObj.ImageID == GameImages.OBJ_CHAINWIRE_FENCE)
                        continue; //it's a fence, which we dont want a door against, so roll again
                }
                map.SetTileModelAt(doorFrontX, doorFrontY, m_Game.GameTiles.FLOOR_ASPHALT); //start the driveway
                map.RemoveMapObjectAt(doorFrontX, doorFrontY); //get rid of tree if there
                map.PlaceMapObjectAt(MakeObjVan(GameImages.OBJ_VAN_PHASE0, m_DiceRoller.Roll(0, 30)), new Point(doorFrontX, doorFrontY));
                placed = true;

            } while (!placed);
            PlaceDoor(map, doorX, doorY, m_Game.GameTiles.FLOOR_OFFICE, MakeObjWoodenDoor());
            // add building image next to doors.
            DecorateOutsideWalls(map, officeBuildingRect, (x, y) => map.GetMapObjectAt(x, y) == null && CountAdjDoors(map, x, y) >= 1 ? GameImages.DECO_ANIMAL_SHELTER : null);

            ///////////////
            // driveway
            ///////////////
            int ex = doorFrontX, ey = doorFrontY;
            do
            {
                int prevex = ex, prevey = ey;
                switch (doorDir)
                {
                    case 0: ex -= 1; break; // west
                    case 1: ex += 1; break; // east
                    case 2: ey -= 1; break; // north
                    case 3: ey += 1; break; // south
                    default: throw new InvalidOperationException("roll for driveway direction outside of range");
                }

                if (map.GetTileAt(ex, ey).Model == m_Game.GameTiles.FLOOR_WALKWAY) //keep paving the driveway until we hit the walkway
                {
                    map.PlaceMapObjectAt(MakeObjChainFenceGate(DoorWindow.STATE_OPEN), new Point(prevex, prevey));
                    break;
                }
                else
                {
                    map.RemoveMapObjectAt(ex, ey); //get rid of plants
                    map.SetTileModelAt(ex, ey, m_Game.GameTiles.FLOOR_ASPHALT);
                }

            } while (1 < 2);

            map.RemoveMapObjectAt(ex, ey);
            map.SetTileModelAt(ex, ey, m_Game.GameTiles.FLOOR_ASPHALT);

            /////////
            // mark as inside and add chairs, shelves and tables
            /////////
            stairsToSurface = new Point(officeInsideRect.Left, officeInsideRect.Top);
            Point stairsPt = stairsToSurface;
            bool chairPlaced = false, tablePlaced = false;
            DoForEachTile(officeInsideRect, (pt) =>
            {
                if (pt == stairsPt)
                    return;

                if (!map.IsWalkable(pt))
                    return;

                if (CountAdjDoors(map, pt.X, pt.Y) > 0)
                    return;

                if (CountAdjWalls(map, pt.X, pt.Y) == 0)
                    return;

                if (!tablePlaced) //just one
                {
                    map.PlaceMapObjectAt(MakeObjTable(GameImages.OBJ_TABLE), pt);
                    tablePlaced = true;
                    return;
                }
                else if (!chairPlaced) //just one
                {
                    map.PlaceMapObjectAt(MakeObjChair(GameImages.OBJ_CHAIR), pt);
                    chairPlaced = true;
                    return;
                }

                // objects
                if (m_DiceRoller.RollChance(50))
                {
                    map.PlaceMapObjectAt(MakeObjShelf(GameImages.OBJ_SHOP_SHELF), pt);
                    // item
                    Item it = null;
                    if (m_DiceRoller.RollChance(50))
                        it = MakeItemBigFlashlight();
                    else
                        it = MakeItemBinoculars();

                    if (it != null)
                        map.DropItemAt(it, pt);
                }
            });
        }

        Map GenerateAnimalShelter_KennelsLevel(Map surfaceMap)
        {
            //////////////////
            // 1. Create map.
            // 2. Floor plan & populate.
            //////////////////

            // 1. Create map.
            int seed = (surfaceMap.Seed << 1) ^ surfaceMap.Seed;
            Map map = new Map(seed, "Animal shelter", 21, 8) //@@MP - expanded the height by 2 (Release 7-6)
            {
                Lighting = Lighting.DARKNESS
            };
            DoForEachTile(map.Rect, (pt) => map.GetTileAt(pt).IsInside = true);

            // 2. Floor plan.
            TileFill(map, m_Game.GameTiles.FLOOR_TILES);
            TileRectangle(map, m_Game.GameTiles.WALL_HOSPITAL, map.Rect);
            // - small cells.
            const int cellWidth = 3;
            const int cellHeight = 3;
            const int yCells = 5;
            List<Rectangle> cells = new List<Rectangle>();
            for (int x = 0; x + cellWidth <= map.Width; x += cellWidth - 1)
            {
                // room.
                Rectangle cellRoom = new Rectangle(x, yCells, cellWidth, cellHeight);
                cells.Add(cellRoom);
                TileFill(map, m_Game.GameTiles.FLOOR_CONCRETE, cellRoom);
                MapObjectFill(map, cellRoom,
                    (pt) =>
                    {
                        if (pt.X == cellRoom.Left || pt.X == cellRoom.Right - 1 || pt.Y == cellRoom.Top || pt.Y == cellRoom.Bottom - 1)
                            return MakeObjKennelFence(GameImages.OBJ_CHAINWIRE_FENCE);
                        else
                            return null;
                    });

                // deco and dog.
                Point kennelPos = new Point(x + 1, yCells + 1);
                map.GetTileAt(kennelPos).AddDecoration(GameImages.DECO_KENNEL);
                Actor dog = CreateNewFeralDog(0);
                map.DropItemAt(MakeItemCookedChicken(), kennelPos);  // give him some food.
                map.DropItemAt(MakeItemCookedChicken(), kennelPos);
                map.DropItemAt(MakeItemCookedChicken(), kennelPos);
                map.PlaceActorAt(dog, kennelPos);

                // gate.
                Point gatePos = new Point(x + 1, yCells);
                map.SetTileModelAt(gatePos.X, gatePos.Y, m_Game.GameTiles.FLOOR_CONCRETE);
                map.RemoveMapObjectAt(gatePos.X, gatePos.Y); //remove fence to make space for the gate
                map.PlaceMapObjectAt(MakeObjChainFenceGate(DoorWindow.STATE_CLOSED), gatePos);

                // zone.
                map.AddZone(MakeUniqueZone("Kennels", cellRoom));
            }
            // - corridor.
            Rectangle corridor = Rectangle.FromLTRB(1, 1, map.Width, yCells);
            map.AddZone(MakeUniqueZone("cages corridor", corridor));

            // done.
            return map;
        }

        protected virtual bool MakeBankBuilding(Map map, Block b, ref int banksCount) //@@MP (Release 4), capped banks per map (Release 7-3)
        {
            ////////////////////////
            // 0. Check suitability
            ////////////////////////
            if (b.InsideRect.Width < 5 || b.InsideRect.Height < 5)
                return false;
            double banksLimit = Math.Round(((double)(map.Width / 10)) / 2.5); //cap the number per map based on its dimensions
            if ((double)banksCount >= banksLimit)
                return false;
            else
                ++banksCount;

            /////////////////////////////
            // 1. Walkway, floor & walls
            /////////////////////////////
            TileRectangle(map, m_Game.GameTiles.FLOOR_WALKWAY, b.Rectangle);
            TileRectangle(map, m_Game.GameTiles.WALL_LIGHT_BROWN, b.BuildingRect);
            TileFill(map, m_Game.GameTiles.FLOOR_BLUE_CARPET, b.InsideRect, (tile, prevmodel, x, y) => tile.IsInside = true);

            ///////////////////////////////
            // 2. Entry door with shop ids
            ///////////////////////////////
#region
            int midX = b.Rectangle.Left + b.Rectangle.Width / 2;
            int midY = b.Rectangle.Top + b.Rectangle.Height / 2;

            //hold the area within the bank. after the door position is determined we'll reduce the area held to only in front of the counter.
            //we'll use this held area to place tables later
            int customerareabottom = b.InsideRect.Bottom;
            int customerareatop = b.InsideRect.Top;
            int customerarealeft = b.InsideRect.Left;
            int customerarearight = b.InsideRect.Right;

            // make doors on one side.
            int doorside = m_DiceRoller.Roll(0, 4);
            switch (doorside)
            {
                case 0:
                    // west
                    PlaceDoor(map, b.BuildingRect.Left, midY, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());

                    //create the back room, safes and counters
                    customerarearight -= 6;
                    for (int i = 1; i <= b.InsideRect.Height; i++)
                    {
                        int y = b.InsideRect.Bottom - i;

                        //place safes along the wall
                        Point safept = new Point(b.InsideRect.Right - 1, y);
                        if (map.IsWalkable(safept))
                            PlaceBankSafe(map, safept); //@@MP (Release 6-5)

                        //place a wall another 2 tiles in from the safes
                        Point wallpt = new Point(b.InsideRect.Right - 3, y);
                        if (map.IsWalkable(wallpt) && wallpt.Y != midY)
                            map.SetTileModelAt(wallpt.X, y, m_Game.GameTiles.WALL_HOSPITAL);
                        else if (wallpt.Y == midY)
                        {
                            //center the secure door opposite the main entrance
                            Point vaultpt = new Point(wallpt.X, midY);
                            PlaceBankVaultDoor(map, vaultpt);
                        }

                        Point counterpt = new Point(b.InsideRect.Right - 5, y);
                        //place the counters, with a openable door opposite the main entrance
                        if (map.IsWalkable(counterpt) && counterpt.Y != midY)
                            map.PlaceMapObjectAt(MakeObjBankTeller(GameImages.OBJ_BANK_TELLER), counterpt);
                        else if (counterpt.Y == midY)
                            PlaceDoor(map, counterpt.X, counterpt.Y, m_Game.GameTiles.FLOOR_BLUE_CARPET, MakeObjWoodenDoor());
                    }
                    break;
                case 1:
                    // east
                    PlaceDoor(map, b.BuildingRect.Right - 1, midY, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());

                    //create the back room, safes and counters
                    customerarealeft += 6;
                    for (int i = 1; i <= b.InsideRect.Height; i++)
                    {
                        int y = b.InsideRect.Bottom - i;

                        //place safes along the wall
                        Point safept = new Point(b.InsideRect.Left, y);
                        if (map.IsWalkable(safept))
                            PlaceBankSafe(map, safept); //@@MP (Release 6-5)

                        //place a wall another 2 tiles in from the safes
                        Point wallpt = new Point(b.InsideRect.Left + 2, y);
                        if (map.IsWalkable(wallpt) && wallpt.Y != midY)
                            map.SetTileModelAt(wallpt.X, y, m_Game.GameTiles.WALL_HOSPITAL);
                        else if (wallpt.Y == midY)
                        {
                            //center the secure door opposite the main entrance
                            Point vaultpt = new Point(wallpt.X, midY);
                            PlaceBankVaultDoor(map, vaultpt);
                        }

                        Point counterpt = new Point(b.InsideRect.Left + 4, y);
                        //place the counters, with a openable door opposite the main entrance
                        if (map.IsWalkable(counterpt) && counterpt.Y != midY)
                            map.PlaceMapObjectAt(MakeObjBankTeller(GameImages.OBJ_BANK_TELLER), counterpt);
                        else if (counterpt.Y == midY)
                            PlaceDoor(map, counterpt.X, counterpt.Y, m_Game.GameTiles.FLOOR_BLUE_CARPET, MakeObjWoodenDoor());
                    }
                    break;
                case 2:
                    // north
                    PlaceDoor(map, midX, b.BuildingRect.Top, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());

                    //create the back room, safes and counters
                    customerareabottom -= 6;
                    for (int i = 0; i <= b.InsideRect.Width; i++)
                    {
                        int x = b.InsideRect.Left + i;

                        //place safes along the wall
                        Point safept = new Point(x, b.InsideRect.Bottom - 1);
                        if (map.IsWalkable(safept))
                            PlaceBankSafe(map, safept); //@@MP (Release 6-5)

                        //place a wall another 2 tiles in from the safes
                        Point wallpt = new Point(x, b.InsideRect.Bottom - 3);
                        if (map.IsWalkable(wallpt) && wallpt.X != midX)
                            map.SetTileModelAt(x, wallpt.Y, m_Game.GameTiles.WALL_HOSPITAL);
                        else if (wallpt.X == midX)
                        {
                            //center the secure door opposite the main entrance
                            Point vaultpt = new Point(midX, wallpt.Y);
                            PlaceBankVaultDoor(map, vaultpt);
                        }

                        Point counterpt = new Point(x, b.InsideRect.Bottom - 5);
                        //place the counters, with a openable door opposite the main entrance
                        if (map.IsWalkable(counterpt) && counterpt.X != midX)
                            map.PlaceMapObjectAt(MakeObjBankTeller(GameImages.OBJ_BANK_TELLER), counterpt);
                        else if (counterpt.X == midX)
                            PlaceDoor(map, counterpt.X, counterpt.Y, m_Game.GameTiles.FLOOR_BLUE_CARPET, MakeObjWoodenDoor());
                    }
                    break;
                case 3:
                    // south
                    PlaceDoor(map, midX, b.BuildingRect.Bottom - 1, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());

                    //create the back room, safes and counters
                    customerareatop += 6;
                    for (int i = 0; i <= b.InsideRect.Width; i++)
                    {
                        int x = b.InsideRect.Left + i;

                        //place safes along the wall
                        Point safept = new Point(x, b.InsideRect.Top);
                        if (map.IsWalkable(safept))
                            PlaceBankSafe(map, safept); //@@MP (Release 6-5)

                        //place a wall another 2 tiles in from the safes
                        Point wallpt = new Point(x, b.InsideRect.Top + 2);
                        if (map.IsWalkable(wallpt) && wallpt.X != midX)
                            map.SetTileModelAt(x, wallpt.Y, m_Game.GameTiles.WALL_HOSPITAL);
                        else if (wallpt.X == midX)
                        {
                            //center the secure door opposite the main entrance
                            Point vaultpt = new Point(midX, wallpt.Y);
                            PlaceBankVaultDoor(map, vaultpt);
                        }

                        Point counterpt = new Point(x, b.InsideRect.Top + 4);
                        //place the counters, with a openable door opposite the main entrance
                        if (map.IsWalkable(counterpt) && counterpt.X != midX)
                            map.PlaceMapObjectAt(MakeObjBankTeller(GameImages.OBJ_BANK_TELLER), counterpt);
                        else if (counterpt.X == midX)
                            PlaceDoor(map, counterpt.X, counterpt.Y, m_Game.GameTiles.FLOOR_BLUE_CARPET, MakeObjWoodenDoor());
                    }
                    break;
            }

            // add building image next to doors.
            DecorateOutsideWalls(map, b.BuildingRect, (x, y) => map.GetMapObjectAt(x, y) == null && CountAdjDoors(map, x, y) >= 1 ? GameImages.DECO_BANK_SIGN : null);
#endregion

            ///////////
            // 3. Add tables and chairs
            ///////////
#region
            int nbTables = 0;
            switch (doorside)
            {
                case 0:
                case 1:
                    nbTables = m_DiceRoller.Roll(Math.Max((customerarealeft - customerarearight), b.InsideRect.Height / 6), Math.Max((customerarealeft - customerarearight), b.InsideRect.Height / 6));
                    break;
                case 2:
                case 3:
                    nbTables = m_DiceRoller.Roll(Math.Max(b.InsideRect.Width, (customerareabottom - customerareatop) / 6), Math.Max(b.InsideRect.Width, (customerareabottom - customerareatop) / 6));
                    break;
            }

            int tablesareawidth = Math.Abs(customerarealeft - customerarearight);
            int tablesareaheight = Math.Abs(customerareabottom - customerareatop);
            Rectangle insideRoom = new Rectangle(customerarealeft, customerareatop, tablesareawidth, tablesareaheight);
            for (int i = 0; i < nbTables; i++)
            {
                MapObjectPlaceInGoodPosition(map, insideRoom,
                    (pt) => !IsADoorNSEW(map, pt.X, pt.Y),
                    m_DiceRoller,
                    (pt) =>
                    {
                        // two chairs around //@@MP (Release 3)
                        Rectangle adjTableRect = new Rectangle(pt.X - 1, pt.Y - 1, 3, 3);
                        adjTableRect.Intersect(insideRoom);
                        MapObjectPlaceInGoodPosition(map, adjTableRect,
                            (pt2) => pt2 != pt && !IsADoorNSEW(map, pt2.X, pt2.Y),
                            m_DiceRoller,
                            (pt2) => MakeObjCouch(GameImages.OBJ_COUCH));
                        MapObjectPlaceInGoodPosition(map, adjTableRect,
                            (pt2) => pt2 != pt && !IsADoorNSEW(map, pt2.X, pt2.Y),
                            m_DiceRoller,
                            (pt2) => MakeObjCouch(GameImages.OBJ_COUCH));

                        // table.
                        MapObject table = MakeObjTable(GameImages.OBJ_TABLE);
                        return table;
                    });
            };
#endregion

            ///////////
            // 4. Zone
            ///////////
            // demark building.
            string buildingName = "Bank";
            map.AddZone(MakeUniqueZone(buildingName, b.BuildingRect));
            // walkway zones.
            MakeWalkwayZones(map, b);

            // Done
            return true;
        }

        private void PlaceBankSafe(Map map, Point safept) //@@MP (Release 6-5)
        {
            if (m_DiceRoller.RollChance(40)) //don't want too many safes or it would be too easy
                map.PlaceMapObjectAt(MakeObjOpenBankSafe(GameImages.OBJ_BANK_SAFE_OPEN), safept);
            else
                map.PlaceMapObjectAt(MakeObjClosedBankSafe(GameImages.OBJ_BANK_SAFE_CLOSED), safept);
        }

        private void PlaceBankVaultDoor(Map map, Point pt) //@@MP (Release 6-5)
        {
            //randomly select 
            int roll = m_DiceRoller.Roll(0, 2);  //@@MP - disabled locked doors, as players were reincarnating as actors who are trapped behind them in the vault rooms (Release 7-6)
            switch (roll)
            {
                case 0: map.PlaceMapObjectAt(MakeObjIronDoor(DoorWindow.STATE_OPEN), pt); break;
                case 1: map.PlaceMapObjectAt(MakeObjIronDoor(DoorWindow.STATE_BROKEN), pt); break;
                case 2: map.PlaceMapObjectAt(MakeObjIronDoor(DoorWindow.STATE_LOCKED), pt); break;
                default: throw new InvalidOperationException("unhandled roll");
            }
        }

        /// <summary>
        /// Either an Office (for large enough buildings) or an Agency (for small buildings).
        /// </summary>
        protected virtual CHARBuildingType MakeCHARBuilding(Map map, Block b)
        {
            ///////////////////////////////
            // Offices are large buildings.
            // Agency are small ones.
            ///////////////////////////////
            if (b.InsideRect.Width < 8 || b.InsideRect.Height < 8)
            {
                // small, make it an Agency.
                if (MakeCHARAgency(map, b))
                    return CHARBuildingType.AGENCY;
                else
                    return CHARBuildingType.NONE;
            }
            else
            {
                if (MakeCHAROffice(map, b))
                    return CHARBuildingType.OFFICE;
                else
                    return CHARBuildingType.NONE;
            }
        }

        static string[] CHAR_POSTERS = { GameImages.DECO_CHAR_POSTER1, GameImages.DECO_CHAR_POSTER2, GameImages.DECO_CHAR_POSTER3 };

        protected virtual bool MakeCHARAgency(Map map, Block b)
        {
            /////////////////////////////
            // 1. Walkway, floor & walls
            /////////////////////////////
            TileRectangle(map, m_Game.GameTiles.FLOOR_WALKWAY, b.Rectangle);
            TileRectangle(map, m_Game.GameTiles.WALL_CHAR_OFFICE, b.BuildingRect);
            TileFill(map, m_Game.GameTiles.FLOOR_OFFICE, b.InsideRect,
                (tile, prevmodel, x, y) =>
                {
                    tile.IsInside = true;
                    tile.AddDecoration(GameImages.DECO_CHAR_FLOOR_LOGO);
                });

            //////////////////////////
            // 2. Decide orientation.
            //////////////////////////          
            bool horizontalCorridor = (b.InsideRect.Width >= b.InsideRect.Height);

            /////////////////
            // 3. Entry door 
            /////////////////
#region
            int midX = b.Rectangle.Left + b.Rectangle.Width / 2;
            int midY = b.Rectangle.Top + b.Rectangle.Height / 2;

            // make doors on one side.
#region
            if (horizontalCorridor)
            {
                bool west = m_DiceRoller.RollChance(50);

                if (west)
                {
                    // west
                    PlaceDoor(map, b.BuildingRect.Left, midY, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                    if (b.InsideRect.Height >= 8)
                    {
                        PlaceDoor(map, b.BuildingRect.Left, midY - 1, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                        if (b.InsideRect.Height >= 12)
                            PlaceDoor(map, b.BuildingRect.Left, midY + 1, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                    }
                }
                else
                {
                    // east
                    PlaceDoor(map, b.BuildingRect.Right - 1, midY, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                    if (b.InsideRect.Height >= 8)
                    {
                        PlaceDoor(map, b.BuildingRect.Right - 1, midY - 1, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                        if (b.InsideRect.Height >= 12)
                            PlaceDoor(map, b.BuildingRect.Right - 1, midY + 1, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                    }
                }
            }
            else
            {
                bool north = m_DiceRoller.RollChance(50);

                if (north)
                {
                    // north
                    PlaceDoor(map, midX, b.BuildingRect.Top, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                    if (b.InsideRect.Width >= 8)
                    {
                        PlaceDoor(map, midX - 1, b.BuildingRect.Top, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                        if (b.InsideRect.Width >= 12)
                            PlaceDoor(map, midX + 1, b.BuildingRect.Top, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                    }
                }
                else
                {
                    // south
                    PlaceDoor(map, midX, b.BuildingRect.Bottom - 1, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                    if (b.InsideRect.Width >= 8)
                    {
                        PlaceDoor(map, midX - 1, b.BuildingRect.Bottom - 1, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                        if (b.InsideRect.Width >= 12)
                            PlaceDoor(map, midX + 1, b.BuildingRect.Bottom - 1, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                    }
                }
            }
#endregion

            // add office image next to doors.
            string officeImage = GameImages.DECO_CHAR_OFFICE;
            DecorateOutsideWalls(map, b.BuildingRect, (x, y) => map.GetMapObjectAt(x, y) == null && CountAdjDoors(map, x, y) >= 1 ? officeImage : null);
#endregion

            ////////////////
            // 4. Furniture
            ////////////////
#region
            // chairs on the sides.
            MapObjectFill(map, b.InsideRect,
                (pt) =>
                {
                    if (CountAdjWalls(map, pt.X, pt.Y) < 3)
                        return null;
                    return MakeObjChair(GameImages.OBJ_CHAR_CHAIR);
                });
            // walls/pilars in the middle.
            TileFill(map, m_Game.GameTiles.WALL_CHAR_OFFICE, new Rectangle(b.InsideRect.Left + b.InsideRect.Width / 2 - 1, b.InsideRect.Top + b.InsideRect.Height / 2 - 1, 3, 2),
                (tile, model, x, y) =>
                {
                    tile.AddDecoration(CHAR_POSTERS[m_DiceRoller.Roll(0, CHAR_POSTERS.Length)]);
                });
            //@@MP - computers in middle (Release 3)
            int nbComputers = 8;
            for (int i = 0; i < nbComputers; i++)
            {
                MapObjectPlaceInGoodPosition(map, b.InsideRect,
                (pt) => CountAdjWalls(map, pt.X, pt.Y) == 0 && !IsADoorNSEW(map, pt.X, pt.Y),
                m_DiceRoller,
                (pt) => MakeObjWorkstation(GameImages.OBJ_CHAR_DESKTOP));
            }
#endregion

            //////////////
            // 5. Posters
            //////////////
#region
            // outside.
            DecorateOutsideWalls(map, b.BuildingRect,
                (x, y) =>
                {
                    if (CountAdjDoors(map, x, y) > 0)
                        return null;
                    else
                    {
                        if (m_DiceRoller.RollChance(25))
                            return CHAR_POSTERS[m_DiceRoller.Roll(0, CHAR_POSTERS.Length)];
                        else
                            return null;
                    }
                });
#endregion

            ////////////
            // 6. Zones.
            ////////////
            map.AddZone(MakeUniqueZone("CHAR Agency", b.BuildingRect));
            MakeWalkwayZones(map, b);

            // Done
            return true;
        }

        protected virtual bool MakeCHAROffice(Map map, Block b)
        {

            /////////////////////////////
            // 1. Walkway, floor & walls
            /////////////////////////////
            TileRectangle(map, m_Game.GameTiles.FLOOR_WALKWAY, b.Rectangle);
            TileRectangle(map, m_Game.GameTiles.WALL_CHAR_OFFICE, b.BuildingRect);
            TileFill(map, m_Game.GameTiles.FLOOR_OFFICE, b.InsideRect, (tile, prevmodel, x, y) => tile.IsInside = true);

            //////////////////////////
            // 2. Decide orientation.
            //////////////////////////          
            bool horizontalCorridor = (b.InsideRect.Width >= b.InsideRect.Height);

            /////////////////
            // 3. Entry door 
            /////////////////
#region
            int midX = b.Rectangle.Left + b.Rectangle.Width / 2;
            int midY = b.Rectangle.Top + b.Rectangle.Height / 2;
            Direction doorSide;

            // make doors on one side.
#region
            if (horizontalCorridor)
            {
                bool west = m_DiceRoller.RollChance(50);

                if (west)
                {
                    doorSide = Direction.W;
                    // west
                    PlaceDoor(map, b.BuildingRect.Left, midY, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                    if (b.InsideRect.Height >= 8)
                    {
                        PlaceDoor(map, b.BuildingRect.Left, midY - 1, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                        if (b.InsideRect.Height >= 12)
                            PlaceDoor(map, b.BuildingRect.Left, midY + 1, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                    }
                }
                else
                {
                    doorSide = Direction.E;
                    // east
                    PlaceDoor(map, b.BuildingRect.Right - 1, midY, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                    if (b.InsideRect.Height >= 8)
                    {
                        PlaceDoor(map, b.BuildingRect.Right - 1, midY - 1, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                        if (b.InsideRect.Height >= 12)
                            PlaceDoor(map, b.BuildingRect.Right - 1, midY + 1, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                    }
                }
            }
            else
            {
                bool north = m_DiceRoller.RollChance(50);

                if (north)
                {
                    doorSide = Direction.N;
                    // north
                    PlaceDoor(map, midX, b.BuildingRect.Top, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                    if (b.InsideRect.Width >= 8)
                    {
                        PlaceDoor(map, midX - 1, b.BuildingRect.Top, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                        if (b.InsideRect.Width >= 12)
                            PlaceDoor(map, midX + 1, b.BuildingRect.Top, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                    }
                }
                else
                {
                    doorSide = Direction.S;
                    // south
                    PlaceDoor(map, midX, b.BuildingRect.Bottom - 1, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                    if (b.InsideRect.Width >= 8)
                    {
                        PlaceDoor(map, midX - 1, b.BuildingRect.Bottom - 1, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                        if (b.InsideRect.Width >= 12)
                            PlaceDoor(map, midX + 1, b.BuildingRect.Bottom - 1, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                    }
                }
            }
#endregion

            // add office image next to doors.
            string officeImage = GameImages.DECO_CHAR_OFFICE;
            DecorateOutsideWalls(map, b.BuildingRect, (x, y) => map.GetMapObjectAt(x, y) == null && CountAdjDoors(map, x, y) >= 1 ? officeImage : null);

            // barricade entry doors.
            BarricadeDoors(map, b.BuildingRect, Rules.BARRICADING_MAX);
#endregion

            ///////////////////////
            // 4. Make entry hall.
            ///////////////////////
#region
            const int hallDepth = 3;
            if (doorSide == Direction.N)
            {
                TileHLine(map, m_Game.GameTiles.WALL_CHAR_OFFICE, b.InsideRect.Left, b.InsideRect.Top + hallDepth, b.InsideRect.Width);
            }
            else if (doorSide == Direction.S)
            {
                TileHLine(map, m_Game.GameTiles.WALL_CHAR_OFFICE, b.InsideRect.Left, b.InsideRect.Bottom - 1 - hallDepth, b.InsideRect.Width);
            }
            else if (doorSide == Direction.E)
            {
                TileVLine(map, m_Game.GameTiles.WALL_CHAR_OFFICE, b.InsideRect.Right - 1 - hallDepth, b.InsideRect.Top, b.InsideRect.Height);
            }
            else if (doorSide == Direction.W)
            {
                TileVLine(map, m_Game.GameTiles.WALL_CHAR_OFFICE, b.InsideRect.Left + hallDepth, b.InsideRect.Top, b.InsideRect.Height);
            }
            else
                throw new InvalidOperationException("unhandled door side");
#endregion

            /////////////////////////////////////
            // 5. Make central corridor & wings
            /////////////////////////////////////
#region
            Rectangle corridorRect;
            Point corridorDoor;
            if (doorSide == Direction.N)
            {
                corridorRect = new Rectangle(midX - 1, b.InsideRect.Top + hallDepth, 3, b.BuildingRect.Height - 1 - hallDepth);
                corridorDoor = new Point(corridorRect.Left + 1, corridorRect.Top);
            }
            else if (doorSide == Direction.S)
            {
                corridorRect = new Rectangle(midX - 1, b.BuildingRect.Top, 3, b.BuildingRect.Height - 1 - hallDepth);
                corridorDoor = new Point(corridorRect.Left + 1, corridorRect.Bottom - 1);
            }
            else if (doorSide == Direction.E)
            {
                corridorRect = new Rectangle(b.BuildingRect.Left, midY - 1, b.BuildingRect.Width - 1 - hallDepth, 3);
                corridorDoor = new Point(corridorRect.Right - 1, corridorRect.Top + 1);
            }
            else if (doorSide == Direction.W)
            {
                corridorRect = new Rectangle(b.InsideRect.Left + hallDepth, midY - 1, b.BuildingRect.Width - 1 - hallDepth, 3);
                corridorDoor = new Point(corridorRect.Left, corridorRect.Top + 1);
            }
            else
                throw new InvalidOperationException("unhandled door side");

            TileRectangle(map, m_Game.GameTiles.WALL_CHAR_OFFICE, corridorRect);
            PlaceDoor(map, corridorDoor.X, corridorDoor.Y, m_Game.GameTiles.FLOOR_OFFICE, MakeObjCharDoor());
#endregion

            /////////////////////////
            // 6. Make office rooms.
            /////////////////////////
#region
            // make wings.
            Rectangle wingOne;
            Rectangle wingTwo;
            if (horizontalCorridor)
            {
                // top side.
                wingOne = new Rectangle(corridorRect.Left, b.BuildingRect.Top, corridorRect.Width, 1 + corridorRect.Top - b.BuildingRect.Top);
                // bottom side.
                wingTwo = new Rectangle(corridorRect.Left, corridorRect.Bottom - 1, corridorRect.Width, 1 + b.BuildingRect.Bottom - corridorRect.Bottom);
            }
            else
            {
                // left side
                wingOne = new Rectangle(b.BuildingRect.Left, corridorRect.Top, 1 + corridorRect.Left - b.BuildingRect.Left, corridorRect.Height);
                // right side
                wingTwo = new Rectangle(corridorRect.Right - 1, corridorRect.Top, 1 + b.BuildingRect.Right - corridorRect.Right, corridorRect.Height);
            }

            // make rooms in each wing with doors leaving toward corridor.
            const int officeRoomsSize = 4;

            List<Rectangle> officesOne = new List<Rectangle>();
            MakeRoomsPlan(map, ref officesOne, wingOne, officeRoomsSize, officeRoomsSize);

            List<Rectangle> officesTwo = new List<Rectangle>();
            MakeRoomsPlan(map, ref officesTwo, wingTwo, officeRoomsSize, officeRoomsSize);

            List<Rectangle> allOffices = new List<Rectangle>(officesOne.Count + officesTwo.Count);
            allOffices.AddRange(officesOne);
            allOffices.AddRange(officesTwo);

            foreach (Rectangle roomRect in officesOne)
            {
                TileRectangle(map, m_Game.GameTiles.WALL_CHAR_OFFICE, roomRect);
            }
            foreach (Rectangle roomRect in officesTwo)
            {
                TileRectangle(map, m_Game.GameTiles.WALL_CHAR_OFFICE, roomRect);
            }

            foreach (Rectangle roomRect in officesOne)
            {
                if (horizontalCorridor)
                    PlaceDoor(map, roomRect.Left + roomRect.Width / 2, roomRect.Bottom - 1, m_Game.GameTiles.FLOOR_OFFICE, MakeObjCharDoor());
                else
                    PlaceDoor(map, roomRect.Right - 1, roomRect.Top + roomRect.Height / 2, m_Game.GameTiles.FLOOR_OFFICE, MakeObjCharDoor());
            }
            foreach (Rectangle roomRect in officesTwo)
            {
                if (horizontalCorridor)
                    PlaceDoor(map, roomRect.Left + roomRect.Width / 2, roomRect.Top, m_Game.GameTiles.FLOOR_OFFICE, MakeObjCharDoor());
                else
                    PlaceDoor(map, roomRect.Left, roomRect.Top + roomRect.Height / 2, m_Game.GameTiles.FLOOR_OFFICE, MakeObjCharDoor());
            }

            // tables with chairs.
            foreach (Rectangle roomRect in allOffices)
            {
                // table.
                Point tablePos = new Point(roomRect.Left + roomRect.Width / 2, roomRect.Top + roomRect.Height / 2);
                map.PlaceMapObjectAt(MakeObjTable(GameImages.OBJ_CHAR_TABLE), tablePos);

                // try to put a computer and chair in the room
                int nbChairs = 1;
                Rectangle insideRoom = new Rectangle(roomRect.Left + 1, roomRect.Top + 1, roomRect.Width - 2, roomRect.Height - 2);
                if (!insideRoom.IsEmpty)
                {
                    for (int i = 0; i < nbChairs; i++)
                    {
                        Rectangle adjTableRect = new Rectangle(tablePos.X - 1, tablePos.Y - 1, 3, 3);
                        adjTableRect.Intersect(insideRoom);
                        MapObjectPlaceInGoodPosition(map, adjTableRect,
                            (pt) => pt != tablePos,
                            m_DiceRoller,
                            (pt) => MakeObjChair(GameImages.OBJ_CHAR_CHAIR));

                        //@@MP - match each chair with a computer (Release 3)
                        MapObjectPlaceInGoodPosition(map, adjTableRect,
                            (pt) => pt != tablePos && !IsADoorNSEW(map, pt.X, pt.Y),
                            m_DiceRoller,
                            (pt) => MakeObjWorkstation(GameImages.OBJ_CHAR_DESKTOP));
                    }
                }
            }
#endregion

            ////////////////
            // 7. Add items.
            ////////////////
#region
            // drop goodies in rooms.
            foreach (Rectangle roomRect in allOffices)
            {
                ItemsDrop(map, roomRect,
                    (pt) =>
                    {
                        Tile tile = map.GetTileAt(pt.X, pt.Y);
                        if (tile.Model != m_Game.GameTiles.FLOOR_OFFICE)
                            return false;
                        MapObject mapObj = map.GetMapObjectAt(pt);
                        if (mapObj != null)
                            return false;
                        return true;
                    },
                    (pt) => MakeRandomCHAROfficeItem());
            }
#endregion

            ///////////
            // 8. Zone
            ///////////
            Zone zone = MakeUniqueZone("CHAR Office", b.BuildingRect);
            zone.SetGameAttribute<bool>(ZoneAttributes.IS_CHAR_OFFICE, true);
            map.AddZone(zone);
            MakeWalkwayZones(map, b);

            // Done
            return true;
        }

        protected virtual bool MakeOrdinaryOffice(Map map, Block b) //@@MP - plain, non-CHAR (Release 7-3)
        {
            /////////////////////////////
            // 1. Walkway, floor & walls
            /////////////////////////////
            TileRectangle(map, m_Game.GameTiles.FLOOR_WALKWAY, b.Rectangle);
            TileRectangle(map, m_Game.GameTiles.WALL_CONCRETE, b.BuildingRect);
            TileFill(map, m_Game.GameTiles.FLOOR_OFFICE, b.InsideRect, (tile, prevmodel, x, y) => tile.IsInside = true);

            //////////////////////////
            // 2. Decide orientation.
            //////////////////////////          
            bool horizontalCorridor = (b.InsideRect.Width >= b.InsideRect.Height);

            /////////////////
            // 3. Entry door 
            /////////////////
#region
            int midX = b.Rectangle.Left + b.Rectangle.Width / 2;
            int midY = b.Rectangle.Top + b.Rectangle.Height / 2;
            Direction doorSide;

            // make doors on one side.
#region
            if (horizontalCorridor)
            {
                bool west = m_DiceRoller.RollChance(50);

                if (west)
                {
                    doorSide = Direction.W;
                    // west
                    PlaceDoor(map, b.BuildingRect.Left, midY, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                    if (b.InsideRect.Height >= 8)
                    {
                        PlaceDoor(map, b.BuildingRect.Left, midY - 1, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                        if (b.InsideRect.Height >= 12)
                            PlaceDoor(map, b.BuildingRect.Left, midY + 1, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                    }
                }
                else
                {
                    doorSide = Direction.E;
                    // east
                    PlaceDoor(map, b.BuildingRect.Right - 1, midY, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                    if (b.InsideRect.Height >= 8)
                    {
                        PlaceDoor(map, b.BuildingRect.Right - 1, midY - 1, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                        if (b.InsideRect.Height >= 12)
                            PlaceDoor(map, b.BuildingRect.Right - 1, midY + 1, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                    }
                }
            }
            else
            {
                bool north = m_DiceRoller.RollChance(50);

                if (north)
                {
                    doorSide = Direction.N;
                    // north
                    PlaceDoor(map, midX, b.BuildingRect.Top, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                    if (b.InsideRect.Width >= 8)
                    {
                        PlaceDoor(map, midX - 1, b.BuildingRect.Top, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                        if (b.InsideRect.Width >= 12)
                            PlaceDoor(map, midX + 1, b.BuildingRect.Top, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                    }
                }
                else
                {
                    doorSide = Direction.S;
                    // south
                    PlaceDoor(map, midX, b.BuildingRect.Bottom - 1, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                    if (b.InsideRect.Width >= 8)
                    {
                        PlaceDoor(map, midX - 1, b.BuildingRect.Bottom - 1, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                        if (b.InsideRect.Width >= 12)
                            PlaceDoor(map, midX + 1, b.BuildingRect.Bottom - 1, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjGlassDoor());
                    }
                }
            }
#endregion

            // add office image next to doors.
            string officeImage = GameImages.DECO_GENERIC_OFFICE;
            DecorateOutsideWalls(map, b.BuildingRect, (x, y) => map.GetMapObjectAt(x, y) == null && CountAdjDoors(map, x, y) >= 1 ? officeImage : null);
#endregion

            ///////////////////////
            // 4. Make foyer.
            ///////////////////////
#region
            const int foyerDepth = 3;
            Rectangle foyerRect;
            if (doorSide == Direction.N)
            {
                TileHLine(map, m_Game.GameTiles.WALL_LIGHT_BROWN, b.InsideRect.Left, b.InsideRect.Top + foyerDepth, b.InsideRect.Width);
                foyerRect = new Rectangle(b.InsideRect.Left + 1, b.InsideRect.Top + 1, b.InsideRect.Width - 2, foyerDepth);
            }
            else if (doorSide == Direction.S)
            {
                TileHLine(map, m_Game.GameTiles.WALL_LIGHT_BROWN, b.InsideRect.Left, b.InsideRect.Bottom - 1 - foyerDepth, b.InsideRect.Width);
                foyerRect = new Rectangle(b.InsideRect.Left + 1, b.InsideRect.Bottom - foyerDepth, b.InsideRect.Width - 2, foyerDepth);
            }
            else if (doorSide == Direction.E)
            {
                TileVLine(map, m_Game.GameTiles.WALL_LIGHT_BROWN, b.InsideRect.Right - 1 - foyerDepth, b.InsideRect.Top, b.InsideRect.Height);
                foyerRect = new Rectangle(b.InsideRect.Right - foyerDepth, b.InsideRect.Top + 1, foyerDepth, b.InsideRect.Height - 2);
            }
            else if (doorSide == Direction.W)
            {
                TileVLine(map, m_Game.GameTiles.WALL_LIGHT_BROWN, b.InsideRect.Left + foyerDepth, b.InsideRect.Top, b.InsideRect.Height);
                foyerRect = new Rectangle(b.InsideRect.Left + 1, b.InsideRect.Top + 1, foyerDepth, b.InsideRect.Height - 2);
            }
            else
                throw new InvalidOperationException("unhandled door side");
#endregion

            /////////////////////////////////////
            // 5. Make central corridor & wings
            /////////////////////////////////////
#region
            Rectangle corridorRect;
            Point corridorDoor, receptionPos;
            if (doorSide == Direction.N)
            {
                corridorRect = new Rectangle(midX - 1, b.InsideRect.Top + foyerDepth, 3, b.BuildingRect.Height - 1 - foyerDepth);
                corridorDoor = new Point(corridorRect.Left + 1, corridorRect.Top);
                receptionPos = new Point(corridorRect.Left, corridorRect.Top - 1);
            }
            else if (doorSide == Direction.S)
            {
                corridorRect = new Rectangle(midX - 1, b.BuildingRect.Top, 3, b.BuildingRect.Height - 1 - foyerDepth);
                corridorDoor = new Point(corridorRect.Left + 1, corridorRect.Bottom - 1);
                receptionPos = new Point(corridorRect.Left, corridorRect.Bottom);
            }
            else if (doorSide == Direction.E)
            {
                corridorRect = new Rectangle(b.BuildingRect.Left, midY - 1, b.BuildingRect.Width - 1 - foyerDepth, 3);
                corridorDoor = new Point(corridorRect.Right - 1, corridorRect.Top + 1);
                receptionPos = new Point(corridorRect.Right, corridorRect.Top);
            }
            else if (doorSide == Direction.W)
            {
                corridorRect = new Rectangle(b.InsideRect.Left + foyerDepth, midY - 1, b.BuildingRect.Width - 1 - foyerDepth, 3);
                corridorDoor = new Point(corridorRect.Left, corridorRect.Top + 1);
                receptionPos = new Point(corridorRect.Left - 1, corridorRect.Top);
            }
            else
                throw new InvalidOperationException("unhandled door side");

            TileRectangle(map, m_Game.GameTiles.WALL_LIGHT_BROWN, corridorRect);
            PlaceDoor(map, corridorDoor.X, corridorDoor.Y, m_Game.GameTiles.FLOOR_OFFICE, MakeObjGlassDoor());

            //foyer objects
            //-reception desk
            map.PlaceMapObjectAt(MakeObjReceptionDesk(GameImages.OBJ_CLINIC_DESK), receptionPos);
            //-try to put couches into the foyer
            int nbCouches = 6;
            for (int i = 0; i < nbCouches; i++)
            {
                MapObjectPlaceInGoodPosition(map, foyerRect,
                    (pt) => !IsADoorNSEW(map, pt.X, pt.Y) && map.IsWalkable(pt.X, pt.Y) && CountAdjWalls(map, pt) >= 3,
                    m_DiceRoller,
                    (pt) => MakeObjCouch(GameImages.OBJ_COUCH));
            }
#endregion

            /////////////////////////
            // 6. Make office rooms.
            /////////////////////////
#region
            // make wings.
            Rectangle wingOne;
            Rectangle wingTwo;
            if (horizontalCorridor)
            {
                // top side.
                wingOne = new Rectangle(corridorRect.Left, b.BuildingRect.Top, corridorRect.Width, 1 + corridorRect.Top - b.BuildingRect.Top);
                // bottom side.
                wingTwo = new Rectangle(corridorRect.Left, corridorRect.Bottom - 1, corridorRect.Width, 1 + b.BuildingRect.Bottom - corridorRect.Bottom);
            }
            else
            {
                // left side
                wingOne = new Rectangle(b.BuildingRect.Left, corridorRect.Top, 1 + corridorRect.Left - b.BuildingRect.Left, corridorRect.Height);
                // right side
                wingTwo = new Rectangle(corridorRect.Right - 1, corridorRect.Top, 1 + b.BuildingRect.Right - corridorRect.Right, corridorRect.Height);
            }

            // make rooms in each wing with doors leaving toward corridor.
            const int officeRoomsSize = 4;

            List<Rectangle> officesOne = new List<Rectangle>();
            MakeRoomsPlan(map, ref officesOne, wingOne, officeRoomsSize, officeRoomsSize);

            List<Rectangle> officesTwo = new List<Rectangle>();
            MakeRoomsPlan(map, ref officesTwo, wingTwo, officeRoomsSize, officeRoomsSize);

            List<Rectangle> allOffices = new List<Rectangle>(officesOne.Count + officesTwo.Count);
            allOffices.AddRange(officesOne);
            allOffices.AddRange(officesTwo);

            foreach (Rectangle roomRect in officesOne)
            {
                TileRectangle(map, m_Game.GameTiles.WALL_LIGHT_BROWN, roomRect);
                map.AddZone(MakeUniqueZone("room", roomRect));
            }
            foreach (Rectangle roomRect in officesTwo)
            {
                TileRectangle(map, m_Game.GameTiles.WALL_LIGHT_BROWN, roomRect);
                map.AddZone(MakeUniqueZone("room", roomRect));
            }

            foreach (Rectangle roomRect in officesOne)
            {
                if (horizontalCorridor)
                {
                    PlaceDoor(map, roomRect.Left + roomRect.Width / 2, roomRect.Bottom - 1, m_Game.GameTiles.FLOOR_OFFICE, MakeObjGlassDoor());
                }
                else
                {
                    PlaceDoor(map, roomRect.Right - 1, roomRect.Top + roomRect.Height / 2, m_Game.GameTiles.FLOOR_OFFICE, MakeObjGlassDoor());
                }
            }
            foreach (Rectangle roomRect in officesTwo)
            {
                if (horizontalCorridor)
                {
                    PlaceDoor(map, roomRect.Left + roomRect.Width / 2, roomRect.Top, m_Game.GameTiles.FLOOR_OFFICE, MakeObjGlassDoor());
                }
                else
                {
                    PlaceDoor(map, roomRect.Left, roomRect.Top + roomRect.Height / 2, m_Game.GameTiles.FLOOR_OFFICE, MakeObjGlassDoor());
                }
            }

            // tables with chairs.
            foreach (Rectangle roomRect in allOffices)
            {
                // table.
                Point tablePos = new Point(roomRect.Left + roomRect.Width / 2, roomRect.Top + roomRect.Height / 2);
                map.PlaceMapObjectAt(MakeObjTable(GameImages.OBJ_TABLE), tablePos);

                // try to put a computer and chair in the room
                int nbChairs = 1;
                Rectangle insideRoom = new Rectangle(roomRect.Left + 1, roomRect.Top + 1, roomRect.Width - 2, roomRect.Height - 2);
                if (!insideRoom.IsEmpty)
                {
                    for (int i = 0; i < nbChairs; i++)
                    {
                        Rectangle adjTableRect = new Rectangle(tablePos.X - 1, tablePos.Y - 1, 3, 3);
                        adjTableRect.Intersect(insideRoom);
                        MapObjectPlaceInGoodPosition(map, adjTableRect,
                            (pt) => pt != tablePos,
                            m_DiceRoller,
                            (pt) => MakeObjChair(GameImages.OBJ_CHAIR));

                        //@@MP - match each chair with a computer
                        MapObjectPlaceInGoodPosition(map, adjTableRect,
                            (pt) => pt != tablePos && !IsADoorNSEW(map, pt.X, pt.Y),
                            m_DiceRoller,
                            (pt) => MakeObjWorkstation(GameImages.OBJ_DESKTOP_COMPUTER));
                    }

                    MapObjectPlaceInGoodPosition(map, insideRoom,
                        (pt) => CountAdjWalls(map, pt) >= 3 && IsADoorNSEW(map, pt.X, pt.Y),
                        m_DiceRoller,
                        (pt) => MakeObjCouch(GameImages.OBJ_COUCH));
                }
            }
#endregion

            ////////////////
            // 7. Add items.
            ////////////////
#region
            // drop goodies in rooms.
            foreach (Rectangle roomRect in allOffices)
            {
                ItemsDrop(map, roomRect,
                    (pt) =>
                    {
                        Tile tile = map.GetTileAt(pt.X, pt.Y);
                        if (tile.Model != m_Game.GameTiles.FLOOR_OFFICE)
                            return false;
                        MapObject mapObj = map.GetMapObjectAt(pt);
                        if (mapObj != null)
                            return false;
                        return true;
                    },
                    (pt) => MakeRandomOrdinaryOfficeItem());
            }
#endregion

            ///////////
            // 8. Zone
            ///////////
            Zone zone = MakeUniqueZone("Business", b.BuildingRect); //didn't use "office" to avoid clashing with CHAR buildings
            map.AddZone(zone);
            MakeWalkwayZones(map, b);

            // Done
            return true;
        }

        protected virtual ArmyBuildingType MakeArmyOffice(Map map, Block b) //@@MP (Release 6-3)
        {
            // Need at least 8*8
            if (b.InsideRect.Width < 8 || b.InsideRect.Height < 8)
                return ArmyBuildingType.NONE; //too small for an army office

            // we have enough room
            /////////////////////////////
            // 1. Walkway, floor & walls
            /////////////////////////////
            TileRectangle(map, m_Game.GameTiles.FLOOR_WALKWAY, b.Rectangle);
            TileRectangle(map, m_Game.GameTiles.WALL_ARMY_BASE, b.BuildingRect);
            TileFill(map, m_Game.GameTiles.FLOOR_ARMY, b.InsideRect, (tile, prevmodel, x, y) => tile.IsInside = true);

            //////////////////////////
            // 2. Decide orientation.
            //////////////////////////          
            bool horizontalCorridor = (b.InsideRect.Width >= b.InsideRect.Height);

            /////////////////
            // 3. Entry door 
            /////////////////
#region
            int midX = b.Rectangle.Left + b.Rectangle.Width / 2;
            int midY = b.Rectangle.Top + b.Rectangle.Height / 2;
            Direction doorSide;

            // make doors on one side.
#region
            if (horizontalCorridor)
            {
                bool west = m_DiceRoller.RollChance(50);
                if (west)
                {
                    doorSide = Direction.W;
                    // west
                    PlaceDoor(map, b.BuildingRect.Left, midY, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjIronDoor(DoorWindow.STATE_LOCKED)); //need to remove any non-walkable tile
                    if (b.InsideRect.Height >= 8) //more space, so add another door
                    {
                        PlaceDoor(map, b.BuildingRect.Left, midY - 1, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjIronDoor(DoorWindow.STATE_LOCKED));
                        if (b.InsideRect.Height >= 12) //even more space, so add another door
                        {
                            PlaceDoor(map, b.BuildingRect.Left, midY + 1, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjIronDoor(DoorWindow.STATE_LOCKED));
                        }
                    }
                }
                else
                {
                    doorSide = Direction.E;
                    // east
                    PlaceDoor(map, b.BuildingRect.Right - 1, midY, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjIronDoor(DoorWindow.STATE_LOCKED));
                    if (b.InsideRect.Height >= 8)
                    {
                        PlaceDoor(map, b.BuildingRect.Right - 1, midY - 1, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjIronDoor(DoorWindow.STATE_LOCKED));
                        if (b.InsideRect.Height >= 12)
                        {
                            PlaceDoor(map, b.BuildingRect.Right - 1, midY + 1, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjIronDoor(DoorWindow.STATE_LOCKED));
                        }
                    }
                }
            }
            else
            {
                bool north = m_DiceRoller.RollChance(50);
                if (north)
                {
                    doorSide = Direction.N;
                    // north
                    PlaceDoor(map, midX, b.BuildingRect.Top, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjIronDoor(DoorWindow.STATE_LOCKED));
                    if (b.InsideRect.Width >= 8)
                    {
                        PlaceDoor(map, midX - 1, b.BuildingRect.Top, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjIronDoor(DoorWindow.STATE_LOCKED));
                        if (b.InsideRect.Width >= 12)
                        {
                            PlaceDoor(map, midX + 1, b.BuildingRect.Top, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjIronDoor(DoorWindow.STATE_LOCKED));
                        }
                    }
                }
                else
                {
                    doorSide = Direction.S;
                    // south
                    PlaceDoor(map, midX, b.BuildingRect.Bottom - 1, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjIronDoor(DoorWindow.STATE_LOCKED));
                    if (b.InsideRect.Width >= 8)
                    {
                        PlaceDoor(map, midX - 1, b.BuildingRect.Bottom - 1, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjIronDoor(DoorWindow.STATE_LOCKED));
                        if (b.InsideRect.Width >= 12)
                        {
                            PlaceDoor(map, midX + 1, b.BuildingRect.Bottom - 1, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjIronDoor(DoorWindow.STATE_LOCKED));
                        }
                    }
                }
            }
#endregion

            // add office image next to doors.
            string officeImage = GameImages.DECO_ARMY_POSTER1;
            DecorateOutsideWalls(map, b.BuildingRect, (x, y) => map.GetMapObjectAt(x, y) == null && CountAdjDoors(map, x, y) >= 1 ? officeImage : null);
#endregion

            ///////////////////////
            // 4. Make entry hall.
            ///////////////////////
#region
            const int hallDepth = 3;
            if (doorSide == Direction.N)
            {
                TileHLine(map, m_Game.GameTiles.WALL_ARMY_BASE, b.InsideRect.Left, b.InsideRect.Top + hallDepth, b.InsideRect.Width);
            }
            else if (doorSide == Direction.S)
            {
                TileHLine(map, m_Game.GameTiles.WALL_ARMY_BASE, b.InsideRect.Left, b.InsideRect.Bottom - 1 - hallDepth, b.InsideRect.Width);
            }
            else if (doorSide == Direction.E)
            {
                TileVLine(map, m_Game.GameTiles.WALL_ARMY_BASE, b.InsideRect.Right - 1 - hallDepth, b.InsideRect.Top, b.InsideRect.Height);
            }
            else if (doorSide == Direction.W)
            {
                TileVLine(map, m_Game.GameTiles.WALL_ARMY_BASE, b.InsideRect.Left + hallDepth, b.InsideRect.Top, b.InsideRect.Height);
            }
            else
                throw new InvalidOperationException("unhandled door side");
#endregion

            /////////////////////////////////////
            // 5. Make central corridor & wings
            /////////////////////////////////////
#region
            Rectangle corridorRect;
            Point corridorDoor;
            if (doorSide == Direction.N)
            {
                corridorRect = new Rectangle(midX - 1, b.InsideRect.Top + hallDepth, 3, b.BuildingRect.Height - 1 - hallDepth);
                corridorDoor = new Point(corridorRect.Left + 1, corridorRect.Top);
            }
            else if (doorSide == Direction.S)
            {
                corridorRect = new Rectangle(midX - 1, b.BuildingRect.Top, 3, b.BuildingRect.Height - 1 - hallDepth);
                corridorDoor = new Point(corridorRect.Left + 1, corridorRect.Bottom - 1);
            }
            else if (doorSide == Direction.E)
            {
                corridorRect = new Rectangle(b.BuildingRect.Left, midY - 1, b.BuildingRect.Width - 1 - hallDepth, 3);
                corridorDoor = new Point(corridorRect.Right - 1, corridorRect.Top + 1);
            }
            else if (doorSide == Direction.W)
            {
                corridorRect = new Rectangle(b.InsideRect.Left + hallDepth, midY - 1, b.BuildingRect.Width - 1 - hallDepth, 3);
                corridorDoor = new Point(corridorRect.Left, corridorRect.Top + 1);
            }
            else
                throw new InvalidOperationException("unhandled door side");

            TileRectangle(map, m_Game.GameTiles.WALL_ARMY_BASE, corridorRect);
            PlaceDoor(map, corridorDoor.X, corridorDoor.Y, m_Game.GameTiles.FLOOR_ARMY, MakeObjIronDoor(DoorWindow.STATE_CLOSED)); //need to remove any non-walkable tile
#endregion

            /////////////////////////
            // 6. Make office rooms.
            /////////////////////////
#region
            // make wings.
            Rectangle wingOne;
            Rectangle wingTwo;
            if (horizontalCorridor)
            {
                // top side.
                wingOne = new Rectangle(corridorRect.Left, b.BuildingRect.Top, corridorRect.Width, 1 + corridorRect.Top - b.BuildingRect.Top);
                // bottom side.
                wingTwo = new Rectangle(corridorRect.Left, corridorRect.Bottom - 1, corridorRect.Width, 1 + b.BuildingRect.Bottom - corridorRect.Bottom);
            }
            else
            {
                // left side
                wingOne = new Rectangle(b.BuildingRect.Left, corridorRect.Top, 1 + corridorRect.Left - b.BuildingRect.Left, corridorRect.Height);
                // right side
                wingTwo = new Rectangle(corridorRect.Right - 1, corridorRect.Top, 1 + b.BuildingRect.Right - corridorRect.Right, corridorRect.Height);
            }

            // make rooms in each wing with doors leaving toward corridor.
            const int officeRoomsSize = 4;

            List<Rectangle> officesOne = new List<Rectangle>();
            MakeRoomsPlan(map, ref officesOne, wingOne, officeRoomsSize, officeRoomsSize);

            List<Rectangle> officesTwo = new List<Rectangle>();
            MakeRoomsPlan(map, ref officesTwo, wingTwo, officeRoomsSize, officeRoomsSize);

            List<Rectangle> allOffices = new List<Rectangle>(officesOne.Count + officesTwo.Count);
            allOffices.AddRange(officesOne);
            allOffices.AddRange(officesTwo);

            foreach (Rectangle roomRect in officesOne)
            {
                TileRectangle(map, m_Game.GameTiles.WALL_ARMY_BASE, roomRect);
            }
            foreach (Rectangle roomRect in officesTwo)
            {
                TileRectangle(map, m_Game.GameTiles.WALL_ARMY_BASE, roomRect);
            }

            foreach (Rectangle roomRect in officesOne)
            {
                if (horizontalCorridor)
                    PlaceDoor(map, roomRect.Left + roomRect.Width / 2, roomRect.Bottom - 1, m_Game.GameTiles.FLOOR_ARMY, MakeObjIronDoor(DoorWindow.STATE_CLOSED));
                else
                    PlaceDoor(map, roomRect.Right - 1, roomRect.Top + roomRect.Height / 2, m_Game.GameTiles.FLOOR_ARMY, MakeObjIronDoor(DoorWindow.STATE_CLOSED));
            }
            foreach (Rectangle roomRect in officesTwo)
            {
                if (horizontalCorridor)
                    PlaceDoor(map, roomRect.Left + roomRect.Width / 2, roomRect.Top, m_Game.GameTiles.FLOOR_ARMY, MakeObjIronDoor(DoorWindow.STATE_CLOSED));
                else
                    PlaceDoor(map, roomRect.Left, roomRect.Top + roomRect.Height / 2, m_Game.GameTiles.FLOOR_ARMY, MakeObjIronDoor(DoorWindow.STATE_CLOSED));
            }

            // tables with chairs.
            foreach (Rectangle roomRect in allOffices)
            {
                // table.
                Point tablePos = new Point(roomRect.Left + roomRect.Width / 2, roomRect.Top + roomRect.Height / 2);
                map.PlaceMapObjectAt(MakeObjTable(GameImages.OBJ_ARMY_TABLE), tablePos);

                // try to put a computer and chair in the room
                int nbChairs = 1;
                Rectangle insideRoom = new Rectangle(roomRect.Left + 1, roomRect.Top + 1, roomRect.Width - 2, roomRect.Height - 2);
                if (!insideRoom.IsEmpty)
                {
                    for (int i = 0; i < nbChairs; i++)
                    {
                        Rectangle adjTableRect = new Rectangle(tablePos.X - 1, tablePos.Y - 1, 3, 3);
                        adjTableRect.Intersect(insideRoom);
                        MapObjectPlaceInGoodPosition(map, adjTableRect,
                            (pt) => pt != tablePos,
                            m_DiceRoller,
                            (pt) => MakeObjChair(GameImages.OBJ_HOSPITAL_CHAIR));

                        //@@MP - match each chair with a computer (Release 3)
                        MapObjectPlaceInGoodPosition(map, adjTableRect,
                            (pt) => pt != tablePos && !IsADoorNSEW(map, pt.X, pt.Y),
                            m_DiceRoller,
                            (pt) => MakeObjWorkstation(GameImages.OBJ_ARMY_COMPUTER_STATION));
                    }
                }
            }
#endregion

            ////////////////
            // 7. Add items.
            ////////////////
#region
            // drop goodies in rooms.
            foreach (Rectangle roomRect in allOffices)
            {
                ItemsDrop(map, roomRect,
                    (pt) =>
                    {
                        Tile tile = map.GetTileAt(pt.X, pt.Y);
                        if (tile.Model != m_Game.GameTiles.FLOOR_ARMY)
                            return false;
                        MapObject mapObj = map.GetMapObjectAt(pt);
                        if (mapObj != null)
                            return false;
                        return true;
                    },
                    (pt) => MakeRandomCHAROfficeItem());
            }
#endregion

            ///////////
            // 8. Zone
            ///////////
            Zone zone = MakeUniqueZone("Army Office", b.BuildingRect);
            zone.SetGameAttribute<bool>(ZoneAttributes.IS_ARMY_OFFICE, true);
            map.AddZone(zone);
            MakeWalkwayZones(map, b);

            // done
            return ArmyBuildingType.OFFICE;
        }

        protected virtual bool MakeParkBuilding(Map map, Block b, bool isgraveyard) //@@MP - added graveyard argument (Release 4)
        {
            ////////////////////////
            // 0. Check suitability
            ////////////////////////
            if (b.InsideRect.Width < 3 || b.InsideRect.Height < 3)
                return false;

            /////////////////////////////
            // 1. Grass, walkway & fence
            /////////////////////////////
            TileRectangle(map, m_Game.GameTiles.FLOOR_WALKWAY, b.Rectangle);
            TileFill(map, m_Game.GameTiles.FLOOR_GRASS, b.InsideRect);
            MapObjectFill(map, b.BuildingRect,
                (pt) =>
                {
                    /*if (map.IsWalkable(pt)) //@@MP - added walkable check (Release 6-3)
                    {*/
                        bool placeFence = (pt.X == b.BuildingRect.Left || pt.X == b.BuildingRect.Right - 1 || pt.Y == b.BuildingRect.Top || pt.Y == b.BuildingRect.Bottom - 1);
                        if (placeFence)
                        {
                            if (isgraveyard) //@@MP (Release 4)
                                return MakeObjIronRailing(GameImages.OBJ_GRAVEYARD_FENCE); //@@MP - corrected to an iron railing (Release 7-6)
                            else
                                return null; //@@MP - removed park fences (Release 7-3)
                        }
                        else
                            return null;
                    /*}
                    else
                        return null;*/
                });

            ///////////////////////////////
            // 2. Random trees and benches
            ///////////////////////////////
            if (isgraveyard) //@@MP - added the chance to become a graveyard (Release 4)
            {
                MapObjectFill(map, b.InsideRect,
                (pt) =>
                {
                    if (m_DiceRoller.RollChance(PARK_GRAVE_OR_TREE_CHANCE)) //@@MP - use the original tree chance, but within that a higher chance to be a grave instead
                    {
                        int placeObject = m_DiceRoller.Roll(0, 10);
                        switch (placeObject)
                        {
                            case 0: return MakeObjParkTree(m_DiceRoller); //10%
                            case 1:
                            case 2:
                            case 3:
                            case 4:
                            case 5:
                            case 6: return MakeObjTombstone(GameImages.OBJ_PLAIN_TOMBSTONE); //@@MP - took the duplicates out of 1 to 5 and let them fall through (Release 5-3)
                            case 7:
                            case 8:
                            case 9: return MakeObjTombstone(GameImages.OBJ_CROSS_TOMBSTONE); //@@MP - took the duplicates out of 7 & 8 and let them fall through (Release 5-3)
                            default:
                                return null;
                        }
                    }
                    else
                        return null;
                });
            }
            else //trees
            {
                MapObjectFill(map, b.BuildingRect,
                (pt) =>
                {
                    
                    if (m_DiceRoller.RollChance(PARK_TREE_CHANCE))
                        return MakeObjParkTree(m_DiceRoller);
                    else
                        return null;
                });
            }

            //benches
            MapObjectFill(map, b.InsideRect,
                (pt) =>
                {
                    if (m_DiceRoller.RollChance(PARK_BENCH_CHANCE))
                        return MakeObjBench(GameImages.OBJ_BENCH);
                    else
                        return null;
                });

            ///////////////
            // 3. Entrance
            ///////////////
            if (isgraveyard) //@@MP - removed fences for parks (Release 7-3)
            {
                int entranceFace = m_DiceRoller.Roll(0, 4);
                int ex, ey;
                switch (entranceFace)
                {
                    case 0: // west
                        ex = b.BuildingRect.Left;
                        ey = b.BuildingRect.Top + b.BuildingRect.Height / 2;
                        break;
                    case 1: // east
                        ex = b.BuildingRect.Right - 1;
                        ey = b.BuildingRect.Top + b.BuildingRect.Height / 2;
                        break;
                    case 3: // north
                        ex = b.BuildingRect.Left + b.BuildingRect.Width / 2;
                        ey = b.BuildingRect.Top;
                        break;
                    default: // south
                        ex = b.BuildingRect.Left + b.BuildingRect.Width / 2;
                        ey = b.BuildingRect.Bottom - 1;
                        break;
                }
                map.RemoveMapObjectAt(ex, ey);
                map.SetTileModelAt(ex, ey, m_Game.GameTiles.FLOOR_WALKWAY);
            }

            ////////////
            // 4. Items
            ////////////
            if (!isgraveyard) //@@MP - only add stuff to parks
            {
                ItemsDrop(map, b.InsideRect,
                (pt) => map.GetMapObjectAt(pt) == null && m_DiceRoller.RollChance(PARK_ITEM_CHANCE),
                (pt) => MakeRandomParkItem());
            }

            ///////////
            // 5. Zone
            ///////////
            if (isgraveyard)
                map.AddZone(MakeUniqueZone("Graveyard", b.BuildingRect));
            else
                map.AddZone(MakeUniqueZone("Park", b.BuildingRect));
            MakeWalkwayZones(map, b);

            ////////////
            // 6. Pond?  //@@MP - based on alpha 10 shed (Release 6-1)
            ////////////
            if (b.InsideRect.Width > PARK_POND_WIDTH + 2 && b.InsideRect.Height > PARK_POND_HEIGHT + 2)
            {
                if (m_DiceRoller.RollChance(PARK_POND_CHANCE))
                {
                    // roll pond pos - dont put next to park fences!
                    int pondX = m_DiceRoller.Roll(b.InsideRect.Left + 1, b.InsideRect.Right - PARK_POND_WIDTH);
                    int pondY = m_DiceRoller.Roll(b.InsideRect.Top + 1, b.InsideRect.Bottom - PARK_POND_HEIGHT);
                    Rectangle pondRect = new Rectangle(pondX, pondY, PARK_POND_WIDTH, PARK_POND_HEIGHT); //for the edge tiles (a la walls)
                    Rectangle pondInsideRect = new Rectangle(pondX + 1, pondY + 1, PARK_POND_WIDTH - 2, PARK_POND_HEIGHT - 2); //for the total water tiles (a la floor)

                    // clear everything but zones in pond location
                    ClearRectangle(map, pondRect, false);

                    // build it
                    MakeParkPond(map, "Pond", pondRect);

                    // drop a fishing rod           //@@MP (Release 7-6)
                    map.DropItemAt(MakeItemFishingRod(), new Point (pondX, pondY));
                }
            }
            else //add a fire barrel         //@@MP (Release 7-6)
            {
                bool placedBarrel = false;
                MapObjectFill(map, b.InsideRect,
                (pt) =>
                {
                    if (!placedBarrel)
                    {
                        if (m_DiceRoller.RollChance(PARK_BENCH_CHANCE))
                        {
                            placedBarrel = true; //just give us the one
                                return MakeObjFireBarrel(GameImages.OBJ_EMPTY_BIN);
                        }
                        else
                            return null;
                    }
                    else //already got one
                            return null;
                });
            }

            // Done.
            return true;
        }

        protected virtual void MakeParkPond(Map map, string baseZoneName, Rectangle pondBuildingRect)  //@@MP - based on alpha 10 shed (Release 6-1)
        {
            Rectangle pondInsideRect = new Rectangle(pondBuildingRect.X + 1, pondBuildingRect.Y + 1, pondBuildingRect.Width - 2, pondBuildingRect.Height - 2);

            // build & zone
            TileFill(map, m_Game.GameTiles.FLOOR_POND_CENTER, pondInsideRect, (tile, prevTileModel, x, y) => tile.IsInside = false); //@@MP made false (Release 6-1)
            map.AddZone(MakeUniqueZone(baseZoneName, pondInsideRect));
            map.HasFishing = true; //@@MP (Release 7-6)
            map.HasWaterTiles = true; //used by the AI if they are on fire and looking for somewhere to extinguish themselves

            // place the edges and corners in the right orientation //@@MP - making a pond rather than a shed (Release 6-1)
            //WEST
            int westX = pondBuildingRect.Left;
            int westY = pondBuildingRect.Top;
            do
            {
                if (westY == pondBuildingRect.Bottom - 1) //bottom corner
                    map.SetTileModelAt(westX, westY, m_Game.GameTiles.FLOOR_POND_SW_CORNER);
                else if (westY == pondBuildingRect.Top) //top corner
                    map.SetTileModelAt(westX, westY, m_Game.GameTiles.FLOOR_POND_NW_CORNER);
                else //not a corner
                    map.SetTileModelAt(westX, westY, m_Game.GameTiles.FLOOR_POND_W_EDGE);

                westY++;
            } while (westY <= pondBuildingRect.Bottom - 1);

            //EAST
            int eastX = pondBuildingRect.Right - 1;
            int eastY = pondBuildingRect.Top;
            do
            {
                if (eastY == pondBuildingRect.Bottom - 1) //bottom corner
                    map.SetTileModelAt(eastX, eastY, m_Game.GameTiles.FLOOR_POND_SE_CORNER);
                else if (eastY == pondBuildingRect.Top) //top corner
                    map.SetTileModelAt(eastX, eastY, m_Game.GameTiles.FLOOR_POND_NE_CORNER);
                else //not a corner
                    map.SetTileModelAt(eastX, eastY, m_Game.GameTiles.FLOOR_POND_E_EDGE);

                eastY++;
            } while (eastY <= pondBuildingRect.Bottom - 1);

            //NORTH
            int northX = pondBuildingRect.Left;
            int northY = pondBuildingRect.Top;
            do
            {
                if (northX == pondBuildingRect.Left) //west corner
                    map.SetTileModelAt(northX, northY, m_Game.GameTiles.FLOOR_POND_NW_CORNER);
                else if (northX == pondBuildingRect.Right - 1) //east corner
                    map.SetTileModelAt(northX, northY, m_Game.GameTiles.FLOOR_POND_NE_CORNER);
                else //not a corner
                    map.SetTileModelAt(northX, northY, m_Game.GameTiles.FLOOR_POND_N_EDGE);

                northX++;
            } while (northX <= pondBuildingRect.Right - 1);

            //SOUTH
            int southX = pondBuildingRect.Left;
            int southY = pondBuildingRect.Bottom - 1;
            do
            {
                if (southX == pondBuildingRect.Left) //west corner
                    map.SetTileModelAt(southX, southY, m_Game.GameTiles.FLOOR_POND_SW_CORNER);
                else if (southX == pondBuildingRect.Right - 1) //east corner
                    map.SetTileModelAt(southX, southY, m_Game.GameTiles.FLOOR_POND_SE_CORNER);
                else //not a corner
                    map.SetTileModelAt(southX, southY, m_Game.GameTiles.FLOOR_POND_S_EDGE);

                southX++;
            } while (southX <= pondBuildingRect.Right - 1);
        }

        protected virtual bool MakeNarrowPark(Map map, Block b) //@@MP (Release 7-3)
        {
            /////////////////////////////
            // 1. Grass (no walkway nor fence)
            /////////////////////////////
            TileFill(map, m_Game.GameTiles.FLOOR_GRASS, b.BuildingRect);

            ///////////////////////////////
            // 2. Random trees and benches
            ///////////////////////////////
            //trees
            MapObjectFill(map, b.BuildingRect,
            (pt) =>
            {

                if (m_DiceRoller.RollChance(PARK_TREE_CHANCE))
                    return MakeObjParkTree(m_DiceRoller);
                else
                    return null;
            });

            //benches
            MapObjectFill(map, b.BuildingRect,
                (pt) =>
                {
                    if (m_DiceRoller.RollChance(PARK_BENCH_CHANCE))
                        return MakeObjBench(GameImages.OBJ_BENCH);
                    else
                        return null;
                });


            ////////////
            // 3. Items
            ////////////
            ItemsDrop(map, b.BuildingRect,
            (pt) => map.GetMapObjectAt(pt) == null && m_DiceRoller.RollChance(PARK_ITEM_CHANCE),
            (pt) => MakeRandomParkItem());

            ///////////
            // 5. Zone
            ///////////
            map.AddZone(MakeUniqueZone("Park", b.BuildingRect));
            MakeWalkwayZones(map, b);

            // Done.
            return true;
        }

        protected virtual bool MakeTennisCourt(Map map, Block b) //@@MP (Release 7-3)
        {
            ////////////////////////
            // 0. Check suitability
            ////////////////////////
            if (b.BuildingRect.Width != 8 || b.BuildingRect.Height != 10)
                return false;

            /////////////////////////////
            // 1. Edges, walkway & fence
            /////////////////////////////
            TileRectangle(map, m_Game.GameTiles.FLOOR_WALKWAY, b.Rectangle);
            TileRectangle(map, m_Game.GameTiles.FLOOR_TENNIS_COURT_OUTER, b.BuildingRect); //for the outer edges. the actual court comes in step 2
            MapObjectFill(map, b.BuildingRect,
                (pt) =>
                {
                    if (pt.X == b.BuildingRect.Left || pt.X == b.BuildingRect.Right - 1 || pt.Y == b.BuildingRect.Top || pt.Y == b.BuildingRect.Bottom - 1) //place fence
                        return MakeObjFence(GameImages.OBJ_CHAINWIRE_FENCE);
                    else
                        return null;
                });

            ///////////////
            // 2. Place down the fixed-layout court
            ///////////////
            List<TileModel> listOfTilesToPlace = new List<TileModel>
            {
#region
                m_Game.GameTiles.FLOOR_TENNIS_COURT_10, m_Game.GameTiles.FLOOR_TENNIS_COURT_11, m_Game.GameTiles.FLOOR_TENNIS_COURT_12, m_Game.GameTiles.FLOOR_TENNIS_COURT_13,
                m_Game.GameTiles.FLOOR_TENNIS_COURT_14, m_Game.GameTiles.FLOOR_TENNIS_COURT_15, m_Game.GameTiles.FLOOR_TENNIS_COURT_18, m_Game.GameTiles.FLOOR_TENNIS_COURT_19,
                m_Game.GameTiles.FLOOR_TENNIS_COURT_20, m_Game.GameTiles.FLOOR_TENNIS_COURT_21, m_Game.GameTiles.FLOOR_TENNIS_COURT_22, m_Game.GameTiles.FLOOR_TENNIS_COURT_23, 
                m_Game.GameTiles.FLOOR_TENNIS_COURT_26, m_Game.GameTiles.FLOOR_TENNIS_COURT_27, m_Game.GameTiles.FLOOR_TENNIS_COURT_28, m_Game.GameTiles.FLOOR_TENNIS_COURT_29, 
                m_Game.GameTiles.FLOOR_TENNIS_COURT_30, m_Game.GameTiles.FLOOR_TENNIS_COURT_31, m_Game.GameTiles.FLOOR_TENNIS_COURT_34, m_Game.GameTiles.FLOOR_TENNIS_COURT_35, 
                m_Game.GameTiles.FLOOR_TENNIS_COURT_36, m_Game.GameTiles.FLOOR_TENNIS_COURT_37, m_Game.GameTiles.FLOOR_TENNIS_COURT_38, m_Game.GameTiles.FLOOR_TENNIS_COURT_39, 
                m_Game.GameTiles.FLOOR_TENNIS_COURT_42, m_Game.GameTiles.FLOOR_TENNIS_COURT_43, m_Game.GameTiles.FLOOR_TENNIS_COURT_44, m_Game.GameTiles.FLOOR_TENNIS_COURT_45, 
                m_Game.GameTiles.FLOOR_TENNIS_COURT_46, m_Game.GameTiles.FLOOR_TENNIS_COURT_47, m_Game.GameTiles.FLOOR_TENNIS_COURT_50, m_Game.GameTiles.FLOOR_TENNIS_COURT_51, 
                m_Game.GameTiles.FLOOR_TENNIS_COURT_52, m_Game.GameTiles.FLOOR_TENNIS_COURT_53, m_Game.GameTiles.FLOOR_TENNIS_COURT_54, m_Game.GameTiles.FLOOR_TENNIS_COURT_55, 
                m_Game.GameTiles.FLOOR_TENNIS_COURT_58, m_Game.GameTiles.FLOOR_TENNIS_COURT_59, m_Game.GameTiles.FLOOR_TENNIS_COURT_60, m_Game.GameTiles.FLOOR_TENNIS_COURT_61, 
                m_Game.GameTiles.FLOOR_TENNIS_COURT_62, m_Game.GameTiles.FLOOR_TENNIS_COURT_63, m_Game.GameTiles.FLOOR_TENNIS_COURT_66, m_Game.GameTiles.FLOOR_TENNIS_COURT_67, 
                m_Game.GameTiles.FLOOR_TENNIS_COURT_68, m_Game.GameTiles.FLOOR_TENNIS_COURT_69, m_Game.GameTiles.FLOOR_TENNIS_COURT_70, m_Game.GameTiles.FLOOR_TENNIS_COURT_71
#endregion
            };
            int globalPieceIndex = 0; //all 80 tiles that make up the court
            int toPlaceIndex = 0; //the 48 tiles that we need to place

            for (int y = b.BuildingRect.Top; y < b.BuildingRect.Bottom; y++) 
            {
                for (int x = b.BuildingRect.Left; x < b.BuildingRect.Right; x++)
                {
                    ++globalPieceIndex;
                    if (globalPieceIndex == 72)
                        break; //71 is the last global piece we need to place down manually

                    if (map.GetTileAt(x, y).Model == m_Game.GameTiles.FLOOR_TENNIS_COURT_OUTER)
                        continue; //we're on an outer edge that we set earlier

                    map.SetTileModelAt(x, y, listOfTilesToPlace[toPlaceIndex]);
                    if (toPlaceIndex < 47)
                        ++toPlaceIndex;
                    else
                        break; //the last piece we need to place down manually
                }
                if (globalPieceIndex == 72)
                    break; //71 is the last global piece we need to place down manually
            }

            ///////////////
            // 3. Entrance
            ///////////////
            int entranceFace = m_DiceRoller.Roll(0, 4);
            int ex, ey;
            switch (entranceFace)
            {
                case 0: // west
                    ex = b.BuildingRect.Left;
                    ey = b.BuildingRect.Top + b.BuildingRect.Height / 2;
                    break;
                case 1: // east
                    ex = b.BuildingRect.Right - 1;
                    ey = b.BuildingRect.Top + b.BuildingRect.Height / 2;
                    break;
                case 3: // north
                    ex = b.BuildingRect.Left + b.BuildingRect.Width / 2;
                    ey = b.BuildingRect.Top;
                    break;
                default: // south
                    ex = b.BuildingRect.Left + b.BuildingRect.Width / 2;
                    ey = b.BuildingRect.Bottom - 1;
                    break;
            }
            map.RemoveMapObjectAt(ex, ey);
            map.PlaceMapObjectAt(MakeObjChainFenceGate(DoorWindow.STATE_CLOSED), new Point(ex, ey));

            ////////////
            // 4. Items
            ////////////
            ItemsDrop(map, b.InsideRect,
            (pt) => map.GetMapObjectAt(pt) == null && m_DiceRoller.RollChance(10), //10%
            (pt) => MakeItemTennisRacket());

            ///////////
            // 5. Zone
            ///////////
            map.AddZone(MakeUniqueZone("Tennis court", b.BuildingRect));
            MakeWalkwayZones(map, b);

            // Done.
            return true;
        }

        protected virtual bool MakeBasketballCourt(Map map, Block b) //@@MP (Release 7-3)
        {
            ////////////////////////
            // 0. Check suitability
            ////////////////////////
            if (b.BuildingRect.Width != 10 || b.BuildingRect.Height != 8)
                return false;

            /////////////////////////////
            // 1. Edges, walkway & fence
            /////////////////////////////
            TileRectangle(map, m_Game.GameTiles.FLOOR_WALKWAY, b.Rectangle);
            TileRectangle(map, m_Game.GameTiles.FLOOR_BASKETBALL_COURT_OUTER, b.BuildingRect); //for the outer edges. the actual court comes in step 2
            MapObjectFill(map, b.BuildingRect,
                (pt) =>
                {
                    if (pt.X == b.BuildingRect.Left || pt.X == b.BuildingRect.Right - 1 || pt.Y == b.BuildingRect.Top || pt.Y == b.BuildingRect.Bottom - 1) //place fence
                        return MakeObjFence(GameImages.OBJ_CHAINWIRE_FENCE);
                    else
                        return null;
                });

            ///////////////
            // 2. Place down the fixed-layout court
            ///////////////
            List<TileModel> listOfTilesToPlace = new List<TileModel>
            {
#region
                m_Game.GameTiles.FLOOR_BASKETBALL_COURT_18, m_Game.GameTiles.FLOOR_BASKETBALL_COURT_19, m_Game.GameTiles.FLOOR_BASKETBALL_COURT_20, m_Game.GameTiles.FLOOR_BASKETBALL_COURT_21,
                m_Game.GameTiles.FLOOR_BASKETBALL_COURT_22, m_Game.GameTiles.FLOOR_BASKETBALL_COURT_23, m_Game.GameTiles.FLOOR_BASKETBALL_COURT_24, m_Game.GameTiles.FLOOR_BASKETBALL_COURT_25,
                m_Game.GameTiles.FLOOR_BASKETBALL_COURT_27, m_Game.GameTiles.FLOOR_BASKETBALL_COURT_28, m_Game.GameTiles.FLOOR_BASKETBALL_COURT_29, m_Game.GameTiles.FLOOR_BASKETBALL_COURT_30,
                m_Game.GameTiles.FLOOR_BASKETBALL_COURT_31, m_Game.GameTiles.FLOOR_BASKETBALL_COURT_32, m_Game.GameTiles.FLOOR_BASKETBALL_COURT_33, m_Game.GameTiles.FLOOR_BASKETBALL_COURT_34,
                m_Game.GameTiles.FLOOR_BASKETBALL_COURT_36, m_Game.GameTiles.FLOOR_BASKETBALL_COURT_37, m_Game.GameTiles.FLOOR_BASKETBALL_COURT_38, m_Game.GameTiles.FLOOR_BASKETBALL_COURT_39,
                m_Game.GameTiles.FLOOR_BASKETBALL_COURT_40, m_Game.GameTiles.FLOOR_BASKETBALL_COURT_41, m_Game.GameTiles.FLOOR_BASKETBALL_COURT_42, m_Game.GameTiles.FLOOR_BASKETBALL_COURT_43,
                m_Game.GameTiles.FLOOR_BASKETBALL_COURT_45, m_Game.GameTiles.FLOOR_BASKETBALL_COURT_46, m_Game.GameTiles.FLOOR_BASKETBALL_COURT_47, m_Game.GameTiles.FLOOR_BASKETBALL_COURT_48,
                m_Game.GameTiles.FLOOR_BASKETBALL_COURT_49, m_Game.GameTiles.FLOOR_BASKETBALL_COURT_50, m_Game.GameTiles.FLOOR_BASKETBALL_COURT_51, m_Game.GameTiles.FLOOR_BASKETBALL_COURT_52,
                m_Game.GameTiles.FLOOR_BASKETBALL_COURT_54, m_Game.GameTiles.FLOOR_BASKETBALL_COURT_55, m_Game.GameTiles.FLOOR_BASKETBALL_COURT_56, m_Game.GameTiles.FLOOR_BASKETBALL_COURT_57,
                m_Game.GameTiles.FLOOR_BASKETBALL_COURT_58, m_Game.GameTiles.FLOOR_BASKETBALL_COURT_59, m_Game.GameTiles.FLOOR_BASKETBALL_COURT_60, m_Game.GameTiles.FLOOR_BASKETBALL_COURT_61,
                m_Game.GameTiles.FLOOR_BASKETBALL_COURT_63, m_Game.GameTiles.FLOOR_BASKETBALL_COURT_64, m_Game.GameTiles.FLOOR_BASKETBALL_COURT_65, m_Game.GameTiles.FLOOR_BASKETBALL_COURT_66,
                m_Game.GameTiles.FLOOR_BASKETBALL_COURT_67, m_Game.GameTiles.FLOOR_BASKETBALL_COURT_68, m_Game.GameTiles.FLOOR_BASKETBALL_COURT_69, m_Game.GameTiles.FLOOR_BASKETBALL_COURT_70
#endregion
            };
            int globalPieceIndex = 0; //all 80 tiles that make up the court
            int toPlaceIndex = 0; //the 48 tiles that we need to place
            bool fireBarrelPlaced = false; //@@MP - added (Release 7-6)

            for (int y = b.BuildingRect.Top; y < b.BuildingRect.Bottom; y++)
            {
                for (int x = b.BuildingRect.Left; x < b.BuildingRect.Right; x++)
                {
                    ++globalPieceIndex;
                    if (globalPieceIndex == 71)
                        break; //70 is the last global piece we need to place down manually

                    if (map.GetTileAt(x, y).Model == m_Game.GameTiles.FLOOR_BASKETBALL_COURT_OUTER)
                        continue; //we're on an outer edge that we set earlier

                    map.SetTileModelAt(x, y, listOfTilesToPlace[toPlaceIndex]);
                    //these two spots have rings
                    if (listOfTilesToPlace[toPlaceIndex] == m_Game.GameTiles.FLOOR_BASKETBALL_COURT_36 || listOfTilesToPlace[toPlaceIndex] == m_Game.GameTiles.FLOOR_BASKETBALL_COURT_43)
                        map.PlaceMapObjectAt(MakeObjBasketballRing(GameImages.OBJ_BASKETBALL_RING), new Point(x, y));
                    else if (!fireBarrelPlaced && m_DiceRoller.RollChance(5)) //@@MP - added (Release 7-6)
                    {
                        map.PlaceMapObjectAt(MakeObjFireBarrel(GameImages.OBJ_EMPTY_BIN), new Point(x, y));
                        fireBarrelPlaced = true;
                    }

                    if (toPlaceIndex < 47)
                        ++toPlaceIndex;
                    else
                        break; //the last piece we need to place down manually
                }
                if (globalPieceIndex == 71)
                    break; //70 is the last global piece we need to place down manually
            }
            
            ///////////////
            // 3. Entrance
            ///////////////
            int entranceFace = m_DiceRoller.Roll(0, 4);
            int ex, ey;
            switch (entranceFace)
            {
                case 0: // west
                    ex = b.BuildingRect.Left;
                    ey = b.BuildingRect.Top + b.BuildingRect.Height / 2;
                    break;
                case 1: // east
                    ex = b.BuildingRect.Right - 1;
                    ey = b.BuildingRect.Top + b.BuildingRect.Height / 2;
                    break;
                case 3: // north
                    ex = b.BuildingRect.Left + b.BuildingRect.Width / 2;
                    ey = b.BuildingRect.Top;
                    break;
                default: // south
                    ex = b.BuildingRect.Left + b.BuildingRect.Width / 2;
                    ey = b.BuildingRect.Bottom - 1;
                    break;
            }
            map.RemoveMapObjectAt(ex, ey);
            map.PlaceMapObjectAt(MakeObjChainFenceGate(DoorWindow.STATE_CLOSED), new Point(ex, ey));

            ///////////
            // 4. Zone
            ///////////
            map.AddZone(MakeUniqueZone("Basketball court", b.BuildingRect));
            MakeWalkwayZones(map, b);

            // Done.
            return true;
        }

        protected virtual bool MakeHousingBuilding(Map map, Block b) // alpha10.1 makes apartement or house
        {
            // alpha10.1 decide floorplan

            if (m_DiceRoller.RollChance(HOUSE_IS_APARTMENTS_CHANCE)) // apartment?
                if (MakeApartmentsBuilding(map, b))
                    return true;

            return MakeVanillaHousingBuilding(map, b); // vanilla house?
        }

        protected virtual bool MakeApartmentsBuilding(Map map, Block b) // alpha10.1 apartment
        {
            ////////////////////////
            // 0. Check suitability
            ////////////////////////
            if (b.InsideRect.Width < 9 || b.InsideRect.Height < 9)
                return false;
            if (b.InsideRect.Width > 17 || b.InsideRect.Height > 17)
                return false;

            // I pretty much copied and edited the char office algorithm. lame but i'm lazy.

            /////////////////////////////
            // 1. Walkway, floor & walls
            /////////////////////////////
            TileRectangle(map, m_Game.GameTiles.FLOOR_WALKWAY, b.Rectangle);
            TileRectangle(map, m_Game.GameTiles.WALL_BRICK, b.BuildingRect);
            TileFill(map, m_Game.GameTiles.FLOOR_PLANKS, b.InsideRect, (tile, prevmodel, x, y) => tile.IsInside = true);

            //////////////////////////
            // 2. Decide orientation.
            //////////////////////////          
            bool horizontalCorridor = (b.InsideRect.Width >= b.InsideRect.Height);

            /////////////////////////////////////
            // 3. Entry door and opposite window
            /////////////////////////////////////
#region
            int midX = b.Rectangle.Left + b.Rectangle.Width / 2;
            int midY = b.Rectangle.Top + b.Rectangle.Height / 2;
            Direction doorSide;

            if (horizontalCorridor)
            {
                bool west = m_DiceRoller.RollChance(50);

                if (west)
                {
                    doorSide = Direction.W;
                    // west
                    PlaceDoor(map, b.BuildingRect.Left, midY, m_Game.GameTiles.FLOOR_PLANKS, MakeObjWoodenDoor());
                    PlaceDoor(map, b.BuildingRect.Right - 1, midY, m_Game.GameTiles.FLOOR_PLANKS, MakeObjWindow());
                }
                else
                {
                    doorSide = Direction.E;
                    // east
                    PlaceDoor(map, b.BuildingRect.Right - 1, midY, m_Game.GameTiles.FLOOR_PLANKS, MakeObjWoodenDoor());
                    PlaceDoor(map, b.BuildingRect.Left, midY, m_Game.GameTiles.FLOOR_PLANKS, MakeObjWindow());
                }
            }
            else
            {
                bool north = m_DiceRoller.RollChance(50);

                if (north)
                {
                    doorSide = Direction.N;
                    // north
                    PlaceDoor(map, midX, b.BuildingRect.Top, m_Game.GameTiles.FLOOR_PLANKS, MakeObjWoodenDoor());
                    PlaceDoor(map, midX, b.BuildingRect.Bottom - 1, m_Game.GameTiles.FLOOR_PLANKS, MakeObjWindow());
                }
                else
                {
                    doorSide = Direction.S;
                    // south
                    PlaceDoor(map, midX, b.BuildingRect.Bottom - 1, m_Game.GameTiles.FLOOR_PLANKS, MakeObjWoodenDoor());
                    PlaceDoor(map, midX, b.BuildingRect.Top, m_Game.GameTiles.FLOOR_PLANKS, MakeObjWindow());
                }
            }
#endregion

            //////////////////////////////////////////////
            // 4. Make central corridor & side apartments
            //////////////////////////////////////////////
#region
            Rectangle corridorRect;
            if (doorSide == Direction.N)
                corridorRect = new Rectangle(midX, b.InsideRect.Top, 1, b.BuildingRect.Height - 1);
            else if (doorSide == Direction.S)
                corridorRect = new Rectangle(midX, b.BuildingRect.Top, 1, b.BuildingRect.Height - 1);
            else if (doorSide == Direction.E)
                corridorRect = new Rectangle(b.BuildingRect.Left, midY, b.BuildingRect.Width - 1, 1);
            else if (doorSide == Direction.W)
                corridorRect = new Rectangle(b.InsideRect.Left, midY, b.BuildingRect.Width - 1, 1);
            else
                throw new InvalidOperationException("apartment: unhandled door side");
#endregion

            //////////////////////
            // 5. Make apartments
            //////////////////////
#region
            // make wings.
            Rectangle wingOne;
            Rectangle wingTwo;
            if (horizontalCorridor)
            {
                // top side.
                wingOne = Rectangle.FromLTRB(b.BuildingRect.Left, b.BuildingRect.Top, b.BuildingRect.Right, corridorRect.Top);
                // bottom side.
                wingTwo = Rectangle.FromLTRB(b.BuildingRect.Left, corridorRect.Bottom, b.BuildingRect.Right, b.BuildingRect.Bottom);
            }
            else
            {
                // left side
                wingOne = Rectangle.FromLTRB(b.BuildingRect.Left, b.BuildingRect.Top, corridorRect.Left, b.BuildingRect.Bottom);
                // right side
                wingTwo = Rectangle.FromLTRB(corridorRect.Right, b.BuildingRect.Top, b.BuildingRect.Right, b.BuildingRect.Bottom);
            }

            // make apartements in each wing with doors leaving toward corridor and windows to the outside
            // pick sizes so the apartements are not cut into multiple rooms by MakeRoomsPlan
            int apartmentMinXSize, apartmentMinYSize;
            if (horizontalCorridor)
            {
                apartmentMinXSize = 4;
                apartmentMinYSize = b.BuildingRect.Height / 2;
            }
            else
            {
                apartmentMinXSize = b.BuildingRect.Width / 2;
                apartmentMinYSize = 4;
            }

            List<Rectangle> apartementsWingOne = new List<Rectangle>();
            MakeRoomsPlan(map, ref apartementsWingOne, wingOne, apartmentMinXSize, apartmentMinYSize);
            List<Rectangle> apartementsWingTwo = new List<Rectangle>();
            MakeRoomsPlan(map, ref apartementsWingTwo, wingTwo, apartmentMinXSize, apartmentMinYSize);

            List<Rectangle> allApartments = new List<Rectangle>(apartementsWingOne.Count + apartementsWingTwo.Count);
            allApartments.AddRange(apartementsWingOne);
            allApartments.AddRange(apartementsWingTwo);

            foreach (Rectangle apartRect in apartementsWingOne)
                TileRectangle(map, m_Game.GameTiles.WALL_BRICK, apartRect);
            foreach (Rectangle roomRect in apartementsWingTwo)
                TileRectangle(map, m_Game.GameTiles.WALL_BRICK, roomRect);

            // put door leading to corridor; and an opposite window if outer wall / a door if inside
            foreach (Rectangle apartRect in apartementsWingOne)
            {
                if (horizontalCorridor)
                {
                    PlaceDoor(map, apartRect.Left + apartRect.Width / 2, apartRect.Bottom - 1, m_Game.GameTiles.FLOOR_PLANKS, MakeObjWoodenDoor());
                    PlaceDoor(map, apartRect.Left + apartRect.Width / 2, apartRect.Top, m_Game.GameTiles.FLOOR_PLANKS, MakeObjWindow());
                }
                else
                {
                    PlaceDoor(map, apartRect.Right - 1, apartRect.Top + apartRect.Height / 2, m_Game.GameTiles.FLOOR_PLANKS, MakeObjWoodenDoor());
                    PlaceDoor(map, apartRect.Left, apartRect.Top + apartRect.Height / 2, m_Game.GameTiles.FLOOR_PLANKS, MakeObjWindow());
                }
            }
            foreach (Rectangle apartRect in apartementsWingTwo)
            {
                if (horizontalCorridor)
                {
                    PlaceDoor(map, apartRect.Left + apartRect.Width / 2, apartRect.Top, m_Game.GameTiles.FLOOR_PLANKS, MakeObjWoodenDoor());
                    PlaceDoor(map, apartRect.Left + apartRect.Width / 2, apartRect.Bottom - 1, m_Game.GameTiles.FLOOR_PLANKS, MakeObjWindow());
                }
                else
                {
                    PlaceDoor(map, apartRect.Left, apartRect.Top + apartRect.Height / 2, m_Game.GameTiles.FLOOR_PLANKS, MakeObjWoodenDoor());
                    PlaceDoor(map, apartRect.Right - 1, apartRect.Top + apartRect.Height / 2, m_Game.GameTiles.FLOOR_PLANKS, MakeObjWindow());
                }
            }

            // fill appartments with furniture and items
            // an "apartment" is one big room that fits all the housing roles: bedroom, kitchen and living room.
            foreach (Rectangle apartRect in allApartments)
            {
                // bedroom
                FillHousingRoomContents(map, apartRect, 0);
                // kitchen
                FillHousingRoomContents(map, apartRect, 8);
                // living room
                FillHousingRoomContents(map, apartRect, 5);
            }
#endregion

            ///////////
            // 6. Zone
            ///////////
            Zone zone = MakeUniqueZone("Apartments", b.BuildingRect);
            map.AddZone(zone);
            MakeWalkwayZones(map, b);

            // done
            return true;
        }

        protected virtual bool MakeVanillaHousingBuilding(Map map, Block b) // alpha10.1 pre-alpha10.1 regular houses
        {
            ////////////////////////
            // 0. Check suitability
            ////////////////////////
            if (b.InsideRect.Width < 4 || b.InsideRect.Height < 4)
                return false;

            /////////////////////////////
            // 1. Walkway, floor & walls
            /////////////////////////////
            TileRectangle(map, m_Game.GameTiles.FLOOR_WALKWAY, b.Rectangle);
            TileRectangle(map, m_Game.GameTiles.WALL_BRICK, b.BuildingRect);
            TileFill(map, m_Game.GameTiles.FLOOR_PLANKS, b.InsideRect, (tile, prevmodel, x, y) => tile.IsInside = true);

            ///////////////////////
            // 2. Rooms floor plan
            ///////////////////////
            List<Rectangle> roomsList = new List<Rectangle>();
            MakeRoomsPlan(map, ref roomsList, b.BuildingRect, 5, 5);

            /////////////////
            // 3. Make rooms
            /////////////////
#region
            // alpha10 make some housings floor plan non rectangular by randomly chosing not to place one border room
            // and replace it with a special "outside" room : a garden, a parking lot.
            int iOutsideRoom = -1;
            HouseOutsideRoomType outsideRoom = HouseOutsideRoomType._FIRST;
            if (roomsList.Count >= HOUSE_OUTSIDE_ROOM_NEED_MIN_ROOMS && m_DiceRoller.RollChance(HOUSE_OUTSIDE_ROOM_CHANCE))
            {
                for (; ; )
                {
                    iOutsideRoom = m_DiceRoller.Roll(0, roomsList.Count);
                    Rectangle r = roomsList[iOutsideRoom];
                    if (r.Left == b.BuildingRect.Left || r.Right == b.BuildingRect.Right || r.Top == b.BuildingRect.Top || r.Bottom == b.BuildingRect.Bottom)
                        break;
                }
                outsideRoom = (HouseOutsideRoomType)m_DiceRoller.Roll((int)HouseOutsideRoomType._FIRST, (int)HouseOutsideRoomType._COUNT);
            }

            for (int i = 0; i < roomsList.Count; i++)
            {
                Rectangle roomRect = roomsList[i];
                if (iOutsideRoom == i)
                {
                    // make sure all tiles are marked as outside
                    DoForEachTile(roomRect, (pt) => map.GetTileAt(pt).IsInside = false);

                    // then shrink it properly so we dont overlap with tiles from other rooms and mess things up.
                    if (roomRect.Left != b.BuildingRect.Left)
                    {
                        roomRect.X++;
                        roomRect.Width--;
                    }
                    if (roomRect.Right != b.BuildingRect.Right)
                    {
                        roomRect.Width--;
                    }
                    if (roomRect.Top != b.BuildingRect.Top)
                    {
                        roomRect.Y++;
                        roomRect.Height--;
                    }
                    if (roomRect.Bottom != b.BuildingRect.Bottom)
                    {
                        roomRect.Height--;
                    }

                    // then fill the outside room
                    switch (outsideRoom)
                    {
                        case HouseOutsideRoomType.GARDEN:
                            TileFill(map, m_Game.GameTiles.FLOOR_GRASS, roomRect);
                            DoForEachTile(roomRect,
                                (pos) =>
                                {
                                    if (map.GetTileAt(pos).Model == m_Game.GameTiles.FLOOR_GRASS && m_DiceRoller.RollChance(HOUSE_GARDEN_TREE_CHANCE))
                                        map.PlaceMapObjectAt(MakeObjTree(PARK_TREES[m_DiceRoller.Roll(0, PARK_TREES.Length)]), pos);
                                });
                            break;

                        case HouseOutsideRoomType.PARKING_LOT:
                            TileFill(map, m_Game.GameTiles.FLOOR_ASPHALT, roomRect);
                            DoForEachTile(roomRect,
                                (pos) =>
                                {
                                    if (map.GetTileAt(pos).Model == m_Game.GameTiles.FLOOR_ASPHALT && m_DiceRoller.RollChance(HOUSE_PARKING_LOT_CAR_CHANCE))
                                        map.PlaceMapObjectAt(MakeObjWreckedCar(m_DiceRoller), pos);
                                });
                            break;
                    }
                }
                else
                {
                    MakeHousingRoom(map, roomRect, m_Game.GameTiles.FLOOR_PLANKS, m_Game.GameTiles.WALL_BRICK);
                    FillHousingRoomContents(map, roomRect);
                }
            }

            // once all rooms are done, enclose the outside room
            if (iOutsideRoom != -1)
            {
                Rectangle roomRect = roomsList[iOutsideRoom];
                switch (outsideRoom)
                {
                    case HouseOutsideRoomType.GARDEN:
                        DoForEachTile(roomRect,
                            (pos) =>
                            {
                                if ((pos.X == roomRect.Left || pos.X == roomRect.Right - 1 || pos.Y == roomRect.Top || pos.Y == roomRect.Bottom - 1) && map.GetTileAt(pos).Model == m_Game.GameTiles.FLOOR_GRASS)
                                {
                                    map.RemoveMapObjectAt(pos.X, pos.Y); // make sure trees are removed
                                    // place a picket fence, choosing the appropriate oritentation   //@@MP (Release 7-3)
                                    foreach (Direction dir in Direction.COMPASS_NSEW)
                                    {
                                        Point next = pos + dir;
                                        if (map.IsInBounds(next) && map.GetTileAt(next).Model == m_Game.GameTiles.FLOOR_WALKWAY)
                                        {
                                            if (dir == Direction.N || dir == Direction.S) //if has footpath south or north then use EW
                                                map.PlaceMapObjectAt(MakeObjWoodenFence(GameImages.OBJ_PICKET_FENCE_EW), pos);
                                            else if (dir == Direction.E) //else if has footpath east then use NS_right
                                                map.PlaceMapObjectAt(MakeObjWoodenFence(GameImages.OBJ_PICKET_FENCE_NS_RIGHT), pos);
                                            else if (dir == Direction.W) //else if has footpath west then use NS_left
                                                map.PlaceMapObjectAt(MakeObjWoodenFence(GameImages.OBJ_PICKET_FENCE_NS_LEFT), pos);

                                            break;
                                        }
                                    }
                                }
                            });
                        break;

                    case HouseOutsideRoomType.PARKING_LOT:
                        DoForEachTile(roomRect,
                            (pos) =>
                            {
                                bool isLotEntry = (pos.X == roomRect.Left + roomRect.Width / 2) || (pos.Y == roomRect.Top + roomRect.Height / 2);
                                if (!isLotEntry && ((pos.X == roomRect.Left || pos.X == roomRect.Right - 1 || pos.Y == roomRect.Top || pos.Y == roomRect.Bottom - 1) && map.GetTileAt(pos).Model == m_Game.GameTiles.FLOOR_ASPHALT))
                                {
                                    map.RemoveMapObjectAt(pos.X, pos.Y); // make sure cars are removed
                                    map.PlaceMapObjectAt(MakeObjFence(GameImages.OBJ_CHAINWIRE_FENCE), pos);
                                }
                            });
                        break;
                }
            }
#endregion

            ////////////////////////////
            // 4. Fix door-less building
            ////////////////////////////
#region
            bool hasOutsideDoor = false;
            for (int x = b.BuildingRect.Left; x < b.BuildingRect.Right && !hasOutsideDoor; x++)
                for (int y = b.BuildingRect.Top; y < b.BuildingRect.Bottom && !hasOutsideDoor; y++)
                {
                    if (!map.GetTileAt(x, y).IsInside)
                    {
                        DoorWindow door = map.GetMapObjectAt(x, y) as DoorWindow;
                        if (door != null && !door.IsWindow)
                            hasOutsideDoor = true;
                    }
                }
            if (!hasOutsideDoor)
            {
                // alpha10 list all the exit windows, pick one and replace with a door.

                // list all exit windows
                List<Point> buildingExits = new List<Point>(8);
                for (int x = b.BuildingRect.Left; x < b.BuildingRect.Right; x++)
                    for (int y = b.BuildingRect.Top; y < b.BuildingRect.Bottom; y++)
                    {
                        if (!map.GetTileAt(x, y).IsInside)
                        {
                            DoorWindow window = map.GetMapObjectAt(x, y) as DoorWindow;
                            if (window != null && window.IsWindow)
                            {
                                buildingExits.Add(new Point(x, y));
                            }
                        }
                    }

                // replace an exit window with a door
                if (buildingExits.Count > 0)
                {
                    Point newDoorPos = buildingExits[m_DiceRoller.Roll(0, buildingExits.Count)];
                    map.RemoveMapObjectAt(newDoorPos.X, newDoorPos.Y);
                    map.PlaceMapObjectAt(MakeObjWoodenDoor(), newDoorPos);
                    hasOutsideDoor = true;
                }

                // if we did not found an exit window to replace this is a bug, it should never happen.
                // i'm lazy and assume this never happens and throw an exception.
                if (hasOutsideDoor == false)
                {
                    Logger.WriteLine(Logger.Stage.RUN_MAIN, "ERROR: house has no exit, should never happen; sector@" + map.District.WorldPosition + " house@" + b.BuildingRect);
                    throw new InvalidOperationException("house has not exit, should never happen. read the log.");
                }
            }
#endregion

            ////////////////
            // 5. Basement?
            ////////////////
#region
            if (m_DiceRoller.RollChance(HOUSE_BASEMENT_CHANCE))
            {
                Map basementMap = GenerateHouseBasementMap(map, b);
                m_Params.District.AddUniqueMap(basementMap);
            }
#endregion

            ///////////
            // 6. Zone
            ///////////
            map.AddZone(MakeUniqueZone("Housing", b.BuildingRect));
            MakeWalkwayZones(map, b);

            // Done
            return true;
        }

        protected virtual void MakeSewersMaintenanceBuilding(Map map, bool isSurface, Block b, Map linkedMap, Point exitPosition)
        {
            ///////////////
            // Outer walls.
            ///////////////
            // if sewers dig room.
            if (!isSurface)
                TileFill(map, m_Game.GameTiles.FLOOR_CONCRETE, b.InsideRect);
            // outer walls.
            TileRectangle(map, m_Game.GameTiles.WALL_SEWER, b.BuildingRect);
            // make sure its marked as inside (in case we replace a park for instance)
            for (int x = b.InsideRect.Left; x < b.InsideRect.Right; x++)
                for (int y = b.InsideRect.Top; y < b.InsideRect.Bottom; y++)
                    map.GetTileAt(x, y).IsInside = true;

            //////////////////
            // Entrance door.
            //////////////////
            // pick door side and put tags.
#region
            int doorX, doorY;
            Direction digDirection;
            int sideRoll = m_DiceRoller.Roll(0, 4);
            switch (sideRoll)
            {
                case 0: // north.
                    digDirection = Direction.N;
                    doorX = b.BuildingRect.Left + b.BuildingRect.Width / 2;
                    doorY = b.BuildingRect.Top;

                    map.GetTileAt(doorX - 1, doorY).AddDecoration(GameImages.DECO_SEWERS_BUILDING);
                    map.GetTileAt(doorX + 1, doorY).AddDecoration(GameImages.DECO_SEWERS_BUILDING);
                    break;

                case 1: // south.
                    digDirection = Direction.S;
                    doorX = b.BuildingRect.Left + b.BuildingRect.Width / 2;
                    doorY = b.BuildingRect.Bottom - 1;

                    map.GetTileAt(doorX - 1, doorY).AddDecoration(GameImages.DECO_SEWERS_BUILDING);
                    map.GetTileAt(doorX + 1, doorY).AddDecoration(GameImages.DECO_SEWERS_BUILDING);
                    break;

                case 2: // west.
                    digDirection = Direction.W;
                    doorX = b.BuildingRect.Left;
                    doorY = b.BuildingRect.Top + b.BuildingRect.Height / 2;


                    map.GetTileAt(doorX, doorY - 1).AddDecoration(GameImages.DECO_SEWERS_BUILDING);
                    map.GetTileAt(doorX, doorY + 1).AddDecoration(GameImages.DECO_SEWERS_BUILDING);
                    break;

                case 3: // east.
                    digDirection = Direction.E;
                    doorX = b.BuildingRect.Right - 1;
                    doorY = b.BuildingRect.Top + b.BuildingRect.Height / 2;


                    map.GetTileAt(doorX, doorY - 1).AddDecoration(GameImages.DECO_SEWERS_BUILDING);
                    map.GetTileAt(doorX, doorY + 1).AddDecoration(GameImages.DECO_SEWERS_BUILDING);
                    break;
                default:
                    throw new InvalidOperationException("unhandled roll");
            }
            // add the door.
            PlaceDoor(map, doorX, doorY, m_Game.GameTiles.FLOOR_CONCRETE, MakeObjIronDoor(DoorWindow.STATE_CLOSED));
            BarricadeDoors(map, b.BuildingRect, Rules.BARRICADING_MAX);
#endregion

            /////////////////////////////////
            // Hole/Ladder to sewers/surface.
            /////////////////////////////////
            // add exit.
            map.GetTileAt(exitPosition.X, exitPosition.Y).AddDecoration(isSurface ? GameImages.DECO_SEWER_HOLE : GameImages.DECO_SEWER_LADDER);
            map.SetExitAt(exitPosition, new Exit(linkedMap, exitPosition) { IsAnAIExit = true });

            ///////////////////////////////////////////////////
            // If sewers, dig corridor until we reach a tunnel.
            ///////////////////////////////////////////////////
            if (!isSurface)
            {
                Point digPos = new Point(doorX, doorY) + digDirection;
                while (map.IsInBounds(digPos) && !map.GetTileAt(digPos.X, digPos.Y).Model.IsWalkable)
                {
                    // corridor.
                    map.SetTileModelAt(digPos.X, digPos.Y, m_Game.GameTiles.FLOOR_CONCRETE);
                    // continue digging.
                    digPos += digDirection;
                }
            }

            /////////////////////
            // Furniture & Items.
            /////////////////////
            // bunch of objects near walls with construction items on them.
            int nbTables = m_DiceRoller.Roll(Math.Max(b.InsideRect.Width, b.InsideRect.Height), 2 * Math.Max(b.InsideRect.Width, b.InsideRect.Height));
            for (int i = 0; i < nbTables; i++)
            {
                MapObjectPlaceInGoodPosition(map, b.InsideRect,
                    (pt) => CountAdjWalls(map, pt.X, pt.Y) >= 3 && CountAdjDoors(map, pt.X, pt.Y) == 0,
                    m_DiceRoller,
                    (pt) =>
                    {
                        // add item.
                        map.DropItemAt(MakeShopConstructionItem(), pt);

                        // add object.
                        int roll = m_DiceRoller.Roll(0, 4); //added variety     //@@MP (Release 7-6)
                        switch (roll)
                        {
                            case 0: return MakeObjTable(GameImages.OBJ_TABLE);
                            case 1: return MakeObjBarrels(GameImages.OBJ_BARRELS);
                            case 2: return MakeObjFireBarrel(GameImages.OBJ_EMPTY_BARREL);
                            case 3: return MakeObjWorkbench(GameImages.OBJ_WORKBENCH);
                            default:
                                throw new InvalidOperationException("unhandled roll");
                        }
                    });
            }
            // a generator with torch if lucky.
            MapObjectPlaceInGoodPosition(map, b.InsideRect,
                (pt) => CountAdjWalls(map, pt.X, pt.Y) >= 3 && CountAdjDoors(map, pt.X, pt.Y) == 0,
                m_DiceRoller,
                (pt) =>
                {
                    // add food.
                    map.DropItemAt(MakeItemBigFlashlight(), pt);

                    // add fridge.
                    return MakeObjPowerGenerator(GameImages.OBJ_POWERGEN_OFF, GameImages.OBJ_POWERGEN_ON); //@@MP (Release 6-2)
                });

            ////////////////////////////////////
            // Add the poor maintenance guy/gal.
            ////////////////////////////////////
            Actor poorGuy = CreateNewCivilian(0, 3, 1);
            ActorPlace(m_DiceRoller, b.Rectangle.Width * b.Rectangle.Height, map, poorGuy, b.InsideRect.Left, b.InsideRect.Top, b.InsideRect.Width, b.InsideRect.Height);

            //////////////
            // Make zone.
            //////////////
            map.AddZone(MakeUniqueZone(RogueGame.NAME_SEWERS_MAINTENANCE, b.BuildingRect));

            // Done...
        }

        protected virtual void MakeSubwayStationBuilding(Map map, bool isSurface, Block b, Map linkedMap, Point exitPosition)
        {
            ///////////////
            // Outer walls.
            ///////////////
#region
            // if sewers dig room.
            if (!isSurface)
                TileFill(map, m_Game.GameTiles.FLOOR_CONCRETE, b.InsideRect);
            // outer walls.
            TileRectangle(map, m_Game.GameTiles.WALL_SUBWAY, b.BuildingRect);
            // make sure its marked as inside (in case we replace a park for instance)
            for (int x = b.InsideRect.Left; x < b.InsideRect.Right; x++)
                for (int y = b.InsideRect.Top; y < b.InsideRect.Bottom; y++)
                    map.GetTileAt(x, y).IsInside = true;
#endregion

            ////////////
            // Entrance
            ////////////
#region
            // pick door/corridor side and put tags.
            // if not surface, we must dig toward the rails.
            int entryFenceX, entryFenceY;
            Direction digDirection;
            int sideRoll;
            if (isSurface)
                sideRoll = m_DiceRoller.Roll(0, 4);
            else
                sideRoll = b.Rectangle.Bottom < map.Width / 2 ? 1 : 0;
            switch (sideRoll)
            {
                case 0: // north.
                    digDirection = Direction.N;
                    entryFenceX = b.BuildingRect.Left + b.BuildingRect.Width / 2;
                    entryFenceY = b.BuildingRect.Top;

                    if (isSurface)
                    {
                        map.GetTileAt(entryFenceX - 1, entryFenceY).AddDecoration(GameImages.DECO_SUBWAY_BUILDING);
                        map.GetTileAt(entryFenceX + 1, entryFenceY).AddDecoration(GameImages.DECO_SUBWAY_BUILDING);
                    }
                    break;

                case 1: // south.
                    digDirection = Direction.S;
                    entryFenceX = b.BuildingRect.Left + b.BuildingRect.Width / 2;
                    entryFenceY = b.BuildingRect.Bottom - 1;

                    if (isSurface)
                    {
                        map.GetTileAt(entryFenceX - 1, entryFenceY).AddDecoration(GameImages.DECO_SUBWAY_BUILDING);
                        map.GetTileAt(entryFenceX + 1, entryFenceY).AddDecoration(GameImages.DECO_SUBWAY_BUILDING);
                    }
                    break;

                case 2: // west.
                    digDirection = Direction.W;
                    entryFenceX = b.BuildingRect.Left;
                    entryFenceY = b.BuildingRect.Top + b.BuildingRect.Height / 2;

                    if (isSurface)
                    {
                        map.GetTileAt(entryFenceX, entryFenceY - 1).AddDecoration(GameImages.DECO_SUBWAY_BUILDING);
                        map.GetTileAt(entryFenceX, entryFenceY + 1).AddDecoration(GameImages.DECO_SUBWAY_BUILDING);
                    }
                    break;

                case 3: // east.
                    digDirection = Direction.E;
                    entryFenceX = b.BuildingRect.Right - 1;
                    entryFenceY = b.BuildingRect.Top + b.BuildingRect.Height / 2;

                    if (isSurface)
                    {
                        map.GetTileAt(entryFenceX, entryFenceY - 1).AddDecoration(GameImages.DECO_SUBWAY_BUILDING);
                        map.GetTileAt(entryFenceX, entryFenceY + 1).AddDecoration(GameImages.DECO_SUBWAY_BUILDING);
                    }
                    break;
                default:
                    throw new InvalidOperationException("unhandled roll");
            }
            // add door if surface.
            if (isSurface)
            {
                map.SetTileModelAt(entryFenceX, entryFenceY, m_Game.GameTiles.FLOOR_CONCRETE);
                map.PlaceMapObjectAt(MakeObjGlassDoor(), new Point(entryFenceX, entryFenceY));
            }
#endregion

            ///////////////////////////
            // Stairs to the other map.
            ///////////////////////////
#region
            // add exits.
            for (int ex = exitPosition.X - 1; ex <= exitPosition.X + 1; ex++)
            {
                Point thisExitPos = new Point(ex, exitPosition.Y);
                map.GetTileAt(thisExitPos.X, thisExitPos.Y).AddDecoration(isSurface ? GameImages.DECO_STAIRS_DOWN : GameImages.DECO_STAIRS_UP);
                map.SetExitAt(thisExitPos, new Exit(linkedMap, thisExitPos) { IsAnAIExit = true });
            }
#endregion

            ///////////////////////////////////////////////////
            // If subway : 
            // - dig corridor until we reach the rails.
            // - dig platform and make corridor zone.
            // - add closed iron fences between corridor and platform.
            // - make power room.
            ///////////////////////////////////////////////////
#region
            if (!isSurface)
            {
                // - dig corridor until we reach the rails.
#region
                map.SetTileModelAt(entryFenceX, entryFenceY, m_Game.GameTiles.FLOOR_CONCRETE);
                map.SetTileModelAt(entryFenceX + 1, entryFenceY, m_Game.GameTiles.FLOOR_CONCRETE);
                map.SetTileModelAt(entryFenceX - 1, entryFenceY, m_Game.GameTiles.FLOOR_CONCRETE);
                map.SetTileModelAt(entryFenceX - 2, entryFenceY, m_Game.GameTiles.WALL_STONE);
                map.SetTileModelAt(entryFenceX + 2, entryFenceY, m_Game.GameTiles.WALL_STONE);

                Point digPos = new Point(entryFenceX, entryFenceY) + digDirection;
                while (map.IsInBounds(digPos) && !map.GetTileAt(digPos.X, digPos.Y).Model.IsWalkable)
                {
                    // corridor.
                    map.SetTileModelAt(digPos.X, digPos.Y, m_Game.GameTiles.FLOOR_CONCRETE);
                    map.SetTileModelAt(digPos.X - 1, digPos.Y, m_Game.GameTiles.FLOOR_CONCRETE);
                    map.SetTileModelAt(digPos.X + 1, digPos.Y, m_Game.GameTiles.FLOOR_CONCRETE);
                    map.SetTileModelAt(digPos.X - 2, digPos.Y, m_Game.GameTiles.WALL_STONE);
                    map.SetTileModelAt(digPos.X + 2, digPos.Y, m_Game.GameTiles.WALL_STONE);

                    // continue digging.
                    digPos += digDirection;
                }
#endregion

                // - dig platform and make corridor zone.
#region
                const int platformExtend = 10;
                const int platformWidth = 3;
                Rectangle platformRect;
                int platformLeft = Math.Max(0, b.BuildingRect.Left - platformExtend);
                int platformRight = Math.Min(map.Width - 1, b.BuildingRect.Right + platformExtend);
                int benchesLine;
                if (digDirection == Direction.S)
                {
                    platformRect = Rectangle.FromLTRB(platformLeft, digPos.Y - platformWidth, platformRight, digPos.Y);
                    benchesLine = platformRect.Top;
                    map.AddZone(MakeUniqueZone("corridor", Rectangle.FromLTRB(entryFenceX - 1, entryFenceY, entryFenceX + 1 + 1, platformRect.Top)));
                }
                else
                {
                    platformRect = Rectangle.FromLTRB(platformLeft, digPos.Y + 1, platformRight, digPos.Y + 1 + platformWidth);
                    benchesLine = platformRect.Bottom - 1;
                    map.AddZone(MakeUniqueZone("corridor", Rectangle.FromLTRB(entryFenceX - 1, platformRect.Bottom, entryFenceX + 1 + 1, entryFenceY + 1)));
                }
                TileFill(map, m_Game.GameTiles.FLOOR_CONCRETE, platformRect);

                // - iron benches in platform.
                for (int bx = platformRect.Left; bx < platformRect.Right; bx++)
                {
                    if (CountAdjWalls(map, bx, benchesLine) < 3)
                    {
                        if (CountAdjWalls(map, bx, benchesLine) == 2) //@@MP (Release 7-6)
                            map.PlaceMapObjectAt(MakeObjFireBarrel(GameImages.OBJ_EMPTY_BIN), new Point(bx, benchesLine)); //puts it next to the first benches on the platform side of the gates
                        
                        continue;
                    }
                    map.PlaceMapObjectAt(MakeObjIronBench(GameImages.OBJ_IRON_BENCH), new Point(bx, benchesLine));
                }

                // - platform zone.
                map.AddZone(MakeUniqueZone("platform", platformRect));
#endregion

                // - add closed iron gates between corridor and platform.
#region
                Point ironFencePos;
                if (digDirection == Direction.S)
                    ironFencePos = new Point(entryFenceX, platformRect.Top - 1);
                else
                    ironFencePos = new Point(entryFenceX, platformRect.Bottom);
                map.PlaceMapObjectAt(MakeObjIronGate(GameImages.OBJ_GATE_CLOSED), new Point(ironFencePos.X + 1, ironFencePos.Y));
                map.PlaceMapObjectAt(MakeObjIronGate(GameImages.OBJ_GATE_CLOSED), new Point(ironFencePos.X, ironFencePos.Y));
                map.PlaceMapObjectAt(MakeObjIronGate(GameImages.OBJ_GATE_CLOSED), new Point(ironFencePos.X - 1, ironFencePos.Y));
#endregion

                // - make power room.
                #region
                // access in the corridor, going toward the center of the map.
                Point powerRoomEntry;
                Rectangle powerRoomRect;
                const int powerRoomWidth = 4;
                const int powerRoomHalfHeight = 2;
                if (entryFenceX > map.Width / 2)
                {
                    // west.
                    powerRoomEntry = new Point(entryFenceX - 2, entryFenceY + powerRoomHalfHeight * digDirection.Vector.Y);
                    powerRoomRect = Rectangle.FromLTRB(powerRoomEntry.X - powerRoomWidth, powerRoomEntry.Y - powerRoomHalfHeight, powerRoomEntry.X + 1, powerRoomEntry.Y + powerRoomHalfHeight + 1);
                }
                else
                {
                    // east.
                    powerRoomEntry = new Point(entryFenceX + 2, entryFenceY + powerRoomHalfHeight * digDirection.Vector.Y);
                    powerRoomRect = Rectangle.FromLTRB(powerRoomEntry.X, powerRoomEntry.Y - powerRoomHalfHeight, powerRoomEntry.X + powerRoomWidth, powerRoomEntry.Y + powerRoomHalfHeight + 1);
                }

                // carve power room.
                TileFill(map, m_Game.GameTiles.FLOOR_CONCRETE, powerRoomRect);
                TileRectangle(map, m_Game.GameTiles.WALL_STONE, powerRoomRect);

                // add door with signs.
                PlaceDoor(map, powerRoomEntry.X, powerRoomEntry.Y, m_Game.GameTiles.FLOOR_CONCRETE, MakeObjIronDoor(DoorWindow.STATE_CLOSED));
                map.GetTileAt(powerRoomEntry.X, powerRoomEntry.Y - 1).AddDecoration(GameImages.DECO_POWER_SIGN_BIG);
                map.GetTileAt(powerRoomEntry.X, powerRoomEntry.Y + 1).AddDecoration(GameImages.DECO_POWER_SIGN_BIG);

                // add power generators along wall.
                MapObjectFill(map, powerRoomRect,
                    (pt) =>
                    {
                        if (!map.GetTileAt(pt).Model.IsWalkable)
                            return null;
                        if (CountAdjWalls(map, pt.X, pt.Y) < 3 || CountAdjDoors(map, pt.X, pt.Y) > 0)
                            return null;
                        return MakeObjPowerGenerator(GameImages.OBJ_POWERGEN_OFF, GameImages.OBJ_POWERGEN_ON);
                    });

#endregion
            }
#endregion

            /////////////////////
            // Furniture & Items.
            /////////////////////
            // iron benches in station.
#region
            for (int bx = b.InsideRect.Left; bx < b.InsideRect.Right; bx++)
                for (int by = b.InsideRect.Top; by < b.InsideRect.Bottom; by++)
                {
                    // next to walls and no doors.
                    if (CountAdjWalls(map, bx, by) < 3 || CountAdjDoors(map, bx, by) > 0)
                        continue;

                    // in corners
                    if (CountAdjWalls(map, bx, by) == 5) //@@MP (Release 7-6)               //!IsADoorNSEW(map, bx, by) && 
                    {
                        map.PlaceMapObjectAt(MakeObjFireBarrel(GameImages.OBJ_EMPTY_BIN), new Point(bx, by));
                        continue;
                    }

                    // not on an exit            //@@MP (Release 7-6)
                    if (map.GetExitAt(new Point(bx, by)) != null)
                        continue;

                    // not next to stairs.
                    if (m_Game.Rules.GridDistance(new Point(bx, by), new Point(entryFenceX, entryFenceY)) < 2)
                        continue;

                    // bench.
                    map.PlaceMapObjectAt(MakeObjIronBench(GameImages.OBJ_IRON_BENCH), new Point(bx, by));
                }
#endregion

            /////////////////////////////////////
            // Add subway police guy on surface.
            /////////////////////////////////////
            if (isSurface)
            {
                Actor policeMan = CreateNewPoliceman(0);
                ActorPlace(m_DiceRoller, b.Rectangle.Width * b.Rectangle.Height, map, policeMan, b.InsideRect.Left, b.InsideRect.Top, b.InsideRect.Width, b.InsideRect.Height);
            }

            //////////////
            // Make zone.
            //////////////
            map.AddZone(MakeUniqueZone(RogueGame.NAME_SUBWAY_STATION, b.BuildingRect));
        }
#endregion

#region Rooms
        protected virtual void MakeRoomsPlan(Map map, ref List<Rectangle> list, Rectangle rect, int minRoomsXSize, int minRoomsYSize)// alpha10.1 allow different x and y min size
        {
            ////////////
            // 1. Split
            ////////////
            int splitX, splitY;
            Rectangle topLeft, topRight, bottomLeft, bottomRight;
            QuadSplit(rect, minRoomsXSize, minRoomsYSize, out splitX, out splitY, out topLeft, out topRight, out bottomLeft, out bottomRight);

            ///////////////////
            // 2. Termination?
            ///////////////////
            if (topRight.IsEmpty && bottomLeft.IsEmpty && bottomRight.IsEmpty)
            {
                list.Add(rect);
                return;
            }

            //////////////
            // 3. Recurse
            //////////////
            // always top left.
            MakeRoomsPlan(map, ref list, topLeft, minRoomsXSize, minRoomsYSize);
            // then recurse in non empty quads.
            // we shift and inflante the quads cause we want rooms walls and doors to overlap.
            if (!topRight.IsEmpty)
            {
                topRight.Offset(-1, 0);
                ++topRight.Width;
                MakeRoomsPlan(map, ref list, topRight, minRoomsXSize, minRoomsYSize);
            }
            if (!bottomLeft.IsEmpty)
            {
                bottomLeft.Offset(0, -1);
                ++bottomLeft.Height;
                MakeRoomsPlan(map, ref list, bottomLeft, minRoomsXSize, minRoomsYSize);
            }
            if (!bottomRight.IsEmpty)
            {
                bottomRight.Offset(-1, -1);
                ++bottomRight.Width;
                ++bottomRight.Height;
                MakeRoomsPlan(map, ref list, bottomRight, minRoomsXSize, minRoomsYSize);
            }
        }

        protected virtual void MakeHousingRoom(Map map, Rectangle roomRect, TileModel floor, TileModel wall)
        {
            ////////////////////
            // 1. Floor & Walls
            ////////////////////
            TileFill(map, floor, roomRect);
            TileRectangle(map, wall, roomRect.Left, roomRect.Top, roomRect.Width, roomRect.Height,
                (tile, prevmodel, x, y) =>
                {
                    // if we have a door there, don't put a wall!
                    if (map.GetMapObjectAt(x, y) != null)
                        map.SetTileModelAt(x, y, floor);
                });

            //////////////////////
            // 2. Doors & Windows
            //////////////////////
            int midX = roomRect.Left + roomRect.Width / 2;
            int midY = roomRect.Top + roomRect.Height / 2;
            const int outsideDoorChance = 25;

            PlaceIf(map, midX, roomRect.Top, floor,
                (x, y) => HasNoObjectAt(map, x, y) && IsAccessible(map, x, y) && CountAdjDoors(map, x, y) == 0,
                (x, y) => IsInside(map, x, y) || m_DiceRoller.RollChance(outsideDoorChance) ? MakeObjWoodenDoor() : MakeObjWindow());
            PlaceIf(map, midX, roomRect.Bottom - 1, floor,
                (x, y) => HasNoObjectAt(map, x, y) && IsAccessible(map, x, y) && CountAdjDoors(map, x, y) == 0,
                (x, y) => IsInside(map, x, y) || m_DiceRoller.RollChance(outsideDoorChance) ? MakeObjWoodenDoor() : MakeObjWindow());
            PlaceIf(map, roomRect.Left, midY, floor,
                (x, y) => HasNoObjectAt(map, x, y) && IsAccessible(map, x, y) && CountAdjDoors(map, x, y) == 0,
                (x, y) => IsInside(map, x, y) || m_DiceRoller.RollChance(outsideDoorChance) ? MakeObjWoodenDoor() : MakeObjWindow());
            PlaceIf(map, roomRect.Right - 1, midY, floor,
                (x, y) => HasNoObjectAt(map, x, y) && IsAccessible(map, x, y) && CountAdjDoors(map, x, y) == 0,
                (x, y) => IsInside(map, x, y) || m_DiceRoller.RollChance(outsideDoorChance) ? MakeObjWoodenDoor() : MakeObjWindow());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        /// <param name="roomRect"></param>
        /// <param name="role">-1 roll at random; 0-4 bedroom, 5-7 living room, 8-9 kitchen</param> // FIXME -- room role should be an enum and not hardcoded numbers
        protected virtual void FillHousingRoomContents(Map map, Rectangle roomRect, int role = -1) //@@MP (Release 3), alpha10.1 can force room role [optional param]
        {
            Rectangle insideRoom = new Rectangle(roomRect.Left + 1, roomRect.Top + 1, roomRect.Width - 2, roomRect.Height - 2);

            /* @@MP - general object creation patterns
            kitchen:
                1 table and 1 chair next to it (in middle)
                1 fridge (at edges)
                sink (at edges)    
                stoveoven (at edges)
                shelves (at edges)
                counter (at edges)
                plant (in middle)

            living room:
                1 to 3 tables, each with a chair next to it (in middle)
                1 to 3 drawers (at edges)
                television (in middle)
                armchair (in middle)
                lamp (in middle)
                piano (in middle)

            bedroom:
                1 to 3 beds, each with a night table (at edges)
                1 to 4 wardrobes or drawers (at edges)
                television (anywhere)
                lamp (in middle)
            */

            // Decide room role.
            if (role == -1) // alpha10.1 roll room role if not set
                role = m_DiceRoller.Roll(0, 10);
            switch (role)
            {
                // 1. Bedroom? 0-4 = 50%
#region
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                    {
#region
                        // beds with night tables.
                        int nbBeds = m_DiceRoller.Roll(1, 3);
                        for (int i = 0; i < nbBeds; i++)
                        {
                            MapObjectPlaceInGoodPosition(map, insideRoom,
                                (pt) => CountAdjWalls(map, pt.X, pt.Y) >= 4 && !IsADoorNSEW(map, pt.X, pt.Y), //CountAdjDoors(map, pt.X, pt.Y) <= 1,
                                m_DiceRoller,
                                (pt) =>
                                {
                                    // one night table around with item.
                                    Rectangle adjBedRect = new Rectangle(pt.X - 1, pt.Y - 1, 3, 3);
                                    adjBedRect.Intersect(insideRoom);
                                    MapObjectPlaceInGoodPosition(map, adjBedRect,
                                        (pt2) => pt2 != pt && CountAdjWalls(map, pt2.X, pt2.Y) >= 0 && !IsADoorNSEW(map, pt2.X, pt2.Y),
                                        m_DiceRoller,
                                        (pt2) =>
                                        {
                                            // item.
                                            Item it = MakeRandomBedroomItem();
                                            if (it != null)
                                                map.DropItemAt(it, pt2);

                                            // night table.
                                            return MakeObjNightTable(GameImages.OBJ_NIGHT_TABLE);
                                        });

                                    // bed.
                                    MapObject bed = MakeObjBed(GameImages.OBJ_BED);
                                    return bed;
                                });
                        }

                        // wardrobe/shelves with items //@@MP (Release 3)
                        int nbWardrobeOrDrawer = m_DiceRoller.Roll(1, 3);
                        for (int i = 0; i < nbWardrobeOrDrawer; i++)
                        {
                            MapObjectPlaceInGoodPosition(map, insideRoom,
                                                (pt) => CountAdjWalls(map, pt.X, pt.Y) >= 1 && !IsADoorNSEW(map, pt.X, pt.Y),
                                                m_DiceRoller,
                                                (pt) =>
                                                {
                                                    // item.
                                                    Item it = MakeRandomBedroomItem();
                                                    if (it != null)
                                                        map.DropItemAt(it, pt);

                                                    // wardrobe or drawer
                                                    if (m_DiceRoller.RollChance(50))
                                                        return MakeObjWardrobe(GameImages.OBJ_WARDROBE);
                                                    else
                                                        return MakeObjHouseDrawers(GameImages.OBJ_HOUSE_DRAWERS);
                                                });
                        }

                        //television //@@MP (Release 3)
                        MapObjectPlaceInGoodPosition(map, insideRoom,
                                (pt) => CountAdjWalls(map, pt.X, pt.Y) == 0 && !IsADoorNSEW(map, pt.X, pt.Y),
                                m_DiceRoller,
                                (pt) => MakeObjTelevision(GameImages.OBJ_TELEVISION));

                        //lamp - lowest priority in terms of space //@@MP (Release 3)
                        MapObjectPlaceInGoodPosition(map, insideRoom,
                                (pt) => CountAdjWalls(map, pt.X, pt.Y) == 0 && !IsADoorNSEW(map, pt.X, pt.Y),
                                m_DiceRoller,
                                (pt) => MakeObjStandingLamp(GameImages.OBJ_STANDING_LAMP));

                        break;
#endregion
                    }
#endregion

                // 2. Living room? 5-6-7 = 30%
#region
                case 5:
                case 6:
                case 7:
                    {
#region
                        // tables with chairs.
                        /*int nbTables = m_DiceRoller.Roll(1, 2);
                        for (int i = 0; i < nbTables; i++)
                        {*/
                            MapObjectPlaceInGoodPosition(map, insideRoom,
                                (pt) => CountAdjWalls(map, pt.X, pt.Y) == 0 && !IsADoorNSEW(map, pt.X, pt.Y),
                                m_DiceRoller,
                                (pt) =>
                                {
                                    // items.
                                    for (int ii = 0; ii < HOUSE_LIVINGROOM_ITEMS_ON_TABLE; ii++)
                                    {
                                        Item it = MakeRandomKitchenItem();
                                        if (it != null)
                                            map.DropItemAt(it, pt);
                                    }

                                    // three chairs around //@@MP (Release 3)
                                    Rectangle adjTableRect = new Rectangle(pt.X - 1, pt.Y - 1, 3, 3);
                                    adjTableRect.Intersect(insideRoom);
                                    MapObjectPlaceInGoodPosition(map, adjTableRect,
                                        (pt2) => pt2 != pt && !IsADoorNSEW(map, pt2.X, pt2.Y),
                                        m_DiceRoller,
                                        (pt2) => MakeObjChair(GameImages.OBJ_CHAIR));
                                    MapObjectPlaceInGoodPosition(map, adjTableRect,
                                        (pt2) => pt2 != pt && !IsADoorNSEW(map, pt2.X, pt2.Y),
                                        m_DiceRoller,
                                        (pt2) => MakeObjChair(GameImages.OBJ_CHAIR));
                                    MapObjectPlaceInGoodPosition(map, adjTableRect,
                                        (pt2) => pt2 != pt && !IsADoorNSEW(map, pt2.X, pt2.Y),
                                        m_DiceRoller,
                                        (pt2) => MakeObjChair(GameImages.OBJ_CHAIR));


                                    // table.
                                    MapObject table = MakeObjTable(GameImages.OBJ_TABLE);
                                    return table;
                                });
                        //}

                        //television //@@MP (Release 3)
                        MapObjectPlaceInGoodPosition(map, insideRoom,
                                (pt) => CountAdjWalls(map, pt.X, pt.Y) == 0 && !IsADoorNSEW(map, pt.X, pt.Y),
                                m_DiceRoller,
                                (pt) => MakeObjTelevision(GameImages.OBJ_TELEVISION));

                        //couch //@@MP (Release 3)
                        int nbCouches = m_DiceRoller.Roll(1, 2);
                        for (int i = 0; i < nbCouches; i++)
                        {
                            MapObjectPlaceInGoodPosition(map, insideRoom,
                                                (pt) => CountAdjWalls(map, pt.X, pt.Y) == 0 && !IsADoorNSEW(map, pt.X, pt.Y),
                                                m_DiceRoller,
                                                (pt) => MakeObjCouch(GameImages.OBJ_COUCH));
                        }

                        // drawers.
                        int nbDrawers = m_DiceRoller.Roll(1, 2);
                        for (int i = 0; i < nbDrawers; i++)
                        {
                            MapObjectPlaceInGoodPosition(map, insideRoom,
                                                (pt) => CountAdjWalls(map, pt.X, pt.Y) >= 1 && !IsADoorNSEW(map, pt.X, pt.Y),
                                                m_DiceRoller,
                                                (pt) => MakeObjDrawer(GameImages.OBJ_DRAWER));
                        }

                        //book shelves //@@MP (Release 3)
                        int nbBookShelves = m_DiceRoller.Roll(1, 2);
                        for (int i = 0; i < nbBookShelves; i++)
                        {
                            MapObjectPlaceInGoodPosition(map, insideRoom,
                                            (pt) => CountAdjWalls(map, pt.X, pt.Y) >= 1 && !IsADoorNSEW(map, pt.X, pt.Y),
                                            m_DiceRoller,
                                            (pt) =>
                                            {
                                                // item.
                                                Item it = MakeItemBook(m_DiceRoller);
                                                if (it != null && RogueGame.Options.IsSanityEnabled) //@@MP - added check (Release 7-6)
                                                    map.DropItemAt(it, pt);

                                                // wardrobe or drawer
                                                return MakeObjBookshelves(GameImages.OBJ_BOOK_SHELVES);
                                            });
                        }

                        //lamp - lowest priority in terms of space //@@MP (Release 3)
                        if (m_DiceRoller.RollChance(75))
                        {
                            MapObjectPlaceInGoodPosition(map, insideRoom,
                                (pt) => CountAdjWalls(map, pt.X, pt.Y) == 0 && !IsADoorNSEW(map, pt.X, pt.Y),
                                m_DiceRoller,
                                (pt) => MakeObjStandingLamp(GameImages.OBJ_STANDING_LAMP));
                        }
                        else //@@MP (Release 6-2)
                        {
                            MapObjectPlaceInGoodPosition(map, insideRoom,
                                (pt) => CountAdjWalls(map, pt.X, pt.Y) == 0 && !IsADoorNSEW(map, pt.X, pt.Y),
                                m_DiceRoller,
                                (pt) => MakeObjPiano(GameImages.OBJ_PIANO));
                        }

                        break;
#endregion
                    }
#endregion

                // 3. Kitchen? 8-9 = 20%
#region
                case 8:
                case 9:
                    {
#region
                        // fridge with items
                        MapObjectPlaceInGoodPosition(map, insideRoom,
                                            (pt) => CountAdjWalls(map, pt.X, pt.Y) >= 1 && !IsADoorNSEW(map, pt.X, pt.Y),
                                            m_DiceRoller,
                                            (pt) =>
                                            {
                                                // items.
                                                for (int ii = 0; ii < HOUSE_KITCHEN_ITEMS_IN_FRIDGE; ii++)
                                                {
                                                    Item it = MakeRandomKitchenItem();
                                                    if (it != null)
                                                        map.DropItemAt(it, pt);
                                                }

                                                // fridge
                                                return MakeObjFridge(GameImages.OBJ_FRIDGE);
                                            });

                        //sink //@@MP (Release 3)
                        MapObjectPlaceInGoodPosition(map, insideRoom,
                                            (pt) => CountAdjWalls(map, pt.X, pt.Y) >= 1 && !IsADoorNSEW(map, pt.X, pt.Y),
                                            m_DiceRoller,
                                            (pt) => MakeObjKitchenSink(GameImages.OBJ_KITCHEN_SINK));

                        //stove oven //@@MP (Release 3)
                        MapObjectPlaceInGoodPosition(map, insideRoom,
                                (pt) => CountAdjWalls(map, pt.X, pt.Y) >= 1 && !IsADoorNSEW(map, pt.X, pt.Y),
                                m_DiceRoller,
                                (pt) => MakeObjStoveOven(GameImages.OBJ_STOVEOVEN));

                        //counters //@@MP (Release 3)
                        int nbCounters = m_DiceRoller.Roll(1, 2);
                        for (int i = 0; i < nbCounters; i++)
                        {
                            MapObjectPlaceInGoodPosition(map, insideRoom,
                                                (pt) => CountAdjWalls(map, pt.X, pt.Y) >= 1 && !IsADoorNSEW(map, pt.X, pt.Y),
                                                m_DiceRoller,
                                                (pt) =>
                                                {
                                                    // items.
                                                    for (int ii = 0; ii < HOUSE_KITCHEN_ITEMS_ON_TABLE; ii++)
                                                    {
                                                        Item it = MakeRandomKitchenItem();
                                                        if (it != null)
                                                            map.DropItemAt(it, pt);
                                                    }

                                                    return MakeObjKitchenCounter(GameImages.OBJ_KITCHEN_COUNTER);
                                                });
                        }

                        //shelves //@@MP (Release 3)
                        int nbShelves = m_DiceRoller.Roll(1, 2);
                        for (int i = 0; i < nbShelves; i++)
                        {
                            MapObjectPlaceInGoodPosition(map, insideRoom,
                                                (pt) => CountAdjWalls(map, pt.X, pt.Y) >= 3 && !IsADoorNSEW(map, pt.X, pt.Y),
                                                m_DiceRoller,
                                                (pt) => MakeObjHouseShelves(GameImages.OBJ_HOUSE_SHELVES));
                        }

                        //potted plant //@@MP (Release 3)
                        MapObjectPlaceInGoodPosition(map, insideRoom,
                                (pt) => CountAdjWalls(map, pt.X, pt.Y) == 0 && !IsADoorNSEW(map, pt.X, pt.Y),
                                m_DiceRoller,
                                (pt) => MakeObjPottedPlant(GameImages.OBJ_POTTED_PLANT));

                        // table with item & chair - lowest priority in terms of space //@@MP
                        /*MapObjectPlaceInGoodPosition(map, insideRoom,
                            (pt) => CountAdjWalls(map, pt.X, pt.Y) == 0 && CountAdjDoors(map, pt.X, pt.Y) == 0,
                            m_DiceRoller,
                            (pt) =>
                            {
                                // items.
                                for (int ii = 0; ii < HOUSE_KITCHEN_ITEMS_ON_TABLE; ii++)
                                {
                                    Item it = MakeRandomKitchenItem();
                                    if (it != null)
                                        map.DropItemAt(it, pt);
                                }

                                // one chair around.
                                Rectangle adjTableRect = new Rectangle(pt.X - 1, pt.Y - 1, 3, 3);
                                MapObjectPlaceInGoodPosition(map, adjTableRect,
                                    (pt2) => pt2 != pt && CountAdjDoors(map, pt2.X, pt2.Y) == 0,
                                    m_DiceRoller,
                                    (pt2) => MakeObjChair(GameImages.OBJ_CHAIR));

                                // table.
                                return MakeObjTable(GameImages.OBJ_TABLE);
                            });*/

                        break;
#endregion
                    }

                default:
                    throw new InvalidOperationException("unhandled roll");
#endregion
            }
        }

#endregion

#region Items
        protected Item MakeRandomShopItem(ShopType shop)
        {
            switch (shop)
            {
                case ShopType.CONSTRUCTION:
                    return MakeShopConstructionItem();
                case ShopType.GENERAL_STORE:
                    return MakeShopGeneralItem();
                case ShopType.GROCERY:
                    return MakeShopGroceryItem();
                case ShopType.GUNSHOP:
                    return MakeShopGunshopItem();
                case ShopType.PHARMACY:
                    return MakeShopPharmacyItem();
                case ShopType.SPORTSWEAR:
                    return MakeShopSportsWearItem();
                case ShopType.HUNTING:
                    return MakeHuntingShopItem();
                case ShopType.LIQUOR: //@@MP (Release 4)
                    return MakeShopLiquorItem();
                default:
                    throw new ArgumentOutOfRangeException("shop","unhandled shoptype");
            }
        }

        protected Item MakeRandomMallShopItem(MallShopType shop) //@@MP (Release 7-3)
        {
            switch (shop)
            {
                case MallShopType.BOOKSTORE:
                    if (m_DiceRoller.RollChance(80))
                        return MakeItemBook(m_DiceRoller);
                    else
                        return MakeItemMagazines(m_DiceRoller);
                case MallShopType.LIQUOR:
                    return MakeItemAlcohol();
                case MallShopType.GROCERY:
                    return MakeShopGroceryItem();
                case MallShopType.PHARMACY:
                    return MakeShopPharmacyItem();
                case MallShopType.SPORTING_GOODS:
                    return MakeShopSportsWearItem();
                default:
                    throw new ArgumentOutOfRangeException("shop", "unhandled mallshoptype");
            }
        }

        public Item MakeShopGroceryItem()
        {
            int roll = m_DiceRoller.Roll(0, 3); //@@MP - added vegies and changed roll type (Relase 5-5)
            switch (roll)
            {
                case 0: return MakeItemCannedFood();
                case 1: return MakeItemGroceries();
                case 2: return MakeItemVegetables();
                default:
                    throw new InvalidOperationException("unhandled roll");
            }
        }

        public Item MakeShopPharmacyItem()
        {
            int randomItem = m_DiceRoller.Roll(0, 7);
            switch (randomItem)
            {
                case 0: return MakeItemSmallMedikit();
                case 1: return MakeItemLargeMedikit();
                case 2: return MakeItemPillsSLP();
                case 3: return MakeItemPillsSTA();
                case 4: //@@MP - if Sanity is disabled generate other (minor) pharmacy items instead (Release 1)
                    if (RogueGame.Options.IsSanityEnabled) //@MP - fixed crappy implementation (Release 5-2)
                        return MakeItemPillsSAN();
                    else
                        return MakeItemSmallMedikit();
                case 5: return MakeItemStenchKiller();
                case 6: return Rules.HasAntiviralPills(m_Game.Session.GameMode) ? MakeItemPillsAntiviral() : MakeItemSmallMedikit(); //@@MP - re-worked (Release 7-6)
                default:
                    throw new InvalidOperationException("unhandled roll");
            }
        }

        public Item MakeShopSportsWearItem()
        {
            int roll = m_DiceRoller.Roll(0, 14);

            switch (roll)
            {
                case 0: 
                case 1: return MakeItemHockeyStick();
                case 2: return MakeItemGolfClub();
                case 3: 
                case 4: return MakeItemIronGolfClub();
                case 5: 
                case 6: return MakeItemBaseballBat();
                case 7: return MakeItemSleepingBag(); //@@MP (Release 7-3)
                case 8: 
                case 9: return MakeItemTennisRacket();
                case 10:
                    if (RogueGame.Options.IsSanityEnabled) //@@MP - added check (Release 7-6)
                        return MakeItemMagazines(m_DiceRoller);
                    else
                        return MakeItemStenchKiller();
                case 11:
                case 12: return MakeItemGlowsticksBox(); //@@MP (Release 7-1)
                case 13: return MakeItemFishingRod(); //@@MP (Release 7-6)
                default:
                    throw new InvalidOperationException("unhandled roll");
            }
        }

        public Item MakeShopConstructionItem() //@@MP - split each item into its own single roll (Release 3)
        {
            int roll = m_DiceRoller.Roll(0, 13);
            switch (roll)
            {
                case 0: return MakeItemStandardAxe();
                case 1: return MakeItemPipeWrench();
                case 2: return m_DiceRoller.RollChance(50) ? MakeItemShovel() : MakeItemShortShovel();
                case 3: return MakeItemPickaxe();
                case 4: return MakeItemCrowbar();
                case 5: return m_DiceRoller.RollChance(50) ? MakeItemHugeHammer() : MakeItemSmallHammer();
                case 6: return MakeItemSprayPaint();
                case 7: return m_DiceRoller.RollChance(75) ? MakeItemBigFlashlight() : MakeItemFlashlight();
                case 8: return MakeItemBarbedWire();
                case 9: return MakeItemNailGun(); //@@MP (Release 5-1)
                case 10: return MakeItemNailGunAmmo(); //@@MP (Release 5-1)
                case 11: return MakeItemChainsaw(); //@@MP (Release 7-1)
                case 12: return MakeItemPaintThinner(); //@@MP (Release 7-6)
                default:
                    throw new InvalidOperationException("unhandled roll");
            }
        }

        public Item MakeShopGunshopItem()
        {
            // Weapons (40%) vs Ammo (60%)
            if (m_DiceRoller.RollChance(40))
            {
                int roll = m_DiceRoller.Roll(0, 7); //@@MP (Release 3)

                switch (roll)
                {
                    case 0: 
                    case 1: return MakeItemRandomPistol();
                    case 2: 
                    case 3: return MakeItemShotgun();
                    case 4:
                    case 5: return MakeItemHuntingRifle();
                    case 6: return MakeItemDoubleBarrel(); //@@MP (Release 7-6)
                    default:
                        return null;
                }
            }
            else
            {
                int roll = m_DiceRoller.Roll(0, 7);

                switch (roll)
                {
                    case 0: 
                    case 1: 
                    case 2: return MakeItemLightPistolAmmo();
                    case 3: 
                    case 4: return MakeItemShotgunAmmo();
                    case 5: 
                    case 6: return MakeItemLightRifleAmmo();
                    default:
                        return null;
                }
            }
        }

        public Item MakeHuntingShopItem()
        {
            // Weapons/Ammo (60%) Outfits&Traps (40%)
            if (m_DiceRoller.RollChance(60)) //@@MP (Release 3)
            {
                // Weapons(40) Ammo(60)
                if (m_DiceRoller.RollChance(40))
                {
                    int roll = m_DiceRoller.Roll(0, 4);

                    switch (roll)
                    {
                        case 0: return MakeItemHuntingRifle();
                        case 1: return MakeItemHuntingCrossbow();
                        case 2: return MakeItemFishingRod(); //@@MP (Release 7-6)
                        case 3: return MakeItemHunterVest(); //@@MP (Release 7-6)
                        default:
                            return null;
                    }
                }
                else
                {
                    int roll = m_DiceRoller.Roll(0, 4);

                    switch (roll)
                    {
                        case 0: return MakeItemLightRifleAmmo();
                        case 1: return MakeItemBoltsAmmo();
                        case 2: return MakeItemCombatKnife(); //@@MP (Release 3)
                        case 3: return MakeItemHunterVest(); //@@MP (Release 7-6)
                        default:
                            return null;
                    }
                }
            }
            else
            {
                // Outfits&Traps
                int roll = m_DiceRoller.Roll(0, 4);
                switch (roll)
                {
                    case 0: return MakeItemHunterVest();
                    case 1: return MakeItemBearTrap();
                    case 2: return MakeItemBinoculars(); //@@MP added (Release 7-1)
                    case 3: return MakeItemStenchKiller(); //@@MP added (Release 1)
                    default: 
                        return null;
                }
            }
        }

        public Item MakeShopGeneralItem()
        {
            int roll = m_DiceRoller.Roll(0, 9);
            switch (roll)
            { //@@MP - changed the items to be more realistic (Release 4)
                case 0: return MakeShopPharmacyItem();
                case 1: 
                case 2: return MakeItemSnackBar(); //@@MP - was torch (Release 7-1)
                case 3: return MakeShopGroceryItem();
                case 4:
                    if (RogueGame.Options.IsSanityEnabled) //@@MP - added check (Release 7-6)
                        return MakeItemMagazines(m_DiceRoller);
                    else
                        return MakeItemSmallMedikit();
                case 5:
                    if (RogueGame.Options.IsSanityEnabled) //@@MP - added check (Release 7-6)
                        return MakeItemCigarettes();
                    else
                        return MakeItemStenchKiller();
                case 6: return MakeItemCellPhone(); //@MP - added more phones, because they were too rare (Release 5-4)
                case 7: return MakeItemEnergyDrink(); //@@MP (Release 7-1)
                case 8: return MakeItemMatches(); //@@MP (Release 7-6)
                default: 
                    throw new InvalidOperationException("unhandled roll");
            }
        }

        public Item MakeHospitalItem()
        {
            int randomItem = m_DiceRoller.Roll(0, 8);
            switch (randomItem)
            {
                case 0: 
                case 1: return MakeItemSmallMedikit();
                case 2:
                case 3: return MakeItemLargeMedikit();
                case 4: return MakeItemPillsSLP();
                case 5: return MakeItemPillsSTA();
                case 6: //@@MP - if Sanity is disabled generate other (minor) hospital items instead (Release 1)
                    if (RogueGame.Options.IsSanityEnabled) //@MP - fixed crappy implementation (Release 5-2)
                        return MakeItemPillsSAN();
                    else
                        return MakeItemLargeMedikit();
                case 7: return Rules.HasAntiviralPills(m_Game.Session.GameMode) ? MakeItemPillsAntiviral() : MakeItemSmallMedikit(); //@MP - handles new antiviral pills option (Release 5-2)

                default:
                    throw new InvalidOperationException("unhandled roll");
            }
        }
        
        public Item MakeRandomBedroomItem()
        {
            int randomItem = m_DiceRoller.Roll(0, 20);

            switch (randomItem)
            {
                case 0: 
                case 1: return MakeItemSmallMedikit();
                case 2: return MakeItemCandlesBox();
                case 3:
                    if (RogueGame.Options.IsSanityEnabled) //@MP - fixed crappy implem (Release 5-2)
                        return MakeItemPillsSAN();
                    else
                        return MakeItemPillsSLP();
                case 4: return MakeItemTennisRacket(); //@@MP (Release 3)
                case 5: return MakeItemIronGolfClub(); //@@MP (Release 3)
                case 6: return MakeItemBaseballBat();
                case 7: return MakeItemRandomPistol();
                case 8: // rare fire weapon
                        if (m_DiceRoller.RollChance(50))
                            return MakeItemShotgun();
                        else
                            return MakeItemHuntingRifle();
                case 9: 
                case 10:
                case 11: return MakeItemCellPhone();
                case 12: return MakeItemFlashlight();
                case 13: return MakeItemHockeyStick(); //@@MP (Release 3)
                case 14: 
                case 15: return MakeItemStenchKiller();
                case 16: return MakeItemCigarettes(); //@@MP (Release 4)
                case 17:
                    if (RogueGame.Options.IsSanityEnabled) //@@MP - added check (Release 7-6)
                    {
                        if (m_DiceRoller.RollChance(25))
                            return MakeItemBook(m_DiceRoller);
                        else
                            return MakeItemMagazines(m_DiceRoller);
                    }
                    else
                        return MakeItemLargeMedikit();
                case 18:
                    if (m_DiceRoller.RollChance(10))
                        return MakeItemNunchaku();
                    else
                        return MakeItemHunterVest();
                case 19:
                    if (m_DiceRoller.RollChance(15))
                        return MakeItemFishingRod();
                    else
                        return MakeItemBigFlashlight();

                default: throw new InvalidOperationException("unhandled roll");
            }
        }

        public Item MakeRandomKitchenItem()
        {
            int roll = m_DiceRoller.Roll(0, 10);
            switch (roll)
            {
                case 0:
                case 1: return MakeItemCannedFood();
                case 2:
                case 3: return MakeItemGroceries();
                case 4:
                case 5: return MakeItemVegetables(); //@@MP - added vegies and changed roll type (Relase 5-5)
                case 6: return MakeItemFryingPan(); //@@MP (Release 7-6)
                case 7: return MakeItemCleaver(); //@@MP (Release 7-6)
                case 8: return MakeItemMatches(); //@@MP (Release 7-6)
                case 9: return MakeItemKitchenKnife(); //@@MP (Release 7-6)
                default:
                    throw new InvalidOperationException("unhandled roll");
            }
        }

        public Item MakeRandomCHAROfficeItem()
        {
            int randomItem = m_DiceRoller.Roll(0, 11);
            switch (randomItem)
            {
                case 0:
                    // weapons:
                    if (m_DiceRoller.RollChance(20))
                        return MakeItemRandomPistol();
                    else
                    {
                        if (m_DiceRoller.RollChance(30))
                            return MakeItemShotgun();
                        else
                            return MakeItemShotgunAmmo();
                    }

                case 1: 
                case 2:
                    if (Rules.HasAntiviralPills(m_Game.Session.GameMode)) //MP - switched from m_DiceRoller.RollChance(50) to pills unless they are disabled (Release 5-2)
                        return MakeItemPillsAntiviral();
                    else
                        return MakeItemLargeMedikit();
                case 3:
                case 4: // rare tracker items //@@MP - was previously only 50% to drop either (Release 3)
                    if (m_DiceRoller.RollChance(75))
                        return MakeItemZTracker();
                    else
                        return MakeItemBlackOpsGPS();
                case 5: return MakeItemMatches();

                default: return null; // 50% chance to find nothing.
            }
        }

        public Item MakeRandomOrdinaryOfficeItem() //@@MP - plain, non-CHAR (Release 7-3)
        {
            int randomItem = m_DiceRoller.Roll(0, 11);
            switch (randomItem)
            {
                case 0:
                case 1:
                    if (m_DiceRoller.RollChance(50))
                        return MakeItemEnergyDrink();
                    else
                        return MakeItemPillsSTA();
                case 2:
                case 3: return MakeItemSnackBar();
                case 4: return MakeItemCellPhone();
                case 5: return MakeItemMatches();

                default: return null; // 50% chance to find nothing.
            }
        }

        public Item MakeRandomParkItem()
        {
            int randomItem = m_DiceRoller.Roll(0, 7);
            switch (randomItem)
            {
                case 0: return MakeItemSprayPaint();
                case 1: return MakeItemBaseballBat();
                case 2: return MakeItemCannedFood();
                case 3:
                    if (RogueGame.Options.IsSanityEnabled) //@@MP - added check (Release 7-6)
                    {
                        if (m_DiceRoller.RollChance(50))
                            return MakeItemBook(m_DiceRoller);
                        else
                            return MakeItemMagazines(m_DiceRoller);
                    }
                    else
                        return MakeItemFishingRod();
                case 4: return MakeItemFlashlight();
                case 5:
                    if (RogueGame.Options.IsSanityEnabled) //@@MP - added check (Release 7-6)
                        return MakeItemCigarettes();
                    else
                        return MakeItemSleepingBag();
                case 6: return MakeItemCellPhone();
                default: throw new InvalidOperationException("Out of Range: unhandled item roll");
            }
        }

        public Item MakeShopLiquorItem() //@@MP (Release 4)
        {
            if (m_DiceRoller.RollChance(97))
                return MakeItemAlcohol();
            else
                return MakeItemMatches();
        }

        public Item MakeJunkyardItem() //@@MP (Release 7-6)
        {
            int roll = m_DiceRoller.Roll(0, 15);
            switch (roll)
            {
                case 0: return MakeItemPipeWrench();
                case 1: return MakeItemCrowbar();
                case 2: return MakeItemHugeHammer();
                case 3: return MakeItemSprayPaint();
                case 4: return MakeItemBigFlashlight();
                case 5: return MakeItemSpikes();
                case 6: return MakeItemShotgun();
                case 7: return MakeItemSiphonKit();
                case 8: return MakeItemPaintThinner();
                case 9:
                case 10:
                case 11:
                case 12:
                case 13:
                case 14: return MakeItemBarbedWire();
                default:
                    throw new InvalidOperationException("unhandled roll");
            }
        }

        public Item MakeFarmShedItem() //@@MP (Release 7-6)
        {
            int roll = m_DiceRoller.Roll(0, 12);
            switch (roll)
            {
                case 0: return MakeItemShovel();
                case 1: return MakeItemPickaxe();
                case 2: return MakeItemChainsaw();
                case 3: return MakeItemPitchFork();
                case 4: return MakeItemScythe();
                case 5: 
                case 6: 
                case 7:
                case 8:
                case 9: return MakeItemVegetableSeeds();
                case 10: return MakeItemMachete();
                case 11:
                    if (m_DiceRoller.RollChance(15))
                        return MakeItemFishingRod();
                    else
                        return MakeItemVegetableSeeds();
                default:
                    throw new InvalidOperationException("unhandled roll");
            }
        }
#endregion

#region Decorations

        static readonly string[] POSTERS = { GameImages.DECO_POSTERS1, GameImages.DECO_POSTERS2 };
        protected virtual void DecorateOutsideWallsWithPosters(Map map, Rectangle rect, int chancePerWall)
        {
            DecorateOutsideWalls(map, rect,
                (x, y) =>
                {
                    if (m_DiceRoller.RollChance(chancePerWall))
                    {
                        return POSTERS[m_DiceRoller.Roll(0, POSTERS.Length)];
                    }
                    else
                        return null;
                });
        }

        static readonly string[] TAGS = { GameImages.DECO_TAGS1, GameImages.DECO_TAGS2, GameImages.DECO_TAGS3, GameImages.DECO_TAGS4, GameImages.DECO_TAGS5, GameImages.DECO_TAGS6, GameImages.DECO_TAGS7 };

        protected virtual void DecorateOutsideWallsWithTags(Map map, Rectangle rect, int chancePerWall)
        {
            DecorateOutsideWalls(map, rect,
                (x, y) =>
                {
                    if (m_DiceRoller.RollChance(chancePerWall))
                    {
                        return TAGS[m_DiceRoller.Roll(0, TAGS.Length)];
                    }
                    else
                        return null;
                });
        }
#endregion

#region Populating special buildings
        protected virtual void PopulateCHAROfficeBuilding(Map map, Block b)
        {
            // Guards
            for (int i = 0; i < MAX_CHAR_GUARDS_PER_OFFICE; i++)
            {
                Actor newGuard = CreateNewCHARGuard(0);
                newGuard.Inventory.AddAll(MakeItemShotgun());
                newGuard.Inventory.AddAll(MakeItemShotgunAmmo());
                newGuard.Inventory.AddAll(MakeItemShotgunAmmo());
                newGuard.Inventory.AddAll(MakeItemShotgunAmmo());
                ActorPlace(m_DiceRoller, 100, map, newGuard, b.InsideRect.Left, b.InsideRect.Top, b.InsideRect.Width, b.InsideRect.Height);
            }

        }

        protected virtual void PopulateArmyOfficeBuilding(Map map, Block b) //@@MP (Release 6-3)
        {
            // undead
            for (int i = 0; i < 8; i++)
            {
                Actor newUndead = MakeZombified(null, CreateNewArmyNationalGuard(0, "Private"),0);
                ActorPlace(m_DiceRoller, 100, map, newUndead, b.InsideRect.Left, b.InsideRect.Top, b.InsideRect.Width, b.InsideRect.Height);
            }

        }
#endregion

#region Special Locations

#region House Basement
        Map GenerateHouseBasementMap(Map map, Block houseBlock)
        {
            // make map.
#region
            Rectangle rect = houseBlock.BuildingRect;
            int seed = map.Seed << 1 + rect.Left * map.Height + rect.Top;
            Map basement = new Map(seed, String.Format("basement{0}{1}@{2}-{3}", m_Params.District.WorldPosition.X, m_Params.District.WorldPosition.Y, rect.Left + rect.Width / 2, rect.Top + rect.Height / 2), rect.Width, rect.Height)
            {
                Lighting = Lighting.DARKNESS
            };
            basement.AddZone(MakeUniqueZone("basement", basement.Rect));
#endregion

            // enclose.
#region
            TileFill(basement, m_Game.GameTiles.FLOOR_CONCRETE, (tile, model, x, y) => tile.IsInside = true);
            TileRectangle(basement, m_Game.GameTiles.WALL_BRICK, new Rectangle(0, 0, basement.Width, basement.Height));
#endregion

            // link to house with stairs.
#region
            Point surfaceStairs = new Point();
            for (; ; )
            {
                // roll.
                surfaceStairs.X = m_DiceRoller.Roll(rect.Left, rect.Right);
                surfaceStairs.Y = m_DiceRoller.Roll(rect.Top, rect.Bottom);

                // valid if walkable, inside & no blocking object.
                if (!map.GetTileAt(surfaceStairs.X,surfaceStairs.Y).Model.IsWalkable)
                    continue;
                if (map.GetMapObjectAt(surfaceStairs.X, surfaceStairs.Y) != null)
                    continue;
                if (!map.GetTileAt(surfaceStairs.X, surfaceStairs.Y).IsInside) //alpha 10
                    continue;

                // good post.
                break;
            }
            Point basementStairs = new Point(surfaceStairs.X - rect.Left, surfaceStairs.Y - rect.Top);
            AddExit(map, surfaceStairs, basement, basementStairs, GameImages.DECO_STAIRS_DOWN, true);
            AddExit(basement, basementStairs, map, surfaceStairs, GameImages.DECO_STAIRS_UP, true);
#endregion

            // random pillars/walls.
#region
            DoForEachTile(basement.Rect, //@@MP - unused parameter (Release 5-7)
                (pt) =>
                {
                    if (!m_DiceRoller.RollChance(HOUSE_BASEMENT_PILAR_CHANCE))
                        return;
                    if (pt == basementStairs)
                        return;
                    basement.SetTileModelAt(pt.X, pt.Y, m_Game.GameTiles.WALL_BRICK);
                });
#endregion

            // fill with home furniture/crap and items.
#region
            MapObjectFill(basement, basement.Rect,
                (pt) =>
                {
                    if (!m_DiceRoller.RollChance(HOUSE_BASEMENT_OBJECT_CHANCE_PER_TILE))
                        return null;

                    if (basement.GetExitAt(pt) != null)
                        return null;
                    if (!basement.IsWalkable(pt.X, pt.Y))
                        return null;

                    int roll = m_DiceRoller.Roll(0, 11); //@@MP (Release 3)
                    switch (roll)
                    {
                        case 0: // junk
                        case 1:
                            return MakeObjJunk(GameImages.OBJ_JUNK);
                        case 2: // barrels.
                                return MakeObjBarrels(GameImages.OBJ_BARRELS);
                        case 3:
                                return MakeObjFireBarrel(GameImages.OBJ_EMPTY_BARREL); //@@MP (Release 7-6)
                        case 4: // table with random item.
                            {
                                Item it = MakeShopConstructionItem();
                                basement.DropItemAt(it, pt);
                                return MakeObjTable(GameImages.OBJ_TABLE);
                            };
                        case 5: // drawer with random item.
                            {
                                Item it = MakeShopConstructionItem();
                                basement.DropItemAt(it, pt);
                                return MakeObjDrawer(GameImages.OBJ_DRAWER);
                            };
                        case 6: // bed.
                            return MakeObjCouch(GameImages.OBJ_COUCH);
                        case 7:
                        case 8:
                        case 9:
                        case 10: //@@MP (Release 3)
                            {
                                basement.PlaceMapObjectAt(MakeObjShelf(GameImages.OBJ_SHOP_SHELF), pt);
                                basement.DropItemAt(MakeItemCannedFood(), pt);
                                basement.DropItemAt(MakeItemCannedFood(), pt);
                                basement.DropItemAt(MakeItemCannedFood(), pt);
                                basement.DropItemAt(MakeItemCannedFood(), pt);
                                basement.DropItemAt(MakeItemCannedFood(), pt);
                                basement.DropItemAt(MakeItemCannedFood(), pt);
                                basement.DropItemAt(MakeItemCannedFood(), pt);
                                return null;
                            }
                        default:
                            throw new InvalidOperationException("unhandled roll");
                    }
                });
#endregion

            // rats!
#region
            if (Rules.HasZombiesInBasements(m_Game.Session.GameMode))
            {
                DoForEachTile(basement.Rect, //@@MP - unused parameter (Release 5-7)
                    (pt) =>
                    {
                        if (!basement.IsWalkable(pt.X, pt.Y))
                            return;
                        if (basement.GetExitAt(pt) != null)
                            return;

                        if (m_DiceRoller.RollChance(SHOP_BASEMENT_ZOMBIE_RAT_CHANCE))
                            basement.PlaceActorAt(CreateNewBasementRatZombie(0), pt);
                    });
            }
#endregion

            // weapons cache?
#region
            if (m_DiceRoller.RollChance(HOUSE_BASEMENT_WEAPONS_CACHE_CHANCE))
            {
                MapObjectPlaceInGoodPosition(basement, basement.Rect,
                    (pt) =>
                    {
                        if (basement.GetExitAt(pt) != null)
                            return false;
                        if (!basement.IsWalkable(pt.X, pt.Y))
                            return false;
                        if (basement.GetMapObjectAt(pt) != null)
                            return false;
                        if (basement.GetItemsAt(pt) != null)
                            return false;
                        return true;
                    },
                    m_DiceRoller,
                    (pt) =>
                    {
                        // two grenades...
                        basement.DropItemAt(MakeItemGrenade(), pt);
                        basement.DropItemAt(MakeItemGrenade(), pt);

                        // and a handfull of gunshop items.
                        for (int i = 0; i < 5; i++)
                        {
                            Item it = MakeShopGunshopItem();
                            basement.DropItemAt(it, pt);
                        }

                        // shelf.
                        MapObject shelf = MakeObjShelf(GameImages.OBJ_SHOP_SHELF);
                        return shelf;
                    });
            }
#endregion

            // music.  // alpha10
            basement.BgMusic = null; //@@MP - was Subway (Release 7-4)

            // done.
            return basement;
        }
#endregion

#region CHAR Underground Facility
        public Map GenerateUniqueMap_CHARUnderground(Map surfaceMap, Zone officeZone, int mapSize, out Point baseEntryPos) 
            //alpha 10, added baseEntryPos- it's used to mark the CHAR build in City Info
            //@@MP - added mapSize so we use the same district size as the player chooses (Release 7-4)
        {
            /////////////////////////
            // 1. Create basic secret map.
            // 2. Link to office.
            // 3. Create rooms.
            // 4. Furniture & Items.
            // 5. Posters & Blood.
            // 6. Populate.
            // 7. Add uniques.
            // 8. Music.  // alpha10
            /////////////////////////

            // 1. Create basic secret map.
#region
            // huge map.
            Map underground = new Map((surfaceMap.Seed << 3) ^ surfaceMap.Seed, "CHAR Underground Facility", mapSize, mapSize)
            {
                Lighting = Lighting.DARKNESS,
                IsSecret = true
            };
            // fill & enclose.
            TileFill(underground, m_Game.GameTiles.FLOOR_OFFICE, (tile, model, x, y) => tile.IsInside = true);
            TileRectangle(underground, m_Game.GameTiles.WALL_CHAR_OFFICE, new Rectangle(0, 0, underground.Width, underground.Height));
#endregion

            // 2. Link to office.
#region
            // find surface point in office:
            // - in a random office room.
            // - set exit somewhere walkable inside.
            // - iron door, barricade the door.
            //Zone roomZone = null;
            Point surfaceExit = new Point();
            while (true)    // loop until found.
            {
                /*   //@@MP (Release 7-6)
                // find a random room.
                do
                {
                    int x = m_DiceRoller.Roll(officeZone.Bounds.Left, officeZone.Bounds.Right);
                    int y = m_DiceRoller.Roll(officeZone.Bounds.Top, officeZone.Bounds.Bottom);
                    List<Zone> zonesHere = surfaceMap.GetZonesAt(x, y);
                    if (zonesHere == null || zonesHere.Count == 0)
                        continue;
                    foreach (Zone z in zonesHere)
                        if (z.Name.Contains("room"))
                        {
                            roomZone = z;
                            break;
                        }
                }
                while (roomZone == null);
                */

                // find somewhere walkable inside.
                bool foundSurfaceExit = false;
                int attempts = 0;
                do
                {
                    surfaceExit.X = m_DiceRoller.Roll(officeZone.Bounds.Left, officeZone.Bounds.Right);
                    surfaceExit.Y = m_DiceRoller.Roll(officeZone.Bounds.Top, officeZone.Bounds.Bottom);
                    foundSurfaceExit = surfaceMap.IsWalkable(surfaceExit.X, surfaceExit.Y);
                    ++attempts;
                }
                while (attempts < 100 && !foundSurfaceExit);

                // failed?
                if (foundSurfaceExit == false)
                    continue;

                // found everything, good!
                break;
            }

            // remember position // alpha10
            baseEntryPos = surfaceExit;

            // barricade the main entry doors.
            DoForEachTile(officeZone.Bounds, //@@MP - unused parameter (Release 5-7)
                (pt) =>
                {
                    DoorWindow door = surfaceMap.GetMapObjectAt(pt) as DoorWindow;
                    if (door == null)
                        return;
                    if (surfaceMap.HasAnyAdjacentInMap(pt, (adj) => surfaceMap.GetTileAt(adj).Model == m_Game.GameTiles.FLOOR_WALKWAY))  //@@MP - previously it barricaded all doors (Release 7-6)
                    {
                        surfaceMap.RemoveMapObjectAt(pt.X, pt.Y);
                        door = MakeObjIronDoor(DoorWindow.STATE_CLOSED);
                        door.BarricadePoints = Rules.BARRICADING_MAX;
                        surfaceMap.PlaceMapObjectAt(door, pt);
                    }
                    else
                        return;
                });

            // stairs.
            // underground : in the middle of the map.
            Point undergroundStairs = new Point(underground.Width / 2, underground.Height / 2);
            underground.SetExitAt(undergroundStairs, new Exit(surfaceMap, surfaceExit));
            underground.GetTileAt(undergroundStairs.X, undergroundStairs.Y).AddDecoration(GameImages.DECO_STAIRS_UP);
            surfaceMap.SetExitAt(surfaceExit, new Exit(underground, undergroundStairs));
            surfaceMap.GetTileAt(surfaceExit.X, surfaceExit.Y).AddDecoration(GameImages.DECO_STAIRS_DOWN);
            // floor logo.
            ForEachAdjacent(underground, undergroundStairs.X, undergroundStairs.Y, (pt) => underground.GetTileAt(pt).AddDecoration(GameImages.DECO_CHAR_FLOOR_LOGO));
#endregion

            // 3. Create floorplan & rooms.
#region
            // make 4 quarters, splitted by a crossed corridor.
            const int corridorHalfWidth = 1;
            Rectangle qTopLeft = Rectangle.FromLTRB(0, 0, underground.Width / 2 - corridorHalfWidth, underground.Height / 2 - corridorHalfWidth);
            Rectangle qTopRight = Rectangle.FromLTRB(underground.Width / 2 + 1 + corridorHalfWidth, 0, underground.Width, qTopLeft.Bottom);
            Rectangle qBotLeft = Rectangle.FromLTRB(0, underground.Height / 2 + 1 + corridorHalfWidth, qTopLeft.Right, underground.Height);
            Rectangle qBotRight = Rectangle.FromLTRB(qTopRight.Left, qBotLeft.Top, underground.Width, underground.Height);

            // split all the map in rooms.
            const int minRoomSize = 6;
            List<Rectangle> roomsList = new List<Rectangle>();
            MakeRoomsPlan(underground, ref roomsList, qBotLeft, minRoomSize, minRoomSize);
            MakeRoomsPlan(underground, ref roomsList, qBotRight, minRoomSize, minRoomSize);
            MakeRoomsPlan(underground, ref roomsList, qTopLeft, minRoomSize, minRoomSize);
            MakeRoomsPlan(underground, ref roomsList, qTopRight, minRoomSize, minRoomSize);

            // make the rooms walls.
            foreach (Rectangle roomRect in roomsList)
            {
                TileRectangle(underground, m_Game.GameTiles.WALL_CHAR_OFFICE, roomRect);
            }

            // add room doors.
            // quarters have door side preferences to lead toward the central corridors.
            foreach (Rectangle roomRect in roomsList)
            {
                Point westEastDoorPos = roomRect.Left < underground.Width / 2 ? 
                    new Point(roomRect.Right - 1, roomRect.Top + roomRect.Height / 2) : 
                    new Point(roomRect.Left, roomRect.Top + roomRect.Height / 2);
                if (underground.GetMapObjectAt(westEastDoorPos) == null)
                {
                    DoorWindow door = MakeObjCharDoor();
                    PlaceDoorIfAccessibleAndNotAdjacent(underground, westEastDoorPos.X, westEastDoorPos.Y, m_Game.GameTiles.FLOOR_OFFICE, 6, door);
                }

                Point northSouthDoorPos = roomRect.Top < underground.Height / 2 ? 
                    new Point(roomRect.Left + roomRect.Width / 2, roomRect.Bottom - 1) : 
                    new Point(roomRect.Left + roomRect.Width / 2, roomRect.Top);
                if (underground.GetMapObjectAt(northSouthDoorPos) == null)
                {
                    DoorWindow door = MakeObjCharDoor();
                    PlaceDoorIfAccessibleAndNotAdjacent(underground, northSouthDoorPos.X, northSouthDoorPos.Y, m_Game.GameTiles.FLOOR_OFFICE, 6, door);
                }
            }

            // add iron doors closing each corridor.
            for (int x = qTopLeft.Right; x < qBotRight.Left; x++)
            {
                PlaceDoor(underground, x, qTopLeft.Bottom - 1, m_Game.GameTiles.FLOOR_OFFICE, MakeObjIronDoor(DoorWindow.STATE_CLOSED));
                PlaceDoor(underground, x, qBotLeft.Top, m_Game.GameTiles.FLOOR_OFFICE, MakeObjIronDoor(DoorWindow.STATE_CLOSED));
            }
            for (int y = qTopLeft.Bottom; y < qBotLeft.Top; y++)
            {
                PlaceDoor(underground, qTopLeft.Right - 1, y, m_Game.GameTiles.FLOOR_OFFICE, MakeObjIronDoor(DoorWindow.STATE_CLOSED));
                PlaceDoor(underground, qTopRight.Left, y, m_Game.GameTiles.FLOOR_OFFICE, MakeObjIronDoor(DoorWindow.STATE_CLOSED));
            }
#endregion

            // 4. Rooms, furniture & items.
#region
            // furniture + items in rooms.
            // room roles with zones:
            // - corners room : Power Room.
            // - top left quarter : armory.
            // - top right quarter : storage.
            // - bottom left quarter : living.
            // - bottom right quarter : pharmacy.
            bool placedBioForceGun = false; //@@MP - a special new weapon. only 1 per game (Release 7-6)
            foreach (Rectangle roomRect in roomsList)
            {
                Rectangle insideRoomRect = new Rectangle(roomRect.Left + 1, roomRect.Top + 1, roomRect.Width - 2, roomRect.Height - 2);
                string roomName = "<noname>";

                // special room?
                // one power room in each corner.
                bool isPowerRoom = (roomRect.Left == 0 && roomRect.Top == 0) ||
                    (roomRect.Left == 0 && roomRect.Bottom == underground.Height) ||
                    (roomRect.Right == underground.Width && roomRect.Top == 0) ||
                    (roomRect.Right == underground.Width && roomRect.Bottom == underground.Height);
                if (isPowerRoom)
                {
                    roomName = "Power Room";
                    MakeCHARPowerRoom(underground, roomRect, insideRoomRect);
                }
                else
                {
                    // common room.
                    int roomRole = (roomRect.Left < underground.Width / 2 && roomRect.Top < underground.Height / 2) ? 0 :
                        (roomRect.Left >= underground.Width / 2 && roomRect.Top < underground.Height / 2) ? 1 :
                        (roomRect.Left < underground.Width / 2 && roomRect.Top >= underground.Height / 2) ? 2 :
                        3;
                    switch (roomRole)
                    {
                        case 0: // armory room.
                            {
                                roomName = "Armory";
                                MakeCHARArmoryRoom(underground, insideRoomRect);
                                break;
                            }
                        case 1: // storage room.
                            {
                                roomName = "Storage";
                                MakeCHARStorageRoom(underground, insideRoomRect);
                                break;
                            }
                        case 2: // living room.
                            {
                                /*roomName = "Living";
                                MakeCHARLivingRoom(underground, insideRoomRect);*/
                                roomName = "Lab"; //@@MP - more thematic (Release 3)
                                MakeCHARLabRoom(underground, insideRoomRect, ref placedBioForceGun);
                                break;
                            }
                        case 3: // pharmacy.
                            {
                                roomName = "Pharmacy";
                                MakeCHARPharmacyRoom(underground, insideRoomRect);
                                break;
                            }
                        default:
                            throw new InvalidOperationException("unhandled role");
                    }
                }

                underground.AddZone(MakeUniqueZone(roomName, insideRoomRect));
            }
#endregion

            // 5. Posters & Blood.
#region
            // char propaganda posters & blood almost everywhere.
            for(int x = 0; x < underground.Width;x++)
                for (int y = 0; y < underground.Height; y++)
                {
                    // poster on wall?
                    if (m_DiceRoller.RollChance(25))
                    {
                        Tile tile = underground.GetTileAt(x,y);
                        if (tile.Model.IsWalkable)
                            continue;
                        tile.AddDecoration(CHAR_POSTERS[m_DiceRoller.Roll(0, CHAR_POSTERS.Length)]);
                    }

                    // large blood?
                    if (m_DiceRoller.RollChance(10)) //@@MP - was 20 (Release 3)
                    {
                        Tile tile = underground.GetTileAt(x, y);
                        if (tile.Model.IsWalkable)
                            tile.AddDecoration(GameImages.DECO_BLOODIED_FLOOR);
                        else
                            tile.AddDecoration(GameImages.DECO_BLOODIED_WALL);
                    }
                    else if (m_DiceRoller.RollChance(20)) // small blood? //@@MP (Release 3)
                    {
                        Tile tile = underground.GetTileAt(x, y);
                        if (tile.Model.IsWalkable)
                            tile.AddDecoration(GameImages.DECO_BLOODIED_FLOOR_SMALL);
                        else
                            tile.AddDecoration(GameImages.DECO_BLOODIED_WALL_SMALL);
                    }
                }
#endregion

            // 6. Populate.
            // don't block exits!
#region
            // leveled up undeads!
            int nbZombies = underground.Width;  // 100 for 100.
            for (int i = 0; i < nbZombies; i++)
            {
                Actor undead = CreateNewUndead(0);
                for (; ; )
                {
                    GameActors.IDs upID = m_Game.NextUndeadEvolution((GameActors.IDs)undead.Model.ID);
                    if (upID == (GameActors.IDs) undead.Model.ID)
                        break;
                    undead.Model = m_Game.GameActors[upID];
                }
                ActorPlace(m_DiceRoller, underground.Width * underground.Height, underground, undead, (pt) => underground.GetExitAt(pt) == null);
            }         
   
            // CHAR scientists.
            int nbScientists = underground.Width / 10; // 10 for 100.
            for (int i = 0; i < nbScientists; i++)
            {
                Actor guard = CreateNewCHARScientist(0);
                ActorPlace(m_DiceRoller, underground.Width * underground.Height, underground, guard, (pt) => underground.GetExitAt(pt) == null);
            }
#endregion

            // 7. Add uniques.
            // TODO... //@@MP - looks like RoguedJack had some plans for a boss or special items
#region
#endregion

            // 8. Music.   // alpha10
            underground.BgMusic = GameMusics.CHAR_UNDERGROUND_FACILITY;

            // done.
            return underground;
        }

        void MakeCHARArmoryRoom(Map map, Rectangle roomRect)
        {
            // Shelves with weapons/ammo along walls.
            MapObjectFill(map, roomRect,
                (pt) =>
                {
                    if (CountAdjWalls(map, pt.X, pt.Y) < 2)
                        return null;
                    // don't block doors
                    if (IsADoorNSEW(map, pt.X, pt.Y)) //@@MP (Release 7-6)
                        return null;
                    // dont block exits!
                    if (map.GetExitAt(pt) != null)
                        return null;

                    // shelf + tracker/armor/weapon.
                    if (m_DiceRoller.RollChance(GameOptions.ResourcesAvailabilityToInt(RogueGame.Options.ResourcesAvailability))) //@@MP - Resources Availability option (Release 7-4)
                    {
                        Item it;
                        int bioforceAmmoChance = GameOptions.ResourcesAvailabilityToInt(RogueGame.Options.ResourcesAvailability);
                        bioforceAmmoChance = (int)bioforceAmmoChance / 3;
                        if (m_DiceRoller.RollChance(bioforceAmmoChance)) //@@MP - added (Release 7-6)
                            it = MakeItemBioForceGunAmmo();
                        else if (m_DiceRoller.RollChance(20))
                            it = MakeItemCHARLightBodyArmor();
                        else if (m_DiceRoller.RollChance(20))
                        {
                            it = m_DiceRoller.RollChance(50) ? MakeItemZTracker() : MakeItemBlackOpsGPS();
                        }
                        else
                        {
                            // rare grenades.
                            if (m_DiceRoller.RollChance(20))
                            {
                                it = MakeItemGrenade();
                            }
                            else
                            {
                                // weapon vs ammo.
                                if (m_DiceRoller.RollChance(20)) //@@MP - was 30 (Release 7-6)
                                {
                                    it = m_DiceRoller.RollChance(50) ? MakeItemTacticalShotgun() : MakeItemPrecisionRifle();
                                }
                                else
                                {
                                    it = m_DiceRoller.RollChance(50) ? MakeItemShotgunAmmo() : MakeItemPrecisionRifleAmmo();
                                }
                            }
                        }
                        map.DropItemAt(it, pt);
                    }

                    MapObject shelf = MakeObjShelf(GameImages.OBJ_SHOP_SHELF);
                    return shelf;
                });
        }

        void MakeCHARStorageRoom(Map map, Rectangle roomRect)
        {
            // Replace floor with concrete.
            TileFill(map, m_Game.GameTiles.FLOOR_CONCRETE, roomRect);

            // Objects.
            // Barrels & Junk in the middle of the room.
            MapObjectFill(map, roomRect,
                (pt) =>
                {
                    if (CountAdjWalls(map, pt.X, pt.Y) > 0)
                        return null;
                    // dont block exits!
                    if (map.GetExitAt(pt) != null)
                        return null;

                    // barrels/junk?
                    if (m_DiceRoller.RollChance(47))
                        return m_DiceRoller.RollChance(50) ? MakeObjJunk(GameImages.OBJ_JUNK) : MakeObjBarrels(GameImages.OBJ_BARRELS);
                    else if (m_DiceRoller.RollChance(3)) //@@MP (Release 7-6)
                        return MakeObjFireBarrel(GameImages.OBJ_EMPTY_BARREL);
                    else
                    {
                        if (m_DiceRoller.RollChance(GameOptions.ResourcesAvailabilityToInt(RogueGame.Options.ResourcesAvailability))) //@@MP - Resources Availability option (Release 7-4)
                            map.DropItemAt(MakeItemCannedFood(), pt);
                        return null;
                    }
                });

            // Items.
            // Construction items in this mess.
            for(int x = roomRect.Left; x < roomRect.Right;x++)
                for (int y = roomRect.Top; y < roomRect.Bottom; y++)
                {
                    if (CountAdjWalls(map, x, y) > 0)
                        continue;
                    if(map.GetMapObjectAt(x,y) != null)
                        continue;

                    if (m_DiceRoller.RollChance(GameOptions.ResourcesAvailabilityToInt(RogueGame.Options.ResourcesAvailability))) //@@MP - Resources Availability option (Release 7-4)
                        map.DropItemAt(MakeShopConstructionItem(), x, y);
                }
        }

        void MakeCHARLabRoom(Map map, Rectangle roomRect, ref bool placedBioForceGun) //@@MP - added labs to replace CHAR living rooms (Release 3)
        {
            bool placedCHARdocument = false;
            // Replace floor with tiles with painted logo.
            TileFill(map, m_Game.GameTiles.FLOOR_TILES, roomRect);//, (tile, model, x, y) => tile.AddDecoration(GameImages.DECO_CHAR_FLOOR_LOGO));

            // Objects.
            // vats along walls.
            MapObjectFill(map, roomRect,
                (pt) =>
                {
                    if (CountAdjWalls(map, pt.X, pt.Y) < 3)
                        return null;
                    // dont block exits!
                    if (map.GetExitAt(pt) != null)
                        return null;

                    // bed/fridge?
                    if (m_DiceRoller.RollChance(50))
                    {
                        if (m_DiceRoller.RollChance(75))
                            return MakeObjCHARvat(GameImages.OBJ_CHAR_VAT); 
                        else
                            return MakeObjWorkstation(GameImages.OBJ_CHAR_DESKTOP);
                    }
                    else
                        return null;
                });
            // desktops and tables in the middle of the room
            MapObjectFill(map, roomRect,
                (pt) =>
                {
                    if (CountAdjWalls(map, pt.X, pt.Y) > 0)
                        return null;
                    // dont block exits!
                    if (map.GetExitAt(pt) != null)
                        return null;

                    // tables/chairs.
                    if (m_DiceRoller.RollChance(30))
                    {
                        if (m_DiceRoller.RollChance(50))
                            return MakeObjWorkstation(GameImages.OBJ_CHAR_DESKTOP);
                        else
                        {
                            int armorChance = GameOptions.ResourcesAvailabilityToInt(RogueGame.Options.ResourcesAvailability);
                            //armorChance = (int)armorChance / 3;
                            if (m_DiceRoller.RollChance(armorChance))
                                map.DropItemAt(MakeItemBiohazardSuit(), pt); //@@MP - added (Release 7-6)
                            return MakeObjTable(GameImages.OBJ_CHAR_TABLE);
                        }
                    }
                    else
                    {
                        if (!placedCHARdocument)
                        {
                            Item it = null;
                            int roll = m_DiceRoller.Roll(0, 5);
                            switch (roll)
                            {
                                case 0:
                                    it = new Item(m_Game.GameItems.UNIQUE_CHAR_DOCUMENT1) { IsUnique = true, IsForbiddenToAI = true };
                                    break;
                                case 1:
                                    it = new Item(m_Game.GameItems.UNIQUE_CHAR_DOCUMENT2) { IsUnique = true, IsForbiddenToAI = true };
                                    break;
                                case 2:
                                    it = new Item(m_Game.GameItems.UNIQUE_CHAR_DOCUMENT3) { IsUnique = true, IsForbiddenToAI = true };
                                    break;
                                case 3:
                                    it = new Item(m_Game.GameItems.UNIQUE_CHAR_DOCUMENT4) { IsUnique = true, IsForbiddenToAI = true };
                                    break;
                                case 4:
                                    it = new Item(m_Game.GameItems.UNIQUE_CHAR_DOCUMENT5) { IsUnique = true, IsForbiddenToAI = true };
                                    break;
                                case 5:
                                    it = new Item(m_Game.GameItems.UNIQUE_CHAR_DOCUMENT6) { IsUnique = true, IsForbiddenToAI = true };
                                    break;
                                default:
                                    throw new InvalidOperationException("unhandled roll");
                            }
                            map.DropItemAt(it, pt);
                            placedCHARdocument = true; //@@MP - only drop one per room
                        }

                        return null;
                    }
                });

            if (!placedBioForceGun) //@@MP - added (Release 7-6)
            {
                bool placed = false;
                MapObjectPlaceInGoodPosition(map, roomRect,
                   (pt) => map.GetMapObjectAt(pt) == null,
                   m_DiceRoller,
                   (pt) =>
                   {
                       map.DropItemAt(MakeItemBioForceGun(), pt);
                       placed = true;

                       // trolley.
                       MapObject trolley = MakeObjCHARtrolley(GameImages.OBJ_CHAR_TROLLEY);
                       return trolley;
                   });
                
                placedBioForceGun = placed; //@@MP - only drop one per game
            }
        }

        void MakeCHARLivingRoom(Map map, Rectangle roomRect) //@@MP - no longer used (Release 3)
        {
            // Replace floor with wood with painted logo.
            TileFill(map, m_Game.GameTiles.FLOOR_PLANKS, roomRect, (tile, model, x, y) => tile.AddDecoration(GameImages.DECO_CHAR_FLOOR_LOGO));
            
            // Objects.
            // Beds/Fridges along walls.
            MapObjectFill(map, roomRect,
                (pt) =>
                {
                    if (CountAdjWalls(map, pt.X, pt.Y) < 3)
                        return null;
                    // dont block exits!
                    if (map.GetExitAt(pt) != null)
                        return null;

                    // bed/fridge?
                    if (m_DiceRoller.RollChance(30))
                    {
                        if (m_DiceRoller.RollChance(50))
                            return MakeObjBed(GameImages.OBJ_BED);
                        else
                            return MakeObjFridge(GameImages.OBJ_FRIDGE);
                    }
                    else
                        return null;
                });
            // Tables(with canned food) & Chairs in the middle.
            MapObjectFill(map, roomRect,
                (pt) =>
                {
                    if (CountAdjWalls(map, pt.X, pt.Y) > 0)
                        return null;
                    // dont block exits!
                    if (map.GetExitAt(pt) != null)
                        return null;

                    // tables/chairs.
                    if (m_DiceRoller.RollChance(30))
                    {
                        if (m_DiceRoller.RollChance(30))
                        {
                            MapObject table = MakeObjTable(GameImages.OBJ_CHAR_TABLE);
                            map.DropItemAt(MakeItemCannedFood(), pt);
                            return table;
                        }
                        else
                            return MakeObjChair(GameImages.OBJ_CHAR_CHAIR);
                    }
                    else
                        return null;
                });
        }

        void MakeCHARPharmacyRoom(Map map, Rectangle roomRect)
        {
            // Shelves with medicine along walls.
            MapObjectFill(map, roomRect,
                (pt) =>
                {
                    if (CountAdjWalls(map, pt.X, pt.Y) < 2)
                        return null;
                    // don't block doors
                    if (IsADoorNSEW(map, pt.X, pt.Y)) //@@MP (Release 7-6)
                        return null;
                    // dont block exits!
                    if (map.GetExitAt(pt) != null)
                        return null;

                    // table + meds.
                    if (m_DiceRoller.RollChance(30))
                    {
                        if (m_DiceRoller.RollChance(GameOptions.ResourcesAvailabilityToInt(RogueGame.Options.ResourcesAvailability))) //@@MP - Resources Availability option (Release 7-4)
                        {
                            Item it = MakeHospitalItem();
                            map.DropItemAt(it, pt);
                        }
                    }

                    MapObject shelf = MakeObjShelf(GameImages.OBJ_SHOP_SHELF);
                    return shelf;
                });
        }

        void MakeCHARPowerRoom(Map map, Rectangle wallsRect, Rectangle roomRect)
        {
            // Replace floor with concrete.
            TileFill(map, m_Game.GameTiles.FLOOR_CONCRETE, roomRect);

            // add deco power sign next to doors.
            DoForEachTile(wallsRect, //@@MP - unused parameter (Release 5-7)
                (pt) =>
                {
                    if (!(map.GetMapObjectAt(pt) is DoorWindow))
                        return;
                    DoForEachAdjacentInMap(map, pt, (
                        ptAdj) =>
                        {
                            Tile tile = map.GetTileAt(ptAdj);
                            if (tile.Model.IsWalkable)
                                return;
                            tile.RemoveAllDecorations();
                            tile.AddDecoration(GameImages.DECO_POWER_SIGN_BIG);
                        });
                });

            // add power generators along walls.
            DoForEachTile(roomRect, //@@MP - unused parameter (Release 5-7)
                (pt) =>
                {
                    if (!map.GetTileAt(pt).Model.IsWalkable)
                        return;
                    if (map.GetExitAt(pt) != null)
                        return;
                    if (CountAdjWalls(map, pt.X, pt.Y) < 3)
                        return;

                    PowerGenerator powGen = MakeObjPowerGenerator(GameImages.OBJ_POWERGEN_OFF, GameImages.OBJ_POWERGEN_ON);
                    map.PlaceMapObjectAt(powGen, pt);
                });
        }
#endregion

#region Police Station
        void MakePoliceStation(Map map, List<Block> freeBlocks, out Block policeBlock)
        {
            ////////////////////////////////
            // 1. Pick a block.
            // 2. Generate surface station.
            // 3. Generate level -1.
            // 4. Generate level -2.
            // 5. Link maps.
            // 6. Add maps to district.
            // 7. Set unique maps.
            ////////////////////////////////

            // 1. Pick a block.
            // any random block will do.
            policeBlock = freeBlocks[m_DiceRoller.Roll(0, freeBlocks.Count)];

            // 2. Generate surface station.
            Point surfaceStairsPos;

            GeneratePoliceStation(map, policeBlock, out surfaceStairsPos);

            // 3. Generate Offices level (-1).
            Map officesLevel = GeneratePoliceStation_OfficesLevel(map); //@@MP - unused parameter (Release 5-7)

            // 4. Generate Jails level (-2).
            Map jailsLevel = GeneratePoliceStation_JailsLevel(officesLevel);
            
            // alpha10 music
            officesLevel.BgMusic = jailsLevel.BgMusic = GameMusics.SURFACE;

            // 5. Link maps.
            // surface <-> offices level
            AddExit(map, surfaceStairsPos, officesLevel, new Point(2,1), GameImages.DECO_STAIRS_DOWN, true);
            AddExit(officesLevel, new Point(2,1), map, surfaceStairsPos, GameImages.DECO_STAIRS_UP, true);

            // offices <-> jails
            AddExit(officesLevel, new Point(1, officesLevel.Height - 2), jailsLevel, new Point(2, 1), GameImages.DECO_STAIRS_DOWN, true);
            AddExit(jailsLevel, new Point(2, 1), officesLevel, new Point(1, officesLevel.Height - 2), GameImages.DECO_STAIRS_UP, true);

            // 6. Add maps to district.
            m_Params.District.AddUniqueMap(officesLevel);
            m_Params.District.AddUniqueMap(jailsLevel);

            // 7. Set unique maps.
            m_Game.Session.UniqueMaps.PoliceStation_OfficesLevel = new UniqueMap() { TheMap = officesLevel };
            m_Game.Session.UniqueMaps.PoliceStation_JailsLevel = new UniqueMap() { TheMap = jailsLevel };

            // done!
        }

        void GeneratePoliceStation(Map surfaceMap, Block policeBlock, out Point stairsToLevel1) //surface level
        {
            // Fill & Enclose Building.
            TileFill(surfaceMap, m_Game.GameTiles.FLOOR_TILES, policeBlock.InsideRect);
            TileRectangle(surfaceMap, m_Game.GameTiles.WALL_POLICE_STATION, policeBlock.BuildingRect);
            TileRectangle(surfaceMap, m_Game.GameTiles.FLOOR_WALKWAY, policeBlock.Rectangle);
            DoForEachTile(policeBlock.InsideRect, (pt) => surfaceMap.GetTileAt(pt).IsInside = true); //@@MP - unused parameter (Release 5-7)

            // Entrance to the south with police signs.
            Point entryDoorPos = new Point(policeBlock.BuildingRect.Left + policeBlock.BuildingRect.Width / 2, policeBlock.BuildingRect.Bottom - 1);
            surfaceMap.GetTileAt(entryDoorPos.X - 1, entryDoorPos.Y).AddDecoration(GameImages.DECO_POLICE_STATION);
            surfaceMap.GetTileAt(entryDoorPos.X + 1, entryDoorPos.Y).AddDecoration(GameImages.DECO_POLICE_STATION);

            // Entry hall.
            Rectangle entryHall = Rectangle.FromLTRB(policeBlock.BuildingRect.Left, policeBlock.BuildingRect.Top + 2, policeBlock.BuildingRect.Right, policeBlock.BuildingRect.Bottom);
            TileRectangle(surfaceMap, m_Game.GameTiles.WALL_POLICE_STATION, entryHall);
            PlaceDoor(surfaceMap, entryHall.Left + entryHall.Width / 2, entryHall.Top, m_Game.GameTiles.FLOOR_TILES, MakeObjIronDoor(DoorWindow.STATE_CLOSED));
            PlaceDoor(surfaceMap, entryDoorPos.X, entryDoorPos.Y, m_Game.GameTiles.FLOOR_TILES, MakeObjGlassDoor());
            DoForEachTile(entryHall, //@@MP - unused parameter (Release 5-7)
                (pt) =>
                {
                    if (!surfaceMap.IsWalkable(pt.X, pt.Y))
                        return;
                    if (CountAdjWalls(surfaceMap, pt.X, pt.Y) == 0)
                        return;
                    if (IsADoorNSEW(surfaceMap, pt.X, pt.Y))
                        return;
                    if (CountAdjDoors(surfaceMap, pt.X, pt.Y) > 0)
                    {
                        if (pt.Y != entryDoorPos.Y - 1) //don't put them near the front door
                            surfaceMap.PlaceMapObjectAt(MakeObjFireBarrel(GameImages.OBJ_EMPTY_BIN), pt); //@@MP - added (Release 7-6)
                        return;
                    }

                    surfaceMap.PlaceMapObjectAt(MakeObjBench(GameImages.OBJ_BENCH), pt);
                });

            // Place stairs, north side.
            stairsToLevel1 = new Point(entryDoorPos.X, policeBlock.InsideRect.Top);

            // Zone.
            surfaceMap.AddZone(MakeUniqueZone("Police Station", policeBlock.BuildingRect));
            MakeWalkwayZones(surfaceMap, policeBlock);
        }

        Map GeneratePoliceStation_OfficesLevel(Map surfaceMap) //@@MP - unused parameter (Release 5-7)
        {
            //////////////////
            // 1. Create map.
            // 2. Floor plan.
            // 3. Populate.
            //////////////////

            // 1. Create map.
            int seed = (surfaceMap.Seed << 1) ^ surfaceMap.Seed;
            Map map = new Map(seed, "Police Station - Offices", 22, 20) //@@MP - made 2 tiles wider (Release 7-6)
            {
                Lighting = Lighting.LIT
            };
            DoForEachTile(map.Rect, (pt) => map.GetTileAt(pt).IsInside = true); //@@MP - unused parameter (Release 5-7)

            // 2. Floor plan.
            TileFill(map, m_Game.GameTiles.FLOOR_TILES);
            TileRectangle(map, m_Game.GameTiles.WALL_POLICE_STATION, map.Rect);
            // - offices rooms on the east side, doors leading west.
            Rectangle officesRect = Rectangle.FromLTRB(5, 0, map.Width, map.Height);
            List<Rectangle> roomsList = new List<Rectangle>();
            MakeRoomsPlan(map, ref roomsList, officesRect, 7, 7);
            foreach (Rectangle roomRect in roomsList)
            {
                Rectangle inRoomRect = Rectangle.FromLTRB(roomRect.Left + 1, roomRect.Top + 1, roomRect.Right - 1, roomRect.Bottom - 1);
                // 2 kind of rooms.
                // - farthest east from corridor : security.
                // - others : offices.
                if (roomRect.Right == map.Width)
                {
                    // Police Security Room.
#region
                    // make room with door.
                    TileRectangle(map, m_Game.GameTiles.WALL_POLICE_STATION, roomRect);
                    PlaceDoor(map, roomRect.Left, roomRect.Top + roomRect.Height / 2, m_Game.GameTiles.FLOOR_CONCRETE, MakeObjIronDoor(DoorWindow.STATE_CLOSED));

                    // shelves with weaponry & armor next to the walls.
                    DoForEachTile(inRoomRect, //@@MP - unused parameter (Release 5-7)
                        (pt) =>
                        {
                            if (!map.IsWalkable(pt.X, pt.Y) || CountAdjMapObjects(map, pt.X, pt.Y) > 1 || CountAdjDoors(map, pt.X, pt.Y) > 0) //CountAdjWalls(map, pt.X, pt.Y) == 0 
                                return;

                            // shelf.
                            map.PlaceMapObjectAt(MakeObjShelf(GameImages.OBJ_SHOP_SHELF), pt);

                            // weaponry/armor/radios.
                            Item it = null;
                            int roll = m_DiceRoller.Roll(0, 20);
                            switch(roll)
                            {
                                // 20% armors
                                case 0:
                                case 1:
                                case 2:
                                case 3:
                                    it = m_DiceRoller.RollChance(50) ? MakeItemPoliceJacket() : MakeItemPoliceRiotArmor(); break;

                                // 10% precision rifle/ammo - 20% rifle 80% ammo    //@@MP (Release 7-6)
                                case 4:
                                case 5:
                                    it = m_DiceRoller.RollChance(20) ? MakeItemPrecisionRifle() : MakeItemPrecisionRifleAmmo(); break;

                                // 5% smoke grenade
                                case 6: it = MakeItemSmokeGrenade(); break; //@@MP (Release 7-2)

                                // 5% light/radio
                                case 7: it = m_DiceRoller.RollChance(50) ? (m_DiceRoller.RollChance(50) ? MakeItemFlashlight() : MakeItemBigFlashlight()) : MakeItemPoliceRadio(); break;

                                // 20% pistol/ammo - 20% pistol 80% ammo
                                case 8:
                                case 9:
                                case 10:
                                case 11:
                                    it = m_DiceRoller.RollChance(20) ? MakeItemRandomPistol() : MakeItemLightPistolAmmo(); break;

                                // 20% shotgun/ammo - 20% shotgun 80% ammo
                                case 12:
                                case 13:
                                case 14:
                                case 15:
                                    it = m_DiceRoller.RollChance(20) ? MakeItemTacticalShotgun() : MakeItemShotgunAmmo(); break;

                                // 10% flares kit
                                case 16:
                                case 17:
                                    it = MakeItemFlaresKit(); break; //@@MP (Release 7-1)

                                // 5% riot shield
                                case 18: it = MakeItemPoliceRiotShield(); break; //@@MP (Release 7-2)

                                // 5% truncheon/stun gun - 30% truncheon, 70% stun gun
                                case 19: it = m_DiceRoller.RollChance(30) ? MakeItemTruncheon() : MakeItemStunGun(); break; //@@MP (Release 7-2)

                                default:
                                    throw new InvalidOperationException("unhandled roll");

                            }

                            map.DropItemAt(it, pt);
                                
                        });

                    // zone.
                    map.AddZone(MakeUniqueZone("security", inRoomRect));
#endregion
                }
                else
                {
                    // Police Office Room.
#region
                    // make room with door.
                    TileFill(map, m_Game.GameTiles.FLOOR_PLANKS, roomRect);
                    TileRectangle(map, m_Game.GameTiles.WALL_POLICE_STATION, roomRect);
                    PlaceDoor(map, roomRect.Left, roomRect.Top + roomRect.Height / 2, m_Game.GameTiles.FLOOR_PLANKS, MakeObjWoodenDoor());

                    for (int i = 0; i < 7; i++)
                    {
                        // add furniture : 1 table, 2 chairs.
                        MapObjectPlaceInGoodPosition(map, inRoomRect,
                        (pt) => map.IsWalkable(pt.X, pt.Y) && CountAdjDoors(map, pt.X, pt.Y) == 0,
                        m_DiceRoller,
                        (pt) =>
                        {
                            // item.  //@@MP (Release 7-3)
                            if (m_DiceRoller.RollChance(99))
                            {
                                Item it = MakeShopGeneralItem();
                                if (it != null)
                                    map.DropItemAt(it, pt);
                            }
                            return MakeObjTable(GameImages.OBJ_TABLE);
                        });

                        MapObjectPlaceInGoodPosition(map, inRoomRect,
                            (pt) => map.IsWalkable(pt.X, pt.Y) && CountAdjDoors(map, pt.X, pt.Y) == 0 && CountAdjMapObjects(map, pt.X, pt.Y) > 0,
                            m_DiceRoller,
                            (pt) => MakeObjChair(GameImages.OBJ_CHAIR));

                        MapObjectPlaceInGoodPosition(map, inRoomRect,
                            (pt) => map.IsWalkable(pt.X, pt.Y) && CountAdjDoors(map, pt.X, pt.Y) == 0 && CountAdjMapObjects(map, pt.X, pt.Y) > 0,
                            m_DiceRoller,
                            (pt) => MakeObjChair(GameImages.OBJ_CHAIR));
                    }

                    // zone.
                    map.AddZone(MakeUniqueZone("office", inRoomRect));
#endregion
                }
            }
            // - benches in corridor.
            DoForEachTile(new Rectangle(1, 1, 1, map.Height - 2), //@@MP - unused parameter (Release 5-7)
                (pt) =>
                {
                    if (pt.Y % 2 == 1)
                        return;
                    if (!map.IsWalkable(pt))
                        return;
                    if (CountAdjWalls(map, pt) != 3)
                        return;

                    map.PlaceMapObjectAt(MakeObjIronBench(GameImages.OBJ_IRON_BENCH), pt);
                });

            // 3. Populate.
            // - cops.
            const int nbCops = 5;
            for (int i = 0; i < nbCops; i++)
            {
                Actor cop = CreateNewPoliceman(0);
                ActorPlace(m_DiceRoller, map.Width * map.Height, map, cop);
            }

            // done.
            return map;
        }

        Map GeneratePoliceStation_JailsLevel(Map surfaceMap)
        {
            //////////////////
            // 1. Create map.
            // 2. Floor plan.
            // 3. Populate.
            //////////////////

            // 1. Create map.
            int seed = (surfaceMap.Seed << 1) ^ surfaceMap.Seed;
            Map map = new Map(seed, "Police Station - Jails", 22, 10) //@@MP - expanded the height by 4 (Release 7-6)
            {
                Lighting = Lighting.LIT //@@MP - was Darkness (Release 6-4)
            };
            DoForEachTile(map.Rect, (pt) => map.GetTileAt(pt).IsInside = true); //@@MP - unused parameter (Release 5-7)

            // 2. Floor plan.
            TileFill(map, m_Game.GameTiles.FLOOR_TILES);
            TileRectangle(map, m_Game.GameTiles.WALL_POLICE_STATION, map.Rect);
            // - small cells.
            const int cellWidth = 3;
            const int cellHeight = 5;
            const int yCells = 5;
            List<Rectangle> cells = new List<Rectangle>();
            for (int x = 0; x + cellWidth <= map.Width; x += cellWidth - 1)
            {
                // bench
                Point benchPos = new Point(x + 1, yCells - 4);
                if (benchPos != new Point(map.Width - 3, 1) && benchPos != new Point(1, 1) && benchPos != new Point(3, 1)) //don't do the last one, as that's where the guard station fence is, nor the first two because of the stairs
                    map.PlaceMapObjectAt(MakeObjIronBench(GameImages.OBJ_IRON_BENCH), benchPos);

                // room.
                Rectangle cellRoom = new Rectangle(x, yCells, cellWidth, cellHeight);
                cells.Add(cellRoom);
                TileFill(map, m_Game.GameTiles.FLOOR_CONCRETE, cellRoom);
                TileRectangle(map, m_Game.GameTiles.WALL_POLICE_STATION, cellRoom);

                // bed.
                Point bedPos = new Point(x + 1, yCells + 2);
                map.PlaceMapObjectAt(MakeObjBed(GameImages.OBJ_ARMY_BUNK_BED), bedPos);
                if (bedPos == new Point(map.Width - 3, 7)) //@@MP - lazy hack to make the special prisoner stuck directly adjacent to their cell door (Release 7-6)
                {
                    map.GetMapObjectAt(bedPos).JumpLevel = 0;
                    map.GetMapObjectAt(bedPos).IsWalkable = false;
                    map.GetMapObjectAt(bedPos).BreakState = MapObject.Break.UNBREAKABLE;
                }

                // toilet
                Point toiletPos = new Point(x + 1, yCells + 3);
                map.PlaceMapObjectAt(MakeObjToilet(GameImages.OBJ_TOILET), toiletPos);

                // gate.
                Point gatePos = new Point(x + 1, yCells);
                map.SetTileModelAt(gatePos.X, gatePos.Y, m_Game.GameTiles.FLOOR_CONCRETE);
                map.PlaceMapObjectAt(MakeObjIronGate(GameImages.OBJ_GATE_CLOSED), gatePos);

                // zone.
                map.AddZone(MakeUniqueZone(RogueGame.NAME_POLICE_STATION_JAILS_CELL, cellRoom));
            }
            // - corridor.
            Rectangle corridor = Rectangle.FromLTRB(1, 1, map.Width, yCells);
            map.AddZone(MakeUniqueZone("cells corridor", corridor));
            // - the switch to open/close the cells.
            map.PlaceMapObjectAt(MakeObjIronFence(GameImages.OBJ_IRON_FENCE), new Point(map.Width - 3, 1)); //@@MP - added a wall to funnel the player to power generator (Release 7-6)
            map.PlaceMapObjectAt(MakeObjIronFence(GameImages.OBJ_IRON_FENCE), new Point(map.Width - 3, 2));
            map.PlaceMapObjectAt(MakeObjIronFence(GameImages.OBJ_IRON_FENCE), new Point(map.Width - 3, 3));
            map.PlaceMapObjectAt(MakeObjBankTeller(GameImages.OBJ_BANK_TELLER), new Point(map.Width - 2, 1)); //@@MP - added to look like a guard station (Release 6-1)
            map.DropItemAt(MakeItemShotgunAmmo(), new Point(map.Width - 2, 2)); //and some ammo to sweeten the deal
            map.DropItemAt(MakeItemShotgunAmmo(), new Point(map.Width - 2, 2)); //and some ammo to sweeten the deal
            map.DropItemAt(MakeItemShotgunAmmo(), new Point(map.Width - 2, 2)); //and some ammo to sweeten the deal
            map.DropItemAt(MakeItemTacticalShotgun(), new Point(map.Width - 2, 2)); //place a gun to entice the player over
            PlaceDoor(map, map.Width - 2, 3, m_Game.GameTiles.FLOOR_TILES, MakeObjIronDoor(DoorWindow.STATE_LOCKED)); //@@MP - added for enticing the player towards (Release 6-1), locked it as it should have been originally (Release 7-6)
            map.PlaceMapObjectAt(MakeObjPowerGenerator(GameImages.OBJ_POWERGEN_OFF, GameImages.OBJ_POWERGEN_ON), new Point(map.Width - 2, 4)); //@@MP - moved generator 1 south (Release 6-1)

            // 3. Populate.
            // - prisoners in each cell.
            //   keep the last cell for the special prisonner.
            for (int i = 0; i < cells.Count - 1; i++)
            {
                Rectangle cell = cells[i];

                // jailed. Civilian.
                Actor prisoner = CreateNewPrisoner(0, 1);

                // drop him.
                map.PlaceActorAt(prisoner, new Point(cell.Left + 1, cell.Top + 1));
            }
            // - Special prisoner in the last cell.
            Rectangle lastCell = cells[cells.Count - 1];
            Actor specialPrisoner = CreateNewCivilian(0, 0, 1);
            specialPrisoner.IsUnique = true; //@@MP - fix for RS Alpha 10 bug that made prisonner invincible (Release 7-6)
            specialPrisoner.Name = "The Prisoner Who Should Not Be";
            for (int i = 0; i < specialPrisoner.Inventory.MaxCapacity; i++)
                specialPrisoner.Inventory.AddAll(MakeItemArmyRation());
            map.PlaceActorAt(specialPrisoner, new Point(lastCell.Left + 1, lastCell.Top + 1));
            m_Game.Session.UniqueActors.PoliceStationPrisonner = new UniqueActor()
            {
                TheActor = specialPrisoner,
                IsSpawned = true
            };

            // done.
            return map;
        }
#endregion

#region Hospital
        /// <summary>
        /// Layout :
        ///  0 floor: Entry Hall.
        /// -1 floor: Admissions (short term patients).
        /// -2 floor: Offices. (doctors)
        /// -3 floor: Patients. (nurses, injured patients)
        /// -4 floor: Storage. (bunch of meds & pills; blocked by closed gates, need power on)
        /// -5 floor: Power. (restore power to the whole building = lights, open storage gates)
        /// </summary>
        /// <param name="map"></param>
        /// <param name="freeBlocks"></param>
        /// <param name="hospitalBlock"></param>
        void MakeHospital(Map map, List<Block> freeBlocks, out Block hospitalBlock)
        {
            ////////////////////////////////
            // 1. Pick a block.
            // 2. Generate surface building.
            // 3. Generate other levels maps.
            // 4. Link maps.
            // 5. Add maps to district.
            // 6. Set unique maps.
            ////////////////////////////////

            // 1. Pick a block.
            // any random block will do.
            hospitalBlock = freeBlocks[m_DiceRoller.Roll(0, freeBlocks.Count)];

            // 2. Generate surface.
            GenerateHospitalEntryHall(map, hospitalBlock);

            // 3. Generate other levels maps.
            Map admissions = GenerateHospital_Admissions((map.Seed << 1) ^ map.Seed);
            Map offices = GenerateHospital_Offices((map.Seed << 2) ^ map.Seed);
            Map patients = GenerateHospital_Patients((map.Seed << 3) ^ map.Seed);
            Map storage = GenerateHospital_Storage((map.Seed << 4) ^ map.Seed);
            Map power = GenerateHospital_Power((map.Seed << 5) ^ map.Seed);

            // alpha10 music
            admissions.BgMusic = offices.BgMusic = patients.BgMusic = storage.BgMusic = power.BgMusic = GameMusics.HOSPITAL;

            // 4. Link maps.
            // entry <-> admissions
            Point entryStairs = new Point(hospitalBlock.InsideRect.Left + hospitalBlock.InsideRect.Width / 2, hospitalBlock.InsideRect.Top);
            Point admissionsUpStairs = new Point(admissions.Width / 2, 1);
            AddExit(map, entryStairs, admissions, admissionsUpStairs, GameImages.DECO_STAIRS_DOWN, true);
            AddExit(admissions, admissionsUpStairs, map, entryStairs, GameImages.DECO_STAIRS_UP, true);

            // admissions <-> offices
            Point admissionsDownStairs = new Point(admissions.Width / 2, admissions.Height - 2);
            Point officesUpStairs = new Point(offices.Width / 2, 1);
            AddExit(admissions, admissionsDownStairs, offices, officesUpStairs, GameImages.DECO_STAIRS_DOWN, true);
            AddExit(offices, officesUpStairs, admissions, admissionsDownStairs, GameImages.DECO_STAIRS_UP, true);

            // offices <-> patients
            Point officesDownStairs = new Point(offices.Width / 2, offices.Height - 2);
            Point patientsUpStairs = new Point(patients.Width / 2, 1);
            AddExit(offices, officesDownStairs, patients, patientsUpStairs, GameImages.DECO_STAIRS_DOWN, true);
            AddExit(patients, patientsUpStairs, offices, officesDownStairs, GameImages.DECO_STAIRS_UP, true);

            // patients <-> storage
            Point patientsDownStairs = new Point(patients.Width / 2, patients.Height - 2);
            Point storageUpStairs = new Point(1, 1);
            AddExit(patients, patientsDownStairs, storage, storageUpStairs, GameImages.DECO_STAIRS_DOWN, true);
            AddExit(storage, storageUpStairs, patients, patientsDownStairs, GameImages.DECO_STAIRS_UP, true);

            // storage <-> power
            Point storageDownStairs = new Point(storage.Width - 2, 1);
            Point powerUpStairs = new Point(1, 1);
            AddExit(storage, storageDownStairs, power, powerUpStairs, GameImages.DECO_STAIRS_DOWN, true);
            AddExit(power, powerUpStairs, storage, storageDownStairs, GameImages.DECO_STAIRS_UP, true);

            // 5. Add maps to district.
            m_Params.District.AddUniqueMap(admissions);
            m_Params.District.AddUniqueMap(offices);
            m_Params.District.AddUniqueMap(patients);
            m_Params.District.AddUniqueMap(storage);
            m_Params.District.AddUniqueMap(power);

            // 6. Set unique maps.
            m_Game.Session.UniqueMaps.Hospital_Admissions = new UniqueMap() { TheMap = admissions };
            m_Game.Session.UniqueMaps.Hospital_Offices = new UniqueMap() { TheMap = offices };
            m_Game.Session.UniqueMaps.Hospital_Patients = new UniqueMap() { TheMap = patients };
            m_Game.Session.UniqueMaps.Hospital_Storage = new UniqueMap() { TheMap = storage };
            m_Game.Session.UniqueMaps.Hospital_Power = new UniqueMap() { TheMap = power };

            // done!
        }

        void GenerateHospitalEntryHall(Map surfaceMap, Block block)
        {
            // Fill & Enclose Building.
            TileFill(surfaceMap, m_Game.GameTiles.FLOOR_TILES, block.InsideRect);
            TileRectangle(surfaceMap, m_Game.GameTiles.WALL_HOSPITAL, block.BuildingRect);
            TileRectangle(surfaceMap, m_Game.GameTiles.FLOOR_WALKWAY, block.Rectangle);
            DoForEachTile(block.InsideRect, (pt) => surfaceMap.GetTileAt(pt).IsInside = true); //@@MP - unused parameter (Release 5-7)

            // 2 entrances to the south with signs.
            Point entryRightDoorPos = new Point(block.BuildingRect.Left + block.BuildingRect.Width / 2, block.BuildingRect.Bottom - 1);
            Point entryLeftDoorPos = new Point(entryRightDoorPos.X - 1, entryRightDoorPos.Y);
            surfaceMap.GetTileAt(entryLeftDoorPos.X - 1, entryLeftDoorPos.Y).AddDecoration(GameImages.DECO_HOSPITAL);
            surfaceMap.GetTileAt(entryRightDoorPos.X + 1, entryRightDoorPos.Y).AddDecoration(GameImages.DECO_HOSPITAL);

            // Entry hall = whole building.
            Rectangle entryHall = Rectangle.FromLTRB(block.BuildingRect.Left, block.BuildingRect.Top, block.BuildingRect.Right, block.BuildingRect.Bottom);
            PlaceDoor(surfaceMap, entryRightDoorPos.X, entryRightDoorPos.Y, m_Game.GameTiles.FLOOR_TILES, MakeObjGlassDoor());
            PlaceDoor(surfaceMap, entryLeftDoorPos.X, entryLeftDoorPos.Y, m_Game.GameTiles.FLOOR_TILES, MakeObjGlassDoor());
            DoForEachTile(entryHall, //@@MP - unused parameter (Release 5-7)
                (pt) =>
                {
                    // benches only on west & east sides.
                    if (pt.Y == block.InsideRect.Top || pt.Y == block.InsideRect.Bottom - 1)
                        return;
                    if (!surfaceMap.IsWalkable(pt.X, pt.Y))
                        return;
                    if (CountAdjWalls(surfaceMap, pt.X, pt.Y) == 0 || CountAdjDoors(surfaceMap, pt.X, pt.Y) > 0)
                        return;
                    surfaceMap.PlaceMapObjectAt(MakeObjIronBench(GameImages.OBJ_IRON_BENCH), pt);
                });

            // Zone.
            surfaceMap.AddZone(MakeUniqueZone("Hospital", block.BuildingRect));
            MakeWalkwayZones(surfaceMap, block);
        }

        Map GenerateHospital_Admissions(int seed)
        {
            //////////////////
            // 1. Create map.
            // 2. Floor plan.
            // 3. Populate.
            //////////////////

            // 1. Create map.
            Map map = new Map(seed, "Hospital - Admissions", 13, 33)
            {
                Lighting = Lighting.LIT
            };
            DoForEachTile(map.Rect, (pt) => map.GetTileAt(pt).IsInside = true); //@@MP - unused parameter (Release 5-7)
            TileFill(map, m_Game.GameTiles.FLOOR_TILES);
            TileRectangle(map, m_Game.GameTiles.WALL_HOSPITAL, map.Rect);

            // 2. Floor plan.
            // One central south->north corridor with admission rooms on each sides.
            const int roomSize = 5;

            // 1. Central corridor.
            Rectangle corridor = new Rectangle(roomSize - 1, 0, 5, map.Height);
            TileRectangle(map, m_Game.GameTiles.WALL_HOSPITAL, corridor);
            map.AddZone(MakeUniqueZone("corridor", corridor));

            // 2. Admission rooms, all similar 5x5 rooms (3x3 inside)            
            Rectangle leftWing = new Rectangle(0, 0, roomSize, map.Height);
            for (int roomY = 0; roomY <= map.Height - roomSize; roomY += roomSize - 1)
            {
                Rectangle room = new Rectangle(leftWing.Left, roomY, roomSize, roomSize);
                MakeHospitalPatientRoom(map, "patient room", room, true);
            }

            Rectangle rightWing = new Rectangle(map.Rect.Right - roomSize, 0, roomSize, map.Height);
            for (int roomY = 0; roomY <= map.Height - roomSize; roomY += roomSize - 1)
            {
                Rectangle room = new Rectangle(rightWing.Left, roomY, roomSize, roomSize);
                MakeHospitalPatientRoom(map, "patient room", room, false);
            }

            // 3. Populate.
            // patients in rooms.
            const int nbPatients = 10;
            for (int i = 0; i < nbPatients; i++)
            {
                // create.
                Actor patient = CreateNewHospitalPatient(); //@@MP - unused parameter (Release 5-7)
                // place.
                ActorPlace(m_DiceRoller, map.Width * map.Height, map, patient, (pt) => map.HasZonePartiallyNamedAt(pt, "patient room"));
            }

            // nurses & doctor in corridor.
            const int nbNurses = 4;
            for (int i = 0; i < nbNurses; i++)
            {
                // create.
                Actor nurse = CreateNewHospitalNurse(); //@@MP - unused parameter (Release 5-7)
                // place.
                ActorPlace(m_DiceRoller, map.Width * map.Height, map, nurse, (pt) => map.HasZonePartiallyNamedAt(pt, "corridor"));
            }
            const int nbDoctor = 1;
            for (int i = 0; i < nbDoctor; i++)
            {
                // create.
                Actor nurse = CreateNewHospitalDoctor(); //@@MP - unused parameter (Release 5-7)
                // place.
                ActorPlace(m_DiceRoller, map.Width * map.Height, map, nurse, (pt) => map.HasZonePartiallyNamedAt(pt, "corridor"));
            }

            // done.
            return map;
        }

        Map GenerateHospital_Offices(int seed)
        {
            //////////////////
            // 1. Create map.
            // 2. Floor plan.
            // 3. Populate.
            //////////////////

            // 1. Create map.
            Map map = new Map(seed, "Hospital - Offices", 13, 33)
            {
                Lighting = Lighting.LIT
            };
            DoForEachTile(map.Rect, (pt) => map.GetTileAt(pt).IsInside = true); //@@MP - unused parameter (Release 5-7)
            TileFill(map, m_Game.GameTiles.FLOOR_TILES);
            TileRectangle(map, m_Game.GameTiles.WALL_HOSPITAL, map.Rect);

            // 2. Floor plan.
            // One central south->north corridor with offices rooms on each sides.
            const int roomSize = 5;

            // 1. Central corridor.
            Rectangle corridor = new Rectangle(roomSize - 1, 0, 5, map.Height);
            TileRectangle(map, m_Game.GameTiles.WALL_HOSPITAL, corridor);
            map.AddZone(MakeUniqueZone("corridor", corridor));

            // 2. Offices rooms, all similar 5x5 rooms (3x3 inside)
            Rectangle leftWing = new Rectangle(0, 0, roomSize, map.Height);
            for (int roomY = 0; roomY <= map.Height - roomSize; roomY += roomSize - 1)
            {
                Rectangle room = new Rectangle(leftWing.Left, roomY, roomSize, roomSize);
                MakeHospitalOfficeRoom(map, "office", room, true);
            }

            Rectangle rightWing = new Rectangle(map.Rect.Right - roomSize, 0, roomSize, map.Height);
            for (int roomY = 0; roomY <= map.Height - roomSize; roomY += roomSize - 1)
            {
                Rectangle room = new Rectangle(rightWing.Left, roomY, roomSize, roomSize);
                MakeHospitalOfficeRoom(map, "office", room, false);
            }

            // 3. Populate.
            // nurses & doctor in offices.
            const int nbNurses = 5;
            for (int i = 0; i < nbNurses; i++)
            {
                // create.
                Actor nurse = CreateNewHospitalNurse(); //@@MP - unused parameter (Release 5-7)
                // place.
                ActorPlace(m_DiceRoller, map.Width * map.Height, map, nurse, (pt) => map.HasZonePartiallyNamedAt(pt, "office"));
            }
            const int nbDoctor = 2;
            for (int i = 0; i < nbDoctor; i++)
            {
                // create.
                Actor nurse = CreateNewHospitalDoctor(); //@@MP - unused parameter (Release 5-7)
                // place.
                ActorPlace(m_DiceRoller, map.Width * map.Height, map, nurse, (pt) => map.HasZonePartiallyNamedAt(pt, "office"));
            }

            // done.
            return map;
        }

        Map GenerateHospital_Patients(int seed)
        {
            //////////////////
            // 1. Create map.
            // 2. Floor plan.
            // 3. Populate.
            //////////////////

            // 1. Create map.
            Map map = new Map(seed, "Hospital - Patients", 13, 49)
            {
                Lighting = Lighting.LIT
            };
            DoForEachTile(map.Rect, (pt) => map.GetTileAt(pt).IsInside = true); //@@MP - unused parameter (Release 5-7)
            TileFill(map, m_Game.GameTiles.FLOOR_TILES);
            TileRectangle(map, m_Game.GameTiles.WALL_HOSPITAL, map.Rect);

            // 2. Floor plan.
            // One central south->north corridor with admission rooms on each sides.
            const int roomSize = 5;

            // 1. Central corridor.
            Rectangle corridor = new Rectangle(roomSize - 1, 0, 5, map.Height);
            TileRectangle(map, m_Game.GameTiles.WALL_HOSPITAL, corridor);
            map.AddZone(MakeUniqueZone("corridor", corridor));

            // 2. Patients rooms, all similar 5x5 rooms (3x3 inside)            
            Rectangle leftWing = new Rectangle(0, 0, roomSize, map.Height);
            for (int roomY = 0; roomY <= map.Height - roomSize; roomY += roomSize - 1)
            {
                Rectangle room = new Rectangle(leftWing.Left, roomY, roomSize, roomSize);
                MakeHospitalPatientRoom(map, "patient room", room, true);
            }

            Rectangle rightWing = new Rectangle(map.Rect.Right - roomSize, 0, roomSize, map.Height);
            for (int roomY = 0; roomY <= map.Height - roomSize; roomY += roomSize - 1)
            {
                Rectangle room = new Rectangle(rightWing.Left, roomY, roomSize, roomSize);
                MakeHospitalPatientRoom(map, "patient room", room, false);
            }

            // 3. Populate.
            // patients in rooms.
            const int nbPatients = 20;
            for (int i = 0; i < nbPatients; i++)
            {
                // create.
                Actor patient = CreateNewHospitalPatient(); //@@MP - unused parameter (Release 5-7)
                // place.
                ActorPlace(m_DiceRoller, map.Width * map.Height, map, patient, (pt) => map.HasZonePartiallyNamedAt(pt, "patient room"));
            }

            // nurses & doctor in corridor.
            const int nbNurses = 8;
            for (int i = 0; i < nbNurses; i++)
            {
                // create.
                Actor nurse = CreateNewHospitalNurse(); //@@MP - unused parameter (Release 5-7)
                // place.
                ActorPlace(m_DiceRoller, map.Width * map.Height, map, nurse, (pt) => map.HasZonePartiallyNamedAt(pt, "corridor"));
            }
            const int nbDoctor = 2;
            for (int i = 0; i < nbDoctor; i++)
            {
                // create.
                Actor nurse = CreateNewHospitalDoctor(); //@@MP - unused parameter (Release 5-7)
                // place.
                ActorPlace(m_DiceRoller, map.Width * map.Height, map, nurse, (pt) => map.HasZonePartiallyNamedAt(pt, "corridor"));
            }

            // done.
            return map;
        }

        Map GenerateHospital_Storage(int seed)
        {
            //////////////////
            // 1. Create map.
            // 2. Floor plan.
            //////////////////

            // 1. Create map.
            Map map = new Map(seed, "Hospital - Storage", 51, 16)
            {
                Lighting = Lighting.DARKNESS
            };
            DoForEachTile(map.Rect, (pt) => map.GetTileAt(pt).IsInside = true); //@@MP - unused parameter (Release 5-7)
            TileFill(map, m_Game.GameTiles.FLOOR_TILES);
            TileRectangle(map, m_Game.GameTiles.WALL_HOSPITAL, map.Rect);

            // 2. Floor plan.
            // 1 north corridor linking stairs.
            // 1 central corridor to storage rooms, locked by an iron gate.
            // 1 south corridor to other storage rooms.

            // 1 north corridor linking stairs.
            const int northCorridorHeight = 4;
            Rectangle northCorridorRect = Rectangle.FromLTRB(0, 0, map.Width, northCorridorHeight);
            TileRectangle(map, m_Game.GameTiles.WALL_HOSPITAL, northCorridorRect);
            map.AddZone(MakeUniqueZone("north corridor", northCorridorRect));

            // 1 corridor to storage rooms, locked by an iron gate.
            const int corridorHeight = 4;
            Rectangle centralCorridorRect = Rectangle.FromLTRB(0, northCorridorRect.Bottom - 1, map.Width, northCorridorRect.Bottom - 1 + corridorHeight);
            TileRectangle(map, m_Game.GameTiles.WALL_HOSPITAL, centralCorridorRect);
            map.SetTileModelAt(1, centralCorridorRect.Top, m_Game.GameTiles.FLOOR_TILES);
            map.PlaceMapObjectAt(MakeObjIronGate(GameImages.OBJ_GATE_CLOSED), new Point(1, centralCorridorRect.Top));
            map.AddZone(MakeUniqueZone("central corridor", centralCorridorRect));
            // storage rooms.
            const int storageWidth = 5;
            const int storageHeight = 4;
            Rectangle storageCentral = new Rectangle(2, centralCorridorRect.Bottom - 1, map.Width - 2, storageHeight);
            for (int roomX = storageCentral.Left; roomX <= map.Width - storageWidth; roomX += storageWidth - 1)
            {
                Rectangle room = new Rectangle(roomX, storageCentral.Top, storageWidth, storageHeight);
                MakeHospitalStorageRoom(map, "storage", room);
            }
            map.SetTileModelAt(1, storageCentral.Top, m_Game.GameTiles.FLOOR_TILES);

            // 1 south corridor to other storage rooms.
            Rectangle southCorridorRect = Rectangle.FromLTRB(0, storageCentral.Bottom - 1, map.Width, storageCentral.Bottom - 1 + corridorHeight);
            TileRectangle(map, m_Game.GameTiles.WALL_HOSPITAL, southCorridorRect);
            map.SetTileModelAt(1, southCorridorRect.Top, m_Game.GameTiles.FLOOR_TILES);
            map.AddZone(MakeUniqueZone("south corridor", southCorridorRect));
            // storage rooms.
            Rectangle storageSouth = new Rectangle(2, southCorridorRect.Bottom - 1, map.Width - 2, storageHeight);
            for (int roomX = storageSouth.Left; roomX <= map.Width - storageWidth; roomX += storageWidth - 1)
            {
                Rectangle room = new Rectangle(roomX, storageSouth.Top, storageWidth, storageHeight);
                MakeHospitalStorageRoom(map, "storage", room);
            }
            map.SetTileModelAt(1, storageSouth.Top, m_Game.GameTiles.FLOOR_TILES);

            // done.
            return map;
        }

        Map GenerateHospital_Power(int seed)
        {
            //////////////////
            // 1. Create map.
            // 2. Floor plan.
            // 3. Populate.
            //////////////////

            // 1. Create map.
            Map map = new Map(seed, "Hospital - Power", 10, 10)
            {
                Lighting = Lighting.DARKNESS
            };
            DoForEachTile(map.Rect, (pt) => map.GetTileAt(pt).IsInside = true); //@@MP - unused parameter (Release 5-7)
            TileFill(map, m_Game.GameTiles.FLOOR_CONCRETE);
            TileRectangle(map, m_Game.GameTiles.WALL_BRICK, map.Rect);

            // 2. Floor plan.
            // one narrow corridor separated from the power gen room by iron fences.
            // barricade room for the Enraged Patient.

            // corridor with fences.
            Rectangle corridor = Rectangle.FromLTRB(1, 1, 3, map.Height);
            map.AddZone(MakeUniqueZone("corridor", corridor));
            for (int yFence = 1; yFence < map.Height - 2; yFence++)
                map.PlaceMapObjectAt(MakeObjKennelFence(GameImages.OBJ_IRON_FENCE), new Point(2, yFence));

            // power room.
            Rectangle room = Rectangle.FromLTRB(3, 0, map.Width, map.Height);
            map.AddZone(MakeUniqueZone("power room", room));

            // power generators.
            DoForEachTile(room, //@@MP - unused parameter (Release 5-7)
                (pt) =>
                {
                    if (pt.X == room.Left)
                        return;
                    if (!map.IsWalkable(pt))
                        return;
                    if (CountAdjWalls(map, pt) < 3)
                        return;

                    map.PlaceMapObjectAt(MakeObjPowerGenerator(GameImages.OBJ_POWERGEN_OFF, GameImages.OBJ_POWERGEN_ON), pt);
                });

            // 3. Populate.
            // deranged patient!
            ActorModel model = m_Game.GameActors.DerangedPatient;
            Actor jason = model.CreateNamed(m_Game.GameFactions.ThePsychopaths, "deranged patient", false, 0);
            jason.IsUnique = true;
            jason.Doll.AddDecoration(DollPart.SKIN, GameImages.ACTOR_DERANGED_PATIENT);
            GiveStartingSkillToActor(jason, Skills.IDs.TOUGH);
            GiveStartingSkillToActor(jason, Skills.IDs.TOUGH);
            GiveStartingSkillToActor(jason, Skills.IDs.TOUGH);
            GiveStartingSkillToActor(jason, Skills.IDs.STRONG);
            GiveStartingSkillToActor(jason, Skills.IDs.STRONG);
            GiveStartingSkillToActor(jason, Skills.IDs.STRONG);
            GiveStartingSkillToActor(jason, Skills.IDs.AGILE);
            GiveStartingSkillToActor(jason, Skills.IDs.AGILE);
            GiveStartingSkillToActor(jason, Skills.IDs.AGILE);
            GiveStartingSkillToActor(jason, Skills.IDs.HIGH_STAMINA);
            GiveStartingSkillToActor(jason, Skills.IDs.HIGH_STAMINA);
            GiveStartingSkillToActor(jason, Skills.IDs.HIGH_STAMINA);
            jason.Inventory.AddAll(MakeItemBonesaw());
            map.PlaceActorAt(jason, new Point(map.Width / 2, map.Height / 2));
            m_Game.Session.UniqueActors.DerangedPatient = new UniqueActor()
            {
                TheActor = jason,
                IsSpawned = true
            };

            // done.
            return map;
        }

        Actor CreateNewHospitalPatient() //@@MP - unused parameter (Release 5-7)
        {
            // decide model.
            ActorModel model = m_Rules.Roll(0, 2) == 0 ? m_Game.GameActors.MaleCivilian : m_Game.GameActors.FemaleCivilian;

            // create.
            Actor patient = model.CreateNumberedName(m_Game.GameFactions.TheCivilians, 0);
            SkinNakedHuman(m_DiceRoller, patient);
            GiveNameToActor(m_DiceRoller, patient);
            patient.Name = "Patient " + patient.Name;
            //patient.Controller = new CivilianAI();  // alpha10.1 defined by model like other actors         

            // skills.
            GiveRandomSkillsToActor(m_DiceRoller, patient, 1);

            // add patient uniform.
            patient.Doll.AddDecoration(DollPart.TORSO, GameImages.HOSPITAL_PATIENT_UNIFORM);

            // give items
            patient.Inventory.AddAll(MakeItemArmyRation());
            patient.Inventory.AddAll(MakeItemSnackBar());

            // done.
            return patient;
        }

        Actor CreateNewHospitalNurse() //@@MP - unused parameter (Release 5-7)
        {
            // create.
            Actor nurse = m_Game.GameActors.FemaleCivilian.CreateNumberedName(m_Game.GameFactions.TheCivilians, 0);
            SkinNakedHuman(m_DiceRoller, nurse);
            GiveNameToActor(m_DiceRoller, nurse);
            nurse.Name = "Nurse " + nurse.Name;
            //nurse.Controller = new CivilianAI(); // alpha10.1 defined by model like other actors

            // add uniform.
            nurse.Doll.AddDecoration(DollPart.TORSO, GameImages.HOSPITAL_NURSE_UNIFORM);

            // skills : 1 + 1-Medic.
            GiveStartingSkillToActor(nurse, Skills.IDs.MEDIC);
            GiveStartingSkillToActor(nurse, Skills.IDs.MEDIC);
            GiveStartingSkillToActor(nurse, Skills.IDs.MEDIC);
            GiveRandomSkillsToActor(m_DiceRoller, nurse, 1);

            // give items
            nurse.Inventory.AddAll(MakeHospitalItem());
            nurse.Inventory.AddAll(MakeHospitalItem());
            nurse.Inventory.AddAll(MakeHospitalItem());
            nurse.Inventory.AddAll(MakeHospitalItem());
            nurse.Inventory.AddAll(MakeItemFlashlight());
            nurse.Inventory.AddAll(MakeItemArmyRation());
            nurse.Inventory.AddAll(MakeItemArmyRation());

            // done.
            return nurse;
        }

        Actor CreateNewHospitalDoctor() //@@MP - unused parameter (Release 5-7)
        {
            // create.
            Actor doctor = m_Game.GameActors.MaleCivilian.CreateNumberedName(m_Game.GameFactions.TheCivilians, 0);
            SkinNakedHuman(m_DiceRoller, doctor);
            GiveNameToActor(m_DiceRoller, doctor);
            doctor.Name = "Doctor " + doctor.Name;
            //doctor.Controller = new CivilianAI(); // alpha10.1 defined by model like other actors

            // add uniform.
            doctor.Doll.AddDecoration(DollPart.TORSO, GameImages.HOSPITAL_DOCTOR_UNIFORM);

            // skills : 1 + 3-Medic + 1-Leadership.
            GiveStartingSkillToActor(doctor, Skills.IDs.MEDIC);
            GiveStartingSkillToActor(doctor, Skills.IDs.MEDIC);
            GiveStartingSkillToActor(doctor, Skills.IDs.MEDIC);
            GiveStartingSkillToActor(doctor, Skills.IDs.MEDIC);
            GiveStartingSkillToActor(doctor, Skills.IDs.MEDIC);
            GiveStartingSkillToActor(doctor, Skills.IDs.LEADERSHIP);
            GiveStartingSkillToActor(doctor, Skills.IDs.LEADERSHIP);
            GiveRandomSkillsToActor(m_DiceRoller, doctor, 1);

            // give items
            doctor.Inventory.AddAll(MakeItemLargeMedikit());
            doctor.Inventory.AddAll(MakeItemLargeMedikit());
            doctor.Inventory.AddAll(MakeItemSmallMedikit());
            doctor.Inventory.AddAll(MakeItemSmallMedikit());
            doctor.Inventory.AddAll(MakeItemFlashlight());
            doctor.Inventory.AddAll(MakeItemArmyRation());
            doctor.Inventory.AddAll(MakeItemArmyRation());

            // done.
            return doctor;
        }

        void MakeHospitalPatientRoom(Map map, string baseZoneName, Rectangle room, bool isFacingEast)
        {
            TileRectangle(map, m_Game.GameTiles.WALL_HOSPITAL, room);
            map.AddZone(MakeUniqueZone(baseZoneName, room));

            int xDoor = (isFacingEast ? room.Right - 1 : room.Left);

            // door in the corner.
            PlaceDoor(map, xDoor, room.Top + 1, m_Game.GameTiles.FLOOR_TILES, MakeObjHospitalDoor());

            // bed in the middle in the south.
            Point bedPos = new Point(room.Left + room.Width / 2, room.Bottom - 2);
            map.PlaceMapObjectAt(MakeObjBed(GameImages.OBJ_HOSPITAL_BED), bedPos);

            // chair and nighttable on either side of the bed.
            map.PlaceMapObjectAt(MakeObjChair(GameImages.OBJ_HOSPITAL_CHAIR), new Point(isFacingEast ? bedPos.X + 1 : bedPos.X - 1, bedPos.Y));
            Point tablePos = new Point(isFacingEast ? bedPos.X - 1 : bedPos.X + 1, bedPos.Y);
            map.PlaceMapObjectAt(MakeObjNightTable(GameImages.OBJ_HOSPITAL_NIGHT_TABLE), tablePos);

            // chance of some meds/food/book on nightable.
            if (m_DiceRoller.RollChance(50))
            {
                int roll = m_DiceRoller.Roll(0, 3);
                Item it = null;
                switch (roll)
                {
                    case 0: it = MakeShopPharmacyItem(); break;
                    case 1: it = MakeItemCannedFood(); break;
                    case 2:
                        if (RogueGame.Options.IsSanityEnabled) //@@MP - added check (Release 7-6)
                            it = MakeItemMagazines(m_DiceRoller);
                        else
                            it = MakeItemLargeMedikit();
                        break;
                }
                if (it != null)
                    map.DropItemAt(it, tablePos);
            }

            // wardrobe in the corner.
            map.PlaceMapObjectAt(MakeObjWardrobe(GameImages.OBJ_HOSPITAL_WARDROBE), new Point(isFacingEast ? room.Left + 1: room.Right - 2, room.Top + 1));
        }

        void MakeHospitalOfficeRoom(Map map, string baseZoneName, Rectangle room, bool isFacingEast)
        {
            TileFill(map, m_Game.GameTiles.FLOOR_PLANKS, room);
            TileRectangle(map, m_Game.GameTiles.WALL_HOSPITAL, room);
            map.AddZone(MakeUniqueZone(baseZoneName, room));

            int xDoor = (isFacingEast ? room.Right - 1 : room.Left);
            int yDoor = room.Top+2;

            // door in the middle.
            PlaceDoor(map, xDoor, yDoor, m_Game.GameTiles.FLOOR_TILES, MakeObjWoodenDoor());

            // chairs and table facing the door.
            int xTable = (isFacingEast ? room.Left + 2 : room.Right - 3);
            map.PlaceMapObjectAt(MakeObjTable(GameImages.OBJ_TABLE), new Point(xTable, yDoor));
            if (m_DiceRoller.RollChance(50)) //@@MP (Release 7-6)
                map.DropItemAt(MakeHospitalItem(), new Point(xTable, yDoor));
            map.PlaceMapObjectAt(MakeObjChair(GameImages.OBJ_CHAIR), new Point(xTable - 1, yDoor));
            map.PlaceMapObjectAt(MakeObjChair(GameImages.OBJ_CHAIR), new Point(xTable + 1, yDoor));
        }

        void MakeHospitalStorageRoom(Map map, string baseZoneName, Rectangle room)
        {
            TileRectangle(map, m_Game.GameTiles.WALL_HOSPITAL, room);
            map.AddZone(MakeUniqueZone(baseZoneName, room));

            // door.
            PlaceDoor(map, room.Left + 2, room.Top, m_Game.GameTiles.FLOOR_TILES, MakeObjHospitalDoor());

            // shelves with meds.
            DoForEachTile(room, //@@MP - unused parameter (Release 5-7)
                (pt) =>
                {
                    if (!map.IsWalkable(pt))
                        return;

                    if (CountAdjDoors(map, pt.X, pt.Y) > 0)
                        return;

                    // shelf.
                    map.PlaceMapObjectAt(MakeObjShelf(GameImages.OBJ_SHOP_SHELF), pt);

                    // full stacks of meds or canned food.
                    Item it;
                    it = m_DiceRoller.RollChance(80) ? MakeHospitalItem() : MakeItemCannedFood();
                    if (it.Model.IsStackable)
                        it.Quantity = it.Model.StackingLimit;
                    map.DropItemAt(it, pt);
                });


            // chance to spawn a nurse             // alpha10 
            if (m_DiceRoller.RollChance(20)) // alpha10.1 increased to 20% (avg 5 nurses for 24 storage rooms)
            {
                bool spawnedActor = false;
                DoForEachTile(room,
                    (pt) =>
                    {
                        if (spawnedActor)
                            return;
                        if (!map.IsWalkable(pt))
                            return;
                        if (map.GetMapObjectAt(pt) != null)
                            return;
                        map.PlaceActorAt(CreateNewHospitalNurse(), pt);
                        spawnedActor = true;
                    });
            }
        }

#endregion

#region Shopping mall
        //@@MP (Release 7-3)
        /// <summary>
        /// Layout :
        ///  0 floor: Entry level.
        /// +1 floor: Food court, cinemas and supermarket.
        /// -1 floor: Car park. Power. (restore power to the whole building = lights)
        /// </summary>
        /// <param name="map"></param>
        void MakeShoppingMall(Map map, List<Block> freeBlocks, out Block mallBlock)
        {
            ////////////////////////////////
            // 1. Generate surface building.
            // 2. Generate other levels maps.
            // 3. Link maps.
            // 4. Add maps to district.
            // 5. Set unique linked maps.
            ////////////////////////////////

            mallBlock = freeBlocks[0];//[m_DiceRoller.Roll(0, freeBlocks.Count)];

            // 1. Generate ground floor.
            GenerateShoppingMallGroundFloor(map, mallBlock);

            // 2. Generate other levels maps.
            Map upperlevel = GenerateShoppingMall_UpperLevel((map.Seed >> 1) ^ map.Seed);
            Map parking = GenerateShoppingMall_Parking((map.Seed << 1) ^ map.Seed);

            // alpha10 music
            map.BgMusic = upperlevel.BgMusic = parking.BgMusic = GameMusics.SHOPPING_MALL;

            // 3. Link maps.
#region
            int l = mallBlock.InsideRect.Left, t = mallBlock.InsideRect.Top;
            // ground <-> upper level
            Point groundStairs1 = new Point(l+13,t+21); //ground
            Point upperStairs1 = new Point(18,25); //upper
            AddExit(map, groundStairs1, upperlevel, upperStairs1, GameImages.DECO_STAIRS_UP, true);
            AddExit(upperlevel, upperStairs1, map, groundStairs1, GameImages.DECO_STAIRS_DOWN, true);
            Point groundStairs2 = new Point(l+13, t+22);
            Point upperStairs2 = new Point(18, 26);
            AddExit(map, groundStairs2, upperlevel, upperStairs2, GameImages.DECO_STAIRS_UP, true);
            AddExit(upperlevel, upperStairs2, map, groundStairs2, GameImages.DECO_STAIRS_DOWN, true);
            Point groundStairs3 = new Point(l+29, t+21); //ground
            Point upperStairs3 = new Point(34, 25); //upper
            AddExit(map, groundStairs3, upperlevel, upperStairs3, GameImages.DECO_STAIRS_UP, true);
            AddExit(upperlevel, upperStairs3, map, groundStairs3, GameImages.DECO_STAIRS_DOWN, true);
            Point groundStairs4 = new Point(l+29, t+22);
            Point upperStairs4 = new Point(34, 26);
            AddExit(map, groundStairs4, upperlevel, upperStairs4, GameImages.DECO_STAIRS_UP, true);
            AddExit(upperlevel, upperStairs4, map, groundStairs4, GameImages.DECO_STAIRS_DOWN, true);

            // ground <-> parking
            Point groundStairs5 = new Point(l+21, t+21);
            Point parkingStairs1 = new Point(1, 25);
            AddExit(map, groundStairs5, parking, parkingStairs1, GameImages.DECO_STAIRS_DOWN, true);
            AddExit(parking, parkingStairs1, map, groundStairs5, GameImages.DECO_STAIRS_UP, true);
            Point groundStairs6 = new Point(l+21, t+22); //ground
            Point parkingStairs2 = new Point(1, 26); //upper
            AddExit(map, groundStairs6, parking, parkingStairs2, GameImages.DECO_STAIRS_DOWN, true);
            AddExit(parking, parkingStairs2, map, groundStairs6, GameImages.DECO_STAIRS_UP, true);
#endregion

            // 4. Add linked maps to district.
            m_Params.District.AddUniqueMap(map);
            m_Params.District.AddUniqueMap(upperlevel);
            m_Params.District.AddUniqueMap(parking);

            // 5. Set unique linked maps.
            m_Game.Session.UniqueMaps.ShoppingMall_GroundFloor = new UniqueMap() { TheMap = map };
            m_Game.Session.UniqueMaps.ShoppingMall_UpperLevel = new UniqueMap() { TheMap = upperlevel };
            m_Game.Session.UniqueMaps.ShoppingMall_Parking = new UniqueMap() { TheMap = parking };

            // done!
        }

        void GenerateShoppingMallGroundFloor(Map surfaceMap, Block block)
        {
            // Fill & Enclose Building.
            TileRectangle(surfaceMap, m_Game.GameTiles.FLOOR_WALKWAY, block.Rectangle);
            TileRectangle(surfaceMap, m_Game.GameTiles.WALL_MALL, block.BuildingRect);
            TileFill(surfaceMap, m_Game.GameTiles.FLOOR_WHITE_TILE, block.InsideRect);
            DoForEachTile(block.InsideRect, (pt) => surfaceMap.GetTileAt(pt).IsInside = true);
            int l = block.InsideRect.Left - 1, t = block.InsideRect.Top - 1, b = block.InsideRect.Bottom, r = block.InsideRect.Right; //to fix jank

            // 1. mall entrances with signs.
#region
            List<Point> doorList = new List<Point> {
                new Point(l, t+21), new Point(l, t+22), new Point(l, t+23), new Point(l, t+24), //east
                new Point(l+13, t), new Point(l+14, t), new Point(l+15, t), new Point(l+29, t), new Point(l+30, t), new Point(l+31, t), //north
                new Point(r, t+21), new Point(r, t+22), new Point(r, t+23), new Point(r, t+24), //east
                new Point(l+13, b), new Point(l+14, b), new Point(l+15, b), new Point(l+29, b), new Point(l+30, b), new Point(l+31, b) //south
            };
            foreach (Point doorPoint in doorList)
            {
                surfaceMap.RemoveMapObjectAt(doorPoint.X, doorPoint.Y);
                PlaceDoor(surfaceMap, doorPoint.X, doorPoint.Y, m_Game.GameTiles.FLOOR_WHITE_TILE, MakeObjGlassDoor());
            }
            List<Point> the = new List<Point> { new Point(l+12, t), new Point(l+28,t), new Point(r,t+25), new Point(l+12,b), new Point(l+28,b), new Point(l,t+20) };
            foreach (Point thePoint in the)
                surfaceMap.GetTileAt(thePoint.X, thePoint.Y).AddDecoration(GameImages.DECO_MALL_SIGN_THE);

            List<Point> mall = new List<Point> { new Point(l+16, t), new Point(l+32,t), new Point(r,t+20), new Point(l+16,b), new Point(l+32,b), new Point(l, t+25) };
            foreach (Point mallPoint in mall)
                surfaceMap.GetTileAt(mallPoint.X, mallPoint.Y).AddDecoration(GameImages.DECO_MALL_SIGN_MALL);
#endregion
            
            // 2. seats, plants, bins and registers
#region
            List<Point> seats = new List<Point> { new Point(l+15, t+8), new Point(l+15, t+9), new Point(l+15, t+10), new Point(l+29, t+8), new Point(l+29, t+9), new Point(l+29, t+10),
                new Point(l+15,t+33), new Point(l+15, t+34), new Point(l+15, t+35), new Point(l+29, t+33), new Point(l+29, t+34), new Point(l+29, t+35)};
            foreach (Point seatPoint in seats)
                surfaceMap.PlaceMapObjectAt(MakeObjBench(GameImages.OBJ_BENCH), new Point(seatPoint.X, seatPoint.Y));

            List<Point> plants = new List<Point> { new Point(l+15, t+7), new Point(l+15, t+11), new Point(l+29, t+7), new Point(l+29, t+11),
                new Point(l+15, t+32), new Point(l+15, t+36), new Point(l+29, t+32), new Point(l+29, t+36) };
            foreach (Point plantPoint in plants)
                surfaceMap.PlaceMapObjectAt(MakeObjPottedPlant(GameImages.OBJ_POTTED_PLANT), new Point(plantPoint.X, plantPoint.Y));

            List<Point> bins = new List<Point> { new Point(15, 24), new Point(15, 25), new Point(33, 24), new Point(33, 25) };
            foreach (Point binPoint in bins)
                surfaceMap.PlaceMapObjectAt(MakeObjFireBarrel(GameImages.OBJ_EMPTY_BIN), new Point(binPoint.X, binPoint.Y));

            List<Point> registers = new List<Point> { new Point(l+1, t+9), new Point(l+17, t+9), new Point(l+33, t+1), new Point(l+1, t+19), new Point(l+27, t+19), new Point(l+44, t+11),
                new Point(l+17, t+26), new Point(l+33, t+34), new Point(l+11, t+44), new Point(l+17, t+36), new Point(l+33, t+36)};
            foreach (Point registerPoint in registers)
                surfaceMap.PlaceMapObjectAt(MakeObjCheckout(GameImages.OBJ_CLINIC_DESK), new Point(registerPoint.X, registerPoint.Y));
            #endregion

            // 3. Make shops
            // walls
            #region
            Rectangle shopRect1 = new Rectangle(l, t, 13, 11);
            Block shopBlock1 = new Block(shopRect1); //barber
            TileRectangle(surfaceMap, m_Game.GameTiles.WALL_MALL, shopRect1);
            TileFill(surfaceMap, m_Game.GameTiles.FLOOR_PLANKS, shopBlock1.BuildingRect);

            Rectangle shopRect2 = new Rectangle(l+16, t, 13, 11);
            Block shopBlock2 = new Block(shopRect2); //books
            TileRectangle(surfaceMap, m_Game.GameTiles.WALL_MALL, shopRect2);
            TileFill(surfaceMap, m_Game.GameTiles.FLOOR_BLUE_CARPET, shopBlock2.BuildingRect);

            Rectangle shopRect3 = new Rectangle(l+32, t, 14, 11);
            Block shopBlock3 = new Block(shopRect3); //clothing
            TileRectangle(surfaceMap, m_Game.GameTiles.WALL_MALL, shopRect3);
            TileFill(surfaceMap, m_Game.GameTiles.FLOOR_BLUE_CARPET, shopBlock3.BuildingRect);

            Rectangle shopRect4 = new Rectangle(l, t+10, 13, 11);
            Block shopBlock4 = new Block(shopRect4); //clothing
            TileRectangle(surfaceMap, m_Game.GameTiles.WALL_MALL, shopRect4);
            TileFill(surfaceMap, m_Game.GameTiles.FLOOR_WHITE_TILE, shopBlock4.BuildingRect);

            Rectangle shopRect5 = new Rectangle(l+16, t+10, 13, 11);
            Block shopBlock5 = new Block(shopRect5); //clothing
            TileRectangle(surfaceMap, m_Game.GameTiles.WALL_MALL, shopRect5);
            TileFill(surfaceMap, m_Game.GameTiles.FLOOR_TILES, shopBlock5.BuildingRect);

            Rectangle shopRect6 = new Rectangle(l+32, t+10, 14, 11);
            Block shopBlock6 = new Block(shopRect6); //sporting goods
            TileRectangle(surfaceMap, m_Game.GameTiles.WALL_MALL, shopRect6);
            TileFill(surfaceMap, m_Game.GameTiles.FLOOR_WHITE_TILE, shopBlock6.BuildingRect);

            Rectangle shopRect7 = new Rectangle(l, t+25, 13, 11);
            Block shopBlock7 = new Block(shopRect7); //electronics
            TileRectangle(surfaceMap, m_Game.GameTiles.WALL_MALL, shopRect7);
            TileFill(surfaceMap, m_Game.GameTiles.FLOOR_BLUE_CARPET, shopBlock7.BuildingRect);

            Rectangle shopRect8 = new Rectangle(l+16, t+25, 13, 11);
            Block shopBlock8 = new Block(shopRect8); //mobiles
            TileRectangle(surfaceMap, m_Game.GameTiles.WALL_MALL, shopRect8);
            TileFill(surfaceMap, m_Game.GameTiles.FLOOR_PLANKS, shopBlock8.BuildingRect);

            Rectangle shopRect9 = new Rectangle(l+32, t+25, 14, 11);
            Block shopBlock9 = new Block(shopRect9); //books
            TileRectangle(surfaceMap, m_Game.GameTiles.WALL_MALL, shopRect9);
            TileFill(surfaceMap, m_Game.GameTiles.FLOOR_RED_CARPET, shopBlock9.BuildingRect);

            Rectangle shopRect10 = new Rectangle(l, t+35, 13, 11);
            Block shopBlock10 = new Block(shopRect10); //pharmacy
            TileRectangle(surfaceMap, m_Game.GameTiles.WALL_MALL, shopRect10);
            TileFill(surfaceMap, m_Game.GameTiles.FLOOR_OFFICE, shopBlock10.BuildingRect);

            Rectangle shopRect11 = new Rectangle(l+16, t+35, 13, 11);
            Block shopBlock11 = new Block(shopRect11); //liquor
            TileRectangle(surfaceMap, m_Game.GameTiles.WALL_MALL, shopRect11);
            TileFill(surfaceMap, m_Game.GameTiles.FLOOR_TILES, shopBlock11.BuildingRect);

            Rectangle shopRect12 = new Rectangle(l+32, t+35, 14, 11);
            Block shopBlock12 = new Block(shopRect12); //clothing
            TileRectangle(surfaceMap, m_Game.GameTiles.WALL_MALL, shopRect12);
            TileFill(surfaceMap, m_Game.GameTiles.FLOOR_PLANKS, shopBlock12.BuildingRect);
#endregion

            // make entryways for each shop
            KeyValuePairWithDuplicates entryPoints = new KeyValuePairWithDuplicates() {
                {12,3},{12,4},{12,5},{16,3},{16,4},{16,5},{28,3},{28,4},{28,5},{32,3},{32,4},{32,5}, //, stores 1-3, north/south
                {12,17},{12,18},{12,19},{12,20},{16,13},{16,14},{16,15},{28,13},{28,14},{28,15},{32,17},{32,18},{32,19},{32,20}, //stores 4-6, north/south
                {9,20},{10,20},{11,20},{21,20},{22,20},{23,20},{33,20},{34,20},{35,20}, //stores 4-6, east/west
                {9,25},{10,25},{11,25},{12,25},{21,25},{22,25},{23,25},{32,25},{33,25},{34,25},{35,25}, //stores 7-9, east/west
                {12,26},{12,27},{12,28},{16,28},{16,29},{16,30},{28,28},{28,29},{28,30},{32,26},{32,27},{32,28}, //stores 7-9, north/south
                {12,38},{12,39},{12,40},{16,38},{16,39},{16,40},{28,38},{28,39},{28,40},{32,38},{32,39},{32,40} //stores 10-12, north/south
            };
            foreach (KeyValuePair<int, int> entryPoint in entryPoints)
            {
                surfaceMap.SetTileModelAt(l + entryPoint.Key, t + entryPoint.Value, m_Game.GameTiles.FLOOR_WHITE_TILE);
            };
            
            // add signage to each store
#region
            // stores 1-3
            surfaceMap.GetTileAt(l+12, t+2).AddDecoration(GameImages.DECO_SHOP_BARBER);
            surfaceMap.GetTileAt(l+12, t+6).AddDecoration(GameImages.DECO_SHOP_BARBER);
            surfaceMap.GetTileAt(l+16, t+2).AddDecoration(GameImages.DECO_SHOP_BOOKSTORE);
            surfaceMap.GetTileAt(l+16, t+6).AddDecoration(GameImages.DECO_SHOP_BOOKSTORE);
            surfaceMap.GetTileAt(l+28, t+2).AddDecoration(GameImages.DECO_SHOP_BOOKSTORE);
            surfaceMap.GetTileAt(l+28, t+6).AddDecoration(GameImages.DECO_SHOP_BOOKSTORE);
            surfaceMap.GetTileAt(l+32, t+2).AddDecoration(GameImages.DECO_SHOP_CLOTHES_STORE);
            surfaceMap.GetTileAt(l+32, t+6).AddDecoration(GameImages.DECO_SHOP_CLOTHES_STORE);
            // stores 4-6
            // --north-south
            surfaceMap.GetTileAt(l+12, t+16).AddDecoration(GameImages.DECO_SHOP_CLOTHES_STORE);
            surfaceMap.GetTileAt(l+16, t+12).AddDecoration(GameImages.DECO_SHOP_SPORTSWEAR);
            surfaceMap.GetTileAt(l+16, t+16).AddDecoration(GameImages.DECO_SHOP_SPORTSWEAR);
            surfaceMap.GetTileAt(l+28, t+12).AddDecoration(GameImages.DECO_SHOP_SPORTSWEAR);
            surfaceMap.GetTileAt(l+28, t+16).AddDecoration(GameImages.DECO_SHOP_SPORTSWEAR);
            surfaceMap.GetTileAt(l+32, t+16).AddDecoration(GameImages.DECO_SHOP_ELECTRONICS);
            // --east-west
            surfaceMap.GetTileAt(l+8, t+20).AddDecoration(GameImages.DECO_SHOP_CLOTHES_STORE);
            surfaceMap.GetTileAt(l+20, t+20).AddDecoration(GameImages.DECO_SHOP_SPORTSWEAR);
            surfaceMap.GetTileAt(l+24, t+20).AddDecoration(GameImages.DECO_SHOP_SPORTSWEAR);
            surfaceMap.GetTileAt(l+36, t+20).AddDecoration(GameImages.DECO_SHOP_ELECTRONICS);
            // stores 7-9
            // --north-south
            surfaceMap.GetTileAt(l+12, t+29).AddDecoration(GameImages.DECO_SHOP_DEALERSHIP);
            surfaceMap.GetTileAt(l+16, t+27).AddDecoration(GameImages.DECO_SHOP_MOBILES);
            surfaceMap.GetTileAt(l+16, t+31).AddDecoration(GameImages.DECO_SHOP_MOBILES);
            surfaceMap.GetTileAt(l+28, t+27).AddDecoration(GameImages.DECO_SHOP_MOBILES);
            surfaceMap.GetTileAt(l+28, t+31).AddDecoration(GameImages.DECO_SHOP_MOBILES);
            surfaceMap.GetTileAt(l+32, t+29).AddDecoration(GameImages.DECO_SHOP_BOOKSTORE);
            // --east-west
            surfaceMap.GetTileAt(l+8, t+25).AddDecoration(GameImages.DECO_SHOP_DEALERSHIP);
            surfaceMap.GetTileAt(l+20, t+25).AddDecoration(GameImages.DECO_SHOP_MOBILES);
            surfaceMap.GetTileAt(l+24, t+25).AddDecoration(GameImages.DECO_SHOP_MOBILES);
            surfaceMap.GetTileAt(l+36, t+25).AddDecoration(GameImages.DECO_SHOP_BOOKSTORE);
            // stores 10-12
            surfaceMap.GetTileAt(l+12, t+37).AddDecoration(GameImages.DECO_SHOP_PHARMACY);
            surfaceMap.GetTileAt(l+12, t+41).AddDecoration(GameImages.DECO_SHOP_PHARMACY);
            surfaceMap.GetTileAt(l+16, t+37).AddDecoration(GameImages.DECO_SHOP_LIQUOR);
            surfaceMap.GetTileAt(l+16, t+41).AddDecoration(GameImages.DECO_SHOP_LIQUOR);
            surfaceMap.GetTileAt(l+28, t+37).AddDecoration(GameImages.DECO_SHOP_LIQUOR);
            surfaceMap.GetTileAt(l+28, t+41).AddDecoration(GameImages.DECO_SHOP_LIQUOR);
            surfaceMap.GetTileAt(l+32, t+37).AddDecoration(GameImages.DECO_SHOP_CLOTHES_STORE);
            surfaceMap.GetTileAt(l+32, t+41).AddDecoration(GameImages.DECO_SHOP_CLOTHES_STORE);
#endregion
            
            // create displays/shelves
            Dictionary<Block, MallShopType> shops = new Dictionary<Block, MallShopType>() { { shopBlock1, MallShopType.BARBER },{ shopBlock2,MallShopType.BOOKSTORE },{ shopBlock3,MallShopType.CLOTHING },
                { shopBlock4,MallShopType.CLOTHING },{ shopBlock5,MallShopType.SPORTING_GOODS },{ shopBlock6,MallShopType.ELECTRONICS },{ shopBlock8,MallShopType.MOBILES },
                { shopBlock9,MallShopType.BOOKSTORE },{ shopBlock10,MallShopType.PHARMACY },{ shopBlock11,MallShopType.LIQUOR },{ shopBlock12,MallShopType.CLOTHING } };
            
            foreach (KeyValuePair<Block, MallShopType> shop in shops)
            {
                MakeMallShopDisplays(surfaceMap, shop.Key, shop.Value);
            };

            // other shops without the normal shelves style
#region Barber shop
            // Barber shop has a slightly different layout to all the other ordinary stores     //@@MP (Release 7-6)
            // Assumes that the barber shop is #1 and that the position and dimension haven't been altered.
            // Replace the two outer rows (northmost and southmost) of display tables with chairs and adjacent sinks.

            DoForEachTile(shopRect1, (pt) =>
            {
                MapObject shopObj = surfaceMap.GetMapObjectAt(pt);
                if (shopObj != null && shopObj.TheName == "the wigs display") //it's a display for wigs
                {
                    //only check north and south of the object, as all the displays on the ends of four rows are adjacent to the west or east wall
                    if (!surfaceMap.GetTileAt(new Point(pt.X, pt.Y - 2)).Model.IsWalkable) //there's a wall 2 tiles to the north, so this is an outer row of displays
                    {
                        surfaceMap.RemoveMapObjectAt(pt.X, pt.Y); //get rid of the wig display so we can replace it
                        surfaceMap.PlaceMapObjectAt(MakeObjChair(GameImages.OBJ_BARBER_CHAIR), pt);
                        surfaceMap.PlaceMapObjectAt(MakeObjBathroomBasin(GameImages.OBJ_BATHROOM_BASIN), new Point(pt.X, pt.Y - 1));
                    }
                    else if (!surfaceMap.GetTileAt(new Point(pt.X, pt.Y + 2)).Model.IsWalkable) //there's a wall 2 tiles to the north, so this is an outer row of displays
                    {
                        surfaceMap.RemoveMapObjectAt(pt.X, pt.Y); //get rid of the wig display so we can replace it
                        surfaceMap.PlaceMapObjectAt(MakeObjChair(GameImages.OBJ_BARBER_CHAIR), pt);
                        surfaceMap.PlaceMapObjectAt(MakeObjBathroomBasin(GameImages.OBJ_BATHROOM_BASIN), new Point(pt.X, pt.Y + 1));
                    }
                }
                else if (shopObj != null && shopObj.TheName == "the checkout")
                    surfaceMap.RemoveMapObjectAt(pt.X, pt.Y); //remove the checkout as it looks out of place here

            });
#endregion
#region Car dealership
            //cars
            surfaceMap.PlaceMapObjectAt(MakeObjDisplayCar(m_DiceRoller), new Point(l+3, t+28));
            surfaceMap.PlaceMapObjectAt(MakeObjDisplayCar(m_DiceRoller), new Point(l+6, t+27));
            surfaceMap.PlaceMapObjectAt(MakeObjDisplayCar(m_DiceRoller), new Point(l+9, t+28));
            surfaceMap.PlaceMapObjectAt(MakeObjDisplayCar(m_DiceRoller), new Point(l+5, t+30));
            surfaceMap.PlaceMapObjectAt(MakeObjDisplayCar(m_DiceRoller), new Point(l+7, t+30));
            surfaceMap.PlaceMapObjectAt(MakeObjDisplayCar(m_DiceRoller), new Point(l+3, t+32));
            surfaceMap.PlaceMapObjectAt(MakeObjDisplayCar(m_DiceRoller), new Point(l+6, t+33));
            surfaceMap.PlaceMapObjectAt(MakeObjDisplayCar(m_DiceRoller), new Point(l+9, t+32));
            //tables, chairs, couches
            surfaceMap.PlaceMapObjectAt(MakeObjChair(GameImages.OBJ_CHAIR), new Point(l+2, t+26));
            surfaceMap.PlaceMapObjectAt(MakeObjTable(GameImages.OBJ_TABLE), new Point(l+1, t+26));
            surfaceMap.PlaceMapObjectAt(MakeObjChair(GameImages.OBJ_CHAIR), new Point(l+1, t+27));
            surfaceMap.PlaceMapObjectAt(MakeObjCouch(GameImages.OBJ_COUCH), new Point(l+1, t+29));
            surfaceMap.PlaceMapObjectAt(MakeObjCouch(GameImages.OBJ_COUCH), new Point(l+1, t+30));
            surfaceMap.PlaceMapObjectAt(MakeObjCouch(GameImages.OBJ_COUCH), new Point(l+1, t+31));
            surfaceMap.PlaceMapObjectAt(MakeObjChair(GameImages.OBJ_CHAIR), new Point(l+1, t+33));
            surfaceMap.PlaceMapObjectAt(MakeObjTable(GameImages.OBJ_TABLE), new Point(l+1, t+34));
            surfaceMap.PlaceMapObjectAt(MakeObjChair(GameImages.OBJ_CHAIR), new Point(l+2, t+34));
            surfaceMap.PlaceMapObjectAt(MakeObjChair(GameImages.OBJ_CHAIR), new Point(l+11, t+33));
            surfaceMap.PlaceMapObjectAt(MakeObjTable(GameImages.OBJ_TABLE), new Point(l+11, t+34));
            surfaceMap.PlaceMapObjectAt(MakeObjChair(GameImages.OBJ_CHAIR), new Point(l+10, t+34));
#endregion

            // Zone.
            //surfaceMap.Lighting = Lighting.OUTSIDE;  //@@MP - doesn't work, makes the sky not visible even when outside for some reason...
            surfaceMap.AddZone(MakeUniqueZone("Shopping Mall", block.BuildingRect));
            MakeWalkwayZones(surfaceMap, block);
        }

        Map GenerateShoppingMall_UpperLevel(int seed)
        {
            //////////////////
            // 1. Create map.
            // 2. Floor plan.
            //////////////////

            // 1. Create map.
            Map map = new Map(seed, "Shopping Mall - Upper Level", 51, 51)
            {
                Lighting = Lighting.DARKNESS,
                HasWaterTiles = true //the pool with plam trees. HasWaterTiles is used by the AI if they are on fire and looking for somewhere to extinguish themselves
            };
            DoForEachTile(map.Rect, (pt) => map.GetTileAt(pt).IsInside = true);
            TileFill(map, m_Game.GameTiles.FLOOR_WHITE_TILE);
            TileRectangle(map, m_Game.GameTiles.WALL_MALL, map.Rect);

            // Floor plan.
            // 2. top left: food court
            // 3. top right: supermarket
            // 4. central row
            // 5. bottom: cinemas

#region Food court
            // zone
            Rectangle foodCourtRect = new Rectangle(1, 1, 25, 22);
            TileRectangle(map, m_Game.GameTiles.WALL_MALL, new Rectangle(1,22,4,1)); //little piece of wall running internally
            map.AddZone(MakeUniqueZone("food court", foodCourtRect));

            // counters
            MapObjectFill(map, new Rectangle(1, 2, 24, 1),
                (pt) =>
                {
                    //place the price board on the wall behind the counter
                    string priceBoardImage = null;
                    switch (m_DiceRoller.Roll(0, 5))
                    {
                        case 0: priceBoardImage = GameImages.DECO_FOOD_COURT_PRICEBOARD1; break;
                        case 1: priceBoardImage = GameImages.DECO_FOOD_COURT_PRICEBOARD2; break;
                        case 2: priceBoardImage = GameImages.DECO_FOOD_COURT_PRICEBOARD3; break;
                        case 3: priceBoardImage = GameImages.DECO_FOOD_COURT_PRICEBOARD4; break;
                        case 4: priceBoardImage = GameImages.DECO_FOOD_COURT_PRICEBOARD5; break;
                    }
                    Point priceBoardPT = new Point(pt.X, pt.Y - 2);
                    map.GetTileAt(priceBoardPT).AddDecoration(priceBoardImage);

                    //select which counter we'll have
                    string counterImage = null;
                    switch (m_DiceRoller.Roll(0,5))
                    {
                        case 0: counterImage = GameImages.OBJ_FOOD_COURT_COUNTER1; break;
                        case 1: counterImage = GameImages.OBJ_FOOD_COURT_COUNTER2; break;
                        case 2: counterImage = GameImages.OBJ_FOOD_COURT_COUNTER3; break;
                        case 3: counterImage = GameImages.OBJ_FOOD_COURT_COUNTER4; break;
                        case 4: counterImage = GameImages.OBJ_FOOD_COURT_COUNTER5; break;
                    }

                    //add an item to the counter            //@@MP (Release 7-6)
                    int roll = m_DiceRoller.Roll(0, 4);
                    switch (roll)
                    {
                        case 0: map.DropItemAt(MakeItemCookedChicken(), pt); break;
                        case 1: map.DropItemAt(MakeItemFryingPan(), pt); break;
                        case 2: map.DropItemAt(MakeItemCleaver(), pt); break;
                        case 3: map.DropItemAt(MakeItemKitchenKnife(), pt); break;
                    }

                    //now place the counter
                    return MakeObjCounter(counterImage);
                });

            // chairs and tables
            KeyValuePairWithDuplicates tablePoints = new KeyValuePairWithDuplicates() {
                {3,7},{3,11},{3,15},{3,19},{7,7},{7,11},{7,15},{7,19},{11,7},{11,19},{16,7},{16,19},
                {20,7},{20,11},{20,15},{20,19},{24,7},{24,11},{24,15},{24,19}
            };

            foreach (KeyValuePair<int, int> tablePoint in tablePoints)
            {
                Point tablePT = new Point(tablePoint.Key, tablePoint.Value);
                //central table with a chair each at NSEW
                map.PlaceMapObjectAt(MakeObjTable(GameImages.OBJ_FOOD_COURT_TABLE), tablePT);
                foreach (Direction d in Direction.COMPASS_NSEW)
                {
                    Point next = tablePT + d;
                    map.PlaceMapObjectAt(MakeObjChair(GameImages.OBJ_FOOD_COURT_CHAIR), next);
                }
            };

            // pool with palm trees
            //-outer edge
            Rectangle poolOuter = new Rectangle(11, 11, 6, 5);
            TileRectangle(map, m_Game.GameTiles.FLOOR_CONCRETE, poolOuter);
            //-plants on corners
            map.PlaceMapObjectAt(MakeObjPottedPlant(GameImages.OBJ_POTTED_PLANT), new Point(11, 11));
            map.PlaceMapObjectAt(MakeObjPottedPlant(GameImages.OBJ_POTTED_PLANT), new Point(11, 15));
            map.PlaceMapObjectAt(MakeObjPottedPlant(GameImages.OBJ_POTTED_PLANT), new Point(16, 11));
            map.PlaceMapObjectAt(MakeObjPottedPlant(GameImages.OBJ_POTTED_PLANT), new Point(16, 15));
            //-pool
            Rectangle poolWater = new Rectangle(12, 12, 4, 3);
            TileRectangle(map, m_Game.GameTiles.FLOOR_FOOD_COURT_POOL, poolWater);
            //-palm tree in the middle
            map.PlaceMapObjectAt(MakeObjTree(GameImages.OBJ_FOOD_COURT_PALM_TREE), new Point(13, 13));
            map.PlaceMapObjectAt(MakeObjTree(GameImages.OBJ_FOOD_COURT_PALM_TREE), new Point(14, 13));

            //bins dotted around the place
            map.PlaceMapObjectAt(MakeObjFireBarrel(GameImages.OBJ_EMPTY_BIN), new Point(1, 5));
            map.PlaceMapObjectAt(MakeObjFireBarrel(GameImages.OBJ_EMPTY_BIN), new Point(1, 21));
            map.PlaceMapObjectAt(MakeObjFireBarrel(GameImages.OBJ_EMPTY_BIN), new Point(10, 13));
            map.PlaceMapObjectAt(MakeObjFireBarrel(GameImages.OBJ_EMPTY_BIN), new Point(13, 10));
            map.PlaceMapObjectAt(MakeObjFireBarrel(GameImages.OBJ_EMPTY_BIN), new Point(14, 10));
            map.PlaceMapObjectAt(MakeObjFireBarrel(GameImages.OBJ_EMPTY_BIN), new Point(13, 16));
            map.PlaceMapObjectAt(MakeObjFireBarrel(GameImages.OBJ_EMPTY_BIN), new Point(14, 16));
            map.PlaceMapObjectAt(MakeObjFireBarrel(GameImages.OBJ_EMPTY_BIN), new Point(17, 13));
            map.PlaceMapObjectAt(MakeObjFireBarrel(GameImages.OBJ_EMPTY_BIN), new Point(13, 22));
            map.PlaceMapObjectAt(MakeObjFireBarrel(GameImages.OBJ_EMPTY_BIN), new Point(14, 22));
            map.PlaceMapObjectAt(MakeObjFireBarrel(GameImages.OBJ_EMPTY_BIN), new Point(25, 22));
            #endregion

#region Supermarket
            //walls and floor
            Rectangle supermarketRect = new Rectangle(26, 1, 25, 22);
            TileRectangle(map, m_Game.GameTiles.WALL_MALL, supermarketRect);
            TileFill(map, m_Game.GameTiles.FLOOR_TILES, new Rectangle(supermarketRect.Left + 1, supermarketRect.Top + 1, 23, 20));
            map.AddZone(MakeUniqueZone("Supermarket", supermarketRect));
            //entry
            map.GetTileAt(27, 22).AddDecoration(GameImages.DECO_SHOP_GROCERY);
            TileRectangle(map, m_Game.GameTiles.FLOOR_WHITE_TILE, new Rectangle(28, 22, 5, 1));
            map.GetTileAt(33, 22).AddDecoration(GameImages.DECO_SHOP_GROCERY);
            //shelves
#region
            Rectangle supermarketInsideRect = new Rectangle(27, 2, 23, 17);
            int alleysStartX = supermarketInsideRect.Left;
            int alleysStartY = supermarketInsideRect.Top;
            int alleysEndX = supermarketInsideRect.Right;
            int alleysEndY = supermarketInsideRect.Bottom;
            bool horizontalAlleys = supermarketInsideRect.Width >= supermarketInsideRect.Height;
            int centralAlley;

            if (horizontalAlleys)
            {
                ++alleysStartX;
                --alleysEndX;
                centralAlley = supermarketInsideRect.Left + supermarketInsideRect.Width / 2;
            }
            else
            {
                ++alleysStartY;
                --alleysEndY;
                centralAlley = supermarketInsideRect.Top + supermarketInsideRect.Height / 2;
            }
            Rectangle alleysRect = Rectangle.FromLTRB(alleysStartX, alleysStartY, alleysEndX, alleysEndY);

            MapObjectFill(map, alleysRect,
                (pt) =>
                {
                    bool addShelf;

                    if (horizontalAlleys)
                        addShelf = ((pt.Y - alleysRect.Top) % 2 == 1) && pt.X != centralAlley;
                    else
                        addShelf = ((pt.X - alleysRect.Left) % 2 == 1) && pt.Y != centralAlley;

                    if (addShelf)
                    {
                        map.DropItemAt(MakeShopGroceryItem(), pt);
                        return MakeObjShelf(GameImages.OBJ_SHOP_SHELF);
                    }
                    else
                        return null;
                });
#endregion
            //checkouts
            map.PlaceMapObjectAt(MakeObjCheckout(GameImages.OBJ_SUPERMARKET_CHECKOUT), new Point(34, 20));
            map.PlaceMapObjectAt(MakeObjCheckout(GameImages.OBJ_SUPERMARKET_CHECKOUT), new Point(36, 20));
            map.PlaceMapObjectAt(MakeObjCheckout(GameImages.OBJ_SUPERMARKET_CHECKOUT), new Point(38, 20));
            map.PlaceMapObjectAt(MakeObjCheckout(GameImages.OBJ_SUPERMARKET_CHECKOUT), new Point(40, 20));
            map.PlaceMapObjectAt(MakeObjCheckout(GameImages.OBJ_SUPERMARKET_CHECKOUT), new Point(42, 20));
            map.PlaceMapObjectAt(MakeObjCheckout(GameImages.OBJ_SUPERMARKET_CHECKOUT), new Point(44, 20));
            map.PlaceMapObjectAt(MakeObjCheckout(GameImages.OBJ_SUPERMARKET_CHECKOUT), new Point(46, 20));
            map.PlaceMapObjectAt(MakeObjCheckout(GameImages.OBJ_SUPERMARKET_CHECKOUT), new Point(48, 20));
#endregion

#region Central section
            //bathrooms
            map.PlaceMapObjectAt(MakeObjWoodenDoor(), new Point(3, 23));
            map.PlaceMapObjectAt(MakeObjToilet(GameImages.OBJ_TOILET), new Point(1, 24));
            map.PlaceMapObjectAt(MakeObjBathroomBasin(GameImages.OBJ_BATHROOM_BASIN), new Point(2, 24));
            map.SetTileModelAt(3, 24, m_Game.GameTiles.WALL_MALL);
            TileRectangle(map, m_Game.GameTiles.WALL_MALL, new Rectangle(1,25,3,2)); //wall between bathrooms
            map.SetTileModelAt(3, 27, m_Game.GameTiles.WALL_MALL);
            map.PlaceMapObjectAt(MakeObjToilet(GameImages.OBJ_TOILET), new Point(1, 27));
            map.PlaceMapObjectAt(MakeObjBathroomBasin(GameImages.OBJ_BATHROOM_BASIN), new Point(2, 27));
            map.PlaceMapObjectAt(MakeObjWoodenDoor(), new Point(3, 28));

            //plants and seating
            map.PlaceMapObjectAt(MakeObjFireBarrel(GameImages.OBJ_EMPTY_BIN), new Point(6, 28));
            map.PlaceMapObjectAt(MakeObjPottedPlant(GameImages.OBJ_POTTED_PLANT), new Point(7, 28));
            map.PlaceMapObjectAt(MakeObjCouch(GameImages.OBJ_COUCH), new Point(8, 28));
            map.PlaceMapObjectAt(MakeObjCouch(GameImages.OBJ_COUCH), new Point(9, 28));
            map.PlaceMapObjectAt(MakeObjCouch(GameImages.OBJ_COUCH), new Point(10, 28));
            map.PlaceMapObjectAt(MakeObjCouch(GameImages.OBJ_COUCH), new Point(11, 28));
            map.PlaceMapObjectAt(MakeObjCouch(GameImages.OBJ_COUCH), new Point(12, 28));
            map.PlaceMapObjectAt(MakeObjPottedPlant(GameImages.OBJ_POTTED_PLANT), new Point(13, 28));
            map.PlaceMapObjectAt(MakeObjFireBarrel(GameImages.OBJ_EMPTY_BIN), new Point(14, 28));
            map.PlaceMapObjectAt(MakeObjFireBarrel(GameImages.OBJ_EMPTY_BIN), new Point(23, 28));
            map.PlaceMapObjectAt(MakeObjPottedPlant(GameImages.OBJ_POTTED_PLANT), new Point(24, 28));
            map.PlaceMapObjectAt(MakeObjCouch(GameImages.OBJ_COUCH), new Point(25, 28));
            map.PlaceMapObjectAt(MakeObjCouch(GameImages.OBJ_COUCH), new Point(26, 28));
            map.PlaceMapObjectAt(MakeObjCouch(GameImages.OBJ_COUCH), new Point(27, 28));
            map.PlaceMapObjectAt(MakeObjCouch(GameImages.OBJ_COUCH), new Point(28, 28));
            map.PlaceMapObjectAt(MakeObjCouch(GameImages.OBJ_COUCH), new Point(29, 28));
            map.PlaceMapObjectAt(MakeObjPottedPlant(GameImages.OBJ_POTTED_PLANT), new Point(30, 28));
            map.PlaceMapObjectAt(MakeObjFireBarrel(GameImages.OBJ_EMPTY_BIN), new Point(31, 28));

            //cinema entry
            map.PlaceMapObjectAt(MakeObjFireBarrel(GameImages.OBJ_EMPTY_BIN), new Point(38, 23));
            map.PlaceMapObjectAt(MakeObjBench(GameImages.OBJ_BENCH), new Point(39, 23));
            map.PlaceMapObjectAt(MakeObjBench(GameImages.OBJ_BENCH), new Point(40, 23));
            map.PlaceMapObjectAt(MakeObjBench(GameImages.OBJ_BENCH), new Point(39, 28));
            map.PlaceMapObjectAt(MakeObjBench(GameImages.OBJ_BENCH), new Point(40, 28));
            map.PlaceMapObjectAt(MakeObjFireBarrel(GameImages.OBJ_EMPTY_BIN), new Point(38, 28));

            //cinema foyer
            TileRectangle(map, m_Game.GameTiles.WALL_MALL, new Rectangle(41, 23, 1, 2)); //north side of the entry
            map.GetTileAt(41, 24).AddDecoration(GameImages.DECO_CINEMA_SIGN);
            TileRectangle(map, m_Game.GameTiles.WALL_MALL, new Rectangle(41, 27, 1, 2)); //south side of the entry
            map.GetTileAt(41, 27).AddDecoration(GameImages.DECO_CINEMA_SIGN);
            Rectangle cinemaFoyer = new Rectangle(42, 23, 8, 6);
            TileFill(map, m_Game.GameTiles.FLOOR_RED_CARPET, cinemaFoyer);
            map.PlaceMapObjectAt(MakeObjReceptionDesk(GameImages.OBJ_BANK_TELLER), new Point(48, 23));
            map.PlaceMapObjectAt(MakeObjReceptionDesk(GameImages.OBJ_BANK_TELLER), new Point(48, 24));
            map.PlaceMapObjectAt(MakeObjReceptionDesk(GameImages.OBJ_BANK_TELLER), new Point(48, 25));
            map.PlaceMapObjectAt(MakeObjReceptionDesk(GameImages.OBJ_BANK_TELLER), new Point(48, 26));
            map.PlaceMapObjectAt(MakeObjReceptionDesk(GameImages.OBJ_BANK_TELLER), new Point(48, 27));
            map.PlaceMapObjectAt(MakeObjReceptionDesk(GameImages.OBJ_BANK_TELLER), new Point(48, 28));
            map.PlaceMapObjectAt(MakeObjCouch(GameImages.OBJ_COUCH), new Point(42, 23)); //north side of the entry
            map.PlaceMapObjectAt(MakeObjCouch(GameImages.OBJ_COUCH), new Point(43, 23));
            map.PlaceMapObjectAt(MakeObjCouch(GameImages.OBJ_COUCH), new Point(44, 23));
            map.PlaceMapObjectAt(MakeObjCouch(GameImages.OBJ_COUCH), new Point(45, 23));
            map.PlaceMapObjectAt(MakeObjDrawer(GameImages.OBJ_LECTERN), new Point(46, 29)); //ticket check
            map.PlaceMapObjectAt(MakeObjCouch(GameImages.OBJ_COUCH), new Point(42, 28)); //south side of the entry
            map.PlaceMapObjectAt(MakeObjCouch(GameImages.OBJ_COUCH), new Point(43, 28));
            map.PlaceMapObjectAt(MakeObjCouch(GameImages.OBJ_COUCH), new Point(44, 28));
            #endregion

            //cinemas
#region
            TileRectangle(map, m_Game.GameTiles.WALL_MALL, new Rectangle(1, 29, 50, 1)); //wall ecapsulating corridor
            //-walls
            TileRectangle(map, m_Game.GameTiles.FLOOR_RED_CARPET, new Rectangle(45, 29, 3, 1)); //entry to corridor
            TileFill(map, m_Game.GameTiles.FLOOR_RED_CARPET, new Rectangle(1, 30, 49, 20)); //corridor and cinemas
            TileRectangle(map, m_Game.GameTiles.WALL_RED_CURTAINS, new Rectangle(1, 30, 1, 21)); //left wall
            TileRectangle(map, m_Game.GameTiles.WALL_RED_CURTAINS, new Rectangle(1, 50, 50, 1)); //bottom wall
            TileRectangle(map, m_Game.GameTiles.WALL_RED_CURTAINS, new Rectangle(50, 30, 1, 21)); //right wall
            TileRectangle(map, m_Game.GameTiles.WALL_RED_CURTAINS, new Rectangle(1, 32, 50, 1)); //top wall
            TileRectangle(map, m_Game.GameTiles.WALL_RED_CURTAINS, new Rectangle(26, 32, 1, 19)); //cinema dividing wall
            //-bins in corridor
            map.PlaceMapObjectAt(MakeObjFireBarrel(GameImages.OBJ_EMPTY_BIN), new Point(2, 30));
            map.PlaceMapObjectAt(MakeObjFireBarrel(GameImages.OBJ_EMPTY_BIN), new Point(2, 31));
            map.PlaceMapObjectAt(MakeObjFireBarrel(GameImages.OBJ_EMPTY_BIN), new Point(49, 30));
            map.PlaceMapObjectAt(MakeObjFireBarrel(GameImages.OBJ_EMPTY_BIN), new Point(49, 31));
            //-doors and signs
            Dictionary<int, int> doorPoints = new Dictionary<int, int>() {
                {3,32},{4,32},{23,32},{24,32},{28,32},{29,32},{47,32},{48,32}
            };
            foreach (KeyValuePair<int, int> doorPoint in doorPoints)
            {
                map.SetTileModelAt(doorPoint.Key, doorPoint.Value, m_Game.GameTiles.FLOOR_RED_CARPET);
                map.PlaceMapObjectAt(MakeObjWoodenDoor(), new Point(doorPoint.Key, doorPoint.Value));
            }
            map.GetTileAt(5, 32).AddDecoration(GameImages.DECO_CINEMA2);
            map.GetTileAt(22, 32).AddDecoration(GameImages.DECO_CINEMA2);
            map.GetTileAt(30, 32).AddDecoration(GameImages.DECO_CINEMA1);
            map.GetTileAt(46, 32).AddDecoration(GameImages.DECO_CINEMA1);
            //-seats
            KeyValuePairWithDuplicates rowStarts = new KeyValuePairWithDuplicates() {
                {4,34},{4,36},{4,38},{4,40},{4,42},{4,44},{29,34},{29,36},{29,38},{29,40},{29,42},{29,44}
            };
            foreach (KeyValuePair<int, int> rowstartPoint in rowStarts)
            {
                int rowWidth = 20;
                if (rowstartPoint.Key == 29) //cinema1, slightly smaller
                    rowWidth = 19;

                MapObjectFill(map, new Rectangle(rowstartPoint.Key, rowstartPoint.Value, rowWidth, 1),
                (pt) =>
                {
                    if (m_DiceRoller.RollChance(20)) //@@MP (Release 7-6)
                        map.DropItemAt(MakeItemSnackBar(), pt);
                    return MakeObjSeat(GameImages.OBJ_CINEMA_SEAT);
                });
            }
            //-screens
            Rectangle cinema1Screen = new Rectangle(28, 49, 21, 1);
            MapObjectFill(map, cinema1Screen,
            (pt) =>
            {
                return MakeObjCinemaScreen(GameImages.OBJ_CINEMA_SCREEN);
            });
            Rectangle cinema2Screen = new Rectangle(3, 49, 22, 1);
            MapObjectFill(map, cinema2Screen,
            (pt) =>
            {
                return MakeObjCinemaScreen(GameImages.OBJ_CINEMA_SCREEN);
            });
#endregion

            // done.
            return map;
        }

        Map GenerateShoppingMall_Parking(int seed)
        {
            //////////////////
            // 1. Create map.
            // 2. Entry and power rooms.
            // 3. Parking.
            //////////////////

            // 1. Create map.
            Map map = new Map(seed, "Shopping Mall - Parking", 51, 51)
            {
                Lighting = Lighting.DARKNESS
            };
            DoForEachTile(map.Rect, (pt) => map.GetTileAt(pt).IsInside = true);
            TileFill(map, m_Game.GameTiles.FLOOR_ASPHALT); //for the car spaces area
            TileFill(map, m_Game.GameTiles.FLOOR_WALKWAY, new Rectangle(1,1,5,50)); //for the entry and power rooms area
            TileRectangle(map, m_Game.GameTiles.WALL_CONCRETE, map.Rect);

            // 2. Entry and power rooms.
#region
            TileRectangle(map, m_Game.GameTiles.WALL_CONCRETE, new Rectangle(4,1,1,49)); //wall separating stairs and power rooms from car spaces
            //-openings from entry to parking area
            map.SetTileModelAt(4, 15, m_Game.GameTiles.FLOOR_WALKWAY);
            map.SetTileModelAt(4, 16, m_Game.GameTiles.FLOOR_WALKWAY);
            map.SetTileModelAt(4, 25, m_Game.GameTiles.FLOOR_WALKWAY);
            map.SetTileModelAt(4, 26, m_Game.GameTiles.FLOOR_WALKWAY);
            map.SetTileModelAt(4, 35, m_Game.GameTiles.FLOOR_WALKWAY);
            map.SetTileModelAt(4, 36, m_Game.GameTiles.FLOOR_WALKWAY);
            //-bins, plants and benches
            map.PlaceMapObjectAt(MakeObjFireBarrel(GameImages.OBJ_EMPTY_BIN), new Point(1, 16));
            map.PlaceMapObjectAt(MakeObjPottedPlant(GameImages.OBJ_POTTED_PLANT), new Point(1, 17));
            map.PlaceMapObjectAt(MakeObjBench(GameImages.OBJ_BENCH), new Point(1, 18));
            map.PlaceMapObjectAt(MakeObjBench(GameImages.OBJ_BENCH), new Point(1, 19));
            map.PlaceMapObjectAt(MakeObjBench(GameImages.OBJ_BENCH), new Point(1, 20));
            map.PlaceMapObjectAt(MakeObjPottedPlant(GameImages.OBJ_POTTED_PLANT), new Point(1, 21));
            map.PlaceMapObjectAt(MakeObjFireBarrel(GameImages.OBJ_EMPTY_BIN), new Point(1, 22));
            map.PlaceMapObjectAt(MakeObjFireBarrel(GameImages.OBJ_EMPTY_BIN), new Point(1, 29));
            map.PlaceMapObjectAt(MakeObjPottedPlant(GameImages.OBJ_POTTED_PLANT), new Point(1, 30));
            map.PlaceMapObjectAt(MakeObjBench(GameImages.OBJ_BENCH), new Point(1, 31));
            map.PlaceMapObjectAt(MakeObjBench(GameImages.OBJ_BENCH), new Point(1, 32));
            map.PlaceMapObjectAt(MakeObjBench(GameImages.OBJ_BENCH), new Point(1, 33));
            map.PlaceMapObjectAt(MakeObjPottedPlant(GameImages.OBJ_POTTED_PLANT), new Point(1, 34));
            map.PlaceMapObjectAt(MakeObjFireBarrel(GameImages.OBJ_EMPTY_BIN), new Point(1, 35));

            //-power room north.
            //--door
            map.SetTileModelAt(1, 11, m_Game.GameTiles.WALL_CONCRETE);
            map.PlaceMapObjectAt(MakeObjIronDoor(DoorWindow.STATE_CLOSED), new Point(2, 11));
            map.SetTileModelAt(3, 11, m_Game.GameTiles.WALL_CONCRETE);
            //--zone
            Rectangle powerRoomNorth = new Rectangle(1,1,3,10);
            map.AddZone(MakeUniqueZone("power room", powerRoomNorth));
            //--power generators.
            DoForEachTile(powerRoomNorth,
                (pt) =>
                {
                    if (CountAdjWalls(map, pt) < 3)
                        return;

                    map.PlaceMapObjectAt(MakeObjPowerGenerator(GameImages.OBJ_POWERGEN_OFF, GameImages.OBJ_POWERGEN_ON), pt);
                });

            //-power room south.
            //--door
            map.SetTileModelAt(1, 39, m_Game.GameTiles.WALL_CONCRETE);
            map.PlaceMapObjectAt(MakeObjIronDoor(DoorWindow.STATE_CLOSED), new Point(2, 39));
            map.SetTileModelAt(3, 39, m_Game.GameTiles.WALL_CONCRETE);
            //--zone
            Rectangle powerRoomSouth = new Rectangle(1, 40, 3, 10);
            map.AddZone(MakeUniqueZone("power room", powerRoomSouth));
            //--power generators.
            DoForEachTile(powerRoomSouth,
                (pt) =>
                {
                    if (CountAdjWalls(map, pt) < 3)
                        return;

                    map.PlaceMapObjectAt(MakeObjPowerGenerator(GameImages.OBJ_POWERGEN_OFF, GameImages.OBJ_POWERGEN_ON), pt);
                });
#endregion

            // 3. Parking.
#region
            //-edges
            TileRectangle(map, m_Game.GameTiles.PARKING_ASPHALT_NS, new Rectangle(6, 1, 42, 1)); //top row
            TileRectangle(map, m_Game.GameTiles.PARKING_ASPHALT_EW, new Rectangle(49, 2, 1, 47)); //right side column
            TileRectangle(map, m_Game.GameTiles.PARKING_ASPHALT_NS, new Rectangle(6, 49, 42, 1)); //bottom row
            //-central columns
            KeyValuePairWithDuplicates northColumns = new KeyValuePairWithDuplicates() {
                {9,4},{14,4},{19,4},{24,4},{29,4},{34,4},{39,4},{44,4}, //--north of dividing lanes
                {9,27},{14,27},{19,27},{24,27},{29,27},{34,27},{39,27},{44,27}//--south of dividing lanes
            };
            foreach (KeyValuePair<int, int> columnStartingPt in northColumns)
            {
                for (int i = 0; i <= 20; ++i)
                {
                    map.SetTileModelAt(columnStartingPt.Key, columnStartingPt.Value + i, m_Game.GameTiles.PARKING_ASPHALT_EW); //parking spot
                    if (i % 5 == 0)
                        map.SetTileModelAt(columnStartingPt.Key + 1, columnStartingPt.Value + i, m_Game.GameTiles.WALL_PILLAR_CONCRETE); //load-bearing pillar
                    else
                        map.PlaceMapObjectAt(MakeObjIronRailing(GameImages.OBJ_RAILING), new Point(columnStartingPt.Key + 1, columnStartingPt.Value + i)); //railing
                    map.SetTileModelAt(columnStartingPt.Key + 2, columnStartingPt.Value + i, m_Game.GameTiles.PARKING_ASPHALT_EW); //parking spot
                }
            }

            //-cars
            MapObjectFill(map, new Rectangle(6,2, 44, 48),
                (pt) =>
                {
                    if (map.GetTileAt(pt).Model == m_Game.GameTiles.PARKING_ASPHALT_EW || map.GetTileAt(pt).Model == m_Game.GameTiles.PARKING_ASPHALT_NS)
                    {
                        if (m_DiceRoller.RollChance(20))
                            return MakeObjAbandonedCar(m_DiceRoller);
                    }
                    
                    return null;
                });
            
#endregion

            // done.
            return map;
        }

        void MakeMallShopDisplays(Map map, Block b, MallShopType shopType)
        {
            // Make sections alleys with displays.    
            int alleysStartX = b.BuildingRect.Left;
            int alleysStartY = b.BuildingRect.Top;
            int alleysEndX = b.BuildingRect.Right;
            int alleysEndY = b.BuildingRect.Bottom;
            bool horizontalAlleys = b.Rectangle.Width >= b.Rectangle.Height;
            int centralAlley;

            if (horizontalAlleys)
            {
                ++alleysStartX;
                --alleysEndX;
                centralAlley = b.InsideRect.Left + b.InsideRect.Width / 2;
            }
            else
            {
                ++alleysStartY;
                --alleysEndY;
                centralAlley = b.InsideRect.Top + b.InsideRect.Height / 2;
            }
            Rectangle alleysRect = Rectangle.FromLTRB(alleysStartX, alleysStartY, alleysEndX, alleysEndY);

            //make shelves/displays
            MapObjectFill(map, alleysRect,
                (pt) =>
                {
                    bool addShelf;

                    if (horizontalAlleys)
                        addShelf = ((pt.Y - alleysRect.Top) % 2 == 1) && pt.X != centralAlley;
                    else
                        addShelf = ((pt.X - alleysRect.Left) % 2 == 1) && pt.Y != centralAlley;

                    if (addShelf)
                    {
                        MapObject display = null;
                        switch (shopType) //different style of display depending on the store type
                        {
                            case MallShopType.BARBER: //@@MP (Release 7-6)
                                switch (m_DiceRoller.Roll(0, 3))
                                {
                                    case 0: display = MakeObjWigsDisplay(GameImages.OBJ_WIGS_DISPLAY1); break;
                                    case 1: display = MakeObjWigsDisplay(GameImages.OBJ_WIGS_DISPLAY2); break;
                                    case 2: display = MakeObjWigsDisplay(GameImages.OBJ_WIGS_DISPLAY3); break;
                                }
                                break;
                            case MallShopType.LIQUOR:
                            case MallShopType.SPORTING_GOODS:
                            case MallShopType.GROCERY:
                            case MallShopType.PHARMACY:
                                display = MakeObjShelf(GameImages.OBJ_SHOP_SHELF);
                                map.DropItemAt(MakeRandomMallShopItem(shopType), pt);
                                break;
                            case MallShopType.BOOKSTORE:
                                display = MakeObjBookshelves(GameImages.OBJ_BOOK_SHELVES);
                                if (RogueGame.Options.IsSanityEnabled) //@@MP - added check (Release 7-6)
                                    map.DropItemAt(MakeRandomMallShopItem(shopType), pt);
                                break;
                            case MallShopType.CLOTHING:
                                switch (m_DiceRoller.Roll(0, 3))
                                {
                                    case 0: display = MakeObjClothesDisplay(GameImages.OBJ_CLOTHES_WALL1); break;
                                    case 1: display = MakeObjClothesDisplay(GameImages.OBJ_CLOTHES_WALL2); break;
                                    case 2: display = MakeObjClothesDisplay(GameImages.OBJ_SHOES_WALL); break;
                                }
                                break;
                            case MallShopType.MOBILES:
                                display = MakeObjTable(GameImages.OBJ_MOBILES_TABLE);
                                map.DropItemAt(MakeItemCellPhone(), pt);
                                break;
                            case MallShopType.DEALERSHIP: display = null; break;
                            case MallShopType.ELECTRONICS:
                                switch (m_DiceRoller.Roll(0, 6))
                                {
                                    case 0: display = MakeObjFridge(GameImages.OBJ_FRIDGE); break;
                                    case 1: display = MakeObjTelevision(GameImages.OBJ_TELEVISION); break;
                                    case 2: display = MakeObjTable(GameImages.OBJ_LAPTOPS_TABLE); break;
                                    case 3: display = MakeObjHouseholdMachine(GameImages.OBJ_WASHING_MACHINE, "washing machine"); break;
                                    case 4: display = MakeObjHouseholdMachine(GameImages.OBJ_DRYER, "dryer"); break;
                                    case 5: display = MakeObjHouseholdMachine(GameImages.OBJ_DISHWASHER, "dishwasher"); break;
                                }
                                break;
                            default: throw new InvalidOperationException("unhandled mall shop type");
                        }

                        return display;
                    }
                    else
                        return null;
                });
        }
#endregion

#region Army Base
        //@@MP (Release 6-3)
        public Map GenerateUniqueMap_ArmyBase(Map surfaceMap, Zone officeZone, int mapSize, out Point baseEntryPos)
            //@@MP - added mapSize so we use the same district size as the player chooses (Release 7-4)
        {
            /////////////////////////
            // 1. Create basic secret map.
            // 2. Link to office.
            // 3. Create rooms.
            // 4. Furniture & Items.
            // 5. Posters & Blood.
            // 6. Populate.
            // 7. Add uniques.
            // 8. Music.  // alpha10
            /////////////////////////

            // 1. Create basic secret map.
#region
            // huge map.
            Map underground = new Map((surfaceMap.Seed << 3) ^ surfaceMap.Seed, "Army Base", mapSize, mapSize)
            {
                Lighting = Lighting.DARKNESS,
                IsSecret = true
            };
            // fill & enclose.
            TileFill(underground, m_Game.GameTiles.FLOOR_ARMY, (tile, model, x, y) => tile.IsInside = true);
            TileRectangle(underground, m_Game.GameTiles.WALL_ARMY_BASE, new Rectangle(0, 0, underground.Width, underground.Height));
#endregion

            // 2. Link to above ground office.
#region
            // find surface point in office:
            // - in a random office room.
            // - set exit somewhere walkable inside.
            // - iron door, barricade the door.
            //Zone roomZone = null;
            Point surfaceExit = new Point();
            while (true)    // loop until found.
            {
                /*   //@@MP (Release 7-6)
                // find a random room.
                do
                {
                    int x = m_DiceRoller.Roll(officeZone.Bounds.Left, officeZone.Bounds.Right);
                    int y = m_DiceRoller.Roll(officeZone.Bounds.Top, officeZone.Bounds.Bottom);
                    List<Zone> zonesHere = surfaceMap.GetZonesAt(x, y);
                    if (zonesHere == null || zonesHere.Count == 0)
                        continue;
                    foreach (Zone z in zonesHere)
                        if (z.Name.Contains("room"))
                        {
                            roomZone = z;
                            break;
                        }
                }
                while (roomZone == null);
                */

                // find somewhere walkable inside.
                bool foundSurfaceExit = false;
                int attempts = 0;
                do
                {
                    surfaceExit.X = m_DiceRoller.Roll(officeZone.Bounds.Left, officeZone.Bounds.Right);
                    surfaceExit.Y = m_DiceRoller.Roll(officeZone.Bounds.Top, officeZone.Bounds.Bottom);
                    foundSurfaceExit = surfaceMap.IsWalkable(surfaceExit.X, surfaceExit.Y);
                    ++attempts;
                }
                while (attempts < 100 && !foundSurfaceExit);

                // failed?
                if (foundSurfaceExit == false)
                    continue;

                // found everything, good!
                break;
            }

            // remember position // alpha10
            baseEntryPos = surfaceExit;

            /*    //@@MP - no need, front doors are locked (Release 7-6)
            // barricade the rooms door.
            DoForEachTile(officeZone.Bounds, //@@MP - unused parameter (Release 5-7)
                (pt) =>
                {
                    DoorWindow door = surfaceMap.GetMapObjectAt(pt) as DoorWindow;
                    if (door == null)
                        return;
                    surfaceMap.RemoveMapObjectAt(pt.X, pt.Y);
                    door = MakeObjIronDoor(DoorWindow.STATE_CLOSED);
                    door.BarricadePoints = Rules.BARRICADING_MAX;
                    surfaceMap.PlaceMapObjectAt(door, pt);
                });
                */

            // stairs.
            // underground : in the middle of the map.
            Point undergroundStairs = new Point(underground.Width / 2, underground.Height / 2);
            underground.SetExitAt(undergroundStairs, new Exit(surfaceMap, surfaceExit));
            underground.GetTileAt(undergroundStairs.X, undergroundStairs.Y).AddDecoration(GameImages.DECO_STAIRS_UP);
            surfaceMap.SetExitAt(surfaceExit, new Exit(underground, undergroundStairs));
            surfaceMap.GetTileAt(surfaceExit.X, surfaceExit.Y).AddDecoration(GameImages.DECO_STAIRS_DOWN);
            // floor logo.
            ForEachAdjacent(underground, undergroundStairs.X, undergroundStairs.Y, (pt) => underground.GetTileAt(pt).AddDecoration(GameImages.DECO_ARMY_FLOOR_LOGO));
#endregion

            // 3. Create floorplan & rooms.
#region
            // make 4 quarters, splitted by a crossed corridor.
            const int corridorHalfWidth = 1;
            Rectangle qTopLeft = Rectangle.FromLTRB(0, 0, underground.Width / 2 - corridorHalfWidth, underground.Height / 2 - corridorHalfWidth);
            Rectangle qTopRight = Rectangle.FromLTRB(underground.Width / 2 + 1 + corridorHalfWidth, 0, underground.Width, qTopLeft.Bottom);
            Rectangle qBotLeft = Rectangle.FromLTRB(0, underground.Height / 2 + 1 + corridorHalfWidth, qTopLeft.Right, underground.Height);
            Rectangle qBotRight = Rectangle.FromLTRB(qTopRight.Left, qBotLeft.Top, underground.Width, underground.Height);

            // split all the map in rooms.
            const int minRoomSize = 6;
            List<Rectangle> roomsList = new List<Rectangle>();
            MakeRoomsPlan(underground, ref roomsList, qBotLeft, minRoomSize, minRoomSize);
            MakeRoomsPlan(underground, ref roomsList, qBotRight, minRoomSize, minRoomSize);
            MakeRoomsPlan(underground, ref roomsList, qTopLeft, minRoomSize, minRoomSize);
            MakeRoomsPlan(underground, ref roomsList, qTopRight, minRoomSize, minRoomSize);

            // make the rooms walls.
            foreach (Rectangle roomRect in roomsList)
            {
                TileRectangle(underground, m_Game.GameTiles.WALL_ARMY_BASE, roomRect);
            }

            // add room doors.
            // quarters have door side preferences to lead toward the central corridors.
            foreach (Rectangle roomRect in roomsList)
            {
                Point westEastDoorPos = roomRect.Left < underground.Width / 2 ?
                    new Point(roomRect.Right - 1, roomRect.Top + roomRect.Height / 2) :
                    new Point(roomRect.Left, roomRect.Top + roomRect.Height / 2);
                if (underground.GetMapObjectAt(westEastDoorPos) == null)
                {
                    DoorWindow door = MakeObjIronDoor(DoorWindow.STATE_CLOSED);
                    PlaceDoorIfAccessibleAndNotAdjacent(underground, westEastDoorPos.X, westEastDoorPos.Y, m_Game.GameTiles.FLOOR_ARMY, 6, door);
                }

                Point northSouthDoorPos = roomRect.Top < underground.Height / 2 ?
                    new Point(roomRect.Left + roomRect.Width / 2, roomRect.Bottom - 1) :
                    new Point(roomRect.Left + roomRect.Width / 2, roomRect.Top);
                if (underground.GetMapObjectAt(northSouthDoorPos) == null)
                {
                    DoorWindow door = MakeObjIronDoor(DoorWindow.STATE_CLOSED);
                    PlaceDoorIfAccessibleAndNotAdjacent(underground, northSouthDoorPos.X, northSouthDoorPos.Y, m_Game.GameTiles.FLOOR_ARMY, 6, door);
                }
            }

            // add iron doors closing each corridor.
            for (int x = qTopLeft.Right; x < qBotRight.Left; x++)
            {
                PlaceDoor(underground, x, qTopLeft.Bottom - 1, m_Game.GameTiles.FLOOR_ARMY, MakeObjIronDoor(DoorWindow.STATE_CLOSED));
                PlaceDoor(underground, x, qBotLeft.Top, m_Game.GameTiles.FLOOR_ARMY, MakeObjIronDoor(DoorWindow.STATE_CLOSED));
            }
            for (int y = qTopLeft.Bottom; y < qBotLeft.Top; y++)
            {
                PlaceDoor(underground, qTopLeft.Right - 1, y, m_Game.GameTiles.FLOOR_ARMY, MakeObjIronDoor(DoorWindow.STATE_CLOSED));
                PlaceDoor(underground, qTopRight.Left, y, m_Game.GameTiles.FLOOR_ARMY, MakeObjIronDoor(DoorWindow.STATE_CLOSED));
            }
#endregion

            // 4. Rooms, furniture & items.
#region
            // furniture + items in rooms.
            // room roles with zones:
            // - corners room : Power Room.
            // - top left quarter : armory.
            // - top right quarter : storage.
            // - bottom left quarter : living.
            // - bottom right quarter : pharmacy.
            foreach (Rectangle roomRect in roomsList)
            {
                Rectangle insideRoomRect = new Rectangle(roomRect.Left + 1, roomRect.Top + 1, roomRect.Width - 2, roomRect.Height - 2);
                string roomName = "<noname>";

                // special room?
                // one power room in each corner.
                bool isPowerRoom = (roomRect.Left == 0 && roomRect.Top == 0) ||
                    (roomRect.Left == 0 && roomRect.Bottom == underground.Height) ||
                    (roomRect.Right == underground.Width && roomRect.Top == 0) ||
                    (roomRect.Right == underground.Width && roomRect.Bottom == underground.Height);
                if (isPowerRoom)
                {
                    roomName = "Power Room";
                    MakeArmyPowerRoom(underground, roomRect, insideRoomRect);
                }
                else
                {
                    // common room.
                    int roomRole = (roomRect.Left < underground.Width / 2 && roomRect.Top < underground.Height / 2) ? 0 :
                        (roomRect.Left >= underground.Width / 2 && roomRect.Top < underground.Height / 2) ? 1 :
                        (roomRect.Left < underground.Width / 2 && roomRect.Top >= underground.Height / 2) ? 2 :
                        3;
                    switch (roomRole)
                    {
                        case 0: // armory room.
                            {
                                roomName = "Armory";
                                MakeArmyArmoryRoom(underground, insideRoomRect);
                                break;
                            }
                        case 1: // command room
                            {
                                roomName = "Command";
                                MakeArmyCommandRoom(underground, insideRoomRect);
                                break;
                            }
                        case 2: // living room.
                            {
                                roomName = "Living";
                                MakeArmyRecRoom(underground, insideRoomRect);
                                break;
                            }
                        case 3: // pharmacy or storage room.
                            {
                                if (m_DiceRoller.RollChance(50))
                                {
                                    roomName = "Storage";
                                    MakeArmyStorageRoom(underground, insideRoomRect);
                                    break;
                                }
                                else
                                {
                                    roomName = "Pharmacy";
                                    MakeArmyPharmacyRoom(underground, insideRoomRect);
                                    break;
                                }
                            }
                        default:
                            throw new InvalidOperationException("unhandled role");
                    }
                }

                underground.AddZone(MakeUniqueZone(roomName, insideRoomRect));
            }
#endregion

            // 5. Posters & Blood.
#region
            // army posters & blood almost everywhere.
            for (int x = 0; x < underground.Width; x++)
                for (int y = 0; y < underground.Height; y++)
                {
                    // poster on wall?
                    if (m_DiceRoller.RollChance(25))
                    {
                        Tile tile = underground.GetTileAt(x, y);
                        if (tile.Model.IsWalkable)
                            continue;
                        tile.AddDecoration(ARMY_POSTERS[m_DiceRoller.Roll(0, ARMY_POSTERS.Length)]);
                    }

                    // large blood?
                    if (m_DiceRoller.RollChance(10)) //@@MP - was 20 (Release 3)
                    {
                        Tile tile = underground.GetTileAt(x, y);
                        if (tile.Model.IsWalkable)
                            tile.AddDecoration(GameImages.DECO_BLOODIED_FLOOR);
                        else
                            tile.AddDecoration(GameImages.DECO_BLOODIED_WALL);
                    }
                    else if (m_DiceRoller.RollChance(20)) // small blood? //@@MP (Release 3)
                    {
                        Tile tile = underground.GetTileAt(x, y);
                        if (tile.Model.IsWalkable)
                            tile.AddDecoration(GameImages.DECO_BLOODIED_FLOOR_SMALL);
                        else
                            tile.AddDecoration(GameImages.DECO_BLOODIED_WALL_SMALL);
                    }
                }
#endregion

            // 6. Populate.
#region
            // leveled up undeads!
            int nbZombies = underground.Width;  // 100 for 100.
            for (int i = 0; i < nbZombies; i++)
            {
                Actor undead = CreateNewUndead(0);
                for (; ; )
                {
                    GameActors.IDs upID = m_Game.NextUndeadEvolution((GameActors.IDs)undead.Model.ID);
                    if (upID == (GameActors.IDs)undead.Model.ID)
                        break;
                    undead.Model = m_Game.GameActors[upID];
                }
                ActorPlace(m_DiceRoller, underground.Width * underground.Height, underground, undead, (pt) => underground.GetExitAt(pt) == null); // don't block exits!
            }
#endregion

            // 7. Add uniques.
            // looks like RoguedJack had some plans for a boss or special items for the CHAR underground that the army base is copied from
#region
#endregion

            // 8. Music.   // alpha10
            underground.BgMusic = GameMusics.CHAR_UNDERGROUND_FACILITY;

            // done.
            return underground;
        }

        static string[] ARMY_POSTERS = { GameImages.DECO_ARMY_POSTER1, GameImages.DECO_ARMY_POSTER2, GameImages.DECO_ARMY_POSTER3 };

        void MakeArmyArmoryRoom(Map map, Rectangle roomRect)
        {
            //@@MP - only one per game (Release 7-6)
            bool minigunSpawned = false;
            bool grenadelauncherSpawned = false;

            // Shelves with weapons/ammo along walls.
            MapObjectFill(map, roomRect,
                (pt) =>
                {
                    if (CountAdjWalls(map, pt.X, pt.Y) < 2)
                        return null;
                    // don't block doors
                    if (IsADoorNSEW(map, pt.X, pt.Y)) //@@MP (Release 7-6)
                        return null;
                    // dont block exits!
                    if (map.GetExitAt(pt) != null)
                        return null;

                    // table + tracker/armor/weapon.
                    if (m_DiceRoller.RollChance(GameOptions.ResourcesAvailabilityToInt(RogueGame.Options.ResourcesAvailability))) //@@MP - Resources Availability option (Release 7-4)
                    {
                        Item it;
                        int randomItem = m_DiceRoller.Roll(0, 34);
                        switch (randomItem)
                        {
                            case 0: it = MakeItemArmyRifle(); break;
                            case 1:
                            case 2:
                            case 3:
                            case 4:
                            case 5:
                            case 6: it = MakeItemHeavyRifleAmmo(); break;
                            case 7: it = MakeItemArmyPistol(); break;
                            case 8:
                            case 9:
                            case 10: it = MakeItemHeavyPistolAmmo(); break;
                            case 11: it = MakeItemTacticalShotgun(); break;
                            case 12:
                            case 13:
                            case 14:
                            case 15:
                            case 16: it = MakeItemShotgunAmmo(); break;
                            case 17: it = MakeItemGrenade(); break;
                            case 18:
                            case 19: it = MakeItemArmyBodyArmor(); break;
                            case 20: it = MakeItemArmyPrecisionRifle(); break;
                            case 21:
                            case 22:
                            case 23: it = MakeItemPrecisionRifleAmmo(); break;//@@MP (Release 6-6)
                            case 24: it = MakeItemNightVisionGoggles(); break;
                            case 25: it = MakeItemC4Explosive(); break;
                            case 26: it = MakeItemFlamethrower(); break; //@@MP (Release 7-1)
                            case 27: 
                            case 28: 
                            case 29: it = MakeItemMinigunAmmo(); break; //@@MP (Release 7-6)
                            case 30: 
                            case 31: 
                                if (!minigunSpawned)  //@@MP (Release 7-6)
                                {
                                    it = MakeItemMinigun();
                                    minigunSpawned = true;
                                }
                                else
                                    it = MakeItemMinigunAmmo();
                                break;
                            case 32:
                            case 33:
                                if (!grenadelauncherSpawned)  //@@MP (Release 7-6)
                                {
                                    it = MakeItemGrenadeLauncher();
                                    grenadelauncherSpawned = true;
                                }
                                else
                                    it = MakeItemGrenadeLauncherAmmo();
                                break;
                            default:
                                throw new InvalidOperationException("unhandled roll");
                        }

                        map.DropItemAt(it, pt);
                    }

                    MapObject shelf = MakeObjShelf(GameImages.OBJ_SHOP_SHELF);
                    return shelf;
                });
        }

        void MakeArmyStorageRoom(Map map, Rectangle roomRect)
        {
            // Replace floor with concrete.
            TileFill(map, m_Game.GameTiles.FLOOR_CONCRETE, roomRect);

            // Objects.
            // Barrels & Junk in the middle of the room.
            MapObjectFill(map, roomRect,
                (pt) =>
                {
                    if (CountAdjWalls(map, pt.X, pt.Y) > 0)
                        return null;
                    // dont block exits!
                    if (map.GetExitAt(pt) != null)
                        return null;

                    // barrels/junk?
                    if (m_DiceRoller.RollChance(50))
                    {
                        //@@MP - added items that were in the armory before (Release 7-6)
                        if (m_DiceRoller.RollChance(5))
                            map.DropItemAt(MakeItemFlaresKit(), pt);

                        if (m_DiceRoller.RollChance(5))
                            map.DropItemAt(MakeItemSmokeGrenade(), pt);

                        if (m_DiceRoller.RollChance(5))
                            map.DropItemAt(MakeItemFlashbang(), pt);

                        return m_DiceRoller.RollChance(50) ? MakeObjJunk(GameImages.OBJ_JUNK) : MakeObjBarrels(GameImages.OBJ_BARRELS);
                    }
                    else
                        return null;
                });

            // Items.
            for (int x = roomRect.Left; x < roomRect.Right; x++)
                for (int y = roomRect.Top; y < roomRect.Bottom; y++)
                {
                    if (CountAdjWalls(map, x, y) > 0)
                        continue;
                    if (map.GetMapObjectAt(x, y) != null)
                        continue;

                    if (m_DiceRoller.RollChance(GameOptions.ResourcesAvailabilityToInt(RogueGame.Options.ResourcesAvailability))) //@@MP - Resources Availability option (Release 7-4)
                        map.DropItemAt(MakeItemArmyRation(), x, y);
                }
        }

        void MakeArmyCommandRoom(Map map, Rectangle roomRect)
        {
            // Replace floor with tiles with painted logo.
            TileFill(map, m_Game.GameTiles.FLOOR_ARMY, roomRect);//, (tile, model, x, y) => tile.AddDecoration(GameImages.DECO_ARMY_FLOOR_LOGO));

            // Objects.
            // radios along walls.
            MapObjectFill(map, roomRect,
                (pt) =>
                {
                    if (CountAdjWalls(map, pt.X, pt.Y) < 3)
                        return null;
                    // dont block exits!
                    if (map.GetExitAt(pt) != null)
                        return null;

                    // computer/radio?
                    if (m_DiceRoller.RollChance(66))
                    {
                        if (m_DiceRoller.RollChance(25))
                            return MakeObjWorkstation(GameImages.OBJ_ARMY_COMPUTER_STATION);
                        else
                        {
                            if (m_DiceRoller.RollChance(10))
                                map.DropItemAt(MakeItemBlackOpsGPS(), pt); //@@MP - moved from the armory (Release 7-6)
                            
                            return MakeObjArmyRadioCupboard(GameImages.OBJ_ARMY_RADIO_CUPBOARD);
                        }
                    }
                    else
                        return null;
                });
            // desktops and tables in the middle of the room
            MapObjectFill(map, roomRect,
                (pt) =>
                {
                    if (CountAdjWalls(map, pt.X, pt.Y) > 0)
                        return null;
                    // dont block exits!
                    if (map.GetExitAt(pt) != null)
                        return null;

                    // tables/chairs.
                    if (m_DiceRoller.RollChance(25))
                    {
                        if (m_DiceRoller.RollChance(75))
                            return MakeObjWorkstation(GameImages.OBJ_ARMY_COMPUTER_STATION);
                        else
                            return MakeObjTable(GameImages.OBJ_ARMY_TABLE);
                    }
                    return null;
                });
        }

        void MakeArmyRecRoom(Map map, Rectangle roomRect)
        {
            // Replace floor with wood with painted logo.
            TileFill(map, m_Game.GameTiles.FLOOR_ARMY, roomRect);//, (tile, model, x, y) => tile.AddDecoration(GameImages.DECO_CHAR_FLOOR_LOGO));

            // Objects.
            // Beds/Footlockers along walls.
            MapObjectFill(map, roomRect,
                (pt) =>
                {
                    if (CountAdjWalls(map, pt.X, pt.Y) < 3)
                        return null;
                    // dont block exits!
                    if (map.GetExitAt(pt) != null)
                        return null;

                    // bed/fridge?
                    if (m_DiceRoller.RollChance(50))
                    {
                        if (m_DiceRoller.RollChance(50))
                            return MakeObjBed(GameImages.OBJ_ARMY_BUNK_BED);
                        else
                            return MakeObjArmyFootlocker(GameImages.OBJ_ARMY_FOOTLOCKER);
                    }
                    else
                        return null;
                });
            // Tables(with canned food) & Chairs in the middle.
            MapObjectFill(map, roomRect,
                (pt) =>
                {
                    if (CountAdjWalls(map, pt.X, pt.Y) > 0)
                        return null;
                    // dont block exits!
                    if (map.GetExitAt(pt) != null)
                        return null;

                    // tables/chairs.
                    if (m_DiceRoller.RollChance(30))
                    {
                        if (m_DiceRoller.RollChance(30))
                        {
                            if (m_DiceRoller.RollChance(GameOptions.ResourcesAvailabilityToInt(RogueGame.Options.ResourcesAvailability))) //@@MP - Resources Availability option (Release 7-4)
                                map.DropItemAt(MakeItemCannedFood(), pt);
                            return MakeObjTable(GameImages.OBJ_ARMY_TABLE);
                        }
                        else
                            return MakeObjChair(GameImages.OBJ_HOSPITAL_CHAIR);
                    }
                    else
                        return null;
                });
        }

        void MakeArmyPharmacyRoom(Map map, Rectangle roomRect)
        {
            // Shelves with medicine along walls.
            MapObjectFill(map, roomRect,
                (pt) =>
                {
                    if (CountAdjWalls(map, pt.X, pt.Y) < 2)
                        return null;
                    // don't block doors
                    if (IsADoorNSEW(map, pt.X, pt.Y)) //@@MP (Release 7-6)
                        return null;
                    // dont block exits!
                    if (map.GetExitAt(pt) != null)
                        return null;

                    // table + meds.
                    if (m_DiceRoller.RollChance(GameOptions.ResourcesAvailabilityToInt(RogueGame.Options.ResourcesAvailability))) //@@MP - Resources Availability option (Release 7-4)
                    {
                        Item it = MakeHospitalItem();
                        map.DropItemAt(it, pt);
                    }

                    MapObject shelf = MakeObjShelf(GameImages.OBJ_SHOP_SHELF);
                    return shelf;
                });
        }

        void MakeArmyPowerRoom(Map map, Rectangle wallsRect, Rectangle roomRect)
        {
            // Replace floor with concrete.
            TileFill(map, m_Game.GameTiles.FLOOR_CONCRETE, roomRect);

            // add deco power sign next to doors.
            DoForEachTile(wallsRect, //@@MP - unused parameter (Release 5-7)
                (pt) =>
                {
                    if (!(map.GetMapObjectAt(pt) is DoorWindow))
                        return;
                    DoForEachAdjacentInMap(map, pt, (
                        ptAdj) =>
                    {
                        Tile tile = map.GetTileAt(ptAdj);
                        if (tile.Model.IsWalkable)
                            return;
                        tile.RemoveAllDecorations();
                        tile.AddDecoration(GameImages.DECO_POWER_SIGN_BIG);
                    });
                });

            // add power generators along walls.
            DoForEachTile(roomRect, //@@MP - unused parameter (Release 5-7)
                (pt) =>
                {
                    if (!map.GetTileAt(pt).Model.IsWalkable)
                        return;
                    if (map.GetExitAt(pt) != null)
                        return;
                    if (CountAdjWalls(map, pt.X, pt.Y) < 3)
                        return;

                    PowerGenerator powGen = MakeObjPowerGenerator(GameImages.OBJ_POWERGEN_OFF, GameImages.OBJ_POWERGEN_ON);
                    map.PlaceMapObjectAt(powGen, pt);
                });
        }
#endregion
#endregion

#region Actors

        public void GiveRandomItemToActor(DiceRoller roller, Actor actor, int spawnTime)
        {
            Item it = null;

            // rare item chance after Day X
            int day = new WorldTime(spawnTime).Day;
            if (day > Rules.GIVE_RARE_ITEM_DAY && roller.RollChance(Rules.GIVE_RARE_ITEM_CHANCE))
            {
                int roll = roller.Roll(0, 6);
                switch (roll)
                {
                    case 0: it = MakeItemGrenade(); break;
                    case 1: it = MakeItemArmyBodyArmor(); break;
                    case 2: it = MakeItemHeavyPistolAmmo(); break;
                    case 3: it = MakeItemHeavyRifleAmmo(); break;
                    case 4: it = Rules.HasAntiviralPills(m_Game.Session.GameMode) ? MakeItemPillsAntiviral() : MakeItemLargeMedikit(); //@MP - handles new antiviral pills option (Release 5-2)
                        break;
                    case 5: it = MakeItemShovel(); break;
                    default: it = null; break;
                }
            }
            else
            {
                // standard item.
                int roll = roller.Roll(0, 9);
                switch (roll)
                {
                    case 0: it = MakeRandomShopItem(ShopType.CONSTRUCTION); break;
                    case 1: it = MakeRandomShopItem(ShopType.GENERAL_STORE); break;
                    case 2: it = MakeRandomShopItem(ShopType.GROCERY); break;
                    case 3: it = MakeRandomShopItem(ShopType.GUNSHOP); break;
                    case 4: it = MakeRandomShopItem(ShopType.PHARMACY); break;
                    case 5: it = MakeRandomShopItem(ShopType.SPORTSWEAR); break;
                    case 6: it = MakeRandomShopItem(ShopType.HUNTING); break;
                    case 7: it = MakeRandomBedroomItem(); break;
                    case 8: it = MakeRandomKitchenItem(); break;
                    default: it = null; break;
                }
            }

            if (it != null)
                actor.Inventory.AddAll(it);
        }

        public Actor CreateNewRefugee(int spawnTime, int itemsToCarry)
        {
            Actor newRefugee;

            // civilian, policeman?
            if (m_DiceRoller.RollChance(Params.PolicemanChance))
            {
                newRefugee = CreateNewPoliceman(spawnTime);
                /*   //@@MP - items are already set (Release 7-3)
                // add random items.
                for(int i = 0; i < itemsToCarry && newRefugee.Inventory.CountItems < newRefugee.Inventory.MaxCapacity; i++)
                    GiveRandomItemToActor(m_DiceRoller, newRefugee, spawnTime);
                */
            }
            else
            {
                newRefugee = CreateNewCivilian(spawnTime, itemsToCarry, 1);
            }

            // give skills : 1 per day + 1 for starting.
            int nbSkills = 1 + new WorldTime(spawnTime).Day;
            base.GiveRandomSkillsToActor(m_DiceRoller, newRefugee, nbSkills);

            // done.
            return newRefugee;
        }

        public Actor CreateNewSurvivor(int spawnTime)
        {
            // decide model.
            bool isMale = m_Rules.Roll(0, 2) == 0;
            ActorModel model = isMale ? m_Game.GameActors.MaleCivilian : m_Game.GameActors.FemaleCivilian;

            // create.
            Actor survivor = model.CreateNumberedName(m_Game.GameFactions.TheSurvivors, spawnTime);

            // setup.
            base.GiveNameToActor(m_DiceRoller, survivor);
            DressCivilian(m_DiceRoller, survivor);
            survivor.Doll.AddDecoration(DollPart.HEAD, isMale ? GameImages.SURVIVOR_MALE_BANDANA : GameImages.SURVIVOR_FEMALE_BANDANA);
            base.GiveStartingSkillToActor(survivor, Skills.IDs.HAULER); //@@MP (Release 7-6)

            // give items, good survival gear (7 items).
#region
            // 1,2   1 can of food, 1 amr.
            survivor.Inventory.AddAll(MakeItemCannedFood());
            survivor.Inventory.AddAll(MakeItemArmyRation());
            // 3,4.  1 ranged weapon with 1 ammo box or grenade.
            if (m_DiceRoller.RollChance(50))
            {
                survivor.Inventory.AddAll(MakeItemPrecisionRifle()); //@@MP - was army rifle (Release 7-6)
                if (m_DiceRoller.RollChance(75)) //@@MP - was 50 (Release 7-6)
                    survivor.Inventory.AddAll(MakeItemPrecisionRifleAmmo());
                else
                    survivor.Inventory.AddAll(MakeItemGrenade());
            }
            else
            {
                survivor.Inventory.AddAll(MakeItemShotgun());
                if (m_DiceRoller.RollChance(75)) //@@MP - was 50 (Release 7-6)
                    survivor.Inventory.AddAll(MakeItemShotgunAmmo());
                else
                    survivor.Inventory.AddAll(MakeItemGrenade());
            }
            // 5    1 healing item.
            survivor.Inventory.AddAll(MakeItemLargeMedikit());

            // 6    1 pill item.
            switch (m_DiceRoller.Roll(0, 3))
            {
                case 0: survivor.Inventory.AddAll(MakeItemPillsSLP()); break;
                case 1: survivor.Inventory.AddAll(MakeItemPillsSTA()); break;
                case 2:
                    if (RogueGame.Options.IsSanityEnabled)
                        survivor.Inventory.AddAll(MakeItemPillsSAN());
                    else
                        survivor.Inventory.AddAll(MakeItemLargeMedikit());
                    break;
            }
            // 7    1 armor.
            survivor.Inventory.AddAll(MakeItemHunterVest()); //@@MP - was army armor (Release 7-6)

            // 8    1 flashlight.
            survivor.Inventory.AddAll(MakeItemBigFlashlight()); //@@MP (Release 7-6)
#endregion

            // give skills : 1 per day + 5 as bonus.
            int nbSkills = 3 + new WorldTime(spawnTime).Day;
            base.GiveRandomSkillsToActor(m_DiceRoller, survivor, nbSkills);

            // AI.
            //survivor.Controller = new CivilianAI(); // alpha10.1 defined by model like other actors

            // slightly randomize Food and Sleep - 0..25%.
            int foodDeviation = (int)(0.25f * survivor.FoodPoints);
            survivor.FoodPoints = survivor.FoodPoints - m_Rules.Roll(0, foodDeviation);
            int sleepDeviation = (int)(0.25f * survivor.SleepPoints);
            survivor.SleepPoints = survivor.SleepPoints - m_Rules.Roll(0, sleepDeviation);

            // done.
            return survivor;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public Actor CreateNewNakedHuman(int spawnTime) //@@MP - unused parameter (Release 5-7)
        {
            // decide model.
            ActorModel model = m_Rules.Roll(0, 2) == 0 ? m_Game.GameActors.MaleCivilian : m_Game.GameActors.FemaleCivilian;

            // create.
            Actor civilian = model.CreateNumberedName(m_Game.GameFactions.TheCivilians, spawnTime);

            // done.
            return civilian;
        }

        public Actor CreateNewCivilian(int spawnTime, int itemsToCarry, int skills)
        {
            // decide model.
            ActorModel model = m_Rules.Roll(0, 2) == 0 ? m_Game.GameActors.MaleCivilian : m_Game.GameActors.FemaleCivilian;

            // create.
            Actor civilian = model.CreateNumberedName(m_Game.GameFactions.TheCivilians, spawnTime);

            // setup.
            DressCivilian(m_DiceRoller, civilian);
            base.GiveNameToActor(m_DiceRoller, civilian);
            for (int i = 0; i < itemsToCarry; i++)
                GiveRandomItemToActor(m_DiceRoller, civilian, spawnTime);
            civilian.Inventory.AddAll(MakeItemRandomFlashlight()); //@@MP (Release 7-6)
            base.GiveRandomSkillsToActor(m_DiceRoller, civilian, skills);
            //civilian.Controller = new CivilianAI();  // alpha10.1 defined by model like other actors

            // slightly randomize Food and Sleep - 0..25%.
            int foodDeviation = (int)(0.25f * civilian.FoodPoints);
            civilian.FoodPoints = civilian.FoodPoints - m_Rules.Roll(0, foodDeviation);
            int sleepDeviation = (int)(0.25f * civilian.SleepPoints);
            civilian.SleepPoints = civilian.SleepPoints - m_Rules.Roll(0, sleepDeviation);

            // done.
            return civilian;
        }

        public Actor CreateNewPoliceman(int spawnTime)
        {
            // model.
            ActorModel model = m_Game.GameActors.Policeman;

            // create.
            Actor newCop = model.CreateNumberedName(m_Game.GameFactions.ThePolice, spawnTime);

            // setup.
            DressPolice(m_DiceRoller, newCop);
            base.GiveNameToActor(m_DiceRoller, newCop);
            newCop.Name = "Cop " + newCop.Name;
            base.GiveRandomSkillsToActor(m_DiceRoller, newCop, 1);
            base.GiveStartingSkillToActor(newCop, Skills.IDs.FIREARMS);
            base.GiveStartingSkillToActor(newCop, Skills.IDs.LEADERSHIP);
            base.GiveStartingSkillToActor(newCop, Skills.IDs.HAULER); //@@MP (Release 7-2)
            base.GiveStartingSkillToActor(newCop, Skills.IDs.HAULER); //@@MP (Release 7-2)
            //newCop.Controller = new CivilianAI(); // alpha10.1 defined by model like other actors

            // give items.
            if (m_DiceRoller.RollChance(50))
            {
                // pistol
                newCop.Inventory.AddAll(MakeItemRevolver()); //@@MP - was pistol (Release 7-6)
                newCop.Inventory.AddAll(MakeItemLightPistolAmmo());
                newCop.Inventory.AddAll(MakeItemLightPistolAmmo()); //@@MP (Release 7-2)
            }
            else
            {
                // shotgun
                newCop.Inventory.AddAll(MakeItemShotgun());
                newCop.Inventory.AddAll(MakeItemShotgunAmmo());
                newCop.Inventory.AddAll(MakeItemShotgunAmmo()); //@@MP (Release 7-2)
            }
            newCop.Inventory.AddAll(MakeItemTruncheon());
            newCop.Inventory.AddAll(MakeItemFlashlight());
            newCop.Inventory.AddAll(MakeItemPoliceRadio());
            if (m_DiceRoller.RollChance(50))
            {
                if (m_DiceRoller.RollChance(80))
                    newCop.Inventory.AddAll(MakeItemPoliceJacket());
                else
                    newCop.Inventory.AddAll(MakeItemPoliceRiotArmor());
            }
            newCop.Inventory.AddAll(MakeItemStunGun()); //@@MP (Release 7-2)

            // done.
            return newCop;
        }

        public Actor CreateNewPrisoner(int spawnTime, int skills) //@@MP - added (Release 7-6)
        {
            // decide model.
            ActorModel model = m_Rules.Roll(0, 2) == 0 ? m_Game.GameActors.MaleCivilian : m_Game.GameActors.FemaleCivilian;

            // create.
            Actor prisoner = model.CreateNumberedName(m_Game.GameFactions.TheCivilians, spawnTime);

            // setup.
            DressPrisoner(m_DiceRoller, prisoner);
            base.GiveNameToActor(m_DiceRoller, prisoner);
            base.GiveRandomSkillsToActor(m_DiceRoller, prisoner, skills);
            //civilian.Controller = new CivilianAI();  // alpha10.1 defined by model like other actors

            // make sure he is stripped of all default items!
            while (!prisoner.Inventory.IsEmpty)
                prisoner.Inventory.RemoveAllQuantity(prisoner.Inventory[0]);

            // give him some food.
            prisoner.Inventory.AddAll(MakeItemGroceries());
            prisoner.Inventory.AddAll(MakeItemCannedFood()); //@@MP (Release 7-6)
            prisoner.Inventory.AddAll(MakeItemCannedFood()); //@@MP (Release 7-6)
            prisoner.Inventory.AddAll(MakeItemCannedFood()); //@@MP (Release 7-6)
            prisoner.Inventory.AddAll(MakeItemCannedFood()); //@@MP (Release 7-6)
            prisoner.Inventory.AddAll(MakeItemCannedFood()); //@@MP (Release 7-6)
            prisoner.Inventory.AddAll(MakeItemCannedFood()); //@@MP (Release 7-6)

            // slightly randomize Food and Sleep - 0..25%.
            int foodDeviation = (int)(0.25f * prisoner.FoodPoints);
            prisoner.FoodPoints = prisoner.FoodPoints - m_Rules.Roll(0, foodDeviation);
            int sleepDeviation = (int)(0.25f * prisoner.SleepPoints);
            prisoner.SleepPoints = prisoner.SleepPoints - m_Rules.Roll(0, sleepDeviation);

            // done.
            return prisoner;
        }

        public Actor CreateNewUndead(int spawnTime)
        {
            Actor newUndead;

            if (Rules.HasAllZombies(m_Game.Session.GameMode))
            {
                // decide model.
                ActorModel undeadModel;
                int chance = m_Rules.Roll(0, 100);
                undeadModel = (chance < RogueGame.Options.SpawnSkeletonChance ? m_Game.GameActors.Skeleton :
                    chance < RogueGame.Options.SpawnSkeletonChance + RogueGame.Options.SpawnZombieChance ? m_Game.GameActors.Zombie :
                    chance < RogueGame.Options.SpawnSkeletonChance + RogueGame.Options.SpawnZombieChance + RogueGame.Options.SpawnZombieMasterChance ? m_Game.GameActors.ZombieMaster :
                     m_Game.GameActors.Skeleton);

                // create.
                newUndead = undeadModel.CreateNumberedName(m_Game.GameFactions.TheUndeads, spawnTime);
            }
            else
            {
                // zombified.
                newUndead = MakeZombified(null, CreateNewCivilian(spawnTime, 0, 0), spawnTime);
                // skills?
                WorldTime time = new WorldTime(spawnTime);
                int nbSkills = time.Day / 2;
                if (nbSkills > 0)
                {
                    for (int i = 0; i < nbSkills; i++)
                    {
                        Skills.IDs? zombifiedSkill = m_Game.ZombifySkill((Skills.IDs)m_Rules.Roll(0, (int)Skills.IDs._COUNT));
                        if (zombifiedSkill.HasValue)
                            m_Game.SkillUpgrade(newUndead, zombifiedSkill.Value);
                    }
                    RecomputeActorStartingStats(newUndead);
                }
            }

            // done.
            return newUndead;
        }

        public Actor MakeZombified(Actor zombifier, Actor deadVictim, int turn)
        {
            // create actor.
            string zombiefiedName = String.Format("{0}'s zombie", deadVictim.UnmodifiedName);
            ActorModel zombiefiedModel = deadVictim.Doll.Body.IsMale ? m_Game.GameActors.MaleZombified : m_Game.GameActors.FemaleZombified;
            Faction zombieFaction = (zombifier == null ? m_Game.GameFactions.TheUndeads : zombifier.Faction);
            Actor newZombie = zombiefiedModel.CreateNamed(zombieFaction, zombiefiedName, deadVictim.IsPluralName, turn);

            // dress as victim.
            for (DollPart p = DollPart._FIRST; p < DollPart._COUNT; p++)
            {
                List<string> partDecos = deadVictim.Doll.GetDecorations(p);
                if (partDecos != null)
                {
                    foreach (string deco in partDecos)
                        newZombie.Doll.AddDecoration(p, deco);
                }
            }

            // add blood.
            newZombie.Doll.AddDecoration(DollPart.TORSO, GameImages.BLOODIED);

            return newZombie;
        }

        public Actor CreateNewSewersUndead(int spawnTime)
        {
            if (!Rules.HasAllZombies(m_Game.Session.GameMode))
                return CreateNewUndead(spawnTime);

            // decide model. 
            ActorModel undeadModel = m_DiceRoller.RollChance(80) ? m_Game.GameActors.RatZombie : m_Game.GameActors.Zombie;

            // create.
            Actor newUndead = undeadModel.CreateNumberedName(m_Game.GameFactions.TheUndeads, spawnTime);

            // done.
            return newUndead;
        }

        public Actor CreateNewBasementRatZombie(int spawnTime)
        {
            if (!Rules.HasAllZombies(m_Game.Session.GameMode))
                return CreateNewUndead(spawnTime);

            return m_Game.GameActors.RatZombie.CreateNumberedName(m_Game.GameFactions.TheUndeads, spawnTime);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public Actor CreateNewSubwayUndead(int spawnTime)
        {
            if (!Rules.HasAllZombies(m_Game.Session.GameMode))
                return CreateNewUndead(spawnTime);

            // standard zombies.
            ActorModel undeadModel = m_Game.GameActors.Zombie;

            // create.
            Actor newUndead = undeadModel.CreateNumberedName(m_Game.GameFactions.TheUndeads, spawnTime);

            // done.
            return newUndead;
        }

        public Actor CreateNewCHARGuard(int spawnTime)
        {
            // model.
            ActorModel model = m_Game.GameActors.CHARGuard;

            // create.
            Actor newGuard = model.CreateNumberedName(m_Game.GameFactions.TheCHARCorporation, spawnTime);

            // setup.
            DressCHARGuard(m_DiceRoller, newGuard);
            base.GiveNameToActor(m_DiceRoller, newGuard);
            newGuard.Name = "Gd. " + newGuard.Name;

            // starting skills.        //@@MP (Release 8-1)
            base.GiveStartingSkillToActor(newGuard, Skills.IDs.HAULER); 
            base.GiveStartingSkillToActor(newGuard, Skills.IDs.HAULER);
            base.GiveStartingSkillToActor(newGuard, Skills.IDs.HAULER);
            base.GiveStartingSkillToActor(newGuard, Skills.IDs.AWAKE);
            base.GiveStartingSkillToActor(newGuard, Skills.IDs.AWAKE);
            base.GiveStartingSkillToActor(newGuard, Skills.IDs.STRONG_PSYCHE);
            base.GiveStartingSkillToActor(newGuard, Skills.IDs.STRONG_PSYCHE);

            // give items.
            //@@MP - removed guns to make this function more generic. guns & ammo are now added at the calling parent (Release 8-1)
            newGuard.Inventory.AddAll(MakeItemArmyRation()); //@@MP (Release 8-1)
            newGuard.Inventory.AddAll(MakeItemArmyRation()); //@@MP (Release 8-1)
            newGuard.Inventory.AddAll(MakeItemArmyRation()); //@@MP (Release 8-1)
            newGuard.Inventory.AddAll(MakeItemCHARLightBodyArmor());
            newGuard.Inventory.AddAll(MakeItemBigFlashlight()); //@@MP (Release 7-2)
            if (Rules.HasAntiviralPills(m_Game.Session.GameMode)) //@@MP (Release 7-2)
                newGuard.Inventory.AddAll(MakeItemPillsAntiviral());
            else
                newGuard.Inventory.AddAll(MakeItemLargeMedikit());

            // done.
            return newGuard;
        }

        public Actor CreateNewCHARScientist(int spawnTime) //@@MP - added (Release 8-1)
        {
            // model.
            ActorModel model = m_Game.GameActors.CHARScientist;

            // create.
            Actor newScientist = model.CreateNumberedName(m_Game.GameFactions.TheCHARCorporation, spawnTime);

            // setup.
            DressCHARScientist(m_DiceRoller, newScientist);
            base.GiveNameToActor(m_DiceRoller, newScientist);
            newScientist.Name = "Dr. " + newScientist.Name;

            // starting skills.
            base.GiveStartingSkillToActor(newScientist, Skills.IDs.HAULER);
            base.GiveStartingSkillToActor(newScientist, Skills.IDs.HAULER);
            base.GiveStartingSkillToActor(newScientist, Skills.IDs.HAULER);
            base.GiveStartingSkillToActor(newScientist, Skills.IDs.NECROLOGY);
            base.GiveStartingSkillToActor(newScientist, Skills.IDs.NECROLOGY);
            base.GiveStartingSkillToActor(newScientist, Skills.IDs.NECROLOGY);
            base.GiveStartingSkillToActor(newScientist, Skills.IDs.NECROLOGY);
            base.GiveStartingSkillToActor(newScientist, Skills.IDs.NECROLOGY);
            base.GiveStartingSkillToActor(newScientist, Skills.IDs.STRONG_PSYCHE);
            base.GiveStartingSkillToActor(newScientist, Skills.IDs.STRONG_PSYCHE);

            // give items.
            newScientist.Inventory.AddAll(MakeItemCHARLaptop());
            newScientist.Inventory.AddAll(MakeItemZTracker());
            newScientist.Inventory.AddAll(MakeItemPistol());
            newScientist.Inventory.AddAll(MakeItemLightPistolAmmo());
            newScientist.Inventory.AddAll(MakeItemArmyRation());
            newScientist.Inventory.AddAll(MakeItemArmyRation());
            newScientist.Inventory.AddAll(MakeItemArmyRation());
            newScientist.Inventory.AddAll(MakeItemBiohazardSuit());
            newScientist.Inventory.AddAll(MakeItemBigFlashlight());
            if (Rules.HasAntiviralPills(m_Game.Session.GameMode))
                newScientist.Inventory.AddAll(MakeItemPillsAntiviral());
            else
                newScientist.Inventory.AddAll(MakeItemLargeMedikit());

            // done.
            return newScientist;
        }

        public Actor CreateNewArmyNationalGuard(int spawnTime, string rankName)
        {
            // model.
            ActorModel model = m_Game.GameActors.NationalGuard;

            // create.
            Actor newNat = model.CreateNumberedName(m_Game.GameFactions.TheArmy, spawnTime);

            // setup.
            DressArmy(m_DiceRoller, newNat);
            base.GiveNameToActor(m_DiceRoller, newNat);
            newNat.Name = rankName + " " + newNat.Name;

            // starting skills
            GiveStartingSkillToActor(newNat, Skills.IDs.CARPENTRY); //carpentry for building small barricades.
            GiveStartingSkillToActor(newNat, Skills.IDs.FIREARMS); //alpha 10
            GiveStartingSkillToActor(newNat, Skills.IDs.FIREARMS); //@@MP (Release 7-2)
            GiveStartingSkillToActor(newNat, Skills.IDs.MEDIC); //@@MP (Release 7-2)
            GiveStartingSkillToActor(newNat, Skills.IDs.HAULER); //@@MP (Release 7-2)
            GiveStartingSkillToActor(newNat, Skills.IDs.HAULER); //@@MP (Release 7-2)
            GiveStartingSkillToActor(newNat, Skills.IDs.HAULER); //@@MP (Release 7-2)

            // give skills : 1 per day after min arrival date.
            int nbSkills = new WorldTime(spawnTime).Day - RogueGame.NATGUARD_DAY;
            if (nbSkills > 0)
                base.GiveRandomSkillsToActor(m_DiceRoller, newNat, nbSkills);

            // give items.
            newNat.Inventory.AddAll(MakeItemArmyRifle());
            newNat.Inventory.AddAll(MakeItemHeavyRifleAmmo());
            newNat.Inventory.AddAll(MakeItemHeavyRifleAmmo()); //@@MP (Release 7-2)
            newNat.Inventory.AddAll(MakeItemArmyPistol());
            newNat.Inventory.AddAll(MakeItemHeavyPistolAmmo());
            newNat.Inventory.AddAll(MakeItemArmyBodyArmor());
            ItemBarricadeMaterial planks = MakeItemWoodenPlank();
            planks.Quantity = m_Game.GameItems.WOODENPLANK.StackingLimit;
            newNat.Inventory.AddAll(planks);
            newNat.Inventory.AddAll(MakeItemArmyRation()); //@@MP (Release 7-2)
            newNat.Inventory.AddAll(MakeItemLargeMedikit()); //@@MP (Release 7-2)
            newNat.Inventory.AddAll(MakeItemBigFlashlight()); //@@MP (Release 7-6)

            // done.
            return newNat;
        }

        public Actor CreateNewBikerMan(int spawnTime, GameGangs.IDs gangId)
        {
             // decide model.
            ActorModel model = m_Game.GameActors.BikerMan;

            // create.
            Actor newBiker = model.CreateNumberedName(m_Game.GameFactions.TheBikers, spawnTime);

            // setup.
            newBiker.GangID = (int)gangId;
            DressBiker(m_DiceRoller, newBiker);
            base.GiveNameToActor(m_DiceRoller, newBiker);
            newBiker.Controller = new GangAI();

            // give items.
            newBiker.Inventory.AddAll(m_DiceRoller.RollChance(50) ? MakeItemCrowbar() : MakeItemBarbedWireBat());
            newBiker.Inventory.AddAll(MakeItemBikerGangJacket(gangId));
            newBiker.Inventory.AddAll(MakeItemLiquorForMolotov()); //@@MP (Release 7-2)
            newBiker.Inventory.AddAll(MakeItemCigarettes()); //@@MP (Release 7-2)
            newBiker.Inventory.AddAll(MakeItemBigFlashlight()); //@@MP (Release 7-2)
            newBiker.Inventory.AddAll(MakeItemMatches()); //@@MP (Release 7-6)
            newBiker.Inventory.AddAll(MakeItemSiphonKit()); //@@MP (Release 7-2)

            // give skills : 1 per day after min arrival date.
            int nbSkills = new WorldTime(spawnTime).Day - RogueGame.BIKERS_RAID_DAY;
            if (nbSkills > 0)
                base.GiveRandomSkillsToActor(m_DiceRoller, newBiker, nbSkills);

            // done.
            return newBiker;
        }

        public Actor CreateNewGangstaMan(int spawnTime, GameGangs.IDs gangId)
        {
            // decide model.
            ActorModel model = m_Game.GameActors.GangstaMan;

            // create.
            Actor newGangsta = model.CreateNumberedName(m_Game.GameFactions.TheGangstas, spawnTime);

            // setup.
            newGangsta.GangID = (int)gangId;
            DressGangsta(m_DiceRoller, newGangsta);
            base.GiveNameToActor(m_DiceRoller, newGangsta);
            newGangsta.Controller = new GangAI();

            // give items.
            newGangsta.Inventory.AddAll(m_DiceRoller.RollChance(50) ? MakeItemSMG() : MakeItemMachete());
            newGangsta.Inventory.AddAll(MakeItemLiquorForMolotov()); //@@MP (Release 7-2)
            newGangsta.Inventory.AddAll(MakeItemCigarettes()); //@@MP (Release 7-2)
            newGangsta.Inventory.AddAll(MakeItemFlashlight()); //@@MP (Release 7-2)
            newGangsta.Inventory.AddAll(MakeItemLightPistolAmmo()); //@@MP (Release 7-2)
            newGangsta.Inventory.AddAll(MakeItemMatches()); //@@MP (Release 7-6)
            newGangsta.Inventory.AddAll(MakeItemBrassKnuckles()); //@@MP (Release 7-6)

            // give skills : 1 per day after min arrival date.
            int nbSkills = new WorldTime(spawnTime).Day - RogueGame.GANGSTAS_RAID_DAY;
            if (nbSkills > 0)
                base.GiveRandomSkillsToActor(m_DiceRoller, newGangsta, nbSkills);

            // done.
            return newGangsta;
        }

        public Actor CreateNewBlackOps(int spawnTime, string rankName)
        {
            // model.
            ActorModel model = m_Game.GameActors.BlackOps;

            // create.
            Actor newBO = model.CreateNumberedName(m_Game.GameFactions.TheBlackOps, spawnTime);

            // setup.
            DressBlackOps(m_DiceRoller, newBO);
            base.GiveNameToActor(m_DiceRoller, newBO);
            newBO.Name = rankName + " " + newBO.Name;

            // starting skills.    //@@MP (Release 7-2)
            GiveStartingSkillToActor(newBO, Skills.IDs.AGILE);
            GiveStartingSkillToActor(newBO, Skills.IDs.AWAKE);
            GiveStartingSkillToActor(newBO, Skills.IDs.AWAKE);
            GiveStartingSkillToActor(newBO, Skills.IDs.FIREARMS);
            GiveStartingSkillToActor(newBO, Skills.IDs.FIREARMS);
            GiveStartingSkillToActor(newBO, Skills.IDs.FIREARMS);
            GiveStartingSkillToActor(newBO, Skills.IDs.BOWS_EXPLOSIVES);
            GiveStartingSkillToActor(newBO, Skills.IDs.HAULER);
            GiveStartingSkillToActor(newBO, Skills.IDs.HAULER); //@@MP (Release 7-6)
            GiveStartingSkillToActor(newBO, Skills.IDs.HAULER); //@@MP (Release 8-1)
            GiveStartingSkillToActor(newBO, Skills.IDs.HIGH_STAMINA);
            GiveStartingSkillToActor(newBO, Skills.IDs.LIGHT_FEET);
            GiveStartingSkillToActor(newBO, Skills.IDs.LIGHT_FEET);
            GiveStartingSkillToActor(newBO, Skills.IDs.LIGHT_SLEEPER);
            GiveStartingSkillToActor(newBO, Skills.IDs.MARTIAL_ARTS);
            GiveStartingSkillToActor(newBO, Skills.IDs.MEDIC);
            GiveStartingSkillToActor(newBO, Skills.IDs.STRONG_PSYCHE);
            GiveStartingSkillToActor(newBO, Skills.IDs.STRONG_PSYCHE);
            GiveStartingSkillToActor(newBO, Skills.IDs.NECROLOGY);

            // give items.
            newBO.Inventory.AddAll(MakeItemPrecisionRifle());
            newBO.Inventory.AddAll(MakeItemPrecisionRifleAmmo()); //@@MP (Release 6-6)
            newBO.Inventory.AddAll(MakeItemPrecisionRifleAmmo());
            newBO.Inventory.AddAll(MakeItemArmyPistol());
            newBO.Inventory.AddAll(MakeItemHeavyPistolAmmo());
            newBO.Inventory.AddAll(MakeItemBlackOpsGPS());
            newBO.Inventory.AddAll(MakeItemLargeMedikit()); //@@MP (Release 7-2)
            newBO.Inventory.AddAll(MakeItemNightVisionGoggles()); //@@MP (Release 7-2)
            newBO.Inventory.AddAll(MakeItemArmyRation()); //@@MP (Release 7-6)
            newBO.Inventory.AddAll(MakeItemArmyRation()); //@@MP (Release 8-1)

            // done.
            return newBO;
        }

        public Actor CreateNewFeralDog(int spawnTime)
        {
            // model
            ActorModel model = m_Game.GameActors.FeralDog;

            // create
            Actor newDog = model.CreateNumberedName(m_Game.GameFactions.TheFerals, spawnTime);

            // skin
            SkinDog(m_DiceRoller, newDog);

            // done.
            return newDog;
        }

        public Actor CreateNewRabbit(int spawnTime) //@@MP (Release 7-6)
        {
            // model
            ActorModel model = m_Game.GameActors.Rabbit;

            // create
            Actor newRabbit = model.CreateNumberedName(m_Game.GameFactions.TheUnintelligentAnimals, spawnTime);

            // skin
            newRabbit.Doll.AddDecoration(DollPart.SKIN, GameImages.RABBIT_SKIN_EAST);

            // done.
            return newRabbit;
        }

        public Actor CreateNewChicken(int spawnTime) //@@MP (Release 7-6)
        {
            // model
            ActorModel model = m_Game.GameActors.Chicken;

            // create
            Actor newChicken = model.CreateNumberedName(m_Game.GameFactions.TheUnintelligentAnimals, spawnTime);

            // skin
            newChicken.Doll.AddDecoration(DollPart.SKIN, GameImages.CHICKEN_SKIN_EAST);

            // done.
            return newChicken;
        }
#endregion

#region Exits
        /// <summary>
        /// Add the Exit with the decoration.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="fromPosition"></param>
        /// <param name="to"></param>
        /// <param name="toPosition"></param>
        /// <param name="exitImageID"></param>
        static void AddExit(Map from, Point fromPosition, Map to, Point toPosition, string exitImageID, bool isAnAIExit) //@@MP - made static (Release 5-7)
        {
            from.SetExitAt(fromPosition, new Exit(to, toPosition) { IsAnAIExit = isAnAIExit });
            from.GetTileAt(fromPosition).AddDecoration(exitImageID);
        }
#endregion

#region Zones
        protected static void MakeWalkwayZones(Map map, Block b) //@@MP - made static (Release 5-7)
        {
            /*
             *  NNNE
             *  W  E
             *  W  E
             *  WSSS
             *
             */
            Rectangle r = b.Rectangle;

            // N
            map.AddZone(MakeUniqueZone("sidewalk", new Rectangle(r.Left, r.Top, r.Width - 1, 1)));
            // S
            map.AddZone(MakeUniqueZone("sidewalk", new Rectangle(r.Left + 1, r.Bottom - 1, r.Width - 1, 1)));
            // E
            map.AddZone(MakeUniqueZone("sidewalk", new Rectangle(r.Right - 1, r.Top, 1, r.Height - 1)));
            // W
            map.AddZone(MakeUniqueZone("sidewalk", new Rectangle(r.Left, r.Top + 1, 1, r.Height - 1)));
        }
#endregion
    }
}
