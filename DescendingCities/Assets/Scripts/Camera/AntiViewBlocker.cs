using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiViewBlocker : MonoBehaviour
{
    public new List<MeshRenderer> renderer = new List<MeshRenderer>();
    public MeshRenderer[] _redPriv;
    public bool disableRenderer;
    public bool update;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Disable()
    {
        //Debug.Log("Updating...");
        MeshRenderer thisObj = gameObject.GetComponent<MeshRenderer>();
        if (thisObj != null)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
        foreach (MeshRenderer _mr in renderer)
        {
            _mr.enabled = false;
        }
        //Debug.Log("Done");
    }

    public void Enable()
    {
        //Debug.Log("Updating...");
        MeshRenderer thisObj = gameObject.GetComponent<MeshRenderer>();
        if (thisObj != null)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
        foreach (MeshRenderer _mr in renderer)
        {
            _mr.enabled = true;
        }
        //Debug.Log("Done");
    }
}
