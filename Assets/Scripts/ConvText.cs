using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ConvText : MonoBehaviour {

    Conversation conv;
    Text text;

	// Use this for initialization
	void Start () {
        conv = GameObject.Find("Mushman").GetComponent<Conversation>();
	    text = this.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        text.text = conv.getText();
	}
}