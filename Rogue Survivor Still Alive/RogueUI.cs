using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using djack.RogueSurvivor.Data;
using djack.RogueSurvivor.Engine;
using djack.RogueSurvivor.Gameplay;
using SFML;
using KeyEventArgs = SFML.Window.KeyEventArgs;
using Keys = SFML.Window.Keyboard.Key;
using SFML.Graphics;
using SFML.System;
using Color = System.Drawing.Color;
using MouseButtons = SFML.Window.Mouse.Button;

namespace djack.RogueSurvivor
{
    public partial class RogueUI
    {
        private void DoEvents()
        {
            if (renderWindow.IsOpen)
            {
                renderWindow.DispatchEvents();
                
                var view = new SFML.Graphics.View((Vector2f)canvas.Texture.Size / 2.0f, (Vector2f)canvas.Texture.Size);

                // a scale that preserves aspect ratio and pillarboxes/letterboxes the canvas on the screen
                float scale = Math.Min((float)renderWindow.Size.X / canvas.Texture.Size.X, (float)renderWindow.Size.Y / canvas.Texture.Size.Y);
                float w = canvas.Texture.Size.X * scale;
                float h = canvas.Texture.Size.Y * scale;
                float wf = w / renderWindow.Size.X;
                float hf = h / renderWindow.Size.Y;
                view.Viewport = new FloatRect((1.0f - wf) / 2.0f, (1.0f - hf) / 2.0f, wf, hf);

                renderWindow.SetView(view);

                renderWindow.Clear(SFML.Graphics.Color.Black);            

                var spr = new Sprite(canvas.Texture);
                spr.Position = (Vector2f)canvas.Texture.Size / 2.0f;
                spr.Origin = (Vector2f)canvas.Texture.Size / 2.0f;
                spr.Scale = new Vector2f(1, -1);
                renderWindow.Draw(spr);
                renderWindow.Display();
            } 
        }

        #region Fields
        RogueGame m_Game;
        #endregion

        #region Properties
        internal RogueGame Game
        {
            get { return m_Game; }
        }
        #endregion

        SFML.Graphics.RenderWindow renderWindow;
        RenderTexture canvas;
        List<Drawable> drawables = new List<Drawable>();
        SFML.Graphics.Font sfmlFont;
        SFML.Graphics.Font sfmlBoldFont;
        RenderTexture minimap;
        List<Drawable> minimapDrawables = new List<Drawable>();
        static Shader grayLevelShader;
        Point mouseLocation = new Point(0,0);
        // tweak per font for best results
        const int FONT_SIZE = 16;
        const int FONT_Y_ADJUST = -5;
        const int CHAR_WIDTH = 8;
        const int CHAR_HEIGHT = 14;

        class GrayLevelSprite : Drawable
        {
            Sprite spr;
            float grayLevel;

            public GrayLevelSprite(Sprite spr, float grayLevel)
            {
                this.spr = spr;
                this.grayLevel = grayLevel;
            }

            public void Draw(RenderTarget target, RenderStates states)
            {
                var st = new RenderStates(states);
                st.Shader = grayLevelShader;
                st.Shader.SetUniform("grayLevel", grayLevel);
                spr.Draw(target, st);
            }
        }

