using System;
using System.Collections.Generic;
using System.Drawing;

using djack.RogueSurvivor.Data;
using djack.RogueSurvivor.Engine.Items;

namespace djack.RogueSurvivor.Engine
{
    /// <summary>
    /// Line Of Sight & Field Of View computing, line tracing and assorted utilities.
    /// </summary>
    static class LOS
    {
        #region Line tracing

#if false
        /// <summary>
        /// Ensure symetric results by ordering coordinates.
        /// </summary>
        /// <param name="maxSteps"></param>
        /// <param name="map"></param>
        /// <param name="xFrom"></param>
        /// <param name="yFrom"></param>
        /// <param name="xTo"></param>
        /// <param name="yTo"></param>
        /// <param name="line"></param>
        /// <param name="fn"></param>
        /// <returns></returns>
        public static bool SymetricBresenhamTrace(int maxSteps, Map map, int xFrom, int yFrom, int xTo, int yTo, List<Point> line, Func<int, int, bool> fn)
        {         
            ///////////////////////////////////////////////
            // Ensure symetry by arbitrary ordering points
            ///////////////////////////////////////////////
            if (xFrom + yFrom > xTo + yTo)
            {
                // swap from and to
                int swap;

                swap = xFrom;
                xFrom = xTo;
                xTo = swap;

                swap = yFrom;
                yFrom = yTo;
                yTo = swap;
            }

            /////////////////////
            // Then do bresenham
            /////////////////////
            return AsymetricBresenhamTrace(maxSteps, map, xFrom, yFrom, xTo, yTo, line, fn);
        }
#endif

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxSteps"></param>
        /// <param name="map"></param>
        /// <param name="xFrom"></param>
        /// <param name="yFrom"></param>
        /// <param name="xTo"></param>
        /// <param name="yTo"></param>
        /// <param name="line">if not null, the method adds the points forming the segment, source position included.</param>
        /// <param name="fn"></param>
        /// <returns></returns>
        public static bool AsymetricBresenhamTrace(int maxSteps, int xFrom, int yFrom, int xTo, int yTo, List<Point> line, Func<int, int, bool> fn) //@@MP - unused parameter (Release 5-7)
        {
            // From Roguebasin
            // http://roguebasin.roguelikedevelopment.org/index.php?title=Bresenham%27s_Line_Algorithm

            int delta_x = Math.Abs(xTo - xFrom) << 1;
            int delta_y = Math.Abs(yTo - yFrom) << 1;

            // if xFrom == xTo or yFrom == yTo, then it does not matter what we set here
            int ix = xTo > xFrom ? 1 : -1;
            int iy = yTo > yFrom ? 1 : -1;

            // plot
            if (line != null)
                line.Add(new Point(xFrom, yFrom));

            int stepCount = 0;

            if (delta_x >= delta_y)
            {
                // error may go below zero
                int error = delta_y - (delta_x >> 1);

                while (xFrom != xTo)
                {
                    if (error >= 0)
                    {
                        if (error != 0 || ix > 0)
                        {
                            yFrom += iy;
                            error -= delta_x;
                        }
                        // else do nothing
                    }
                    // else do nothing

                    xFrom += ix;
                    error += delta_y;

                    if (++stepCount > maxSteps)
                        return false;

                    // plot
                    if (!fn(xFrom, yFrom))
                        return false;
                    if (line != null)
                        line.Add(new Point(xFrom, yFrom));
                }
            }
            else
            {
                // error may go below zero
                int error = delta_x - (delta_y >> 1);

                while (yFrom != yTo)
                {
                    if (error >= 0)
                    {
                        if (error != 0 || iy > 0)
                        {
                            xFrom += ix;
                            error -= delta_y;
                        }
                        // else do nothing
                    }
                    // else do nothing

                    yFrom += iy;
                    error += delta_x;

                    if (++stepCount > maxSteps)
                        return false;

                    // plot
                    if (!fn(xFrom, yFrom))
                        return false;
                    if (line != null)
                        line.Add(new Point(xFrom, yFrom));
                }
            }

            // all clear.
            return true;
        }

#if false
        public static bool SymetricBresenhamTrace(Map map, int xFrom, int yFrom, int xTo, int yTo, List<Point> line, Func<int, int, bool> fn)
        {
            return SymetricBresenhamTrace(Int32.MaxValue, map, xFrom, yFrom, xTo, yTo, line, fn);
        }
#endif

