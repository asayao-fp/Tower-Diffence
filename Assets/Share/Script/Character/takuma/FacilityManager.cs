using UnityEngine;

public abstract class FacilityManager : MonoBehaviour
{
    /* マテリアルの配列 */
    public MeshRenderer[] obj_materials;

    /* ゲーム中に変化するパラメータ */
    public struct gameStatus{
        public float hp;
    }

    /* 死んでる時のフラグ */
    public bool isEnd;

    /* 攻撃するか */
    public abstract bool check();

    /* 攻撃時に呼ばれる */
    public abstract void Attack();

    /* 召喚時に呼ばれる */
    public abstract void Generate(Vector3 pos,Vector3 scale,StatueData f);

    /* 攻撃範囲内に敵がいるか(Statueのみ) */
    public abstract void EnemyOnArea(GameObject obj);

    /* ユニークIDをセットする */
    public abstract void setId(int id);

    /* HPを変更する */
    public abstract void addHP(int hp);

    /* 死んだ時に呼ばれる */
    public abstract void Dead();

    /* Statueの設定を取得 */
    public abstract StatueData getSData();

    /* Gobrinの設定を取得 */
    public abstract GobrinData getGData();

    /* 現在のゲーム中パラメータを取得 */
    public abstract gameStatus getStatus();

    /* 現在のHPを取得 */
    public abstract float getHP();

    /* 敵が攻撃範囲内にいるか */
    public abstract void checkEnemy(); 

    /* オブジェクトのユニークid */
    public int obj_num; 

    public bool isStatue;
}
