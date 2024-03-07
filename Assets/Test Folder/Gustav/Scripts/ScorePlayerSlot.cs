using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ScorePlayerSlot : MonoBehaviour
{
    private TextMeshProUGUI nameTag;
    private TextMeshProUGUI value;
    [SerializeField] private int index;

    void Start()
    {
        UI.OnSave += InputField_OnSave;

        nameTag = transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        value = transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();

        SetPlayerData();
    }

    private void InputField_OnSave(object sender, EventArgs e)
    {
        SetPlayerData();
    }

    public void SetPlayerData()
    {
        if (UIDataManager.instance.score[index] > 0)
        {
            string name = UIDataManager.instance.scoreNames[index];
            int score = UIDataManager.instance.score[index];

            nameTag.text = name;
            value.text = score.ToString();
        }
        else
        {
            nameTag.text = "";
            value.text = "";
        }
    }
}