        public static bool AsymetricBresenhamTrace(int xFrom, int yFrom, int xTo, int yTo, List<Point> line, Func<int, int, bool> fn) //@@MP - unused parameter (Release 5-7)
        {
            return AsymetricBresenhamTrace(Int32.MaxValue, xFrom, yFrom, xTo, yTo, line, fn);
        }

        public static Direction DirectionTo(int xFrom, int yFrom, int xTo, int yTo) //@@MP - unused parameter (Release 5-7)
        {
            List<Point> line = new List<Point>();
            AsymetricBresenhamTrace(1, xFrom, yFrom, xTo, yTo, line,
                (x, y) => true);

            return Direction.FromVector(line[0]);
        }

        public static bool CanTraceViewLine(Location fromLocation, Point toPosition, int maxRange) //@@MP - unused parameter (Release 5-7)
        {
            Map map = fromLocation.Map;
            Point goal = toPosition;

            return AsymetricBresenhamTrace(maxRange,
                fromLocation.Position.X, fromLocation.Position.Y,
                toPosition.X, toPosition.Y,
                null,
                (x, y) =>
                {
                    if (map.IsTransparent(x, y)) return true;
                    if (x == goal.X && y == goal.Y) return true;
                    return false;
                });
        }

        public static bool CanTraceViewLine(Location fromLocation, Point toPosition)
        {
            return CanTraceViewLine(fromLocation, toPosition, Int32.MaxValue);
        }

        /// <summary>
        /// Checks if can fire from a position to another.
        /// </summary>
        /// <param name="fromLocation"></param>
        /// <param name="toPosition"></param>
        /// <param name="maxRange"></param>
        /// <param name="line">if not null will contains the entire fire line, even if obstructed.</param>
        /// <returns>true if fire line is clear.</returns>
        public static bool CanTraceFireLine(Location fromLocation, Point toPosition, int maxRange, List<Point> line) //@@MP - unused parameter (Release 5-7)
        {
            Map map = fromLocation.Map;
            Point start = fromLocation.Position;
            Point goal = toPosition;
            bool fireLineClear = true;

            AsymetricBresenhamTrace(maxRange,
                fromLocation.Position.X, fromLocation.Position.Y,
                toPosition.X, toPosition.Y,
                line,
                (x, y) =>
                {
                    if (x == start.X && y == start.Y) return true;
                    if (x == goal.X && y == goal.Y) return true;
                    if (map.IsBlockingFire(x, y)) fireLineClear = false;
                    return true;
                });

            return fireLineClear;
        }

        public static bool CanTraceThrowLine(Location fromLocation, Point toPosition, int maxRange, List<Point> line) //@@MP - unused parameter (Release 5-7)
        {
            Map map = fromLocation.Map;
            Point start = fromLocation.Position;
            Point goal = toPosition;
            bool throwLineClear = true;

            // check line.
            AsymetricBresenhamTrace(maxRange,
                fromLocation.Position.X, fromLocation.Position.Y,
                toPosition.X, toPosition.Y,
                line,
                (x, y) =>
                {
                    if (x == start.X && y == start.Y) return true;
                    if (x == goal.X && y == goal.Y) return true;
                    if (map.IsBlockingThrow(x, y)) throwLineClear = false;
                    return true;
                });

            // we can't throw on something blocking, no matter the rest of the line is clear.
            if (map.IsBlockingThrow(toPosition.X, toPosition.Y))
                throwLineClear = false;

            // done.
            return throwLineClear;
        }
        #endregion

        #region Computing FOV
        static bool FOVSub(Location fromLocation, Point toPosition, int maxRange, ref HashSet<Point> visibleSet) //@@MP - unused parameter (Release 5-7)
        {
#if false
            return CanTraceViewLine(fromLocation, toPosition, maxRange);
#endif

            // Asymetric bresenham : use the fact we are tracing FROM to TO to add visible tiles on the fly.
            // Pros: fixed "holes" in fov : if you can see a tile you can see everything in its line too.
            // Cons: rare cases of asymetry in FOV : i can see you, but you can't see me.
            Map map = fromLocation.Map;
            HashSet<Point> visibleSetRef = visibleSet;  // necessary to have a local variable in lambda call.
            Point goal = toPosition;

            return AsymetricBresenhamTrace(maxRange,
                fromLocation.Position.X, fromLocation.Position.Y,
                toPosition.X, toPosition.Y,
                null,
                (x, y) =>
                {
                    bool viewThrough =
                        (x == goal.X && y == goal.Y) ? true :
                        map.IsTransparent(x, y) ? true :
                        false;

                    if (viewThrough)
                        visibleSetRef.Add(new Point(x, y));

                    return viewThrough;
                });
        }

