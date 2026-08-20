using NUnit.Framework;
using UnityEngine;
using DoofusAdventure;

namespace DoofusAdventure.Tests
{
    public sealed class GameConfigTests
    {
        [Test]
        public void FromJson_UsesProvidedDiaryValues_WhenValuesAreValid()
        {
            const string json = "{\"player_data\":{\"speed\":3},\"pulpit_data\":{\"min_pulpit_destroy_time\":4,\"max_pulpit_destroy_time\":5,\"pulpit_spawn_time\":2.5}}";

            var config = GameConfig.FromJson(json, out _);

            Assert.That(config.player_data.speed, Is.EqualTo(3f));
            Assert.That(config.pulpit_data.min_pulpit_destroy_time, Is.EqualTo(4f));
            Assert.That(config.pulpit_data.max_pulpit_destroy_time, Is.EqualTo(5f));
            Assert.That(config.pulpit_data.pulpit_spawn_time, Is.EqualTo(2.5f));
        }

        [Test]
        public void FromJson_SwapsReversedLifetimeRange()
        {
            const string json = "{\"player_data\":{\"speed\":3},\"pulpit_data\":{\"min_pulpit_destroy_time\":5,\"max_pulpit_destroy_time\":4,\"pulpit_spawn_time\":2.5}}";

            var config = GameConfig.FromJson(json, out var diagnostic);

            Assert.That(config.pulpit_data.min_pulpit_destroy_time, Is.EqualTo(4f));
            Assert.That(config.pulpit_data.max_pulpit_destroy_time, Is.EqualTo(5f));
            Assert.That(diagnostic, Does.Contain("corrected"));
        }

        [Test]
        public void ChooseOpenNeighbor_ReturnsCardinalUnoccupiedPosition()
        {
            var origin = Vector2Int.zero;
            var occupied = new System.Collections.Generic.HashSet<Vector2Int>
            {
                origin,
                Vector2Int.up,
                Vector2Int.right,
                Vector2Int.down
            };

            var neighbor = PulpitGrid.ChooseOpenNeighbor(origin, occupied);

            Assert.That(neighbor, Is.EqualTo(Vector2Int.left));
        }
    }
}