        #region Init
        public RogueUI()
        {
            Logger.WriteLine(Logger.Stage.INIT_MAIN, "creating sfml window...");
            if (SetupConfig.Window == SetupConfig.eWindow.WINDOW_FULLSCREEN) {
                renderWindow = new SFML.Graphics.RenderWindow(SFML.Window.VideoMode.DesktopMode, "Rogue Survivor: " + SetupConfig.GAME_VERSION, SFML.Window.Styles.None);
            } else {
                renderWindow = new SFML.Graphics.RenderWindow(new SFML.Window.VideoMode(1024, 768), "Rogue Survivor: " + SetupConfig.GAME_VERSION);
            }
            renderWindow.SetVerticalSyncEnabled(true);
            var icon = new SFML.Graphics.Image("./Resources/Images/icon.png");
            renderWindow.SetIcon(icon.Size.X, icon.Size.Y, icon.Pixels);
            renderWindow.KeyPressed += (sender, e) => {
                UI_PostKey(e);
            };
            renderWindow.Closed += (sender, e) => {
                UI_DoQuit();
            };
            renderWindow.MouseMoved += (sender, e) => {
                mouseLocation.X = e.X;
                mouseLocation.Y = e.Y;
            };
            renderWindow.MouseButtonPressed += (sender, e) => {
                UI_PostMouseButtons(e.Button);
            };
            renderWindow.Resized += (sender, e) => {
                var size = renderWindow.Size;
                size.X = Math.Max(size.X, RogueGame.CANVAS_WIDTH);
                size.Y = Math.Max(size.Y, RogueGame.CANVAS_HEIGHT);
                renderWindow.Size = size;
            };

            canvas = new RenderTexture(RogueGame.CANVAS_WIDTH, RogueGame.CANVAS_HEIGHT);
            canvas.Texture.Smooth = true;

            grayLevelShader = Shader.FromString(
                @"
                void main()
                {
                    // transform the vertex position
                    gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;

                    // transform the texture coordinates
                    gl_TexCoord[0] = gl_TextureMatrix[0] * gl_MultiTexCoord0;

                    // forward the vertex color
                    gl_FrontColor = gl_Color;
                }
                ",
                null,
                @"
                uniform sampler2D texture;
                uniform float grayLevel;

                void main()
                {
                    // lookup the pixel in the texture
                    vec4 pixel = texture2D(texture, gl_TexCoord[0].xy);

                    // multiply it by the color
                    gl_FragColor = gl_Color * pixel;

                    float maxrgb = max(gl_FragColor.r, max(gl_FragColor.g, gl_FragColor.b));
                    float minrgb = min(gl_FragColor.r, min(gl_FragColor.g, gl_FragColor.b));
                    float brightness = (maxrgb + minrgb) / 2.0f;
                    //float brightness = maxrgb;
                    gl_FragColor.rgb = vec3(grayLevel * brightness);
                }
                "
            );

            sfmlFont = new SFML.Graphics.Font("./Resources/Fonts/regular.ttf");
            sfmlBoldFont = new SFML.Graphics.Font("./Resources/Fonts/bold.ttf");
            minimap = new RenderTexture(RogueGame.MINITILE_SIZE * RogueGame.MAP_MAX_WIDTH, RogueGame.MINITILE_SIZE * RogueGame.MAP_MAX_HEIGHT);;

            Logger.WriteLine(Logger.Stage.INIT_MAIN, "create RogueGame...");
            m_Game = new RogueGame(this);
        }

        void LoadResources()
        {
            Logger.WriteLine(Logger.Stage.INIT_GFX, "loading images...");
            GameImages.LoadResources(this);
            Logger.WriteLine(Logger.Stage.INIT_GFX, "loading images done");
        }
        #endregion

        public void Run() 
        {
            LoadResources();
            m_Game.Run();
        }

        #region RogueUI implementation
        
        #region Input
        bool m_HasKey = false;
        KeyEventArgs m_InKey;

        public KeyEventArgs UI_WaitKey()
        {
            m_HasKey = false;
            while (true)
            {
                DoEvents();
                if (m_HasKey)
                    break;
            }
            return m_InKey;
        }

        public KeyEventArgs UI_PeekKey()
        {
            DoEvents();
            if (m_HasKey)
            {
                m_HasKey = false;
                return m_InKey;
            }
            else
                return null;
        }

