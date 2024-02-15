using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UIElements;

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

    [SerializeField] ParticleSystem _waterSplashSystem;

    public void SpawnParticle(ParticleSystem system, Vector3 position)
    {
        GameObject obj = Instantiate(system, position, Quaternion.identity, transform).gameObject;

        Destroy(obj, system.main.duration);
}
    public void SpawnWaterSplash(Vector3 position)
    {
        GameObject obj = Instantiate(_waterSplashSystem, position, Quaternion.identity, transform).gameObject;

        Destroy(obj, _waterSplashSystem.main.duration);
    }
}
