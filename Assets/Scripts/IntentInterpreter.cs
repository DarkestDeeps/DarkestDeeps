using UnityEngine;
using System.Collections;

public class IntentInterpreter : MonoBehaviour {

    int agility;

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
                    Debug.Log("He Blocky");
                    return Enemy.Action.BLOCK;
                }
                else
                {
                    return Enemy.Action.STRAFE; //PLEASE HIT ME
                }
        }
        return Enemy.Action.IDLE;
    }
}