using UnityEngine;
using System.Collections;

public class Conversation : MonoBehaviour {
    public string[] tree1; //The first conversation tree this is what the conversation will start with
    public string[] tree2; //The Second conversation tree this tree SHOULD be set up to begin where the conversation can branch
    public string[] options; //The choices the player has in convo
    private string[] active; //Is a placeholder for the currently stored node
    public int branchAt; //Stores where the conversation will branch from
    private int pos; //The current position

    GameObject ButtonConvo1;
    GameObject ButtonConvo2;
    GameObject ButtonNext;

	// Use this for initialization
	void Start () {
        active = new string[tree1.Length];
        active = (string[])tree1.Clone();
        pos = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (pos == branchAt) {
            Instantiate(ButtonConvo1, new Vector3(30, 0, 0), Quaternion.identity);
            Instantiate(ButtonConvo2, new Vector3(60, 0, 0), Quaternion.identity);
        }
	}

    IEnumerator doCheckAI() { //WHY IS THIS HERE
        while (true) {
            yield return new WaitForSeconds(.1f);
        }
    }

    public void switchActive(int i) { //Used to switch the currently active conversation tree
        if (i == 1) {
            active = (string[])tree1.Clone();
        } else {
            active = (string[])tree2.Clone();
            pos = 0; //Reset the position if switch to a new tree
        }
    }

    public string getOptions(int i){ //Returns the different decisions that the player can make (0 for option 1, 1 for option 2)
        return options[i];
    }

    public void increment() { //Increments the current position in the tree
        pos++;
    }

    public string getText(){ //Returns the current text in the tree
        return active[pos];
    }

}