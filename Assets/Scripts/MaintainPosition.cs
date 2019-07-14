using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaintainPosition : MonoBehaviour
{
    //constants
    private const float EDGE_OF_WORLD = -15.0f;

    // Start is called before the first frame update
    void Start()
    {
        enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        //expand camera if pieces fall out of camera view, zoom out
        //make sure to expand camera when towers fall
        if (Mathf.Abs(transform.position.x) > GameBoard.boardSize)
        {
            Camera.main.orthographicSize = GameBoard.cameraSize + Mathf.Abs(transform.position.x) / GameBoard.boardSize;
        }
        //if shape falls out of the world, create new shape is necessary and destroy object
        if (isOutOfWorld())
        {
            if (gameObject.GetComponent<Group>().enabled)
                FindObjectOfType<createShape>().createNewObject();
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //make sure shape does not shift too much on collision with new shape
        roundPosition(0.1f);
    }


    void OnCollisionStay2D(Collision2D collision)
    {
        //round to 3 decimal places
        roundPosition(0.001f);
    }

    void roundPosition(float factor)
    {
        //convert x position to two decimal place
        float x = GameBoard.round(transform.position.x, factor);
        Vector3 newPosition = new Vector3(x, transform.position.y, transform.position.z);
        transform.position = newPosition;
    }

    bool isOutOfWorld()
    {
        //if block is out of bounds of the world
        if ((transform.position.y <= EDGE_OF_WORLD) || !GameBoard.validPosition(transform.position))
            return true;
        else
            return false;
    }
}
