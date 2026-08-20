using UnityEngine;
using System.Collections.Generic;

namespace DoofusAdventure
{
    public sealed class GameSession : MonoBehaviour
    {
        private DoofusController doofus;
        private PulpitSpawner spawner;
        private readonly HashSet<int> visitedPulpits = new HashSet<int>();

        public bool IsGameOver { get; private set; }

        public int Score { get; private set; }

        public void Initialize(DoofusController player, PulpitSpawner platformSpawner)
        {
            doofus = player;
            spawner = platformSpawner;
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
            doofus.Stop();
            spawner.Stop();
            Debug.Log("Game over: Doofus fell from the Pulpits.");
        }
    }
}
