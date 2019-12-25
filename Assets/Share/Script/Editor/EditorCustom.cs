using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorCustom : MonoBehaviour
{
    [MenuItem("CustomMenu/DebugMode")]
    static void SetDebugMode()
    {
        bool isDebugMode = Menu.GetChecked(Constants.DEBUG_MODE_MENU_PATH);

        //今設定されている値と逆にする
        Menu.SetChecked(Constants.DEBUG_MODE_MENU_PATH, !isDebugMode);
    }

    [MenuItem("CustomMenu/IsStatue")]
    static void SetDebugStatue()
    {
        bool isDebugStatus = Menu.GetChecked(Constants.DEBUG_IS_STATUE);
        Menu.SetChecked(Constants.DEBUG_IS_STATUE, !isDebugStatus);
    }
}



