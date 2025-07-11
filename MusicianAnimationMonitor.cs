using System;
using UnityEngine;

namespace Busker
{
    public enum MusicianAnimationState
    {
        GuitarSlow,
        GuitarFast,
        Bowing,
        RandomWalk,
        Idle
    }

    [RequireComponent(typeof(Animator))]
    public class MusicianAnimationMonitor : MonoBehaviour
    {
        public event EventHandler OnPerformanceStart;
        public event EventHandler OnPerformanceStop;
        public event EventHandler OnPerformanceEnd;
        public event EventHandler OnGuitarSlowStart;
        public event EventHandler OnGuitarSlowEnd;
        public event EventHandler OnGuitarFastStart;
        public event EventHandler OnGuitarFastEnd;
        public event EventHandler OnBowStart;
        public event EventHandler OnBowEnd;
        public event EventHandler OnRandomWalkStart;
        public event EventHandler OnRandomWalkEnd;

        [SerializeField] private int guitarSlowLoopCount = 0;
        [SerializeField] private int guitarFastLoopCount = 0;

        [SerializeField] private string guitarSlowToFastAnimBool;
        [SerializeField] private string guitarFastToBowAnimBool;
        [SerializeField] private string performanceStartAnimBool;

        private MusicianAnimationState currentState = MusicianAnimationState.Idle;

        private int currentGuitarSlowCount = 0;
        private int currentGuitarFastCount = 0;

        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void StartPerformance()
        {
            OnPerformanceStart?.Invoke(this, EventArgs.Empty);

            currentState = MusicianAnimationState.GuitarSlow;
            animator.SetBool(performanceStartAnimBool, true);
            currentGuitarSlowCount = 0;
            currentGuitarFastCount = 0;

            //animator.SetTrigger("StartGuitarSlow"); // Make sure this trigger exists in Animator
        }

        public void IsActive() 
        {
        
        
        }

        public void EndPerformance()
        {
            OnPerformanceEnd?.Invoke(this, EventArgs.Empty);
        }

        // Called by animation event at end of GuitarSlow loop
        public void TriggerGuitarSlowEnd()
        {
            //OnGuitarSlowEnd?.Invoke(this, EventArgs.Empty);
            Debug.Log("Entered the function TriggerGuitarSlowEnd)");
            currentGuitarSlowCount = currentGuitarSlowCount + 1; // i miss python xD
            // if the current count is below the loop count we simply return 
            Debug.Log($"Current Guitar Slow Count: {currentGuitarSlowCount}, Loop Count: {guitarSlowLoopCount}");
            if (currentGuitarSlowCount < guitarSlowLoopCount) return;

            // if current count is above or equal to the loop count
            // we trigger the event and transition to fast guitar
            OnGuitarSlowEnd?.Invoke(this, EventArgs.Empty);

            // need to restart the currentGuiterSlowCount
            currentGuitarSlowCount = 0;

            this.currentState = MusicianAnimationState.GuitarFast;
            animator.SetBool(guitarSlowToFastAnimBool, true); // transition to fast guitar
            Debug.Log("Guitar slow ended ");
        }

        public void TriggerGuitarSlowStart()
        {
            OnGuitarSlowStart?.Invoke(this, EventArgs.Empty);
            Debug.Log("Guitar Slow Started ");
        }

        // Called by animation event at end of GuitarFast loop
        public void TriggerGuitarFastEnd()
        {
            // we will follow the exact same logic as the function TriggerGuitarSlowEnd

            currentGuitarFastCount++;
            
            if (currentGuitarFastCount < guitarFastLoopCount) return;

            OnGuitarFastEnd?.Invoke(this, EventArgs.Empty);
            currentGuitarFastCount = 0;
            this.currentState = MusicianAnimationState.Bowing;
            animator.SetBool(guitarSlowToFastAnimBool, false); // reset the slow to fast transition
            animator.SetBool(guitarFastToBowAnimBool, true);
            Debug.Log("Guitar fast endedd");
        }

        public void TriggerGuitarFastStart()
        {
            OnGuitarFastStart?.Invoke(this, EventArgs.Empty);
            Debug.Log("Guitar Fast started ");
        }

        public void TriggerBowStart()
        {
            OnBowStart?.Invoke(this, EventArgs.Empty);
            Debug.Log("Bow Started");
        }

        public void TriggerBowEnd()
        {
            OnBowEnd?.Invoke(this, EventArgs.Empty);
            Debug.Log("Bow ended");
        }

        public void TriggerPerformanceStop()
        {
            OnPerformanceStop?.Invoke(this, EventArgs.Empty);
        }

        public void TriggerRandomWalkStart()
        {
            OnRandomWalkStart?.Invoke(this, EventArgs.Empty);
        }

        public void TriggerRandomWalkEnd()
        {
            OnRandomWalkEnd?.Invoke(this, EventArgs.Empty);
        }
    }
}
