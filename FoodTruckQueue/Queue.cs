using UnityEngine;
using System.Collections.Generic;
using System;
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
        private bool isQueueMoving = false;
        private bool isQueueHeadInAction = false;
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
            // we get all the queue participants 
            GetQueueParticipants();
            // listen to the head of the queue -> currentParticipant
            if (currentParticipant == null) 
            {
                Debug.LogWarning("Current participant is null, cannot start queue.");
                return;
            }
            ListenToQueueParticipant(currentParticipant);
            MoveQueue();


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
        void ListenToQueueParticipant(IQueueParticipant queueParticipant) 
        {
            
            queueParticipant.OnActionStart += QueueParticipant_OnActionStart;
            queueParticipant.OnActionEnd += QueueParticipant_OnActionEnd;
            queueParticipant.OnActionPointReached += QueueParticipant_OnActionPointReached;

        }
        void UnlistenToQueueParticipant(IQueueParticipant queueParticipant)
        {
            queueParticipant.OnActionStart -= QueueParticipant_OnActionStart;
            queueParticipant.OnActionEnd -= QueueParticipant_OnActionEnd;
            queueParticipant.OnActionPointReached -= QueueParticipant_OnActionPointReached;
        }

        private void QueueParticipant_OnActionPointReached(object sender, EventArgs e)
        {
            if (isQueueMoving) 
            {
                // need to stop the queue
                StopQueue();
            }
            if (isQueueHeadInAction) return;
            isQueueHeadInAction = true;
            this.currentParticipant.StartAction();
        }


        private void QueueParticipant_OnActionStart(object sender, EventArgs e) 
        {
            // currently do not know what to do 
        }

        private void QueueParticipant_OnActionEnd(object sender, EventArgs e) 
        {
            this.currentParticipant.EndAction();
            isQueueHeadInAction = false;
            // we need to change the current head of the queue
            // before changing the head of the queue
            // we unsubsribe from the current participant events
            UnlistenToQueueParticipant(currentParticipant);
            // we then need to change teh head of the queue
            ChangeHeadOfQueue();
            // we then need to listen to the new head of the queue
            ListenToQueueParticipant(currentParticipant);


        }

        private void ChangeHeadOfQueue() 
        {
            if (IsQueueEmpty()) return;
            
            queueParticipants.RemoveAt(queueParticipants.Count - 1);
            this.currentParticipant = queueParticipants[queueParticipants.Count - 1];

        }
        private bool  IsQueueEmpty() 
        {
            return queueParticipants.Count == 0;

        }

        /// <summary>
        /// this function will be used to move the queue
        /// when the head of the queue has finished their action
        /// </summary>
        void MoveQueue() 
        {
            if (isQueueMoving) return;
            isQueueMoving = true;
            foreach (IQueueParticipant participant in queueParticipants)
            {
                participant.Move();
            }

        }

        /// <summary>
        /// this function will be used to stop the queue
        /// when the head of the queue has reached the action point 
        /// </summary>
        void StopQueue() 
        {
            if(!isQueueMoving) return;
            isQueueMoving = false;
            foreach (IQueueParticipant participant in queueParticipants)
            {
                participant.Stop();
            }   

        }

    }


}
