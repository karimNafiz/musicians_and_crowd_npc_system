using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Utility;

[RequireComponent(typeof(Collider))]
public class QueueParticipant : MonoBehaviour, IQueueParticipant
{
    [Header("References")]
    [SerializeField] private TwoBoneIKConstraint rightHandIkConstraint;
    [SerializeField] private Animator animator;

    [Header("Animator Parameters")]
    [SerializeField] private string isMoveAnimBool = "IsMoving";
    [SerializeField] private string isActionAnimBool = "IsInAction";

    [Header("IK Settings")]
    [SerializeField] private float ikWeightChangeSpeed = 1f; // units per second

    [SerializeField] private float actionDuration = 0.25f;

    public event EventHandler OnActionStart;
    public event EventHandler OnActionEnd;
    public event EventHandler OnActionPointReached;

    private bool isInAction = false;
    private bool isMoving = false;
    private Coroutine ikRoutine;

    // need this to check collision with FoodTruckAction script's gameobject
    [SerializeField] private LocationReachedTrigger locationReachedTrigger;

    // TODO: need to refactor the position of listening to the event
    public void Start()
    {
        locationReachedTrigger.OnLocationReached += LocationReachedTrigger_OnLocationReached;
    }

    private void LocationReachedTrigger_OnLocationReached(object sender, Collider e)
    {
        Debug.Log($"Location reached by: {e.gameObject.name} ----------------------------------------------");

        if (!e.gameObject == this.gameObject) return;
        NotifyLocationReached();
    }

    public void NotifyLocationReached()
    {
        OnActionPointReached?.Invoke(this, EventArgs.Empty);
        StartAction(); // this will be ignored if already in action
    }
    public void SetRightHandIKTarget(Transform target) 
    {
        this.rightHandIkConstraint.data.target = target;

    }


    public void StartAction()
    {
        if (isInAction) return;
        isInAction = true;
        //animator.SetBool(isActionAnimBool, true);
        animator.SetBool(isMoveAnimBool, false);
        OnActionStart?.Invoke(this, EventArgs.Empty);

        // Start IK weight increase and decrease coroutine
        if (ikRoutine != null)
            StopCoroutine(ikRoutine);
        ikRoutine = StartCoroutine(PerformPickUpAction());
    }

    public void EndAction()
    {
        if (!isInAction) return;

        isInAction = false;
        //animator.SetBool(isActionAnimBool, false);
        OnActionEnd?.Invoke(this, EventArgs.Empty);
    }

    public void Move()
    {
        if (isMoving) return;
        isMoving = true;
        animator.SetBool(isMoveAnimBool, true);
    }

    public void Stop()
    {
        if (!isMoving) return;
        isMoving = false;
        animator.SetBool(isMoveAnimBool, false);
    }

    private IEnumerator PerformPickUpAction()
    {
        // Increase weight to 1
        yield return StartCoroutine(BlendIKWeight(0f, 1f));

        // Optional: hold for a short duration
        yield return new WaitForSeconds(actionDuration);

        // Decrease weight to 0
        yield return StartCoroutine(BlendIKWeight(1f, 0f));

        EndAction(); // Finish action after animation
    }

    private IEnumerator BlendIKWeight(float start, float end)
    {
        float elapsed = 0f;
        while (!Mathf.Approximately(rightHandIkConstraint.weight, end))
        {
            elapsed += Time.deltaTime * ikWeightChangeSpeed;
            rightHandIkConstraint.weight = Mathf.Lerp(start, end, elapsed);
            yield return null;
        }
        rightHandIkConstraint.weight = end;
    }

    private void Reset()
    {
        // Auto-assign Animator if missing
        if (animator == null)
            animator = GetComponent<Animator>();
    }






}
