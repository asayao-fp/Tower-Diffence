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
            }
        }else{
            Destroy(gameObject);
        }
    }
    private string currentPlayerName;

    public void logIn(String id,String pw){
        NCMBUser.LogInAsync(id,pw,(NCMBException e) => {
            if(e == null){
                currentPlayerName = id;
                GameSettings.printLog("[UserAuth] LOGIN!! name : " + currentPlayerName);

                NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("userdata");
                query.WhereEqualTo("name",currentPlayerName);
                query.FindAsync((List<NCMBObject> objList,NCMBException ex) =>{
                    if(ex == null){
                        PlayerPrefs.SetString(UserData.USERDATA_NAME,(string)objList[0]["name"]);
                        PlayerPrefs.SetInt(UserData.USERDATA_EXP,System.Convert.ToInt32(objList[0]["exp"]));
                        PlayerPrefs.SetInt(UserData.USERDATA_LEVEL,System.Convert.ToInt32(objList[0]["level"]));
                        for(int i=1;i<6;i++){
                            PlayerPrefs.SetString(UserData.USERDATA_SETOBJ + i,(string)objList[0]["setobj_" + i]);
                        }                            
                    }
                });
  
            	SceneManager.LoadScene ("StageSelectScene");
            }
        });
    }

    public void signUp(String id,String pw){
        NCMBUser user = new NCMBUser();
        user.UserName = id;
        user.Password = pw;
        user.SignUpAsync((NCMBException e) =>{
            if(e == null){
                currentPlayerName = id;
                GameSettings.printLog("[UserAuth] SIGN UP!! name : " + currentPlayerName);
            }
        });

        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("userdata");
        query.WhereEqualTo("name",currentPlayerName);
        query.FindAsync((List<NCMBObject> objList,NCMBException ex) =>{
            if(ex == null){
                //初回登録
                if(objList.Count == 0){
                    NCMBObject obj = new NCMBObject("userdata");
                    obj["name"] = currentPlayerName;
                    obj["exp"] = 0;
                    obj["level"] = 1;
                    for(int i=1;i<6;i++){
                        obj["setobj_" + i] = "facility_" + i;
                    }

                    obj.SaveAsync();

                    PlayerPrefs.SetString(UserData.USERDATA_NAME,(string)obj["name"]);
                    PlayerPrefs.SetInt(UserData.USERDATA_EXP,System.Convert.ToInt32(obj["exp"]));
                    PlayerPrefs.SetInt(UserData.USERDATA_LEVEL,System.Convert.ToInt32(obj["level"]));
                    for(int i=1;i<6;i++){
                        PlayerPrefs.SetString(UserData.USERDATA_SETOBJ + i,(string)obj["setobj_" + i]);
                    }                            
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

        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("userdata");
        query.WhereEqualTo("name",currentPlayerName);
        query.FindAsync((List<NCMBObject> objList,NCMBException ex) =>{
            if(ex == null){
                objList[0]["exp"] = PlayerPrefs.GetInt(UserData.USERDATA_EXP,0);
                objList[0]["level"] = PlayerPrefs.GetInt(UserData.USERDATA_LEVEL,1);
                for(int i=1;i<6;i++){
                    objList[0]["setobj_" + i] = PlayerPrefs.GetString(UserData.USERDATA_SETOBJ + i,"facility_" + i);
                }

                objList[0].SaveAsync();
            }
        });

    }
}
