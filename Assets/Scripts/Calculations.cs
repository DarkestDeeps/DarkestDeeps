using UnityEngine;
using System.Collections;

public class Calculations : MonoBehaviour {


    public static float calcAttack(int smrt, int hp, float rage) { //A calculation for the intent of the AI. Used to determine what it will do
        float intent = (smrt/(20/hp)+rage); //Calculated based on intelligence, hp, and rage of the creature. Rage is the baseline & lowest percentage you can have. 0 being full rage.
        return intent; //Returns a float value to be used as a percentage (I.E. 50% chance of picking the right move)
    }

    public static float calcRanInc(int from, int to) { //Calculates a random number including the final number inputted, rather than up to
        float result = Random.Range(from, (to+1));
        return result;
    }

    public static float calcRanBetter(int from, int to, float previous) { //A better randomization method. Used to more frequently get a different output
        float result = Random.Range(from, to);
        if (result == previous) {
            result = Random.Range(from, to);
        }
        return result;
    }

    public static bool calcInsideRadius(Vector3 radius, GameObject target, GameObject current){
        if(((current.transform.position - target.transform.position).sqrMagnitude) <= radius.sqrMagnitude){
            return true;
        }else{
            return false;
        }
    }
}