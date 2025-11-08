using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.Rendering.Universal;

public class FindNonURPCameras_AllScenes
{
    [MenuItem("Tools/Find Non-URP Cameras (All .unity)")]
    public static void FindInAllScenes()
    {
        var sceneGuids = AssetDatabase.FindAssets("t:Scene");
        foreach (var guid in sceneGuids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);

            if (path.StartsWith("Packages/"))
                continue;

            var scene = EditorSceneManager.OpenScene(path);

            foreach (var cam in Object.FindObjectsByType<Camera>(FindObjectsSortMode.None))
            {
                if (!cam.TryGetComponent<UniversalAdditionalCameraData>(out _))
                    Debug.LogWarning($"(ALL) Scene '{scene.name}' - Camera '{cam.name}' missing URP data. Path: {path}", cam);
            }
        }

        Debug.Log("Scan (all scenes) complete. (Packages/¡¦ skipped)");
    }
}
