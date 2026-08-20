using System.Collections;
using DoofusAdventure;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace DoofusAdventure.PlayModeTests
{
    public sealed class GameSessionFlowPlayModeTests
    {
        [UnityTest]
        public IEnumerator StartGame_BeginsThePulpitLoopOnlyAfterPlayerStarts()
        {
            var root = new GameObject("SessionFlowTest");
            var session = root.AddComponent<GameSession>();
            var spawner = root.AddComponent<PulpitSpawner>();
            var player = new GameObject("Player");
            player.AddComponent<CharacterController>();
            var controller = player.AddComponent<DoofusController>();
            var config = GameConfig.Default;

            controller.Initialize(session, config.player_data.speed);
            session.Initialize(controller, spawner, config);
            Assert.That(session.HasStarted, Is.False);
            Assert.That(spawner.IsRunning, Is.False);

            session.StartGame();
            Assert.That(session.HasStarted, Is.True);
            Assert.That(spawner.IsRunning, Is.True);
            Assert.That(spawner.StartingPulpit, Is.Not.Null);

            Object.Destroy(root);
            Object.Destroy(player);
            yield return null;
        }
    }
}
