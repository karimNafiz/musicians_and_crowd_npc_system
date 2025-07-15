using Spawn;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;


public class CroudMember : MonoBehaviour, Spawnable, IInitializable
{
    [SerializeField] private Animator animator;
    [SerializeField] private float blendDuration = 0.25f;

    private PlayableGraph graph;
    private AnimationMixerPlayable mixer;
    private AnimationPlayableOutput output;
    private AnimationClipPlayable mainPlayable;
    private AnimationClipPlayable oneShotPlayable;

    private float blendWeight = 0f;
    private bool isBlending = false;
    private float blendTimer = 0f;

    private bool returningToMain = false;

    private void Awake()
    {
        if (animator == null)
        {
            Debug.LogWarning("Animator is null for the crowd member");
            return;
        }

        graph = PlayableGraph.Create($"{name}_Graph");
        graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);

        output = AnimationPlayableOutput.Create(graph, "Animation", animator);

        // Create dummy playables to initialize
        mainPlayable = AnimationClipPlayable.Create(graph, null);
        mainPlayable.SetApplyFootIK(true);
        //mainPlayable.SetDuration(double.MaxValue);

        oneShotPlayable = AnimationClipPlayable.Create(graph, null);
        oneShotPlayable.SetApplyFootIK(true);

        mixer = AnimationMixerPlayable.Create(graph, 2, true);
        mixer.ConnectInput(0, mainPlayable, 0);
        mixer.ConnectInput(1, oneShotPlayable, 0);
        mixer.SetInputWeight(0, 1f);
        mixer.SetInputWeight(1, 0f);

        output.SetSourcePlayable(mixer);
        graph.Play();
    }

    public void PlayMainLoop(AnimationClip clip)
    {
        if (!clip || !graph.IsValid()) return;

        mainPlayable.Destroy(); // destroy previous if exists
        mainPlayable = AnimationClipPlayable.Create(graph, clip);
        mainPlayable.SetApplyFootIK(true);
        //mainPlayable.SetLoopTime(true);

        mixer.DisconnectInput(0);
        mixer.ConnectInput(0, mainPlayable, 0);
        mixer.SetInputWeight(0, 1f);
        mixer.SetInputWeight(1, 0f);
    }

    public void PlayOneShot(AnimationClip clip)
    {
        if (!clip || !graph.IsValid()) return;

        oneShotPlayable.Destroy();
        oneShotPlayable = AnimationClipPlayable.Create(graph, clip);
        oneShotPlayable.SetApplyFootIK(true);
        //oneShotPlayable.SetLoopTime(false);

        mixer.DisconnectInput(1);
        mixer.ConnectInput(1, oneShotPlayable, 0);

        blendWeight = 0f;
        blendTimer = 0f;
        isBlending = true;
        returningToMain = false;

        oneShotPlayable.SetTime(0);
        oneShotPlayable.SetDuration(clip.length);
    }

    private void Update()
    {
        if (!graph.IsValid() || !isBlending) return;

        blendTimer += Time.deltaTime;
        float t = Mathf.Clamp01(blendTimer / blendDuration);

        if (!returningToMain)
        {
            // Blending into one-shot
            blendWeight = t;
            mixer.SetInputWeight(0, 1f - blendWeight);
            mixer.SetInputWeight(1, blendWeight);

            if (t >= 1f && oneShotPlayable.GetTime() >= oneShotPlayable.GetAnimationClip().length)
            {
                // Time to return to main
                returningToMain = true;
                blendTimer = 0f;
            }
        }
        else
        {
            // Blending back to main
            blendWeight = 1f - t;
            mixer.SetInputWeight(0, 1f - blendWeight);
            mixer.SetInputWeight(1, blendWeight);

            if (t >= 1f)
            {
                isBlending = false;
                returningToMain = false;
                mixer.SetInputWeight(0, 1f);
                mixer.SetInputWeight(1, 0f);
            }
        }
    }

    private void OnDestroy()
    {
        if (graph.IsValid())
            graph.Destroy();
    }

    public Spawnable Spawn(Vector3 position)
    {
        this.transform.position = position;
        this.gameObject.SetActive(true);
        return this;
    }

    public Spawnable Despawn()
    {
        this.gameObject.SetActive(false);
        return this;
    }

    public void Initialize()
    {
        // changing this for testing purposes
        this.SetActive(true);
    }

    // we just check if the flag incoming is different than the current state 
    // then 
    public void SetActive(bool flag) 
    {
        if (!(flag & this.gameObject.activeSelf)) 
        {
            this.gameObject.SetActive(flag);
        }
    }
}
