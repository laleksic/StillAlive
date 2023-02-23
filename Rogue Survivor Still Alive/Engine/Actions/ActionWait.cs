using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using djack.RogueSurvivor.Data;

namespace djack.RogueSurvivor.Engine.Actions
{
    class ActionWait : ActorAction
    {
        #region Fields
        bool m_IsFishing;
        #endregion

        #region Init
        public ActionWait(Actor actor, RogueGame game, bool isFishing = false) //@@MP - added a flag for fishing (Release 7-6)
            : base(actor, game)
        {
            m_IsFishing = isFishing;
        }
        #endregion

        #region Implementation
        public override bool IsLegal()
        {
            return true;
        }

        public override void Perform()
        {
            m_Game.DoWait(m_Actor, m_IsFishing);
        }
        #endregion
    }
}
