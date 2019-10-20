using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager4Callback : MonoBehaviour
{
 private void OnParticleCollision(GameObject other){
     Debug.Log("particlllslls : " + other.name);
 }
}