        public void UI_PostKey(KeyEventArgs e)
        {
            if (e == null) //@@MP (Release 5-7)
                throw new ArgumentNullException("e", "null e");

            // ignore Shift/Ctrl/Alt alone.
            switch (e.Code)
            {
                case Keys.LShift:
                case Keys.RShift:
                case Keys.RControl:
                case Keys.LControl:
                case Keys.LAlt:
                case Keys.RAlt:
                    return;
                default: 
                    break;
            }

            m_HasKey = true;
            m_InKey = e;

            /////////////////////
            // Cheats / Dev Tools
            /////////////////////
#if DEBUG
            // F6 - CHEAT - reveal all. //@@MP - and refill status (basically god mode)
            if (e.Code == Keys.F6)
            {
                if (m_Game.Session != null && m_Game.Session.CurrentMap != null)
                {
                    m_Game.Session.CurrentMap.SetAllAsVisited();
                    UI_Repaint();
                }
                m_Game.Player.HitPoints = m_Game.Rules.ActorMaxHPs(m_Game.Player);
                m_Game.Player.StaminaPoints = m_Game.Rules.ActorMaxSTA(m_Game.Player);
                m_Game.Player.FoodPoints = m_Game.Rules.ActorMaxFood(m_Game.Player);
                m_Game.Player.SleepPoints = m_Game.Rules.ActorMaxSleep(m_Game.Player);
                if (m_Game.Session.GameMode != GameMode.GM_STANDARD) m_Game.Player.Infection = 0;
                if (RogueGame.Options.IsSanityEnabled) m_Game.Player.Sanity = m_Game.Rules.ActorMaxSanity(m_Game.Player); //@MP - fixed crappy implem (Release 5-2)
                //m_Game.Player.Inventory.MaxCapacity = 10;
                /*Tile tile = m_Game.Session.CurrentMap.GetTileAt(m_Game.Player.Location.Position);
                if (tile == null)
                    return;
                else if (tile.IsInside)
                    m_Game.Player.SleepPoints = (m_Game.Rules.ActorMaxSleep(m_Game.Player) / 2);*/

                /*Logger.WriteLine(Logger.Stage.RUN_MAIN, String.Format("{0} murders.", m_Game.Player.MurdersCounter.ToString()));
                foreach (Actor a in m_Game.Player.Location.Map.Actors)
                {
                    if (m_Game.Player.IsSelfDefenceFrom(a))
                        Logger.WriteLine(Logger.Stage.RUN_MAIN, String.Format("Self-defense from: {0}.", a.TheName));

                    if (m_Game.Rules.AreGroupEnemies(m_Game.Player, a))
                        Logger.WriteLine(Logger.Stage.RUN_MAIN, String.Format("Group enemy: {0}.", a.TheName));
                }*/
            }
            // F7 - DEV - toggle FPS
            if (e.Code == Keys.F7)
            {
                m_GameCanvas.ShowFPS = !m_GameCanvas.ShowFPS;
                UI_Repaint();
            }
            // F8 - DEV - resize to normal size
            if (e.Code == Keys.F8)
            {
                m_GameCanvas.NeedRedraw = true;
                SetClientSizeCore(RogueGame.CANVAS_WIDTH, RogueGame.CANVAS_HEIGHT);
                UI_Repaint();
            }
            // F9 - DEV - Show actors stats
            if (e.Code == Keys.F9)
            {
                m_Game.DEV_ToggleShowActorsStats();
                UI_Repaint();
                //m_Game.Session.World.Weather = Weather.CLEAR;
                //m_Game.DEV_KillAllActorsInMap();

                //Logger.WriteLine(Logger.Stage.RUN_MAIN, String.Format("{0}", m_Game.Player.Location.Map.CountAntiviralPills(m_Game)));
            }
            // F10 - DEV DEBUG - Reveal secret locations
            if (e.Code == Keys.F10)
            {
                m_Game.Session.PlayerKnows_CHARUndergroundFacilityLocation = true;
                m_Game.Session.CHARUndergroundFacility_Activated = true;
                m_Game.Session.UniqueMaps.CHARUndergroundFacility.TheMap.IsSecret = false;
                m_Game.Session.PlayerKnows_ArmyBaseLocation = true;
                m_Game.Session.ArmyBaseUnderground_Activated = true;
                m_Game.Session.UniqueMaps.ArmyBase.TheMap.IsSecret = false;
                m_Game.Session.PlayerKnows_HelicopterArrivalDetails = true;
                //m_Game.Player.Inventory.AddAsMuchAsPossible(new Item(m_Game.GameItems.UNIQUE_ARMY_ACCESS_BADGE), out int blah);

                //trigger a special event
                //m_Game.FireEvent_ArmySupplies(m_Game.Player.Location.Map.District.EntryMap);


                //warp to target map/zone
                #region
                /*
                bool targetFound = false;
                //-districts
                for (int y = 0; y < m_Game.Session.World.Size; y++)
                {
                    if (targetFound)
                        break;
                    for (int x = 0; x < m_Game.Session.World.Size; x++)
                    {
                        if (targetFound)
                            break;

                        District d = m_Game.Session.World[x, y];
                        //if (d.Kind == DistrictKind.GREEN)
                        //{
                            //-maps
                            for (int a = 0; a < d.EntryMap.Width; a++)
                            {
                                if (targetFound)
                                    break;
                                for (int b = 0; b < d.EntryMap.Height; b++)
                                {
                                    if (targetFound)
                                        break;

                                    //-zones
                                    System.Collections.Generic.List<Zone> zones = d.EntryMap.GetZonesAt(a, b);
                                    if (zones != null)
                                    {
                                        foreach (Zone z in zones)
                                        {
                                            if (z.Name.Contains("Mall"))
                                            {
                                                targetFound = true;
                                                m_Game.Session.CurrentMap = d.EntryMap;
                                                d.EntryMap.PlaceActorAt(m_Game.Player, new Point(a,b));
                                                d.EntryMap.MoveActorToFirstPosition(m_Game.Player);
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        //}
                    }
                }
                */
                #endregion
            }

            // F10 - DEV STATS - Show pop graph.
            /*if (e.Code == Keys.F10)
            {
                District d = m_Game.Player.Location.Map.District;

                UI_Clear(Color.Black);
                // axis
                UI_DrawLine(Color.White, 0, 0, 0, RogueGame.CANVAS_HEIGHT);
                UI_DrawLine(Color.White, 0, RogueGame.CANVAS_HEIGHT, RogueGame.CANVAS_WIDTH, RogueGame.CANVAS_HEIGHT);
                // plot.
                int prevL = 0;
                int prevU = 0;
                const int XSCALE = WorldTime.TURNS_PER_HOUR;
                const int YSCALE = 10;
                for (int turn = 0; turn < m_Game.Session.WorldTime.TurnCounter; turn += XSCALE)
                {
                    if (turn % WorldTime.TURNS_PER_DAY == 0)
                        UI_DrawLine(Color.White, turn / XSCALE, RogueGame.CANVAS_HEIGHT, turn / XSCALE, 0);

                    Session.DistrictStat.Record? r = m_Game.Session.GetStatRecord(d, turn);
                    if (r == null) break;
                    int L = r.Value.livings;
                    UI_DrawLine(Color.Green, 
                        (turn - 1)/XSCALE, RogueGame.CANVAS_HEIGHT - YSCALE * prevL, 
                        turn/XSCALE, RogueGame.CANVAS_HEIGHT - YSCALE * L);
                    int U = r.Value.undeads;
                    UI_DrawLine(Color.Red, 
                        (turn - 1)/XSCALE, RogueGame.CANVAS_HEIGHT - YSCALE * prevU, 
                        turn/XSCALE, RogueGame.CANVAS_HEIGHT - YSCALE * U);
                    prevL = L;
                    prevU = U;
                }
                UI_Repaint();
                UI_WaitKey();
            }*/

            // F11 - DEV - Toggle player invincibility
            if (e.Code == Keys.F11)
            {
                m_Game.DEV_TogglePlayerInvincibility();
                UI_Repaint();
            }

            // F12 - DEV - Max trust for all player followers
            if (e.Code == Keys.F12)
            {
                m_Game.DEV_MaxTrust();
                UI_Repaint();
            }

            // INSERT - DEV - Toggle bot mode // alpha10.1
            if (e.Code == Keys.Insert)
            {
                m_Game.BotToggleControl();
                UI_Repaint();
            }

            // HOME - DEV misc
            if (e.Code == Keys.Home)
            {
                /*
                //@@MP - drop ItemLight
                Item item = m_Game.Player.GetEquippedItem(DollPart.LEFT_HAND);
                m_Game.Player.Inventory.RemoveAllQuantity(item);
                item.EquippedPart = DollPart.NONE;*/

                //@@MP - make it rain
                m_Game.Session.World.Weather = Weather.HEAVY_RAIN;
                UI_Repaint();
            }
#endif
        }

