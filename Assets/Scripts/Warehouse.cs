using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warehouse : MonoBehaviour
{
    public GameObject guideline;
    public PhysicsMaterial2D shapeMaterial;
    public PhysicsMaterial2D icyMaterial;
    void Start()
    {
        //get guidelines for shape
        guideline = Instantiate(guideline, new Vector3(GameBoard.StartGuidePos, 0, 0), Quaternion.identity);
    }

    void Update()
    {

    }

    //accessors and mutators
    public ref GameObject getGuideline()
    {
        return ref guideline;
    }

    public ref PhysicsMaterial2D getShapeMaterial()
    {
        return ref shapeMaterial;
    }

    public ref PhysicsMaterial2D getIcyMaterial()
    {
        return ref icyMaterial;
    }
}
