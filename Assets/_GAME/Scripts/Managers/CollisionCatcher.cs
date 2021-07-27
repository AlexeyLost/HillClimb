using System;
using UnityEngine;

namespace GAME.Managers
{
    public class CollisionCatcher : MonoBehaviour
    {
        public event Action<Collider2D> TriggerEnter;
        public event Action<Collider2D> TriggerExit;
        public event Action<Collision2D> CollisionEnter; 
        public event Action<Collision2D> CollisionExit; 

        private void OnTriggerEnter2D(Collider2D other) => TriggerEnter?.Invoke(other);
        private void OnTriggerExit2D(Collider2D other) => TriggerExit?.Invoke(other);

        private void OnCollisionEnter2D(Collision2D other) => CollisionEnter?.Invoke(other);
        private void OnCollisionExit2D(Collision2D other) => CollisionExit?.Invoke(other);
    }
}