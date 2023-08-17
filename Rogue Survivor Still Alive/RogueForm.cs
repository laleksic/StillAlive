using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using djack.RogueSurvivor.Data;
using djack.RogueSurvivor.Engine;
using djack.RogueSurvivor.Gameplay;

namespace djack.RogueSurvivor
{
    public partial class RogueForm : Form, IRogueUI
    {
        #region Fields
        RogueGame m_Game;
        Font m_NormalFont;
        Font m_BoldFont;
        #endregion

        #region Properties
        internal RogueGame Game
        {
            get { return m_Game; }
        }
        #endregion

        #region Init
        public RogueForm()
        {
            Logger.WriteLine(Logger.Stage.INIT_MAIN, "creating main form...");

            Logger.WriteLine(Logger.Stage.INIT_MAIN, "Form::InitializeComponent...");
            InitializeComponent();

            this.Text = "Rogue Survivor: " + SetupConfig.GAME_VERSION + " [";
            switch (SetupConfig.Video) //@@MP (Release 5-3)
            {
                case SetupConfig.eVideo.VIDEO_GDI_PLUS: this.Text += "GDI+ video,"; break;
            }
            switch (SetupConfig.Sound)
            {
                case SetupConfig.eSound.SOUND_SFML: this.Text += " SFML sound"; break;
                case SetupConfig.eSound.SOUND_NOSOUND: this.Text += " no sound"; break;
            }
            this.Text += "]";

            Logger.WriteLine(Logger.Stage.INIT_MAIN, "Form::SetClientSizeCore...");
            SetClientSizeCore(RogueGame.CANVAS_WIDTH, RogueGame.CANVAS_HEIGHT);
            // prevent flickering (gdi conflicting with directx?)
            Logger.WriteLine(Logger.Stage.INIT_MAIN, "Form::SetStyle...");
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);

            Logger.WriteLine(Logger.Stage.INIT_MAIN, "create font 1...");
            m_NormalFont = new Font("Lucida Console", 8.25f, FontStyle.Regular);
            Logger.WriteLine(Logger.Stage.INIT_MAIN, "create font 2...");
            m_BoldFont = new Font("Lucida Console", 8.25f, FontStyle.Bold);

            Logger.WriteLine(Logger.Stage.INIT_MAIN, "create RogueGame...");
            m_Game = new RogueGame(this);

            Logger.WriteLine(Logger.Stage.INIT_MAIN, "bind form...");
            m_GameCanvas.BindForm(this);
            //m_GameCanvas.ShowFPS = true;

            Logger.WriteLine(Logger.Stage.INIT_MAIN, "creating main form done.");

            //@@MP - optional fullscreen (Release 5-5)
            switch (SetupConfig.Window)
            {
                case SetupConfig.eWindow.WINDOW_FULLSCREEN:
                    this.WindowState = FormWindowState.Maximized;
                    this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                    //this.Bounds = Screen.PrimaryScreen.Bounds;
                    break;
                case SetupConfig.eWindow.WINDOW_WINDOWED:
                    this.WindowState = FormWindowState.Maximized;
                    this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
                    break;
            }
        }

        void LoadResources()
        {
            Logger.WriteLine(Logger.Stage.INIT_GFX, "loading images...");
            GameImages.LoadResources(this);
            Logger.WriteLine(Logger.Stage.INIT_GFX, "loading images done");
        }
        #endregion

