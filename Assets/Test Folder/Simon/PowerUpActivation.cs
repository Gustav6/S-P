using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PowerUpActivation : MonoBehaviour
{
    internal PowerUpTypes thisType;

    private PlayerStats playerStat;

    private float _time, _multipler = 1;

    private void Start()
    {
        playerStat = PlayerStats.Instance;
    }

    private void Update()
    {
        if (Mathf.Abs(_time) <= 1 && _time >= 0) 
            _time += Time.deltaTime * _multipler;
        else
        {
            _multipler = -_multipler;
            _time += Time.deltaTime * _multipler;
        }

        transform.position = new Vector2(transform.position.x, TransitionSystem.SmoothStop3(_time));
    }


    // TODO: Deactivate other powerups.
    private void OnTriggerEnter2D(Collider2D triggerInfo)
    {
        if (!triggerInfo.CompareTag("Player"))
            return;

        PowerUp previousPowerUp = playerStat.GetComponent<PowerUp>();

        if (previousPowerUp != null)
            playerStat.ClearEquippedAbility();

        switch (thisType)
        {
            default:
            case PowerUpTypes.Anything:
            case PowerUpTypes.Dash:
                try
                {
                    Destroy(previousPowerUp);
                    playerStat.NewAbilityEquipped(playerStat.gameObject.AddComponent<Dash>());
                }
                catch
                {
                    playerStat.NewAbilityEquipped(playerStat.gameObject.AddComponent<Dash>());
                }
                
                break;
            case PowerUpTypes.Haste:
                try
                {
                    Destroy(previousPowerUp);
                    playerStat.NewAbilityEquipped(playerStat.gameObject.AddComponent<HastePowerup>());
                }
                catch
                {
                    playerStat.NewAbilityEquipped(playerStat.gameObject.AddComponent<HastePowerup>());
                }

                break;
            case PowerUpTypes.Tank:
                try
                {
                    Destroy(previousPowerUp);
                    playerStat.NewAbilityEquipped(playerStat.gameObject.AddComponent<ScalePowerup>());
                }
                catch
                {
                    playerStat.NewAbilityEquipped(playerStat.gameObject.AddComponent<ScalePowerup>());
                }

                break;
        }

        Destroy(gameObject);
    }
}
