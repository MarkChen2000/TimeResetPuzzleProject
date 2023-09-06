using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Manager : MonoBehaviour
{

    [SerializeField] Player_StatsController player_StatsController;
    [SerializeField] Player_AbilityController player_AbilityController;
    
    [SerializeField] TMP_Text Text_Power, Text_TimeScale;
    [SerializeField] Slider Slider_Power, Slider_TimeScale;

    [SerializeField] TMP_Text Text_Ability;

    [SerializeField] Transform UI_Panel_Stars;
    [SerializeField] GameObject Obj_LevelResult, Obj_PlayerStats, Obj_StarsIcon, Obj_BlankStarIcon;

    public static UI_Manager UI_ManagerSin;
    void Awake()
    {
        //Singleton
        if (UI_ManagerSin != null && UI_ManagerSin != this) Destroy(this);
        else UI_ManagerSin = this;

        InitializeUI();
    }

    void InitializeUI()
    {
        Slider_TimeScale.maxValue = 1;

        Slider_Power.maxValue = player_StatsController.playerStats.MaxMP;
        //Text_Ability = player_AbilityController

        Obj_PlayerStats.SetActive(true);
        Obj_LevelResult.SetActive(false);
    }

    void Start()
    {
        UpdateStatsDisplay();
        UpdateTimeScaleDisplay();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateTimeScaleDisplay();
    }

    void UpdateTimeScaleDisplay()
    {
        Text_TimeScale.text = Time.timeScale.ToString("0.00");
        Slider_TimeScale.value = Time.timeScale;
    }

    public void UpdateStatsDisplay()
    {
        Text_Power.text = ((int)player_StatsController.playerStats.CurrentMP).ToString();
        Slider_Power.value = player_StatsController.playerStats.CurrentMP;
    }

    public void ShowGameResultAndClearStats( List<bool> scorestats )
    {
        Obj_PlayerStats.SetActive(false);
        Obj_LevelResult.SetActive(true);

        ShowTheScore(scorestats);

    }

    void ShowTheScore( List<bool> scorestats)
    {
        int i = 0;
        foreach ( var item in scorestats )
        {
            Debug.Log(item +" "+ i++);
            if ( item ) {
                Instantiate<GameObject>(Obj_StarsIcon, UI_Panel_Stars).SetActive(true);
            }
            else {
                Instantiate<GameObject>(Obj_BlankStarIcon, UI_Panel_Stars).SetActive(true);
            }
        }
    }

}
