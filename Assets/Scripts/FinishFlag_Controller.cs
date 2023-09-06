using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishFlag_Controller : MonoBehaviour
{
    BoxCollider2D _collider;

    void Awake()
    {
        _collider = this.GetComponent<BoxCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.name == "Player")
        {
            Game_Manager.Game_ManagerSin.LevelComplete();
            _collider.enabled = false;
        }
    }

}
