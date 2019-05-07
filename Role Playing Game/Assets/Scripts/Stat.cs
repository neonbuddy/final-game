using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stat : MonoBehaviour
{

    private Image content;  //Image of casting bar that is being changed

    [SerializeField]
    private Text statValue;


    [SerializeField]
    private float lerpSpeed;    //Effects how fast bar fills

    private float currentFill;

    private float currentValue;

    public float MyMaxValue { get; set; }

    public float MyCurrentValue
    {
        get
        {
            return currentValue;
        }

        set
        {
            if (value > MyMaxValue)
            {
                currentValue = MyMaxValue;
            }
            else if(value < 0)
            {
                currentValue = 0;
            }
            else
            {
                currentValue = value;
            }

            currentFill = currentValue / MyMaxValue;  //Lowers to the slider level values of 0->1

            if(statValue != null)       //Used for the players healthbar since they have text. Catches error with enemy having no hp text
            {
                statValue.text = currentValue + "/" + MyMaxValue;  //Updates text value
            }

            
        }
    }

    // Used to initialize
    void Start()
    {
        content = GetComponent<Image>();        //Puts image into content so can change fill amount
        
    }

    // Update is called once per frame
    void Update()
    {
        //Makes sure bars are updated
        HandleBar();

        //Debug.Log(MyCurrentValue);  // Unit test for health above max or below min
    }

    /// <summary>
    /// Initializes health/mana bars
    /// </summary>
    /// <param name="currentValue"> Current value of bar/param>
    /// <param name="maxValue"> Max value of bar/param>
    public void Initialize(float currentValue, float maxValue)
    {
        if(content == null)
        {
            content = GetComponent<Image>();
        }

        MyMaxValue = maxValue;
        MyCurrentValue = currentValue;
        content.fillAmount = MyCurrentValue / MyMaxValue;   //Initializes target with correct values right away
    }

    /// <summary>
    /// Makes sure GUI bars update
    /// </summary>
    private void HandleBar()
    {
        if(currentFill != content.fillAmount)
        {
            //Lerps fill amount so there is smooth transition in reduction and refill
            content.fillAmount = Mathf.Lerp(content.fillAmount, currentFill, Time.deltaTime * lerpSpeed);
        }
    }

}
