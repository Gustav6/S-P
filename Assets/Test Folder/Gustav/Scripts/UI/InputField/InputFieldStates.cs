using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldDeselectedState : UIBaseState
{
    private InputFieldStateManager manager;
    private readonly float timeItTakes = 0.25f;

    public override void EnterState(BaseStateManager referenceManager)
    {
        manager = (InputFieldStateManager)referenceManager;

        manager.DefaultDeselectTransition(timeItTakes, manager.Pointers, manager.transform, manager.outlineImage, manager.text);
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
public class InputFieldSelectedState : UIBaseState
{
    private InputFieldStateManager manager;

    private readonly Color textColor = new(1, 1, 1, 1);
    private readonly float timeItTakes = 0.25f;

    public override void EnterState(BaseStateManager referenceManager)
    {
        if (!UIStateManager.Instance.Transitioning)
        {
            manager = (InputFieldStateManager)referenceManager;

            referenceManager.StartCoroutine(WaitCoroutine(timeItTakes));
            manager.DefaultSelectTransition(timeItTakes, manager.Pointers, manager.transform, manager.outlineImage, null);

            TransitionSystem.AddColorTransition(new ColorTransition(manager.text, textColor, timeItTakes, TransitionType.SmoothStop2));
        }
        else
        {
            manager.SwitchState(manager.deselectedState);
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

public class InputFieldPressedState : UIBaseState
{
    private InputFieldStateManager manager;
    private InputField inputFieldInstance;

    private readonly Color textColor = new(1, 1, 0, 0.8f);
    private readonly float timeItTakes = 0.25f;

    public override void EnterState(BaseStateManager referenceManager)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySound("Click");
        }

        manager = (InputFieldStateManager)referenceManager;
        inputFieldInstance = (InputField)referenceManager.UIInstance;
        TransitionSystem.AddColorTransition(new ColorTransition(manager.text, textColor, timeItTakes, TransitionType.SmoothStop2));

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

                    for (int i = 0; i < UIDataManager.instance.scoreLeaderBoard.Count; i++)
                    {
                        bool canAdd = true;

                        // Replace player with less score
                        //for (int y = 0; y < UIDataManager.instance.scoreLeaderBoard.Count; y++)
                        //{

                        //}

                        // temp check
                        if (canAdd && UIDataManager.instance.scoreLeaderBoard.ElementAt(i).Key == null)
                        {
                            //UIDataManager.instance.scoreLeaderBoard.Add(manager.text.text, 0);
                            UIDataManager.instance.CurrentData.scoreLeadBoardNames[i] = manager.text.text;
                            UIDataManager.instance.CurrentData.scoreLeaderBoardValues[i] = 1;
                            break;
                        }
                    }

                    for (int i = 0; i < UIDataManager.instance.waveLeaderBoard.Count; i++)
                    {
                        bool canAdd = true;

                        // Replace player with less score
                        //for (int y = 0; y < UIDataManager.instance.scoreLeaderBoard.Count; y++)
                        //{

                        //}

                        // temp check
                        if (canAdd && UIDataManager.instance.waveLeaderBoard.ElementAt(i).Key == null)
                        {
                            UIDataManager.instance.CurrentData.waveLeadBoardNames[i] = manager.text.text;
                            UIDataManager.instance.CurrentData.waveLeaderBoardValues[i] = 1;
                            break;
                        }
                    }

                    SaveSystem.Instance.SaveData(UIDataManager.instance.CurrentData);
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
    }

    public override void ExitState(BaseStateManager referenceManager)
    {
        if (manager.text.text.Length == 0)
        {
            manager.text.text = "WRITE NAME";
        }
    }
}
