using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public GameObject player;
	Rigidbody rigid;
    Animator anim;

    public float health;
    public float attack;

    private enum State
    {
        STATE_CHASE,
        STATE_IDLE,
        STATE_ATTACK
    }

	// Use this for initialization
	void Start () {
	
		rigid = gameObject.GetComponent<Rigidbody> ();
        anim = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

        if (checkState() == State.STATE_CHASE)
        {
            lookAtPlayer();
            anim.SetBool("Chasing", true);
        }
        else if (checkState() == State.STATE_ATTACK)
        {
            anim.SetBool("Chasing", false);
            attackTarget();
        }
        else
        {
            anim.SetBool("Chasing", false);
            //Do nothing
        }
	}

	void OnTriggerEnter(Collider hitObject) {

		if (hitObject.tag == "PlayerWeapon") {
            Weapon hitWeapon = hitObject.GetComponent<Weapon>();
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("React")) { 
                anim.SetTrigger("Hit");
                health = health - hitWeapon.getDamage();
                if (health <= 0)
                {
                    anim.SetTrigger("Dead");
                    Destroy(this);
                }
            }
		}
	}

    void attackTarget()
    {
        anim.SetTrigger("Attack");
    }

    private State checkState() 
    {
        if ((transform.position - player.transform.position).sqrMagnitude < 50) {
            if ((transform.position - player.transform.position).sqrMagnitude < 1.5f) {
                return State.STATE_ATTACK;
            }
            return State.STATE_CHASE;
        }
        else
        {
            return State.STATE_IDLE;
        }
    }

    private void lookAtPlayer()
    {
        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
    }
}
