using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PowerUpActivation : MonoBehaviour
{
    internal PowerUpTypes thisType;

    private PlayerStats playerStat;

    private void Start()
    {
        playerStat = PlayerStats.Instance;
    }

    private void OnTriggerEnter2D(Collider2D triggerInfo)
    {
        if (!triggerInfo.CompareTag("Player"))
            return;

        switch (thisType)
        {
            default:
            case PowerUpTypes.Anything:
            case PowerUpTypes.Dash:
                if (playerStat.GetComponent<Dash>() == null)
                    playerStat.NewAbilityEquipped(playerStat.gameObject.AddComponent<Dash>());
                else
                {
                    Debug.Log("Ability already equipped");
                }
                
                break;
        }

        Destroy(gameObject);
    }
}
