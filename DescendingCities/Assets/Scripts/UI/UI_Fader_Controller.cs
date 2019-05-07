using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Fader_Controller : MonoBehaviour
{
    private Animator animator;
    private Image faderImage;

    // Start is called before the first frame update
    void Start()    {
        animator = GetComponent<Animator>();
        faderImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()    {
        
    }

    public void FadeIn()
    {
        faderImage.material.color = Color.white;
        animator.SetBool("FadeActive", true);
    }

    public void FadeOut(float _delay)
    {
        Invoke("Fading", _delay);
        
    }

    void Fading()
    {
        animator.SetBool("FadeActive", false);
    }
}
