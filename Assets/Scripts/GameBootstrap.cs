using UnityEngine;

namespace DoofusAdventure
{
    public static class GameBootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void InitializeAuthoredScene()
        {
            var session = Object.FindFirstObjectByType<GameSession>();
            var spawner = Object.FindFirstObjectByType<PulpitSpawner>();
            var doofusController = Object.FindFirstObjectByType<DoofusController>();
            var cameraFollow = Object.FindFirstObjectByType<CameraFollow>();
            var scoreHud = Object.FindFirstObjectByType<ScoreHud>();
            var gameUi = Object.FindFirstObjectByType<GameUi>();

            if (session == null || spawner == null || doofusController == null ||
                cameraFollow == null || scoreHud == null || gameUi == null)
            {
                Debug.LogError("Main scene is missing one or more authored game objects. Use Doofus > Build Editable Main Scene.");
                return;
            }

            var config = GameConfig.Load(out var diagnostic);
            Debug.Log(diagnostic);

            HideEditorPreviewObjects(spawner.transform);
            session.Initialize(doofusController, spawner, config);
            doofusController.Initialize(session, config.player_data.speed);
            scoreHud.Initialize(session);
            cameraFollow.Initialize(doofusController.transform);
            gameUi.Initialize(session, cameraFollow.transform);
        }

        private static void HideEditorPreviewObjects(Transform root)
        {
            foreach (Transform child in root)
            {
                if (child.CompareTag("EditorOnly"))
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
    }
}
