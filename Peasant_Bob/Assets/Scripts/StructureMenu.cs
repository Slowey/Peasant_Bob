using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureMenu : MonoBehaviour {

    public delegate void ClickFunction();
    public delegate void CancelFunction();
    public delegate float TimeLeftFunction();

    public GameObject guiItemPrefab;

    struct MenuItem
    {
        public ClickFunction clicFunc;
        public CancelFunction cancFunc;
        public TimeLeftFunction timeFunc;
    }

    List<MenuItem> menuItems = new List<MenuItem>();
    List<GameObject> openMenu = new List<GameObject>();

    bool menuOpen = false;
    GameObject canvasObj;
    void click()
    {

    }

    float timef()
    {
        return 0;
    }

	// Use this for initialization
	void Start () {
        canvasObj = GameObject.Find("Canvas");
        AddMenuItem(click, click, timef);
        AddMenuItem(click, click, timef);


    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.E) && menuOpen == false)
        {
            CreateMenu();
            menuOpen = true;
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && menuOpen == true)
        {
            menuOpen = false;
            foreach (var item in openMenu)
            {
                Destroy(item);
            }
            openMenu.Clear();
        }
		
        //if(menuOpen)
        //{
        //    // Get direction to building from camera
        //    Vector3 playerPos = new Vector3(0, 4, 0);
        //    Vector3 direction = transform.position - playerPos;
        //    direction.Normalize();

        //    UpdateMenu();
        //}

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
    }

    void CreateMenu()
    {
        // Create circle for each
        if (menuItems.Count == 0)
        {
            return;
        }
        else if(menuItems.Count == 1)
        {
            // Create one circle in middle
            GameObject obj = Instantiate(guiItemPrefab, canvasObj.transform);
            RectTransform rectTrans = obj.GetComponent<RectTransform>();
            rectTrans.localPosition = new Vector3(0, 0, 0);
            openMenu.Add(obj);
        }
        else
        {
            float degreesBetween = 360.0f/menuItems.Count;
            Vector3 startPos = new Vector3(0, 80, 0);

            for (int i = 0; i < menuItems.Count; i++)
            {
                GameObject obj = Instantiate(guiItemPrefab, canvasObj.transform);
                RectTransform rectTrans = obj.GetComponent<RectTransform>();
                rectTrans.localPosition = startPos;
                openMenu.Add(obj);

                startPos = Quaternion.Euler(0, 0, degreesBetween) * startPos;
            }
        }

    }

    //void UpdateMenu()
    //{
    //    // Create circle for each
    //    if (menuItems.Count == 0)
    //    {
    //        return;
    //    }
    //    else if (menuItems.Count == 1)
    //    {
    //        // Create one circle in middle
    //        RectTransform rectTrans = openMenu[0].GetComponent<RectTransform>();
    //        rectTrans.localPosition = new Vector3(0, 0, 0);
    //    }
    //    else
    //    {

    //    }
    //}
}
