using UnityEngine;
using System.Collections.Generic;
namespace FoodTruck.Queue 
{
    public class Queue : MonoBehaviour
    {
        private List<IQueueParticipant> queueParticipants;

        /// <summary>
        /// the current participant will represent the head of the queue
        /// to optimize memory and performance, I will use the end of the queueParticipant as the head of the queue
        /// to make the queue work properly, the last children needs to be the head of the queue
        /// </summary>

        private IQueueParticipant currentParticipant;
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        /// <summary>
        /// This function will be used to start the queue
        /// </summary>
        void StartQueue() 
        {
            queueParticipants = new List<IQueueParticipant>();
            GetQueueParticipants();

        }

        /// <summary>
        /// we use this function to get the queue participants
        /// this function expects a reference to the list of queue participants
        /// </summary>
        void GetQueueParticipants() 
        {
            if(queueParticipants == null)
            {
                Debug.LogError("Queue participants list is null.");
                return;
            }
            for (int i = 0; i < this.transform.childCount; i++) 
            {
                if (this.transform.GetChild(i).TryGetComponent<IQueueParticipant>(out IQueueParticipant participant)) 
                {
                    queueParticipants.Add(participant);

                }
            
            }
            // assigning the head of the queue to the last participant in the list 

            this.currentParticipant = queueParticipants[queueParticipants.Count - 1];

        }




        /// <summary>
        /// this function will be used to move the queue
        /// when the head of the queue has finished their action
        /// </summary>
        void MoveQueue() 
        { }

        /// <summary>
        /// this function will be used to stop the queue
        /// when the head of the queue has reached the action point 
        /// </summary>
        void StopQueue() 
        {}

    }


}
