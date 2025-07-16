using UnityEngine;
using System.Collections.Generic;
using System;

namespace FoodTruck.Queue
{
    public class Queue : MonoBehaviour
    {
        // maybe have an SO of queue participants
        // SO stores ququeue participants 
        // and when we want to refill the queue
        // we can spawn at the start of the queue
        // TODO:
        // 





        private List<IQueueParticipant> queueParticipants;
        private IQueueParticipant currentParticipant;

        private bool isQueueMoving = false;
        private bool isQueueHeadInAction = false;
        private bool isActive = false;

        public bool IsActive => isActive;

        void Start()
        {
            // StartQueue could be triggered here or from external script
            StartQueue();
        }

        void Update()
        {
            // Optional: trigger-based logic can go here
        }

        /// <summary>
        /// Initializes and starts the queue.
        /// </summary>
        void StartQueue()
        {
            queueParticipants = new List<IQueueParticipant>();
            GetQueueParticipants();

            if (IsQueueEmpty())
            {
                Debug.LogWarning("Queue is empty. Cannot start.");
                isActive = false;
                return;
            }

            currentParticipant = queueParticipants[queueParticipants.Count - 1];
            ListenToQueueParticipant(currentParticipant);
            isActive = true;
            MoveQueue();
        }

        /// <summary>
        /// Gathers all IQueueParticipant children under this object.
        /// </summary>
        void GetQueueParticipants()
        {
            if (queueParticipants == null)
            {
                Debug.LogError("Queue participants list is null.");
                return;
            }

            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).TryGetComponent<IQueueParticipant>(out IQueueParticipant participant))
                {
                    queueParticipants.Add(participant);
                }
            }

            if (IsQueueEmpty())
            {
                Debug.LogWarning("No valid queue participants found.");
            }
        }

        /// <summary>
        /// Begins listening to the specified queue participant's events.
        /// </summary>
        void ListenToQueueParticipant(IQueueParticipant participant)
        {
            participant.OnActionStart += QueueParticipant_OnActionStart;
            participant.OnActionEnd += QueueParticipant_OnActionEnd;
            participant.OnActionPointReached += QueueParticipant_OnActionPointReached;
        }

        /// <summary>
        /// Stops listening to the specified queue participant's events.
        /// </summary>
        void UnlistenToQueueParticipant(IQueueParticipant participant)
        {
            participant.OnActionStart -= QueueParticipant_OnActionStart;
            participant.OnActionEnd -= QueueParticipant_OnActionEnd;
            participant.OnActionPointReached -= QueueParticipant_OnActionPointReached;
        }

        private void QueueParticipant_OnActionPointReached(object sender, EventArgs e)
        {
            if (!isActive) return;

            if (isQueueMoving)
                StopQueue();

            if (isQueueHeadInAction) return;

            isQueueHeadInAction = true;
            // the queue's sole responsiblity is to manage the queue 
            // so I have decided to no call the .StartAction() function from here
            //currentParticipant.StartAction();
        }

        private void QueueParticipant_OnActionStart(object sender, EventArgs e)
        {
            // Optionally do something when action starts
        }

        private void QueueParticipant_OnActionEnd(object sender, EventArgs e)
        {
            if (!isActive) return;

            currentParticipant.EndAction();
            isQueueHeadInAction = false;

            UnlistenToQueueParticipant(currentParticipant);
            ChangeHeadOfQueue();

            if (!isActive) return; // queue might be empty now

            ListenToQueueParticipant(currentParticipant);
            MoveQueue();
        }

        /// <summary>
        /// Safely advances the queue to the next head.
        /// </summary>
        private void ChangeHeadOfQueue()
        {
            if (IsQueueEmpty())
            {
                isActive = false;
                currentParticipant = null;
                Debug.Log("Queue is now empty.");
                return;
            }

            queueParticipants.RemoveAt(queueParticipants.Count - 1);

            if (IsQueueEmpty())
            {
                isActive = false;
                currentParticipant = null;
                Debug.Log("Queue is now empty after removing participant.");
                return;
            }

            currentParticipant = queueParticipants[queueParticipants.Count - 1];
        }

        /// <summary>
        /// Whether the queue has no participants left.
        /// </summary>
        private bool IsQueueEmpty()
        {
            return queueParticipants == null || queueParticipants.Count == 0;
        }

        /// <summary>
        /// Starts moving all participants in the queue.
        /// </summary>
        void MoveQueue()
        {
            if (isQueueMoving || !isActive || IsQueueEmpty()) return;

            isQueueMoving = true;
            foreach (IQueueParticipant participant in queueParticipants)
            {
                participant.Move();
            }
        }

        /// <summary>
        /// Stops all participants in the queue.
        /// </summary>
        void StopQueue()
        {
            if (!isQueueMoving || !isActive || IsQueueEmpty()) return;

            isQueueMoving = false;
            foreach (IQueueParticipant participant in queueParticipants)
            {
                participant.Stop();
            }
        }
    }
}
