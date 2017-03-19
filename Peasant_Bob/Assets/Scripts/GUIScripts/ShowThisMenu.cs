using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowThisMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowThis(bool p_show)
    {
        foreach (var item in gameObject.GetComponentsInChildren<Image>())
        {
            item.enabled = p_show;
        }
        
    }
}
