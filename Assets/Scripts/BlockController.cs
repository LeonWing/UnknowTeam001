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
		GameCore = GameObject.Find ("GameCore");
		Manager = GameCore.GetComponent<GameManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		VerticalSpeed = Manager.VerticalSpeed;
		//print (VerticalSpeed);
		transform.position = new Vector3 (transform.position.x, transform.position.y + VerticalSpeed, transform.position.z);
	}

	void OnBecameInvisible()
	{
		Destroy (gameObject);
	}


}