        public Point UI_GetMousePosition()
        {
            DoEvents();
            return mouseLocation;
        }

        bool m_HasMouseButtons = false;
        MouseButtons m_MouseButtons;

        public void UI_PostMouseButtons(MouseButtons buttons)
        {
            m_HasMouseButtons = true;
            m_MouseButtons = buttons;
        }

        public MouseButtons? UI_PeekMouseButtons()
        {
            if (!m_HasMouseButtons)
                return null;

            m_HasMouseButtons = false;
            return m_MouseButtons;
        }

        #endregion

        #region Delay
        public void UI_Wait(int msecs)
        {
            UI_Repaint();
            Thread.Sleep(msecs);
        }
        #endregion

        #region Canvas Painting
        public void UI_Repaint()
        {
            canvas.Clear(SFML.Graphics.Color.Black);
            foreach (var drawable in drawables.OrderBy(x => x is SFML.Graphics.Text ? 1 : 0))
            {
                canvas.Draw(drawable);
            }
            DoEvents();
        }

        public void UI_Clear(Color clearColor)
        {
            drawables.Clear();
        }

        public void UI_DrawImage(string imageID, int gx, int gy)
        {
            Texture tex = GameImages.Get(imageID);
            Sprite spr = new Sprite(tex);
            spr.Position = new Vector2f(gx, gy);
            drawables.Add(spr);
        }

