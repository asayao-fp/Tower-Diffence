using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UseItemManager : MonoBehaviour
{
    UserBaggage ub;

    public GameObject[] items;
    public TextMeshProUGUI[] itemlabels;
    private int layoutMax = 6; //一列に表示できる数

    public GameObject showDescPanel;
    public TextMeshProUGUI desc;

    private int type;//選択中のアイテム

    /**
    * アイテムレイアウト
    * 左上 -> -250,100 隣　-> -150,100 (x += 100)
    * 左   -> -250,0 (y -= 100);¥
    * 左下 -> -250,-100
    */

    public void setBaggage(){
        if(ub == null){
            ub = FindObjectOfType<UserAuth>().userBaggage;
        }

        Vector2 layoutPos = new Vector2(-250,100);
        int layoutnum = 0; //現在の列数
        int layoutheight = 0; //現在の行数

        for(int i=0;i<items.Length;i++){
            if(ub.getItem(i) > 0){
                items[i].transform.localPosition = layoutPos;
                itemlabels[i].text = "X " + ub.getItem(i);
                layoutnum++;
                if(layoutnum == layoutMax){
                    layoutPos.Set(-250,layoutPos.y - 100);
                }else{
                    layoutPos.x += 100;
                }
            }else{
                items[i].gameObject.SetActive(false);
            }

        }

    }

    public void useItem(){
        GameSettings.printLog("[UseItemManager] userItem num : " + type + " has : " + ub.getItem(type));

        if(ub.getItem(type) > 0){
            ub.useItem(type);
            int exp = PlayerPrefs.GetInt(UserData.USERDATA_EXP);
            switch(type){
                case 0:
                    PlayerPrefs.SetInt(UserData.USERDATA_EXP,exp + 10);
                    break;
                case 1:
                    PlayerPrefs.SetInt(UserData.USERDATA_EXP,exp + 100);
                    break;
                case 2:
                    PlayerPrefs.SetInt(UserData.USERDATA_EXP,exp + 1000);
                    break;
                case 3:
                    //expboostはgamesettingsにそれ用のフラグ追加？
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    //レベルアップ
                    break;
                case 7:
                    //ガチャ
                    break;
            }
        }

        closeShowDesc();
        setBaggage();
    }

    public void closeShowDesc(){
        showDescPanel.SetActive(false);
    }
    public void showDesc(int type){
        if(showDescPanel.activeSelf){
            showDescPanel.SetActive(false);
            return;
        }


        showDescPanel.SetActive(true);
        string desctext = "";
        switch(type){
            case 0:
                desctext = "経験値を獲得します（小）\n";
                break;
            case 1:
                desctext = "経験値を獲得します（中）\n";
                break;
            case 2:
                desctext = "経験値を獲得します（大）\n";
                break;
            case 3:
                desctext = "経験値獲得量が増加します（小）\n";
                break;
            case 4:
                desctext = "経験値獲得量が増加します（中）\n";
                break;
            case 5:
                desctext = "経験値獲得量が増加します（小）\n";
                break;
            case 6:
                desctext = "レベル上限がアップします\n";
                break;
            case 7:
                desctext = "ゲームで使用するアイテムをランダムで使用します（小）\n";
                break;
        }

        desc.text = desctext;
        this.type = type;
    }
}
