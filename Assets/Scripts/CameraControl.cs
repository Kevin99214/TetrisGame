using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    //constants
    private Vector3 CAMERA_SPEED = new Vector3(0, 0.05f, 0);
    //starting height of camera
    private const float CAMERA_START_HEIGHT = -1;
    //height range
    private const float HEIGHT_RANGE = 0.1f;


    //0 represents default/reset value
    //1 represents something has been hit
    //2 represents nothing has been hit
    // private int hitCollider = 0;
    // //store raycast results
    // private RaycastHit2D hit;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(transform.position.x, CAMERA_START_HEIGHT, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        // //create ray to find tallest point
        // hit = Physics2D.Raycast(new Vector2(-20, GameBoard.Height), Vector2.right);

        // //if collider hit
        // if (hit.collider != null)
        // {
        //     //if previous check showed no collider but now collider hit
        //     //found highest point, update camera position
        //     if (hitCollider == 2)
        //     {
        //         updateCamera();
        //     }
        //     else
        //     {
        //         hitCollider = 1;
        //         GameBoard.Height = GameBoard.round(GameBoard.Height + 0.5f, 0.5f);
        //     }
        // }
        // else
        // {
        //     //if previous point showed collider hit but now no collider hit
        //     //found highest point, update camera position
        //     if (hitCollider == 1)
        //     {
        //         updateCamera();
        //     }
        //     else
        //     {
        //         hitCollider = 2;
        //         GameBoard.Height = GameBoard.round(GameBoard.Height - 0.5f, 0.5f);
        //     }
        // }

        updateCamera();

        // if block is higher than what camera can see, increase size of camera
        // Debug.Log("Height: " + GameBoard.Height + " Camera: " + (Camera.main.transform.position.y + Camera.main.orthographicSize));
        if (GameBoard.MovingHeight > (Camera.main.transform.position.y + Camera.main.orthographicSize)
                && (Camera.main.orthographicSize < GameBoard.MaxCameraSize))
        {
            Camera.main.orthographicSize += CAMERA_SPEED.y;
        }
        //camera can now see blocks, shrink camera back to default
        else if (Camera.main.orthographicSize > GameBoard.cameraSize)
        {
            Camera.main.orthographicSize -= CAMERA_SPEED.y;
        }
    }

    void updateCamera()
    {
        //check if height of tower has changed
        if (GameBoard.TempHeight != GameBoard.Height)
        {
            GameBoard.Height = GameBoard.TempHeight;
        }
        //reset temp height 
        GameBoard.TempHeight = -10f;

        //keep camera at starting height until blocks reach above 0
        if (GameBoard.Height > (CAMERA_START_HEIGHT))
        {
            //if camera is below max height and delta height is within range
            //move camera up
            if ((transform.position.y < GameBoard.Height) &&
                (Mathf.Abs(transform.position.y - GameBoard.Height) > HEIGHT_RANGE))
            {
                transform.position += CAMERA_SPEED;
            }
            //if camera is above max height and delta height is within range
            //move camera up
            else if ((transform.position.y > GameBoard.Height) &&
                (Mathf.Abs(transform.position.y - GameBoard.Height) > HEIGHT_RANGE))
            {
                transform.position -= CAMERA_SPEED;
            }
        }
    }
}
