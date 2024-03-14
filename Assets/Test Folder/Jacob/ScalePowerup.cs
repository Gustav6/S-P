using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalePowerup : PowerUp
{
    bool _isUsingPowerup = false;

    private void Start()
    {
        duration = 10f;

        powerUpSprite = EquipmentManager.Instance.ReturnPowerupSprite(PowerUpTypes.Tank);

        EquipmentManager.Instance.OnPowerUpEquipped?.Invoke();
    }

    public override void UsePowerUp()
    {
        EquipmentManager.Instance.OnPowerUpUsed?.Invoke();

        if (_isUsingPowerup)
            return;

        _isUsingPowerup = true;
        StartCoroutine(Scale());
    }

    IEnumerator Scale()
    {
        PlayerStats.Instance.ActivateAbilityStats(new StatBlock(1, 1.25f, 1.5f, 1, 1.5f, 1));

        float time = 0;

        while (time < 0.25f)
        {
            yield return null;
            time += Time.deltaTime;
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.5f, time / 0.25f);
        }

        yield return new WaitForSeconds(duration - 0.5f);

        time = 0;

        while (time < 0.25f)
        {
            yield return null;
            time += Time.deltaTime;
            transform.localScale = Vector3.Lerp(Vector3.one * 1.5f, Vector3.one, time / 0.25f);
        }

        OnDeactivatePowerUp();
    }

    public override void OnDeactivatePowerUp()
    {
        transform.localScale = Vector3.one;

        PlayerStats.Instance.DeActivateAbilityStats();
        PlayerStats.Instance.ClearEquippedAbility();
    }
}
