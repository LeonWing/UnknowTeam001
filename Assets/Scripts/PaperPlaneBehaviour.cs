using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperPlaneBehaviour : MonoBehaviour {

    public float RotateSpeed = 200.0f;
    private float RotateValue;
    // Use this for initialization
    void Start ()
    {
        //飞机初始位置
        transform.localEulerAngles = new Vector3(230, 0, 270);
    }
	
	// Update is called once per frame
	void Update ()
    {
        //飞机转向
        RotateValue = transform.localEulerAngles.z - Input.GetAxis("Horizontal") * RotateSpeed * Time.deltaTime;
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, Mathf.Clamp(RotateValue, 30, 150) );
    }
}
