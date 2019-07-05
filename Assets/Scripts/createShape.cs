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
    private int prevShape = -1;


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
        //create a random number
        int shapeNum = Random.Range(0, NUM_OF_SHAPES);
        //make sure a different shape is generated everytime
        while (prevShape == shapeNum)
            shapeNum = Random.Range(0, NUM_OF_SHAPES);
        //assign prevShape 
        prevShape = shapeNum;
        //get location of where to spawn shape
        float yPos = (GameBoard.Height > 0) ? (STARTING_HEIGHT + GameBoard.Height) : STARTING_HEIGHT;
        //create the shape
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