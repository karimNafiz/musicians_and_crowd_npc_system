using UnityEngine;

namespace Spawn 
{
    public interface Spawnable
    {
        public abstract Spawnable Spawn(Vector3 position);
        public abstract Spawnable Despawn();
    }


}
