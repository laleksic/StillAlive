using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using SFML;
using KeyEventArgs = SFML.Window.KeyEventArgs;
using Keys = SFML.Window.Keyboard.Key;

namespace djack.RogueSurvivor.Engine
{
    class KeyEventArgsComparer : IComparer<KeyEventArgs> 
    {
        public int Compare(KeyEventArgs x, KeyEventArgs y)
        {
            if (x.Code == y.Code)
            {
                if (x.Control == y.Control)
                {
                    if (x.Alt == y.Alt)
                    {
                        if (x.Shift == y.Shift)
                        {
                            return 0;
                        }
                        return Comparer<bool>.Default.Compare(x.Shift, y.Shift);
                    }
                    return Comparer<bool>.Default.Compare(x.Alt, y.Alt);
                }
                return Comparer<bool>.Default.Compare(x.Control, y.Control);
            }
            return Comparer<int>.Default.Compare((int)x.Code, (int)y.Code);
        }
    }

    [Serializable]
    class Keybindings
    {
        #region Fields
        Dictionary<PlayerCommand, KeyEventArgs> m_CommandToKeyData;
        SortedDictionary<KeyEventArgs, PlayerCommand> m_KeyToCommand;
        #endregion

        #region Init
        public Keybindings()
        {
            m_CommandToKeyData = new Dictionary<PlayerCommand, KeyEventArgs>();
            m_KeyToCommand = new SortedDictionary<KeyEventArgs, PlayerCommand>(new KeyEventArgsComparer());
            ResetToDefaults();
        }

        public void ResetToDefaults()
        {
            m_CommandToKeyData.Clear();
            m_KeyToCommand.Clear();

            Set(PlayerCommand.BUILD_MODE, Keys.B);
            Set(PlayerCommand.BREAK_MODE, Keys.K);
            Set(PlayerCommand.CLOSE_DOOR, Keys.C);
            Set(PlayerCommand.FIRE_MODE, Keys.F);
            Set(PlayerCommand.HELP_MODE, Keys.H);
            Set(PlayerCommand.KEYBINDING_MODE, Keys.K, false, false, true);

            Set(PlayerCommand.ITEM_SLOT_0, Keys.Num1);
            Set(PlayerCommand.ITEM_SLOT_1, Keys.Num2);
            Set(PlayerCommand.ITEM_SLOT_2, Keys.Num3);
            Set(PlayerCommand.ITEM_SLOT_3, Keys.Num4);
            Set(PlayerCommand.ITEM_SLOT_4, Keys.Num5);
            Set(PlayerCommand.ITEM_SLOT_5, Keys.Num6);
            Set(PlayerCommand.ITEM_SLOT_6, Keys.Num7);
            Set(PlayerCommand.ITEM_SLOT_7, Keys.Num8);
            Set(PlayerCommand.ITEM_SLOT_8, Keys.Num9);
            Set(PlayerCommand.ITEM_SLOT_9, Keys.Num0);

            Set(PlayerCommand.ABANDON_GAME, Keys.A, false, false, true);
            Set(PlayerCommand.ADVISOR, Keys.H, false, false, true);
            Set(PlayerCommand.BURY_CORPSE, Keys.B, false, false, true); //@@MP (Release 7-6)
            Set(PlayerCommand.CITY_INFO, Keys.M);
            Set(PlayerCommand.COOK_FOOD, Keys.C, true, false, false); //@@MP (Release 7-6)
            Set(PlayerCommand.DESTROY_ITEM, Keys.D, true, false, false); //@@MP (Release 7-6)
            Set(PlayerCommand.EAT_CORPSE, Keys.E, false, false, true);
            Set(PlayerCommand.ESC, Keys.Escape); //@@MP (Release 7-4)
            Set(PlayerCommand.GIVE_ITEM, Keys.G);
            Set(PlayerCommand.HINTS_SCREEN_MODE, Keys.H, true, false, false);
            Set(PlayerCommand.ICONS_LEGEND, Keys.F1); //@@MP (Release 6-1)
            Set(PlayerCommand.NEGOTIATE_TRADE, Keys.E);
            Set(PlayerCommand.LOAD_GAME, Keys.L, false, false, true);
            Set(PlayerCommand.INSPECTION_MODE, Keys.I); //@@MP (Release 7-1)
            Set(PlayerCommand.MAKE_COOKING_FIRE, Keys.F, true, false, false); //@@MP (Release 7-6)
            Set(PlayerCommand.MARK_ENEMIES_MODE, Keys.E, true, false, false);
            Set(PlayerCommand.MESSAGE_LOG, Keys.M, false, false, true);
            Set(PlayerCommand.MOVE_E, Keys.Numpad6);
            Set(PlayerCommand.MOVE_N, Keys.Numpad8);
            Set(PlayerCommand.MOVE_NE, Keys.Numpad9);
            Set(PlayerCommand.MOVE_NW, Keys.Numpad7);
            Set(PlayerCommand.MOVE_S, Keys.Numpad2);
            Set(PlayerCommand.MOVE_SE, Keys.Numpad3);
            Set(PlayerCommand.MOVE_SW, Keys.Numpad1);
            Set(PlayerCommand.MOVE_W, Keys.Numpad4);
            Set(PlayerCommand.OPTIONS_MODE, Keys.O, false, false, true);
            Set(PlayerCommand.ORDER_MODE, Keys.O);
            Set(PlayerCommand.PULL_MODE, Keys.P, true, false, false); // alpha10
            Set(PlayerCommand.PUSH_MODE, Keys.P);
            Set(PlayerCommand.QUIT_GAME, Keys.Q, false, false, true);
            Set(PlayerCommand.REVIVE_CORPSE, Keys.R, false, false, true);
            Set(PlayerCommand.RUN_TOGGLE, Keys.R);
            Set(PlayerCommand.SAVE_GAME, Keys.S, false, false, true);
            Set(PlayerCommand.SCREENSHOT, Keys.N, false, false, true);
            Set(PlayerCommand.SHOUT, Keys.S);
            Set(PlayerCommand.SLEEP, Keys.Z);
            Set(PlayerCommand.SWITCH_PLACE, Keys.S, true, false, false);
            Set(PlayerCommand.SWAP_INVENTORY, Keys.Y); //@@MP (Release 8-2)
            Set(PlayerCommand.LEAD_MODE, Keys.T);
            Set(PlayerCommand.UNLOAD_AMMO, Keys.U); //@@MP (Release 7-6)
            Set(PlayerCommand.USE_SPRAY, Keys.A);
            Set(PlayerCommand.USE_EXIT, Keys.X);
            Set(PlayerCommand.WAIT_OR_SELF, Keys.Numpad5);
            Set(PlayerCommand.WAIT_LONG, Keys.W);
        }
        #endregion

