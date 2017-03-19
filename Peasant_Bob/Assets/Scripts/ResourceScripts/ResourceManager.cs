using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour {
    Dictionary<ResourceType, List<GameObject>> m_allResources = new Dictionary<ResourceType, List<GameObject>>(); // This should perhaps be in a resource manager or something
    Dictionary<ResourceType, int> m_availableResources = new Dictionary<ResourceType, int>();


    // Use this for initialization
    void Start() {
        m_availableResources.Add(ResourceType.Wood, 1000);
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

    public GameObject FindOneObjectOfType(Vector3 p_worldPosition, float p_maxDistance, ResourceType p_type)
    {
        if (!m_allResources.ContainsKey(p_type))
        {
            // We dont have that resource
            return null;
        }

        List<GameObject> objectsInRange = GetAllResourcesInRange(p_worldPosition, p_maxDistance, p_type);
        if (objectsInRange.Count <= 0)
        {
            return null;
        }
        int objectToSend = Random.Range(0, objectsInRange.Count - 1);
        return objectsInRange[objectToSend];
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

    public void GiveAmountOfType(int p_amount, ResourceType p_type)
    {
        if (!m_availableResources.ContainsKey(p_type))
        {
            m_availableResources.Add(p_type, 0);
        }
        m_availableResources[p_type] += p_amount;
    }

    // Returns if succesful
    public bool TakeResource(int p_amount, ResourceType p_type)
    {
        if (ResourceExists(p_amount, p_type))
        {
            m_availableResources[p_type] -= p_amount;
            return true;
        }
        return false;
    }

    public bool ResourceExists(int p_amount, ResourceType p_type)
    {
        return m_availableResources.ContainsKey(p_type) && m_availableResources[p_type] >= p_amount;
    }

    public int GetResourceRemaining(ResourceType p_type)
    {
        if (m_availableResources.ContainsKey(p_type))
        {
            return m_availableResources[p_type];
        }
        return 0;
    }

    private List<GameObject> GetAllResourcesInRange(Vector3 p_position,float p_range, ResourceType p_type)
    {
        List<GameObject> objects = new List<GameObject>();
        foreach (var item in m_allResources[p_type])
        {
            Vector3 objectPosition = item.transform.position;
            if (Vector3.Distance(objectPosition, p_position) <= p_range)
            {
                objects.Add(item);
            }
        }
        return objects;
    }
}
