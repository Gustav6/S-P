using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    #region Singleton

    public static ParticleManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("More than one instance of ParticleManager found on " + gameObject + ", destroying instance");
            Destroy(this);
        }
    }

    #endregion

    public void SpawnParticle(ParticleSystem system, Vector3 position)
    {
        GameObject obj = Instantiate(system, position + transform.position, Quaternion.identity, transform).gameObject;

        Destroy(obj, system.main.duration);
    }
}
