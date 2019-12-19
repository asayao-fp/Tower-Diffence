using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynchronizeManager : MonoBehaviour{
    public class NetObject{
      public string name;
      public Vector3 pos;
    }
    public class AttackObj{
        public int unique_id;
        public int attacktype;
    }
  
    public Dictionary<int,NetObject> net_objs;
    public Dictionary<int,GameObject> net_objs4manager;
    public Dictionary<int,int> net_addHpmanager;
    public Dictionary<int,AttackObj> net_attackManager;
    public int[] delete_objs;
    public int ncount; //上のnet_objsの数と合わせる
    public GameProgress4Online gp4Online;
    public GameSettings gs;
    public PhotonView pv;
    public GameObject pvMaster;//もう一つのphotonview
    public int gstatus; //ゲームのステータス
    public float gametime; //ゲーム時間
    public string countdown;
    public float crystalhp;// クリスタルのhp
    public int skilltype; //スキルタイプ

    public bool iscdead;

    public InputManager4Online i4online;
    public int acount;
     

    void Awake(){
        net_objs = new Dictionary<int,NetObject>();
        net_objs4manager = new Dictionary<int,GameObject>();
        net_addHpmanager = new Dictionary<int,int>();
        net_attackManager = new Dictionary<int,AttackObj>();
        delete_objs = new int[0];
        ncount = 0;
        acount = 0;
        gp4Online = GameObject.FindWithTag("GameManager").GetComponent<GameProgress4Online>();
        i4online = GameObject.FindWithTag("GameManager").GetComponent<InputManager4Online>();
        gs = GameObject.FindWithTag("StaticObjects").GetComponent<GameSettings>();
        pv = GetComponent<PhotonView>(); 
        gstatus = -1;

        crystalhp = -1;
        skilltype = -1;
        iscdead = false;
    }

    void Start(){
        GameObject[] objs = GameObject.FindGameObjectsWithTag("NetObj");
        for(int i=0;i<objs.Length;i++){
            if(objs[i].name.StartsWith("synchronize") && objs[i].gameObject != this.gameObject){
                pvMaster = objs[i].gameObject;
                break;
            }
        }
    }

    public void Set4CrystalHP(float chp){
        crystalhp = chp;
    }
    public void Set4AddHP(int unique_id,int hp){
        net_addHpmanager.Add(unique_id,hp);
    }
    public void Set4Manager(Dictionary<int,GameObject> objs){
        net_objs4manager = new Dictionary<int, GameObject>(objs);
    }
    

    public void Set4DeadObj(int[] deleteobjs){
        delete_objs = deleteobjs;
    }

    public void setCrystalDead(bool isdead){
        iscdead = isdead;
    }

    public void SetCountDown(string str){
        countdown = str;
    }
    //generateした時に呼ぶ
    public void Set(string name,Vector3 pos){
        NetObject no = new NetObject();
        no.name = name;
        no.pos = pos;
        net_objs.Add(ncount++,no);

    }

    public void SetAttack(int unique_id,int type){
        AttackObj ao = new AttackObj();
        ao.unique_id = unique_id;
        ao.attacktype = type;
        net_attackManager.Add(acount++,ao);
    }

    public void SetGameStatus(int status){

        gstatus = status;
    }

    public void SetGameTime(float time){
        gametime = time;
    }

    public void setSkillType(int type){
        skilltype = type;
    }

    public void doSkill4Net(int type){
        gp4Online.doSkill4Net(type);
    }


    //pv.isMine && isParent 送信だけ　(召喚する)
    //!pv.isMine && isParent 受信、送信

    //pv.isMine && !isParent 送信だけ(召喚はしない）
    //!pv.isMine && !isParent 親の受信（ここで召喚する）
    private int count = 0;
    void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info){

      if(stream.isWriting){
        
          stream.SendNext((string)"" + countdown + "__" + count);
          stream.SendNext((string)"" + gstatus + "__" + count);
          stream.SendNext((string)"" + gametime + "__" + count);
          stream.SendNext((string)("" + crystalhp + "__" + count));
          stream.SendNext((string)"" + (iscdead ? "true" : "false") + "__" + count);

          //召喚
          stream.SendNext((string)"" + net_objs.Count + "__" + count);
          if(net_objs.Count > 0){
              for(int i=0;i<net_objs.Count;i++){
                  string datastr = "generate_" + net_objs[i].name + "_x_" + net_objs[i].pos.x + "_y_" + net_objs[i].pos.y + "_z_" + net_objs[i].pos.z + "_roottype_" + i4online.roottype;
                  stream.SendNext((string)datastr);
              }
              ncount = 0;
              net_objs.Clear();
          }

          //攻撃マネージャー
          stream.SendNext((string)"" + net_attackManager.Count + "__" + count);
          if(net_attackManager.Count > 0){
              for(int i=0;i<net_attackManager.Count;i++){
                  string datastr = "fmattack_" + net_attackManager[i].unique_id + "_attack_" + net_attackManager[i].attacktype;
                  stream.SendNext((string)datastr);
              }
              acount = 0;
              net_attackManager.Clear();
          }
          
          //facilityのHP、位置同期
          stream.SendNext((string)("" + net_objs4manager.Count + "__" + count));
          if(net_objs4manager.Count > 0){
              foreach(KeyValuePair<int,GameObject> pair in net_objs4manager){
                  string datastr = "notexit__" + count;
                  if(pair.Value != null){
                      FacilityManager fm = pair.Value.GetComponent<FacilityManager>();
                      int attack = fm.isAttacking ? 1 : 0;
                      if(attack == 1){
                          gp4Online.setAttackFlag(pair.Key,false);
                      }
                      string fmstr = fm.isStatue ? "statue_" + attack : "gobrin_" + ((GobrinManager)fm).animator.GetInteger("state");
                    
                      Vector3 p = pair.Value.transform.position;
                      Vector3 r = pair.Value.transform.rotation.eulerAngles;
                      datastr = "facilitymanager_uniqueid_" + pair.Key + "_hp_" + fm.getHP() + "_x_" + p.x + "_y_" + p.y + "_z_" + p.z + "_rotx_" + r.x + "_roty_" + r.y + "_rotz_" + r.z + "_fm_" + fmstr;
                  }
                  stream.SendNext((string)datastr);
              }
          }

          //死んだオブジェクト同期
          stream.SendNext((string)("" + delete_objs.Length + "__" + count));
          if(delete_objs.Length > 0){
              for(int i=0;i<delete_objs.Length;i++){
                  stream.SendNext((int)delete_objs[i]);
              }
          }

/*
          stream.SendNext((string)("" + net_addHpmanager.Count + "__" + count));
          if(net_addHpmanager.Count > 0){
              foreach(KeyValuePair<int,GameObject> pair in net_addHpmanager){
                  string datastr = "noexit";
                  if(pair.Value != null){
                      stream.SendNext((string)"addhp_unique_id_" + pair.Key + "_hp_" + pair.Value)
                  }else{
                      stream.SendNext((string)datastr);
                  }
              }
              net_addHpmanager.Clear();
          }
*/



          //スキル発動
          stream.SendNext((string)("" + skilltype + "__" + count));
          if(skilltype != -1){
              skilltype = -1;
          }

          count++;
          if(count > 1000000){
              count = 0;
          }
          
      }
      else
      {
        //カウントダウン
        string cdown = (string)stream.ReceiveNext();
        cdown = cdown.Substring(0,cdown.IndexOf("__"));
        gp4Online.setCDown(cdown);

        //ゲームステータス
        string type = (string)stream.ReceiveNext();
        type = type.Substring(0,type.IndexOf("__"));
        gp4Online.setStatus(int.Parse(type));

        //ゲーム時間
        string time = (string)stream.ReceiveNext();
        time = time.Substring(0,time.IndexOf("__"));
        gp4Online.setTime(float.Parse(time));

        //クリスタルのhp
        type = (string)stream.ReceiveNext();        
        type = type.Substring(0,type.IndexOf("__"));
        if(float.Parse(type) != -1){
            gp4Online.setCrystalHP(float.Parse(type)); 
        }

        
        //クリスタル死んだか
        type = (string)stream.ReceiveNext();
        type = type.Substring(0,type.IndexOf("__"));
        if(type.Equals("true")){
            gp4Online.setCDead(true);
        }

        //召喚
        type = (string)stream.ReceiveNext();
        type = type.Substring(0,type.IndexOf("__"));

        if(int.Parse(type) > 0){
            for(int i=0;i<int.Parse(type);i++){
                string obj = (string)stream.ReceiveNext();
                //召喚
                if(obj.StartsWith("generate_")){        
                    int diffx = obj.IndexOf("_x_");
                    int diffy = obj.IndexOf("_y_");
                    int diffz = obj.IndexOf("_z_");
                    int diffroot = obj.IndexOf("_roottype_");
                    string name = obj.Substring(9,diffx - 9);
                    string posx = obj.Substring(diffx+3,diffy - diffx - 3);
                    string posy = obj.Substring(diffy+3,diffz - diffy - 3);
                    string posz = obj.Substring(diffz+3,diffroot - diffz - 3);
                    string root = obj.Substring(diffroot + 10);
                    GameSettings.printLog("[SynchronizeManager] name : " + name  + " x : " + float.Parse(posx) + " y : " + float.Parse(posy) +  " z : " + float.Parse(posz) +  " roottype : " + root);
                    Vector3 gpos = new Vector3(float.Parse(posx),float.Parse(posy),float.Parse(posz));
                    gp4Online.setInputRootType(int.Parse(root));
                    gp4Online.Generate(name,gpos,gs.isStatue(),gp4Online.isParent,false);

                    //自分が親で、相手から送られてきたものは自分側のSynchronizeManagerでもう一度送り返す
                    if(!pv.isMine && gp4Online.isParent){
                        pvMaster.GetComponent<SynchronizeManager>().Set(name,gpos);              
                    }
                }        
            }
        }

        type = (string)stream.ReceiveNext();
        type = type.Substring(0,type.IndexOf("__"));

        if(int.Parse(type) > 0){
            for(int i=0;i<int.Parse(type);i++){
                string obj = (string)stream.ReceiveNext();
                //攻撃
                if(obj.StartsWith("fmattack_")){
                    int diffat = obj.IndexOf("_attack_");
                    string unique = obj.Substring(9,diffat - 9);
                    string atype = obj.Substring(diffat + 8);
                    gp4Online.synchronizeAttack(int.Parse(unique),int.Parse(atype));
                    GameSettings.printLog("[SynchronizeManager] unique_id : " + unique + " attacktype : " + atype);
                }
            }
        }

        //HP管理
        type = (string)stream.ReceiveNext();
        type = type.Substring(0,type.IndexOf("__"));
        if(int.Parse(type) > 0){
            for(int i=0;i<int.Parse(type);i++){
                string receivestr = (string)stream.ReceiveNext();
                if(receivestr.StartsWith("facilitymanager_")){
                    int diffhp = receivestr.IndexOf("_hp_");
                    int diffx = receivestr.IndexOf("_x_");
                    int diffy = receivestr.IndexOf("_y_");
                    int diffz = receivestr.IndexOf("_z_");
                    int diffrx = receivestr.IndexOf("_rotx_");
                    int diffry = receivestr.IndexOf("_roty_");
                    int diffrz = receivestr.IndexOf("_rotz_");
                    int difffm = receivestr.IndexOf("_fm_");
                    int unique_id = int.Parse(receivestr.Substring(25,diffhp - 25));
                    float hp = float.Parse(receivestr.Substring(diffhp + 4,diffx - diffhp - 4));
                    float x = float.Parse(receivestr.Substring(diffx + 3,diffy - diffx - 3));
                    float y = float.Parse(receivestr.Substring(diffy + 3,diffz - diffy - 3));
                    float z = float.Parse(receivestr.Substring(diffz + 3,diffrx - diffz - 3));
                    float rx = float.Parse(receivestr.Substring(diffrx + 6,diffry - diffrx - 6));
                    float ry = float.Parse(receivestr.Substring(diffry + 6,diffrz - diffry - 6));
                    float rz = float.Parse(receivestr.Substring(diffrz + 6,difffm - diffrz - 6));
                    string fms = receivestr.Substring(difffm + 4);
                    gp4Online.synchronizePosHP(unique_id,hp,x,y,z,rx,ry,rz,fms);
                }
            }
        }

        //死亡したオブジェクト
        type = (string)stream.ReceiveNext();
        type = type.Substring(0,type.IndexOf("__"));
        if(int.Parse(type) > 0){
            for(int i=0;i<int.Parse(type);i++){
                int deletenum = (int)stream.ReceiveNext();
                if(deletenum != -1){
                    gp4Online.deadManager(deletenum);
                }
            }
        }


        //スキル発動
        type = (string)stream.ReceiveNext();
        type = type.Substring(0,type.IndexOf("__"));
        if(int.Parse(type) != -1){
            gp4Online.doSkill4Net(int.Parse(type));
            if(!pv.isMine && gp4Online.isParent){
//                pvMaster.GetComponent<SynchronizeManager>().setSkillType(int.Parse(type));
//                  pvMaster.GetComponent<SynchronizeManager>().doSkill4Net(int.Parse(type));
            }
        }

        //addHP(できたら）
      }
    }

}