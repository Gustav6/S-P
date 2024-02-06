using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] popUps;
    private int popUpIndex;
    bool skipTutorial = false;

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
        if (Input.GetKeyDown(KeyCode.F)) {
            popUpIndex++;
        }
    }
}
