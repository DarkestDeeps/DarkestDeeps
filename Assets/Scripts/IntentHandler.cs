using UnityEngine;
using System.Collections;

public class IntentHandler {

    public enum Intent //Holds enums for different options of intent
    {
        INTENT_IDLE = 0, 
        INTENT_ATTACK_STRONG = 1,
        INTENT_CHARGE = 2,
        INTENT_BLOCK = 3,
        INTENT_STRAFE = 4,
        INTENT_ATTACK_LIGHT = 5,
        INTENT_RETREAT = 6,
        INTENT_APPROACH = 7
    };

    private Intent currentIntent;

    public void checkIntent(Animator anim) //Checks the animation currently playing for the player and then assigns a reaction based off of it.
    {

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Slash") || anim.GetCurrentAnimatorStateInfo(0).IsName("Offhand Slash")) //If the player is doing X then Respond X
        {
            setIntent(Intent.INTENT_ATTACK_LIGHT);
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Strafe"))
        {
            setIntent(Intent.INTENT_STRAFE);
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Dash"))
        {
            setIntent(Intent.INTENT_CHARGE);
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Spin_Atack"))
        {
            setIntent(Intent.INTENT_ATTACK_STRONG);
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_Idle"))
        {
            setIntent(Intent.INTENT_IDLE);
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("RunForward"))
        {
            setIntent(Intent.INTENT_APPROACH);
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Run Back"))
        {
            setIntent(Intent.INTENT_RETREAT);
        }
    }

    public Intent getIntent() //Checks the current intent for the AI
    {
        return currentIntent;
    }

    public void setIntent(Intent i) //Sets the current intent for the Ai
    {
        currentIntent = i;
    }
}
