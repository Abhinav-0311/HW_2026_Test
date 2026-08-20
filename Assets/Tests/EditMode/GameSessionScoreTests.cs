using DoofusAdventure;
using NUnit.Framework;
using UnityEngine;

namespace DoofusAdventure.EditModeTests
{
    public sealed class GameSessionScoreTests
    {
        [Test]
        public void RegisterPulpitReached_IncreasesScoreOnlyForNewPulpits()
        {
            var sessionObject = new GameObject("Session");
            var startingPulpitObject = new GameObject("Starting Pulpit");
            var nextPulpitObject = new GameObject("Next Pulpit");
            var session = sessionObject.AddComponent<GameSession>();
            var startingPulpit = startingPulpitObject.AddComponent<Pulpit>();
            var nextPulpit = nextPulpitObject.AddComponent<Pulpit>();

            startingPulpit.Initialize(null, Vector2Int.zero, Vector2Int.right, 0, 4f, 2.5f);
            nextPulpit.Initialize(null, Vector2Int.right, Vector2Int.up, 1, 4f, 2.5f);
            session.SetStartingPulpit(startingPulpit);

            session.RegisterPulpitReached(startingPulpit);
            Assert.That(session.Score, Is.EqualTo(0));

            session.RegisterPulpitReached(nextPulpit);
            session.RegisterPulpitReached(nextPulpit);
            Assert.That(session.Score, Is.EqualTo(1));

            Object.DestroyImmediate(sessionObject);
            Object.DestroyImmediate(startingPulpitObject);
            Object.DestroyImmediate(nextPulpitObject);
        }
    }
}
