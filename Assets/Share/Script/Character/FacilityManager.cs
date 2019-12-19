
using UnityEngine;
using UnityEngine.UI;

public abstract class FacilityManager : MonoBehaviour
{
    /* AIかどうかのフラグ */
    public bool isAI;

    /* マテリアルの配列 */
    public MeshRenderer[] obj_materials;

    /* ゲーム中に変化するパラメータ */
    public struct gameStatus{
        public float hp;
    }

    /* ゲーム中に変化するパラメータ */
    protected gameStatus gstatus; 

    /* 自分のパラメータ */
    [SerializeField]
    protected StatueData statue;

    /* スキル割り振りのステータスを追加 */
    public void setAddStatus(AddStatus astatus){
        if(astatus == null) return;
        statue.hp += astatus.hp;
        statue.attack += astatus.attack;
        statue.atkInterval += (int)(astatus.speed * 0.15f);

        GameSettings.printLog("[StatueManager] name : " + astatus.name + " , hp : " + statue.hp + " , attack : " + statue.attack + " , speed : " + statue.atkInterval);
    }
    
    /* 死んでる時のフラグ */
    public bool isEnd;

    /*　死んだ時にフラグをセット */
    public void setEnd(){
        setEnd(false);
    }

    public void setEnd(bool isend)
    {
        isEnd = isend;
    }

    /* 攻撃するか */
    public bool check(){
        return isEnd || (gp != null && (gp.getStatus() != gp.NOW_GAME));
    }

    /* 攻撃時に呼ばれる */
    public abstract void Attack();

    /* 召喚時に呼ばれる */
    public virtual void Generate(Vector3 pos,StatueData f,bool isAI){
        setEnd(true);

        this.isAI = isAI;
        //初期化
        GameObject gameui = GameObject.Find("GameUI");
        foreach (Transform child in gameui.transform)
        {
            if (child.gameObject.name.Equals(this.gameObject.name))
            {
                gbm = child.gameObject.GetComponent<GenerateBarManager>();
            }
        }
        gp = GameObject.FindWithTag("GameManager").GetComponent<GameProgress>();
        generateEfect = ResourceManager.getObject("Other/" + generateName);
        deadEffect = ResourceManager.getObject("Other/" + deadName);
        atkEffect = ResourceManager.getObject("Attack/" + atkName);
        atkCheckEffect = ResourceManager.getObject("Other/" + atkCheckName);
        fieldEffect = ResourceManager.getObject("Other/" + FieldName);
        hpbar.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.3f, this.transform.position.z);
        hpbar.fillAmount = 1;
        gstatus.hp = statue.hp;
        
    }

    /* ユニークIDをセットする */
    public void setId(int id){
        obj_num = id;
    }

    /* HPを変更する */
    public void addHP(int hp){
      gstatus.hp += hp;
      if(gstatus.hp >= statue.hp){
        gstatus.hp = statue.hp;
      }
    
      if(hiteffect != null)hiteffect.Play(true);

    }

    /* 死んだ時に呼ばれる */
    public virtual void Dead(){
        //消滅エフェクト実行
        setView(false);

        GameObject geneObj = Instantiate(deadEffect, transform.position, Quaternion.identity) as GameObject;
        geneObj.transform.parent = this.transform;
        geneObj.transform.localPosition = deadEffect.transform.position;
        geneObj.transform.localScale = deadEffect.transform.localScale;
        geneObj.transform.localRotation = deadEffect.transform.localRotation;
        ParticleSystem p = geneObj.GetComponent<ParticleSystem>();
        p.Play();

        Destroy(this.gameObject, 2);

    }

    /* 設定を取得 */
    public StatueData getSData(){
        return statue;
    }

    /* 現在のゲーム中パラメータを取得 */
    public gameStatus getStatus(){
        return gstatus;
    }

    /* 現在のHPを取得 */
    public float getHP(){
        return hpbar.fillAmount;
    }

    /* HPを設定(オンライン通信で使用)*/
    public void setHP(float hp){
        hpbar.fillAmount = hp;
    }

    /* 敵が攻撃範囲内にいるか */
    public abstract void checkEnemy(); 

    /* オブジェクトのユニークid */
    public int obj_num; 

    /* スタチューかゴブリンか */
    public bool isStatue;

    /* 攻撃を終了する */
    public void attackEnd(){
        isAttacking = false;
    }

    /* 設置数を設定 */
    public void setNum(bool isgenerate){
        if(gbm == null)return;
        gbm.setNum(isgenerate);
    }

    /* 白黒設定用のマテリアル */
    [SerializeField]
    protected GameObject[] viewModels; 

    /* マテリアルの表示 */
    public void setView(){
        setView(true);
    }

    public void setView(bool isshow)
    {
        for (int i = 0; i < viewModels.Length; i++)
        {
            viewModels[i].gameObject.SetActive(isshow);
        }

    }

    /* GameProgress取得 */
    protected GameProgress gp;

    /* デバッグ用フラグ */
    public bool isDebug;

    /* HPバー */
    public Image hpbar;

    /* ヒットエフェクト用パーティクル */
    public ParticleSystem hiteffect;

    /* 攻撃用のエフェクト */
     protected GameObject atkEffect; 

    /* 召喚用エフェクト */
    protected GameObject generateEfect; 

    /* 召喚用エフェクト2 */
    protected GameObject fieldEffect;

    /* 死んだ時のエフェクト */
    protected GameObject deadEffect;

    /* 攻撃可能範囲のエフェクト */
    protected GameObject atkCheckEffect;

    /* 攻撃の名前(Resource読み込み) */
    public string atkName;

    /* 召喚の名前(Resource読み込み) */
    public string generateName;

    /* 召喚の名前2(Resource読み込み) */
    public string FieldName;

    /* 死亡エフェクトの名前(Resource読み込み) */
    public string deadName;

    /* 攻撃可能範囲の名前(Resource読み込み) */
    public string atkCheckName;

    /* 攻撃中かどうかのフラグ */
    public bool isAttacking; 

    /* 召喚数管理用 */
    protected GenerateBarManager gbm;

    /* マテリアルを設定 */
    protected void initMaterial(Material material){
        if(obj_materials == null) return;
        for (int i = 0; i < obj_materials.Length; i++)
        {
            obj_materials[i].material = material;
        }
        for (int i = 0; i < viewModels.Length; i++)
        {
            if (viewModels[i].gameObject.name.Equals("Canvas"))
            {
                viewModels[i].SetActive(false);
            }
        }
    }
}

