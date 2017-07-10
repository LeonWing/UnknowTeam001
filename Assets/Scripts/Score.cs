using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    private long CurrentScore = 0;//当前分数
	private long HighScore = 0;//最高分数
    private Text TextScore = null;//文本框
	GameObject GameCore;
	GameManager Manager;

    void Start ()
    {
		//获取文本框组件
        TextScore = gameObject.GetComponent<Text>();
        //TextScore.text = CurrentScore.ToString();
		//连接游戏控制器
		GameCore = GameObject.Find ("GameCore");
		Manager = GameCore.GetComponent<GameManager> ();
    }
	
	// Update is called once per frame
	void Update () {
		//从游戏控制器获得当前分数及最高分数
		CurrentScore = Manager.CurrentScore;
		HighScore = Manager.HighScore;
		//在文本框中刷新当前分数、最高分数
		TextScore.text = "Top Score: " + HighScore.ToString () + "\nCurrent Score: " + CurrentScore.ToString ();
	}
}
