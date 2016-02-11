using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public GameObject playerObject;
	Rigidbody rigid;
    Animator anim;
    Player player;

    public enum Personality { Defensive, Offensive };
    public enum IntelligenceLevel { High, Moderate, Low };

    public Personality enemyPersonality;
    public IntelligenceLevel intelligence;

    public float health;
    public float attack;

    public bool reactionChosen;

    private enum State
    {
        STATE_CHASE,
        STATE_IDLE,
        STATE_ATTACK
    }

	// Use this for initialization
	void Start () {

        player = playerObject.GetComponent<Player>();
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
        }
        else
        {
            anim.SetBool("Chasing", false);
        }
	}

	void OnTriggerEnter(Collider hitObject) {

		if (hitObject.tag == "PlayerWeapon") {
            Weapon hitWeapon = hitObject.GetComponent<Weapon>();
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("React")) { 
                 if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Defend")) {
                    anim.SetTrigger("Hit");
                    health = health - hitWeapon.getDamage();
                    Debug.Log("Hit!");
                    if (health <= 0)
                    {
                        anim.SetTrigger("Dead");
                        Destroy(this);
                    }
                }
                 else
                 {
                     health = health - (hitWeapon.getDamage() / 2);
                     Debug.Log("Blocked!");
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
        if ((transform.position - playerObject.transform.position).sqrMagnitude < 50) {
            if ((transform.position - playerObject.transform.position).sqrMagnitude < 1.5f) {
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
        transform.LookAt(new Vector3(playerObject.transform.position.x, transform.position.y, playerObject.transform.position.z));
    }

    public static float calcAttack(int smrt, int hp, float rage)
    {
        float intent = (smrt / (20 / hp) + (rage * 5));
        return intent;
    }  

    public void receiveIntent(Player.Intent intent)
    {
        if (intent == Player.Intent.INTENT_ATTACK_LIGHT)
        {
            anim.SetInteger("Reaction", 2);
        }
        if (intent == Player.Intent.INTENT_CHARGE)
        {
            anim.SetInteger("Reaction", 2);
        }
        if (intent == Player.Intent.INTENT_IDLE)
        {
            anim.SetInteger("Reaction", 4);
        }
    }
}
