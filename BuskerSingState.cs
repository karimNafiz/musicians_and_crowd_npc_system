using System.Runtime.CompilerServices;
using UnityEngine;

namespace Busker
{
    /// <summary>
    /// Singing state for the busker — plays an animation and audio.
    /// </summary>
    public class BuskerSingState : BuskerBaseState
    {
        private BuskerContextManager contextManager;



        // possible refactor in the future we can have seperate context manager for each layer
        public BuskerSingState(string animName , string animBool , int animLayer,   BuskerContextManager contextManager, BuskerStates state) : base(animName , animBool , animLayer , contextManager , state)
        {
 
        }

        public override void StartState()
        {
            // at the start make the next state urself
            this.nextState = this.stateKey;   
            contextManager.Animator.SetBool(animBool, true);

        }

        public override void UpdateState()
        {
            animStateInfo = this.contextManager.Animator.GetCurrentAnimatorStateInfo(this.animLayer);
            if (animStateInfo.IsName(this.animName) && animStateInfo.normalizedTime >= 1.0f) 
            {
                this.nextState = BuskerStates.Idle;
            
            }
        }

        public override void ExitState()
        {
            contextManager.Animator.SetBool(animBool, false);

        }

        public override BuskerStates NextState()
        {
            return this.nextState;
        }
    }
}
