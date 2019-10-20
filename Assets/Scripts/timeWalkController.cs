﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class timeWalkController : MonoBehaviour
{
    //define a list where the prefabs will be stored
    public static List<GameObject> myListObjects = new List<GameObject>();
    public static int numSpawned = 0;
    public static int numToSpawn = 3;
    public static int currentObjectIndex = 0;
    public static int objectsListLength = 0;
    public static GameObject currentObject;
    public static int incrementObject = 0;
    public static int trackablesTotal = 0;
    public Text objectNameText;
    public Text debugText;
    private string objectNameString;
    private static GameObject trackables;


    // public Transform tutorialAnimationToHide;

    void Start()
    {
        // NOTE: make sure all building prefabs are inside this folder: "Assets/Resources/Prefabs"

        Object[] subListObjects = Resources.LoadAll("Prefabs", typeof(GameObject));
        foreach (GameObject subListObject in subListObjects)
        {
            GameObject lo = (GameObject)subListObject;
            myListObjects.Add(lo);
            ++objectsListLength;
        }
        GameObject myObj = Instantiate(myListObjects[currentObjectIndex]) as GameObject;
        myObj.transform.parent = gameObject.transform;
        //objectNameString = myObj.name;
        //objectNameString = objectNameString.Substring(5);
        //objectNameText.text = objectNameString.Replace("(Clone)", "");
        objectNameText.text = ""; // blank name until placed
        myObj.transform.gameObject.SetActive(false); // hide object at start (not yet placed)

        currentObject = myObj;

        debugText.text = "Started App";

        //audioData = GetComponent<AudioSource>();
        //audioData.Play(0);
    }

    public void NextPrefab(int incrementValue)
    {

        SpawnNextObject(incrementValue);
    }

    public void SpawnNextObject(int incrementNumber)
    {
        Destroy(currentObject);
        currentObjectIndex = currentObjectIndex + incrementNumber;
        if (currentObjectIndex >= objectsListLength)
        {
            currentObjectIndex = 0;
        }

        if (currentObjectIndex < 0)
        {
            currentObjectIndex = objectsListLength - 1;
        }

        GameObject myObj = Instantiate(myListObjects[currentObjectIndex]) as GameObject;

        myObj.transform.gameObject.SetActive(true); // Is this necessary???

        myObj.transform.parent = gameObject.transform; // set new myObj as child of current object?

        objectNameString = myObj.name;
        objectNameString = objectNameString.Substring(5);
        objectNameText.text = objectNameString.Replace("(Clone)", "");
        debugText.text = "New object: " + objectNameText.text;
        // myObj.transform.position = transform.position; // NO: instead we will use the object's default position

        currentObject = myObj;
        // debugText.text = "currentObjectIndex = " + currentObjectIndex;
    }

    void Update()

    {

    }


    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    ARRaycastManager m_RaycastManager;

}
