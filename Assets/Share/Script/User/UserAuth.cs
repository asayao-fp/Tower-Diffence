using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;
using System;
using System.IO;
using UnityEngine.SceneManagement;

public class UserAuth : MonoBehaviour
{

    private UserAuth instance = null;

    public UserBaggage userBaggage;
    void Awake(){
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(this);

            string name = gameObject.name;
            gameObject.name = name + "(Singleton)";
            GameObject duplicater = GameObject.Find(name);
            if(duplicater != null){
                Destroy(gameObject);
            }else{
                gameObject.name = name;
                instance.userBaggage = GetComponent<UserBaggage>();
            }
        }else{
            Destroy(gameObject);
        }
    }
    private string currentPlayerName;

    public void logIn(String id,String pw){
        pw = "abcdefghijklmn123456789"; //passwordは固定

        NCMBUser.LogInAsync(id,pw,(NCMBException e) => {
            if(e == null){
                currentPlayerName = id;
                GameSettings.printLog("[UserAuth] LOGIN!! name : " + currentPlayerName);

                NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("userinfo");
                query.WhereEqualTo("name",currentPlayerName);
                query.FindAsync((List<NCMBObject> objList,NCMBException ex) =>{
                    if(ex == null){
                        PlayerPrefs.SetString(UserData.USERDATA_NAME,currentPlayerName);
                        PlayerPrefs.SetInt(UserData.USERDATA_EXP,System.Convert.ToInt32(objList[0]["exp"]));
                        PlayerPrefs.SetInt(UserData.USERDATA_LEVEL,System.Convert.ToInt32(objList[0]["level"]));
                        PlayerPrefs.SetString(UserData.USERDATA_STATUE,(string)objList[0]["statue"]);
                        PlayerPrefs.SetString(UserData.USERDATA_GOBLIN,(string)objList[0]["goblin"]);

                        userBaggage.addItem(0,System.Convert.ToInt32(objList[0]["expup_small"]));
                        userBaggage.addItem(1,System.Convert.ToInt32(objList[0]["expup_midium"]));
                        userBaggage.addItem(2,System.Convert.ToInt32(objList[0]["expup_big"]));
                        userBaggage.addItem(3,System.Convert.ToInt32(objList[0]["expboost_small"]));
                        userBaggage.addItem(4,System.Convert.ToInt32(objList[0]["expboost_midium"]));
                        userBaggage.addItem(5,System.Convert.ToInt32(objList[0]["expboost_big"]));
                        userBaggage.addItem(6,System.Convert.ToInt32(objList[0]["levelmax_up"]));
                        userBaggage.addItem(7,System.Convert.ToInt32(objList[0]["gacha_item"]));

                        for(int i=0;i<8;i++){
                            GameSettings.printLog("[UserAuth] userbaggage " + i + " num -> " + userBaggage.getItem(i));
                        }
                    }
                });
                
            	SceneManager.LoadScene ("StageSelectScene");
            }
        });
    }

    public void signUp(String id,String pw){
        pw = "abcdefghijklmn123456789"; //passwordは固定

        NCMBUser user = new NCMBUser();
        user.UserName = id;
        user.Password = pw;
        user.SignUpAsync((NCMBException e) =>{
            if(e == null){
                currentPlayerName = id;
                GameSettings.printLog("[UserAuth] SIGN UP!! name : " + currentPlayerName);
            }
        });

        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("userinfo");
        query.WhereEqualTo("name",currentPlayerName);
        query.FindAsync((List<NCMBObject> objList,NCMBException ex) =>{
            if(ex == null){
                //初回登録
                if(objList.Count == 0){
                    NCMBObject obj = new NCMBObject("userinfo");
                    obj["name"] = currentPlayerName;
                    obj["exp"] = 0;
                    obj["level"] = 1;
                    obj["statue"] = "facility_1,facility_2,facility_3,facility_4,facility_5";
                    obj["goblin"] = "gobrin_1,gobrin_2,gobrin_3";

                    obj["expup_small"] = 0;
                    obj["expup_midium"] = 0;
                    obj["expup_big"] = 0;
                    obj["expboost_small"] = 0;
                    obj["expboost_midium"] = 0;
                    obj["expboost_big"] = 0;
                    obj["levelmax_up"] = 0;
                    obj["gacha_item"] = 0;

                    obj.SaveAsync();

                    PlayerPrefs.SetString(UserData.USERDATA_NAME,(string)obj["name"]);
                    PlayerPrefs.SetInt(UserData.USERDATA_EXP,System.Convert.ToInt32(obj["exp"]));
                    PlayerPrefs.SetInt(UserData.USERDATA_LEVEL,System.Convert.ToInt32(obj["level"]));
                    PlayerPrefs.SetString(UserData.USERDATA_STATUE,(string)obj["statue"]);
                    PlayerPrefs.SetString(UserData.USERDATA_GOBLIN,(string)obj["goblin"]);
                    
                    /*
                    PlayerPrefs.SetInt(UserBaggage.EXPUP_S,System.Convert.ToInt32(obj["expup_small"]));
                    PlayerPrefs.SetInt(UserBaggage.EXPUP_M,System.Convert.ToInt32(obj["expup_midium"]));
                    PlayerPrefs.SetInt(UserBaggage.EXPUP_B,System.Convert.ToInt32(obj["expup_big"]));
                    PlayerPrefs.SetInt(UserBaggage.EXPBOOST_S,System.Convert.ToInt32(obj["expboost_small"]));
                    PlayerPrefs.SetInt(UserBaggage.EXPBOOST_M,System.Convert.ToInt32(obj["expboost_midium"]));
                    PlayerPrefs.SetInt(UserBaggage.EXPBOOST_B,System.Convert.ToInt32(obj["expboost_big"]));
                    PlayerPrefs.SetInt(UserBaggage.LEVELMAX_U,System.Convert.ToInt32(obj["levelmax_up"]));
                    PlayerPrefs.SetInt(UserBaggage.GACHA,System.Convert.ToInt32(obj["gacha_item"]));
                    */
                }
            }
        });
        
    }

    public void logOut(){
        NCMBUser.LogOutAsync((NCMBException e) => {
            if(e == null){
                currentPlayerName = null;
            }
        });
    }

    public string currentPlayer(){
        return currentPlayerName;
    }

    public void saveData(){

        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("userinfo");
        query.WhereEqualTo("name",currentPlayerName);
        query.FindAsync((List<NCMBObject> objList,NCMBException ex) =>{
            if(ex == null){
                objList[0]["exp"] = PlayerPrefs.GetInt(UserData.USERDATA_EXP,0);
                objList[0]["level"] = PlayerPrefs.GetInt(UserData.USERDATA_LEVEL,1);
                /*
                for(int i=1;i<6;i++){
                    objList[0]["setobj_" + i] = PlayerPrefs.GetString(UserData.USERDATA_SETOBJ + i,"facility_" + i);
                }
                */
                objList[0]["expup_small"] = userBaggage.getItem(0);
                objList[0]["expup_midium"] = userBaggage.getItem(1);
                objList[0]["expup_big"] = userBaggage.getItem(2);
                objList[0]["expboost_small"] = userBaggage.getItem(3);
                objList[0]["expboost_midium"] = userBaggage.getItem(4);
                objList[0]["expboost_big"] = userBaggage.getItem(5);
                objList[0]["levelmax_up"] = userBaggage.getItem(6);
                objList[0]["gacha_item"] = userBaggage.getItem(7);


                objList[0].SaveAsync();
            }
        });

    }


}