﻿using UnityEngine;
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
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("Patience: " + patience);
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

        if (action == Action.LIGHT_ATTACK) { //If the AI decides to do a light attack
            lightAttack(); //Do a light attack
            ResetPatience(); //Reset patience to 0
            defend(false); //Stop the AI from defending
        }
        else if (action == Action.BLOCK) {
            defend(true);
            strafe(player.transform, 0);
        } else if (action == Action.STRAFE) {
            strafe(player.transform, 1);
        } else if (action == Action.IDLE) {
            Debug.Log("Disabled");
            strafe(player.transform, 0);
            defend(false);
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