
namespace djack.RogueSurvivor.Gameplay
{
    /// <summary>
    /// Long-running ambient sfx. These may be played in conjunction with background music
    /// </summary>
    static class GameAmbients //@@MP (Release 6-1)
    {
        const string PATH = @"Resources\Ambients\";

        //@@MP (Release 6-1)
        public const string RAIN_OUTSIDE = "outside whilst raining";
        public const string RAIN_OUTSIDE_FILE = PATH + "rain_outside_looped";
        public const string RAIN_INSIDE = "inside whilst raining";
        public const string RAIN_INSIDE_FILE = PATH + "rain_inside_looped";
        //@@MP (Release 6-6)
        public const string THUNDERING_RAIN_OUTSIDE = "outside whilst thundering rain";
        public const string THUNDERING_RAIN_OUTSIDE_FILE = PATH + "thundering_rain_outside_looped";
        public const string THUNDERING_RAIN_INSIDE = "inside whilst thundering rain";
        public const string THUNDERING_RAIN_INSIDE_FILE = PATH + "thundering_rain_inside_looped";

        //@@MP (Release 6-1)
        public const string HELICOPTER_FLYOVER = "helicopter flyover";
        public const string HELICOPTER_FLYOVER_FILE = PATH + "helicopter_flyover";

        //@@MP - for when a helicopter is stationary or hovering (Release 6-4)
        public const string STATIONARY_HELICOPTER_FARTHEST = "stationary helicopter farthest";
        public const string STATIONARY_HELICOPTER_FARTHEST_FILE = PATH + "helicopter_static_farthest";
        public const string STATIONARY_HELICOPTER_FAR = "stationary helicopter far";
        public const string STATIONARY_HELICOPTER_FAR_FILE = PATH + "helicopter_static_far";
        public const string STATIONARY_HELICOPTER_NEAR = "stationary helicopter nearby";
        public const string STATIONARY_HELICOPTER_NEAR_FILE = PATH + "helicopter_static_nearby";
        public const string STATIONARY_HELICOPTER_VISIBLE = "stationary helicopter visible";
        public const string STATIONARY_HELICOPTER_VISIBLE_FILE = PATH + "helicopter_static_visible";

        //@@MP (Release 6-6)
        public const string NIGHT_ANIMALS_OUTSIDE = "wild animals";
        public const string NIGHT_ANIMALS_OUTSIDE_FILE = PATH + "night_animals_outside";
        public const string CHURCH_BELLS_OUTSIDE = "outside hearing church bells";
        public const string CHURCH_BELLS_OUTSIDE_FILE = PATH + "church_bells_outside";
        public const string CHURCH_BELLS_INSIDE = "inside hearing church bells";
        public const string CHURCH_BELLS_INSIDE_FILE = PATH + "church_bells_inside";
    }
}
