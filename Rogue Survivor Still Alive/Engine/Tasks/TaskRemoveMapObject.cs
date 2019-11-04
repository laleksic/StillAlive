using System;
using System.Drawing;
using djack.RogueSurvivor.Data;

namespace djack.RogueSurvivor.Engine.Tasks
{
    [Serializable]
    class TaskRemoveMapObject : TimedTask   //@@MP (Release 7-2)
    {
        private int m_X, m_Y;
        private string m_temporaryImageID;
        private MapObject m_originalMapObj;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="turns"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="temporaryImageID">the imageID of the temporary mapobject</param>
        /// <param name="originalMapObj">can't have two mapobj on the same tile, so could temporarily remove the mapobj that is there</param>
        public TaskRemoveMapObject(int turns, int x, int y, string temporaryImageID, MapObject originalMapObj = null)
            : base(turns)
        {
            m_X = x;
            m_Y = y;
            m_temporaryImageID = temporaryImageID;
            m_originalMapObj = originalMapObj;
        }

        public override void Trigger(Map m)
        {
            // first remove the one we want gone
            MapObject mapObj = m.GetMapObjectAt(m_X, m_Y);
            if (mapObj != null && mapObj.ImageID == m_temporaryImageID)
                m.RemoveMapObjectAt(m_X, m_Y);

            // now restore what had already been there, if relevant.
            // we can't have two mapobj on the same tile, so we had to temporarily remove the mapobj that was there
            if (m_originalMapObj != null)
            {
                Point pt = new Point(m_X, m_Y);
                m.PlaceMapObjectAt(m_originalMapObj, pt);
            }
        }
    }
}
