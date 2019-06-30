using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group : MonoBehaviour
{
    //constants
    private const int ROTATE_ANGLE = 90;
    private const float INITIAL_DISTANCE = 100.0f;
    //constant width of guiding rectangles
    private const int WIDTH = 10;
    //constant move distance
    private Vector3 moveDistance = new Vector3(0.5f, 0, 0);
    //constant leeway for slaming block
    private const float LEEWAY = 0.25f;

    //rigidbody of each group
    private Rigidbody2D rb;

    //bool to represent whether a shape has been created after this shape
    private bool isShapeCreated;

    //store value of raycast when user presses drop
    private RaycastHit2D hit;

    //size of each block
    private float BLOCK_SIZE;

    //array holding guidelines
    private GameObject[] guidelines;

    //time variable
    private float time = 0f;

    // Start is called before the first frame update
    void Start()
    {
        //enabled = true;

        //each shape can create one shape
        isShapeCreated = false;

        //get shapes rigidbody
        rb = GameObject.FindObjectOfType<Rigidbody2D>();

        BoxCollider2D[] boxes = GameObject.FindObjectsOfType<BoxCollider2D>();

        //get block size of shape
        BLOCK_SIZE = Camera.main.WorldToScreenPoint(new Vector3(1, 0, 0)).x -
                                Camera.main.WorldToScreenPoint(new Vector3(0, 0, 0)).x;

        drawGuideline();
    }

    // Update is called once per frame
    void Update()
    {
        //speed up downwards when down key is being pressed
        if (Input.GetKey(KeyCode.W))
        {
            rb.velocity = new Vector2(0, -GameBoard.IncreaseSpeed);
        }
        else
        {
            rb.velocity = new Vector2(0, -GameBoard.StartSpeed);
        }

        //rotate clockwise
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Rotate(0, 0, -ROTATE_ANGLE);

            float distanceLeft = findDistanceToBlock(true /* true = x-axis*/, false /* false = left*/);
            float distanceRight = findDistanceToBlock(true /* true = x-axis*/, true /* true = right*/);

            if (!inBoard() || !isValidPosition(distanceLeft) || !isValidPosition(distanceRight))
            {
                transform.Rotate(0, 0, ROTATE_ANGLE);
            }
        }

        //rotate counter-clockwise
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.Rotate(0, 0, ROTATE_ANGLE);

            float distanceLeft = findDistanceToBlock(true /* true = x-axis*/, false /* false = left*/);
            float distanceRight = findDistanceToBlock(true /* true = x-axis*/, true /* true = right*/);

            if (!inBoard() || !isValidPosition(distanceLeft) || !isValidPosition(distanceRight))
            {
                transform.Rotate(0, 0, -ROTATE_ANGLE);
            }
        }

        //move left
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if ((Time.time - time) >= GameBoard.MoveTime)
            {
                float distanceLeft = findDistanceToBlock(true /* true = x-axis*/, false /* false = left*/);

                if (distanceLeft < moveDistance.x && distanceLeft > 0)
                    transform.position -= new Vector3(distanceLeft, 0, 0);
                else
                    transform.position -= moveDistance;

                if (!inBoard() || !isValidPosition(distanceLeft))
                {
                    transform.position += moveDistance;
                }
                time = Time.time;
            }
        }

        //move right
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if ((Time.time - time) >= GameBoard.MoveTime)
            {
                float distanceRight = findDistanceToBlock(true /* true = x-axis*/, true /* true = right*/);

                if (distanceRight < moveDistance.x && distanceRight > 0)
                    transform.position += new Vector3(distanceRight, 0, 0);
                else
                    transform.position += moveDistance;

                if (!inBoard() || !isValidPosition(distanceRight))
                {
                    transform.position -= moveDistance;
                }
                time = Time.time;
            }
        }

        //slam left
        if (Input.GetKeyDown(KeyCode.A))
        {
            //Debug.Log("slam left " + Time.time);

            // float distance = findDistanceToBlock(true /* true = x-axis*/, false /* false = left*/);

            // //slam piece to position right before next block
            // if (distance != INITIAL_DISTANCE)
            //     transform.position -= new Vector3(distance - 0.01f, 0, 0);

            getOpenSpace(false);
        }

        //slam right
        if (Input.GetKeyDown(KeyCode.D))
        {
            //Debug.Log("slam right " + Time.time);

            // float distance = findDistanceToBlock(true /* true = x-axis*/, true /* true = right*/);

            // // slam piece to position right before next block
            // if (distance != INITIAL_DISTANCE)
            //     transform.position += new Vector3(distance - 0.01f, 0, 0);

            getOpenSpace(true);
        }

        //user has decided position, drop piece downwards
        if (Input.GetKeyDown(KeyCode.S))
        {
            //get distance from current block to blocks below
            float distance = findDistanceToBlock(false /* false = y-axis*/, false /* false = down*/);
            //slam piece to position right before next block
            transform.position -= new Vector3(0, distance - 0.05f, 0);

        }

        drawGuideline();
    }

    // void OnGUI()
    // {
    //     foreach (Transform child in transform)
    //     {
    //         //find distance from each child object to block below
    //         float distance = findDistanceToBlock(true /* true = x-axis*/, true /* true = right*/);
    //         //if distance shorter than initial distance cannot be found, set to 0
    //         if (distance == INITIAL_DISTANCE)
    //             distance = 0;

    //         Vector3 pos = Camera.main.WorldToScreenPoint(child.position);

    //         GUI.DrawTexture(new Rect(pos.x - WIDTH / 2,
    //                                  Screen.height - pos.y,
    //                                  distance * BLOCK_SIZE, WIDTH),
    //                                  GameBoard.Texture);

    //         distance = findDistanceToBlock(true /* true = x-axis*/, false /* false = left*/);
    //         //if distance shorter than initial distance cannot be found, set to 0
    //         if (distance == INITIAL_DISTANCE)
    //             distance = 0;

    //         pos = Camera.main.WorldToScreenPoint(child.position);

    //         GUI.DrawTexture(new Rect(pos.x - WIDTH / 2,
    //                                  Screen.height - pos.y,
    //                                  -distance * BLOCK_SIZE, WIDTH),
    //                                  GameBoard.Texture);
    //     }

    // }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //get all contact points
        int numContacts = collision.GetContacts(GameBoard.contacts);
        //search all contact points
        //if contact point is normal pointing in +ve y-direction
        //block has landed on a piece
        for (int i = 0; i < numContacts; i++)
        {
            if (Mathf.Round(GameBoard.contacts[i].normal.y) == 1)
            {
                createNewBlock();
            }
        }
    }

    //check if all blocks are in board bounds
    bool inBoard()
    {
        foreach (Transform child in transform)
        {
            Vector3 pos = child.position;
            if (!GameBoard.validPosition(pos))
            {
                return false;
            }
        }

        return true;
    }

    //MAY NEED DEBUGGING
    bool isValidPosition(float distance)
    {
        //check distance to block for valid position
        if (distance <= 0)
        {
            return false;
        }

        return true;
    }

    void createNewBlock()
    {
        //enable gravity
        rb.gravityScale = 1;

        //check to make sure only 1 shape is created
        if (!isShapeCreated)
        {
            FindObjectOfType<createShape>().createNewObject();
            isShapeCreated = true;
        }

        //check if placed block has increased height of structure
        if (transform.position.y > GameBoard.Height)
            GameBoard.Height = transform.position.y;

        //disable script
        enabled = false;
    }

    //onXAxis: true = x-axis, false = y-axis
    //isRightOrUp: true = +ve x or y, false = -ve x or y
    float findDistanceToBlock(bool onXAxis, bool isRightOrUp)
    {
        float shortestDistance = INITIAL_DISTANCE;

        //find distance from game block to block below
        foreach (Transform child in transform)
        {
            float distance;

            if (onXAxis)
                distance = findDistanceRightLeft(child.position, isRightOrUp);
            else
                distance = findDistanceUpDown(child.position, isRightOrUp);

            if (distance < shortestDistance)
                shortestDistance = distance;
        }
        return shortestDistance;
    }

    float findDistanceUpDown(Vector3 pos, bool up)
    {
        float shortestDistance = INITIAL_DISTANCE;

        //raycast from current shape downwards
        //find distance from current shape's child to shape below
        //cast ray from edge and centre of child to get more precise value
        for (float i = -0.35f; i <= 0.35f; i += 0.35f)
        {
            if (up)
                hit = Physics2D.Raycast(new Vector2(pos.x + i, pos.y + 0.5f), Vector2.up);
            else
            {
                hit = Physics2D.Raycast(new Vector2(pos.x + i, pos.y - 0.5f), Vector2.down);
            }
            //found child object with lowest distance
            if (hit.collider != null)
                if ((hit.distance < shortestDistance) && (hit.rigidbody != rb))
                    shortestDistance = hit.distance;
        }

        return shortestDistance;
    }

    float findDistanceRightLeft(Vector3 pos, bool right)
    {
        float shortestDistance = INITIAL_DISTANCE;

        //raycast from current shape either left or right
        //find distance from current shape's child to shape beside
        //cast ray from edge and centre of child to get more precise value
        for (float i = -0.35f; i <= 0.35f; i += 0.35f)
        {
            if (right)
                hit = Physics2D.Raycast(new Vector2(pos.x + 0.5f, pos.y + i), Vector2.right);
            else
                hit = Physics2D.Raycast(new Vector2(pos.x - 0.5f, pos.y + i), Vector2.left);

            //found child object with lowest distance
            if (hit.collider != null)
                if ((hit.distance < shortestDistance) && (hit.rigidbody != rb))
                    shortestDistance = hit.distance;
        }

        return shortestDistance;
    }

    //set guideline position to shape position
    void drawGuideline()
    {
        float maxX = GameBoard.StartGuidePos;
        float minX = GameBoard.StartGuidePos;
        //find width of object
        foreach (Transform child in transform)
        {
            //rounded x position
            float roundedPosX = GameBoard.round(child.position.x, 0.1f);

            //get max X position
            if (maxX == GameBoard.StartGuidePos)
                maxX = roundedPosX;
            else
            {
                if (roundedPosX > maxX)
                    maxX = roundedPosX;
            }
            //get min X position
            if (minX == GameBoard.StartGuidePos)
                minX = roundedPosX;
            else
            {
                if (roundedPosX < minX)
                    minX = roundedPosX;
            }
        }
        //set position of guideline
        FindObjectOfType<Warehouse>().getGuideline().transform.position =
            new Vector3(transform.position.x, Camera.main.transform.position.y, 0);
        //set width to cover entire shape
        FindObjectOfType<Warehouse>().getGuideline().transform.localScale = new Vector3(maxX - minX + 1, 1, 1);
    }

    void getOpenSpace(bool right)
    {
        float distance = 0f;
        Vector2 position = new Vector2(INITIAL_DISTANCE, INITIAL_DISTANCE);

        //find distance to blocks on right or left
        foreach (Transform child in transform)
        {
            for (float i = -0.35f; i <= 0.35f; i += 0.35f)
            {
                if (right)
                    hit = Physics2D.Raycast(new Vector2(child.position.x + 0.5f, child.position.y + i), Vector2.right);
                else
                    hit = Physics2D.Raycast(new Vector2(child.position.x - 0.5f, child.position.y + i), Vector2.left);

                //found child object with lowest distance
                if (hit.collider != null)
                    if ((hit.distance > distance) && (hit.rigidbody != rb))
                    {
                        distance = hit.distance;
                        if (right)
                            position = new Vector2(child.position.x + 0.35f, child.position.y + i);
                        else
                            position = new Vector2(child.position.x - 0.35f, child.position.y + i);
                    }
            }
        }

        float maxY = 0f;
        float minY = 0f;
        //check if farthest block found
        if (distance != 0f)
        {
            //cast ray upwards, get highest y value
            if (right)
                hit = Physics2D.Raycast(new Vector2(position.x + distance, position.y), Vector2.up);
            else
                hit = Physics2D.Raycast(new Vector2(position.x - distance, position.y), Vector2.up);

            if (hit.collider != null)
                maxY = hit.distance;

            //cast ray downwards, get lowest y value
            if (right)
                hit = Physics2D.Raycast(new Vector2(position.x + distance, position.y), Vector2.down);
            else
                hit = Physics2D.Raycast(new Vector2(position.x - distance, position.y), Vector2.down);

            if (hit.collider != null)
                minY = hit.distance;
        }
        //round open space width to a whole number
        float openSpaceWidth = GameBoard.round(maxY + minY, 1f);

        //Debug.Log(openSpaceWidth);

        //number of blocks on the side
        int sideBlockSize = 0;
        //position of farthest right/left block
        float xPosition = INITIAL_DISTANCE;
        //average position of farthest right/left blocks
        float averagePosition = 0f;

        foreach (Transform child in transform)
        {
            //find how many blocks are on the farthest right/left side
            if (xPosition == INITIAL_DISTANCE)
            {
                xPosition = child.position.x;
                sideBlockSize = 1;
                averagePosition = child.position.y;
            }
            else
            {
                //if slaming right
                if (right)
                {
                    //new farthest side block found
                    if (child.position.x > xPosition)
                    {
                        xPosition = child.position.x;
                        sideBlockSize = 1;
                        averagePosition = child.position.y;
                    }
                    //another block found on the farthest side
                    else if (child.position.x == xPosition)
                    {
                        sideBlockSize++;
                        averagePosition += child.position.y;
                    }
                }
                else
                {
                    if (child.position.x < xPosition)
                    {
                        xPosition = child.position.x;
                        sideBlockSize = 1;
                        averagePosition = child.position.y;
                    }
                    else if (child.position.x == xPosition)
                    {
                        sideBlockSize++;
                        averagePosition += child.position.y;
                    }
                }
            }
        }

        //get average position
        averagePosition = averagePosition / sideBlockSize;

        //get distance to shift block towards
        if (right)
            hit = Physics2D.Raycast(new Vector2(xPosition + 0.5f, averagePosition), Vector2.right);
        else
            hit = Physics2D.Raycast(new Vector2(xPosition - 0.5f, averagePosition), Vector2.left);

        //make sure distance is found
        if (hit.collider != null)
            distance = hit.distance;

        if (((averagePosition - sideBlockSize / 2) > (position.y - minY - LEEWAY))
                && ((averagePosition + sideBlockSize / 2) < (position.y + maxY + LEEWAY)))
        {
            if (right)
                transform.position += new Vector3(distance, 0, 0);
            else
                transform.position -= new Vector3(distance, 0, 0);

            createNewBlock();

            Debug.Log(distance);
        }

        //Debug.Log(distance);

        /* *************************************************************************************************** */
        /* CURRENTLY WORKS UNTIL THIS POINT, SLAMS BLOCK INTO THE CORRECT SIDE */
        /* NEXT STEP: 1. MAKE SURE TO SHIFT BLOCK INTO RIGHT PLACE, AVOID COLLISION DUE TO LEEWAY */
        /*            2. FIX BUG WITH SLAMING TOO FAR WHEN SLAM BUTTON CLICKED, DISTANCE IS MUCH LARGER THAN ACTUAL */
        /* *************************************************************************************************** */
    }
}
