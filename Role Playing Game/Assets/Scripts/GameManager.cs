using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Player player;      //Reference to player

    private NPC currentTarget;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ClickTarget();          
    }

    private void ClickTarget()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())        //If left mouse button clicked on Object with clickable layer it will target
                                                                                                  // && fixes deselection of target when mouse is hovering over UI action bars
        {

            //Translates mouse position from screen point to world point; uses object layer (mask:512) for targetable items
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 512);

            if (hit.collider != null)       //If mouse clicked on target, marks target position
            {
                if(currentTarget != null)       //If already have a target and its not dead
                {
                    currentTarget.DeSelect();   //Deselect the previous target
                }

                currentTarget = hit.collider.GetComponent<NPC>();   //Select current target

                player.MyTarget = currentTarget.Select();           //Target hitbox is returned from Select to MyTarget

                UIManager.MyInstance.ShowTargetFrame(currentTarget);
            }
            else
            {
                
                //Hides target frame
                UIManager.MyInstance.HideTargetFrame();

                if (currentTarget != null)         //If already have target and click somewhere thats not selectable
                {
                    currentTarget.DeSelect();       //Deselect target
                }

                currentTarget = null;               //Set target to null
                player.MyTarget = null;

            }


        }
    }

}
