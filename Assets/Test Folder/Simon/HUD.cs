using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class HUD : MonoBehaviour
{
    private TMP_Text _percentText;

    private void Awake()
    {
        _percentText = GetComponentInChildren<TMP_Text>();
    }

    private void Start()
    {
        if (EquipmentManager.Instance.PlayerTookDamage == null)
            EquipmentManager.Instance.PlayerTookDamage = UpdatePlayerPercentDisplay;
    }

    private void UpdatePlayerPercentDisplay()
    {
        _percentText.text = ((int)PlayerStats.Instance.KnockbackPercent).ToString() + "%";
    }
}
