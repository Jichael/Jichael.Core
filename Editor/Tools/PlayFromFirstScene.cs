using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Jichaels.Core.Editor
{
    
    [InitializeOnLoad]
    public static class PlayFromFirstScene
    {
        private const string MenuName = "Tools/Play from first scene";

        static PlayFromFirstScene()
        {
            EditorApplication.delayCall += () =>
            {
                bool enabled = EditorPrefs.GetBool(MenuName, false);
                SetFirstScene(enabled, false);
            };
        }


        [MenuItem(MenuName)]
        private static void ToggleMenu()
        {
            bool enabled = !EditorPrefs.GetBool(MenuName, false);
            SetFirstScene(enabled, true);
            EditorPrefs.SetBool(MenuName, enabled);
        }

        private static void SetFirstScene(bool enabled, bool showLogs)
        {
            Menu.SetChecked(MenuName, enabled);

            if (enabled)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(0);
                if (scenePath == string.Empty)
                {
                    Debug.LogError(
                        "Could not load scene at build index 0 ! Be sure to add at least one scene in the Build Settings.");
                    return;
                }

                SceneAsset scene = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
                if (scene == null)
                {
                    Debug.LogError($"Could not load scene '{scenePath} from asset database !");
                    return;
                }

                if(showLogs) Debug.Log($"PlayMode will start from scene {scenePath}.");
                EditorSceneManager.playModeStartScene = scene;
            }
            else
            {
                if(showLogs) Debug.Log("PlayMode will start from the active scene (normal behaviour).");
                EditorSceneManager.playModeStartScene = null;
            }
        }

    }
}