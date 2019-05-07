using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour, IComparable<Obstacle>       //Icompa
{

    /// <summary>
    /// Obstacles spriteRenderer
    /// </summary>
    public SpriteRenderer MySpriteRenderer { get; set; }

    /// <summary>
    /// Color used when obstacle solid; not faded
    /// </summary>
    private Color defaultColor;

    /// <summary>
    /// Color used when obstacle is faded out
    /// </summary>
    private Color fadedColor;

    /// <summary>
    /// CompareTo, used for sorting the obstacles. Compares current with nearby obstacles, picking lowest sortorder
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>    
    public int CompareTo(Obstacle other)
    {
        if(MySpriteRenderer.sortingOrder > other.MySpriteRenderer.sortingOrder)
        {
            return 1;        //Means obstace has a higher sortorder
        }
        else if(MySpriteRenderer.sortingOrder < other.MySpriteRenderer.sortingOrder)
        {
            return -1;       //Means obstacle has a lower sor order
        }
            return 0;        //Both sprites should be ordered the same; sort order equal
    }

    // Start is called before the first frame update
    void Start()
    {
        //Creates reference to the spriteRenderer
        MySpriteRenderer = GetComponent<SpriteRenderer>();

        //Creates colors for fading
        defaultColor = MySpriteRenderer.color;
        fadedColor = defaultColor;
        fadedColor.a = 0.7f;        //Lowers alpha value of obstacle so player can be seen

    }

    /// <summary>
    /// Fades out obstacle
    /// </summary>
    public void FadeOut()
    {
        MySpriteRenderer.color = fadedColor;
    }

    /// <summary>
    /// Fades in the obstacle
    /// </summary>
    public void FadeIn()
    {
        MySpriteRenderer.color = defaultColor;
    }

}
