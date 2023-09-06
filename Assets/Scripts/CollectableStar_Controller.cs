using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableStar_Controller : MonoBehaviour
{

    [SerializeField] int StarsNum;

    void Awake()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.name == "Player") {
            try {
                Game_Manager.Game_ManagerSin.CollectedStar(StarsNum);
            }
            catch ( System.IndexOutOfRangeException ) {
                Debug.Log("Star that out of List Number is existed!");
            }
            catch ( System.Exception exc ) {
                Debug.Log(exc.Message);
            }
        }

        gameObject.SetActive(false);
    }

}
