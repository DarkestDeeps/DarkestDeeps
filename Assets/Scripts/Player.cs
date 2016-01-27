using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	public float movementSpeed = 10;
	public float turningSpeed = 120;

	private Animator anim;
	private Camera mainCam;
	private EnemyTargeting targetTracker;

    private float h;
    private float v;

	public enum State {Passive, Attacking};
    public enum Intent { 
        INTENT_ATTACK_LIGHT = 0, 
        INTENT_CHARGE = 1, 
        INTENT_BLOCK =2, 
        INTENT_STRAFE = 3, 
        INTENT_IDLE = 4};

	public State currentState;

    private bool attacking;
    private Intent playerIntent;

    private Weapon heldWeapon;

	void Start() {
		anim = gameObject.GetComponent<Animator>();
		mainCam = Camera.main;
		targetTracker = gameObject.GetComponentInChildren<EnemyTargeting> ();
        attacking = false;
        heldWeapon = gameObject.GetComponentInChildren<Weapon>();
	}
	
	void Update() {

        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

		if (currentState == State.Passive) {
			passiveStateBehaviour ();
		} else {
			attackStateBehaviour();
		}

		if ((Input.GetMouseButton (1)) && (targetTracker.getEnemyCount() != 0)) {
			setCurrentState(State.Attacking);
		} else {
            setCurrentState(State.Passive);
		}
	}

	private void attackStateBehaviour() {

		GameObject enemy = targetTracker.getTargetEnemy();

		Vector3 targetPostition = new Vector3( enemy.transform.position.x, transform.position.y, enemy.transform.position.z ) ;
		transform.LookAt( targetPostition ) ;

        transform.LookAt(targetPostition);

        anim.SetInteger("H_Dir", (int)h);
        anim.SetInteger("V_Dir", (int)v);

        if (v == 1)
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(dashAttack(targetTracker.getTargetEnemy()));
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            attack();
        }
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
			anim.SetBool ("Running", true);
		} else {
			anim.SetBool ("Running", false);
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

    private IEnumerator dashAttack(GameObject target)
    {
        while (true) {

            anim.SetBool("Dashing", true);

            if ((transform.position - target.transform.position).sqrMagnitude > 4)
            {
                yield return null;
            }
            else 
            {
                anim.SetBool("Dashing", false);
                anim.SetTrigger("Attack");
                yield break;
            }
        }
    }

    private void attack()
    {
            anim.SetInteger("AttackChoice", Random.Range(0, 2));
            anim.SetTrigger("Attack");
    }

    public bool isAttacking()
    {
        return attacking;
    }

    public void setAttackStatus(int status)
    {
        attacking = status == 0 ? false : true;
    }

    public void setIntent(int intent)
    {
        targetTracker.broadcastIntent((Intent)intent);
    }
}
