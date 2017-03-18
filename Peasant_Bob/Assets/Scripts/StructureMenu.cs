using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        Debug.Log("clicked");
    }

    void cancelFunc()
    {
        Debug.Log("cancel");
    }

    float timef()
    {
        return 0;
    }

	// Use this for initialization
	void Start () {
        canvasObj = GameObject.Find("Canvas");
        AddMenuItem(click, cancelFunc, timef);
        AddMenuItem(click, cancelFunc, timef);
        AddMenuItem(click, cancelFunc, timef);
        AddMenuItem(click, cancelFunc, timef);
        AddMenuItem(click, cancelFunc, timef);
        AddMenuItem(click, cancelFunc, timef);



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

        // Check input
        if(menuOpen)
        {
            CheckInput();
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

    void CheckInput()
    {
        float x = Input.mousePosition.x;
        float y = Input.mousePosition.y;


        Vector2 mousePoint = new Vector2(x, y);
        mousePoint -= new Vector2(Screen.width/2.0f, Screen.height/2.0f);
        //Debug.Log(mousePoint);

        if (openMenu.Count == 1)
        {
            float radius = openMenu[0].GetComponent<RectTransform>().rect.width/2.0f;
            if (mousePoint.magnitude < radius)
            {
                Debug.Log("inside");
            }
        }
        else
        {
            float degreesBetween = 360.0f / menuItems.Count;

            // 2 = 0, 3 = 0.333 ,4 = 0.5f, 8 = 0.75
            //float dotMax = 1.0f - 1.0f/ menuItems.Count;
            float dotMax = Mathf.Cos((degreesBetween/2.0f)*2*Mathf.PI/360.0f);
            Vector3 curVector = new Vector3(0, 1, 0);

            

            foreach (var item in openMenu)
            {
                float radius = item.GetComponent<RectTransform>().rect.width / 2.0f;
                if (mousePoint.magnitude > radius)
                {


                    float dotVal = Vector2.Dot(mousePoint.normalized, new Vector2(curVector.x, curVector.y));
                    Debug.Log(dotVal + " " + mousePoint.normalized + " " + curVector);

                    if(dotVal > dotMax)
                    {
                        Image image = item.GetComponent<Image>();
                        image.color = new Color(1, 0, 0);
                    }
                    else
                    {
                        Image image = item.GetComponent<Image>();
                        image.color = new Color(1, 1, 1);
                    }

                    curVector = Quaternion.Euler(0, 0, degreesBetween) * curVector;
                    curVector.Normalize();
                }
            }

        }

        foreach (var item in openMenu)
        {

            RectTransform rectTrans = item.GetComponent<RectTransform>();

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
