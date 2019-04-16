using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class JoystickController : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private Image backgroundImage;
    private Image joystickImage;
    private Vector3 inputVector;

    public void Start()
    {
        
        backgroundImage = GetComponent<Image>();
        joystickImage = transform.GetChild(0).GetComponent<Image>();
    }

    public virtual void OnDrag(PointerEventData pointerEventData)
    {
    
        Vector2 position;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(backgroundImage.rectTransform
                                                                   , pointerEventData.position
                                                                   , pointerEventData.pressEventCamera
                                                                   , out position))
        {
            
            position.x = (position.x / backgroundImage.rectTransform.sizeDelta.x);
            position.y = (position.y / backgroundImage.rectTransform.sizeDelta.y);
            
            inputVector = new Vector3(position.x*2, 0, position.y*2);


            if (inputVector.magnitude > 1.0f)
            {
                inputVector = inputVector.normalized;
            }
            
            MoveJoystickPointer();
        }
    }

    public virtual void OnPointerDown(PointerEventData pointerEventData)
    {
        OnDrag(pointerEventData);
      
    }
    public virtual void OnPointerUp(PointerEventData pointerEventData)
    {
          inputVector = Vector3.zero;
          joystickImage.rectTransform.anchoredPosition = Vector3.zero;
    }
 

    private void MoveJoystickPointer()
    {
        joystickImage.rectTransform.anchoredPosition =
            new Vector3(inputVector.x * (backgroundImage.rectTransform.sizeDelta.x / 3),
            inputVector.z * (backgroundImage.rectTransform.sizeDelta.y / 3));
    }

    public float Horizantal()
    {
       
            return inputVector.x;
      
    }
    public float Vertical()
    {
        if (inputVector.z != 0)
            return inputVector.z;
        else
            return Input.GetAxis("Vertical");
    }
}
