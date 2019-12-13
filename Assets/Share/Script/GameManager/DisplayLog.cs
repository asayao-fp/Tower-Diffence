using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class DisplayLog : MonoBehaviour
{
    [SerializeField]
    private Text mytextUI = null;

    [SerializeField]
    private ScrollRect myScrollRect = null;

    private bool preCheck = false;
    private bool nowCheck = false;
    private string DEBUG_VIEW = "DebugView";

    void Update()
    {
        nowCheck = Menu.GetChecked(Constants.DEBUG_MODE_MENU_PATH);
        if (preCheck != nowCheck)
        {
            DisableDebugView(nowCheck);
        }
        preCheck = nowCheck;
    }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        //DontDestroyOnLoad(transform.Find(DEBUG_VIEW).gameObject);
        Application.logMessageReceived += OnLogMessage;
        preCheck = Menu.GetChecked(Constants.DEBUG_MODE_MENU_PATH);
    }

    private void OnDestroy()
    {
        Application.logMessageReceived += OnLogMessage;
    }

    private void OnLogMessage(string i_logText, string i_stackTrace, LogType i_type)
    {
        if (string.IsNullOrEmpty(i_logText) || !Menu.GetChecked(Constants.DEBUG_MODE_MENU_PATH))
        {
            return;
        }

        mytextUI.text += i_logText + System.Environment.NewLine;
        myScrollRect.verticalNormalizedPosition = 0;
        mytextUI.GetComponent<ContentSizeFitter>().SetLayoutVertical();

    }

    private void DisableDebugView(bool isDebugMode)
    {
        Transform debugView = transform.Find(DEBUG_VIEW);

        if (debugView == null)
        {
            Debug.LogError("DebugViewが見つかりませんでした。名前を「" + DEBUG_VIEW + "」にしてください");
            return;
        }

        debugView.gameObject.SetActive(isDebugMode);
    }
}