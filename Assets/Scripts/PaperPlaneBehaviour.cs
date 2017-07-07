using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperPlaneBehaviour : MonoBehaviour {

    public float RotateSpeed = 200.0f;//转向速度
	public int SelfRotateSpeed=2;//自转速度
	private float LastInputAxis=0;//前一帧控制量，用来计算控制缓和曲线梯度
	private Counter Step;//自转用的计步器
	public float RotationLimite = 60;//飞机转向角限制参数
	public float VerticalWidth=5;//垂直坠落半角宽度
	private float HorizontalBoundary;//水平边界
	private oneSpeed onespeed;
	public oneSpeed OneSpeed //单位速度
	{
		get
		{
			return onespeed;	
		}
	}

	GameObject GameCore;//游戏核心对象（内附控制器）
	GameManager Manager;//游戏核心的脚本对象（控制器）

    // Use this for initialization
    void Start ()
    {
        //飞机初始位置
        transform.localEulerAngles = new Vector3(230, 0, 270);
		Step.step = 0;
		Step.d = 1;
		if (SelfRotateSpeed < 0)
			SelfRotateSpeed = 0;
		else if (SelfRotateSpeed > 3)
			SelfRotateSpeed = 3;
		if (RotationLimite < 0)
			RotationLimite = 0;
		else if (RotationLimite >= 80)
			RotationLimite = 80;

		//获取游戏核心对象
		GameCore=GameObject.Find("GameCore");
		//获取控制器
		Manager = GameCore.GetComponent<GameManager> ();
		//从控制器中抓取水平边界值
		HorizontalBoundary = Manager.HorizontalBoundary;
    }
	
	// Update is called once per frame
	void Update ()
    {
		float Axis;
		Axis = Input.GetAxis ("Horizontal");
		//飞机自转
		PaperPlaneSelfRotation(Axis);
		//飞机转向
		PaperPlaneRotation(Axis);

		//飞机移动
		PaperPlaneMotion();

		//刷新飞机的单位速度
		onespeed.x = CalOneSpeedX ();
		onespeed.y = CalOneSpeedY ();
        //RotateValue = transform.localEulerAngles.z - Input.GetAxis("Horizontal") * RotateSpeed * Time.deltaTime;
        //transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, Mathf.Clamp(RotateValue, 30, 150) );
    }

	//当碰撞开始时调用
	void OnCollisionEnter()
	{
		
	}

	//单位速度，包括x分量和y分量
	public struct oneSpeed
	{
		public float x;
		public float y;
	}

	//定义自转用的计步器结构体
	private struct Counter
	{
		//步数
		public int step;
		//去、回标记，正表示去程，负表示回程
		public float d;
	}

	//根据飞机角度计算Y分量单位速度（cos值）
	private float CalOneSpeedY()
	{
		return Mathf.Cos( Mathf.Abs (transform.eulerAngles.z - 90) * Mathf.Deg2Rad);
	}

	//根据飞机角度计算X分量单位速度（sin值）
	private float CalOneSpeedX()
	{
		return Mathf.Sin (Mathf.Abs (transform.eulerAngles.z - 90) * Mathf.Deg2Rad);
	}

	//飞机移动过程
	private void PaperPlaneMotion()
	{
		float Motion;//每帧的移动量
		int Direction;//移动方向，-1向左，1向右

		//方向判断，在第三象限角度方向为-1，第四象限角度方向为1，在垂直宽度范围内（Game Designer定义）为垂直坠落，方向为0
		if (transform.eulerAngles.z < (90 - VerticalWidth))
			Direction = 1;
		else if (transform.eulerAngles.z > (90 + VerticalWidth))
			Direction = -1;
		else
			Direction = 0;

		//定义位移量为方向*水平速度参数（从游戏控制器抓取）*帧时间
		Motion = Direction * Manager.HorizontalSpeed * Time.deltaTime;

		//测试用
		//print (Mathf.Abs(transform.position.x+Motion) + ", "+HorizontalBoundary+", "+(Mathf.Abs(transform.position.x+Motion)<HorizontalBoundary));

		//当飞机位置在水平边界内（Game Designer定义）可以进行移动
		if(Mathf.Abs(transform.position.x+Motion)<HorizontalBoundary)
			transform.position = new Vector3 (transform.position.x + Motion, transform.position.y ,transform.position.z);
	}

	//飞机移动的物理模型过程（测试用）
	private void PaperPlaneMotionGravity(float InputAxis)
	{

	}

	//飞机转向过程
	private void PaperPlaneRotation(float InputAxis)
	{
		float RotateValue=0;//转向角
		RotateValue=InputAxis*RotateSpeed*Time.deltaTime;
		//用于测试z轴角度及转换量（调试用）
		//print (transform.eulerAngles.z+","+RotateValue);
		if (!((transform.eulerAngles.z<(90-RotationLimite)&&RotateValue>0)||(transform.eulerAngles.z>(90+RotationLimite)&&RotateValue<0)))
			transform.RotateAround(transform.position,new Vector3(0,0,1),RotateValue);
	}

	//飞机自传过程
	private void PaperPlaneSelfRotation(float InputAxis)
	{
		float RotateV;//转向角
		int Direction=1;//单位梯度，初始值定义为1，即按下按键阶段

		//定义控制方向的缓和曲线单位梯度，用来定义纸飞机自传的方向，以向右转为例，按下->时向右自转，松开向回自转（归位）
		if (InputAxis - LastInputAxis > 0)
			Direction = 1;
		else if (InputAxis - LastInputAxis < 0)
			Direction = -1;
		else
			Direction = 0;
		
		//测试用的显示去、回标记以及自转步数统计
		//print (Step.d + "," + Step.step);

		//为了限定步数少于缓和曲线帧数（为了避免缓和曲线开始、结束帧数不同造成的无法归位，同时判定去程步数小于10帧，回程步数到0后停止。） 
		if ((Step.d>=0&&Mathf.Abs( Step.step+Direction)<11)||(Step.d<=0&&Step.step>0&&(Step.step+Direction>=0))||(Step.d<=0&&Step.step<0&&(Step.step+Direction<=0)))
		{
			RotateV = Direction * SelfRotateSpeed ;
			transform.Rotate (-RotateV, 0, 0);
			Step.step = Step.step + Direction;
		}

		//标记去程或回程，即自转方向与控制方向一致为去程，自转方向与控制方向相反为回程
		Step.d = Direction*(InputAxis);

		//将本帧技术时的控制值赋给LastInputAxis，计算下一帧缓和曲线梯度的参数
		LastInputAxis = InputAxis;
	}
}
