using UnityEngine;
using Utility;


public class FoodTruckAction : LocationReachedTrigger
{
    


    protected override void OnTriggerEnter(Collider other)
    {
        Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!Trigger entered");
        base.OnTriggerEnter(other);

        if (IsEntered) 
        {
            if (other.TryGetComponent<IQueueParticipant>(out IQueueParticipant queueParticipant)) 
            {
                
                queueParticipant.NotifyLocationReached();

            }
        
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        
    }



}
