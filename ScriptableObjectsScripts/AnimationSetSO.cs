using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "AnimationSetSO", menuName = "Scriptable Objects/AnimationSetSO")]
public class AnimationSetSO : ScriptableObject
{
    public List<AnimationClip> clips;

    public AnimationClip GetRandomClip() 
    {
        if(clips == null || clips.Count == 0)
        {
            Debug.LogWarning("No animation clips available in AnimationSetSO.");
            return null;
        }
        return clips[Random.Range(0, clips.Count)];

    }

}
