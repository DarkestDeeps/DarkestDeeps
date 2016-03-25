using UnityEngine;
using System.Collections;

public class IntentInterpreter : MonoBehaviour {

    int agility;

    IntentHandler.Intent currentIntent;
    IntentHandler.Intent potentialIntent;

    public IntentInterpreter(int agility) //Supposed to be agility, but alex goofed
    {
        this.agility = agility;
    }

    public Enemy.Action InterpretIntent(float patience, IntentHandler.Intent intent)
    {
        switch (intent) //Switch is based on what the player action is
        {
            case IntentHandler.Intent.INTENT_IDLE:
                if (patience <= 70)
                {
                    return Enemy.Action.IDLE;
                }
                else { //When Alex does one thing with dif formatting than everything else
                    return Enemy.Action.LIGHT_ATTACK;
                }
            case IntentHandler.Intent.INTENT_ATTACK_LIGHT:
                if (Random.Range(1,10) >= agility) //If greater than agility, block
                {
                    return Enemy.Action.BLOCK;
                }
                else
                {
                    break;
                }
            case IntentHandler.Intent.INTENT_STRAFE:
                if (patience > 50)
                {
                    return Enemy.Action.STRAFE;
                }
                else
                {
                    break;
                }
            case IntentHandler.Intent.INTENT_RETREAT:
                return Enemy.Action.CHARGE;
        }
        return Enemy.Action.IDLE;
    }
}