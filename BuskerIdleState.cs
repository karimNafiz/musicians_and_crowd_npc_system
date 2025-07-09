using UnityEngine;

namespace Busker
{
    /// <summary>
    /// Idle state for the busker — default non-performing behavior.
    /// </summary>
    public class BuskerIdleState : BuskerBaseState
    {
        private BuskerContextManager contextManager;

        public BuskerIdleState(string animName, string animBool, int animLayer, BuskerContextManager contextManager, BuskerStates state) : base(animName, animBool, animLayer, contextManager, state)
        {
            this.contextManager = contextManager;
        }

        public override void StartState()
        {
            contextManager.Animator.SetBool("isIdle", true);
        }

        public override void UpdateState()
        {
            // For now, remain in idle state.
        }

        public override void ExitState()
        {
            contextManager.Animator.SetBool("isIdle", false);
        }

        public override BuskerStates NextState()
        {
            return BuskerStates.Idle; // For now, stays in idle
        }
    }
}
