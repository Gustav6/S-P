using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorScript : MonoBehaviour
{
    private Image image;
    public Vector3 hotSpot;

    public Sprite pressedSprite;
    public Sprite regulerSprite;

    private void Start()
    {
        image = GetComponent<Image>();
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            image.sprite = pressedSprite;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            image.sprite = regulerSprite;
        }

        transform.position = Input.mousePosition + hotSpot;
    }
}
