using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerSorter : MonoBehaviour
{
    /// <summary>
    /// Reference to players spriteRenderer
    /// </summary>
    private SpriteRenderer parentRenderer;

    /// <summary>
    /// List of obstacles player colliding with
    /// </summary>
    private List<Obstacle> obstacles = new List<Obstacle>();
    
    // Start is called before the first frame update
    void Start()
    {
        parentRenderer = transform.parent.GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// When colliding with an obstacle
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Obstacle")
        {
            //Reference to obstacle
            Obstacle o = collision.GetComponent<Obstacle>();

            //Fades out tree so player can be seen
            o.FadeOut();

            //If hit something and don't collide with anything or if sorting order of object is less player sorting order
            if(obstacles.Count == 0 || o.MySpriteRenderer.sortingOrder - 1 < parentRenderer.sortingOrder)
            {
                //Sorts player's layer so he goes behind Trees (Tree1:Layer 52 & tree2:Layer 50; so player:Layer 50)
                parentRenderer.sortingOrder = o.MySpriteRenderer.sortingOrder - 1;     //Changes sortOrder to be behind obstacle just hit  
            }
            obstacles.Add(o);       //Adds and maintains list of obstacles collided with
        }

    }

    /// <summary>
    /// When stop colliding with an obstacle
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        //If player stops colliding with an obstacle
        if (collision.tag == "Obstacle") 
        {
            //Creates reference to the obstacle
            Obstacle o = collision.GetComponent<Obstacle>();

            o.FadeIn(); //When exit, tree fades in so it's solid

            //Removes obstacle from list
            obstacles.Remove(o);
            
            //When there are no obstacles
            if(obstacles.Count == 0)
            {
                parentRenderer.sortingOrder = 200;                  //If no collision with any obstacles set sorting layer back to default
            }
            else
            {
                obstacles.Sort();
                parentRenderer.sortingOrder = obstacles[0].MySpriteRenderer.sortingOrder - 1;   //Sorts obstacles based on sorting order (hides player behind lowest sort)
            }                                                                                   //(lowest sort = high on list, High sort = low on the list)


        }

        
    }

}