        public void UI_DrawImage(string imageID, int gx, int gy, Color tint)
        {
            Texture tex = GameImages.Get(imageID);
            Sprite spr = new Sprite(tex);
            spr.Color = new SFML.Graphics.Color(tint.R, tint.G, tint.B, tint.A);
            spr.Position = new Vector2f(gx, gy);
            drawables.Add(spr);
        }

        public void UI_DrawImageTransform(string imageID, int gx, int gy, Color tint, float rotation, float scale) //@@MP - added tint (Release 7-2)
        {
            Texture tex = GameImages.Get(imageID);
            Sprite spr = new Sprite(tex);
            spr.Color = new SFML.Graphics.Color(tint.R, tint.G, tint.B, tint.A);
            spr.Position = new Vector2f(gx + (float)RogueGame.TILE_SIZE / 2.0f, gy + (float)RogueGame.TILE_SIZE / 2.0f);
            spr.Scale = new Vector2f(scale, scale);
            spr.Origin = new Vector2f((float)RogueGame.TILE_SIZE / 2.0f, (float)RogueGame.TILE_SIZE / 2.0f);
            spr.Rotation = rotation;
            drawables.Add(spr);
        }

        public void UI_DrawGrayLevelImage(string imageID, int gx, int gy, string grayLevelType) //@@MP - added parameter to allow graylevels for different times of day/location (Release 6-2)
        {
            float grayLevel;
            switch (grayLevelType)
            {
                //@@MP - tiles visited but not within current FOV are now darker (Release 6-2)
                case "daytime": grayLevel = 0.40f; break; //@@MP this was the one and only in vanilla
                case "nighttime_clear": grayLevel = 0.20f; break;
                case "nighttime_clouded": grayLevel = 0.15f; break;
                case "underground_notorch": grayLevel = 0.20f; break;
                case "underground_littorch": grayLevel = 0.10f; break;
                default: throw new ArgumentOutOfRangeException("grayLevelType", "unhandled grayLevelType");
            }            
            Texture tex = GameImages.Get(imageID);
            Sprite spr = new Sprite(tex);
            spr.Position = new Vector2f(gx, gy);
            drawables.Add(new GrayLevelSprite(spr, grayLevel));
        }

        public void UI_DrawTransparentImage(float alpha, string imageID, int gx, int gy)
        {
            Texture tex = GameImages.Get(imageID);
            Sprite spr = new Sprite(tex);
            spr.Color = new SFML.Graphics.Color(0xff, 0xff, 0xff, (byte)((float)(0xff) * alpha));
            spr.Position = new Vector2f(gx, gy);
            drawables.Add(spr);
        }

