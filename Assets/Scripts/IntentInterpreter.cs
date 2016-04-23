using UnityEngine;
using System.Collections;

public class IntentInterpreter : MonoBehaviour {

    int agility;

    Entity ent;

    IntentHandler.Intent currentIntent;
    IntentHandler.Intent potentialIntent;

    public IntentInterpreter(int agility, Entity ent) //Supposed to be agility, but alex goofed
    {
        this.agility = agility;
        this.ent = ent;
    }

    public Enemy.Action InterpretIntent(float patience, IntentHandler.Intent intent)
    {
        switch (intent) //Switch is based on what the player action is
        {
            case IntentHandler.Intent.INTENT_ATTACK_LIGHT:
                if (Random.Range(1, 10) >= agility) //If greater than agility, block
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
            case IntentHandler.Intent.INTENT_APPROACH:
                if (patience < 60)
                {
                    return Enemy.Action.BLOCK;
                }
                else
                {
                    return Enemy.Action.CHARGE;
                }
            case IntentHandler.Intent.INTENT_ATTACK_STRONG:
                if (Random.Range(1, 10) >= agility) //If greater than agility, block
                {
                    return Enemy.Action.BLOCK;
                }
                else
                {
                    break;
                }
        }
        return Enemy.Action.IDLE;
    }
}