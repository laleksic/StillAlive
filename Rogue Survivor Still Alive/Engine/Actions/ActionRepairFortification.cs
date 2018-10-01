using System;
using System.Collections.Generic;
using System.Text;

using djack.RogueSurvivor.Data;
using djack.RogueSurvivor.Engine.MapObjects;

namespace djack.RogueSurvivor.Engine.Actions
{
    class ActionRepairFortification : ActorAction
    {
        #region Fields
        Fortification m_Fort;
        Weather m_Weather; //@@MP - added weather parameter (Release 6-2)
        #endregion

        #region Init
        public ActionRepairFortification(Actor actor, RogueGame game, Fortification fort, Weather weather) //@@MP - added weather parameter (Release 6-2)
            : base(actor, game)
        {
            if (fort == null)
                throw new ArgumentNullException("fort");

            m_Fort = fort;
            m_Weather = weather;
        }
        #endregion

        #region ActorAction
        public override bool IsLegal()
        {
            return m_Game.Rules.CanActorRepairFortification(m_Actor, m_Weather, out m_FailReason); //@@MP - unused parameter (Release 5-7)
        }

        public override void Perform()
        {
            m_Game.DoRepairFortification(m_Actor, m_Fort);
        }
        #endregion
    }
}
