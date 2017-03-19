using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP_AimTrajectoryScript : MonoBehaviour {

    public static TP_AimTrajectoryScript m_instance;
    public int m_sections = 10;
    public float m_maxHeight = 200;
    public float m_maxLength = 100;
    public Color c1 = Color.red;
    public Color c2 = Color.blue;
    private Vector3 m_middle;
    public Vector3 m_endpoint;
    private Vector3 m_startpoint;
    void Awake()
    {
        m_instance = this;
    }
	// Use this for initialization
	void Start () {
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        lineRenderer.widthMultiplier = 0.2f;
        lineRenderer.numPositions = 0;

        // A simple 2 color gradient with a fixed alpha of 1.0f.
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(c1, 0.0f), new GradientColorKey(c2, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
            );
        lineRenderer.colorGradient = gradient;
	}
    public void ToggleTrajectory(bool on)
    {
        LineRenderer t_lineRenderer = GetComponent<LineRenderer>();
        if (on)
        {
            t_lineRenderer.numPositions = m_sections;
        }
        else
        {
            t_lineRenderer.numPositions = 0;
        }

    }
	Vector3 GetQuadraticCoordinates(float t)
    {
        var mousePos = Input.mousePosition;
        var yPosition = Mathf.Min(Screen.height, Mathf.Max(mousePos.y, 0));
        m_startpoint = transform.position;
        //m_startpoint = Vector3.zero;
        m_endpoint = transform.position + transform.forward * (yPosition / Screen.height) * m_maxLength;
        RaycastHit t_hitinfo;
        if (Physics.Raycast(m_endpoint, Vector3.down, out t_hitinfo))
        {
            m_endpoint = t_hitinfo.point;
        }
        //m_endpoint = new Vector3(0, 50, 0);
        m_middle = (m_endpoint - m_startpoint);
        float temp = m_middle.magnitude / 2f;
        m_middle = m_middle.normalized * temp + m_startpoint;
        //m_middle = new Vector3(25, 25f, 0);
        m_middle.y = (yPosition / Screen.height) * m_maxHeight;
        return Mathf.Pow(1 - t, 2) * m_startpoint + 2 * t * (1 - t) * m_middle + Mathf.Pow(t, 2) * m_endpoint;
    }
	// Update is called once per frame
	public void Update () {
        float t;
        LineRenderer t_lineRenderer = GetComponent<LineRenderer>();
        var mousePos = Input.mousePosition;
        var yPosition = Mathf.Min(Screen.height, Mathf.Max(mousePos.y, 0));
        m_startpoint = transform.position;
        //m_startpoint = Vector3.zero;
        m_endpoint = transform.position + transform.forward * (yPosition / Screen.height) * m_maxLength;

        RaycastHit t_hitinfo;
        if (Physics.Raycast(m_endpoint, Vector3.down, out t_hitinfo))
        {
            m_endpoint = t_hitinfo.point;
        }
        List<Vector3>t_vecList =  RangeCombatScript.m_instance.GetTrajectory(m_endpoint, t_lineRenderer.numPositions);
        for (int i = 0; i < t_lineRenderer.numPositions; i++)
        {
            t_lineRenderer.SetPosition(i, t_vecList[i]);
        }

        //for (int i = 0; i < t_lineRenderer.numPositions; i++)
        //{
        //    t = (i / (float)(m_sections - 1));
        //    Vector3 t_temp = GetQuadraticCoordinates(t); 
        //    t_lineRenderer.SetPosition(i, t_temp);
        //    //var t = Time.time;
        //    //t_lineRenderer.SetPosition(i, new Vector3(i * 0.5f, Mathf.Sin(i + t), 0.0f));
        //}

    }
}