        //@@MP - added that other tiles light up by other light sources outside FoV (Release 6-5), switched Rules for Game (Release 6-6)
        //@@MP - added maxItemLightTintRange for lights that cast a tint around the player (Release 7-1)
        public static HashSet<Point> ComputeFOVFor(RogueGame game, Actor actor, WorldTime time, Weather weather, bool checkForOtherLitTiles)//, int maxItemLightTintRange = 0)
        {
            Location fromLocation = actor.Location;
            HashSet<Point> visibleSet = new HashSet<Point>();
            Point from = fromLocation.Position;
            Map map = fromLocation.Map;
            int maxRange = game.Rules.ActorFOV(actor, time, weather);

            //////////////////////////////////////////////
            // Brute force ray-casting with wall fix pass
            //////////////////////////////////////////////
            int xmin = from.X - maxRange;
            int xmax = from.X + maxRange;
            int ymin = from.Y - maxRange;
            int ymax = from.Y + maxRange;
            map.TrimToBounds(ref xmin, ref ymin);
            map.TrimToBounds(ref xmax, ref ymax);
            Point to = new Point();
            List<Point> wallsToFix = new List<Point>();

            //reset tile tints   //@@MP (Release 7-5)
            RemoveTintFromAllTilesOnMap(map);

            // 1st pass : trace line and remember walls that are not visible for 2nd pass.
            for (int x = xmin; x <= xmax; x++)
            {
                to.X = x;
                for (int y = ymin; y <= ymax; y++)
                {
                    to.Y = y;

                    // If we already know tile is visible, pass. //@@MP - moved to top (Release 6-5)
                    if (visibleSet.Contains(to))
                        continue;

                    // Distance check.
                    Point pos = new Point(to.X, to.Y);
                    bool isAdjacent = game.Rules.IsAdjacent(actor.Location.Position, pos);
                    if (!isAdjacent) //@@MP - the circle below cuts out the 'corners'. if maxRange = 0 then the actor can't even see adjacent diagonals (only directly N,S,E,W)
                    {
                        if (game.Rules.LOSDistance(from, to) > maxRange) //@@MP - aims to keep the view range circular: exclude everything outside the circle's range
                            continue;
                    }

                    // all immediately adjacent tiles automatically visible when FOV > 0, even if they don't fit in the circle determined above
                    if (isAdjacent && maxRange > 0)
                    {
                        visibleSet.Add(to);
                        continue;
                    }

                    // Trace line.
                    if (!FOVSub(fromLocation, to, maxRange, ref visibleSet))
                    {
                        // if its a wall (in FoV terms), remember.
                        bool isFovWall = false;
                        Tile tile = map.GetTileAt(x, y);
                        MapObject mapObj = map.GetMapObjectAt(x, y);
                        //Logger.WriteLine(Logger.Stage.RUN_MAIN, mapObj.Location.Position.ToString() + ". objMatTransparent= " + mapObj.IsMaterialTransparent + ". tileTransparent= " + tile.Model.IsTransparent.ToString() + ". mapTransparent= " + map.IsTransparent(x,y).ToString());
                        if (!tile.Model.IsWalkable && !tile.Model.IsTransparent) //@@MP - swapped the order of the two conditions, as currently all tile models are transparent (Release 6-5)
                            isFovWall = true;
                        else if (mapObj != null && !mapObj.IsMaterialTransparent && !mapObj.IsTransparent) //@@MP - added transparency check (Release 6-5), fixed (Release 7-5)
                            isFovWall = true;

                        if (isFovWall)
                            wallsToFix.Add(to);

                        // next.
                        continue;
                    }

                    // Visible.
                    visibleSet.Add(to);
                }
            }

            // 2nd pass : wall fix.
            List<Point> fixedWalls = new List<Point>(wallsToFix.Count);
            foreach (Point wallP in wallsToFix)
            {
                int count = 0;
                foreach (Direction d in Direction.COMPASS)
                {
                    Point next = wallP + d;
                    if (visibleSet.Contains(next))
                    {
                        Tile tile = map.GetTileAt(next.X, next.Y);
                        if (tile.Model.IsTransparent && tile.Model.IsWalkable)
                            ++count;
                    }
                }
                if (count >= 3)
                    fixedWalls.Add(wallP);
            }
            foreach (Point fixedWall in fixedWalls)
            {
                visibleSet.Add(fixedWall);
            }

            // Other light sources, like fires or actors with torches, outside the regular player FoV //@@MP (Release 6-5)
            if (checkForOtherLitTiles && actor.IsPlayer) //@@MP - filtered to player only, otherwise the game runs like a slideshow (Release 7-1)
            {
                /*****************************************
                 * * DID NOT OVERLAP SOME GFX (eg water cover). LEFT IN FOR FUTURE CONSIDERATION
                // get the player's light (if any) //@@MP (Release 7-1)
                string tintImageOnPlayer = null;
                ItemLight playerHeldLight = (actor.GetEquippedItem(DollPart.LEFT_HAND) as ItemLight);
                if (playerHeldLight != null && playerHeldLight.Batteries > 0)
                {
                    tintImageOnPlayer = GetTintForItemLight(game, playerHeldLight);
                }
                
                // ItemLights have a maximum range for which they cast their light, which is often less than the total FOV of the actor.
                // We do this because otherwise the range of the tint could be crazy, because lights add to the existing FOV rather than set the FOV.
                int tintRangeOnPlayer = Math.Min(game.Rules.ActorFOV(actor, time, weather), maxItemLightTintRange);  //@@MP (Release 7-1)
                *****************************************/

                // Now check each point on the map
                for (int x = 0; x < map.Width; x++)
                {
                    for (int y = 0; y < map.Height; y++)
                    {
                        Point spot = new Point(x, y);

                        /*****************************************
                         * DID NOT OVERLAP SOME GFX (eg water cover). LEFT IN FOR FUTURE CONSIDERATION
                        //add a tint for it (as required)  //@@MP (Release 7-1)
                        if (game.Rules.GridDistance(actor.Location.Position, spot) <= tintRangeOnPlayer && tintImageOnPlayer != null)
                            AddTintToTile(map, spot, tintImageOnPlayer);
                        *****************************************/

                        if (spot != actor.Location.Position)
                        {
                            //only consider spots that we actually have line of sight to
                            HashSet<Point> spotSet = new HashSet<Point>();
                            spotSet.Add(spot);
                            if (!FOVSub(fromLocation, spot, 10, ref spotSet))
                                continue; //this spot is not in LOS, skip it
                        }

                        //MapObjects: car fires
                        MapObject mapObj = map.GetMapObjectAt(spot);
                        if (mapObj != null && mapObj.IsOnFire)
                        {
                            visibleSet.Add(spot);

                            foreach (Direction d in Direction.COMPASS)
                            {
                                //lights up the tiles around it
                                Point next = spot + d;
                                if (map.IsInBounds(next))
                                {
                                    visibleSet.Add(next);
                                }
                            }
                            continue; //already lit up this tile and adjacents, no need to chek for other light sources
                        }
                        //tile fires - check these first, as it may also be in adjacent tiles
                        if (map.IsAnyTileFireThere(map, spot))
                        {
                            TileModel tileModel = map.GetTileAt(spot).Model; //@@MP - not on walls because it makes them transparent (Release 6-6)
                            if (!game.GameTiles.IsWallModel(tileModel))
                            {
                                visibleSet.Add(spot);

                                foreach (Direction d in Direction.COMPASS)
                                {
                                    //lights up the tiles around it
                                    Point next = spot + d;
                                    if (map.IsInBounds(next))
                                    {
                                        visibleSet.Add(next);
                                    }
                                }
                                continue;
                            }
                        }
                        //Actors: carrying ItemLights eg torches
                        if (map.GetActorAt(spot) != null)
                        {
                            Actor act = map.GetActorAt(spot);
                            ItemLight heldLight = (act.GetEquippedItem(DollPart.LEFT_HAND) as ItemLight);
                            if (heldLight != null && heldLight.Batteries > 0)
                            {
                                //string tintImage = GetTintForItemLight(game, heldLight);
                                //AddTintToTile(map, spot, tintImage); //DID NOT OVERLAP SOME GFX (eg water cover). LEFT IN FOR FUTURE CONSIDERATION

                                visibleSet.Add(spot);

                                foreach (Direction d in Direction.COMPASS)
                                {
                                    //lights up the tiles around it
                                    Point next = spot + d;
                                    if (map.IsInBounds(next))
                                    {
                                        visibleSet.Add(next);
                                        //AddTintToTile(map, next, tintImage); //DID NOT OVERLAP SOME GFX (eg water cover). LEFT IN FOR FUTURE CONSIDERATION
                                    }
                                }
                                continue;
                            }
                        }
                        //Tile Decorations eg lit candles //@MP (Release 7-1)
                        if (map.GetTileAt(spot).HasDecoration(Gameplay.GameImages.DECO_LIT_CANDLE))
                        {
                            visibleSet.Add(spot);

                            foreach (Direction d in Direction.COMPASS)
                            {
                                //lights up the tiles around it
                                Point next = spot + d;
                                if (map.IsInBounds(next))
                                {
                                    visibleSet.Add(next);
                                }
                            }
                            continue;
                        }
                        //ground inventory items  //@MP (Release 7-1)
                        Inventory inv = map.GetItemsAt(spot);
                        if (inv != null)
                        {
                            foreach (Item item in inv.Items)
                            {
                                ItemLight light = item as ItemLight;
                                if (light != null)
                                {
                                    //we only want to check flares and candles (throwables), as torches should be off if not euqipped by an actor   //@@MP (Release 7-5)
                                    if (light.Model.IsThrowable)
                                    {
                                        visibleSet.Add(spot);
                                        //string tintImage = GetTintForItemLight(game, light);
                                        //AddTintToTile(map, spot, tintImage); //DID NOT OVERLAP SOME GFX (eg water cover). LEFT IN FOR FUTURE CONSIDERATION

                                        foreach (Direction d in Direction.COMPASS)
                                        {
                                            //lights up the tiles around it
                                            Point next = spot + d;
                                            if (map.IsInBounds(next))
                                            {
                                                visibleSet.Add(next);
                                                //AddTintToTile(map, next, tintImage); //DID NOT OVERLAP SOME GFX (eg water cover). LEFT IN FOR FUTURE CONSIDERATION
                                            }
                                        }
                                        //break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return visibleSet;
        }

        static string GetTintForItemLight(RogueGame game, Item light) //@@MP (Release 7-1)
        {
            //unused, as the effect wasn't quite right in practice. left in as it may be revisited

            if (light.Model == game.GameItems.LIGHT_FLARE)
                return Gameplay.GameImages.EFFECT_LIGHT_TINT_FLARE;
            else if (light.Model == game.GameItems.LIGHT_GLOWSTICK)
                return Gameplay.GameImages.EFFECT_LIGHT_TINT_GLOWSTICK;
            else
                return null; //it's not an item that would provide light whilst on the ground
        }

        static void AddTintToTile(Map map, Point pt, string tintImage) //@@MP (Release 7-1)
        {
            //unused, as the effect wasn't quite right in practice. left in as it may be revisited

            if (tintImage != null && !map.GetTileAt(pt).HasDecoration(tintImage))
                map.GetTileAt(pt).AddDecoration(tintImage); //add tint
        }

        static void RemoveTintFromAllTilesOnMap(Map map)  //@@MP (Release 7-1)(Release 7-5)
        {
            //unused, as the effect wasn't quite right in practice. left in as it may be revisited

            //remove tint from tiles where there is no longer that light source
            for (int a = 0; a < map.Width; a++)
            {
                for (int b = 0; b < map.Height; b++)
                {
                    Point spot = new Point(a, b);
                    string tintImage;
                    if (map.GetTileAt(spot).HasDecoration(Gameplay.GameImages.EFFECT_LIGHT_TINT_FLARE))
                        tintImage = Gameplay.GameImages.EFFECT_LIGHT_TINT_FLARE;
                    else if (map.GetTileAt(spot).HasDecoration(Gameplay.GameImages.EFFECT_LIGHT_TINT_GLOWSTICK))
                        tintImage = Gameplay.GameImages.EFFECT_LIGHT_TINT_GLOWSTICK;
                    else
                        continue;

                    map.GetTileAt(spot).RemoveDecoration(tintImage);
                }
            }
        }
#endregion
    }
}
