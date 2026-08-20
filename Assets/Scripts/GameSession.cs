using UnityEngine;

namespace DoofusAdventure
{
    public sealed class GameSession : MonoBehaviour
    {
        private DoofusController doofus;
        private PulpitSpawner spawner;

        public bool IsGameOver { get; private set; }

        public void Initialize(DoofusController player, PulpitSpawner platformSpawner)
        {
            doofus = player;
            spawner = platformSpawner;
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
