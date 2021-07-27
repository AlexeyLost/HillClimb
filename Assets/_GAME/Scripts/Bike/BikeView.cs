using GAME.Managers;
using UnityEngine;

namespace GAME.Bikes
{
    public class BikeView : MonoBehaviour
    {
        public WheelJoint2D backWheelJoint;
        public WheelJoint2D forwardWheelJoint;
        public Transform bodyTransform;
        public CollisionCatcher headTrigger;
        public CollisionCatcher forwardWheelTrigger;
    }
}