        public void UI_DrawLine(Color color, int gxFrom, int gyFrom, int gxTo, int gyTo)
        {
            var sfmlColor = new SFML.Graphics.Color(color.R, color.G, color.B, color.A);
            Vertex a = new Vertex(new Vector2f(gxFrom, gyFrom), sfmlColor);
            Vertex b = new Vertex(new Vector2f(gxTo, gyTo), sfmlColor);
            VertexArray va = new VertexArray(PrimitiveType.Lines);
            va.Append(a);
            va.Append(b);
            drawables.Add(va);
        }

        public void UI_DrawString(Color color, string text, int gx, int gy, Color? shadowColor)
        {
            Text txt = new Text(text, sfmlFont, FONT_SIZE);
            txt.FillColor = new SFML.Graphics.Color(color.R, color.G, color.B, color.A);
            txt.Position = new Vector2f(gx, gy + FONT_Y_ADJUST);
            if (shadowColor.HasValue) {
                Text shadow = new Text(txt);
                shadow.FillColor = new SFML.Graphics.Color(shadowColor.Value.R, shadowColor.Value.G, shadowColor.Value.B, shadowColor.Value.A);
                shadow.Position = new Vector2f(gx + 1, gy + FONT_Y_ADJUST + 1);
                drawables.Add(shadow);
            }
            drawables.Add(txt);
        }

        public void UI_DrawString(Color color, string text, int gx, int gy)
        {
            UI_DrawString(color, text, gx, gy, null);
        }

        public void UI_DrawStringBold(Color color, string text, int gx, int gy, Color? shadowColor)
        {
            Text txt = new Text(text, sfmlBoldFont, FONT_SIZE);
            txt.FillColor = new SFML.Graphics.Color(color.R, color.G, color.B, color.A);
            txt.Position = new Vector2f(gx, gy + FONT_Y_ADJUST);
            if (shadowColor.HasValue) {
                Text shadow = new Text(txt);
                shadow.FillColor = new SFML.Graphics.Color(shadowColor.Value.R, shadowColor.Value.G, shadowColor.Value.B, shadowColor.Value.A);
                shadow.Position = new Vector2f(gx + 1, gy + FONT_Y_ADJUST + 1);
                drawables.Add(shadow);
            }
            drawables.Add(txt);
        }

        public void UI_DrawStringBold(Color color, string text, int gx, int gy)
        {
            UI_DrawString(color, text, gx, gy, null);
        }        

        public void UI_DrawRect(Color color, Rectangle rect)
        {
            RectangleShape r = new RectangleShape(new Vector2f(rect.Width, rect.Height));
            r.OutlineColor = new SFML.Graphics.Color(color.R, color.G, color.B, color.A);
            r.FillColor = SFML.Graphics.Color.Transparent;
            r.Position = new Vector2f(rect.Left, rect.Top);
            drawables.Add(r);
        }

        public void UI_FillRect(Color color, Rectangle rect)
        {
            RectangleShape r = new RectangleShape(new Vector2f(rect.Width, rect.Height));
            r.FillColor = new SFML.Graphics.Color(color.R, color.G, color.B, color.A);
            r.OutlineColor = SFML.Graphics.Color.Transparent;
            r.Position = new Vector2f(rect.Left, rect.Top);
            drawables.Add(r);
        }

