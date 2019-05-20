using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player script which inherits some properties from the Character script
/// Controls user input, Targeting through line of sight, and attacking
/// </summary>
public class Player : Character
{
    /// <summary>
    /// Player current mana
    /// </summary>
    [SerializeField]
    private Stat mana;

    /// <summary>
    /// Player starting mana
    /// </summary>
    private float startMana = 50;

    /// <summary>
    /// Array of blocks which helps create line of sight
    /// </summary>
    [SerializeField]
    private Block[] blocks;


    /// <summary>
    /// Array that holds list of exit points for spells
    /// </summary>
    [SerializeField]
    private Transform[] exitPoints;

    /// <summary>
    /// Changes exit point array value based on key input (point where spell spawns differs by direction facing)
    /// </summary>
    private int exitIndex = 2;      //Set to 2 so it matches default down state


    private SpellBook spellBook;    //references SpellBook script

    //Variables for min and max players postion on map
    private Vector3 min, max;

    /// <summary>
    /// 
    /// </summary>
    protected override void Start()
    {
        spellBook = GetComponent <SpellBook>();             //Gives player SpellBook allowing them access to spells
        mana.Initialize(startMana, startMana);
        base.Start();

    }


    /// <summary>
    /// Overrides characters update function, so so player functions may be executed
    /// </summary>
    protected override void Update()     
    {
       
        GetInput();                         //GetInput Func (keyboard)

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, min.x, max.x),         //Keeps player in bounds
        Mathf.Clamp(transform.position.y, min.y, max.y), transform.position.z);

        base.Update();                      //Executes Character update

        


        //**Unit Testing**

        //health.MyCurrentValue = 1000;
        //health.MyCurrentValue = -1000;

        //InLineOfSight(); //Should normally only be called when pressing attack
        //Debug.Log(LayerMask.GetMask("Block"));  //Finds mask value of Block layer 8 which is 256
    }


    //Gets player input from keyboard  *TO DO ADD Customizable setting?* Possible player movement with mouse clicks
    private void GetInput()
    {
        Direction = Vector2.zero;       //Makes player maintain same movement speed *Resets to 0 after each loop*

        ////***DEBUGGING ONLY***            Testing health and mana gain/loss
        //if (Input.GetKeyDown(KeyCode.I))
        //{
        //    health.MyCurrentValue -= 10;
        //    mana.MyCurrentValue -= 10;
        //}

        //if (Input.GetKeyDown(KeyCode.O))
        //{
        //    health.MyCurrentValue += 10;
        //    mana.MyCurrentValue += 10;
        //}




        ///Gets Keyboard input
        if (Input.GetKey(KeyCode.W))
        {
            exitIndex = 0;
            Direction += Vector2.up;        //Direction is what changes the players animation in character script
        }
        if (Input.GetKey(KeyCode.D))
        {
            exitIndex = 1;
            Direction += Vector2.right;
        }
        if (Input.GetKey(KeyCode.S))
        {
            exitIndex = 2;
            Direction += Vector2.down;
        }
        if (Input.GetKey(KeyCode.A))
        {
            exitIndex = 3;
            Direction += Vector2.left;
        }
        if(IsMoving)
        {
            StopAttack();
        }

    }

    /// <summary>
    /// This makes it so that the player cannot leave the bounds of the game world
    /// </summary>
    /// <param name="min">Players minimum position/param>
    /// <param name="max">Players maximum position/param>
    public void SetLimits(Vector3 min, Vector3 max)
    {
        this.min = min;
        this.max = max;
    }

    private IEnumerator Attack(int spellIndex)               //IEnumerator allows use of yeild return
    {

        Transform currentTarget = MyTarget;

        Spell newSpell = spellBook.CastSpell(spellIndex);          //Spellbook casts a spell based on the index returned. Processed through spell array

        IsAttacking = true;                                     //Indicates player is attacking

        MyAnimator.SetBool("attack", IsAttacking);              //Starts attack animation

        yield return new WaitForSeconds(newSpell.MyCastTime);     //Hardcoded example of cast time *J-Unit Testing? or Debugging*

        if(currentTarget != null && InLineOfSight())
        {
            SpellScript s = Instantiate(newSpell.MySpellPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<SpellScript>();

            s.Initialize(currentTarget, newSpell.MyDamage, transform);     //When new spell is created use current target, damage designated by spellbook, and source which is player
        }

        //Debug.Log("Attack done");              //UNIT TESTING: Making sure casting works (look at StopAttack debug in character)
        StopAttack();       //Ends attack
    }

    /// <summary>
    /// Casts a spell
    /// </summary>
    public void CastSpell(int spellIndex)
    {
        BlockView();

        if (MyTarget != null && MyTarget.GetComponentInParent<Character>().IsAlive && !IsAttacking && !IsMoving && InLineOfSight())       //Checks if able to attack
        {
            attackRoutine = StartCoroutine(Attack(spellIndex));        //Something you do while rest of script runs
        }



    
    }

    /// <summary>
    /// Checks if target in line of sight
    /// </summary>
    /// <returns></returns>
    private bool InLineOfSight()
    {
        if (MyTarget != null)
        {
            //Calculates target's direction
            Vector3 targetDirection = (MyTarget.transform.position - transform.position).normalized;

            //Cast ray from center of player in target direction; using distance between player and target; Raycast only detects block from layer 8 block sprites w/ 2D colliders
            RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, MyTarget.transform.position), 256);

            //If ray doesn't hit block wall, then target in line of sight
            if (hit.collider == null)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Activates and deactivates line of sight blocks, based on direction player facing
    /// </summary>
    private void BlockView()        
    {
        foreach (Block b in blocks)         //Looking for class block in array blocks, everytime block is taken it is referenced by b
        {
            b.Deactivate();                 //disables all blocks
        }

        blocks[exitIndex].Activate();       //Uses exit index because it stores direction facing based on input
    }




    /// <summary>
    /// Stops Player's attack
    /// </summary>
    public void StopAttack()
    {

        spellBook.StopCasting();    //Stops spellbook cast

        IsAttacking = false;
        MyAnimator.SetBool("attack", IsAttacking);  //Stops attack animation

        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            //Debug.Log("attack stopped");
        }
    }


}
