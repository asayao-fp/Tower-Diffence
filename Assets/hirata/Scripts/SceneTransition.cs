
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //ステージボタンが押されたときのコールバック。ボタンのオブジェクト名から遷移先を決定する。
    public void OnStageSelect()
    {
        SceneManager.LoadScene(this.gameObject.name);
    }

    public void OnBackButton()
    {
        string nowScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(GetPrevSceneName(nowScene));
    }

    //前のシーン名を取得する
    public string GetPrevSceneName(string nowScene)
    {
        string prevScene = Constants.TITLE_SCNENE;

        if (nowScene.Equals(Constants.TITLE_SCNENE))
        {
            prevScene = Constants.TITLE_SCNENE;
        }
        else if (nowScene.Equals(Constants.LOGIN_SCNENE))
        {
            prevScene = Constants.TITLE_SCNENE;

        }
        else if (nowScene.Equals(Constants.MENU_SCNENE))
        {
            prevScene = Constants.LOGIN_SCNENE;

        }
        else if (nowScene.Equals(Constants.CONNECT_ROOM_SCNENE))
        {
            prevScene = Constants.MENU_SCNENE;

        }
        else if (nowScene.Equals(Constants.STAGE_SELECT_SCNENE))
        {
            prevScene = Constants.MENU_SCNENE;

        }
        else if (nowScene.Equals(Constants.GAME_SET_SCNENE))
        {
            prevScene = Constants.STAGE_SELECT_SCNENE;

        }



        return prevScene;
    }

}