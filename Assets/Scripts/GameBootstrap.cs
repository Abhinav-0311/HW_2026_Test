using UnityEngine;

namespace DoofusAdventure
{
    public static class GameBootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void CreateGame()
        {
            if (Object.FindFirstObjectByType<GameSession>() != null)
            {
                return;
            }

            var config = GameConfig.Load(out var diagnostic);
            Debug.Log(diagnostic);

            var root = new GameObject("Doofus Adventure");
            var session = root.AddComponent<GameSession>();
            var spawner = root.AddComponent<PulpitSpawner>();

            var doofus = GameObject.CreatePrimitive(PrimitiveType.Cube);
            doofus.name = "Doofus";
            doofus.transform.position = new Vector3(0f, 1.1f, 0f);
            var renderer = doofus.GetComponent<MeshRenderer>();
            renderer.material.color = new Color(0.97f, 0.60f, 0.12f);

            Object.Destroy(doofus.GetComponent<BoxCollider>());
            var controller = doofus.AddComponent<CharacterController>();
            controller.height = 1f;
            controller.radius = 0.45f;
            controller.center = new Vector3(0f, 0.5f, 0f);

            var doofusController = doofus.AddComponent<DoofusController>();
            session.Initialize(doofusController, spawner);
            doofusController.Initialize(session, config.player_data.speed);

            var cameraObject = new GameObject("Main Camera");
            cameraObject.tag = "MainCamera";
            cameraObject.AddComponent<Camera>();
            var cameraFollow = cameraObject.AddComponent<CameraFollow>();
            cameraFollow.Initialize(doofus.transform);

            var lightObject = new GameObject("Directional Light");
            var directionalLight = lightObject.AddComponent<Light>();
            directionalLight.type = LightType.Directional;
            directionalLight.intensity = 1.25f;
            lightObject.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

            spawner.Begin(config);
        }
    }
}
