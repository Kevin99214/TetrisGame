﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaintainPosition : MonoBehaviour
{
    //constants
    private const float EDGE_OF_WORLD = -10.0f;

    // Start is called before the first frame update
    void Start()
    {
        enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
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
        //roundPosition(0.01f);
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
        if (transform.position.y <= EDGE_OF_WORLD)
            return true;
        else
            return false;
    }
}
