using UnityEngine;
using System.Collections;

public class Enemy : Entity {

	public GameObject playerObject;
    Player player;

    //public enum Personality { Defensive, Offensive };
    //public enum IntelligenceLevel { High, Moderate, Low };

    //public Personality enemyPersonality;
    //public IntelligenceLevel intelligence;

    public float health;
    public int agility;

    private float patience;

    private IntentHandler.Intent currentIntent;
    private IntentHandler.Intent potentialIntent;

    private enum State
    {
        STATE_CHASE,
        STATE_IDLE,
        STATE_ATTACK
    }
    public enum Action
    {
        STRAFE,
        IDLE,
        LIGHT_ATTACK,
        STRONG_ATTACK,
        BLOCK,
        CHARGE
    }

    IntentInterpreter intentInterpreter;

    // Use this for initialization
    void Start () {
        player = playerObject.GetComponent<Player>();
        intentInterpreter = new IntentInterpreter(agility);
        patience = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (checkState() == State.STATE_CHASE)
        {
            lookAtTarget(player.transform);
            run(true, 1, Direction.Forward);
        }
        else if (checkState() == State.STATE_ATTACK)
        {
            lookAtTarget(player.transform);
            run(false, 1, Direction.Forward);
            handlePlayerIntent(player.intentHandler.getIntent());
            StepPatience();
        }
        else
        {
            run(false, 1, Direction.Forward);
        }
	}

    //Detects if the player sword passes through enemy collider
	void OnTriggerEnter(Collider hitObject) {

		if (hitObject.tag == "PlayerWeapon") {
            Weapon hitWeapon = hitObject.GetComponent<Weapon>();
            takeDamage(hitWeapon.getDamage());
            Debug.Log(healthPoints);
		}
	}

    private State checkState() 
    {
        if ((transform.position - playerObject.transform.position).sqrMagnitude < 50) {
            if ((transform.position - playerObject.transform.position).sqrMagnitude < 5f) {
                anim.SetBool("AttackState", true);
                return State.STATE_ATTACK;
            }
            anim.SetBool("AttackState", true);
            return State.STATE_CHASE; //Sets it to follow the player
        }
        else
        {
            anim.SetBool("AttackState", false);
            return State.STATE_IDLE; //If the player is out of the range of the Enemy it idles
        }
    }

    public void handlePlayerIntent(IntentHandler.Intent intent)
    {

        Action action = intentInterpreter.InterpretIntent(patience, intent);

        if (action == Action.LIGHT_ATTACK) { //If the AI decides to do a light attack, reset the patience because it attacked.
            lightAttack(); //Do a light attack
            ResetPatience(); //Reset patience to 0
            defend(false); //Stop the AI from defending
        }
        if (action == Action.BLOCK)
        { //Blocks the attack. True for blocking, false to stop.
            defend(true);
        }
        if (action == Action.STRAFE)
        { //Is a placeholder for dodge. 1 or -1 to decide direction, 0 to not strafe. (Fix that soon? (TM))
            strafe(player.transform, player.h);
        }
        else
        {
            strafe(player.transform, 0);
        }
        if (action == Action.IDLE)
        { //Sets to Idle & disables all other movement
            defend(false);
        }
        if (action == Action.CHARGE)
        {
            StartCoroutine(dash(player.transform, maxSpeed * 2));
        }
    }



    public void StepPatience() //Increases patience by an arbitrary amount (up to 100) based on Time between frames
    {
        if (patience < 100) {
            patience = patience + 25f * Time.deltaTime;
        }
    }

    public void ResetPatience()
    {
        patience = 0;
    }
}