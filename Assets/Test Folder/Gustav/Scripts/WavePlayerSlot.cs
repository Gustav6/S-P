using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class WavePlayerSlot : MonoBehaviour
{
    private TextMeshProUGUI nameTag;
    private TextMeshProUGUI value;
    [SerializeField] private int index;

    void Start()
    {
        InputField.OnSave += InputField_OnSave;

        nameTag = transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        value = transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();

        SetPlayerData();
    }

    private void InputField_OnSave(object sender, System.EventArgs e)
    {
        SetPlayerData();
    }

    public void SetPlayerData()
    {
        if (UIDataManager.instance.wave[index] > 0)
        {
            string name = UIDataManager.instance.waveNames[index];
            int wave = UIDataManager.instance.wave[index];

            nameTag.text = name;
            value.text = wave.ToString();
        }
        else
        {
            nameTag.text = "";
            value.text = "";
        }
    }
}
