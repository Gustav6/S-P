using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
    internal Sprite powerUpSprite;

    public abstract void UsePowerUp();

    public abstract void OnDeactivatePowerUp();
}
