using System.Collections.Generic;
using UnityEngine;

namespace DoofusAdventure
{
    public static class PulpitGrid
    {
        private static readonly Vector2Int[] CardinalDirections =
        {
            Vector2Int.up,
            Vector2Int.right,
            Vector2Int.down,
            Vector2Int.left
        };

        public static Vector2Int ChooseOpenNeighbor(Vector2Int origin, ISet<Vector2Int> occupied)
        {
            var candidates = new List<Vector2Int>(CardinalDirections.Length);
            foreach (var direction in CardinalDirections)
            {
                var candidate = origin + direction;
                if (!occupied.Contains(candidate))
                {
                    candidates.Add(candidate);
                }
            }

            if (candidates.Count == 0)
            {
                return origin + Vector2Int.right;
            }

            return candidates[Random.Range(0, candidates.Count)];
        }

        public static Vector3 ToWorldPosition(Vector2Int gridPosition, float platformSize)
        {
            return new Vector3(gridPosition.x * platformSize, 0f, gridPosition.y * platformSize);
        }
    }
}
