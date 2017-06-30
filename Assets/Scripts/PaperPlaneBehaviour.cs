using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperPlaneBehaviour : MonoBehaviour {

    public float RotateSpeed = 200.0f;//转向速度
	public int SelfRotateSpeed=100;//自转速度
	private float LastInputAxis=0;//前一帧控制量，用来计算控制缓和曲线梯度
	private Counter Step;//自转用的计步器
	private float RotationSum=0.0f;
    // Use this for initialization
    void Start ()
    {
        //飞机初始位置
        transform.localEulerAngles = new Vector3(230, 0, 270);
		Step.step = 0;
		Step.d = 1;
    }
	
	// Update is called once per frame
	void Update ()
    {
		
		//飞机自转
		PaperPlaneSelfRotation(Input.GetAxis("Horizontal"));
		//飞机转向
		PaperPlaneRotation(Input.GetAxis("Horizontal"));
        //RotateValue = transform.localEulerAngles.z - Input.GetAxis("Horizontal") * RotateSpeed * Time.deltaTime;
        //transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, Mathf.Clamp(RotateValue, 30, 150) );
    }

	//定义自转用的计步器结构体
	private struct Counter
	{
		//步数
		public int step;
		//去、回标记，正表示去程，负表示回程
		public float d;
	}

	//飞机转向过程
	private void PaperPlaneRotation(float InputAxis)
	{
		float RotateValue=0;//转向角
		RotateValue=-InputAxis*RotateSpeed*Time.deltaTime;
		//用于测试z轴角度及转换量（调试用）
		//print (transform.eulerAngles.z+","+RotateValue);
		if (!((transform.eulerAngles.z<30&&RotateValue>0)||(transform.eulerAngles.z>150&&RotateValue<0)))
			transform.RotateAround(new Vector3(-30,30,-15),new Vector3(0,0,1),RotateValue);
	}

	//飞机自传过程
	private void PaperPlaneSelfRotation(float InputAxis)
	{
		float RotateV;//转向角
		int Direction=1;

		//定义控制方向的缓和曲线单位梯度，用来定义纸飞机自传的方向，以向右转为例，按下->时向右自转，送开始向回自转（归位）
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
			transform.Rotate (RotateV, 0, 0);
			Step.step = Step.step + Direction;
		}

		//标记去程或回程，即自转方向与控制方向一致为去程，自转方向与控制方向相反为回程
		Step.d = Direction*(InputAxis);

		//将本帧技术时的控制值赋给LastInputAxis，计算下一帧缓和曲线梯度的参数
		LastInputAxis = InputAxis;
	}
}
