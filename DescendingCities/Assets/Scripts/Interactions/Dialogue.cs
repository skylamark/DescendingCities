using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    private Canvas canvas;
    public Text text;
    public string toShow;

    private void Start()
    {
        canvas = GetComponent<Canvas>();
        toShow = PlayerManagement.player.CurrProblem;


    }
    void Update()
    {
        if (PlayerManagement.player.CurrProblem!=null)
        {
            canvas.enabled = true;
            text.text = toShow;
          
        }
        else
        {

            canvas.enabled = false;
        }
    }

    public void Solve()
    {
        toShow
            = PlayerManagement.player.findSolution();
    }
}
