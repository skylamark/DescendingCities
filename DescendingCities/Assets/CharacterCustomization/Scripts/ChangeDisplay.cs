using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class ChangeDisplay : MonoBehaviour {

	
	public Texture[] characterTextures;
	public GameObject[] classes;
	public Texture[] defaultClassesTextures;
	public Texture[] alternativeClassesTextures;
	private int currentCharTexture = 0;
	private Renderer rend;
	private int currentClass = 0;
	private int currentClassTexture = 0;
	
	private Vector3 defaultPos;

	void Start () {
		rend = GetComponent<Renderer>();
		defaultPos = transform.parent.position;
		changeCharacterTexture ();
		changeClass ();
		changeClassTexture ();

	}




	void Update () {
		if (Input.GetKeyDown ("end")) {
			changeClass();
		}
	}

	

	public void changeClassTexture() {
		Renderer childRnd;
		for (int i = 0; i < classes.Length; i++) {

			for (int j = 0; j < classes [i].transform.childCount; j++) {
				childRnd = classes [i].transform.GetChild (j).gameObject.GetComponent<Renderer> ();
				if (childRnd != null) {
					if (currentClassTexture == 0) {
						childRnd.material.mainTexture = defaultClassesTextures[i];
					} else {
						childRnd.material.mainTexture = alternativeClassesTextures[i];
					}

				}
			}

		}
		if (currentClassTexture == 0) {
			currentClassTexture = 1;
		} else {
			currentClassTexture = 0;
		}
	}

	public void changeClass() {
		for (int i = 0; i < classes.Length; i++) {
			classes [i].SetActive (i == currentClass);
		}
		currentClass++;
		if(currentClass >= classes.Length) {
			currentClass = 0;
		}	
	}

	public void changeCharacterTexture() {
		rend.material.mainTexture = characterTextures[currentCharTexture++];
		if(currentCharTexture >= characterTextures.Length)
		{
			currentCharTexture = 0;
		}
	}

    public void playGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
