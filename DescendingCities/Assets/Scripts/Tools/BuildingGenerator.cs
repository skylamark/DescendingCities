using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGenerator : MonoBehaviour
{
    public GameObject[] cubeHouses;
    public GameObject[] builtHouses;
    //private GameObject[] buildingInProgress;

    [Header("Fixed House Dimensions")]
    public float floorHeight;
    public float roofHeight;
    public float windowHeight;
    public float DoorHeight;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CollectAllCubes()
    {
        cubeHouses = GameObject.FindGameObjectsWithTag("CubeHouse");
    }

    public void ConvertToBuilding()
    {
        //foreach GameObject _house in cubeHouses

    }

    public void ShowHouses()
    {
    
    }

    public void ShowCubes()
    {
    
    }


}
