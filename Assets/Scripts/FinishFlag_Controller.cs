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

    bool firsttime = true;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.name == "Player")
        {
            if (!firsttime) return; //collider will collide with 2 colliders of player at the same frame.
            firsttime = false;

            gameObject.SetActive(false);
            Game_Manager.Game_ManagerSin.LevelComplete();
        }
    }

}
