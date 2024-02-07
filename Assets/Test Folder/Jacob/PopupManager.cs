using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupManager : MonoBehaviour
{
    #region Singleton

    public static PopupManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Duplicate instance of PopupManager found on " + gameObject + ", destroying instance");
            Destroy(this);
        }
    }

    #endregion

    [SerializeField] TMP_Text _textPopup;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
            SpawnText("Item aquired!\nBaseball Bat", new Vector2(0, 0), 2);
    }

    public void SpawnText(string text, Vector2 position, float fadeTime)
    {
        TMP_Text textComponent = Instantiate(_textPopup, position, Quaternion.identity, transform);
        textComponent.text = text;

        StartCoroutine(FadeOutText(fadeTime, textComponent));
    }

    IEnumerator FadeOutText(float fadeTime, TMP_Text textComponent)
    {
        float time = fadeTime;

        Color color = textComponent.color;
        Transform textTransform = textComponent.transform;

        Vector2 startPos = textTransform.position;

        while (time >= 0)
        {
            time -= Time.deltaTime;
            yield return null;

            textTransform.position = Vector2.Lerp(startPos + new Vector2(0, fadeTime / 2), startPos, time / fadeTime);

            color.a = time / fadeTime;
            textComponent.color = color;
        }

        Destroy(textTransform.gameObject);
    }
}