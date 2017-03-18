using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP_Motor : MonoBehaviour {

    public static TP_Motor m_instance;
    public float m_moveSpeed = 10f;
    public float m_gravity = 21f;
    public float m_terminalVel = 20f;


    public Vector3 m_moveVector { get; set; }
	public float m_verticalVel { get; set; }
    // Use this for initialization
	void Awake () {
        m_instance = this;
	}
	
	// Update is called once per frame
	public void UpdateMotor () {
        SnapAlignCharacterWithCamera();
        ProcessMotion();
	}

    void ProcessMotion()
    {
        // Transform MoveVector to Wolrd Space
        m_moveVector = transform.TransformDirection(m_moveVector);

        // Normalize movevec if mag > 1
        if (m_moveVector.magnitude > 1)
        {
            m_moveVector = Vector3.Normalize(m_moveVector);
        }
        // multiply normalised movevec with movespeed
        m_moveVector *= m_moveSpeed;

        //Reapply Vertical Vel MoveVector.y
        m_moveVector = new Vector3(m_moveVector.x, m_verticalVel, m_moveVector.z);

        //Apply Gravity
        ApplyGravity();

        // Move the Character in World space
        TP_Controller.m_characterController.Move(m_moveVector * Time.deltaTime);
    }

    void ApplyGravity()
    {
        if (m_moveVector.y > -m_terminalVel)
        {
            m_moveVector = new Vector3(m_moveVector.x, m_moveVector.y - m_gravity * Time.deltaTime, m_moveVector.z);
        }
        if (TP_Controller.m_characterController.isGrounded && m_moveVector.y < -1)
        {
            m_moveVector = new Vector3(m_moveVector.x, -1, m_moveVector.z);
        }
    }

    void SnapAlignCharacterWithCamera()
    {
        if (m_moveVector.x != 0 || m_moveVector.z!=0)
        {
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x,
                Camera.main.transform.eulerAngles.y, transform.eulerAngles.z);
        }
    }
}
