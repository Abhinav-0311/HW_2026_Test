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

            GUI.skin.label.alignment = TextAnchor.UpperLeft;
            GUI.skin.label.fontSize = 28;
            GUI.color = Color.white;
            GUI.contentColor = Color.white;
            GUI.Label(new Rect(24f, 20f, 260f, 46f), $"Score: {session.Score}");

            GUI.skin.label.alignment = previousAlignment;
            GUI.skin.label.fontSize = previousFontSize;
            GUI.color = previousColor;
            GUI.contentColor = previousContentColor;
        }
    }
}
