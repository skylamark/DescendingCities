using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseMiniMap : MonoBehaviour

{

    public RectTransform plusImage;
    public RectTransform minimap;

   public void OpenMiniMap()
    {

        minimap.gameObject.SetActive(true);
        plusImage.gameObject.SetActive(false);
    }

    public void CloseMiniMap()
    {
        minimap.gameObject.SetActive(false);
        plusImage.gameObject.SetActive(true);
    }
}
