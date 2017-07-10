using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public float Speed=10.0f;//下落速度调整值
	public float HorizontalBoundary=100.0f;//飞机水平限位（中心位移量）
	public float ScreenHorizontalBoundary=45.0f;//屏幕水平边界（中心距离）
	public float ScreenVerticalBoundary=80.0f;//屏幕竖直边界（中心距离）
	public GameObject Plane;//飞机对象
	public PaperPlaneBehaviour PlaneScript;//飞机脚本对象（飞机控制器）
	public float BLockDistance=40.0f;//Block的间距
	public GameObject Block=null;//用于存储Block原始对象的
	private GameObject LastBlock;//用于存储当前最后一个被激活的Block
	private double currentScore=0;//当前分数
	private long highScore=0;//最高分数
	public delegate void OnCrash();//设置无参数过程委托
	public OnCrash oncrash=null;//定义委托对象，当飞机坠毁时调用相关过程
	private LinkedList<GameObject> VisibleBlocks;//链表，用于存储所有可见的Block
    private float horizontalSpeed;//水平速度，当前飞机平移速度
	private float verticalSpeed;//垂直速度，用于表示当前滚屏速度
	private double distance;//游戏开始后的飞机行走的总距离

	//对外水平距离只读属性
    public float HorizontalSpeed
    {
        get
        {
            return horizontalSpeed;
        }
    }
	//对外竖直速度只读属性
	public float VerticalSpeed
	{
		get
		{
			return verticalSpeed;
		}
	}
	//对外当前分数只读属性
	public long CurrentScore
	{
		get
		{ 
			return (long)currentScore;
		}
	}
	//对外最高分只读属性
	public long HighScore
	{
		get
		{
			return highScore;
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
		//获取飞机对象及飞机行为脚本
		Plane = GameObject.Find ("PaperPlane");
		PlaneScript = Plane.GetComponent<PaperPlaneBehaviour> ();
		//初始化距离
		distance = 0;
		//在左边生成第一个Block
		LastBlock = CreatBlock (-1,(long)distance);
		//添加飞机坠毁委托
		oncrash += RefreshHighScore;//增加刷新最高分委托
		oncrash += DestroyVisibleBlock;//增加清楚屏幕中激活的Block委托
		//创建激活Block的链表
		VisibleBlocks=new LinkedList<GameObject>();
		//把飞机对象放在链表第一个，用来占位，确保Block可以在链尾随意添加和删除
		VisibleBlocks.AddFirst (Plane);
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
		verticalSpeed = Time.deltaTime * Speed * PlaneScript.OneSpeed.y;
		//累加距离
		distance = distance + verticalSpeed;
		//刷新分数
		currentScore=currentScore+verticalSpeed;
        //刷新水平速度
		horizontalSpeed =Time.deltaTime * Speed* PlaneScript.OneSpeed.x;

	}

	//Block生成过程（生成器），按照距离生成Block
	private void BlockManager()
	{
		//获得上一个Block脚本
		BlockController LastBLockController = LastBlock.GetComponent<BlockController> ();
		//判定，如果当前距离不是上一个Block生成的距离，同时当距离是Block距离的整数倍的时候，生成新的Block，并定义为“当前最后的Block”
		if ((long)distance % BLockDistance == 0 && (long)distance != LastBLockController.distance)
			LastBlock = CreatBlock (-LastBLockController.p, (long)distance);
	}

	//创建Block的函数（2参数），返回一个block（GameObject类）对象，参数p为左右位置参数，-1表示在左边，1表示在右边;d表示生成的距离点
	private GameObject CreatBlock(int p,long d)
	{
		GameObject b;//用于缓存生成的Block对象
		BlockController bc;//用于缓存生成的Block对象中的脚本组件

		if (Mathf.Abs (p) == 1) 
		{
			b = Instantiate (Block, new Vector3 (p * ScreenHorizontalBoundary, -ScreenVerticalBoundary - 5, -20),Block.transform.rotation);
			bc = b.GetComponent<BlockController> ();
			//在新Block中记录生成的相关参数
			bc.p = p;
			bc.distance = d;
			//由于Block的生成、销毁都是在一帧结束后进行，所以在这里添加到链表会报错
			return b;

		}
		else
			return null;
	}

	//在链表中添加新的Block对象
	public void AddBlock(GameObject block)
	{
		VisibleBlocks.AddLast (block);
	}
	//在链表中删除Block对象，同时销毁该Block对象
	public void DeleteBlock(GameObject block)
	{
		VisibleBlocks.Remove (block);
		GameObject.Destroy (block);
	}

	//刷新最高分的过程，同时重置当前分数
	private void RefreshHighScore()
	{
		if(currentScore>highScore)
			highScore = (long)currentScore;
		currentScore = 0;
	}

	//销毁可见的Block过程（除过最后生成的，否则会造成LastBlock对象丢失，影响Block的生成（可处理，暂未处理）
	private void DestroyVisibleBlock()
	{
		LinkedListNode<GameObject> current;//定义当前链表指针
		LinkedListNode<GameObject> toBeDeleted;//用于缓存将要删除的对象
		//将指针移动至飞机之后（即第二位，第一个Block）
		current = VisibleBlocks.First;
		current = current.Next;

		//判定，当下一个的下一个（注意while用了两个next）位置有Block，则进行销毁程序（这样让位了LastBlock）
		while(current.Next!=null)
		{
			//谨慎起见避免删除飞机
			if (!current.Value.Equals (Plane)) {
				toBeDeleted = current;
				current = current.Next;
				DeleteBlock (toBeDeleted.Value);
			} else
				current = current.Next;
		}
	}
		
}
