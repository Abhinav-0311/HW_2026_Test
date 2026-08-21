using UnityEngine;

namespace DoofusAdventure
{
    public sealed class Pulpit : MonoBehaviour
    {
        private PulpitSpawner spawner;
        private float lifetime;
        private float spawnDelay;
        private float elapsed;
        private bool requestedSuccessor;
        private bool expired;

        public Vector2Int GridPosition { get; private set; }

        public int PulpitId { get; private set; }

        public Vector2Int SuccessorGridPosition { get; private set; }

        public void Initialize(
            PulpitSpawner owner,
            Vector2Int gridPosition,
            Vector2Int successorGridPosition,
            int pulpitId,
            float platformLifetime,
            float successorSpawnDelay)
        {
            spawner = owner;
            GridPosition = gridPosition;
            SuccessorGridPosition = successorGridPosition;
            PulpitId = pulpitId;
            lifetime = platformLifetime;
            spawnDelay = successorSpawnDelay;
        }

        private void Update()
        {
            if (expired || spawner == null || !spawner.IsRunning)
            {
                return;
            }

            elapsed += Time.deltaTime;

            // Expiry is evaluated first so a destroyed Pulpit never requests a successor.
            if (elapsed >= lifetime)
            {
                expired = true;
                spawner.Expire(this);
                Destroy(gameObject);
                return;
            }

            // The diary's spawn value is the amount of time left on the current
            // Pulpit when its successor becomes visible, not time since creation.
            if (!requestedSuccessor && lifetime - elapsed <= spawnDelay)
            {
                requestedSuccessor = true;
                spawner.RequestSuccessor(this);
            }
        }

        private void OnGUI()
        {
            if (expired || spawner == null || !spawner.IsRunning || Camera.main == null)
            {
                return;
            }

            var remaining = Mathf.Max(0f, lifetime - elapsed);
            var screenPosition = Camera.main.WorldToScreenPoint(transform.position + new Vector3(-3.1f, 0.65f, -3.1f));
            if (screenPosition.z <= 0f)
            {
                return;
            }

            var previousAlignment = GUI.skin.label.alignment;
            var previousFontSize = GUI.skin.label.fontSize;
            var previousColor = GUI.contentColor;
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUI.skin.label.fontSize = 28;
            var timerColor = remaining <= 1f ? new Color(1f, 0.28f, 0.18f) :
                remaining <= 2f ? new Color(1f, 0.78f, 0.18f) : Color.white;
            var timerRect = new Rect(screenPosition.x - 48f, Screen.height - screenPosition.y - 20f, 96f, 40f);
            GUI.contentColor = new Color(0f, 0f, 0f, 0.7f);
            GUI.Label(new Rect(timerRect.x + 2f, timerRect.y + 2f, timerRect.width, timerRect.height), remaining.ToString("0.00"));
            GUI.contentColor = timerColor;
            GUI.Label(timerRect, remaining.ToString("0.00"));
            GUI.skin.label.alignment = previousAlignment;
            GUI.skin.label.fontSize = previousFontSize;
            GUI.contentColor = previousColor;
        }
    }
}
