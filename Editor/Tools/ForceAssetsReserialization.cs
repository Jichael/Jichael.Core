using UnityEditor;

namespace CustomPackages.Silicom.Core.Editor.Tools
{
    public static class ForceAssetsReserialization
    {
        [MenuItem("Tools/Force reserialize assets", false, 50)]
        private static void ForceReserializeAssets()
        {
            AssetDatabase.ForceReserializeAssets();
        }
    }
}