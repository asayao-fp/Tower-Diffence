using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class GameInitializer : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        //リリースビルドの時はなにもしない
        if (!Debug.isDebugBuild)
        {
            return;
        }

        //カスタムメニューのデバックにチェックがついている場合のみ
        if (Menu.GetChecked(Constants.DEBUG_MODE_MENU_PATH))
        {
            //staticManagerをResourceフォルダからロード
            //Awakeで使われているため、シーンに配置できなかった
            var staticManager = Resources.Load("Manager/StaticManager") as GameObject;
            GameObject staticManagerObj = GameObject.Instantiate(staticManager);
            staticManagerObj.name = staticManager.name;

            //skillNumManagerをResourceフォルダからロード
            //InitUserDataのnextSceneの処理を呼ぶため
            var skillManager = Resources.Load("Manager/init") as GameObject;
            GameObject skillManagerObj = GameObject.Instantiate(skillManager);
            skillManagerObj.name = skillManager.name;


            //GameSettignの初期化処理
            GameObject staticObj = GameObject.FindWithTag("StaticObjects");
            GameObject skillObj = GameObject.FindWithTag("GameController");
            GameSettings gs;
            SkillNumManager snm;
            gs = staticObj.GetComponent<GameSettings>();
            gs.setOnlineType(false);
            gs.setStatue(Menu.GetChecked(Constants.DEBUG_IS_STATUE));
            snm = skillObj.GetComponent<SkillNumManager>();
            snm.Start();
            gs.setStatus(snm.getAllStatus());


            //Awakeで使われていなやつは、初期化シーンに配置して読み込み
            if (!SceneManager.GetSceneByName(Constants.INITIALIZE_SCENE).IsValid())
            {
                SceneManager.LoadScene(Constants.INITIALIZE_SCENE, LoadSceneMode.Additive);
            }
        }

    }
}
