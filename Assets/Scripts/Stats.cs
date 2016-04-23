using UnityEngine;
using System.Collections;

public class Stats : MonoBehaviour {

    //Stuff
    public float health;
    public float stamina;

    public float speed;
    public float strafeSpeed;
    public float dashSpeed;
    public float maxSpeed;
    public float acceleration;
    public float deceleration;

    public void setHealth(float health)
    {
        this.health = health;
    }

    public void takeHealth(float mod)
    {
        this.health -= mod;
    }

    public void addHealthValue(float mod)
    {
        this.health += mod;
    }

    //Adds health percentage of current
    public void addHealthPercent(float mod)
    {
        this.health = health * (1 + (mod / 100));
    }

    //Subtracts health by percentage
    public void takeHealthPercent(float mod)
    {
        this.health = health * (mod / 100);
    }

    public float getHealth()
    {
        return this.health;
    }

    public void setStamina(float stamina)
    {
        this.stamina = stamina;
    }

    public void takeStamina(float mod)
    {
        this.stamina -= mod;
    }

    public void addStaminaValue(float mod)
    {
        this.stamina += mod;
    }

    //Adds stamina percentage of current
    public void addStaminaPercent(float mod)
    {
        this.stamina = stamina * (1 + (mod / 100));
    }

    //Subtracts stamina by percentage
    public void takeStaminaPercent(float mod)
    {
        this.stamina = stamina * (mod / 100);
    }

    public float getStamina()
    {
        return this.stamina;
    }
}
