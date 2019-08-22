using UnityEngine;

public abstract class FacilityManager : MonoBehaviour
{
   
    public abstract void Attack();
    public abstract void Generate(Vector3 pos,Vector3 scale,Facility f);
    public abstract void EnemyOnArea(GameObject obj);
    public abstract void setId(int id);
    public abstract void addHP(int hp);
    public abstract void Dead();
    public abstract Facility getFinfo();

}
