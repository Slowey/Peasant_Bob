using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureMenu : MonoBehaviour {

    public delegate void ClickFunction();
    public delegate void CancelFunction();
    public delegate float TimeLeftFunction();



    struct MenuItem
    {
        public ClickFunction clicFunc;
        public CancelFunction cancFunc;
        public TimeLeftFunction timeFunc;
    }

    List<MenuItem> menuItems = new List<MenuItem>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /**
    Use case:
        The user clicks the button, which starts producing a peasant.
        Somewhere the system should pass the produce peasant function, and a Time left function

    **/

    void AddMenuItem(ClickFunction func, CancelFunction cancFunc, TimeLeftFunction timeFunc)
    {
        MenuItem newItem;
        newItem.clicFunc = func;
        newItem.cancFunc = cancFunc;
        newItem.timeFunc = timeFunc;

        menuItems.Add(newItem);

        CreateMenu();
    }

    void CreateMenu()
    {
        // Create circle for each


    }
}
