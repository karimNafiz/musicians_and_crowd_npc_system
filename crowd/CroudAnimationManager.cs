using UnityEngine;
using System.Collections.Generic;


/// <summary>
/// this class will manage the crowd animations
/// it would be better if we can make this generic 
/// we can have a interface call IClipPlayable ->
/// and inheritors of the IClipPlayable must implment the PlayClip method
/// this approach will greately make this class more generic and re-usable
/// but right now I just want to get the crowd animations working
/// </summary>


public class CroudAnimationManager 
{
    private readonly List<CroudMember> members;


    // important note: in this constructor we are taking a reference to a list of CroudMembers
    // any changes made to the list will reflect in the original list 
    // be extra careful with what we do in the list
    // thats why to constrain the user of this class im changing the members variable to only readonly 
    public CroudAnimationManager(List<CroudMember> members) 
    {

        this.members = members;
        
    }

    public void PlayMainLoop(AnimationSetSO clipSet)
    {
        foreach (CroudMember member in members) 
        {
            AnimationClip clip = clipSet.GetRandomClip();
            member.PlayMainLoop(clip);
        }
    
    }

    public void PlayOneShot(AnimationSetSO clipSet)
    {
        foreach (CroudMember member in members)
        {
            AnimationClip clip = clipSet.GetRandomClip();
            member.PlayOneShot(clip);
        }
    }

    public void Clear()
    {
        members.Clear();
    }
}
