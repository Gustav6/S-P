using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveInfoPopup : MonoBehaviour
{
    TMP_Text[] _textElements;
    Animator _anim;

    private void Awake()
    {
        _textElements = GetComponentsInChildren<TMP_Text>(true);
        _anim = GetComponent<Animator>();
    }

    private void OnMouseEnter()
    {
        _anim.Play("WaveInfoExpand");
    }

    private void OnMouseExit()
    {
        _anim.Play("WaveInfoRetract");
    }

    public void Enable()
    {
        _anim.Play("WaveInfoAppear");
    }

    public void Disable()
    {
        _anim.Play("WaveInfoDisappear");
    }

    public void SetText(string waveType, string waveDescription)
    {
        _textElements[0].text = waveType;
        _textElements[1].text = waveDescription;
    }
}
