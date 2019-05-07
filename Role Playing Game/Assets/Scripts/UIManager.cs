using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Has reference to all controls, so user can set keybindings
/// </summary>
public class UIManager : MonoBehaviour
{
    //Singleton structure that can be reused; It is set up like this, because it should be able to be accessed in other areas but it should still inherit from the MonoBehavior class
    private static UIManager instance;          

    /// <summary>
    /// When UI manager is accessed; if there is no set instance, it is set and returned. (Same instance will always be returned)
    /// </summary>
    public static UIManager MyInstance      //Since it is static it may be accessed on the class level from other scripts. (Ex. under GameManager)
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }

            return instance;
        }
    }


    /// <summary>
    /// Reference array that holds all action buttons
    /// </summary>
    [SerializeField]
    private Button[] actionButtons;

    /// <summary>
    /// Keycodes for executing player actions
    /// </summary>
    private KeyCode action1, action2, action3;

    /// <summary>
    /// Reference to target enemy/npc frame
    /// </summary>
    [SerializeField]
    private GameObject targetFrame; 


    private Stat healthStat;      //Health stat used to update target frame

    [SerializeField]
    private Image portraitFace;


    
    // Start is called before the first frame update
    void Start()
    {
        healthStat = targetFrame.GetComponentInChildren<Stat>();        //Looks for Stat script under TargetFrame: TargetHealth


        //Spell Keybinds
        action1 = KeyCode.Alpha1;
        action2 = KeyCode.Alpha2;
        action3 = KeyCode.Alpha3;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(action1))
        {
            ActionButtonOnClick(0);
        }
        if (Input.GetKeyDown(action2))
        {
            ActionButtonOnClick(1);
        }
        if (Input.GetKeyDown(action3))
        {
            ActionButtonOnClick(2);
        }
    }

    /// <summary>
    /// Clicks action button based on index
    /// </summary>
    /// <param name="btnIndex"></param>
    private void ActionButtonOnClick(int btnIndex)
    {
        actionButtons[btnIndex].onClick.Invoke();   //Executes function as if it was clicked on by mouse; makes it work with pressing keys
    }

    public void ShowTargetFrame(NPC target)
    {
        targetFrame.SetActive(true);

        healthStat.Initialize(target.MyHealth.MyCurrentValue, target.MyHealth.MyMaxValue);

        portraitFace.sprite = target.MyPortrait;

        target.healthChanged += new HealthChanged(UpdateTargetFrame);   //Target healthChanged event listener; when health is changed update target frame

        target.npcRemoved += new NPCRemoved(HideTargetFrame);           //npcRemoved event listener, when NPC is removed, hides target frame
    }

    public void HideTargetFrame()
    {
        targetFrame.SetActive(false);
    }

    //Listens for target healthChanged event on NPC; called under ShowTargetFrame
    public void UpdateTargetFrame(float health)
    {
        healthStat.MyCurrentValue = health;
    }

}
