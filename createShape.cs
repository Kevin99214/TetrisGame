using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createShape : MonoBehaviour
{
    //constants
    private const float STARTING_HEIGHT = 10.0f;
    private const int NUM_OF_SHAPES = 7;

    //public variables
    public GameObject[] shapePrefabs;

    //private variables
    private GameObject shape;
    private BoxCollider2D[] colliders;
    private Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        createNewObject();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void createNewObject()
    {
        //create a random shape
        int shapeNum = Random.Range(0, NUM_OF_SHAPES);
        float yPos = (GameBoard.Height > 0) ? (STARTING_HEIGHT + GameBoard.Height) : STARTING_HEIGHT;
        shape = Instantiate(shapePrefabs[shapeNum],
                            new Vector3(0, yPos, 0),
                            Quaternion.identity);


        //get shape's rigidbody
        rb = shape.GetComponent<Rigidbody2D>();
        //set speed to constant value and gravity to 0 so to move at constant speed
        rb.velocity = new Vector2(0, -GameBoard.StartSpeed);
        rb.gravityScale = 0;

        //colliders = shape.GetComponents<BoxCollider2D>();
        //Debug.Log(colliders.Length);
    }


}