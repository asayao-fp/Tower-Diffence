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
}



