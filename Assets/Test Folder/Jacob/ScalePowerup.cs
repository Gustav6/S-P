using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalePowerup : PowerUp
{
    bool _isUsingPowerup = false;

    public override void UsePowerUp()
    {
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
            transform.localScale = Vector2.Lerp(Vector2.one, Vector2.one * 1.5f, time / 0.25f);
        }

        yield return new WaitForSeconds(9.5f);

        time = 0;

        while (time < 0.25f)
        {
            yield return null;
            time += Time.deltaTime;
            transform.localScale = Vector2.Lerp(Vector2.one * 1.25f, Vector2.one, time / 0.25f);
        }

        OnDeactivatePowerUp();
    }

    public override void OnDeactivatePowerUp()
    {
        transform.localScale = Vector2.one;

        PlayerStats.Instance.DeActivateAbilityStats();
        PlayerStats.Instance.ClearEquippedAbility();
    }
}
