using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Simon
public abstract class PowerUp : MonoBehaviour
{
    internal Sprite powerUpSprite;

    // Set duration in start method of each powerup
    internal float duration = 0;

    public abstract void UsePowerUp();

    public abstract void OnDeactivatePowerUp();
}
