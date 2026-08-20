using UnityEngine;

namespace DoofusAdventure
{
    public sealed class GameUi : MonoBehaviour
    {
        private GameSession session;
        private TextMesh title;
        private TextMesh detail;
        private TextMesh action;

        public void Initialize(GameSession gameSession, Transform cameraTransform)
        {
            session = gameSession;
            var canvas = new GameObject("Game Screen Text");
            canvas.transform.SetParent(cameraTransform, false);
            canvas.transform.localPosition = new Vector3(0f, 0f, 8f);
            title = CreateText(canvas.transform, new Vector3(0f, 1.2f, 0f), 0.7f);
            detail = CreateText(canvas.transform, new Vector3(0f, 0.15f, 0f), 0.3f);
            action = CreateText(canvas.transform, new Vector3(0f, -0.85f, 0f), 0.4f);
            Refresh();
        }

        private void Update()
        {
            if (session == null)
            {
                return;
            }

            if (!session.HasStarted && Input.GetKeyDown(KeyCode.Space))
            {
                session.StartGame();
            }
            else if (session.IsGameOver && Input.GetKeyDown(KeyCode.R))
            {
                session.RestartGame();
            }

            Refresh();
        }

        private void Refresh()
        {
            if (title == null)
            {
                return;
            }

            var showScreen = !session.HasStarted || session.IsGameOver;
            title.gameObject.SetActive(showScreen);
            detail.gameObject.SetActive(showScreen);
            action.gameObject.SetActive(showScreen);
            if (!showScreen)
            {
                return;
            }

            if (!session.HasStarted)
            {
                title.text = "DOOFUS ADVENTURE";
                detail.text = "Reach new Pulpits before they disappear";
                action.text = "Press SPACE to Start";
            }
            else
            {
                title.text = "GAME OVER";
                detail.text = $"Score: {session.Score}";
                action.text = "Press R to Restart";
            }
        }

        private static TextMesh CreateText(Transform parent, Vector3 localPosition, float characterSize)
        {
            var textObject = new GameObject("Screen Text");
            textObject.transform.SetParent(parent, false);
            textObject.transform.localPosition = localPosition;
            var text = textObject.AddComponent<TextMesh>();
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.anchor = TextAnchor.MiddleCenter;
            text.alignment = TextAlignment.Center;
            text.characterSize = characterSize;
            text.fontSize = 64;
            text.color = Color.white;
            return text;
        }
    }
}
