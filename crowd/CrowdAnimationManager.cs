using UnityEngine;
using System.Collections.Generic;

public class CrowdAnimationManager : MonoBehaviour
{
    private List<CrowdMember> members = new List<CrowdMember>();

    private void Start()
    {
        GetCrowdMembers(members);
    }

    private void GetCrowdMembers(List<CrowdMember> members) 
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            CrowdMember member = this.transform.GetChild(i).GetComponent<CrowdMember>();
            if (member != null)
            {
                members.Add(member);
            }
            else
            {
                Debug.LogWarning($"Child at index {i} does not have a CrowdMember component.");
            }
        }
    }



    public void PlayRandomClipFromSet(AnimationSetSO clipSet)
    {
        foreach (var member in members)
        {
            var clip = clipSet.GetRandomClip();
            member.PlayClip(clip);
        }
    }

    public void Clear()
    {
        members.Clear();
    }
}
