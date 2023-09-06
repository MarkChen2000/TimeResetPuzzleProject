using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_BehaviorController : BehaviorController
{
    //[SerializeField] bool UsingShader = false;
    [SerializeField] SpriteRenderer _SpriteRenderer;
    [SerializeField] Rigidbody2D _RidigBody;
    [SerializeField] Collider2D _Collider;

    public InteractionType _InteractType = InteractionType.Null;

    void Awake()
    {

    }

    // indicator color multiply
    [SerializeField] float DefaultMultiplyVal = 0f; 
    [SerializeField] float MultiplyVal = 0.25f;

    public IEnumerator CanBeInteractedCall()
    {
        _SpriteRenderer.material.SetFloat("_MultiplyVal", MultiplyVal);
        
        yield return new WaitForEndOfFrame(); // next frame automatically turn back to default.
        _SpriteRenderer.material.SetFloat("_MultiplyVal", DefaultMultiplyVal);
    }

    /*public void BeInteracted( GameObject interacter )
    {
        switch ( _InteractType ) {
            case InteractionType.Null:

                break;
            case InteractionType.PickandThrow:
                
                break;
            case InteractionType.Switch:

                break;
        }
    }*/

    public void BePicked( Transform stickParentTrans )
    {
        Debug.Log("Pick " + gameObject.name);

        if (IsBeTimeStoped) BeReleaseTimeStop();

        transform.SetParent(stickParentTrans);
        transform.localPosition = Vector3.zero;

        transform.localScale = Vector3.one * 1f;

        _RidigBody.bodyType = RigidbodyType2D.Kinematic;
        _RidigBody.velocity = Vector2.zero;
        _RidigBody.angularVelocity = 0f;
        _Collider.enabled = false;
    }

    [SerializeField] ContactFilter2D Filters;
    List<Collider2D> touchedobjs = new List<Collider2D>();

    public bool BeThrowed(Vector2 throwDir, float throwPower)
    {
        touchedobjs.Clear();

        _Collider.enabled = true;

        if ( _Collider.OverlapCollider( Filters, touchedobjs ) !=0 ) {
            _Collider.enabled = false;
            /*foreach ( var i in touchedobjs )
            {
                Debug.Log("Touched " + i.name + ", failed to throw.");
            }*/
            Debug.Log("Touched "+touchedobjs[0].name+", failed to throw.");
            return false; //fail to throw.
        }

        transform.SetParent( Game_Manager.RecordedObjsTransSin ); // the objs in this transform means that it will be reset by timeret ability.

        _RidigBody.bodyType = RigidbodyType2D.Dynamic;

        transform.localScale = Vector3.one * 1f;
        _RidigBody.velocity = Vector2.zero;
        _RidigBody.angularVelocity = 0f;
        _RidigBody.AddForce(throwDir.normalized * throwPower);

        //Debug.Log("Throw " + gameObject.name);
        return true; //successfully throwed.
    }

    [HideInInspector] public bool IsBeTimeStoped = false;

    Coroutine TSCoroutine;

    public void BeTimeStoped()
    {
        _RidigBody.bodyType = RigidbodyType2D.Kinematic;
        _RidigBody.velocity = Vector2.zero;
        _RidigBody.angularVelocity = 0f;

        Debug.Log("Time Stop a object: " + gameObject.name.ToString());

        TSCoroutine = StartCoroutine(CountForTimeStop());
        IsBeTimeStoped = true;
    }

    IEnumerator CountForTimeStop()
    {
        yield return new WaitForSecondsRealtime(5f);
        IsBeTimeStoped = false;
        BeReleaseTimeStop();
    }

    public void BeReleaseTimeStop()
    {
        Debug.Log("Relase a Time Stoped object: " + gameObject.name.ToString());

        if (IsBeTimeStoped) {
            StopCoroutine(TSCoroutine);
            IsBeTimeStoped = false;
        }

        _RidigBody.bodyType = RigidbodyType2D.Dynamic;
    }



}
