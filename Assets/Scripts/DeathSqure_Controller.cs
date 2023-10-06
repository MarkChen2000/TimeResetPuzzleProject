using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSqure_Controller : MonoBehaviour
{
    BoxCollider2D _collider;

    [SerializeField] GameObject PlayerSpawnSpot;

    void Awake()
    {
        _collider = this.GetComponent<BoxCollider2D>();
    }

    //bool firsttime = true;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.name == "Player")
        {
            /*if (!firsttime) return; //collider will collide with 2 colliders of player at the same frame.
            firsttime = false;*/

            collision.gameObject.transform.position = PlayerSpawnSpot.transform.position;
        }
    }

}
