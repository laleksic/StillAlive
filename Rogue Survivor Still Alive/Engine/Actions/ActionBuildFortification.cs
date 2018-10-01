using System.Drawing;

using djack.RogueSurvivor.Data;

namespace djack.RogueSurvivor.Engine.Actions
{
    class ActionBuildFortification : ActorAction
    {
        #region Fields
        Point m_BuildPos;
        bool m_IsLarge;
        Weather m_Weather; //@@MP - added weather parameter (Release 6-2)
        #endregion

        #region Init
        public ActionBuildFortification(Actor actor, RogueGame game, Point buildPos, bool isLarge, Weather weather) //@@MP - added weather parameter (Release 6-2)
            : base(actor, game)
        {
            m_BuildPos = buildPos;
            m_IsLarge = isLarge;
            m_Weather = weather;
        }
        #endregion

        #region ActorAction
        public override bool IsLegal()
        {
            return m_Game.Rules.CanActorBuildFortification(m_Actor, m_BuildPos, m_IsLarge, m_Weather); //@@MP - added weather parameter (Release 6-2)
        }

        public override void Perform()
        {
            m_Game.DoBuildFortification(m_Actor, m_BuildPos, m_IsLarge);
        }
        #endregion
    }
}
