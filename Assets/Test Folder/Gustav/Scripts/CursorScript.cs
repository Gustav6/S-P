using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorScript : MonoBehaviour
{
    private Image image;
    public Vector3 hotSpot;

    public Sprite pressedMenuSprite;
    public Sprite regularMenuSprite;

    public Sprite gameSprite;
    public Color gameCrosshairColor;

    private void Start()
    {
        image = GetComponent<Image>();
        Cursor.visible = false;
    }

    private void Update()
    {
        if (UIStateManager.Instance.ActivePrefab != null)
        {
            if (image.sprite != pressedMenuSprite && image.sprite != regularMenuSprite)
            {
                image.color = Color.white;
                transform.localScale = new Vector3(1, 1, 1);
                image.sprite = regularMenuSprite;
            }

            if (Input.GetMouseButtonDown(0))
            {
                image.sprite = pressedMenuSprite;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                image.sprite = regularMenuSprite;
            }

            transform.position = Input.mousePosition + hotSpot;
        }
        else
        {
            if (image.sprite != gameSprite)
            {
                image.color = gameCrosshairColor;
                transform.localScale = new Vector3(0.35f, 0.35f, 1);
                image.sprite = gameSprite;
            }

            transform.position = Input.mousePosition;
        }
    }
}
