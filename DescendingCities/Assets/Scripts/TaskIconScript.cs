using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskIconScript : MonoBehaviour
{

    public Image activeImage;
    public GameObject taskObj;
    private Task task;
    public int taskState;

    public Sprite[] taskSprites;
    // Start is called before the first frame update
    void Start()
    {
        task = taskObj.GetComponent<Task>();
    }

    // Update is called once per frame
    void Update()
    {
        if (taskState != task.status)
        {

            taskState = task.status;
            activeImage.sprite = taskSprites[taskState];
        }
    }
}
