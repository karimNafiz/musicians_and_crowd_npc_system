using UnityEngine;

namespace Busker
{
    /// <summary>
    /// Holds shared components used across busker states.
    /// </summary>
    public class BuskerContextManager
    {
        private Animator animator;
        public Animator Animator => animator;
       
        public BuskerContextManager(Animator animator)
        {
            this.animator = animator;
            
        }
    }
}
