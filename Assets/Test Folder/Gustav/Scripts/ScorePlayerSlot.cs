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
        nameTag = transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        value = transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();

        SetPlayerData();
    }

    public void SetPlayerData()
    {
        if (UIDataManager.instance.waveLeaderBoard[(UIDataManager.instance.waveLeaderBoard.ElementAt(index)).Key] > 0)
        {
            nameTag.text = UIDataManager.instance.scoreLeaderBoard.ElementAt(index).Key;
            value.text = UIDataManager.instance.scoreLeaderBoard.ElementAt(index).Value.ToString();
        }
        else
        {
            nameTag.text = "";
            value.text = "";
        }
    }
}
