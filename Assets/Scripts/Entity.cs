using System;
using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour {

    public float speed;
    public float maxSpeed;
    public float acceleration;
    public float deceleration;

    public float healthPoints;

    public Animator anim;

    private float distance;

    public Weapon weapon;

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
                    speed = -maxSpeed /mod;
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
                speed = speed - deceleration * Time.deltaTime;
                transform.position += transform.forward * speed * Time.deltaTime;
            }
            else if (speed < -0.5)
            {
                speed = speed + deceleration * Time.deltaTime;
                transform.position += transform.forward * speed * Time.deltaTime;
            }
        }
    }

    public void strafe(Transform target, float direction)
    {
        if (direction != 0) { 
            anim.SetBool("Strafing", true);
        }
        else
        {
           anim.SetBool("Strafing", false);
        }

        transform.RotateAround(target.transform.position, Vector3.up, -20 * direction / 7);
    }

    public IEnumerator dash(Transform target, float dashSpeed)
    {

        while (true) { 

            float step = dashSpeed * Time.deltaTime;

            if ((transform.position - target.transform.position).sqrMagnitude > 4)
            {
                anim.SetBool("Dashing", true);
                transform.position = Vector3.MoveTowards(transform.position, target.position, step);
                yield return null;
            }
            else
            {
                anim.SetTrigger("StrongAttack");
                anim.SetBool("Dashing", false);
                yield break;
            }
        }
    }

    public void lookAtTarget(Transform target)
    {
        Vector3 targetPostition = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
        transform.LookAt(targetPostition);
    }

    public void lightAttack()
    {
        anim.SetInteger("AttackChoice", UnityEngine.Random.Range(0, 2));
        anim.SetTrigger("LightAttack");
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

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("React")) {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Defend")) {
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
}
