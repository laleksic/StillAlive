using System;
using System.Drawing;
using djack.RogueSurvivor.Data;

namespace djack.RogueSurvivor.Engine.Tasks
{
    [Serializable]
    class TaskAddMapObject : TimedTask   //@@MP (Release 7-6)
    {
        private int m_X, m_Y;
        private MapObject m_mapObj;
        private bool m_overwriteMapObj;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="turns"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="mapObj">the object you want added</param>
        /// <param name="originalMapObj">can't have two mapobj on the same tile, so this can overwrite any mapobj already there</param>
        public TaskAddMapObject(int turns, int x, int y, MapObject mapObj, bool overwriteMapObj = false)
            : base(turns)
        {
            m_X = x;
            m_Y = y;
            m_mapObj = mapObj;
            m_overwriteMapObj = overwriteMapObj;
        }

        public override void Trigger(Map m)
        {
            Point pt = new Point(m_X, m_Y);

            // first, check for blockers
            if (!m.IsInBounds(pt))
                return; //just an unecessary precaution
            MapObject existingMapObj = m.GetMapObjectAt(pt);
            if (existingMapObj != null && !m_overwriteMapObj)
                return;
            Actor actor = m.GetActorAt(pt);
            if (actor != null)
                return;

            // delete what had already been there, if relevant.
            // we can't have two mapobj on the same tile.
            if (existingMapObj != null)
                m.RemoveMapObjectAt(pt.X, pt.Y);

            // now add the one we want
            m.PlaceMapObjectAt(m_mapObj, pt);
        }
    }
}
