using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace djack.RogueSurvivor.Data
{
    [Serializable]
    enum Activity
    {
        /// <summary>
        /// Doing nothing in particular.
        /// </summary>
        IDLE,

        /// <summary>
        /// Chasing an enemy.
        /// </summary>
        CHASING,

        /// <summary>
        /// Fighting an enemy.
        /// </summary>
        FIGHTING,

        /// <summary>
        /// Following a track.
        /// </summary>
        TRACKING,

        /// <summary>
        /// Fleeing from a danger/enemy.
        /// </summary>
        FLEEING,

        /// <summary>
        /// Following another actor.
        /// </summary>
        FOLLOWING,

        /// <summary>
        /// zzzZZZzzz...
        /// </summary>
        SLEEPING,

        /// <summary>
        /// Following an order.
        /// </summary>
        FOLLOWING_ORDER,

        /// <summary>
        /// Fleeing from a primed explosive.
        /// </summary>
        FLEEING_FROM_EXPLOSIVE,

        //@@MP (Release 6-6)
        /// <summary>
        /// Food
        /// </summary>
        EATING,

        /// <summary>
        /// 
        /// </summary>
        EXPLORING,

        /// <summary>
        /// 
        /// </summary>
        WANDERING,

        /// <summary>
        /// Trapped in the dark
        /// </summary>
        FINDING_EXIT,

        /// <summary>
        /// Using medical item
        /// </summary>
        HEALING,

        /// <summary>
        /// 
        /// </summary>
        WAITING,

        /// <summary>
        /// 
        /// </summary>
        PATROLLING,

        /// <summary>
        /// 
        /// </summary>
        CHATTING,

        /// <summary>
        /// Reviving a corpse
        /// </summary>
        REVIVING,

        /// <summary>
        /// 
        /// </summary>
        BUILDING,

        /// <summary>
        /// Dropping or picking up items
        /// </summary>
        MANAGING_INVENTORY,

        /// <summary>
        /// With another actor
        /// </summary>
        TRADING,

        /// <summary>
        /// For food
        /// </summary>
        SEARCHING,

        /// <summary>
        /// 
        /// </summary>
        SHOUTING,

        /// <summary>
        /// eg teaeing down barricades
        /// </summary>
        DESTROYING,

        /// <summary>
        /// Stamina
        /// </summary>
        RESTING
    }
}
