using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

#region Deselected State
public class InputFieldDeselectedState : UIBaseState
{
    private InputFieldStateManager manager;
    private InputField inputFieldInstance;

    private readonly float timeItTakes = 0.25f;

    public override void EnterState(BaseStateManager referenceManager)
    {
        manager = (InputFieldStateManager)referenceManager;
        inputFieldInstance = (InputField)referenceManager.UIInstance;

        manager.DefaultDeselectTransition(timeItTakes, inputFieldInstance.enableOnSelect, manager.bgImage, manager.transform, manager.text);
    }
    public override void UpdateState(BaseStateManager referenceManager)
    {
        CheckIfSelected(referenceManager, manager.selectedState);
    }

    public override void ExitState(BaseStateManager referenceManager)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySound("Hover");
        }
    }
}
#endregion

#region Selected State
public class InputFieldSelectedState : UIBaseState
{
    private InputFieldStateManager manager;
    private InputField inputFieldInstance;

    private readonly float timeItTakes = 0.2f;

    public override void EnterState(BaseStateManager referenceManager)
    {
        manager = (InputFieldStateManager)referenceManager;
        inputFieldInstance = (InputField)referenceManager.UIInstance;

        if (!UIStateManager.Instance.Transitioning)
        {
            manager.text.color = Color.white;
            referenceManager.StartCoroutine(WaitCoroutine(timeItTakes));
            manager.DefaultSelectTransition(timeItTakes, inputFieldInstance.enableOnSelect, manager.bgImage, manager.transform);
        }
    }

    public override void UpdateState(BaseStateManager referenceManager)
    {
        CheckIfDeselected(referenceManager, manager.deselectedState);

        if (canTransition && HasPressed(manager))
        {
            manager.SwitchState(manager.pressedState);
        }
    }

    public override void ExitState(BaseStateManager referenceManager)
    {

    }
}
#endregion

#region Pressed State
public class InputFieldPressedState : UIBaseState
{
    private InputFieldStateManager manager;
    private InputField inputFieldInstance;

    public override void EnterState(BaseStateManager referenceManager)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySound("Click");
        }

        manager = (InputFieldStateManager)referenceManager;
        inputFieldInstance = (InputField)referenceManager.UIInstance;
        manager.text.color = new Color (1, 1, 0, 0.8f);

        manager.text.text = "";
    }

    public override void UpdateState(BaseStateManager referenceManager)
    {
        foreach (char c in Input.inputString)
        {
            if (c == '\b') // has backspace/delete been pressed?
            {
                if (manager.text.text.Length != 0)
                {
                    manager.text.text = manager.text.text[..^1];
                }
            }
            else if ((c == '\n') || (c == '\r')) // enter/return
            {
                if (manager.text.text != "")
                {
                    Debug.Log("User entered: " + manager.text.text);

                    int rnd1 = Random.Range(1, 10000);
                    int rnd2 = Random.Range(1, 10);

                    AddNameToLeaderBoards(manager.text.text, rnd1, rnd2);
                    SaveLeaderBoard();

                    inputFieldInstance.UpdateLeadboard();
                }

                CheckIfDeselected(referenceManager, manager.deselectedState);
                CheckIfSelected(referenceManager, manager.selectedState);
                break;
            }
            else if (manager.text.text.Length < inputFieldInstance.maxAmountOfLetters)
            {
                manager.text.text += c;
            }
        }

        if (!UIStateManager.Instance.KeyOrControlActive)
        {
            CheckIfDeselected(referenceManager, manager.deselectedState);
            CheckIfSelected(referenceManager, manager.selectedState);
            manager.UIActivated = false;
        }
    }

    public override void ExitState(BaseStateManager referenceManager)
    {
        if (manager.text.text.Length == 0)
        {
            manager.text.text = "WRITE NAME";
        }
        else
        {
            inputFieldInstance.ActivateSelectedFunctions();
        }
    }

    public void SaveLeaderBoard()
    {
        for (int i = 0; i < UIDataManager.instance.scoreNames.Length; i++)
        {
            UIDataManager.instance.CurrentData.scoreLeadBoardNames[i] = UIDataManager.instance.scoreNames[i];
            UIDataManager.instance.CurrentData.scoreLeaderBoardValues[i] = UIDataManager.instance.score[i];
        }

        for (int i = 0; i < UIDataManager.instance.waveNames.Length; i++)
        {
            UIDataManager.instance.CurrentData.waveLeadBoardNames[i] = UIDataManager.instance.waveNames[i];
            UIDataManager.instance.CurrentData.waveLeaderBoardValues[i] = UIDataManager.instance.wave[i];
        }

        SaveSystem.Instance.SaveData(UIDataManager.instance.CurrentData);
    }

    public void AddNameToLeaderBoards(string name, int score, int wave)
    {
        string currentName;

        currentName = name;
        int currentScore = score;

        for (int i = 0; i < UIDataManager.instance.scoreNames.Length; i++)
        {
            int scoreOnCurrentIndex = UIDataManager.instance.score[i];

            if (scoreOnCurrentIndex < currentScore)
            {
                string temp = UIDataManager.instance.scoreNames[i];

                UIDataManager.instance.scoreNames[i] = currentName;
                UIDataManager.instance.score[i] = currentScore;

                currentName = temp;
                currentScore = scoreOnCurrentIndex;
            }
            else if (scoreOnCurrentIndex == 0)
            {
                UIDataManager.instance.scoreNames[i] = currentName;
                UIDataManager.instance.score[i] = currentScore;

                break;
            }
        }

        currentName = name;
        int currentWave = wave;

        for (int i = 0; i < UIDataManager.instance.waveNames.Length; i++)
        {
            int waveOnCurrentIndex = UIDataManager.instance.wave[i];

            if (waveOnCurrentIndex < currentWave)
            {
                string temp = UIDataManager.instance.waveNames[i];

                UIDataManager.instance.waveNames[i] = currentName;
                UIDataManager.instance.wave[i] = currentWave;

                currentName = temp;
                currentWave = waveOnCurrentIndex;
            }
            else if (waveOnCurrentIndex == 0)
            {
                UIDataManager.instance.waveNames[i] = currentName;
                UIDataManager.instance.wave[i] = currentWave;

                break;
            }
        }
    }
}
#endregion
