using System;
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
        const int PARK_BENCH_CHANCE = 5;
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

            // Special buildings.
            #region
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
            if (armyOfficesCount == 0) //unless we've already generated one in the world, as that's all we want
            {
                foreach (Block b in emptyBlocks)
                {
                    if (m_Params.District.Kind == DistrictKind.GREEN)// || m_Params.District.Kind == DistrictKind.GENERAL)
                    {
                        if (armyOfficesCount == 0)
                        {
                            ArmyBuildingType btype = MakeArmyOffice(map, b);
                            if (btype != ArmyBuildingType.NONE)
                            {
                                if (btype == ArmyBuildingType.OFFICE) //it will be because we don't have any others
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
                Logger.WriteLine(Logger.Stage.RUN_MAIN, "Army offices generated: " + armyOfficesCount.ToString());
                foreach (Block b in completedBlocks)
                    emptyBlocks.Remove(b);
            }
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

            // Business District buildings..
            completedBlocks.Clear();
            int charOfficesCount = 0;
            bool hasLibrary = false; //@@MP - (Release 5-3)
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
                    if (rolled >= 33 || NoCHARBuildingMade == true) //@@MP (Release 4)
                    {
                        //@@MP - the first 2 need specific sizes, the other 3 are good for whatever
                        if ((b.InsideRect.Width <= 7 && b.InsideRect.Height < 10) || (b.InsideRect.Width < 10 && b.InsideRect.Height <= 7))
                            MakeMechanicWorkshop(map, b);
                        else if (!hasLibrary && (b.InsideRect.Width > 10 || b.InsideRect.Height > 10)) //@@MP - added a check to ensure only 1 library per district (Release 5-3)
                        {
                            MakeLibraryBuilding(map, b);
                            hasLibrary = true; //@@MP - (Release 5-3)
                        }
                        else
                        {
                            int roll2 = m_DiceRoller.Roll(0, 3);
                            switch (roll2)
                            {
                                case 0: MakeBarBuilding(map, b); break;
                                case 1: MakeBankBuilding(map, b); break;
                                case 2: MakeClinicBuilding(map, b); break;
                            }
                        }

                        completedBlocks.Add(b);
                    }
                }
            }
            foreach (Block b in completedBlocks)
                emptyBlocks.Remove(b);

            // parks.
            completedBlocks.Clear();
            foreach (Block b in emptyBlocks)
            {
                if (m_DiceRoller.RollChance(m_Params.ParkBuildingChance)) //@@MP (Release 4)
                {
                    int rolled = m_DiceRoller.Roll(0, 99);
                    if (rolled >= 30)
                        MakeParkBuilding(map, b, false); //@@MP - ordinary park, 70%
                    else if (rolled >= 15 && rolled < 29)
                        MakeParkBuilding(map, b, true); //@@MP - graveyard, 15%
                    else
                        MakeJunkyard(map, b); //15%

                    completedBlocks.Add(b);
                }
            }
            foreach (Block b in completedBlocks)
                emptyBlocks.Remove(b);

            // all the rest is housings.
            completedBlocks.Clear();
            foreach (Block b in emptyBlocks)
            {
                int rolled = m_DiceRoller.Roll(0, 99);
                if (rolled >= 89) //@@MP (Release 4)
                    MakeChurchBuilding(map, b);
                else
                    MakeHousingBuilding(map, b);

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
                    if (z.Name.Contains(RogueGame.NAME_SEWERS_MAINTENANCE) || z.Name.Contains(RogueGame.NAME_SUBWAY_STATION) || z.Name.Contains("office") || z.Name.Contains("shop"))
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

        protected virtual bool MakeShopBuilding(Map map, Block b)
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
            ShopType shopType = (ShopType)m_DiceRoller.Roll((int)ShopType._FIRST, (int)ShopType._COUNT);

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
                shopBasement.BgMusic = GameMusics.SEWERS;

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
                    map.PlaceMapObjectAt(MakeObjCashRegister(GameImages.OBJ_CASH_REGISTER), cornerpt);
                    if (shopType == ShopType.CONSTRUCTION) //@@MP - add dynamite if it's a Construction store (Release 4), removed roll (Release 6-3)
                        map.DropItemAt(MakeItemDynamite(), cornerpt);
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
            if (b.InsideRect.Width < 6 || b.InsideRect.Height < 6)
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
            string strDoorSide; //@@MP - hold the side of the door for use when creating a cash register at step 6

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
            ItemsDrop(map, b.InsideRect,
                (pt) =>
                {
                    MapObject mapObj = map.GetMapObjectAt(pt);
                    if (mapObj == null)
                        return false;
                    return mapObj.ImageID == GameImages.OBJ_BOOK_SHELVES;
                },
                (pt) => MakeItemBook());
            #endregion

            ///////////
            // 5. Zone
            ///////////
            // shop building.
            map.AddZone(MakeUniqueZone(shopName, b.BuildingRect));
            // walkway zones.
            MakeWalkwayZones(map, b);

            ///////////////////
            // 6. Cash register
            // @@MP - add a cash register in an corner near the door
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
                    map.PlaceMapObjectAt(MakeObjCashRegister(GameImages.OBJ_CASH_REGISTER), cornerpt);
            }
            #endregion

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

                    //@@MP - lectern opposite to the door
                    Point pt = new Point(b.BuildingRect.Right - 2, midY);
                    map.PlaceMapObjectAt(MakeObjDrawer(GameImages.OBJ_LECTERN), pt);
                    //@@MP - wall hanging behind lectern
                    Tile tile = map.GetTileAt(b.BuildingRect.Right - 2, midY - 1);
                    tile.AddDecoration(chosenHanging);
                    tile = map.GetTileAt(b.BuildingRect.Right - 2, midY + 1);
                    tile.AddDecoration(chosenHanging);
                }
                else
                {
                    // east
                    PlaceDoor(map, b.BuildingRect.Right - 1, midY, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjWoodenDoor());

                    //@@MP - lectern opposite to the door
                    Point pt = new Point(b.BuildingRect.Left + 1, midY);
                    map.PlaceMapObjectAt(MakeObjDrawer(GameImages.OBJ_LECTERN), pt);
                    //@@MP - wall hanging behind lectern
                    Tile tile = map.GetTileAt(b.BuildingRect.Left + 1, midY - 1);
                    tile.AddDecoration(chosenHanging);
                    tile = map.GetTileAt(b.BuildingRect.Left + 1, midY + 1);
                    tile.AddDecoration(chosenHanging);
                }
            }
            else
            {
                bool north = m_DiceRoller.RollChance(50);

                if (north)
                {
                    // north
                    PlaceDoor(map, midX, b.BuildingRect.Top, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjWoodenDoor());

                    //@@MP - lectern opposite to the door
                    Point pt = new Point(midX, b.BuildingRect.Bottom - 2);
                    map.PlaceMapObjectAt(MakeObjDrawer(GameImages.OBJ_LECTERN), pt);
                    //@@MP - wall hanging behind lectern
                    Tile tile = map.GetTileAt(midX + 1, b.BuildingRect.Bottom - 2);
                    tile.AddDecoration(chosenHanging);
                    tile = map.GetTileAt(midX - 1, b.BuildingRect.Bottom - 2);
                    tile.AddDecoration(chosenHanging);
                }
                else
                {
                    // south
                    PlaceDoor(map, midX, b.BuildingRect.Bottom - 1, m_Game.GameTiles.FLOOR_WALKWAY, MakeObjWoodenDoor());

                    //@@MP - lectern opposite to the door
                    Point pt = new Point(midX, b.BuildingRect.Top + 1);
                    map.PlaceMapObjectAt(MakeObjDrawer(GameImages.OBJ_LECTERN), pt);
                    //@@MP - wall hanging behind lectern
                    Tile tile = map.GetTileAt(midX + 1, b.BuildingRect.Top + 1);
                    tile.AddDecoration(chosenHanging);
                    tile = map.GetTileAt(midX - 1, b.BuildingRect.Top + 1);
                    tile.AddDecoration(chosenHanging);
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
            // walkway zones.
            MakeWalkwayZones(map, b);
            
            // Done
            return true;
        }

        protected virtual bool MakeBarBuilding(Map map, Block b) //@@MP (Release 4)
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
                            map.PlaceMapObjectAt(MakeObjBarCounter(GameImages.OBJ_KITCHEN_COUNTER), counterpt); //@@MP - changed to BarCounter to keep them jumpable (Release 5-3)
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
                            map.PlaceMapObjectAt(MakeObjBarCounter(GameImages.OBJ_KITCHEN_COUNTER), counterpt); //@@MP - changed to BarCounter to keep them jumpable (Release 5-3)
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
                            map.PlaceMapObjectAt(MakeObjBarCounter(GameImages.OBJ_KITCHEN_COUNTER), counterpt); //@@MP - changed to BarCounter to keep them jumpable (Release 5-3)
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
                            map.PlaceMapObjectAt(MakeObjBarCounter(GameImages.OBJ_KITCHEN_COUNTER), counterpt); //@@MP - changed to BarCounter to keep them jumpable (Release 5-3)
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
        
        protected virtual bool MakeMechanicWorkshop(Map map, Block b) //@@MP (Release 4)
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
                    else
                        return MakeObjWorkbench(GameImages.OBJ_WORKBENCH); //@@MP (Release 5-3)
                });

            MapObjectFill(map, b.InsideRect,
                            (pt) =>
                            {
                                if (m_DiceRoller.RollChance(m_Params.WreckedCarChance))
                                {
                                    Tile tile = map.GetTileAt(pt.X, pt.Y);
                                    if (tile.IsInside && tile.Model.IsWalkable)// && tile.Model != m_Game.GameTiles.WALL_STONE)
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

        protected virtual bool MakeClinicBuilding(Map map, Block b) //@@MP (Release 4)
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
                    else if (m_DiceRoller.RollChance(50))
                    {
                        map.DropItemAt(MakeShopPharmacyItem(), pt);
                        return MakeObjShelf(GameImages.OBJ_CLINIC_CUPBOARD);
                    }
                    else
                        return null;
                });

            // beds, curtain, machinery
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
                        map.DropItemAt(MakeShopPharmacyItem(), pt);

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
                        return MakeObjFence(GameImages.OBJ_FENCE); //@@MP - standard chain wire fence
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
                                if (m_DiceRoller.RollChance(65))
                                {
                                    Tile tile = map.GetTileAt(pt.X, pt.Y);
                                    if (tile.Model.IsWalkable)// && tile.Model != m_Game.GameTiles.WALL_STONE) //@@MP - removed (tile.IsInside && ...) (Release 5-3)
                                    {
                                        MapObject thing;
                                        int rolled = m_DiceRoller.Roll(0, 99);
                                        if (rolled >= 80)
                                            thing = MakeObjWreckedCar(m_DiceRoller); //20%
                                        else if (rolled >= 40 && rolled < 79)
                                            thing = MakeObjJunk(GameImages.OBJ_JUNK); //40%
                                        else
                                            thing = MakeObjBarrels(GameImages.OBJ_BARRELS); //40%

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

        protected virtual bool MakeBankBuilding(Map map, Block b) //@@MP (Release 4)
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
                            map.PlaceMapObjectAt(MakeObjBankSafe(GameImages.OBJ_BANK_SAFE), safept);

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
                            map.PlaceMapObjectAt(MakeObjBankSafe(GameImages.OBJ_BANK_SAFE), safept);

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
                            map.PlaceMapObjectAt(MakeObjBankSafe(GameImages.OBJ_BANK_SAFE), safept);

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
                            map.PlaceMapObjectAt(MakeObjBankSafe(GameImages.OBJ_BANK_SAFE), safept);

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
                    nbTables = m_DiceRoller.Roll(Math.Max((customerarealeft - customerarearight), b.InsideRect.Height), Math.Max((customerarealeft - customerarearight), b.InsideRect.Height) / 3);
                    break;
                case 2:
                case 3:
                    nbTables = m_DiceRoller.Roll(Math.Max(b.InsideRect.Width, (customerareabottom - customerareatop)), Math.Max(b.InsideRect.Width, (customerareabottom - customerareatop) / 3));
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
                            (pt2) => MakeObjChair(GameImages.OBJ_CHAIR));
                        MapObjectPlaceInGoodPosition(map, adjTableRect,
                            (pt2) => pt2 != pt && !IsADoorNSEW(map, pt2.X, pt2.Y),
                            m_DiceRoller,
                            (pt2) => MakeObjChair(GameImages.OBJ_CHAIR));

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

        private void PlaceBankVaultDoor(Map map, Point pt) //@@MP (Release 6-5)
        {
            //randomly select 
            int roll = m_DiceRoller.Roll(0, 3);
            switch (roll)
            {
                case 0:
                    map.PlaceMapObjectAt(MakeObjIronDoor(DoorWindow.STATE_LOCKED), pt);
                    DoorWindow lockedDoor = (map.GetMapObjectAt(pt) as DoorWindow);
                    lockedDoor.SetState(DoorWindow.STATE_LOCKED);
                    break;
                case 1:
                    map.PlaceMapObjectAt(MakeObjIronDoor(DoorWindow.STATE_LOCKED), pt);
                    DoorWindow openDoor = (map.GetMapObjectAt(pt) as DoorWindow);
                    openDoor.SetState(DoorWindow.STATE_OPEN);
                    break;
                case 2:
                    map.PlaceMapObjectAt(MakeObjIronDoor(DoorWindow.STATE_LOCKED), pt);
                    DoorWindow brokenDoor = (map.GetMapObjectAt(pt) as DoorWindow);
                    brokenDoor.SetState(DoorWindow.STATE_BROKEN);
                    break;
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
                (pt) => MakeObjCHARdesktop(GameImages.OBJ_CHAR_DESKTOP));
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
                map.AddZone(MakeUniqueZone("Office room", roomRect));
            }
            foreach (Rectangle roomRect in officesTwo)
            {
                TileRectangle(map, m_Game.GameTiles.WALL_CHAR_OFFICE, roomRect);
                map.AddZone(MakeUniqueZone("Office room", roomRect));
            }

            foreach (Rectangle roomRect in officesOne)
            {
                if (horizontalCorridor)
                {
                    PlaceDoor(map, roomRect.Left + roomRect.Width / 2, roomRect.Bottom - 1, m_Game.GameTiles.FLOOR_OFFICE, MakeObjCharDoor());
                }
                else
                {
                    PlaceDoor(map, roomRect.Right - 1, roomRect.Top + roomRect.Height / 2, m_Game.GameTiles.FLOOR_OFFICE, MakeObjCharDoor());
                }
            }
            foreach (Rectangle roomRect in officesTwo)
            {
                if (horizontalCorridor)
                {
                    PlaceDoor(map, roomRect.Left + roomRect.Width / 2, roomRect.Top, m_Game.GameTiles.FLOOR_OFFICE, MakeObjCharDoor());
                }
                else
                {
                    PlaceDoor(map, roomRect.Left, roomRect.Top + roomRect.Height / 2, m_Game.GameTiles.FLOOR_OFFICE, MakeObjCharDoor());
                }
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
                            (pt) => MakeObjCHARdesktop(GameImages.OBJ_CHAR_DESKTOP));
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
                map.AddZone(MakeUniqueZone("Office room", roomRect));
            }
            foreach (Rectangle roomRect in officesTwo)
            {
                TileRectangle(map, m_Game.GameTiles.WALL_ARMY_BASE, roomRect);
                map.AddZone(MakeUniqueZone("Office room", roomRect));
            }

            foreach (Rectangle roomRect in officesOne)
            {
                if (horizontalCorridor)
                {
                    PlaceDoor(map, roomRect.Left + roomRect.Width / 2, roomRect.Bottom - 1, m_Game.GameTiles.FLOOR_ARMY, MakeObjIronDoor(DoorWindow.STATE_CLOSED));
                }
                else
                {
                    PlaceDoor(map, roomRect.Right - 1, roomRect.Top + roomRect.Height / 2, m_Game.GameTiles.FLOOR_ARMY, MakeObjIronDoor(DoorWindow.STATE_CLOSED));
                }
            }
            foreach (Rectangle roomRect in officesTwo)
            {
                if (horizontalCorridor)
                {
                    PlaceDoor(map, roomRect.Left + roomRect.Width / 2, roomRect.Top, m_Game.GameTiles.FLOOR_ARMY, MakeObjIronDoor(DoorWindow.STATE_CLOSED));
                }
                else
                {
                    PlaceDoor(map, roomRect.Left, roomRect.Top + roomRect.Height / 2, m_Game.GameTiles.FLOOR_ARMY, MakeObjIronDoor(DoorWindow.STATE_CLOSED));
                }
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
                            (pt) => MakeObjCHARdesktop(GameImages.OBJ_ARMY_COMPUTER_STATION));
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
                            if (!isgraveyard) //@@MP (Release 4)
                                return MakeObjFence(GameImages.OBJ_FENCE); //@@MP - standard chain wire fence
                            else
                                return MakeObjIronFence(GameImages.OBJ_GRAVEYARD_FENCE); //@@MP - corrected to an iron fence (Release 5-4)
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
                    bool placeGraveOrTree = m_DiceRoller.RollChance(PARK_GRAVE_OR_TREE_CHANCE); //@@MP - use the original tree chance, but within that a higher chance to be a grave instead
                    if (placeGraveOrTree)
                    {
                        int placeObject = m_DiceRoller.Roll(0, 10);
                        switch (placeObject)
                        {
                            case 0: return MakeObjTree(GameImages.OBJ_TREE); //10%
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
                MapObjectFill(map, b.InsideRect,
                (pt) =>
                {
                    bool placeTree = m_DiceRoller.RollChance(PARK_TREE_CHANCE);
                    if (placeTree)
                        return MakeObjTree(GameImages.OBJ_TREE);
                    else if (m_DiceRoller.RollChance(PARK_ITEM_CHANCE)) //@@MP (Release 4)
                    {
                        map.DropItemAt(MakeItemWildBerries(), pt); //@@MP (Release 5-3)
                        return MakeObjBerryBush(GameImages.OBJ_BERRY_BUSH);
                    }
                    else
                        return null;
                });
            }

            MapObjectFill(map, b.InsideRect,
                (pt) =>
                {
                    bool placeBench = m_DiceRoller.RollChance(PARK_BENCH_CHANCE);
                    if (placeBench)
                        return MakeObjBench(GameImages.OBJ_BENCH);
                    else
                        return null;
                });

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
            map.SetTileModelAt(ex, ey, m_Game.GameTiles.FLOOR_WALKWAY);

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
                    Rectangle pondRect = new Rectangle(pondX, pondY, PARK_POND_WIDTH, PARK_POND_HEIGHT); //for the edge tiles (a al walls)
                    Rectangle pondInsideRect = new Rectangle(pondX + 1, pondY + 1, PARK_POND_WIDTH - 2, PARK_POND_HEIGHT - 2); //for the total water tiles (a la floor)

                    // clear everything but zones in pond location
                    ClearRectangle(map, pondRect, false);

                    // build it
                    MakeParkPondBuilding(map, "Pond", pondRect);
                }
            }

            // Done.
            return true;
        }

        protected virtual void MakeParkPondBuilding(Map map, string baseZoneName, Rectangle pondBuildingRect)  //@@MP - based on alpha 10 shed (Release 6-1)
        {
            Rectangle pondInsideRect = new Rectangle(pondBuildingRect.X + 1, pondBuildingRect.Y + 1, pondBuildingRect.Width - 2, pondBuildingRect.Height - 2);

            // build & zone
            TileFill(map, m_Game.GameTiles.FLOOR_POND_CENTER, pondInsideRect, (tile, prevTileModel, x, y) => tile.IsInside = false); //@@MP made false (Release 6-1)
            map.AddZone(MakeUniqueZone(baseZoneName, pondInsideRect));

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
                                        map.PlaceMapObjectAt(MakeObjTree(GameImages.OBJ_TREE), pos);
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
                                    map.PlaceMapObjectAt(MakeObjWoodenFence(GameImages.OBJ_PICKET_FENCE), pos);
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
                                    map.PlaceMapObjectAt(MakeObjFence(GameImages.OBJ_FENCE), pos);
                                }
                            });
                        break;
                }
            }

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
            // bunch of tables near walls with construction items on them.
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

                        // add table.
                        return MakeObjTable(GameImages.OBJ_TABLE);
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
                        continue;
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
                map.PlaceMapObjectAt(MakeObjIronGate(GameImages.OBJ_GATE_CLOSED), new Point(ironFencePos.X, ironFencePos.Y));
                map.PlaceMapObjectAt(MakeObjIronGate(GameImages.OBJ_GATE_CLOSED), new Point(ironFencePos.X + 1, ironFencePos.Y));
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
                for (int by = b.InsideRect.Top + 1; by < b.InsideRect.Bottom - 1; by++)
                {
                    // next to walls and no doors.
                    if (CountAdjWalls(map, bx, by) < 2 || CountAdjDoors(map, bx, by) > 0)
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
                                                Item it = MakeItemBook();
                                                if (it != null)
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
                                (pt) => MakeObjStandingLamp(GameImages.OBJ_PIANO));
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
                    return MakeItemAlcohol();
                default:
                    throw new ArgumentOutOfRangeException("shop","unhandled shoptype");

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
            int randomItem = m_DiceRoller.Roll(0, 6);
            switch (randomItem)
            {
                case 0: return MakeItemBandages();
                case 1: return MakeItemMedikit();
                case 2: return MakeItemPillsSLP();
                case 3: return MakeItemPillsSTA();
                case 4: //@@MP - if Sanity is disabled generate other (minor) pharmacy items instead (Release 1)
                    if (RogueGame.Options.IsSanityEnabled) //@MP - fixed crappy implementation (Release 5-2)
                        return MakeItemPillsSAN();
                    else
                    {
                        if (m_DiceRoller.RollChance(10)) //@@MP - re-did this section; it was pretty convoluted (Release 5-2)
                            return (RogueGame.Options.VTGAntiviralPills) ? MakeItemPillsAntiviral() : MakeItemBandages();
                        else
                            return MakeItemBandages();
                    }
                case 5: return MakeItemStenchKiller();

                default:
                    throw new InvalidOperationException("unhandled roll");
            }
        }

        public Item MakeShopSportsWearItem() //@@MP (Release 3)
        {
            int roll = m_DiceRoller.Roll(0, 11);

            switch (roll)
            {
                case 0: 
                case 1: return MakeItemHockeyStick();
                case 2: 
                case 3: return MakeItemGolfClub();
                case 4:
                case 5: return MakeItemBaseballBat();
                case 6:
                case 7: return MakeItemIronGolfClub();
                case 8: 
                case 9: return MakeItemTennisRacket();
                case 10: return MakeItemMagazines();
                default:
                    throw new InvalidOperationException("unhandled roll");
            }
        }

        public Item MakeShopConstructionItem() //@@MP - split each item into its own single roll (Release 3)
        {
            int roll = m_DiceRoller.Roll(0, 18);
            switch (roll)
            {
                case 0: return MakeItemStandardAxe();
                case 1: return MakeItemPipeWrench();
                case 2: return MakeItemShovel(); //m_DiceRoller.RollChance(50) ? MakeItemShovel() : MakeItemShortShovel();
                case 3: return MakeItemPickaxe();
                case 4: return MakeItemMachete();
                case 5: return MakeItemCrowbar();
                case 6: return MakeItemHugeHammer(); //m_DiceRoller.RollChance(50) ? MakeItemHugeHammer() : MakeItemSmallHammer();
                case 7: return MakeItemSprayPaint();
                case 8: return MakeItemWoodenPlank();
                case 9: return MakeItemFlashlight();
                case 10: return MakeItemBigFlashlight();
                case 11: return MakeItemSpikes();
                case 12: return MakeItemBarbedWire();
                case 13: return MakeItemSmallHammer();
                case 14: return MakeItemShortShovel();
                case 15: return MakeItemNailGun(); //@@MP (Release 5-1)
                case 16: return MakeItemNailGunAmmo(); //@@MP (Release 5-1)
                case 17: return MakeItemVegetableSeeds(); //@@MP (Release 5-5)    
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
                    case 6: return MakeItemHuntingCrossbow();
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
                    case 1: return MakeItemLightPistolAmmo();
                    case 2: 
                    case 3: return MakeItemShotgunAmmo();
                    case 4:
                    case 5: return MakeItemLightRifleAmmo();
                    case 6: return MakeItemBoltsAmmo();
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
                        case 2: return MakeItemShortShovel(); //@@MP (Release 3)
                        case 3: return MakeItemStandardAxe(); //@@MP (Release 3)
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
                        case 3: return MakeItemMachete(); //@@MP (Release 3)
                        default:
                            return null;
                    }
                }
            }
            else
            {
                // Outfits&Traps
                int roll = m_DiceRoller.Roll(0, 3);
                switch (roll)
                {
                    case 0: return MakeItemHunterVest();
                    case 1: return MakeItemBearTrap();
                    case 3: return MakeItemStenchKiller(); //@@MP added (Release 1)
                    default: 
                        return null;
                }
            }
        }

        public Item MakeShopGeneralItem()
        {
            int roll = m_DiceRoller.Roll(0, 6);
            switch (roll)
            { //@@MP - changed the items to be more realistic (Release 4)
                case 0: return MakeShopPharmacyItem();
                case 1: return MakeShopPharmacyItem();
                case 2: return MakeItemFlashlight();
                case 3: return MakeShopGroceryItem();
                case 4: return MakeItemMagazines();
                case 5: return MakeItemCigarettes();
                case 6: return MakeItemCellPhone(); //@MP - added more phones, because they were too rare (Release 5-4)
                default: 
                    throw new InvalidOperationException("unhandled roll");
            }
        }

        public Item MakeHospitalItem()
        {
            int randomItem = m_DiceRoller.Roll(0, 7);
            switch (randomItem)
            {
                case 0: return MakeItemBandages();
                case 1: return MakeItemMedikit();
                case 2: return MakeItemPillsSLP();
                case 3: return MakeItemPillsSTA();
                case 4: //@@MP - if Sanity is disabled generate other (minor) hospital items instead (Release 1)
                    if (RogueGame.Options.IsSanityEnabled) //@MP - fixed crappy implementation (Release 5-2)
                        return MakeItemPillsSAN();
                    else
                    {
                        if (m_DiceRoller.RollChance(50) && (RogueGame.Options.VTGAntiviralPills)) //@MP - handles new antiviral pills option (Release 5-2)
                            return MakeItemPillsAntiviral();
                        else
                            return MakeItemMedikit();
                    }
                case 5: return MakeItemBandages(); //return MakeItemStenchKiller();
                case 6: return (RogueGame.Options.VTGAntiviralPills) ? MakeItemPillsAntiviral() : MakeItemCannedFood(); //@MP - handles new antiviral pills option (Release 5-2)

                default:
                    throw new InvalidOperationException("unhandled roll");
            }
        }
        
        public Item MakeRandomBedroomItem() //@@ MP (Release 1)(Release 3)
        {
            int randomItem = m_DiceRoller.Roll(0, 25);

            switch (randomItem)
            {
                case 0:
                case 1: return MakeItemBandages();
                case 2: return MakeItemPillsSTA();
                case 3: return MakeItemPillsSLP();
                case 4: //@@MP - if Sanity is disabled generate homely stuff instead (Release 1)
                    if (RogueGame.Options.IsSanityEnabled) //@MP - fixed crappy implem (Release 5-2)
                        return MakeItemPillsSAN();
                    else
                    {
                        if (m_DiceRoller.RollChance(50))
                            return MakeItemBook();
                        else
                            return MakeItemMagazines();
                    }
                case 5: return MakeItemTennisRacket(); //@@MP (Release 3)
                case 6: return MakeItemSmallHammer(); //@@MP (Release 3)
                case 7: return MakeItemIronGolfClub(); //@@MP (Release 3)
                case 8: return MakeItemBaseballBat();
                case 9: return MakeItemRandomPistol();
                case 10: // rare fire weapon
                    if (m_DiceRoller.RollChance(30))
                    {
                        if (m_DiceRoller.RollChance(50))
                            return MakeItemShotgun();
                        else
                            return MakeItemHuntingRifle();
                    }
                    else
                    {
                        if (m_DiceRoller.RollChance(50))
                            return MakeItemShotgunAmmo();
                        else
                            return MakeItemLightRifleAmmo();
                    }
                case 11: 
                case 12:
                case 13: return MakeItemCellPhone();
                case 14:
                case 15: return MakeItemFlashlight();
                case 16: return MakeItemHockeyStick(); //@@MP (Release 3)
                case 17: return MakeItemLightPistolAmmo();
                case 18: return MakeItemPipeWrench(); //@@MP (Release 3)
                case 19: return MakeItemStenchKiller();
                case 20: return MakeItemHunterVest();
                case 21: return MakeItemCigarettes(); //@@MP (Release 4)
                case 22:
                case 23:
                case 24:
                    if (m_DiceRoller.RollChance(50))
                        return MakeItemBook();
                    else
                        return MakeItemMagazines();

                default: throw new InvalidOperationException("unhandled roll");
            }
        }

        public Item MakeRandomKitchenItem()
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

        public Item MakeRandomCHAROfficeItem()
        {
            int randomItem = m_DiceRoller.Roll(0, 10);
            switch (randomItem)
            {
                case 0:
                    // weapons:
                    // - grenade (rare).
                    // - shotgun/ammo
                    if (m_DiceRoller.RollChance(20))
                        return MakeItemGrenade();
                    else
                    {
                        // shotgun/ammo
                        if (m_DiceRoller.RollChance(30))
                            return MakeItemShotgun();
                        else
                            return MakeItemShotgunAmmo();
                    }

                case 1: 
                case 2:
                    if (RogueGame.Options.VTGAntiviralPills) //MP - switched from m_DiceRoller.RollChance(50) to pills unless they are disabled (Release 5-2)
                        return MakeItemPillsAntiviral();
                    else
                        return MakeItemMedikit();

                case 3:
                    return MakeItemCannedFood();

                case 4: // rare tracker items //@@MP - was previously only 50% to drop either (Release 3)
                    if (m_DiceRoller.RollChance(50))
                        return MakeItemZTracker();
                    else
                        return MakeItemBlackOpsGPS();

                default: return null; // 50% chance to find nothing.
            }
        }

        public Item MakeRandomParkItem()
        {
            int randomItem = m_DiceRoller.Roll(0, 9);
            switch (randomItem)
            {
                case 0: return MakeItemSprayPaint();
                case 1: return MakeItemBaseballBat();
                case 2: return MakeItemCannedFood(); //return MakeItemPillsSLP(); //@@MP originally SLP (Release 1)
                case 3: return MakeItemAlcohol(); //return MakeItemPillsSTA(); //@@MP originally STA (Release 1)
                case 4: //return MakeItemPillsSAN(); //@@MP originally SAN (Release 1)
                    if (m_DiceRoller.RollChance(50))
                        return MakeItemBook();
                    else
                        return MakeItemMagazines();
                case 5: return MakeItemFlashlight();
                case 6: return MakeItemCellPhone();
                case 7: return MakeItemWoodenPlank();
                case 8: return MakeItemCigarettes(); //@@MP (Release 4)
                default: throw new InvalidOperationException("Out of Range: unhandled item roll");
            }
        }

        public Item MakeRandomLiquorStoreItem() //@@MP (Release 4)
        {
            return MakeItemAlcohol();
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

        #region Populating buildings
        protected virtual void PopulateCHAROfficeBuilding(Map map, Block b)
        {
            // Guards
            for (int i = 0; i < MAX_CHAR_GUARDS_PER_OFFICE; i++)
            {
                Actor newGuard = CreateNewCHARGuard(0);
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

            // random pilars/walls.
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

                    int roll = m_DiceRoller.Roll(0, 10); //@@MP (Release 3)
                    switch (roll)
                    {
                        case 0: // junk
                        case 5:
                            return MakeObjJunk(GameImages.OBJ_JUNK);
                        case 1: // barrels.
                            return MakeObjBarrels(GameImages.OBJ_BARRELS);
                        case 2: // table with random item.
                            {
                                Item it = MakeShopConstructionItem();
                                basement.DropItemAt(it, pt);
                                return MakeObjTable(GameImages.OBJ_TABLE);
                            };
                        case 3: // drawer with random item.
                            {
                                Item it = MakeShopConstructionItem();
                                basement.DropItemAt(it, pt);
                                return MakeObjDrawer(GameImages.OBJ_DRAWER);
                            };
                        case 4: // bed.
                            return MakeObjBed(GameImages.OBJ_BED);
                        case 6:
                        case 7:
                        case 8:
                        case 9: //@@MP (Release 3)
                            {
                                basement.PlaceMapObjectAt(MakeObjShelf(GameImages.OBJ_SHOP_SHELF), pt);
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
            basement.BgMusic = GameMusics.SEWERS;

            // done.
            return basement;
        }
        #endregion

        #region CHAR Underground Facility
        public Map GenerateUniqueMap_CHARUnderground(Map surfaceMap, Zone officeZone, out Point baseEntryPos) //alpha 10, added baseEntryPos- it's used to mark the CHAR build in City Info
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
            Map underground = new Map((surfaceMap.Seed << 3) ^ surfaceMap.Seed, "CHAR Underground Facility", RogueGame.MAP_MAX_WIDTH, RogueGame.MAP_MAX_HEIGHT)
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
            Zone roomZone = null;
            Point surfaceExit = new Point();
            while (true)    // loop until found.
            {
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

                // find somewhere walkable inside.
                bool foundSurfaceExit = false;
                int attempts = 0;
                do
                {
                    surfaceExit.X = m_DiceRoller.Roll(roomZone.Bounds.Left, roomZone.Bounds.Right);
                    surfaceExit.Y = m_DiceRoller.Roll(roomZone.Bounds.Top, roomZone.Bounds.Bottom);
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

            // barricade the rooms door.
            DoForEachTile(roomZone.Bounds, //@@MP - unused parameter (Release 5-7)
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
            Rectangle qBotLeft = Rectangle.FromLTRB(0, underground.Height/2 + 1 + corridorHalfWidth, qTopLeft.Right, underground.Height);
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
                                MakeCHARLabRoom(underground, insideRoomRect);
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
   
            // CHAR Guards.
            int nbGuards = underground.Width / 10; // 10 for 100.
            for (int i = 0; i < nbGuards; i++)
            {
                Actor guard = CreateNewCHARGuard(0);
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
                    if (CountAdjWalls(map, pt.X, pt.Y) < 3)
                        return null;
                    // dont block exits!
                    if (map.GetExitAt(pt) != null)
                        return null;

                    // table + tracker/armor/weapon.
                    if (m_DiceRoller.RollChance(20))
                    {                        
                        Item it;
                        if (m_DiceRoller.RollChance(20))
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
                                if (m_DiceRoller.RollChance(30))
                                {
                                    it = m_DiceRoller.RollChance(50) ? MakeItemShotgun() : MakeItemHuntingRifle();
                                }
                                else
                                {
                                    it = m_DiceRoller.RollChance(50) ? MakeItemShotgunAmmo() : MakeItemLightRifleAmmo();
                                }
                            }
                        }
                        map.DropItemAt(it, pt);

                        MapObject shelf = MakeObjShelf(GameImages.OBJ_SHOP_SHELF);
                        return shelf;
                    }
                    else
                        return null;
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
                    if (m_DiceRoller.RollChance(50))
                        return m_DiceRoller.RollChance(50) ? MakeObjJunk(GameImages.OBJ_JUNK) : MakeObjBarrels(GameImages.OBJ_BARRELS);
                    else
                    {
                        if (m_DiceRoller.RollChance(20)) //@@MP (Release 3)
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

                    map.DropItemAt(MakeShopConstructionItem(), x, y);
                }
        }

        void MakeCHARLabRoom(Map map, Rectangle roomRect) //@@MP (Release 3)
        {
            Boolean boolPlacedCHARdocument = false;
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
                            return MakeObjCHARdesktop(GameImages.OBJ_CHAR_DESKTOP);
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
                            return MakeObjCHARdesktop(GameImages.OBJ_CHAR_DESKTOP);
                        else
                            return MakeObjCHARdesktop(GameImages.OBJ_CHAR_TABLE);
                    }
                    else
                    {
                        if (!boolPlacedCHARdocument)
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
                            boolPlacedCHARdocument = true; //@@MP - only drop one per room
                        }
                        return null;
                    }
                });
        }

        void MakeCHARLivingRoom(Map map, Rectangle roomRect) //@@MP - no longer used as of release 3
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
                    if (CountAdjWalls(map, pt.X, pt.Y) < 3)
                        return null;
                    // dont block exits!
                    if (map.GetExitAt(pt) != null)
                        return null;

                    // table + meds.
                    if (m_DiceRoller.RollChance(20))
                    {
                        Item it = MakeHospitalItem();                        
                        map.DropItemAt(it, pt);

                        MapObject shelf = MakeObjShelf(GameImages.OBJ_SHOP_SHELF);
                        return shelf;
                    }
                    else
                        return null;
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
            AddExit(map, surfaceStairsPos, officesLevel, new Point(1,1), GameImages.DECO_STAIRS_DOWN, true);
            AddExit(officesLevel, new Point(1,1), map, surfaceStairsPos, GameImages.DECO_STAIRS_UP, true);

            // offices <-> jails
            AddExit(officesLevel, new Point(1, officesLevel.Height - 2), jailsLevel, new Point(1, 1), GameImages.DECO_STAIRS_DOWN, true);
            AddExit(jailsLevel, new Point(1, 1), officesLevel, new Point(1, officesLevel.Height - 2), GameImages.DECO_STAIRS_UP, true);

            // 6. Add maps to district.
            m_Params.District.AddUniqueMap(officesLevel);
            m_Params.District.AddUniqueMap(jailsLevel);

            // 7. Set unique maps.
            m_Game.Session.UniqueMaps.PoliceStation_OfficesLevel = new UniqueMap() { TheMap = officesLevel };
            m_Game.Session.UniqueMaps.PoliceStation_JailsLevel = new UniqueMap() { TheMap = jailsLevel };

            // done!
        }

        void GeneratePoliceStation(Map surfaceMap, Block policeBlock, out Point stairsToLevel1)
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
                    if (CountAdjWalls(surfaceMap, pt.X, pt.Y) == 0 || CountAdjDoors(surfaceMap, pt.X, pt.Y) > 0)
                        return;
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
            Map map = new Map(seed, "Police Station - Offices", 20, 20)
            {
                Lighting = Lighting.LIT
            };
            DoForEachTile(map.Rect, (pt) => map.GetTileAt(pt).IsInside = true); //@@MP - unused parameter (Release 5-7)

            // 2. Floor plan.
            TileFill(map, m_Game.GameTiles.FLOOR_TILES);
            TileRectangle(map, m_Game.GameTiles.WALL_POLICE_STATION, map.Rect);
            // - offices rooms on the east side, doors leading west.
            Rectangle officesRect = Rectangle.FromLTRB(3, 0, map.Width, map.Height);
            List<Rectangle> roomsList = new List<Rectangle>();
            MakeRoomsPlan(map, ref roomsList, officesRect, 5, 5);
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
                            if (!map.IsWalkable(pt.X, pt.Y) || CountAdjWalls(map, pt.X, pt.Y) == 0 || CountAdjDoors(map, pt.X, pt.Y) > 0)
                                return;

                            // shelf.
                            map.PlaceMapObjectAt(MakeObjShelf(GameImages.OBJ_SHOP_SHELF), pt);

                            // weaponry/armor/radios.
                            Item it = null;
                            int roll = m_DiceRoller.Roll(0, 10);
                            switch(roll)
                            {
                                    // 20% armors
                                case 0:
                                case 1:
                                    it = m_DiceRoller.RollChance(50) ? MakeItemPoliceJacket() : MakeItemPoliceRiotArmor();
                                    break;

                                    // 20% light/radio
                                case 2:
                                case 3:
                                    it = m_DiceRoller.RollChance(50) ? (m_DiceRoller.RollChance(50) ? MakeItemFlashlight() : MakeItemBigFlashlight()) : MakeItemPoliceRadio();
                                    break;

                                    // 20% truncheon
                                case 4:
                                case 5:
                                    it = MakeItemTruncheon();
                                    break;

                                    // 20% pistol/ammo - 30% pistol 70% amo
                                case 6:
                                case 7:
                                    it = m_DiceRoller.RollChance(30) ? MakeItemPistol() : MakeItemLightPistolAmmo();
                                    break;

                                    // 20% shotgun/ammo - 30% shotgun 70% amo
                                case 8:
                                case 9:
                                    it = m_DiceRoller.RollChance(30) ? MakeItemShotgun() : MakeItemShotgunAmmo();
                                    break;

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

                    // add furniture : 1 table, 2 chairs.
                    MapObjectPlaceInGoodPosition(map, inRoomRect,
                        (pt) => map.IsWalkable(pt.X, pt.Y) && CountAdjDoors(map, pt.X, pt.Y) == 0,
                        m_DiceRoller,
                        (pt) => MakeObjTable(GameImages.OBJ_TABLE));
                    MapObjectPlaceInGoodPosition(map, inRoomRect,
                        (pt) => map.IsWalkable(pt.X, pt.Y) && CountAdjDoors(map, pt.X, pt.Y) == 0,
                        m_DiceRoller,
                        (pt) => MakeObjChair(GameImages.OBJ_CHAIR));
                    MapObjectPlaceInGoodPosition(map, inRoomRect,
                        (pt) => map.IsWalkable(pt.X, pt.Y) && CountAdjDoors(map, pt.X, pt.Y) == 0,
                        m_DiceRoller,
                        (pt) => MakeObjChair(GameImages.OBJ_CHAIR));

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
            Map map = new Map(seed, "Police Station - Jails", 22, 6)
            {
                Lighting = Lighting.LIT //@@MP - was Darkness (Release 6-4)
            };
            DoForEachTile(map.Rect, (pt) => map.GetTileAt(pt).IsInside = true); //@@MP - unused parameter (Release 5-7)

            // 2. Floor plan.
            TileFill(map, m_Game.GameTiles.FLOOR_TILES);
            TileRectangle(map, m_Game.GameTiles.WALL_POLICE_STATION, map.Rect);
            // - small cells.
            const int cellWidth = 3;
            const int cellHeight = 3;
            const int yCells = 3;
            List<Rectangle> cells = new List<Rectangle>();
            for (int x = 0; x + cellWidth <= map.Width; x += cellWidth - 1)
            {
                // room.
                Rectangle cellRoom = new Rectangle(x, yCells, cellWidth, cellHeight);
                cells.Add(cellRoom);
                TileFill(map, m_Game.GameTiles.FLOOR_CONCRETE, cellRoom);
                TileRectangle(map, m_Game.GameTiles.WALL_POLICE_STATION, cellRoom);

                // couch.
                Point couchPos = new Point(x + 1, yCells + 1);
                map.PlaceMapObjectAt(MakeObjIronBench(GameImages.OBJ_IRON_BENCH), couchPos);

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
            PlaceDoor(map, map.Width - 3, 1,m_Game.GameTiles.FLOOR_TILES, MakeObjIronDoor(DoorWindow.STATE_CLOSED)); //@@MP - added for funneling (Release 6-1)
            map.PlaceMapObjectAt(MakeObjBankTeller(GameImages.OBJ_BANK_TELLER), new Point(map.Width - 2, 1)); //@@MP - added for blocking (Release 6-1)
            map.PlaceMapObjectAt(MakeObjPowerGenerator(GameImages.OBJ_POWERGEN_OFF, GameImages.OBJ_POWERGEN_ON), new Point(map.Width - 2, 2)); //@@MP - moved generator 1 south (Release 6-1)

            // 3. Populate.
            // - prisoners in each cell.
            //   keep the last cell for the special prisonner.
            for (int i = 0; i < cells.Count - 1; i++)
            {
                Rectangle cell = cells[i];

                // jailed. Civilian.
                Actor prisonner = CreateNewCivilian(0, 0, 1);

                // make sure he is stripped of all default items!
                while (!prisonner.Inventory.IsEmpty)
                    prisonner.Inventory.RemoveAllQuantity(prisonner.Inventory[0]);

                // give him some food.
                prisonner.Inventory.AddAll(MakeItemGroceries());

                // drop him.
                map.PlaceActorAt(prisonner, new Point(cell.Left + 1, cell.Top + 1));
            }
            // - Special prisoner in the last cell.
            Rectangle lastCell = cells[cells.Count - 1];
            Actor specialPrisoner = CreateNewCivilian(0, 0, 1);
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
                map.PlaceMapObjectAt(MakeObjIronFence(GameImages.OBJ_IRON_FENCE), new Point(2, yFence));

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
            // enraged patient!
            ActorModel model = m_Game.GameActors.JasonMyers;
            Actor jason = model.CreateNamed(m_Game.GameFactions.ThePsychopaths, "Jason Myers", false, 0);
            jason.IsUnique = true;
            jason.Doll.AddDecoration(DollPart.SKIN, GameImages.ACTOR_JASON_MYERS);
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
            jason.Inventory.AddAll(MakeItemJasonMyersAxe());
            map.PlaceActorAt(jason, new Point(map.Width / 2, map.Height / 2));
            m_Game.Session.UniqueActors.JasonMyers = new UniqueActor()
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
            GiveRandomSkillsToActor(m_DiceRoller, nurse, 1);
            GiveStartingSkillToActor(nurse, Skills.IDs.MEDIC);

            // give items
            nurse.Inventory.AddAll(MakeItemBandages());
            //@@MP (Release 6-2)
            nurse.Inventory.AddAll(MakeItemBandages());
            nurse.Inventory.AddAll(MakeItemFlashlight());

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
            GiveRandomSkillsToActor(m_DiceRoller, doctor, 1);
            GiveStartingSkillToActor(doctor, Skills.IDs.MEDIC);
            GiveStartingSkillToActor(doctor, Skills.IDs.MEDIC);
            GiveStartingSkillToActor(doctor, Skills.IDs.MEDIC);
            GiveStartingSkillToActor(doctor, Skills.IDs.LEADERSHIP);

            // give items
            doctor.Inventory.AddAll(MakeItemMedikit());
            //@@MP (Release 6-2)
            doctor.Inventory.AddAll(MakeItemMedikit());
            doctor.Inventory.AddAll(MakeItemFlashlight());

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
                    case 1: it = MakeItemGroceries(); break;
                    case 2: it = MakeItemBook(); break;
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

        #region Army Base
        //@@MP (Release 6-3)
        public Map GenerateUniqueMap_ArmyBase(Map surfaceMap, Zone officeZone, out Point baseEntryPos)
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
            Map underground = new Map((surfaceMap.Seed << 3) ^ surfaceMap.Seed, "Army Base", RogueGame.MAP_MAX_WIDTH, RogueGame.MAP_MAX_HEIGHT)
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
            Zone roomZone = null;
            Point surfaceExit = new Point();
            while (true)    // loop until found.
            {
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

                // find somewhere walkable inside.
                bool foundSurfaceExit = false;
                int attempts = 0;
                do
                {
                    surfaceExit.X = m_DiceRoller.Roll(roomZone.Bounds.Left, roomZone.Bounds.Right);
                    surfaceExit.Y = m_DiceRoller.Roll(roomZone.Bounds.Top, roomZone.Bounds.Bottom);
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

            // barricade the rooms door.
            DoForEachTile(roomZone.Bounds, //@@MP - unused parameter (Release 5-7)
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
                    if (upID == (GameActors.IDs)undead.Model.ID)
                        break;
                    undead.Model = m_Game.GameActors[upID];
                }
                ActorPlace(m_DiceRoller, underground.Width * underground.Height, underground, undead, (pt) => underground.GetExitAt(pt) == null);
            }

            /*// CHAR Guards.
            int nbGuards = underground.Width / 10; // 10 for 100.
            for (int i = 0; i < nbGuards; i++)
            {
                Actor guard = CreateNewCHARGuard(0);
                ActorPlace(m_DiceRoller, underground.Width * underground.Height, underground, guard, (pt) => underground.GetExitAt(pt) == null);
            }*/
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
            // Shelves with weapons/ammo along walls.
            MapObjectFill(map, roomRect,
                (pt) =>
                {
                    if (CountAdjWalls(map, pt.X, pt.Y) < 3)
                        return null;
                    // dont block exits!
                    if (map.GetExitAt(pt) != null)
                        return null;

                    // table + tracker/armor/weapon.
                    Item it;
                    int randomItem = m_DiceRoller.Roll(0, 19);
                    switch (randomItem)
                    {
                        case 0: it = MakeItemArmyRifle(); break;
                        case 1:
                        case 2:
                        case 3: it = MakeItemHeavyRifleAmmo(); break;
                        case 4: it = MakeItemArmyPistol(); break;
                        case 5:
                        case 6:
                        case 7: it = MakeItemHeavyPistolAmmo(); break;
                        case 8: it = MakeItemShotgun(); break;
                        case 9:
                        case 10:
                        case 11: it = MakeItemShotgunAmmo(); break;
                        case 12: it = MakeItemGrenade(); break;
                        case 13:
                        case 14: it = MakeItemArmyBodyArmor(); break;
                        case 15: it = MakeItemPrecisionRifle(); break;
                        case 16: it = MakeItemBlackOpsGPS(); break;
                        case 17: it = MakeItemNightVisionGoggles(); break;
                        case 18: it = MakeItemC4Explosive(); break;
                        default:
                            throw new InvalidOperationException("unhandled roll");
                    }

                    map.DropItemAt(it, pt);

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
                        return m_DiceRoller.RollChance(50) ? MakeObjJunk(GameImages.OBJ_JUNK) : MakeObjBarrels(GameImages.OBJ_BARRELS);
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
                            return MakeObjArmyComputerStation(GameImages.OBJ_ARMY_COMPUTER_STATION);
                        else
                            return MakeObjArmyRadioCupboard(GameImages.OBJ_ARMY_RADIO_CUPBOARD);
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
                            return MakeObjArmyComputerStation(GameImages.OBJ_ARMY_COMPUTER_STATION);
                        else
                            return MakeObjCHARdesktop(GameImages.OBJ_ARMY_RADIO_CUPBOARD);
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
                    if (CountAdjWalls(map, pt.X, pt.Y) < 3)
                        return null;
                    // dont block exits!
                    if (map.GetExitAt(pt) != null)
                        return null;

                    // table + meds.
                    Item it = MakeHospitalItem();
                    map.DropItemAt(it, pt);

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
                    case 4: it = RogueGame.Options.VTGAntiviralPills ? MakeItemPillsAntiviral() : MakeItemDynamite(); //@MP - handles new antiviral pills option (Release 5-2)
                        break;
                    case 5: it = MakeItemCombatKnife(); break;
                    default: it = null; break;
                }
            }
            else
            {
                // standard item.
                int roll = roller.Roll(0, 10);
                switch (roll)
                {
                    case 0: it = MakeRandomShopItem(ShopType.CONSTRUCTION); break;
                    case 1: it = MakeRandomShopItem(ShopType.GENERAL_STORE); break;
                    case 2: it = MakeRandomShopItem(ShopType.GROCERY); break;
                    case 3: it = MakeRandomShopItem(ShopType.GUNSHOP); break;
                    case 4: it = MakeRandomShopItem(ShopType.PHARMACY); break;
                    case 5: it = MakeRandomShopItem(ShopType.SPORTSWEAR); break;
                    case 6: it = MakeRandomShopItem(ShopType.HUNTING); break;
                    case 7: it = MakeRandomParkItem(); break;
                    case 8: it = MakeRandomBedroomItem(); break;
                    case 9: it = MakeRandomKitchenItem(); break;
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
                // add random items.
                for(int i = 0; i < itemsToCarry && newRefugee.Inventory.CountItems < newRefugee.Inventory.MaxCapacity; i++)
                    GiveRandomItemToActor(m_DiceRoller, newRefugee, spawnTime);
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

            // give items, good survival gear (7 items).
            #region
            // 1,2   1 can of food, 1 amr.
            survivor.Inventory.AddAll(MakeItemCannedFood());
            survivor.Inventory.AddAll(MakeItemArmyRation());
            // 3,4. 1 fire weapon with 1 ammo box or grenade.
            if (m_DiceRoller.RollChance(50))
            {
                survivor.Inventory.AddAll(MakeItemArmyRifle());
                if (m_DiceRoller.RollChance(50))
                    survivor.Inventory.AddAll(MakeItemHeavyRifleAmmo());
                else
                    survivor.Inventory.AddAll(MakeItemGrenade());
            }
            else
            {
                survivor.Inventory.AddAll(MakeItemShotgun());
                if (m_DiceRoller.RollChance(50))
                    survivor.Inventory.AddAll(MakeItemShotgunAmmo());
                else
                    survivor.Inventory.AddAll(MakeItemGrenade());
            }
            // 5    1 healing item.
            survivor.Inventory.AddAll(MakeItemMedikit());

            // 6    1 pill item.
            switch (m_DiceRoller.Roll(0, 3))
            {
                case 0: survivor.Inventory.AddAll(MakeItemPillsSLP()); break;
                case 1: survivor.Inventory.AddAll(MakeItemPillsSTA()); break;
                case 2: survivor.Inventory.AddAll(MakeItemPillsSAN()); break;
            }
            // 7    1 armor.
            survivor.Inventory.AddAll(MakeItemArmyBodyArmor());
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
            //newCop.Controller = new CivilianAI(); // alpha10.1 defined by model like other actors

            // give items.
            if (m_DiceRoller.RollChance(50))
            {
                // pistol
                newCop.Inventory.AddAll(MakeItemPistol());
                newCop.Inventory.AddAll(MakeItemLightPistolAmmo());
            }
            else
            {
                // shoty
                newCop.Inventory.AddAll(MakeItemShotgun());
                newCop.Inventory.AddAll(MakeItemShotgunAmmo());
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

            // done.
            return newCop;
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

            // give items.
            newGuard.Inventory.AddAll(MakeItemShotgun());
            newGuard.Inventory.AddAll(MakeItemShotgunAmmo());
            newGuard.Inventory.AddAll(MakeItemCHARLightBodyArmor());

            // done.
            return newGuard;
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

            // give items 6/7.
            newNat.Inventory.AddAll(MakeItemArmyRifle());
            newNat.Inventory.AddAll(MakeItemHeavyRifleAmmo());
            newNat.Inventory.AddAll(MakeItemArmyPistol());
            newNat.Inventory.AddAll(MakeItemHeavyPistolAmmo());
            newNat.Inventory.AddAll(MakeItemArmyBodyArmor());
            ItemBarricadeMaterial planks = MakeItemWoodenPlank();
            planks.Quantity = m_Game.GameItems.WOODENPLANK.StackingLimit;
            newNat.Inventory.AddAll(planks);

            // starting skills
            GiveStartingSkillToActor(newNat, Skills.IDs.CARPENTRY); //carpentry for building small barricades.
            GiveStartingSkillToActor(newNat, Skills.IDs.FIREARMS); //alpha 10

            // give skills : 1 per day after min arrival date.
            int nbSkills = new WorldTime(spawnTime).Day - RogueGame.NATGUARD_DAY;
            if (nbSkills > 0)
                base.GiveRandomSkillsToActor(m_DiceRoller, newNat, nbSkills);
    
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
            newBiker.Inventory.AddAll(m_DiceRoller.RollChance(50) ? MakeItemCrowbar() : MakeItemBaseballBat());
            newBiker.Inventory.AddAll(MakeItemBikerGangJacket(gangId));

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
            newGangsta.Inventory.AddAll(m_DiceRoller.RollChance(50) ? MakeItemRandomPistol() : MakeItemBaseballBat());


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

            // give items.
            newBO.Inventory.AddAll(MakeItemPrecisionRifle());
            newBO.Inventory.AddAll(MakeItemHeavyRifleAmmo());
            newBO.Inventory.AddAll(MakeItemArmyPistol());
            newBO.Inventory.AddAll(MakeItemHeavyPistolAmmo());
            newBO.Inventory.AddAll(MakeItemBlackOpsGPS());

            // done.
            return newBO;
        }

        public Actor CreateNewFeralDog(int spawnTime)
        {
            Actor newDog;

            // model
            newDog = m_Game.GameActors.FeralDog.CreateNumberedName(m_Game.GameFactions.TheFerals, spawnTime);

            // skin
            SkinDog(m_DiceRoller, newDog);

            // done.
            return newDog;
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
            map.AddZone(MakeUniqueZone("walkway", new Rectangle(r.Left, r.Top, r.Width - 1, 1)));
            // S
            map.AddZone(MakeUniqueZone("walkway", new Rectangle(r.Left + 1, r.Bottom - 1, r.Width - 1, 1)));
            // E
            map.AddZone(MakeUniqueZone("walkway", new Rectangle(r.Right - 1, r.Top, 1, r.Height - 1)));
            // W
            map.AddZone(MakeUniqueZone("walkway", new Rectangle(r.Left, r.Top + 1, 1, r.Height - 1)));
        }
        #endregion
    }
}
