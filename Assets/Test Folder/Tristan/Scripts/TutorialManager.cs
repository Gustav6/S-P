using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] popUps;
    private int popUpIndex;
    bool skipTutorial;
    float timer;
    float timerMax = 2;

    Data ThisData;

    private void Start()
    {
        ThisData = SaveSystem.Instance.LoadData();
        //skipTutorial = ThisData.SkipTutorial;
    }

    void Update()
    {
        for (int i = 0; i < popUps.Length; i++)
        {
            if (i == popUpIndex) {
                popUps[i].SetActive(true);
            }
            else {
                popUps[i].SetActive(false);
            }
        }
        // Update this later
        if (skipTutorial == false)
        {
            if (popUpIndex == 0)
            {
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
                    popUpIndex++;
            }
            else if (popUpIndex == 1)
            {
                // Check if player has picked up
                if (Input.GetButtonDown("Fire1"))
                    popUpIndex++;
            }
            else if (popUpIndex == 2)
            {
                if (Input.GetKeyDown(KeyCode.F))
                    popUpIndex++;
            }
            else if (popUpIndex == 3)
            {
                timer += Time.deltaTime;
                if (timer >= timerMax)
                {
                    popUpIndex++;
                    //ThisData.SkipTutorial = true;
                    SaveSystem.Instance.SaveData(ThisData);
                    //Wave Starts
                }
            }
        }
        else
        {

        }
    }
}
