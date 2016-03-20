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
        BLOCK
    }

    IntentInterpreter intentInterpreter;

    // Use this for initialization
    void Start () {
        player = playerObject.GetComponent<Player>();
        intentInterpreter = new IntentInterpreter(agility);
        patience = 0;
        health = 10000000.0f; //PLEASE REMOVE THIS LATER
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
        }
        else
        {
            run(false, 1, Direction.Forward);
        }

        handlePlayerIntent(player.intentHandler.getIntent());
	}

	void OnTriggerEnter(Collider hitObject) {

		if (hitObject.tag == "PlayerWeapon") {
            Weapon hitWeapon = hitObject.GetComponent<Weapon>();
            takeDamage(hitWeapon.getDamage());
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
            return State.STATE_CHASE;
        }
        else
        {
            anim.SetBool("AttackState", false);
            return State.STATE_IDLE;
        }
    }

    public void handlePlayerIntent(IntentHandler.Intent intent)
    {

        Action action = intentInterpreter.InterpretIntent(patience, intent);

        if (action == Action.LIGHT_ATTACK)
        {
            lightAttack();
            ResetPatience();
        }
        else if (action == Action.BLOCK)
        {
            defend(true);
        }
    }

    public void StepPatience()
    {
        if (patience < 100) {
            patience = patience + 0.5f * Time.deltaTime;
        }
    }

    public void ResetPatience()
    {
        patience = 0;
    }
}
