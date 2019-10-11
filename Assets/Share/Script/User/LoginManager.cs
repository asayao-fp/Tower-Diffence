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

    public void signUp(){
        instance.signUp(signnametext.text,signpasstext.text);

    }

    public void logIn(){
        instance.logIn(loginnametext.text,loginpasstext.text);
    }

    public void showloginPanel(){
        loginpanel.gameObject.SetActive(!loginpanel.gameObject.activeSelf);
    }

    public void showsignpanel(){
        sinuppanel.gameObject.SetActive(!sinuppanel.gameObject.activeSelf);
    }
}
