using UnityEngine;

namespace Busker 
{
    public class BuskerBaseState : BaseState<BuskerStates>
    {
        // I will figure this out later
        public BuskerBaseState(BuskerStates state):base(state) { }

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
