using System;
using System.Collections.Generic;
using System.Text;

using djack.RogueSurvivor.Data;
using djack.RogueSurvivor.Engine.MapObjects;

namespace djack.RogueSurvivor.Engine.Actions
{
    class ActionBarricadeDoor : ActorAction
    {
        #region Fields
        DoorWindow m_Door;
        Weather m_Weather;  //@@MP - added weather parameter (Release 6-2)
        #endregion

        #region Init
        public ActionBarricadeDoor(Actor actor, RogueGame game, DoorWindow door, Weather weather) //@@MP - added weather parameter (Release 6-2)
            : base(actor, game)
        {
            if (door == null)
                throw new ArgumentNullException("door");

            m_Door = door;
            m_Weather = weather; //@@MP - added weather parameter (Release 6-2)
        }
        #endregion

        #region ActorAction
        public override bool IsLegal()
        {
            return m_Game.Rules.CanActorBarricadeDoor(m_Actor, m_Door, m_Weather, out m_FailReason); //@@MP - added weather parameter (Release 6-2)
        }

        public override void Perform()
        {
            m_Game.DoBarricadeDoor(m_Actor, m_Door);
        }
        #endregion
    }
}
