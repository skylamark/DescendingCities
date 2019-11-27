using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{

    public GameObject player;
    public float yOffset;
    private bool hasPlayer = false;


    // Start is called before the first frame update
    void Start()
    {
        GameObject _temp;
        if (player == null)
        {
            _temp = GameObject.Find("AVC_Player");

            if (_temp != null)
            {
                player = _temp;
                hasPlayer = true;
            }
            else { Debug.Log("Player obj not found."); }

        }
    }

    // Update is called once per frame
    void Update()
    {
        GameObject _temp;
        if (player == null)
        {
            _temp = GameObject.Find("AVC_Player");

            if (_temp != null)
            {
                player = _temp;
                hasPlayer = true;
            }
            else { Debug.Log("Player obj not found."); }

        }
    }

    private void LateUpdate()
    {
        if (hasPlayer)
        {
            transform.position = new Vector3(player.transform.position.x, yOffset, player.transform.position.z);
        }
    }
}
