using Busker;
using Spawn;
using UnityEngine;

public class MusicCrowdManager : MonoBehaviour
{
    // I need the crowd animation manager to play the music crowd animations
    // i need the animation manager, to control the animation of the crow members
    [SerializeField] private CrowdAnimationManager crowdAnimationManager;
    // need access to the crowd spawner to ensure spawn of the crow
    [SerializeField] private SpawnGeneric crowdSpawner;

    // need the animation sets 
    [SerializeField] private AnimationSetSO idleAnimationSetSO;
    [SerializeField] private AnimationSetSO listeningAnimationSetSO;
    [SerializeField] private AnimationSetSO clappingAnimationSetSO;


    [SerializeField] private MusicianAnimationMonitor musicianAnimationMonitor;


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
    }

    private void MusicianAnimationMonitor_OnBowStart(object sender, System.EventArgs e)
    {
        crowdAnimationManager.PlayRandomClipFromSet(clappingAnimationSetSO);
    }

    private void MusicianAnimationMonitor_OnGuitarSlowStart(object sender, System.EventArgs e)
    {
        crowdAnimationManager.PlayRandomClipFromSet(listeningAnimationSetSO);
    }

    private void MusicianAnimationMonitor_OnPerformanceEnd(object sender, System.EventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void MusicianAnimationMonitor_OnPerformanceStart(object sender, System.EventArgs e)
    {
        crowdSpawner.RandomizeSpawanableInSpawnArea();
        // need the crow to perform the idle animation set 
        crowdAnimationManager.PlayRandomClipFromSet(idleAnimationSetSO);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
