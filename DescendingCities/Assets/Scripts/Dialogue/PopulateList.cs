using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateList : MonoBehaviour
{
    Text textField;
    void Start()
    {
        textField = GetComponentInChildren<Text>();
        Populate();
    }

    void Update()
    {
        Populate();
    }

    void Populate()
    {
        string s = "VVE";
        List<Owner> ownerList = PlayerManagement.currLevel.Vve;
        for (int i = 0; i < ownerList.Count; i++)
        {
            s = s + "\n" + ownerList[i].Name;
        }

        textField.text = s;

    }
}