using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCheckScript : MonoBehaviour {

    public GameObject camera;

    StructureMenu activeMenu;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
        if(Input.GetKeyDown(KeyCode.E))
        {
            CheckRayCastHit();
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (activeMenu != null)
            {
                activeMenu.DestroyMenu();
                activeMenu = null;
            }
        }

	}

    void CheckRayCastHit()
    {
        // Cast ray
        Vector3 direction;
        if (camera == null)
        {
            direction = transform.forward;
        }
        else
        {
            direction = camera.transform.forward;
        }

        Ray newRay = new Ray();
        newRay.origin = transform.position;
        newRay.direction = direction;
        RaycastHit hitInfo;

        if(Physics.Raycast(newRay, out hitInfo, 50, LayerMask.GetMask("Buildings", "Units")))
        {
            StructureMenu menu = hitInfo.transform.GetComponent<StructureMenu>();
            if (menu != null)
            {
                activeMenu = menu;
                activeMenu.CreateMenu();
            }
            // Else if on unti menu
        }
    }
}
