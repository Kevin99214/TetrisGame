using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard
{
    //variables used in all game objects
    private const float gameBoardSize = 10.0f;
    private static float currentHeight = 0.0f;
    public static ContactPoint2D[] contacts = new ContactPoint2D[10];
    private const float STARTING_SPEED = 1.5f;
    private const int INCREASED_SPEED = 15;
    private const int NUM_BLOCKS = 4;
    private const int START_GUIDE_POS = -50;
    private const float MOVE_TIME = 0.11f;
    private static Texture2D texture;

    //accessors and mutators
    public static float Height
    {
        get
        {
            return GameBoard.currentHeight;
        }
        set
        {
            GameBoard.currentHeight = value;
        }

    }

    public static float StartSpeed
    {
        get
        {
            return STARTING_SPEED;
        }
    }

    public static int StartGuidePos
    {
        get
        {
            return START_GUIDE_POS;
        }
    }

    public static int IncreaseSpeed
    {
        get
        {
            return INCREASED_SPEED;
        }
    }

    public static int blockNumber
    {
        get { return NUM_BLOCKS; }
    }

    public static float MoveTime
    {
        get { return MOVE_TIME; }
    }

    public static ref Texture2D Texture
    {
        get
        {
            if (texture == null)
            {
                texture = new Texture2D(1, 1);
                texture.SetPixel(0, 0, new Color(0.8f, 0.8f, 0.8f, 0.25f));
                texture.Apply();
            }

            return ref texture;
        }
    }

    //helper functions//

    //round a float to factor 
    public static float round(float num, float factor)
    {
        num = Mathf.Round(num / factor) * factor;
        return num;
    }

    //check if piece is in the board
    public static bool validPosition(Vector3 position)
    {
        //make sure shape does not try to move out of bounds
        //round to one decimal place
        float roundedX = GameBoard.round(position.x, 0.1f);
        if ((roundedX >= -1 * gameBoardSize) && (roundedX <= gameBoardSize))
        {
            return true;
        }

        return false;
    }
}
