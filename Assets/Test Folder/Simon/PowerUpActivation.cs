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
