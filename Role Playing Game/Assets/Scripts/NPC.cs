using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void HealthChanged(float health);   //Takes in health, is void so doesn't return. Listens for event

public delegate void NPCRemoved();

public class NPC : Character
{
    public event HealthChanged healthChanged;

    public event NPCRemoved npcRemoved;

    //Allows for initial selection of npc portrait
    [SerializeField]
    private Sprite portrait;

    /// <summary>
    /// Called in UI manager to get portrait of target
    /// </summary>
    public Sprite MyPortrait
    { get => portrait; }

    public virtual void DeSelect()  //Use virtual void so Enemy script can be overridden
    {
        healthChanged -= new HealthChanged(UIManager.MyInstance.UpdateTargetFrame);     //Unassigns healthChanged event when target deselect (Fixes when character dies)
        npcRemoved -= new NPCRemoved(UIManager.MyInstance.HideTargetFrame);             //Unassigns npcRemoved event when target deselected
    }

    public virtual Transform Select()              //Transform is the hitbox of target, trying to select
    {
        return hitBox;
    }


    //Update unit frame when enemy takes damage; Triggered in enemy script.
    public void OnHealthChanged(float health)
    {
        if(healthChanged != null)    //Checks if something is listening to event before triggered, to prevent null reference
        {
            healthChanged(health);      //Value updated by healthChanged event under UIManager
        }


    }

    public void OnNPCRemoved()
    {
        if(npcRemoved != null)      //Triggered 
        {
            npcRemoved();           //Event
        }
        Destroy(gameObject);

    }

}
