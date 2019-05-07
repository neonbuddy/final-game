using System;
using UnityEngine;

[Serializable]       //Allows player to access block class
public class Block              //No monobehavior because it doesn't exisit on an object in the game
{
    [SerializeField]
    private GameObject first, second; //Array holding values of which 2 blocks enabled based on direction facing

    public void Deactivate()
    {
        first.SetActive(false);
        second.SetActive(false);
    }

    public void Activate()
    {
        first.SetActive(true);
        second.SetActive(true);
    }

}
