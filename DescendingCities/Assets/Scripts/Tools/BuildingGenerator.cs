using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Random = UnityEngine.Random;

public class BuildingGenerator : MonoBehaviour
{
    public GameObject[] cubeHouses;
    public GameObject[] storedCubes;
    public GameObject[] storedHomes;

    [Header("Fixed House Dimensions")]
    public float floorHeight;
    public float windowHeight;
    public float DoorHeight;
    [Header("Building Pieces")]
    public GameObject foundation;
    public GameObject floor;
    public GameObject door;
    public GameObject hatch;
    public GameObject[] windows;
    public GameObject[] roofs;
    public GameObject[] decorations;
    public Material[] materialsHomes;
    public Material[] materialCubeHome;


    // Temporary internal variables
    private GameObject houseParentObj;
    private Transform houseTransform;
    private AntiViewBlocker avb;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /* Generator button functions */
    public void CollectAllCubes()
    {
        cubeHouses = GameObject.FindGameObjectsWithTag("CubeHouse");
        for (int i = 0; i < cubeHouses.Length; i++)
        {
            cubeHouses[i].GetComponent<MeshRenderer>().material = materialCubeHome[1];
            string temp = "CubeHome" + i.ToString();
            cubeHouses[i].name = temp;
        }
        if (cubeHouses.Length == 0) { Debug.LogWarning("No Cubes in scene, make sure they have the tag 'CubeHouse'"); }
    }

    public void ClearSelectedCubes()
    {
        if (cubeHouses.Length == 0) { Debug.LogWarning("No Cubes Selected, 'Press Collect CubesHouses'"); }
        for (int i = 0; i < cubeHouses.Length; i++)
        {
            cubeHouses[i].GetComponent<MeshRenderer>().material = materialCubeHome[0];
            cubeHouses[i] = null;
            if (i == (cubeHouses.Length-1))
            {
                System.Array.Resize(ref cubeHouses, 0);
                Debug.Log("Array cleared");
            }
        }
    }

    public void DeleteGeneratedHouses()
    {
        for (int i = 0; i < storedHomes.Length; i++)
        {
            DestroyImmediate(storedHomes[i]);
            storedCubes[i].SetActive(true);
            if (i == (storedHomes.Length - 1))
            {
                System.Array.Resize(ref storedHomes, 0);
                System.Array.Resize(ref storedCubes, 0);
            }
        }
    }

    public void FinalizeConstruction()
    {
        for (int i = 0; i < storedCubes.Length; i++)
        {
            DestroyImmediate(storedCubes[i]);
            if (i == storedCubes.Length) { System.Array.Resize(ref storedCubes, 0); }
        }
    }


