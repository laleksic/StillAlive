using System;

namespace djack.RogueSurvivor.Gameplay
{
    static class GameTips
    {
        public static string[] TIPS = 
        {
            // livings
            "darkness is extra stressful, so livings tire faster at night.",
            "some people will attack others for food.",
            "livings vision deteriorates when sleepy, in the rain or during the night.",
            "seeing a living die in a fire is bad for one's sanity.",
            "livings will usually wake others if a fire breaks out nearby.",
            // undeads
        	"undeads can smell scents left by livings. Some sprays can cover your scent.", //@@MP - this is the maximum characters that should be used!
            "some undeads are smart and will avoid traps and fires.",
            "seeing an undead kill a living is bad for one's sanity.",
            // doing stuff
	        "you can recharge battery-powered items at power generators.",
            "arrows may miss or pierce their target and be reclaimed from nearby.",
            "you can expore the sewers, the subway and basements. you'll need a torch.",
	        "you can shove people around or push objects like cars and shelves.",
            "you can try to wake up other people by shouting near them.",
            "you may find police radios and various trackers helpful.",
            "you can eat or butcher corpses, at the cost of sanity though.",
            "you can try to revive people using the Medic skill.",
            "reviving a living completely restores one's sanity.",
            "some guns can be used in Rapid Fire mode, shooting twice in one turn.",
            // misc rules
        	"scents decay faster in the rain. undead track by scent.",
        	"firearms are more likely to jam in the rain.",
            "certain outfits change the chances to get attention from cops and gangs.",
            "trading and grouping with people are good for your sanity.",
            // followers
            "you can use cellphones to keep contact with your followers.",
            //"followers can help you push objects.",
            "followers can guard you while you sleep.",
            "followers can guard your place while you are away.",
            // misc
            "nowhere is truly safe, but barricades and fortifications can help.",
            "you can plant vegie seeds with a shovel and a clear patch of ground.",
            "most walls and doors can be demolished with C4 and dynamite.",
            "banks have safes, which provide a way to securely store your items.",
            "you might like to change the Animation Delay in the game options.",
            "empty food cans can be turned into an early-warning noise trap.",
            "someone rings church bells at sundown. Night is extra dangerous...",
            "you might like to turn on the Animation Delay in the options menu."
        };
    }
}
