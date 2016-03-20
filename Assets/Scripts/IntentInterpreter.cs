using UnityEngine;
using System.Collections;

public class IntentInterpreter : MonoBehaviour {

    int agility;

    void Start() {
        agility = 0;
    }

    public IntentInterpreter(int agility) //Supposed to be agility, but alex goofed
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
                else { //When Alex does one thing with dif formatting than everything else
                    return Enemy.Action.LIGHT_ATTACK;
                }
            case IntentHandler.Intent.INTENT_ATTACK_LIGHT:
                if (Random.Range(1,10) >= agility)
                {
                    Debug.Log("He Blocky");
                    return Enemy.Action.BLOCK;
                }
                else
                {
                    Debug.Log("EYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYY");
                    return Enemy.Action.IDLE; //PLEASE HIT ME
                }
        }
        return Enemy.Action.IDLE;
    }
}