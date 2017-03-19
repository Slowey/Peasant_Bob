using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP_AimTrajectoryScript : MonoBehaviour {
    public int m_sections = 10;
    public float m_maxHeight = 200;
    public Color c1 = Color.red;
    public Color c2 = Color.blue;
    private Vector3 m_middle;
    private Vector3 m_endpoint;
    private Vector3 m_startpoint;
	// Use this for initialization
	void Start () {
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        lineRenderer.widthMultiplier = 0.2f;
        lineRenderer.numPositions = m_sections;

        // A simple 2 color gradient with a fixed alpha of 1.0f.
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(c1, 0.0f), new GradientColorKey(c2, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
            );
        lineRenderer.colorGradient = gradient;
	}
	Vector3 GetQuadraticCoordinates(float t)
    {
        var mousePos = Input.mousePosition;
        var yPosition = Mathf.Min(Screen.height, Mathf.Max(mousePos.y, 0));
        m_startpoint = transform.position;
        //m_startpoint = Vector3.zero;
        m_endpoint = transform.position + transform.forward * 10f;
        //m_endpoint = new Vector3(0, 50, 0);
        m_middle = (m_endpoint - m_startpoint);
        float temp = m_middle.magnitude / 2f;
        m_middle = m_middle.normalized * temp;
        //m_middle = new Vector3(25, 25f, 0);
        m_middle.y = (yPosition / Screen.height) * m_maxHeight;
        return Mathf.Pow(1 - t, 2) * m_startpoint + 2 * t * (1 - t) * m_middle + Mathf.Pow(t, 2) * m_endpoint;
    }
	// Update is called once per frame
	void Update () {
        float t;
        LineRenderer t_lineRenderer = GetComponent<LineRenderer>();
        for (int i = 0; i < m_sections; i++)
        {
            t = (i / (float)(m_sections - 1));
            Vector3 t_temp = GetQuadraticCoordinates(t); 
            t_lineRenderer.SetPosition(i, t_temp);
            //var t = Time.time;
            //t_lineRenderer.SetPosition(i, new Vector3(i * 0.5f, Mathf.Sin(i + t), 0.0f));
        }

    }
}
