using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public float FallSpeed=10.0f;//下落速度调整值
	public float HorizontalSpeed=10.0f;//水平速度调整值
	public float HorizontalBoundary=40.0f;//屏幕边界（中心位移量）
	public float ScreenHorizontalBoundary=45.0f;
	public float ScreenVerticalBoundary=80.0f;
	public GameObject Plane;//飞机对象
	public PaperPlaneBehaviour PlaneScript;//飞机脚本对象（飞机控制器）
	public float BLockDistance=40.0f;//
	public GameObject Block=null;
	private GameObject LastBlock;

	private float verticalSpeed;//垂直速度，用于表示当前滚屏速度
	private double distance;

	public float VerticalSpeed
	{
		get
		{
			return verticalSpeed;
		}
	}

	//设置距离对外只读属性
	public double Distance
	{
		get
		{
			return distance;
		}
	}

	// Use this for initialization
	void Start () {
		Plane = GameObject.Find ("PaperPlane");
		PlaneScript = Plane.GetComponent<PaperPlaneBehaviour> ();
		distance = 0;
		LastBlock = CreatBlock (-1,(long)distance);
	}

	// Update is called once per frame
	void Update () {
		//刷新游戏参数
		FreshParameters ();
		//利用生成器开始生成Block
		BlockManager ();
		
	}

	//刷新游戏中各种参数的过程
	private void FreshParameters()
	{
		//刷新垂直速度
		verticalSpeed = Time.deltaTime * FallSpeed * PlaneScript.OneSpeed.y;
		//累加距离
		distance = distance + verticalSpeed;

	}

	//Block生成过程（生成器），按照距离生成Block
	private void BlockManager()
	{
		BlockController LastBLockController = LastBlock.GetComponent<BlockController> ();
		if ((long)distance % BLockDistance == 0 && (long)distance != LastBLockController.distance)
			LastBlock = CreatBlock (-LastBLockController.p, (long)distance);
	}

	//创建Block的函数（2参数），返回一个block（GameObject类）对象，参数p为左右位置参数，-1表示在左边，1表示在右边;d表示生成的距离点
	private GameObject CreatBlock(int p,long d)
	{
		GameObject b;
		BlockController bc;
		if (Mathf.Abs (p) == 1) 
		{
			b = Instantiate (Block, new Vector3 (p * ScreenHorizontalBoundary, -ScreenVerticalBoundary - 5, -20),Block.transform.rotation);
			bc = b.GetComponent<BlockController> ();
			bc.p = p;
			bc.distance = d;
			return b;
		}
		else
			return null;
	}
}
