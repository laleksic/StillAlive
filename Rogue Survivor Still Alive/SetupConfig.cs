using System;
using System.IO;
using System.Reflection;

namespace djack.RogueSurvivor
{
    public static class SetupConfig
    {
        //public const string GAME_VERSION = "Still Alive";
        public static string GAME_VERSION = "Still Alive " + typeof(SetupConfig).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion + " ALPHA (CROSS-PLATFORM EDITION!)"; //@@MP remember to update the Assembly info (Release 1)

        public enum eWindow //@@MP (Release 5-5)
        {
            WINDOW_INVALID,
            WINDOW_FULLSCREEN,
            WINDOW_WINDOWED,
            COUNT //@@MP - removed underscore for CLS compliance (Release 5-7)
        }

        public static eWindow Window { get; set; } //@@MP (Release 5-5)
        public static bool WriteLogToFile { get; set; } //@@MP (Release 6-2)

        public static string DirPath
        {
            get
            {
                //return Environment.CurrentDirectory + @"\Config\"; //@@MP - switched to AppData so that non-admins can run the game (Release 5-1)
                string myDocs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                return myDocs + @"/Rogue Survivor/" + SetupConfig.GAME_VERSION + @"/Config/";
            }
        }

        static string FilePath
        {
            get
            {
                return DirPath + @"/setup.dat";
            }
        }

        public static void Save()
        {
            using (StreamWriter sw = File.CreateText(FilePath))
            {
                sw.WriteLine(SetupConfig.Window.ToString()); //@@MP (Release 5-5)
                sw.WriteLine(SetupConfig.WriteLogToFile.ToString()); //@@MP (Release 6-2)
            }
        }

        public static void Load()
        {
            if (File.Exists(FilePath))
            {
                using (StreamReader sr = File.OpenText(FilePath))
                {
                    SetupConfig.Window = toWindow(sr.ReadLine());//@@MP (Release 5-5)
                    SetupConfig.WriteLogToFile = toLogToFile(sr.ReadLine()); //@@MP (Release 6-2)
                }
            }
            else //@@MP - defaults for first run
            {
                if (!Directory.Exists(DirPath))
                    Directory.CreateDirectory(DirPath);

                SetupConfig.Window = eWindow.WINDOW_FULLSCREEN; //@@MP (Release 5-5)
                SetupConfig.WriteLogToFile = false; //@@MP (Release 6-2)

                Save();
            }
        }

        public static eWindow toWindow(string w) //@@MP (Release 5-5)
        {
            if (w == eWindow.WINDOW_FULLSCREEN.ToString())
                return eWindow.WINDOW_FULLSCREEN;
            if (w == eWindow.WINDOW_WINDOWED.ToString())
                return eWindow.WINDOW_WINDOWED;
            return eWindow.WINDOW_INVALID;
        }

        public static bool toLogToFile(string l) //@@MP (Release 6-2)
        {
            if (l == "True")
                return true;
            else
                return false;
        }
    }
}
