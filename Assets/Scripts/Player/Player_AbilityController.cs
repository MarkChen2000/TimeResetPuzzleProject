using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Player_ActiveAbilities
{
    Null, TimeStop, TimeRewind, TimeReset
}

public enum Player_PassiveAbilities
{
    Null, BulletTime
}

public class Player_AbilityController : MonoBehaviour
{
    InputMaster inputControl;
    [SerializeField] Player_StatsController player_StatsController;

    [SerializeField] Player_ActiveAbilities CurrentEquipedAcAbility = Player_ActiveAbilities.Null;
    [SerializeField] Player_PassiveAbilities CurrentEquipedPaAbility = Player_PassiveAbilities.Null;

    void Awake()
    {
        InputSystemInitialize();
    }

    void InputSystemInitialize()
    {
        inputControl = new InputMaster();
        inputControl.Enable();

        inputControl.Player.AcAbilityExcute.performed += content => ExcuteAcAbility();
        inputControl.Player.PaAbilityExcute.performed += content => ExcutePaAbility();
    }

    // Update is called once per frame
    void Update()
    {
        //CheckCanExcuteAcAbility();

        //Debug.Log("Current time scale is: "+Time.timeScale+". Fixedtimescale is: "+Time.fixedDeltaTime);
    }
    
    //Active Abilities

    void ExcuteAcAbility()
    {
        switch (CurrentEquipedAcAbility) {
            case Player_ActiveAbilities.Null:
                break;
            case Player_ActiveAbilities.TimeStop:
                ExcuteTimeStop();
                break;
        }

        Debug.Log("Try Excuting ability: " + CurrentEquipedAcAbility);
    }

    // Active Ability : Time Stop

    //[SerializeField] float CanInteractRadiusofTS = 3f;
    [SerializeField] int CostOfTS = 20;
    [SerializeField] ContactFilter2D InteractableFilter;

    void ExcuteTimeStop()
    {

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider == null) {
            Debug.Log("Didnt hit anything! Failed to excute ability Time Stop.");
            return;
        }

        if (hit.transform.name == "Player") {
            Debug.Log("You can not stop ME!!");
            return;
        }

        GameObject gb = hit.collider.gameObject;
        Item_BehaviorController item_behacon = gb.GetComponent<Item_BehaviorController>();
            
        Debug.Log("Hit " + gb.name);
            
        if (!item_behacon.IsBeTimeStoped) {
            
            if (player_StatsController.playerStats.CurrentMP < CostOfTS) {
                Debug.Log("Not enough MP! Failed to excute ability Time Stop.");
                return;
            }
            item_behacon.BeTimeStoped();
            player_StatsController.CostMP(CostOfTS);
        }
        else {
            item_behacon.BeReleaseTimeStop();
        }
    }



    // Passive Ability

    void ExcutePaAbility()
    {
        switch (CurrentEquipedPaAbility) {
            case Player_PassiveAbilities.Null:
                break;
            case Player_PassiveAbilities.BulletTime:
                ExcuteBulletTime();
                break;
        }

        Debug.Log("Try Excuting ability: " + CurrentEquipedPaAbility);
    }

    // Passive Ability : Bullet Time 

    //[SerializeField] float BTCoolTime = 5f;
    [SerializeField] float BTTimeScale = 0.5f; // how slow will the timescale be when active.
    [SerializeField] float BTChangeScale = 0.5f; // how fast will the timescle turning to target scale.
    [SerializeField] float BTMPCostPerFrame = 10f; // the cost of every frame.

    bool canExcuteBT = true; // prevent excute again in the fade in our fade out of BT.
    bool isActivingBT = false;
    bool isRegenerateMP = false;

    Coroutine _fadeInCor, _bulletTimeCor, _fadeOutCor, _regainMPCor;

    void ExcuteBulletTime()
    {
        if (!canExcuteBT) {
            Debug.Log("Still in the process of fade in or fade out BT.");
            return;
        }

        if (isActivingBT) {
            StopCoroutine(_bulletTimeCor);
            isActivingBT = false;
            _fadeOutCor = StartCoroutine(FadeOutBT());
            return;
        }

        if (isRegenerateMP) {
            StopCoroutine(_regainMPCor);
            isRegenerateMP = false;
        }

        if (player_StatsController.playerStats.CurrentMP <= 0) {
            Debug.Log("Not enough MP!");
            return;
        }

        _fadeInCor = StartCoroutine(FadeInBT());
    }

    IEnumerator FadeInBT()
    {
        canExcuteBT = false;
        while (true) {
            Time.timeScale -= BTChangeScale;
            if (Time.timeScale <= BTTimeScale) {
                Time.timeScale = BTTimeScale;
                break;
            }
            //Debug.Log("Current time scale is: " + Time.timeScale.ToString("0.00") + ". Fixedtimescale is: " + Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }
        canExcuteBT = true;
        _bulletTimeCor = StartCoroutine(BulletTime());
        yield return null;
    }

    IEnumerator BulletTime()
    {
        isActivingBT = true;
        while (player_StatsController.CostMP(BTMPCostPerFrame)) {
            yield return new WaitForFixedUpdate();
        }
        isActivingBT = false;
        _fadeOutCor = StartCoroutine(FadeOutBT());
        yield return null;
    }

    IEnumerator FadeOutBT()
    {
        canExcuteBT = false;
        while (true) {
            Time.timeScale += BTChangeScale;
            if (Time.timeScale >= 1f) {
                Time.timeScale = 1f;
                break;
            }
            //Debug.Log("Current time scale is: " + Time.timeScale.ToString("0.00") + ". Fixedtimescale is: " + Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }
        canExcuteBT = true;

        //RegainMP();
        yield return null;
    }

    /*void RegainMP()
    {
        isRegenerateMP = true;
        _regainMPCor = StartCoroutine(player_StatsController.MPRegenerate());
    }

    public void StopRegainMP()
    {
        StopCoroutine(_regainMPCor);
        isRegenerateMP = false;
    }*/

}
