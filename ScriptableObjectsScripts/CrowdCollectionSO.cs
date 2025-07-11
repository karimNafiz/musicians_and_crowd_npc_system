using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CrowdCollectionSO", menuName = "Scriptable Objects/CrowdCollectionSO")]
public class CrowdCollectionSO : ScriptableObject
{
    public List<CrowdMember> crowdMembers; 
}