    public void ConvertToBuilding()
    {
        if (cubeHouses.Length == 0) { Debug.LogWarning("No Cubes Selected, 'Press Collect CubesHouses'"); }
        else {
            for (int i = 0; i < cubeHouses.Length; i++)
            {
                /* retreive values and set references*/
                houseParentObj = cubeHouses[i].transform.parent.gameObject;
                houseTransform = cubeHouses[i].transform;
                float temp = cubeHouses[i].transform.localScale.y;
                int homeHeight = Mathf.RoundToInt(temp);

                /* Random Generation*/
                int materialInt = Random.Range(0, materialsHomes.Length);
                int roofInt = Random.Range(0, roofs.Length);
                int decorationInt = Random.Range(0, decorations.Length);
                int details = Random.Range(0,0);
                int decoInt = Random.Range(0, decorations.Length);

                //store cube
                StoreCubeHouse(cubeHouses[i].gameObject, i);

                /* Create Home Object at position and set parent*/
                GameObject home = new GameObject("Home") { tag = "Home" };
                home.transform.parent = houseParentObj.transform;
                home.transform.localPosition = new Vector3(0f, 0f, 0f);
                home.transform.rotation = houseTransform.rotation;
                StoreHome(home.gameObject, i);

                //Basement
                GameObject newFoundation = Instantiate(foundation, home.transform, false);
                newFoundation.transform.localPosition = new Vector3(0f, -1.95f, 0f);
                newFoundation.transform.localScale = new Vector3(1f, 1f, 1f);
                newFoundation.GetComponent<MeshRenderer>().material = materialsHomes[materialInt];
                newFoundation.name = "Foundation";

                //floor 0
                GameObject groundfloor = Instantiate(floor, newFoundation.transform, false);
                groundfloor.transform.localPosition = new Vector3(0f, 2f, 0f);
                groundfloor.transform.localScale = new Vector3(1f, 1f, 1f);
                groundfloor.GetComponent<MeshRenderer>().material = materialsHomes[materialInt];
                groundfloor.AddComponent<BoxCollider>();
                BoxCollider col = groundfloor.GetComponent<BoxCollider>();
                groundfloor.AddComponent<AntiViewBlocker>();
                avb = groundfloor.GetComponent<AntiViewBlocker>();
                col.size = new Vector3(6f, cubeHouses[i].transform.localScale.y, 6f);
                float calcColliderHeight = cubeHouses[i].transform.localScale.y / 2f;
                col.center = new Vector3(0f, calcColliderHeight, 0f);
                groundfloor.name = "Floor0";
                groundfloor.tag = "AVB";
                GenerateDetails(groundfloor, materialInt, true);

                // instantiate floors
                if (homeHeight >= 6)
                {
                    if (homeHeight > 6)
                    {
                        GameObject firstfloor = Instantiate(floor, groundfloor.transform, false);
                        firstfloor.transform.localPosition = new Vector3(0f, 3f, 0f);
                        firstfloor.transform.localScale = new Vector3(1f, 1f, 1f);
                        firstfloor.GetComponent<MeshRenderer>().material = materialsHomes[materialInt];
                        firstfloor.name = "Floor1";
                        GenerateDetails(firstfloor, materialInt, false);
                        AddDecorations(firstfloor, materialInt, decoInt);
                        AddToAvb(firstfloor);
                    }
                    else { AddRoof(groundfloor, materialInt, roofInt, floorHeight, decoInt); }
                }
                if (homeHeight >= 9)
                {
                    if (homeHeight > 9)
                    {
                        GameObject secondfloor = Instantiate(floor, groundfloor.transform, false);
                        secondfloor.transform.localPosition = new Vector3(0f, 6f, 0f);
                        secondfloor.transform.localScale = new Vector3(1f, 1f, 1f);
                        secondfloor.GetComponent<MeshRenderer>().material = materialsHomes[materialInt];
                        secondfloor.name = "Floor2";
                        GenerateDetails(secondfloor, materialInt, false);
                        AddDecorations(secondfloor, materialInt, decoInt);
                        AddToAvb(secondfloor);
                    }
                    else { AddRoof(groundfloor, materialInt, roofInt, (floorHeight * 2), decoInt); }

                }
                if (homeHeight >= 12)
                {
                    if (homeHeight > 12)
                    {
                        GameObject thirdfloor = Instantiate(floor, groundfloor.transform, false);
                        thirdfloor.transform.localPosition = new Vector3(0f, 9f, 0f);
                        thirdfloor.transform.localScale = new Vector3(1f, 1f, 1f);
                        thirdfloor.GetComponent<MeshRenderer>().material = materialsHomes[materialInt];
                        thirdfloor.name = "Floor3";
                        GenerateDetails(thirdfloor, materialInt, false);
                        AddDecorations(thirdfloor, materialInt, decoInt);
                        AddToAvb(thirdfloor);
                    }
                    else { AddRoof(groundfloor, materialInt, roofInt, (floorHeight * 3), decoInt); }
                }
                if (i == (cubeHouses.Length - 1)) { System.Array.Resize(ref cubeHouses, 0); }

            }
        }
    }

    void StoreCubeHouse(GameObject storedCube, int i)
    {
        if (storedCubes.Length != cubeHouses.Length) { System.Array.Resize(ref storedCubes, cubeHouses.Length); }
        storedCubes.SetValue(storedCube, i);
        storedCube.SetActive(false);
    }

    void StoreHome(GameObject storedHome, int i)
    {
        if (storedHomes.Length != cubeHouses.Length) { System.Array.Resize(ref storedHomes, cubeHouses.Length); }
        storedHomes.SetValue(storedHome, i);
    }

