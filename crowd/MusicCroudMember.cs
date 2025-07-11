using Busker;
using System.Collections.Generic;
using Spawn;
using UnityEngine;

public class MusicCroudMember : MonoBehaviour
{

    // this is our musician
    // we need to listen to its animation events 
    [SerializeField] private MusicianAnimationMonitor musicianAnimationMonitor;


    // the crowd members we are suppose manage
    private List<CroudMember> members = new();


    private CroudAnimationManager croudAnimationManager;    


    // the spawner
    // while we the music crowd manager is intializing
    // we need to randomize the position of the spawnable objects
    // in our case the spawnable objects are the crowd members
    // the crowd members implement Spawnable interface
    [SerializeField] private SpawnGeneric crowdSpawner;

    // These are the animation sets we need 
    // each set have collection of similar animations
    // instead of using the same animation 
    // we introduce some variability by randomly assigning animations from the set
    [SerializeField] private AnimationSetSO idleAnimationSetSO;
    [SerializeField] private AnimationSetSO listeningAnimationSetSO;
    [SerializeField] private AnimationSetSO clappingAnimationSetSO;




    void Start()
    {
        // i need to know when the slow guitar animation starts
        // so i can switch the crowd the listening animation 
        musicianAnimationMonitor.OnGuitarSlowStart += MusicianAnimationMonitor_OnGuitarSlowStart;
        
        // i need to know when the performance start
        musicianAnimationMonitor.OnPerformanceStart += MusicianAnimationMonitor_OnPerformanceStart;

        // i need to know when the performance ends
        musicianAnimationMonitor.OnPerformanceEnd += MusicianAnimationMonitor_OnPerformanceEnd;

        // need to know when the bow animation start
        musicianAnimationMonitor.OnBowStart += MusicianAnimationMonitor_OnBowStart;

        GetMusicCroud(members);
        // I am initializing them explicitely
        // need to initialize the members from the manager
        // to avoid synchronization issues
        // we can't tell for sure if the managers start will run after the start of all the members
        // to make code more readable 
        foreach (CroudMember member in members)
        {
            member.Initialize();
        }

        // need to initialize our crowd animation manager 
        croudAnimationManager = new CroudAnimationManager(members);

    }

    private void GetMusicCroud(List<CroudMember> members) 
    {
        if(members == null)
        {
            Debug.LogError("members list is null, please provide a valid list to store music crowd members.");
            return;
        }
        for(int i = 0; i < this.transform.childCount; i++)
        {
            var member = this.transform.GetChild(i).GetComponent<CroudMember>();
            if (member != null)
            {
                members.Add(member);
            }
            else
            {
                Debug.LogWarning($"Child at index {i} does not have a CroudMember component.");
            }
        }   
    }


    private void MusicianAnimationMonitor_OnBowStart(object sender, System.EventArgs e)
    {
        Debug.Log("the crowd should play the clapping animation ");
        croudAnimationManager.PlayRandomClipFromSet(clappingAnimationSetSO);
    }

    private void MusicianAnimationMonitor_OnGuitarSlowStart(object sender, System.EventArgs e)
    {
        Debug.Log("the crowd should play the listening animation ");
        croudAnimationManager.PlayRandomClipFromSet(listeningAnimationSetSO);
    }

    private void MusicianAnimationMonitor_OnPerformanceEnd(object sender, System.EventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void MusicianAnimationMonitor_OnPerformanceStart(object sender, System.EventArgs e)
    {
        Debug.Log("the musicians performance just started, need to play the idle animation ");
        crowdSpawner.RandomizeSpawanableInSpawnArea();
        // need the crow to perform the idle animation set 
        croudAnimationManager.PlayRandomClipFromSet(idleAnimationSetSO);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
