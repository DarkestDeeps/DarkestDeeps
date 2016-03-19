using UnityEngine;
using System.Collections;

public class IntentInterpreter : MonoBehaviour {

    int agility;

    public IntentInterpreter(int agility) 
    {
        agility = this.agility;
    }

    public Enemy.Action InterpretIntent(float patience, IntentHandler.Intent intent)
    {
        switch (intent)
        {
            case IntentHandler.Intent.INTENT_IDLE:
                if (patience <= 50)
                {
                    return Enemy.Action.STRAFE;
                }
                else {
                    return Enemy.Action.LIGHT_ATTACK;
                }
            case IntentHandler.Intent.INTENT_ATTACK_LIGHT:
                if (Random.Range(1,10) <= agility)
                {
                    return Enemy.Action.BLOCK;
                }
                else
                {
                    return Enemy.Action.IDLE;
                }
        }
        return Enemy.Action.IDLE;
    }
}
