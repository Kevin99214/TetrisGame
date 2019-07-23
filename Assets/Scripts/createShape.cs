using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createShape : MonoBehaviour
{
    //constants
    private float STARTING_HEIGHT;
    private const int NUM_OF_SHAPES = 7;

    //public variables
    public GameObject[] shapePrefabs;

    //private variables
    private GameObject shape;
    private BoxCollider2D[] colliders;
    private Rigidbody2D rb;
    private int prevShape = -1;
    private bool differentShape = false;


    // Start is called before the first frame update
    void Start()
    {
        //get the starting height of the camera
        STARTING_HEIGHT = Camera.main.transform.position.y + Camera.main.orthographicSize;
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

        //same shape can only be allowed twice in a row
        //if shape has be used twice in a row, set to true to prevent it from a third time
        if ((prevShape == shapeNum) && differentShape)
        {
            while (prevShape == shapeNum)
            {
                shapeNum = Random.Range(0, NUM_OF_SHAPES);
            }
            differentShape = false;
        }
        else if (prevShape == shapeNum)
            differentShape = true;

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