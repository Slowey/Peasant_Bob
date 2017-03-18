using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP_Animator : MonoBehaviour {

    public enum Direction
    {
        Stationary, Forward, Backward, Left, Right,
        LeftForward, RightForward, LeftBackward, RightBackward
    }


    public static TP_Animator m_instance;
    public Direction m_moveDirection { get; set; }

	void Awake()
    {
        m_instance = this;
	}

	void Update()
    {
		
	}

    public void DetermineCurrentMoveDirection()
    {
        var t_forward = false;
        var t_backward = false;
        var t_left = false;
        var t_right = false;

        if (TP_Motor.m_instance.m_moveVector.z > 0)
        {
            t_forward = true;
        }
        if (TP_Motor.m_instance.m_moveVector.z < 0)
        {
            t_backward = true;
        }
        if (TP_Motor.m_instance.m_moveVector.x > 0)
        {
            t_right = true;
        }
        if (TP_Motor.m_instance.m_moveVector.x < 0)
        {
            t_left = true;
        }
        if (t_forward)
        {
            if (t_left)
            {
                m_moveDirection = Direction.LeftForward;
            }
            else if (t_right)
            {
                m_moveDirection = Direction.RightForward;
            }
            else
            {
                m_moveDirection = Direction.Forward;
            }
        }
        else if (t_backward)
        {
            if (t_left)
            {
                m_moveDirection = Direction.LeftBackward;
            }
            else if (t_right)
            {
                m_moveDirection = Direction.RightBackward;
            }
            else
            {
                m_moveDirection = Direction.Backward;
            }
        }
        else if (t_left)
        {
            m_moveDirection = Direction.Left;
        }
        else if (t_right)
        {
            m_moveDirection = Direction.Right;
        }
        else
        {
            m_moveDirection = Direction.Stationary;
        }



        {

        }

    }
}