        #region Getting & Setting
        /// <summary>
        /// Get KeyData (key code & modifiers).
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public KeyEventArgs Get(PlayerCommand command)
        {
            KeyEventArgs key;
            if (m_CommandToKeyData.TryGetValue(command, out key))
                return key;
            else
                return null;
        }

        /// <summary>
        /// Get KeyData in human-friendly format.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public string GetFriendlyFormat(PlayerCommand command) //@@MP (Release 6-6)
        {
            KeyEventArgs key;
            if (m_CommandToKeyData.TryGetValue(command, out key))
            {
                string keyAsString = key.ToString();
                if (keyAsString.Contains(","))
                {
                    keyAsString = keyAsString.Replace(",", " +");
                }
                
                return keyAsString;
            }
            else
                return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">KeyData</param>
        /// <returns></returns>
        public PlayerCommand Get(KeyEventArgs key)
        {
            PlayerCommand cmd;
            if (m_KeyToCommand.TryGetValue(key, out cmd))
                return cmd;
            return PlayerCommand.NONE;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="key">KeyData</param>
        public void Set(PlayerCommand command, KeyEventArgs key)
        {
            // remove previous bind.
            PlayerCommand prevCommand = Get(key);
            if (prevCommand != PlayerCommand.NONE)
            {
                m_CommandToKeyData.Remove(prevCommand);
            }
            KeyEventArgs prevKey = Get(command);
            if (prevKey != null)
            {
                m_KeyToCommand.Remove(prevKey);
            }
                     
            // rebind.
            m_CommandToKeyData[command] = key;
            m_KeyToCommand[key] = command;
        }

        public void Set(PlayerCommand command, Keys code, bool control, bool alt, bool shift)
        {
            var e = new SFML.Window.KeyEvent();
            e.Code = code;
            e.Control = control? 1: 0;
            e.Alt = alt? 1: 0;
            e.Shift = shift? 1: 0;
            e.System = 0;
            var key = new KeyEventArgs(e);
            Set(command, key);
        }

        public void Set(PlayerCommand command, Keys code)
        {
            Set(command, code, false, false, false);
        }       
        #endregion

        #region Checking for keys conflict
        public bool CheckForConflict()
        {
            foreach (KeyEventArgs key1 in m_CommandToKeyData.Values)
            {
                int bound = m_KeyToCommand.Keys.Count((k) => k == key1);
                if (bound > 1)
                    return true;
            }

            return false;
        }
        #endregion

        #region Saving & Loading
        public static void Save(Keybindings kb, string filepath)
        {
            if (kb == null)
                throw new ArgumentNullException("kb");

            Logger.WriteLine(Logger.Stage.RUN_MAIN, "saving keybindings...");

            IFormatter formatter = CreateFormatter();
            Stream stream = null; //@@MP - try/finally ensures that the stream is always closed (Release 5-7)
            try
            {
                stream = CreateStream(filepath, true);
                formatter.Serialize(stream, kb);
                stream.Flush();
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            Logger.WriteLine(Logger.Stage.RUN_MAIN, "saving keybindings... done!");
        }

        /// <summary>
        /// Attempt to load, if failed return bindings with defaults.
        /// </summary>
        /// <returns></returns>
        public static Keybindings Load(string filepath)
        {
            Logger.WriteLine(Logger.Stage.RUN_MAIN, "loading keybindings...");

            Keybindings kb;
            if (File.Exists(filepath))
            {
                IFormatter formatter = CreateFormatter();
                Stream stream = null; //@@MP - try/finally ensures that the stream is always closed (Release 5-7)
                try
                {
                    stream = CreateStream(filepath, false);
                    kb = (Keybindings)formatter.Deserialize(stream);
                }
                finally
                {
                    if (stream != null)
                        stream.Close();
                }
            }
            else
            {
                Logger.WriteLine(Logger.Stage.RUN_MAIN, "failed to load keybindings (first run?), using defaults.");
                kb = new Keybindings();
                kb.ResetToDefaults();
            }

            Logger.WriteLine(Logger.Stage.RUN_MAIN, "loading keybindings... done!");
            return kb;
        }

        static IFormatter CreateFormatter()
        {
            return new BinaryFormatter();
        }

        static Stream CreateStream(string saveName, bool save)
        {
            try
            {
                return new FileStream(saveName,
                save ? FileMode.Create : FileMode.Open,
                save ? FileAccess.Write : FileAccess.Read,
                FileShare.None);
            }
            catch (System.IO.FileNotFoundException)
            {
                return null;
            }
        }
        #endregion
    }
}
