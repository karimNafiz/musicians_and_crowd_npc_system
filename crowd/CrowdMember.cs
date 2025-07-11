using Spawn;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

public class CrowdMember : MonoBehaviour, Spawnable
{
    private PlayableGraph graph;
    private AnimationPlayableOutput output;
    private AnimationClipPlayable currentPlayable;
    [SerializeField] private Animator animator;

    private void Awake()
    {
        if (animator == null) 
        {
            Debug.LogWarning("animator is null for the crow member");
            return;
        }
        graph = PlayableGraph.Create();
        output = AnimationPlayableOutput.Create(graph, "Animation", animator);
    }
    // this is temporary solution
    // hiding the player in start
    private void Start()
    {
        this.gameObject.SetActive(false);
    }

    public void PlayClip(AnimationClip clip)
    {
        if (!clip) return;
        if (graph.IsValid()) graph.Stop();

        currentPlayable = AnimationClipPlayable.Create(graph, clip);
        output.SetSourcePlayable(currentPlayable);
        graph.Play();
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
}
