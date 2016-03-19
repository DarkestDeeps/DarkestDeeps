using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ConvButton : MonoBehaviour {
    
    Conversation conv;

	// Use this for initialization
	void Start () {
        conv = GameObject.Find("Mushman").GetComponent<Conversation>();
        if (this.name == "ButtonNext") {
            this.GetComponentInChildren<Text>().text = "Next";
        } else if (this.name == "ButtonConv1") {
           this.GetComponentInChildren<Text>().text = conv.getOptions(0);
        } else if (this.name == "ButtonConv2") {
           this.GetComponentInChildren<Text>().text = conv.getOptions(1);
        }
        this.GetComponent<Button>().onClick.AddListener(delegate {this.clicked(); });
	}
	
	// Update is called once per frame
	void Update () {
	}

    private void clicked() {
        if (this.name == "ButtonNext") {
            conv.increment();
        } else if (this.name == "ButtonConv1") {
            conv.switchActive(1);
            Destroy(this);
        } else if (this.name == "ButtonConv2") {
            conv.switchActive(2);
            Destroy(this);
        }
    }
}