        public void UI_DrawPopup(string[] lines, Color textColor, Color boxBorderColor, Color boxFillColor, int gx, int gy)
        { //@@MP - the popup that appears when you hover the mouse over actors/items in-game
            if (lines == null) //@@MP (Release 5-7)
                throw new ArgumentNullException("lines", "null lines");

            /////////////////
            // Measure lines
            /////////////////
            int longestLineWidth = 0;
            int totalLineHeight = 0;
            Size[] linesSize = new Size[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                linesSize[i] = new Size(lines[i].Length * CHAR_WIDTH, CHAR_HEIGHT);
                if (linesSize[i].Width > longestLineWidth)
                    longestLineWidth = linesSize[i].Width;
                totalLineHeight += CHAR_HEIGHT;                
            }

            ///////////////////
            // Setup popup box
            ///////////////////
            const int BOX_MARGIN = 2;
            Point boxPos = new Point(gx,gy);
            Size boxSize = new Size(longestLineWidth + 2 * BOX_MARGIN, totalLineHeight + 2 * BOX_MARGIN);
            Rectangle boxRect = new Rectangle(boxPos, boxSize);

            //////////////////
            // Draw popup box
            //////////////////
            try
            {
                UI_FillRect(boxFillColor, boxRect);
                UI_DrawRect(boxBorderColor, boxRect);
            }
            catch (Exception e)
            {
                Logger.WriteLine(Logger.Stage.RUN_GFX, "failed to draw popup box:: " + e.ToString());
                throw; //@@MP (Release 5-7)
            }

            //////////////
            // Draw lines
            //////////////
            int lineX = boxPos.X + BOX_MARGIN;
            int lineY = boxPos.Y + BOX_MARGIN;
            for (int i = 0; i < lines.Length; i++)
            {
                UI_DrawStringBold(textColor, lines[i], lineX, lineY, null);
                lineY += linesSize[i].Height;
            }
        }

        // alpha10
        public void UI_DrawPopupTitle(string title, Color titleColor, string[] lines, Color textColor, Color boxBorderColor, Color boxFillColor, int gx, int gy)
        {
            if (lines == null || lines.Length == 0) //@@MP (Release 7-1)
                return;

            /////////////////
            // Measure lines
            /////////////////
            int longestLineWidth = 0;
            int totalLineHeight = 0;
            Size[] linesSize = new Size[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                linesSize[i] = new Size(lines[i].Length * CHAR_WIDTH, CHAR_HEIGHT);
                if (linesSize[i].Width > longestLineWidth)
                    longestLineWidth = linesSize[i].Width;
                totalLineHeight += CHAR_HEIGHT;   
            }

            Size titleSize = new Size(title.Length * CHAR_WIDTH, CHAR_HEIGHT);
            if (titleSize.Width > longestLineWidth)
                longestLineWidth = titleSize.Width;
            totalLineHeight += titleSize.Height;
            const int TITLE_BAR_LINE = 1;
            totalLineHeight += TITLE_BAR_LINE;

            ///////////////////
            // Setup popup box
            ///////////////////
            const int BOX_MARGIN = 2;
            Point boxPos = new Point(gx, gy);
            Size boxSize = new Size(longestLineWidth + 2 * BOX_MARGIN, totalLineHeight + 2 * BOX_MARGIN);
            Rectangle boxRect = new Rectangle(boxPos, boxSize);

            //////////////////
            // Draw popup box
            //////////////////
            UI_FillRect(boxFillColor, boxRect);
            UI_DrawRect(boxBorderColor, boxRect);

            //////////////
            // Draw title
            //////////////
            int titleX = boxPos.X + BOX_MARGIN + (longestLineWidth - titleSize.Width) / 2;
            int titleY = boxPos.Y + BOX_MARGIN;
            int titleLineY = titleY + titleSize.Height + TITLE_BAR_LINE;
            UI_DrawStringBold(titleColor, title, titleX, titleY, null);
            UI_DrawLine(boxBorderColor, boxRect.Left, titleLineY, boxRect.Right, titleLineY);

            //////////////
            // Draw lines
            //////////////
            int lineX = boxPos.X + BOX_MARGIN;
            int lineY = titleLineY + TITLE_BAR_LINE;

            for (int i = 0; i < lines.Length; i++)
            {
                UI_DrawStringBold(textColor, lines[i], lineX, lineY, null);
                lineY += linesSize[i].Height;
            }
        }

