using UnityEngine;
using System.Collections.Generic;

namespace DoofusAdventure
{
    public sealed class GameSession : MonoBehaviour
    {
        private DoofusController doofus;
        private PulpitSpawner spawner;
        private GameConfig config;
        private readonly HashSet<int> visitedPulpits = new HashSet<int>();

        public bool IsGameOver { get; private set; }

        public bool HasStarted { get; private set; }

        public int Score { get; private set; }

        public void Initialize(DoofusController player, PulpitSpawner platformSpawner, GameConfig gameConfig)
        {
            doofus = player;
            spawner = platformSpawner;
            config = gameConfig;
            IsGameOver = false;
            HasStarted = false;
            Score = 0;
            visitedPulpits.Clear();
        }

        public void StartGame()
        {
            if (HasStarted || IsGameOver || config == null)
            {
                return;
            }

            HasStarted = true;
            spawner.Begin(config);
            SetStartingPulpit(spawner.StartingPulpit);
            doofus.SetMovementEnabled(true);
        }

        public void RestartGame()
        {
            if (!IsGameOver)
            {
                return;
            }

            IsGameOver = false;
            HasStarted = false;
            Score = 0;
            visitedPulpits.Clear();
            spawner.ResetForRestart();
            doofus.ResetToStartPosition();
        }

        public void SetStartingPulpit(Pulpit pulpit)
        {
            if (pulpit != null)
            {
                visitedPulpits.Add(pulpit.PulpitId);
            }
        }

        public void RegisterPulpitReached(Pulpit pulpit)
        {
            if (IsGameOver || pulpit == null || !visitedPulpits.Add(pulpit.PulpitId))
            {
                return;
            }

            Score++;
        }

        public void EndGame()
        {
            if (IsGameOver)
            {
                return;
            }

            IsGameOver = true;
            doofus.SetMovementEnabled(false);
            spawner.Stop();
            Debug.Log("Game over: Doofus fell from the Pulpits.");
        }
    }
}
