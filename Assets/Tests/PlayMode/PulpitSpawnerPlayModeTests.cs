using System.Collections;
using DoofusAdventure;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace DoofusAdventure.PlayModeTests
{
    public sealed class PulpitSpawnerPlayModeTests
    {
        [UnityTest]
        public IEnumerator PlatformLoop_KeepsAtMostTwoPulpitsAlive()
        {
            var root = new GameObject("PulpitSpawnerTest");
            var spawner = root.AddComponent<PulpitSpawner>();
            var config = GameConfig.Default;
            config.pulpit_data.min_pulpit_destroy_time = 4f;
            config.pulpit_data.max_pulpit_destroy_time = 4f;
            config.pulpit_data.pulpit_spawn_time = 2.5f;
            spawner.Begin(config);

            yield return new WaitForSeconds(2.75f);
            Assert.That(spawner.ActivePulpitCount, Is.EqualTo(2));

            yield return new WaitForSeconds(2.5f);
            Assert.That(spawner.ActivePulpitCount, Is.LessThanOrEqualTo(2));
            Assert.That(spawner.ActivePulpitCount, Is.GreaterThanOrEqualTo(1));

            Object.Destroy(root);
            yield return null;
        }
    }
}
