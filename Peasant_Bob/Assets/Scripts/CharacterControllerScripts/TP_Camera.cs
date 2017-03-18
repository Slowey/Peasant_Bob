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
    public float m_occlusionDistanceStep = 0.5f;
    public int m_maxOcclusionChecks = 10;
    public float m_distanceResumeSmooth = 1f;


    private float m_mouseX = 0f;
    private float m_mouseY = 0f;
    private float m_velX = 0f;
    private float m_velY = 0f;
    private float m_velZ = 0f;
    private float m_velDistance = 0f;
    private float m_startDistance = 0f;
    private float m_desiredDistance = 0f;
    private float m_distanceSmoothForOcclusion = 0f;
    private float m_preOccludedDistance = 0f;
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

        var count = 0;
        do
        {
            CalculateDesiredPosition();
            count++;
        } while (CheckIfOccluded(count));

        UpdatePosition();
    }

    void HandlePlayerInput()
    {
        var deadZone = 0.01f;

        //Kommentera in för att ha enbart cameraändringarnär musknapp iklickad
       if (Input.GetMouseButton(1))
       {
           // The RMB is down get mouse Axis input
           m_mouseX += Input.GetAxis("Mouse X") * m_XMouseSense; 
           m_mouseY -= Input.GetAxis("Mouse Y") * m_YMouseSense; 
       }

        //m_mouseX += Input.GetAxis("Mouse X") * m_XMouseSense;
        //m_mouseY -= Input.GetAxis("Mouse Y") * m_YMouseSense;
        // This is where we will limit MouseY
        m_mouseY = Helper.ClampAngle(m_mouseY, m_YMinLimit, m_YMaxLimit);
        if (Input.GetAxis("Mouse ScrollWheel")<-deadZone || Input.GetAxis("Mouse ScrollWheel") > deadZone)
        {
            // Zoom
            m_desiredDistance = Mathf.Clamp(m_distance - Input.GetAxis("Mouse ScrollWheel") * m_mouseWheelSense, m_distanceMin, m_distanceMax);

            m_preOccludedDistance = m_desiredDistance;
            m_distanceSmoothForOcclusion = m_distanceSmooth;
        }

    }

    void CalculateDesiredPosition()
    {

        ResetDesiredDistance();
        // Evaluate distance
        m_distance = Mathf.SmoothDamp(m_distance, m_desiredDistance, ref m_velDistance, m_distanceSmoothForOcclusion);
        // Calculate desired position ska vara fel axis, av nån anledning
        m_desiredPosition = CalculatePosition(m_mouseY, m_mouseX, m_distance);
    }

    Vector3 CalculatePosition(float p_rotX, float p_rotY, float p_distance)
    {
        Vector3 t_direction = new Vector3(0, 0, -p_distance);
        Quaternion t_rotation = Quaternion.Euler(p_rotX, p_rotY, 0);
        return TargetLookAt.position + t_rotation * t_direction;
    }

    bool CheckIfOccluded(int p_count)
    {
        var r_isOccluded = false;

        var t_nearestDistance = CheckCameraPoints(TargetLookAt.position, m_desiredPosition);

        if (t_nearestDistance != -1)
        {
            if (p_count < m_maxOcclusionChecks)
            {
                r_isOccluded = true;
                m_distance -= m_occlusionDistanceStep;
                // Hårdkodat for now 1234 tutorials empiriska nummer. Vi kanske ska byta för vår karaktär va 25 frånbörjan (0.25f)
                if (m_distance < 0.55f)
                {
                    m_distance = 0.55f;
                }
                else
                {
                    m_distance = t_nearestDistance - Camera.main.nearClipPlane;
                }

                m_desiredDistance = m_distance;
                m_distanceSmoothForOcclusion = m_distanceResumeSmooth;
            }
        }

        return r_isOccluded;
    }

    float CheckCameraPoints(Vector3 p_from, Vector3 p_to)
    {
        var t_nearestDistance = -1f;

        RaycastHit t_hitInfo;

        Helper.ClipPlanePoints t_clipPlanePoints = Helper.ClipPlaneAtNear(p_to);

        // Draw lines in the editor to make it easier to visualize and debug
        Debug.DrawLine(p_from, p_to + transform.forward * - Camera.main.nearClipPlane, Color.red);
        Debug.DrawLine(p_from, t_clipPlanePoints.UpperLeft);
        Debug.DrawLine(p_from, t_clipPlanePoints.LowerLeft);
        Debug.DrawLine(p_from, t_clipPlanePoints.UpperRight);
        Debug.DrawLine(p_from, t_clipPlanePoints.LowerRight);

        Debug.DrawLine(t_clipPlanePoints.UpperLeft, t_clipPlanePoints.UpperRight);
        Debug.DrawLine(t_clipPlanePoints.UpperRight, t_clipPlanePoints.LowerRight);
        Debug.DrawLine(t_clipPlanePoints.LowerRight, t_clipPlanePoints.LowerLeft);
        Debug.DrawLine(t_clipPlanePoints.LowerLeft, t_clipPlanePoints.UpperLeft);

        if (Physics.Linecast(p_from, t_clipPlanePoints.UpperLeft, out t_hitInfo) && t_hitInfo.collider.tag != "Player")
        {
            t_nearestDistance = t_hitInfo.distance;
        }

        if (Physics.Linecast(p_from, t_clipPlanePoints.LowerLeft, out t_hitInfo) && t_hitInfo.collider.tag != "Player")
        {
            if (t_hitInfo.distance < t_nearestDistance || t_nearestDistance == -1)
            {
                t_nearestDistance = t_hitInfo.distance;
            }
        }

        if (Physics.Linecast(p_from, t_clipPlanePoints.UpperRight, out t_hitInfo) && t_hitInfo.collider.tag != "Player")
        {
            if (t_hitInfo.distance < t_nearestDistance || t_nearestDistance == -1)
            {
                t_nearestDistance = t_hitInfo.distance;
            }
        }
        if (Physics.Linecast(p_from, t_clipPlanePoints.LowerRight, out t_hitInfo) && t_hitInfo.collider.tag != "Player")
        {
            if (t_hitInfo.distance < t_nearestDistance || t_nearestDistance == -1)
            {
                t_nearestDistance = t_hitInfo.distance;
            }
        }
        if (Physics.Linecast(p_from, p_to + transform.forward * -Camera.main.nearClipPlane, out t_hitInfo) && t_hitInfo.collider.tag != "Player")
        {
            if (t_hitInfo.distance < t_nearestDistance || t_nearestDistance == -1)
            {
                t_nearestDistance = t_hitInfo.distance;
            }
        }

        return t_nearestDistance;
    }

    void ResetDesiredDistance()
    {
        if (m_desiredDistance < m_preOccludedDistance)
        {
            var t_pos = CalculatePosition(m_mouseY, m_mouseX, m_preOccludedDistance);

            var t_nearestDistance = CheckCameraPoints(TargetLookAt.position, t_pos);

            if (t_nearestDistance == -1 || t_nearestDistance > m_preOccludedDistance)
            {
                m_desiredDistance = m_preOccludedDistance;
            }
        }
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
        m_preOccludedDistance = m_distance;
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
