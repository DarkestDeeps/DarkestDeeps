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
    public enum Intent { 
        INTENT_ATTACK_LIGHT = 0, 
        INTENT_CHARGE = 1, 
        INTENT_BLOCK =2, 
        INTENT_STRAFE = 3, 
        INTENT_IDLE = 4
    };

	public State currentState;

    private Intent playerIntent;

	void Start() {
		mainCam = Camera.main;
		targetTracker = gameObject.GetComponentInChildren<EnemyTargeting> ();
        weapon = gameObject.GetComponentInChildren<Weapon>();
        intentHandler = new IntentHandler();
	}
	
	void Update() {

        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

		if (currentState == State.Passive) {
			passiveStateBehaviour ();
		} else {
			attackStateBehaviour();
		}

		if ((Input.GetMouseButton(1)) && (targetTracker.getEnemyCount() != 0)) {
			setCurrentState(State.Attacking);
		} else {
            setCurrentState(State.Passive);
		}
	}

	private void attackStateBehaviour() {

		GameObject enemy = targetTracker.getTargetEnemyObject();

        lookAtTarget(enemy.transform);

        strafe(enemy.transform, h);

        switch ((int)v) {
            case 1:
                run(true, 2, Entity.Direction.Forward);
                break;
            case 0:
                run(false, 2, Entity.Direction.None);
                break;
            case -1:
                run(true, 2, Entity.Direction.Backward);
                break;
        }

        if (Input.GetMouseButtonDown(0))
        {
                if (v > 0)
                {
                    StartCoroutine(dash(enemy.transform, maxSpeed));
                }
                else
                {
                    lightAttack();
                }
        }

        intentHandler.checkIntent(anim);
	}

	private void passiveStateBehaviour() {
		
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

    void OnTriggerEnter(Collider hitObject) {
        if (hitObject.tag == "EnemyWeapon") {
            Weapon hitWeapon = hitObject.GetComponent<Weapon>();
            takeDamage(hitWeapon.getDamage());
        }
    }
}
