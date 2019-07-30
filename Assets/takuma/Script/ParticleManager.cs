using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{

  private void OnParticleSystemStopped(){
    Debug.Log("Particle end ");
    Destroy(this.gameObject);
  }
}
