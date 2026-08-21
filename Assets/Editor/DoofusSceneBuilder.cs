using DoofusAdventure;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class DoofusSceneBuilder
{
    [MenuItem("Doofus/Build Editable Main Scene")]
    public static void BuildEditableMainScene()
    {
        EnsureMaterialsFolder();
        var scene = SceneManager.GetActiveScene();
        foreach (var existingRoot in scene.GetRootGameObjects())
        {
            Object.DestroyImmediate(existingRoot);
        }

        var root = new GameObject("Doofus Adventure");
        root.AddComponent<GameSession>();
        root.AddComponent<PulpitSpawner>();
        root.AddComponent<ScoreHud>();
        root.AddComponent<GameUi>();

        var music = root.AddComponent<AudioSource>();
        music.clip = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Music/Gameplay_Segment_01.mp3");
        music.loop = true;
        music.playOnAwake = true;
        music.spatialBlend = 0f;
        music.volume = 0.45f;

        var doofus = GameObject.CreatePrimitive(PrimitiveType.Cube);
        doofus.name = "Doofus";
        doofus.transform.SetParent(root.transform);
        doofus.transform.position = new Vector3(0f, 1f, 0f);
        doofus.GetComponent<MeshRenderer>().sharedMaterial = CreateMaterial("Doofus Material", new Color(1f, 0.78f, 0.08f));
        Object.DestroyImmediate(doofus.GetComponent<BoxCollider>());
        var controller = doofus.AddComponent<CharacterController>();
        controller.height = 1f;
        controller.radius = 0.45f;
        controller.center = Vector3.zero;
        doofus.AddComponent<DoofusController>();

        var cameraObject = new GameObject("Main Camera");
        cameraObject.transform.SetParent(root.transform);
        cameraObject.tag = "MainCamera";
        cameraObject.AddComponent<Camera>();
        cameraObject.AddComponent<AudioListener>();
        cameraObject.AddComponent<CameraFollow>();
        cameraObject.transform.position = new Vector3(0f, 8f, -10f);
        cameraObject.transform.LookAt(Vector3.zero);

        var lightObject = new GameObject("Directional Light");
        lightObject.transform.SetParent(root.transform);
        lightObject.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
        var directionalLight = lightObject.AddComponent<Light>();
        directionalLight.type = LightType.Directional;
        directionalLight.intensity = 1.25f;

        CreatePreviewPulpit(root.transform, "Pulpit Preview A", Vector3.zero);
        CreatePreviewPulpit(root.transform, "Pulpit Preview B", new Vector3(9f, 0f, 0f));
        AssignSkybox();

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Selection.activeGameObject = root;
    }

    private static void CreatePreviewPulpit(Transform parent, string name, Vector3 position)
    {
        var pulpit = GameObject.CreatePrimitive(PrimitiveType.Cube);
        pulpit.name = name;
        pulpit.tag = "EditorOnly";
        pulpit.transform.SetParent(parent);
        pulpit.transform.position = position;
        pulpit.transform.localScale = new Vector3(GameConfig.PlatformSize, 1f, GameConfig.PlatformSize);
        pulpit.GetComponent<MeshRenderer>().sharedMaterial = CreateMaterial("Preview Pulpit Material", new Color(0.06f, 0.42f, 0.16f));
    }

    private static Material CreateMaterial(string assetName, Color color)
    {
        var path = $"Assets/Materials/{assetName}.mat";
        var material = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (material == null || material.shader == null)
        {
            if (material != null)
            {
                AssetDatabase.DeleteAsset(path);
            }

            var template = AssetDatabase.LoadAssetAtPath<Material>("Assets/Resources/PulpitMaterial.mat");
            if (template == null)
            {
                throw new System.InvalidOperationException("PulpitMaterial.mat is required to create editable scene materials.");
            }

            material = new Material(template);
            AssetDatabase.CreateAsset(material, path);
        }

        material.color = color;
        EditorUtility.SetDirty(material);
        return material;
    }

    private static void EnsureMaterialsFolder()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Materials"))
        {
            AssetDatabase.CreateFolder("Assets", "Materials");
        }
    }

    private static void AssignSkybox()
    {
        const string skyboxPath = "Assets/Materials/Adventure Skybox.mat";
        var skybox = AssetDatabase.LoadAssetAtPath<Material>(skyboxPath);
        if (skybox == null)
        {
            skybox = new Material(Shader.Find("Skybox/Procedural"));
            AssetDatabase.CreateAsset(skybox, skyboxPath);
        }

        skybox.SetColor("_SkyTint", new Color(0.32f, 0.52f, 0.78f));
        skybox.SetColor("_GroundColor", new Color(0.10f, 0.22f, 0.38f));
        skybox.SetFloat("_AtmosphereThickness", 0.45f);
        RenderSettings.skybox = skybox;
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientSkyColor = new Color(0.22f, 0.34f, 0.48f);
        EditorUtility.SetDirty(skybox);
    }
}
