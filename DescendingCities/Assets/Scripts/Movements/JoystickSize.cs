using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoystickSize : MonoBehaviour
{
     private int sizeBackground;
     private int sizePointer;
     private Image backgroundImage;
     private Image joystickImage;

    void Start()
    {
        
        backgroundImage = GetComponent<Image>();
        joystickImage = transform.GetChild(0).GetComponent<Image>();


    }
    private void Update()
    {
        sizeBackground = Screen.width / 6;
        sizePointer = Screen.width / 15;
        backgroundImage.rectTransform.sizeDelta = new Vector2(sizeBackground, sizeBackground);
        joystickImage.rectTransform.sizeDelta = new Vector2(sizePointer, sizePointer);

        backgroundImage.rectTransform.position = new Vector2(sizeBackground / 2, sizeBackground / 2);
    }


}
