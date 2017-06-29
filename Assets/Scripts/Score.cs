using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    public int CurrentScore = 0;
    private Text TextScore = null;

    void Start ()
    {
        TextScore = gameObject.GetComponent<Text>();
        TextScore.text = CurrentScore.ToString();

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
