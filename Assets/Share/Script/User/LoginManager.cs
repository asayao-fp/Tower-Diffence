using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System;

public class LoginManager : MonoBehaviour
{

    public TextMeshProUGUI signnametext;
    public TextMeshProUGUI signpasstext;

    public TextMeshProUGUI loginnametext;
    public TextMeshProUGUI loginpasstext;

    private bool isLogIn;

    private UserAuth instance;

    public GameObject sinuppanel;
    public GameObject loginpanel;

    void Start()
    {
        instance = GetComponent<UserAuth>();
        instance.logOut();

    }

    public void gotoMainPanel(){
        if(PlayerPrefs.GetString(UserData.USERDATA_NAME,"").Equals("")){
            showsignpanel();
        }else{
            instance.logIn(PlayerPrefs.GetString(UserData.USERDATA_NAME,""),"");
        }
    }

    public void signUp(){
        SoundManager.SoundPlay("click1",this.gameObject.name);
        instance.signUp(signnametext.text,"");

    }

    public void logIn(){
        SoundManager.SoundPlay("click1",this.gameObject.name);

        instance.logIn(loginnametext.text,"");
    }

    public void showloginPanel(){
        SoundManager.SoundPlay("click1",this.gameObject.name);


        loginpanel.gameObject.SetActive(!loginpanel.gameObject.activeSelf);
    }

    public void showsignpanel(){
        SoundManager.SoundPlay("click1",this.gameObject.name);

        sinuppanel.gameObject.SetActive(!sinuppanel.gameObject.activeSelf);
    }
}
