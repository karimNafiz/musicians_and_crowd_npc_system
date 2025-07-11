using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

[RequireComponent(typeof(Animator))]
public class TestClipPlayableCode : MonoBehaviour
{
    public AnimationClip animationClip;

    private PlayableGraph playableGraph;

    void Start()
    {
        if (animationClip == null)
        {
            Debug.LogError("AnimationClip not assigned!");
            return;
        }

        // Create the graph
        playableGraph = PlayableGraph.Create("SimpleAnimationPlayableGraph");
        playableGraph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);

        // Create output
        var output = AnimationPlayableOutput.Create(playableGraph, "AnimationOutput", GetComponent<Animator>());

        // Create the clip playable
        var clipPlayable = AnimationClipPlayable.Create(playableGraph, animationClip);

        // Connect the playable to the output
        output.SetSourcePlayable(clipPlayable);

        // Start the graph
        playableGraph.Play();
    }

    void OnDestroy()
    {
        // Always destroy the graph when done
        if (playableGraph.IsValid())
        {
            playableGraph.Destroy();
        }
    }
}
