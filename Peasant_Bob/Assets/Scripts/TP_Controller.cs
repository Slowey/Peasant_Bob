using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP_Controller : MonoBehaviour {

    public static CharacterController m_characterController;
    public static TP_Controller Instance;


	// Use this for initialization
	void Awake () {
        m_characterController = GetComponent<CharacterController>();
        Instance = this;
	}
	
	// Update is called once per frame
	void Update () {
        if (Camera.main == null)
        {
            return;
        }
        GetLocomotionInput();
        TP_Motor.m_instance.UpdateMotor();
	}
    void GetLocomotionInput()
    {
        float deadZone = 0.1f;
        TP_Motor.m_instance.m_moveVector = Vector3.zero;
        float t_vert = Input.GetAxis("Vertical");
        float t_hori = Input.GetAxis("Horizontal");
        if (t_vert  > deadZone || t_vert < -deadZone)
        {
            TP_Motor.m_instance.m_moveVector += new Vector3(0,0, t_vert);
        }
        if (t_hori > deadZone || t_hori < -deadZone)
        {
            TP_Motor.m_instance.m_moveVector += new Vector3(t_hori, 0, 0);
        }

    }
}
