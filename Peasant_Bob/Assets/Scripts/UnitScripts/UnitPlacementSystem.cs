using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPlacementSystem : MonoBehaviour {
    public GameObject UnitToPlace = null;
    public Camera camera;
    public GameObject pointer;
    private Vector3 placePosition;
	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    public void UpdateSystem()
    {
        if (UnitToPlace == null)
        {
            return;
        }
        pointer.GetComponent<MeshRenderer>().enabled = true;
        Vector3 direction;
        if (camera == null)
        {
            direction = transform.forward; // Get from camera
        }
        else
        {
            direction = camera.transform.forward;
        }

        Ray ray = new Ray(transform.position + direction * 0.2f, direction);
        //Ray ray = new Ray(new Vector3(0, 5, 1), Vector3.down);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 1000.0f))
        {
            placePosition = hitInfo.point;
            pointer.transform.position = new Vector3(placePosition.x, placePosition.y + 2, placePosition.z);
        }
    }

    public void PlaceUnit()
    {
        if (UnitToPlace != null)
        {
            UnitToPlace.GetComponent<NavAgent>().SetDestination(placePosition, AgentsManager.AgentStates.Fighting);
        }
        UnitToPlace = null;
        pointer.GetComponent<MeshRenderer>().enabled = false;
    }
}
