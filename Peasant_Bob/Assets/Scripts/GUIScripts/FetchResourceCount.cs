using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FetchResourceCount : MonoBehaviour {
    ResourceManager m_resourceManager;
    public ResourceType resourceToCheck;
	// Use this for initialization
	void Start () {
        m_resourceManager = GameObject.FindGameObjectWithTag("ResourceManager").GetComponent<ResourceManager>();
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<Text>().text = "" + m_resourceManager.GetResourceRemaining(ResourceType.Wood);
    }
}
