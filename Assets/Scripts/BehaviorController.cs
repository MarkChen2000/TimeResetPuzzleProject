using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BehaviorControllerType
{
    Null, Player, Item, NPC
}

public enum InteractionType
{
    Null, Once, TwoSteps
}

public class BehaviorController : MonoBehaviour
{

    // [SerializeField] BehaviourControllerType _BehaviourControllerType = BehaviourControllerType.Null;

    void Awake()
    {
        
    }

}
