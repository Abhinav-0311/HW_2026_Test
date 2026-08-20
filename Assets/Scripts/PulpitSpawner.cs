using System.Collections.Generic;
using UnityEngine;

namespace DoofusAdventure
{
    public sealed class PulpitSpawner : MonoBehaviour
    {
        private readonly List<Pulpit> activePulpits = new List<Pulpit>(2);
        private GameConfig config;
        private Pulpit newestPulpit;
        private Material pulpitMaterial;
        private Material beaconMaterial;
        private GameObject successorBeacon;
        private Vector2Int pendingSuccessorPosition;
        private bool successorPending;

        public bool IsRunning { get; private set; }

        public int ActivePulpitCount
        {
            get
            {
                activePulpits.RemoveAll(pulpit => pulpit == null);
                return activePulpits.Count;
            }
        }

        public void Begin(GameConfig gameConfig)
        {
            config = gameConfig;
            IsRunning = true;
            SpawnInitialPulpit();
        }

        public void Stop()
        {
            IsRunning = false;
        }

        public void RequestSuccessor(Pulpit source)
        {
            if (!IsRunning || source != newestPulpit)
            {
                return;
            }

            successorPending = true;
            pendingSuccessorPosition = source.SuccessorGridPosition;
            TrySpawnPendingSuccessor();
        }

        public void Expire(Pulpit pulpit)
        {
            activePulpits.Remove(pulpit);
            TrySpawnPendingSuccessor();
        }

        private void SpawnInitialPulpit()
        {
            SpawnAt(Vector2Int.zero);
        }

        private void TrySpawnPendingSuccessor()
        {
            activePulpits.RemoveAll(pulpit => pulpit == null);
            if (!successorPending || activePulpits.Count >= 2 || newestPulpit == null)
            {
                return;
            }

            successorPending = false;
            SpawnAt(pendingSuccessorPosition);
        }

        private void SpawnAt(Vector2Int gridPosition)
        {
            if (successorBeacon != null)
            {
                Destroy(successorBeacon);
            }

            var platform = GameObject.CreatePrimitive(PrimitiveType.Cube);
            platform.name = $"Pulpit ({gridPosition.x}, {gridPosition.y})";
            platform.transform.SetParent(transform);
            platform.transform.position = PulpitGrid.ToWorldPosition(gridPosition, GameConfig.PlatformSize);
            platform.transform.localScale = new Vector3(GameConfig.PlatformSize, 1f, GameConfig.PlatformSize);

            var renderer = platform.GetComponent<MeshRenderer>();
            renderer.sharedMaterial = GetPulpitMaterial();

            var pulpit = platform.AddComponent<Pulpit>();
            var lifetime = Random.Range(config.pulpit_data.min_pulpit_destroy_time, config.pulpit_data.max_pulpit_destroy_time);
            activePulpits.Add(pulpit);
            newestPulpit = pulpit;

            var occupied = new HashSet<Vector2Int>();
            foreach (var activePulpit in activePulpits)
            {
                occupied.Add(activePulpit.GridPosition);
            }

            var successorPosition = PulpitGrid.ChooseOpenNeighbor(gridPosition, occupied);
            pulpit.Initialize(this, gridPosition, successorPosition, lifetime, config.pulpit_data.pulpit_spawn_time);
            CreateSuccessorBeacon(successorPosition);
        }

        private void CreateSuccessorBeacon(Vector2Int gridPosition)
        {
            successorBeacon = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            successorBeacon.name = "Next Pulpit Beacon";
            successorBeacon.transform.SetParent(transform);
            successorBeacon.transform.position = PulpitGrid.ToWorldPosition(gridPosition, GameConfig.PlatformSize) + Vector3.up * 1.5f;
            successorBeacon.transform.localScale = Vector3.one * 0.8f;
            Destroy(successorBeacon.GetComponent<Collider>());

            var renderer = successorBeacon.GetComponent<MeshRenderer>();
            renderer.sharedMaterial = GetBeaconMaterial();
        }

        private Material GetPulpitMaterial()
        {
            if (pulpitMaterial != null)
            {
                return pulpitMaterial;
            }

            pulpitMaterial = Resources.Load<Material>("PulpitMaterial");
            if (pulpitMaterial == null)
            {
                Debug.LogError("Pulpit material could not be loaded.");
            }
            return pulpitMaterial;
        }

        private Material GetBeaconMaterial()
        {
            if (beaconMaterial != null)
            {
                return beaconMaterial;
            }

            beaconMaterial = Resources.Load<Material>("BeaconMaterial");
            if (beaconMaterial == null)
            {
                Debug.LogError("Beacon material could not be loaded.");
            }
            return beaconMaterial;
        }
    }
}
