using System.Collections.Generic;
using UnityEngine;

namespace Code.UnityPhysics
{
    public struct UnityPhysicsCollisionDataComponent
    {
        /// <summary>
        /// All currently unhandled collisions. Layer of other collider's gameObject is used as a key.  
        /// </summary>
        public Queue<(int layer, UnityPhysicsCollisionDTO dto)> CollisionsEnter;

        /// <summary>
        /// All GameObjects this entity is currently colliding. Other GameObject is used as a key. 
        /// </summary>
        public Dictionary<GameObject, UnityPhysicsCollisionDTO> CollisionsStay;
    }
}