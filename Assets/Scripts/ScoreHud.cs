using UnityEngine;

namespace DoofusAdventure
{
    public sealed class ScoreHud : MonoBehaviour
    {
        private GameSession session;

        public void Initialize(GameSession gameSession)
        {
            session = gameSession;
        }

        private void OnGUI()
        {
            if (session == null || !session.HasStarted || session.IsGameOver)
            {
                return;
            }

            var previousAlignment = GUI.skin.label.alignment;
            var previousFontSize = GUI.skin.label.fontSize;
            var previousColor = GUI.color;
            var previousContentColor = GUI.contentColor;

            GUI.skin.label.alignment = TextAnchor.UpperCenter;
            GUI.skin.label.fontSize = 30;
            GUI.color = new Color(0.35f, 1f, 0.22f);
            GUI.contentColor = GUI.color;
            GUI.Label(new Rect((Screen.width - 220f) / 2f, 18f, 220f, 40f), $"{session.Score}");
            GUI.skin.label.fontSize = 15;
            GUI.color = Color.white;
            GUI.contentColor = Color.white;
            GUI.Label(new Rect((Screen.width - 220f) / 2f, 56f, 220f, 26f), "SCORE");

            GUI.skin.label.alignment = previousAlignment;
            GUI.skin.label.fontSize = previousFontSize;
            GUI.color = previousColor;
            GUI.contentColor = previousContentColor;
        }
    }
}
