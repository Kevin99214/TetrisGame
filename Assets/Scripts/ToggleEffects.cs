using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleEffects : MonoBehaviour
{
    void Start()
    {

    }

    //all future blocks placed will have lower friction
    //and colour changed lighter
    public void ToggleIcy(bool isIcy)
    {
        if (isIcy)
        {
            Debug.Log("It's Icy!");
            GameBoard.Icy = true;
        }
        else
        {
            Debug.Log("Not so Icy");
            GameBoard.Icy = false;
        }
    }

    //entire camera will have effect collider push blocks eastwards
    public void ToggleWindy(bool isWindy)
    {
        if (isWindy)
        {
            Debug.Log("Whoosh!");
            GameBoard.Windy = true;
        }
        else
        {
            Debug.Log("no whoosh");
            GameBoard.Windy = false;
        }
    }

    //base will move back and forth
    public void ToggleShaky(bool isShaky)
    {
        if (isShaky)
        {
            Debug.Log("Woah! Watch out!");
            GameBoard.Shaky = true;
        }
        else
        {
            Debug.Log("phew, we good");
            GameBoard.Shaky = false;
        }
    }

    //wind + less friction but not as low as ice
    public void ToggleRainy(bool isRainy)
    {
        if (isRainy)
        {
            Debug.Log("It's wet");
            GameBoard.Rainy = true;
        }
        else
        {
            Debug.Log("It's dry");
            GameBoard.Rainy = false;
        }
    }
}
