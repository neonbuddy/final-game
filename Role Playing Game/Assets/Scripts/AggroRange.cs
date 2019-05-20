using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroRange : MonoBehaviour
{

    private Enemy parent;


    private void Start()    
    {
        parent = GetComponentInParent<Enemy>();     //Allows AggroRange to access parent Enemy Class to get target
    }

    /// <summary>
    /// When something enters agro range: if it is the player; targets them and moves based on enemy script
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.tag == "Player")
        {
            parent.SetTarget(collision.transform);
        }
    }






}
