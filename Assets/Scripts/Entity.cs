using UnityEngine;
using System.Collections;
using System;

public class Entity : MonoBehaviour {

    public float speed;
    public float maxSpeed;
    public float acceleration;
    public float deceleration;

    public Animator anim;

    private float distance;

    public enum Direction
    {
        Forward,
        Backward,
        Left,
        Right,
        None
    };

    public void run(bool run, float mod, Direction dir)
    {
        Debug.Log(mod);

        if (run)
        {

            if (dir == Direction.Forward)
            {
                anim.SetBool("RunForward", true);
                Debug.Log("ayyy");

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
                Debug.Log("Above" + speed);
            }
            else if (speed < -0.5)
            {
                speed = speed + deceleration * Time.deltaTime;
                transform.position += transform.forward * speed * Time.deltaTime;
                Debug.Log("Below" + speed);
            }
        }
    }

    public void orbitTarget(Transform target, float direction)
    {
        if (direction != 0) { 
            anim.SetBool("Strafing", true);
        }
        else
        {
           anim.SetBool("Strafing", false);
        }

        transform.RotateAround(target.transform.position, Vector3.up, -20 * direction / 14);
    }
}
