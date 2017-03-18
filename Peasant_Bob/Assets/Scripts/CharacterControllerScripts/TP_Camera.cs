using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP_Camera : MonoBehaviour {

    public static TP_Camera m_instance;

    public Transform TargetLookAt;
    public float m_distance = 5f;
    public float m_distanceMin = 3f;
    public float m_distanceMax = 10f;
    public float m_distanceSmooth = 0.05f;
    public float m_XMouseSense = 5f;
    public float m_YMouseSense = 5f;
    public float m_mouseWheelSense = 5f;
    public float m_XSmooth = 0.05f;
    public float m_YSmooth = 0.1f;
    public float m_YMinLimit = -40f;
    public float m_YMaxLimit = 80f;


    private float m_mouseX = 0f;
    private float m_mouseY = 0f;
    private float m_velX = 0f;
    private float m_velY = 0f;
    private float m_velZ = 0f;
    private float m_velDistance = 0f;
    private float m_startDistance = 0f;
    private float m_desiredDistance = 0f;
    private Vector3 m_position = Vector3.zero;
    private Vector3 m_desiredPosition = Vector3.zero;
    void Awake()
    {
        m_instance = this;
    }
    
	// Use this for initialization
	void Start()
    {
        m_distance = Mathf.Clamp(m_distance, m_distanceMin, m_distanceMax);
        m_startDistance = m_distance;
        Reset();
	}
	
	void LateUpdate()
    {
        if (TargetLookAt == null)
        {
            return;
        }

        HandlePlayerInput();

        CalculateDesiredPosition();

        UpdatePosition();
    }

    void HandlePlayerInput()
    {
        var deadZone = 0.01f;

        //Kommentera in för att ha enbart cameraändringarnär musknapp iklickad
        //if (Input.GetMouseButton(1))
        //{
        //    // The RMB is down get mouse Axis input
        //    m_mouseX += Input.GetAxis("Mouse X") * m_XMouseSense; 
        //    m_mouseY -= Input.GetAxis("Mouse Y") * m_YMouseSense; 
        //}

        m_mouseX += Input.GetAxis("Mouse X") * m_XMouseSense;
        m_mouseY -= Input.GetAxis("Mouse Y") * m_YMouseSense;
        // This is where we will limit MouseY
        m_mouseY = Helper.ClampAngle(m_mouseY, m_YMinLimit, m_YMaxLimit);
        if (Input.GetAxis("Mouse ScrollWheel")<-deadZone || Input.GetAxis("Mouse ScrollWheel") > deadZone)
        {
            // Zoom
            m_desiredDistance = Mathf.Clamp(m_distance - Input.GetAxis("Mouse ScrollWheel") * m_mouseWheelSense, m_distanceMin, m_distanceMax);
        }

    }

    void CalculateDesiredPosition()
    {
        // Evaluate distance
        m_distance = Mathf.SmoothDamp(m_distance, m_desiredDistance, ref m_velDistance, m_distanceSmooth);
        // Calculate desired position ska vara fel axis, av nån anledning
        m_desiredPosition = CalculatePosition(m_mouseY, m_mouseX, m_distance);
    }

    Vector3 CalculatePosition(float p_rotX, float p_rotY, float p_distance)
    {
        Vector3 t_direction = new Vector3(0, 0, -p_distance);
        Quaternion t_rotation = Quaternion.Euler(p_rotX, p_rotY, 0);
        return TargetLookAt.position + t_rotation * t_direction;
    }
    void UpdatePosition()
    {
        var posX = Mathf.SmoothDamp(m_position.x, m_desiredPosition.x, ref m_velX, m_XSmooth);
        var posY = Mathf.SmoothDamp(m_position.y, m_desiredPosition.y, ref m_velY, m_YSmooth);
        var posZ = Mathf.SmoothDamp(m_position.z, m_desiredPosition.z, ref m_velZ, m_XSmooth);
        m_position = new Vector3(posX, posY, posZ);

        transform.position = m_position;

        transform.LookAt(TargetLookAt);
    }
    public void Reset()
    {
        m_mouseX = 0f;
        m_mouseY = 10f;
        m_distance = m_startDistance;
        m_desiredDistance = m_distance;
    }
    public static void UseExistingOrCreateNewMainCamera()
    {
        GameObject t_tempCam;
        GameObject t_targetLookAt;
        TP_Camera t_myCam;

        if (Camera.main != null)
        {
            t_tempCam = Camera.main.gameObject;
        }
        else
        {
            t_tempCam = new GameObject("Main Camera");
            t_tempCam.AddComponent<Camera>();
            t_tempCam.tag = "MainCamera";
        }

        t_tempCam.AddComponent<TP_Camera>();
        t_myCam = t_tempCam.GetComponent<TP_Camera>();

        t_targetLookAt = GameObject.Find("targetLookAt") as GameObject;

        if (t_targetLookAt == null)
        {
            t_targetLookAt = new GameObject("targetLookAt");
            t_targetLookAt.transform.position = Vector3.zero;
        }

        t_myCam.TargetLookAt = t_targetLookAt.transform;
    }
}
