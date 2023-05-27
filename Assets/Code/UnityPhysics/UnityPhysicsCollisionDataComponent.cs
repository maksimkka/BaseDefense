using System.Collections.Generic;

namespace Code.UnityPhysics
{
    public struct UnityPhysicsCollisionDataComponent
    {
        public Queue<(int layer, UnityPhysicsCollisionDTO dto)> CollisionsEnter;
    }
}