    void GenerateDetails(GameObject parentObj, int matInt, bool makedoor)
    {
        int windowInt = Random.Range(0, windows.Length);
        int doorPos=0;
        if (makedoor)
        {
            doorPos = Random.Range(0, 3);
        }


        for (int i = 0; i < 3; i++)
        {
            Vector3 vector = new Vector3(0f,0f,0f);

            if (i == 0) { vector = new Vector3(-2f, 0f, 3f);}
            if (i == 1) { vector = new Vector3(0f, 0f, 3f); }
            if (i == 2) { vector = new Vector3(2f, 0f, 3f); }

            if (makedoor)
            {
                if (i == doorPos)
                {
                    Transform pop = parentObj.transform.parent;
                    GameObject _door = Instantiate(door, pop, false);
                    _door.transform.localPosition = vector;
                    _door.transform.localPosition = new Vector3(_door.transform.localPosition.x, DoorHeight, _door.transform.localPosition.z);
                    _door.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                    _door.GetComponent<MeshRenderer>().material = materialsHomes[matInt];
                    _door.name = "Door";
                }
                else
                {
                    GameObject _window = Instantiate(windows[windowInt], parentObj.transform, false);
                    float windowHeightY;
                    if (windowInt == 1) { windowHeightY = windowHeight + 0.7f; } else { windowHeightY = windowHeight; }
                    _window.transform.localPosition = vector;
                    _window.transform.localPosition = new Vector3(_window.transform.localPosition.x, windowHeightY, _window.transform.localPosition.z);
                    _window.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                    _window.GetComponent<MeshRenderer>().material = materialsHomes[matInt];
                    _window.name = "Window";
                    AddToAvb(_window);
                }
            }
            else
            {
                GameObject _window = Instantiate(windows[windowInt], parentObj.transform, false);
                float windowHeightY;
                if (windowInt == 1) { windowHeightY = windowHeight + 0.7f; } else { windowHeightY = windowHeight; }
                _window.transform.localPosition = vector;
                _window.transform.localPosition = new Vector3(_window.transform.localPosition.x, windowHeightY, _window.transform.localPosition.z);
                _window.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                _window.GetComponent<MeshRenderer>().material = materialsHomes[matInt];
                _window.name = "Window";
                AddToAvb(_window);
            }
        }

    }

    void AddRoof(GameObject parentObj, int _material, int _roofInt, float _roofHeight, int _decoInt)
    {
        GameObject roof = Instantiate(roofs[_roofInt], parentObj.transform, false);
        roof.transform.localPosition = new Vector3(0f, _roofHeight, 0f);
        roof.transform.localScale = new Vector3(1f, 1f, 1f);
        roof.GetComponent<MeshRenderer>().material = materialsHomes[_material];
        roof.name = "Roof";
        AddToAvb(roof);
        int windowInt = Random.Range(0, windows.Length);
        if (_roofInt != 3)
        {
            AddDecorations(roof, _material, _decoInt);
            GameObject roofWindow = Instantiate(windows[windowInt], roof.transform, false);
            float windowHeightY;
            if (windowInt == 1) { windowHeightY = windowHeight + 0.7f; } else { windowHeightY = windowHeight; }
            roofWindow.transform.localPosition = new Vector3(0f, windowHeightY, 3f);
            if (windowInt != 1) { roofWindow.transform.localScale = new Vector3(.5f, .5f, 1f); } else { roofWindow.transform.localScale = new Vector3(.8f, .8f, 1f); }
            roofWindow.GetComponent<MeshRenderer>().material = materialsHomes[_material];
            AddToAvb(roofWindow);
        }
    }

    void AddDecorations(GameObject _objParent, int _matInt, int _decoInt)
    {
        GameObject decoration;

        if (_decoInt == 0)
        {
            decoration = Instantiate(decorations[_decoInt], _objParent.transform, false);
            decoration.transform.localPosition = new Vector3(0f, -0.25f, 2.9f);
            decoration.transform.localScale = new Vector3(1f, 1f, 1f);
            decoration.GetComponent<MeshRenderer>().material = materialsHomes[_matInt];
            AddToAvb(decoration);
        }
        else {
            if (_decoInt == 1)
            {
                for (int i = 0; i < 6; i++)
                {
                    float  _offset = -2.5f + i;
                    decoration = Instantiate(decorations[_decoInt], _objParent.transform, false);
                    decoration.transform.localPosition = new Vector3(_offset, -0.25f, 3f);
                    decoration.transform.localScale = new Vector3(1f, 1f, 1f);
                    decoration.GetComponent<MeshRenderer>().material = materialsHomes[_matInt];
                    AddToAvb(decoration);
                }
            }
            else {
                // no decorations.
            }
        }
    }

    void AddToAvb(GameObject _obj)
    {
        MeshRenderer mr = _obj.GetComponent<MeshRenderer>();
        avb.renderer.Add(mr);
    }
}
