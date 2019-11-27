using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public GameObject player;
    public float offset;
    private bool hasPlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        GameObject _tempObj;
        if (player == null)
        {
            _tempObj = GameObject.Find("AVC_Player");
            if (_tempObj != null)
            {
                player = _tempObj;
            }
            else { Debug.Log("No player object found in scene."); }

        }
        

    }

    // Update is called once per frame
    void Update()
    {
        GameObject _tempObj;
        if (player == null)
        {
            _tempObj = GameObject.Find("AVC_Player");
            if (_tempObj != null)
            {
                player = _tempObj;
            }
            else { Debug.Log("No player object found in scene."); }

        }
    }

    private void LateUpdate()
    {
        if (player != null)
        {
            transform.position = new Vector3(player.transform.position.x, offset, player.transform.position.z);
        }
    }
}
