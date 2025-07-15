using UnityEngine;
using NPC.Handler;
using System.Collections.Generic;

namespace Spawn
{
    /// <summary>
    /// the reason I'm creating a new spawner class is because the previous spawner class is tied with the NPC system
    /// lack of oversight from my end 
    /// </summary>

    ///<summary>
    /// this class is meant to be used by any system to spawn objects of type spawnable
    /// Instead of using spawnable I could have used Apprearable, but spawnable adds more flexibility 
    /// </summary>>

    public class SpawnGeneric : MonoBehaviour
    {



        [SerializeField] private Vector3 randomAreaSize = new Vector3(3, 0, 3);
        [SerializeField] private Vector3 randomAreaOffset = Vector3.zero;
        [SerializeField] private bool isRandomizeOnStart = false;

        // this list will hold the set of spawnable 
        private List<Spawnable> spawnables;

        
        private void Start()
        {
            // potential source of bugs
            // need to be careful with making objects inactive and active
            spawnables = new List<Spawnable>();
            GetSpawnables(spawnables);
            //RandomizeSpawanableInSpawnArea();

        }

        // the reason I'm taking in a reference to a list of type spawnable 
        // is because creating pure functions is much better
        // warning the reference passed to the function, will be modified
        // even though the in c# we pass value by copy, but the reference is being copied
        // so the original List will be modified
        // I dont know why I dropped cs knowledge, i think i had coffee
        private void GetSpawnables(List<Spawnable> spawnables) 
        {
            // do no create a new List here, 
            // if you do create a new list here, you need to return the reference
            if (spawnables == null) 
            {
                Debug.LogError("Spawnables list is null. Please provide a valid list to store spawnable objects.");
                return;
            }

            for (int i = 0; i < this.transform.childCount; i++) 
            {
                
                spawnables.Add(this.transform.GetChild(i).GetComponent<Spawnable>());
            
            }

            Debug.Log("got all the spawnables spawnables count  "+spawnables.Count);
        }

        public void ResetSpawnArea() 
        {
            foreach(Spawnable spawnable in this.spawnables)
            {
                spawnable.Despawn();
            }

        }


        /// <summary>
        /// Randomizes the position of the given NPCs within the spawn area.
        /// </summary>
        /// <param name="npcs">Array of NPCHandler instances to be positioned randomly.</param>
        public void RandomizeSpawanableInSpawnArea()
        {
            Debug.Log("this function is called ");
            // foreach is slower than normal for loop
            foreach (Spawnable spawnable in this.spawnables) 
            {
                spawnable.Spawn(GetRandomPositionOnRandomArea());

            }
        }

        /// <summary>
        /// Returns a random position within the spawn area.
        /// It picks a random point inside a rectangle centered on the spawn area.
        /// </summary>
        public Vector3 GetRandomPositionOnRandomArea()
        {
            float x = Random.Range(-randomAreaSize.x * 0.5f, randomAreaSize.x * 0.5f);
            float z = Random.Range(-randomAreaSize.z * 0.5f, randomAreaSize.z * 0.5f);

            return GetSpawnAreaCentre() + new Vector3(x, 0f, z);
        }

        /// <summary>
        /// Returns the center position of the spawn area including the offset.
        /// </summary>
        public Vector3 GetSpawnAreaCentre()
        {
            return this.transform.position + randomAreaOffset;
        }

        /// <summary>
        /// Draws gizmos in the editor to visualize the spawn area.
        /// </summary>
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0, 1, 0, 0.3f);
            Gizmos.DrawCube(GetSpawnAreaCentre(), randomAreaSize);

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(GetSpawnAreaCentre(), randomAreaSize);
        }
    }
}
