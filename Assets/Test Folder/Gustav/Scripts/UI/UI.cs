using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public UIManager UIManagerInstance { get; private set; }
    public AudioManager AudioManagerInstance { get; private set; }

    public bool activated = false;
    public Vector2 position;

    void Start()
    {
        AudioManagerInstance = GetComponent<AudioManager>();
        UIManagerInstance = GetComponentInParent<UIManager>();
    }
}
