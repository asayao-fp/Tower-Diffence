using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    public string showMessage;
    public Vector2 dialogSize;
    public DialogManager(string mes,Vector2 size){
        showMessage = mes;
        dialogSize = size;
    }

    public void show(){
        //ResourceManager.getObject)
        //Instantiate
        //AddCoponent<DialogManager>();
        //showme,dialog 代入
        
    }

    //インスタンス作る時にoverrideする
    public void okProcess(){

    }

    //インスタンス作る時にoverrideする
    public void closeProcess(){

    }

    void Start()
    {
        
    }
    void Update()
    {
        
    }
}
