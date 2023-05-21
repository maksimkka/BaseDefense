using System.Collections.Generic;
using UnityEngine;

namespace Code.UnityPhysics
{
    public struct UnityPhysicsCollisionDataComponent
    {
        public Queue<(int layer, UnityPhysicsCollisionDTO dto)> CollisionsEnter;
    }
}