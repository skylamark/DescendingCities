using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseMiniMap : MonoBehaviour

{

    public RectTransform plusImage;
    public RectTransform minimap;

   public void OpenMiniMap()
    {

        minimap.gameObject.active = true;
        plusImage.gameObject.active = false;
    }

    public void CloseMiniMap()
    {
        minimap.gameObject.active = false;
        plusImage.gameObject.active = true;
    }
}
