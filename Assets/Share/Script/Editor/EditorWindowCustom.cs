using UnityEngine;
using UnityEditor;

public class EditorWindowCustom : EditorWindow
{
    [MenuItem("CustomMenu/DebugSetting")]
    static void OpenDebugWindow()
    {
        Menu.GetChecked("CustomMenu/DebugSetting");
        EditorWindow.GetWindow<EditorWindowCustom>("DebugSetting");

    }

    int radio = 0;

    void OnGUI()
    {
        radio = GUILayout.SelectionGrid(radio, new string[] { "Statue", "Goblin" }, 1, EditorStyles.radioButton);
    }
}