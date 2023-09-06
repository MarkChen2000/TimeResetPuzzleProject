using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{

    public static Game_Manager Game_ManagerSin;

    public static Transform RecordedObjsTransSin; // Not really sure that can singleton used like this.

    [SerializeField] Player_AbilityController player_AbilityController;

    void Awake()
    {
        //Singleton
        if (Game_ManagerSin != null && Game_ManagerSin != this) Destroy(this);
        else Game_ManagerSin = this;

        if (!GameObject.Find("WorldObjects")) Debug.Log("This level doesnt have necessary transfrom for timereset ablility!");
        else if (RecordedObjsTransSin != null) Destroy(RecordedObjsTransSin);
        else RecordedObjsTransSin = GameObject.Find("WorldObjects").transform ; // every level should has this transfrom!
    }

    void Start()
    {
        RecordInitialStatment();

        InitializeLevelStats();
    }


    [System.Serializable]
    struct ObjectInfo {
        public Transform Transform; // transforms are class, it can be save as site.

        public Vector3 Position; 
        public Quaternion Rotation; // these can be save as value.
    }

    List<ObjectInfo> ObjsInfoList = new List<ObjectInfo>();
    int RecordedObjsNum = 0;

    void RecordInitialStatment()
    {
        RecordedObjsNum = RecordedObjsTransSin.childCount;

        for (int i = 0; i < RecordedObjsNum; i++) {
            ObjectInfo objinfo;
            objinfo.Transform = RecordedObjsTransSin.GetChild(i).transform;
            objinfo.Position = objinfo.Transform.position;
            objinfo.Rotation = objinfo.Transform.rotation;
            ObjsInfoList.Add(objinfo);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ( Input.GetKeyDown(KeyCode.R) ) TimeReset() ;
    }

    void TimeReset()
    {
        for (int i = 0; i < RecordedObjsNum; i++) {
            ObjectInfo objinfo;
            objinfo.Transform = ObjsInfoList[i].Transform;

            if (objinfo.Transform.parent != RecordedObjsTransSin) continue; // it means the obj is locked, doesnt need to reset.

            objinfo.Transform.position = ObjsInfoList[i].Position;
            objinfo.Transform.rotation = ObjsInfoList[i].Rotation;

            Rigidbody2D rigid;
            if (rigid = objinfo.Transform.GetComponent<Rigidbody2D>()) {
                rigid.bodyType = RigidbodyType2D.Dynamic;
                rigid.velocity = Vector2.zero;
                rigid.angularVelocity = 0f;
            }
        }

        Debug.Log("Reseted Time!!");
    }

    [SerializeField] int NumberOfStars = 3;
    List<bool> StarsStats = new List<bool>();

    void InitializeLevelStats()
    {
        StarsStats.Clear();

        for (int i = 0; i < NumberOfStars; i++) {
            StarsStats.Add(false);
        }
    }

    public void CollectedStar(int num)
    {
        if (StarsStats[num - 1]) {
            throw new System.Exception("Same number of Stars be collected!");
        }
        StarsStats[num - 1] = true;
    }


    public void LevelComplete()
    {
        player_AbilityController.StopAllCoroutines();

        UI_Manager.UI_ManagerSin.ShowGameResultAndClearStats( StarsStats );
        Debug.Log("Level Completed!");
    }

    public void ResetLevel()
    {
        Debug.Log("Level Reset!");
    }

}
