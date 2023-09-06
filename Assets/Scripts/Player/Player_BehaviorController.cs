using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_BehaviorController : BehaviorController
{

    [SerializeField] Transform HandPos;

    InputMaster inputControl;

    [SerializeField] LayerMask _CanInteractLayer;
    [SerializeField] float _CanInteractRadius = 3f;

    bool IsInteracting = false;

    void Awake()
    {
        InputSystemInitialize();

    }

    void InputSystemInitialize()
    {
        inputControl = new InputMaster();
        inputControl.Enable();

        inputControl.Player.Interact.performed += content => ExcuteInteract();
    }

    void Update()
    {
        InteractCheck();
    }

    private void Start()
    {
        IsInteracting = false;
    }

    GameObject currentInteractableOjb = null;

    bool InteractCheck() // Can only interact one thing at the same time.
    {
        if ( IsInteracting ) return false;

        Collider2D[] results = Physics2D.OverlapCircleAll(transform.position, _CanInteractRadius, _CanInteractLayer) ;
        if (results.Length != 0)
        {
            GameObject mostclosedObj = null;
            float mostclosedValue = float.MaxValue;

            foreach (Collider2D collider in results)
            {
                //Distance compare.
                if (Vector2.SqrMagnitude(gameObject.transform.position - collider.transform.position) < mostclosedValue)
                {
                    mostclosedValue = Vector2.SqrMagnitude(gameObject.transform.position - collider.transform.position);
                    mostclosedObj = collider.gameObject;
                }
            }
            Item_BehaviorController itemBehaviour = mostclosedObj.gameObject.GetComponent<Item_BehaviorController>();
            StartCoroutine(itemBehaviour.CanBeInteractedCall());
            currentInteractableOjb = mostclosedObj;

            //Debug.Log("interactable!");
            return true;
        }
        else
            currentInteractableOjb = null;
            return false;
    }

    GameObject PickedObj = null;
    [SerializeField] float ThrowPower = 500f;

    void ExcuteInteract()
    {
        if ( IsInteracting )
        {
            if (!PickedObj.GetComponent<Item_BehaviorController>().BeThrowed(new Vector2(transform.localScale.x, 1f), ThrowPower))
            {
                Debug.Log("Failed to throw sth");
                return;
            }
            
            PickedObj = null;
            IsInteracting = false;

            return;
        }

        if (currentInteractableOjb != null)
        {
            Item_BehaviorController item_behaviourCon = currentInteractableOjb.GetComponent<Item_BehaviorController>();

            switch ( item_behaviourCon._InteractType )
            {
                case InteractionType.Null:

                    Debug.Log(currentInteractableOjb.name + " is interacting but type is null!");
                    break;
                case InteractionType.TwoSteps:

                    PickedObj = currentInteractableOjb;
                    PickedObj.GetComponent<Item_BehaviorController>().BePicked( HandPos );
                    IsInteracting = true;

                    break;
                case InteractionType.Once:
                    Debug.Log(currentInteractableOjb.name + "is interacting Once-typed obj??");
                    break;
            }

        }
        else
        {
            Debug.Log("There is no interactable Object!");
            return;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, _CanInteractRadius);
    }

}
