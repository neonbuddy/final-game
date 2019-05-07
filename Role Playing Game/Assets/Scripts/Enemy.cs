using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NPC
{

    [SerializeField]
    private CanvasGroup healthGroup;    //Show healthbar when enemy targeted

    public override Transform Select()
    {
        healthGroup.alpha = 1;          //When enemy selected, healthbar is visible

        return base.Select();           //makes base function from NPC still go through
    }

    public override void DeSelect()
    {
        healthGroup.alpha = 0;          //When enemy deselected, healthbar is invisible

        base.DeSelect();           //makes base function from NPC still go through
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);        //Reduces health and updates MyCurrentValue; TakeDamage is overwritten from character script.

        OnHealthChanged(health.MyCurrentValue);     //healthChanged event is triggered by taking damage (Under UI); sending value to OnHealthChanged.
    }

}
