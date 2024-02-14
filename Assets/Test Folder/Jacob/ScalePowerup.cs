using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalePowerup : PowerUp
{
    public override void UsePowerUp()
    {
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

        PlayerStats.Instance.DeActivateAbilityStats();
        PlayerStats.Instance.ClearEquippedAbility();
    }
}
