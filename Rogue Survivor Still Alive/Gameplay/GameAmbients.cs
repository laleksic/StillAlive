
namespace djack.RogueSurvivor.Gameplay
{
    /// <summary>
    /// Long-running ambient sfx. These may be played in conjunction with background music
    /// </summary>
    static class GameAmbients //@@MP (Release 6-1)
    {
        const string PATH = @"Resources\Ambients\";

        public const string RAIN_OUTSIDE = "outside whilst raining";
        public const string RAIN_OUTSIDE_FILE = PATH + "rain_outside_looped";
        public const string RAIN_INSIDE = "inside whilst raining";
        public const string RAIN_INSIDE_FILE = PATH + "rain_inside_looped";

        //@@MP (Release 6-1)
        public const string HELICOPTER_FLYOVER = "helicopter flyover";
        public const string HELICOPTER_FLYOVER_FILE = PATH + "helicopter_flyover";
    }
}
