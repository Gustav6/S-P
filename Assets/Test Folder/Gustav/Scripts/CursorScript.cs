using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorScript : MonoBehaviour
{
    private Image image;
    public Vector3 hotSpot;

    public Sprite pressedMenuSprite;
    public Sprite regulerMenuSprite;

    public Sprite gameSprite;

    private void Start()
    {
        image = GetComponent<Image>();
        Cursor.visible = false;
    }

    private void Update()
    {
        if (UIStateManager.Instance.ActivePrefab != null)
        {
            if (image.sprite != pressedMenuSprite && image.sprite != regulerMenuSprite)
            {
                image.color = Color.white;
                transform.localScale = new Vector3(1, 1, 1);
                image.sprite = regulerMenuSprite;
            }

            if (Input.GetMouseButtonDown(0))
            {
                image.sprite = pressedMenuSprite;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                image.sprite = regulerMenuSprite;
            }

            transform.position = Input.mousePosition + hotSpot;
        }
        else
        {
            if (image.sprite != gameSprite)
            {
                image.color = new Color(1, 1, 1, 0.5f);
                transform.localScale = new Vector3(0.4f, 0.4f, 1);
                image.sprite = gameSprite;
            }

            transform.position = Input.mousePosition;
        }
    }
}
