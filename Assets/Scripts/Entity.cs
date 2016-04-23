using System;
using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{

    public float speed;
    public float strafeSpeed;
    public float dashSpeed;
    public float maxSpeed;
    public float acceleration;
    public float deceleration;

    public float healthPoints;

    public bool grabbingLedge;

    public Animator anim;

    private float distance;

    public Weapon weapon;

    public CapsuleCollider col;

    public GroundDetection groundDetector;

    public Rigidbody rigid;

    public LedgeDetection ledgeDetector;
    public ObstacleHandler obHandler;

    public enum Direction
    {
        Forward,
        Backward,
        Left,
        Right,
        None
    };

    public enum attackType
    {
        Strong,
        Light
    };

    protected virtual void Update()
    {
        anim.SetBool("Grounded", groundDetector.isGrounded());
    }

    public void run(bool run, float mod, Direction dir)
    {
        if (run)
        {

            if (dir == Direction.Forward)
            {
                anim.SetBool("RunForward", true);

                if (speed < (maxSpeed / mod))
                {
                    speed = speed + acceleration * Time.deltaTime;
                    transform.position += transform.forward * speed * Time.deltaTime;
                }
                else if (speed >= (maxSpeed / mod))
                {
                    speed = maxSpeed / mod;
                    transform.position += transform.forward * speed * Time.deltaTime;
                }
            }
            else if (dir == Direction.Backward)
            {
                anim.SetBool("RunBackward", true);

                if (speed > (maxSpeed / mod))
                {
                    speed = speed - acceleration * Time.deltaTime;
                    transform.position += transform.forward * speed * Time.deltaTime;
                }
                else if (speed <= (maxSpeed / mod))
                {
                    speed = -maxSpeed / mod;
                    transform.position += transform.forward * speed * Time.deltaTime;
                }
            }
        }
        else
        {
            anim.SetBool("RunForward", false);
            anim.SetBool("RunBackward", false);

            if (speed < 0.5f && speed > -0.5)
            {
                speed = 0;
            }
            else if (speed > 0.5)
            {
                speed = speed - deceleration / mod * Time.deltaTime;
                transform.position += transform.forward * speed * Time.deltaTime;
            }
            else if (speed < -0.5)
            {
                speed = speed + deceleration / mod * Time.deltaTime;
                transform.position += transform.forward * speed * Time.deltaTime;
            }
        }
    }

    public void strafe(Transform target, float direction)
    {
        if (direction != 0)
        {
            anim.SetBool("Strafing", true);
            if (strafeSpeed < maxSpeed / 2)
            {
                strafeSpeed = strafeSpeed + acceleration * Time.deltaTime;
                transform.position += (transform.right * strafeSpeed * Time.deltaTime) * direction;
            }
            else
            {
                strafeSpeed = maxSpeed / 2;
                transform.position += (transform.right * strafeSpeed * Time.deltaTime) * direction;
            }
        }
        else
        {
            anim.SetBool("Strafing", false);
            strafeSpeed = 0;
        }
    }

    public void dash(Transform target)
    {

        float dist = Vector3.Distance(target.position, transform.position);

        if (dist > 0.8f)
        {
            anim.SetBool("Dashing", true);
            if (dashSpeed < maxSpeed * 2)
            {
                dashSpeed = dashSpeed + acceleration * Time.deltaTime;
                transform.position += transform.forward * dashSpeed * Time.deltaTime;
            }
            else
            {
                dashSpeed = maxSpeed * 2;
                transform.position += transform.forward * dashSpeed * Time.deltaTime;
            }
        }
        else
        {
            dashSpeed = 0;
            anim.SetTrigger("StrongAttack");
            anim.SetBool("Dashing", false);
        }
    }

    public void lookAtTarget(Transform target)
    {
        if (stopLook())
        {
            Vector3 targetPostition = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
            transform.LookAt(targetPostition);
        }
    }

    public void lightAttack(Transform target)
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Slash") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Offhand Slash"))
        {
            StartCoroutine(moveDistance(transform.forward, 0.75f));
            anim.SetInteger("AttackChoice", UnityEngine.Random.Range(0, 2));
            anim.SetTrigger("LightAttack");
        }
    }

    public void strongAttack()
    {
        anim.SetTrigger("StrongAttack");
    }

    public void defend(bool def)
    {
        anim.SetBool("Defending", def);
    }

    public void takeDamage(float damage)
    {
        healthPoints = healthPoints - damage;

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("React"))
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Defend"))
            {
                healthPoints = healthPoints - damage;
                anim.SetTrigger("Hit");
            }
            else
            {
                healthPoints = healthPoints - (damage / 2);
            }
        }

        if (healthPoints <= 0)
        {
            anim.SetTrigger("Death");
            Destroy(this);
        }
    }

    public void jump()
    {
        if (groundDetector.grounded)
        {
            anim.SetTrigger("Jump");
            rigid.velocity = new Vector3(0, 8, 0);
        }
    }

    //vault over beams
    public void vault()
    {
        if (obHandler.potentialVault == true)
        {
            rigid.useGravity = false;
            col.height = 1;
            StartCoroutine(moveToBeam(new Vector3(transform.position.x, obHandler.beamPosition.y, obHandler.beamPosition.z), 0.1f));
            anim.SetTrigger("Vault");
        }
    }

    public void slide(bool slide)
    {
        if (slide == true)
        {
            anim.SetBool("Slide", true);
            col.height = 1;
            run(false, 5, Direction.Forward);
        }
        else
        {
            col.height = 2;
            anim.SetBool("Slide", false);
        }
    }

    public void grabLedge()
    {
        if (ledgeDetector.canGrabLedge())
        {
            col.height = 1;
            rigid.velocity = new Vector3(0, 0, 0);
            rigid.useGravity = false;
            StartCoroutine(moveToLedge(new Vector3(transform.position.x, ledgeDetector.getLedgeY() - 1.8f, transform.position.z), 1));
            anim.SetTrigger("GrabLedge");
        }
    }

    public void toggleWeaponCollider(int tog)
    {
        weapon.toggleCollider(tog);
    }

    public void resetAnimator()
    {
        anim.SetBool("Strafing", false);
        anim.SetBool("RunBackwards", false);
        anim.SetBool("RunForward", false);
    }

    public bool stopLook()
    { //Checks to see if the player/enemy is not attacking
        AnimatorStateInfo currentAnim = anim.GetCurrentAnimatorStateInfo(0);
        if (currentAnim.IsName("Attack_Idle"))
        { //If the attack_idle animation is currently playing, return false, signifying that the player/enemy is not attacking
            return false;
        }
        else { //Else, return false
            return true;
        }
    }

    public void dodge(Direction dir)
    {
        if (dir == Direction.Right)
        {
            StartCoroutine(moveDistance(transform.right, 1.5f));
        }
        else if (dir == Direction.Left)
        {
            StartCoroutine(moveDistance(transform.right, -1.5f));
        }
    }

    //lol
    public IEnumerator MountLedge()
    {

        anim.SetTrigger("Climb");
        Vector3 startPos = transform.position;
        Vector3 pos1 = transform.position + (transform.up * 1.55f);
        Vector3 pos2 = transform.position + (transform.forward * 0.5f); ;

        bool sectionOneComplete = false;

        while (transform.position != pos2) { 
        if (sectionOneComplete == false)
        {
            while (transform.position != pos1)
            {
                //transform.position = Vector3.Lerp(startPos, pos1, currentTime / timeToMove1);
                transform.position = Vector3.MoveTowards(transform.position, pos1, 2.5f * Time.deltaTime);

                float distance = Vector3.Distance(transform.position, pos1);

                if (distance <= 0.1f)
                {
                    startPos = transform.position;
                    pos2 = transform.position + (transform.forward * 1f);
                    sectionOneComplete = true;
                    yield return new WaitForEndOfFrame();
                }
                yield return new WaitForEndOfFrame();
             }
        }
        else
        {
            while (transform.position != pos2)
            {
                transform.position = Vector3.MoveTowards(transform.position, pos2, 1f * Time.deltaTime);

                float distance = Vector3.Distance(transform.position, pos2);

                if (distance <= 0.1f)
                {
                    grabbingLedge = false;
                    rigid.useGravity = true;
                    col.height = 2;
                    yield break;
                }
                yield return new WaitForEndOfFrame();
            }
        }
        }

    }

    //Moves to a position a certain distance away, over time
    IEnumerator moveDistance(Vector3 dir, float dist)
    {

        //time of coroutine starting, and the time it should take to reach end point
        float currentTime = 0f;
        float timeToMove = 0.5f;

        //the new position to move to, add direction (transform.forward/right/up is relative to direction facing) and then add distance
        Vector3 newPos = transform.position + (dir * dist);

        //while we arent at new position
        while (transform.position != newPos)
        {
            //get current time of frame
            currentTime += Time.deltaTime;
            //move towards new position with Lerp maths
            transform.position = Vector3.Lerp(transform.position, newPos, currentTime/timeToMove);

            //get distance between player and new position
            float distance = Vector3.Distance(transform.position, newPos);

            //if its within 0.1, break the Coroutine
            if (distance <= 0.1f)
            {
                yield break;
            }

            //if its not, start method over to continue moving
            yield return null;
        }
    }

    IEnumerator moveToLedge(Vector3 pos, float time)
    {

        float currentTime = 0f;
        float timeToMove = time;

        while (transform.position != pos)
        {
            currentTime += Time.deltaTime;

            transform.position = Vector3.Lerp(transform.position, pos, currentTime / timeToMove);

            float distance = Vector3.Distance(transform.position, pos);

            if (distance <= 0.1f)
            {
                grabbingLedge = true;
                yield break;
            }

            yield return null;
        }
    }

    IEnumerator moveToBeam(Vector3 pos, float time)
    {

        float currentTime = 0f;
        float timeToMove = time;

        while (transform.position != pos)
        {
            currentTime += Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, pos, 10f * Time.deltaTime);

            float distance = Vector3.Distance(transform.position, pos);

            if (distance <= 0.1f)
            {
                rigid.useGravity = true;
                col.height = 2;
                yield break;
            }

            yield return null;
        }
    }
}
