using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player_StatsController : MonoBehaviour
{
    [SerializeField] Player_AbilityController _playerAbilityController;

    [System.Serializable]
    public struct PlayerStats {
        public float MaxHP, CurretHP;
        public float MaxMP, MPGenerateRate, CurrentMP;
    }

    [SerializeField] public PlayerStats playerStats;

    void Awake()
    {
        StatsInitialize();
    }

    void StatsInitialize()
    {
        playerStats.CurrentMP = playerStats.MaxMP;
    }

    void FixedUpdate() // prevent the timer from the effect of bullet time ability.
    {
        //Debug.Log(Time.time+" "+LastMPCostTime);
        MPRegainTimer();
    }

    [SerializeField] float AutomaticallyRegainMPTime = 3f; // If the MP doesn't be costed in x secs, automatically regain.
    float LastMPCostTime = float.MaxValue;

    void MPRegainTimer()
    {
        if (playerStats.CurrentMP >= playerStats.MaxMP) return;

        if (Time.time >= LastMPCostTime + AutomaticallyRegainMPTime)
        {
            Debug.Log("Regaining MP");
            playerStats.CurrentMP += playerStats.MPGenerateRate;
            if (playerStats.CurrentMP + playerStats.MPGenerateRate > playerStats.MaxMP) {
                playerStats.CurrentMP = playerStats.MaxMP;
            }
            UI_Manager.UI_ManagerSin.UpdateStatsDisplay();
        }
    }

    /*public IEnumerator MPRegenerate()
    {
        while (true) {
            if (playerStats.CurrentMP + playerStats.MPGenerateRate > playerStats.MaxMP) {
                //_playerAbilityController.StopRegainMP();
                playerStats.CurrentMP = playerStats.MaxMP;
                UI_Manager.UI_ManagerSin.UpdateStatsDisplay();

                yield break;
            }

            playerStats.CurrentMP += playerStats.MPGenerateRate;
            UI_Manager.UI_ManagerSin.UpdateStatsDisplay();
            yield return new WaitForFixedUpdate();
        }
    }*/

    public bool CostMP(float cost)
    {
        if (playerStats.CurrentMP - cost < 0) return false;

        playerStats.CurrentMP -= cost;
        UI_Manager.UI_ManagerSin.UpdateStatsDisplay();

        LastMPCostTime = Time.time;
        return true;
    }


}
