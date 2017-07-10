using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour {
	GameObject GameCore;
	GameManager Manager;
	public long distance;//用于标记生成的距离点
	public int p;//用于标记生成的位置，-1为左，1为右

	private float VerticalSpeed;

	// Use this for initialization
	void Start () {
		//连接游戏控制器
		GameCore = GameObject.Find ("GameCore");
		Manager = GameCore.GetComponent<GameManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		VerticalSpeed = Manager.VerticalSpeed;
		//持续按照游戏控制器的竖直移动速度向上移动
		transform.position = new Vector3 (transform.position.x, transform.position.y + VerticalSpeed, transform.position.z);
	}

	//当Block不可见时，调用游戏控制器的销毁程序
	void OnBecameInvisible()
	{
		Manager.DeleteBlock (gameObject);
	}

	//当Block可见时，在游戏控制器内进行注册
	void OnBecameVisible()
	{
		Manager.AddBlock (gameObject);
	}

}
