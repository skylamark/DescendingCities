using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AntiViewBlocker : MonoBehaviour
{
    public List<GameObject> gameObjects;
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
        MeshRenderer _mr = gameObject.GetComponent<MeshRenderer>();
        _mr.enabled = false;
        foreach (GameObject _obj in gameObjects)
        {
            _obj.layer = LayerMask.NameToLayer("MiniMap");
        }

        //Debug.Log("Updating...");
        /*MeshRenderer thisObj = gameObject.GetComponent<MeshRenderer>();
        if (thisObj != null)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
        foreach (MeshRenderer _mr in renderer)
        {
            _mr.enabled = false;

        }*/
        //Debug.Log("Done");
    }

    public void Enable()
    {
        MeshRenderer _mr = gameObject.GetComponent<MeshRenderer>();
        _mr.enabled = true;
        foreach (GameObject _obj in gameObjects)
        {
            _obj.layer = LayerMask.NameToLayer("Default");
        }

        //Debug.Log("Updating...");
        /*MeshRenderer thisObj = gameObject.GetComponent<MeshRenderer>();
        if (thisObj != null)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
        foreach (MeshRenderer _mr in renderer)
        {
            _mr.enabled = true;
        }*/
        //Debug.Log("Done");
    }
}
