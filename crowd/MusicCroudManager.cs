using Busker;
using System.Collections.Generic;
using System.Collections;
using Spawn;
using UnityEngine;
using System;

public class MusicCroudManager : MonoBehaviour
{

    // this is our musician 
    // we listen to events from the musician
    [SerializeField] private MusicianAnimationMonitor musicianAnimationMonitor;

    // the crowd members we are suppose manage
    private List<CroudMember> members = new();

    // random spawner
    // the spawner deals with ISpawner interface
    // it will store references to the children
    [SerializeField] private SpawnGeneric crowdSpawner;

    // animation sets we need
    // when the musician is not performing we need to randomly assign the crowd members from the idle animation set
    // when the musician is performing we need to assign the listening animation set 
    // when the musician is bowing we need to assign from the cheering/clapping animation set
    [SerializeField] private AnimationSetSO idleAnimationSetSO;
    [SerializeField] private AnimationSetSO listeningAnimationSetSO;
    [SerializeField] private AnimationSetSO clappingAnimationSetSO;

    // structs are created within in the stack and not allocated on the heap 
    // we need the structs to pass information to coroutines about delays
    private struct delay 
    {
        public bool isDelay;
        public float minDelay;
        public float maxDelay;
    
    }

    // flag to keep track of the state 
    // if the music crowd manager is active or not 
    private bool isActive = false;


    void Start()
    {
        /// <summary>
        /// listening to musician events
        /// </summary>

        musicianAnimationMonitor.OnMusicianActive += MusicianAnimationMonitor_OnMusicianActive;

        musicianAnimationMonitor.OnMusicianInactive += MusicianAnimationMonitor_OnMusicianInactive;

        // i need to know when the slow guitar animation starts
        // so i can switch the crowd the listening animation 
        musicianAnimationMonitor.OnGuitarSlowStart += MusicianAnimationMonitor_OnGuitarSlowStart;
        
        // i need to know when the performance start
        musicianAnimationMonitor.OnPerformanceStart += MusicianAnimationMonitor_OnPerformanceStart;

        // i need to know when the performance ends
        musicianAnimationMonitor.OnPerformanceEnd += MusicianAnimationMonitor_OnPerformanceEnd;

        // need to know when the bow animation start
        musicianAnimationMonitor.OnBowStart += MusicianAnimationMonitor_OnBowStart;

        // getting hold of the children
        GetMusicCroud(members);

        // I am initializing them explicitely
        // need to initialize the members from the manager
        // to avoid synchronization issues
        // we can't tell for sure if the managers start will run after the start of all the members
        // to make code more readable 

        // right now the Initialize function isn't doing much 
        foreach (CroudMember member in members)
        {
            member.Initialize();
        }

        // need to initialize our crowd animation manager 
        //croudAnimationManager = new CroudAnimationManager(members);

    }

    private void MusicianAnimationMonitor_OnMusicianInactive(object sender, EventArgs e)
    {
        // if the music crowd manager is not active
        // then we simply return
        if(!isActive) return;
        foreach(CroudMember member in members)
        {
            member.SetActive(false);
        }   
    }

    private void MusicianAnimationMonitor_OnMusicianActive(object sender, System.EventArgs e)
    {
        // if the music crowd manager is active 
        // then we do not want to randomize the spawnable objects again
        // no need to change animation states 
        if (isActive) return;
        isActive = true;
        foreach (CroudMember member in members)
        {
            member.SetActive(true);
        }
        crowdSpawner.RandomizeSpawanableInSpawnArea();
        // need the crow to perform the idle animation set 
        PlayMainLoop(new delay()
        {
            isDelay = false,

        },
        idleAnimationSetSO);
        
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
        //Debug.Log("the crowd should play the clapping animation ");
        // first I am going to set the main loop 
        // this will ensure, after the clapping animation which is an one shot animation 
        // we go back to playing the main loop animation 
        // the crowd members are designed that way
        PlayMainLoop(new delay() 
        {
            isDelay = false,
        }, idleAnimationSetSO);
        // then im gonna play the on shot clapping animation
        PlayOneShot(new delay() 
        {
            isDelay = true,
            minDelay = 0.0f,
            maxDelay = 0.7f,
        },clappingAnimationSetSO);

        // this will make sure when the one shot is complete the playable graph will continue playing the main loop
        // we don't have to listen to any other events to go back to the main loop
    
    }

    private void MusicianAnimationMonitor_OnGuitarSlowStart(object sender, System.EventArgs e)
    {
    }

    private void MusicianAnimationMonitor_OnPerformanceEnd(object sender, System.EventArgs e)
    {
        
    }

    // personal preference, if we want to play the main listening loop in the performance start
    // or OnGuitarSlowStart
    private void MusicianAnimationMonitor_OnPerformanceStart(object sender, System.EventArgs e)
    {
        Debug.Log("On Performance Start from music crowd manager ");
        // when the performance starts 
        PlayMainLoop(new delay() 
        {
            isDelay = false,
        } , listeningAnimationSetSO);


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// I am migrating the crowd animation manager code to in this class 
    /// if I have to make crowd animation monobehaviour, then I have to put it in the scene
    /// If I have to put it in the scene I adding more complecity in managing the animation manager which will be a pain in the ass
    /// </summary>
    /// 
    private void PlayMainLoop(delay delay , AnimationSetSO clipSet)
    {
        foreach (CroudMember member in members)
        {
            AnimationClip clip = clipSet.GetRandomClip();
            StartCoroutine(ScheduleFunctionWithRandomDelay(delay , () => { member.PlayMainLoop(clip); } ));
            
        }

    }

    private void PlayOneShot(delay delay ,  AnimationSetSO clipSet)
    {
        foreach (CroudMember member in members)
        {
            AnimationClip clip = clipSet.GetRandomClip();
            StartCoroutine(ScheduleFunctionWithRandomDelay(delay ,  () => { member.PlayOneShot(clip); } ));
            
        }
    }
    IEnumerator ScheduleFunctionWithRandomDelay(delay delay,  Action callback) 
    {
        if (delay.isDelay) 
        {
            float delay_time  = UnityEngine.Random.Range(delay.minDelay, delay.maxDelay);
            yield return new WaitForSeconds(delay_time);
        
        }
        callback?.Invoke();


    }

    public void Clear()
    {
        members.Clear();
    }



}