        // alpha10
        public void UI_DrawPopupTitleColors(string title, Color titleColor, string[] lines, Color[] colors, Color boxBorderColor, Color boxFillColor, int gx, int gy)
        {
            if (lines == null || lines.Length == 0 || colors == null || colors.Length == 0) //@@MP (Release 7-1)
                return;

            /////////////////
            // Measure lines
            /////////////////
            int longestLineWidth = 0;
            int totalLineHeight = 0;
            Size[] linesSize = new Size[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                linesSize[i] = new Size(lines[i].Length * CHAR_WIDTH, CHAR_HEIGHT);
                if (linesSize[i].Width > longestLineWidth)
                    longestLineWidth = linesSize[i].Width;
                totalLineHeight += CHAR_HEIGHT;   
            }

            Size titleSize = new Size(title.Length * CHAR_WIDTH, CHAR_HEIGHT);
            if (titleSize.Width > longestLineWidth)
                longestLineWidth = titleSize.Width;
            totalLineHeight += titleSize.Height;
            const int TITLE_BAR_LINE = 1;
            totalLineHeight += TITLE_BAR_LINE;

            ///////////////////
            // Setup popup box
            ///////////////////
            const int BOX_MARGIN = 2;
            Point boxPos = new Point(gx, gy);
            Size boxSize = new Size(longestLineWidth + 2 * BOX_MARGIN, totalLineHeight + 2 * BOX_MARGIN);
            Rectangle boxRect = new Rectangle(boxPos, boxSize);

            //////////////////
            // Draw popup box
            //////////////////
            UI_FillRect(boxFillColor, boxRect);
            UI_DrawRect(boxBorderColor, boxRect);

            //////////////
            // Draw title
            //////////////
            int titleX = boxPos.X + BOX_MARGIN + (longestLineWidth - titleSize.Width) / 2;
            int titleY = boxPos.Y + BOX_MARGIN;
            int titleLineY = titleY + titleSize.Height + TITLE_BAR_LINE;
            UI_DrawStringBold(titleColor, title, titleX, titleY, null);
            UI_DrawLine(boxBorderColor, boxRect.Left, titleLineY, boxRect.Right, titleLineY);

            //////////////
            // Draw lines
            //////////////
            int lineX = boxPos.X + BOX_MARGIN;
            int lineY = titleLineY + TITLE_BAR_LINE;

            for (int i = 0; i < lines.Length; i++)
            {
                UI_DrawStringBold(colors[i], lines[i], lineX, lineY, null);
                lineY += linesSize[i].Height;
            }
        }

        public void UI_ClearMinimap(Color color)
        {
            minimapDrawables.Clear();
        }

        public void UI_SetMinimapColor(int x, int y, Color color)
        {
            RectangleShape r = new RectangleShape(new Vector2f(RogueGame.MINITILE_SIZE, RogueGame.MINITILE_SIZE));
            r.FillColor = new SFML.Graphics.Color(color.R, color.G, color.B, color.A);
            r.OutlineColor = SFML.Graphics.Color.Transparent;
            r.Position = new Vector2f(x * RogueGame.MINITILE_SIZE, y * RogueGame.MINITILE_SIZE);
            minimapDrawables.Add(r);            
        }

        public void UI_DrawMinimap(int gx, int gy)
        {
            minimap.Clear(SFML.Graphics.Color.Black);            
            foreach (var drawable in minimapDrawables)
            {
                minimap.Draw(drawable);
            }
            minimap.Display();

            Sprite spr = new Sprite(minimap.Texture);
            spr.Position = new Vector2f(gx, gy);
            drawables.Add(spr);            
        }
        
        #endregion

        #region Canvas scaling
        public Point UI_TransformMouseCoordinates(Point coord)
        {
            var v = renderWindow.MapPixelToCoords(new Vector2i(coord.X, coord.Y), renderWindow.GetView());
            return new Point((int)v.X, (int)v.Y);
        }
        #endregion

        #region Screenshot
        public string UI_SaveScreenshot(string filePath)
        {
            var img = canvas.Texture.CopyToImage();
            img.FlipVertically();
            img.SaveToFile(filePath+".png");
            return filePath+".png";
        }

        public string UI_ScreenshotExtension()
        {
            return ".png";
        }
        #endregion

        #region Exiting
        public void UI_DoQuit()
        {
        }
        #endregion
        #endregion
    }
}
