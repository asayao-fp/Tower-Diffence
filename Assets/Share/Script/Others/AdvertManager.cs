using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
public class AdvertManager : MonoBehaviour
{
    public void advertisementShow(){
        if(Advertisement.IsReady()){
            Advertisement.Show();
        }
    }
}