        #region Form overloads
        #region Disable close button
        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.LinkDemand, Flags = System.Security.Permissions.SecurityPermissionFlag.UnmanagedCode)]
            get
            {
            CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CP_NOCLOSE_BUTTON;
                return cp;
            }
        }
        #endregion

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            LoadResources();
            m_Game.Run();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            m_GameCanvas.FillGameForm();
            Invalidate(true);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (m_Game.IsGameRunning)
            {
                e.Cancel = true;
                MessageBox.Show(this, "The game is still running. Please quit inside the game.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
            }
        }
        #endregion

        #region IRogueUI implementation
        
        #region Input
        bool m_HasKey = false;
        KeyEventArgs m_InKey;

        public KeyEventArgs UI_WaitKey()
        {
            m_HasKey = false;
            while (true)
            {
                Application.DoEvents();
                if (m_HasKey)
                    break;
                Thread.Sleep(1);
            }
            return m_InKey;
        }

        public KeyEventArgs UI_PeekKey()
        {
            Thread.Sleep(1);
            Application.DoEvents();
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
            switch (e.KeyCode)
            {
                case Keys.ShiftKey:
                case Keys.Shift:
                case Keys.LShiftKey:
                case Keys.RShiftKey:
                case Keys.Control:
                case Keys.ControlKey:
                case Keys.RControlKey:
                case Keys.LControlKey:
                case Keys.Alt:
                    return;
                default: 
                    break;
            }

            m_HasKey = true;
            m_InKey = e;
            e.Handled = true;

            /////////////////////
            // Cheats / Dev Tools
            /////////////////////
#if DEBUG
            // F6 - CHEAT - reveal all. //@@MP - and refill status (basically god mode)
            if (e.KeyCode == Keys.F6)
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
            if (e.KeyCode == Keys.F7)
            {
                m_GameCanvas.ShowFPS = !m_GameCanvas.ShowFPS;
                UI_Repaint();
            }
            // F8 - DEV - resize to normal size
            if (e.KeyCode == Keys.F8)
            {
                m_GameCanvas.NeedRedraw = true;
                SetClientSizeCore(RogueGame.CANVAS_WIDTH, RogueGame.CANVAS_HEIGHT);
                UI_Repaint();
            }
            // F9 - DEV - Show actors stats
            if (e.KeyCode == Keys.F9)
            {
                m_Game.DEV_ToggleShowActorsStats();
                UI_Repaint();
                //m_Game.Session.World.Weather = Weather.CLEAR;
                //m_Game.DEV_KillAllActorsInMap();

                //Logger.WriteLine(Logger.Stage.RUN_MAIN, String.Format("{0}", m_Game.Player.Location.Map.CountAntiviralPills(m_Game)));
            }
            // F10 - DEV DEBUG - Reveal secret locations
            if (e.KeyCode == Keys.F10)
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
            /*if (e.KeyCode == Keys.F10)
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
            if (e.KeyCode == Keys.F11)
            {
                m_Game.DEV_TogglePlayerInvincibility();
                UI_Repaint();
            }

            // F12 - DEV - Max trust for all player followers
            if (e.KeyCode == Keys.F12)
            {
                m_Game.DEV_MaxTrust();
                UI_Repaint();
            }

            // INSERT - DEV - Toggle bot mode // alpha10.1
            if (e.KeyCode == Keys.Insert)
            {
                m_Game.BotToggleControl();
                UI_Repaint();
            }

            // HOME - DEV misc
            if (e.KeyCode == Keys.Home)
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
            Thread.Sleep(1);
            Application.DoEvents();
            return m_GameCanvas.MouseLocation;
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

        public void UI_SetCursor(Cursor cursor)
        {
            if (cursor == Cursor)            
                return;

            this.Cursor = cursor;
            Application.DoEvents();
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
            /*Invalidate();
            Update();*/
            Refresh();
            Application.DoEvents();
        }

        public void UI_Clear(Color clearColor)
        {
            m_GameCanvas.Clear(clearColor);
        }

        public void UI_DrawImage(string imageID, int gx, int gy)
        {
            m_GameCanvas.AddImage(GameImages.Get(imageID), gx, gy);
        }

        public void UI_DrawImage(string imageID, int gx, int gy, Color tint)
        {
            m_GameCanvas.AddImage(GameImages.Get(imageID), gx, gy, tint);
        }

        public void UI_DrawImageTransform(string imageID, int gx, int gy, Color tint, float rotation, float scale) //@@MP - added tint (Release 7-2)
        {
            m_GameCanvas.AddImageTransform(GameImages.Get(imageID), gx, gy, tint, rotation, scale);
        }

        public void UI_DrawGrayLevelImage(string imageID, int gx, int gy, string grayLevelType) //@@MP - added parameter to allow graylevels for different times of day/location (Release 6-2)
        {
            m_GameCanvas.AddImage(GameImages.GetGrayLevel(imageID, grayLevelType), gx, gy);
        }

        public void UI_DrawTransparentImage(float alpha, string imageID, int gx, int gy)
        {
            m_GameCanvas.AddTransparentImage(alpha, GameImages.Get(imageID), gx, gy);
        }

        public void UI_DrawPoint(Color color, int gx, int gy)
        {
            m_GameCanvas.AddPoint(color, gx, gy);
        }

        public void UI_DrawLine(Color color, int gxFrom, int gyFrom, int gxTo, int gyTo)
        {
            m_GameCanvas.AddLine(color, gxFrom, gyFrom, gxTo, gyTo);
        }

        public void UI_DrawString(Color color, string text, int gx, int gy, Color? shadowColor)
        {
            if (shadowColor != null)
                m_GameCanvas.AddString(m_NormalFont, shadowColor.Value, text, gx + 1, gy + 1);
            m_GameCanvas.AddString(m_NormalFont, color, text, gx, gy);
        }

        public void UI_DrawStringBold(Color color, string text, int gx, int gy, Color? shadowColor)
        {
            if (shadowColor != null)
                m_GameCanvas.AddString(m_BoldFont, shadowColor.Value, text, gx + 1, gy + 1);
            m_GameCanvas.AddString(m_BoldFont, color, text, gx, gy);
        }

        public void UI_DrawRect(Color color, Rectangle rect)
        {
            if (rect.Width <= 0 || rect.Height <= 0)
                throw new ArgumentOutOfRangeException("rect","rectangle Width/Height <= 0");

            m_GameCanvas.AddRect(color, rect);
        }

        public void UI_FillRect(Color color, Rectangle rect)
        {
            if (rect.Width <= 0 || rect.Height <= 0)
                throw new ArgumentOutOfRangeException("rect","rectangle Width/Height <= 0");

            m_GameCanvas.AddFilledRect(color, rect);
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
                linesSize[i] = TextRenderer.MeasureText(lines[i], m_BoldFont);
                if (linesSize[i].Width > longestLineWidth)
                    longestLineWidth = linesSize[i].Width;
                totalLineHeight += linesSize[i].Height;                
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
                m_GameCanvas.AddFilledRect(boxFillColor, boxRect);
                m_GameCanvas.AddRect(boxBorderColor, boxRect);
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
                m_GameCanvas.AddString(m_BoldFont, textColor, lines[i], lineX, lineY);
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
                linesSize[i] = TextRenderer.MeasureText(lines[i], m_BoldFont);
                if (linesSize[i].Width > longestLineWidth)
                    longestLineWidth = linesSize[i].Width;
                totalLineHeight += linesSize[i].Height;
            }

            Size titleSize = TextRenderer.MeasureText(title, m_BoldFont);
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
            m_GameCanvas.AddFilledRect(boxFillColor, boxRect);
            m_GameCanvas.AddRect(boxBorderColor, boxRect);

            //////////////
            // Draw title
            //////////////
            int titleX = boxPos.X + BOX_MARGIN + (longestLineWidth - titleSize.Width) / 2;
            int titleY = boxPos.Y + BOX_MARGIN;
            int titleLineY = titleY + titleSize.Height + TITLE_BAR_LINE;
            m_GameCanvas.AddString(m_BoldFont, titleColor, title, titleX, titleY);
            m_GameCanvas.AddLine(boxBorderColor, boxRect.Left, titleLineY, boxRect.Right, titleLineY);

            //////////////
            // Draw lines
            //////////////
            int lineX = boxPos.X + BOX_MARGIN;
            int lineY = titleLineY + TITLE_BAR_LINE;

            for (int i = 0; i < lines.Length; i++)
            {
                m_GameCanvas.AddString(m_BoldFont, textColor, lines[i], lineX, lineY);
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
                linesSize[i] = TextRenderer.MeasureText(lines[i], m_BoldFont);
                if (linesSize[i].Width > longestLineWidth)
                    longestLineWidth = linesSize[i].Width;
                totalLineHeight += linesSize[i].Height;
            }

            Size titleSize = TextRenderer.MeasureText(title, m_BoldFont);
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
            m_GameCanvas.AddFilledRect(boxFillColor, boxRect);
            m_GameCanvas.AddRect(boxBorderColor, boxRect);

            //////////////
            // Draw title
            //////////////
            int titleX = boxPos.X + BOX_MARGIN + (longestLineWidth - titleSize.Width) / 2;
            int titleY = boxPos.Y + BOX_MARGIN;
            int titleLineY = titleY + titleSize.Height + TITLE_BAR_LINE;
            m_GameCanvas.AddString(m_BoldFont, titleColor, title, titleX, titleY);
            m_GameCanvas.AddLine(boxBorderColor, boxRect.Left, titleLineY, boxRect.Right, titleLineY);

            //////////////
            // Draw lines
            //////////////
            int lineX = boxPos.X + BOX_MARGIN;
            int lineY = titleLineY + TITLE_BAR_LINE;

            for (int i = 0; i < lines.Length; i++)
            {
                m_GameCanvas.AddString(m_BoldFont, colors[i], lines[i], lineX, lineY);
                lineY += linesSize[i].Height;
            }
        }

        public void UI_ClearMinimap(Color color)
        {
            m_GameCanvas.ClearMinimap(color);
        }

        public void UI_SetMinimapColor(int x, int y, Color color)
        {
            m_GameCanvas.SetMinimapColor(x, y, color);
        }

        public void UI_DrawMinimap(int gx, int gy)
        {
            m_GameCanvas.DrawMinimap(gx, gy);
        }
        
        #endregion

        #region Canvas scaling
        public float UI_GetCanvasScaleX()
        {
            return m_GameCanvas.ScaleX;
        }

        public float UI_GetCanvasScaleY()
        {
            return m_GameCanvas.ScaleY;
        }
        #endregion

        #region Screenshot
        public string UI_SaveScreenshot(string filePath)
        {
            return m_GameCanvas.SaveScreenShot(filePath);
        }

        public string UI_ScreenshotExtension()
        {
            return m_GameCanvas.ScreenshotExtension();
        }
        #endregion

        #region Exiting
        public void UI_DoQuit()
        {
            Close();
        }
        #endregion
        #endregion
    }
}
