using UnityEngine;

namespace DoofusAdventure
{
    public sealed class GameUi : MonoBehaviour
    {
        private GameSession session;

        public void Initialize(GameSession gameSession, Transform cameraTransform)
        {
            session = gameSession;
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
        }

        private void OnGUI()
        {
            if (session == null || (session.HasStarted && !session.IsGameOver))
            {
                return;
            }

            var panel = new Rect((Screen.width - 420f) / 2f, (Screen.height - 230f) / 2f, 420f, 230f);
            var titleStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = 30, normal = { textColor = Color.white } };
            var bodyStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = 17, normal = { textColor = new Color(0.9f, 0.95f, 1f) } };
            var buttonStyle = new GUIStyle(GUI.skin.button) { fontSize = 18 };

            var previousColor = GUI.color;
            GUI.color = new Color(0.015f, 0.04f, 0.09f, 0.74f);
            GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), Texture2D.whiteTexture);
            GUI.color = new Color(0.05f, 0.13f, 0.25f, 0.98f);
            GUI.DrawTexture(panel, Texture2D.whiteTexture);
            GUI.color = previousColor;

            if (!session.HasStarted)
            {
                GUI.Label(new Rect(panel.x, panel.y + 28f, panel.width, 42f), "DOOFUS ADVENTURE", titleStyle);
                GUI.Label(new Rect(panel.x + 30f, panel.y + 84f, panel.width - 60f, 38f), "Reach new Pulpits before they disappear", bodyStyle);
                if (GUI.Button(new Rect(panel.x + 110f, panel.y + 150f, 200f, 46f), "Start  (Space)", buttonStyle))
                {
                    session.StartGame();
                }
            }
            else
            {
                GUI.Label(new Rect(panel.x, panel.y + 30f, panel.width, 42f), "GAME OVER", titleStyle);
                GUI.Label(new Rect(panel.x, panel.y + 88f, panel.width, 34f), $"Score: {session.Score}", bodyStyle);
                if (GUI.Button(new Rect(panel.x + 110f, panel.y + 150f, 200f, 46f), "Restart  (R)", buttonStyle))
                {
                    session.RestartGame();
                }
            }
        }
    }
}
