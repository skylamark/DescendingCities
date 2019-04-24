using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButton : MonoBehaviour
{
    public void ExitHome()
    {
        Debug.Log("Clicked");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);

    }
}
