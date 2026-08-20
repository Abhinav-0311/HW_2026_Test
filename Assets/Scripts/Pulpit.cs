using UnityEngine;

namespace DoofusAdventure
{
    public sealed class Pulpit : MonoBehaviour
    {
        private PulpitSpawner spawner;
        private float lifetime;
        private float spawnDelay;
        private float elapsed;
        private bool requestedSuccessor;
        private bool expired;

        public Vector2Int GridPosition { get; private set; }

        public Vector2Int SuccessorGridPosition { get; private set; }

        public void Initialize(
            PulpitSpawner owner,
            Vector2Int gridPosition,
            Vector2Int successorGridPosition,
            float platformLifetime,
            float successorSpawnDelay)
        {
            spawner = owner;
            GridPosition = gridPosition;
            SuccessorGridPosition = successorGridPosition;
            lifetime = platformLifetime;
            spawnDelay = successorSpawnDelay;
        }

        private void Update()
        {
            if (expired || spawner == null || !spawner.IsRunning)
            {
                return;
            }

            elapsed += Time.deltaTime;

            // Expiry is evaluated before a successor request. At the exact boundary this
            // releases the old platform before a deferred successor could create a third.
            if (elapsed >= lifetime)
            {
                expired = true;
                spawner.Expire(this);
                Destroy(gameObject);
                return;
            }

            if (!requestedSuccessor && elapsed >= spawnDelay)
            {
                requestedSuccessor = true;
                spawner.RequestSuccessor(this);
            }
        }
    }
}
