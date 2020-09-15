using System;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEngine;

namespace CustomPackages.Silicom.Core.Editor.Tools
{
    [InitializeOnLoad]
    public class DisableEditorShortcutsOnPlay
    {
        private const string MenuName = "Tools/Shortcuts/Switch to EmptyProfil in PlayMode";
        private const string EmptyProfile = "EmptyProfile";

        static DisableEditorShortcutsOnPlay()
        {
            ShortcutManager.instance.activeProfileId = ShortcutManager.defaultProfileId;
            EditorApplication.playModeStateChanged += DetectPlayModeState;
        }

        [MenuItem("Tools/Shortcuts/Create EmptyProfile")]
        private static void CreateEmptyProfile()
        {
            try
            {
                ShortcutManager.instance.CreateProfile(EmptyProfile);
            }
            catch (Exception)
            {
                Debug.Log("EmptyProfile already exist !");
                return;
            }

            ShortcutManager.instance.activeProfileId = EmptyProfile;
            foreach (var pid in ShortcutManager.instance.GetAvailableShortcutIds())
                ShortcutManager.instance.RebindShortcut(pid, ShortcutBinding.empty);
            ShortcutManager.instance.activeProfileId = ShortcutManager.defaultProfileId;
            Debug.Log("EmptyProfile created.");
        }

        [MenuItem("Tools/Shortcuts/Delete EmptyProfile")]
        private static void DeleteEmptyProfile()
        {
            try
            {
                ShortcutManager.instance.DeleteProfile(EmptyProfile);
            }
            catch (Exception)
            {
                Debug.Log("EmptyProfile does not exist !");
            }
        }


        [MenuItem(MenuName, false, -10)]
        private static void ToggleMenu()
        {
            bool enabled = !EditorPrefs.GetBool(MenuName);
            Menu.SetChecked(MenuName, enabled);
            EditorPrefs.SetBool(MenuName, enabled);
        }

        private static void DetectPlayModeState(PlayModeStateChange state)
        {

            switch (state)
            {
                case PlayModeStateChange.EnteredPlayMode:
                    if (!EditorPrefs.GetBool(MenuName, false)) return;
                    ShortcutManager.instance.activeProfileId = EmptyProfile;
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    ShortcutManager.instance.activeProfileId = ShortcutManager.defaultProfileId;
                    break;
                case PlayModeStateChange.EnteredEditMode:
                    break;
                case PlayModeStateChange.ExitingEditMode:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
    }
}