using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour {
    Dictionary<ResourceType, List<GameObject>> m_allResources = new Dictionary<ResourceType, List<GameObject>>(); // This should perhaps be in a resource manager or something



    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {

    }

    public void AddNewResource(GameObject p_resource, ResourceType p_type)
    {
        if (!m_allResources.ContainsKey(p_type))
        {
            m_allResources.Add(p_type, new List<GameObject>());
        }
        m_allResources[p_type].Add(p_resource);
    }

    public GameObject FindClosestResourceOfType(Vector3 p_worldPosition, float p_maxDistance, ResourceType p_type)
    {
        if (!m_allResources.ContainsKey(p_type))
        {
            // We dont have that resource
            return null;
        }
        float shortestDistance = p_maxDistance;
        GameObject closestResource = null;
        foreach (var item in m_allResources[p_type])
        {
            float distance = Vector3.Distance(p_worldPosition, item.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestResource = item;
            }
        }
        return closestResource;
    }
}
