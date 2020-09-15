using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CustomPackages.Silicom.Core.Editor.Tools
{
    public static class MissingReferencesFinder
    {
        [MenuItem("Tools/Show Missing Object References in scene", false, 50)]
        public static void FindMissingReferencesInCurrentScene()
        {
            var objects = GetSceneObjects();
            FindMissingReferences(objects);
        }

        private static void FindMissingReferences(GameObject[] objects)
        {
            int missingRef = 0;
            foreach (var go in objects)
            {
                var components = go.GetComponents<Component>();

                foreach (var c in components)
                {
                    if (!c)
                    {
                        Debug.LogError($"Missing component on : {FullPath(go)}", go);
                        missingRef++;
                        continue;
                    }

                    SerializedObject so = new SerializedObject(c);
                    var sp = so.GetIterator();

                    while (sp.NextVisible(true))
                    {
                        if (sp.propertyType != SerializedPropertyType.ObjectReference) continue;
                        if (sp.objectReferenceValue != null || sp.objectReferenceInstanceIDValue == 0) continue;
                        ShowError(go, c.GetType().Name, ObjectNames.NicifyVariableName(sp.name));
                        missingRef++;
                    }
                }
            }

            if (missingRef == 0) Debug.Log("No missing reference found !");
        }

        private static GameObject[] GetSceneObjects()
        {
            List<GameObject> list = new List<GameObject>();
            foreach (var go in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                if (string.IsNullOrEmpty(AssetDatabase.GetAssetPath(go)) && go.hideFlags == HideFlags.None)
                    list.Add(go);
            }

            return list.ToArray();
        }

        private static void ShowError(GameObject go, string componentName, string propertyName)
        {
            Debug.LogError($"Missing reference in {FullPath(go)}. Component: {componentName}, property: {propertyName}",
                go);
        }

        private static string FullPath(GameObject go)
        {
            var parent = go.transform.parent;
            return parent == null ? go.name : $"{FullPath(parent.gameObject)}/{go.name}";
        }
    }
}