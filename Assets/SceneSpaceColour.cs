using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSpaceColour : MonoBehaviour
{

    GameObject[] shapeList;

    void Awake()
    {
        shapeList = GameObject.FindGameObjectsWithTag("ShapeTemplate");
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject shape in shapeList)
        {
            shape.GetComponent<MeshRenderer>().material.color = this.GetComponent<MeshRenderer>().material.color;
        }
    }
}