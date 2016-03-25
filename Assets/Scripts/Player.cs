using UnityEngine;
using System.Collections;

public class Player : Entity {
	
	public float movementSpeed = 10;
	public float turningSpeed = 120;

	private Camera mainCam;
	private EnemyTargeting targetTracker;
    public IntentHandler intentHandler;

    public float h;
    public float v;

	public enum State {Passive, Attacking};

	public State currentState;
    public State potentialState;

	void Start() {
		mainCam = Camera.main;
		targetTracker = gameObject.GetComponentInChildren<EnemyTargeting> ();
        weapon = gameObject.GetComponentInChildren<Weapon>();
        intentHandler = new IntentHandler();
        currentState = State.Passive;
        potentialState = State.Passive;
	}
	
	void Update() {

        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        if ((Input.GetMouseButton(1)) && (targetTracker.getEnemyCount() != 0))
        {
            potentialState = State.Attacking;
        }
        else {
            potentialState = State.Passive;
        }

        if (currentState != potentialState)
        {
            switchStates(potentialState);
        }

        if (currentState == State.Passive)
        {
            passiveStateBehaviour();
        }
        else
        {
            attackStateBehaviour();
        }
	}

	private void attackStateBehaviour() {


        anim.SetBool("AttackState", true);

		GameObject enemy = targetTracker.getTargetEnemyObject();

        lookAtTarget(enemy.transform);

        strafe(enemy.transform, h);

        switch ((int)v) {
            case 1:
                run(true, 3, Entity.Direction.Forward);
                break;
            case 0:
                run(false, 3, Entity.Direction.None);
                break;
            case -1:
                run(true, 3, Entity.Direction.Backward);
                break;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                    Debug.Log("AHHHHHHHHH");
                    StartCoroutine(dash(enemy.transform, maxSpeed * 2));
                }
                else
                {
                    lightAttack();
                }
        }

        intentHandler.checkIntent(anim);
	}

	private void passiveStateBehaviour() {

        anim.SetBool("AttackState", false);

        Vector3 cameraForward = mainCam.transform.TransformDirection(Vector3.forward);
		cameraForward.y = 0;    
	
		Vector3 cameraRight = mainCam.transform.TransformDirection(Vector3.right);
		Vector3 targetDirection = h * cameraRight + v * cameraForward;
		Vector3 lookDirection = targetDirection.normalized;
		
		if (lookDirection != Vector3.zero) {
			transform.rotation = Quaternion.LookRotation (lookDirection);
		}
		
		if (h != 0 || v != 0) {
            run(true, 1, Entity.Direction.Forward);
		} else {
            run(false, 1, Entity.Direction.Forward);
		}

	}

    public void setCurrentState(State state)
    {
        if (state == State.Passive)
        {
            currentState = State.Passive;
            anim.SetBool("AttackState", false);
        }
        else
        {
            currentState = State.Attacking;
            anim.SetBool("AttackState", true);
        }
    }

    public State getCurrentState()
    {
        return currentState;
    }

    void OnTriggerEnter(Collider hitObject)
    {
        if (hitObject.tag == "EnemyWeapon")
        {
            Weapon hitWeapon = hitObject.GetComponent<Weapon>();
            takeDamage(hitWeapon.getDamage());
        }
    }

    void switchStates(State newState)
    {
        Debug.Log("Switching!");
        resetAnimator();
        currentState = newState;
    }
}
