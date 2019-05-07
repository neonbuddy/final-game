using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellBook : MonoBehaviour
{
    [SerializeField]
    private Image castingBar;       //Reference to player spell casting bar

    [SerializeField]
    private Image spellIcon;        //Reference to image of spell

    [SerializeField]
    private Text spellName;         //Reference to spell name

    [SerializeField]
    private Text spellCastTime;     //Reference to spell cast time

    [SerializeField]
    private CanvasGroup canvasGroup;    //Canvas group that allows casting bar to dissapear/ appear on casting



    /// <summary>
    /// Array that holds spells
    /// </summary>
    [SerializeField]
    private Spell[] spells;

    /// <summary>
    /// Reference to coroutine which allows spell casting
    /// </summary>
    private Coroutine spellRoutine;

    /// <summary>
    /// Reference to coroutine which fades out castBar
    /// </summary>
    private Coroutine fadeRoutine;

    public Spell CastSpell(int index)       //Inherits from spell script
    {
        castingBar.fillAmount = 0;  //Resets casting bar after each spell

        castingBar.color = spells[index].MyBarColor;    //Resets bar color
           
        spellIcon.sprite = spells[index].MyIcon;        //Resets spell Icon

        spellName.text = spells[index].MyName;          //Resets spell name

        spellRoutine = StartCoroutine(Progress(index)); //Starts spell cast, canceled if player moves

        fadeRoutine = StartCoroutine(FadeBar());        //Starts to fade casting bar

        return spells[index];
    }

    /// <summary>
    /// coroutine that updates casting bar by time left
    /// </summary>
    /// <param name="index">Index of spell to cast/param>
    /// <returns></returns>
    private IEnumerator Progress(int index)
    {
        float timePassed = Time.deltaTime;                  //Initally start time; records how much time passed

        float rate = 1.0f / spells[index].MyCastTime;       //Take max / by castTime to get rate of lowering bar

        float progress = 0.0f;                              //Current progress of spell cast (Value changes based on how much time has past)

        while(progress <= 1.0)  
        {
            castingBar.fillAmount = Mathf.Lerp(0, 1, progress);     //Updates bar based on progress

            progress += rate * Time.deltaTime;

            timePassed += Time.deltaTime;                   //Value changes based on how much time has past

            spellCastTime.text = (spells[index].MyCastTime - timePassed).ToString("F2");        //F2 means time formated 

            if(spells[index].MyCastTime - timePassed < 0)       //Makes sure time doesnt go below 0
            {
                spellCastTime.text = "0.00";
            }

            yield return null;              //Makes loop run without pausing
        }

        StopCasting();      //Resets spell Coroutine
    }

    /// <summary>
    /// Makes casting bar dissapear when spell is cast or canceled
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeBar()
    {

        float rate = 1.0f / 0.50f;  //Decides how fast casting bar fades away (num on bot: Slower = higher, faster = lower)

        float progress = 0.0f;                              //Value changes based on how much time has past

        while (progress <= 1.0)
        { 
            canvasGroup.alpha = Mathf.Lerp(0, 1, progress);
           

            progress += rate * Time.deltaTime;

            yield return null;              //Makes loop run without pausing
        }


    }

    public void StopCasting()
    {
        if(fadeRoutine != null)             //Makes casting bar disappear after casting
        {
            StopCoroutine(fadeRoutine);
            canvasGroup.alpha = 0;
            fadeRoutine = null;
        }

        if (spellRoutine != null)           //Stops spellRoutine
        {
            StopCoroutine(spellRoutine);
            spellRoutine = null;
        }
    }


}
