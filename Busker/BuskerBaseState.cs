using UnityEngine;

namespace Busker 
{
    public class BuskerBaseState : BaseState<BuskerStates>
    {

        protected string animBool;
        protected string animName;
        protected int animLayer;



        protected BuskerStates nextState;



        // this is for caching the animation state info
        protected AnimatorStateInfo animStateInfo;
        // I will figure this out later
        public BuskerBaseState(string animName , string animBool, int animLayer , BuskerContextManager contextManager , BuskerStates state):base(state) 
        {
            this.animBool = animBool;
            this.animName = animName;
            this.animLayer = animLayer;
        
        }

        public override void ExitState()
        {
            throw new System.NotImplementedException();
        }

        public override BuskerStates NextState()
        {
            throw new System.NotImplementedException();
        }

        public override void StartState()
        {
            throw new System.NotImplementedException();
        }

        public override void UpdateState()
        {
            throw new System.NotImplementedException();
        }
    }


}
