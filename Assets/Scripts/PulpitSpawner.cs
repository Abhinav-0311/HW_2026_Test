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
        private Vector2Int pendingSuccessorPosition;
        private bool successorPending;
        private int nextPulpitId;

        public bool IsRunning { get; private set; }

        public Pulpit StartingPulpit { get; private set; }

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
            if (IsRunning)
            {
                return;
            }

            config = gameConfig;
            nextPulpitId = 0;
            newestPulpit = null;
            successorPending = false;
            pendingSuccessorPosition = default;
            ClearPreviousRuntimeObjects();
            IsRunning = true;
            SpawnInitialPulpit();
        }

        public void Stop()
        {
            IsRunning = false;
        }

        public void ResetForRestart()
        {
            IsRunning = false;
            successorPending = false;
            newestPulpit = null;
            StartingPulpit = null;
            nextPulpitId = 0;

            foreach (var pulpit in activePulpits)
            {
                if (pulpit != null)
                {
                    Destroy(pulpit.gameObject);
                }
            }

            activePulpits.Clear();
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
            StartingPulpit = SpawnAt(Vector2Int.zero);
        }

        private void ClearPreviousRuntimeObjects()
        {
            activePulpits.Clear();
            StartingPulpit = null;

            for (var index = transform.childCount - 1; index >= 0; index--)
            {
                var child = transform.GetChild(index).gameObject;
                if (child.name.StartsWith("Pulpit (") || child.name == "Next Pulpit Beacon" || child.name == "Score Point")
                {
                    Destroy(child);
                }
            }
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

        private Pulpit SpawnAt(Vector2Int gridPosition)
        {
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
            pulpit.Initialize(this, gridPosition, successorPosition, nextPulpitId++, lifetime, config.pulpit_data.pulpit_spawn_time);
            return pulpit;
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

    }
}
