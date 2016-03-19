using UnityEngine;
using System.Collections;

public class IntentHandler {

    public enum Intent
    {
        INTENT_ATTACK_LIGHT = 0,
        INTENT_ATTACK_STRONG = 1,
        INTENT_CHARGE = 2,
        INTENT_BLOCK = 3,
        INTENT_STRAFE = 4,
        INTENT_IDLE = 5,
        INTENT_RETREAT = 6,
        INTENT_APPROACH = 7
    };

    private Intent currentIntent;

    public void checkIntent(Animator anim)
    {

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Slash") || anim.GetCurrentAnimatorStateInfo(0).IsName("Offhand Slash"))
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

    public Intent getIntent()
    {
        return currentIntent;
    }

    public void setIntent(Intent i)
    {
        currentIntent = i;
    }
}
