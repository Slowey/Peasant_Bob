﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StructureMenu : MonoBehaviour {

    public delegate void ClickFunction();
    public delegate void CancelFunction();
    public delegate float PercentageDoneFunction();
    public delegate int NumberActiveFunction();
    public delegate bool AvaliableFunction();



    public GameObject guiItemPrefab;

    struct MenuItem
    {
        public ClickFunction clickFunc;
        public CancelFunction cancFunc;
        public PercentageDoneFunction percFunc;
        public NumberActiveFunction numFunc;
        public AvaliableFunction avilFunc;
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

    float perf()
    {
        return 0.5f;
    }

    bool avilFun()
    {
        return true;
    }
    bool avilFun2()
    {
        return false;
    }
    int numFun()
    {
        return 1;
    }

    // Use this for initialization
    void Start () {
        canvasObj = GameObject.Find("Canvas");
    }
	
	// Update is called once per frame
	void Update () {

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

    public void AddMenuItem(ClickFunction func, CancelFunction cancFunc, PercentageDoneFunction perFunc, NumberActiveFunction numFunc, AvaliableFunction avilFunc)
    {
        MenuItem newItem;
        newItem.clickFunc = func;
        newItem.cancFunc = cancFunc;
        newItem.percFunc = perFunc;
        newItem.numFunc = numFunc;
        newItem.avilFunc = avilFunc;

        menuItems.Add(newItem);
    }

    public void CreateMenu()
    {
        menuOpen = true;

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

    public void DestroyMenu()
    {
        menuOpen = false;
        foreach (var item in openMenu)
        {
            Destroy(item);
        }
        openMenu.Clear();
    }

    void CheckInput()
    {
        float x = Input.mousePosition.x;
        float y = Input.mousePosition.y;

        bool rightPressed = Input.GetKeyDown(KeyCode.Mouse1);
        bool leftPressed = Input.GetKeyDown(KeyCode.Mouse0);


        Vector2 mousePoint = new Vector2(x, y);
        mousePoint -= new Vector2(Screen.width / 2.0f, Screen.height / 2.0f);
        //Debug.Log(mousePoint);

        if (openMenu.Count == 1)
        {
            float radius = openMenu[0].GetComponent<RectTransform>().rect.width / 2.0f;
            if (mousePoint.magnitude < radius)
            {
                UpdateVisuals(0, true);
                if (leftPressed)
                {
                    menuItems[0].clickFunc();
                }
                else if (rightPressed)
                {
                    menuItems[0].cancFunc();
                }
            }
            else
            {
                UpdateVisuals(0, false);
            }
        }
        else
        {
            float degreesBetween = 360.0f / menuItems.Count;

            // 2 = 0, 3 = 0.333 ,4 = 0.5f, 8 = 0.75
            //float dotMax = 1.0f - 1.0f/ menuItems.Count;
            float dotMax = Mathf.Cos((degreesBetween / 2.0f) * 2 * Mathf.PI / 360.0f);
            Vector3 curVector = new Vector3(0, 1, 0);


            float radius = openMenu[0].GetComponent<RectTransform>().rect.width / 2.0f;

            for (int i = 0; i < menuItems.Count; i++)
            {
                float dotVal = Vector2.Dot(mousePoint.normalized, new Vector2(curVector.x, curVector.y));

                if (mousePoint.magnitude > radius)
                {
                    if (dotVal > dotMax)
                    {

                        UpdateVisuals(i, true);

                        if (leftPressed)
                        {
                            menuItems[i].clickFunc();
                        }
                        else if (rightPressed)
                        {
                            menuItems[i].cancFunc();
                        }
                    }
                    else
                    {
                        UpdateVisuals(i, false);
                    }
                }
                else
                {
                    UpdateVisuals(i, false);

                }

                curVector = Quaternion.Euler(0, 0, degreesBetween) * curVector;
                curVector.Normalize();
            }
        }

    }
    

    void UpdateVisuals(int i, bool inside)
    {
        RectTransform rectTrans = openMenu[i].GetComponent<RectTransform>();
        Image image = openMenu[i].GetComponent<Image>();

        if (menuItems[i].avilFunc())
        {
            if(inside)
            {

                image.color = new Color(1, 1, 1);
            }
            else
            {
                image.color = new Color(0.7f, 0.7f, 0.7f);
            }
        }
        else
        {
            image.color = new Color(0.25f, 0.25f, 0.25f);
        }

        float percDone = menuItems[i].percFunc();
        int numActive = menuItems[i].numFunc();

        Text text1 = openMenu[i].transform.GetChild(0).GetComponent<Text>();
        Text text2 = openMenu[i].transform.GetChild(1).GetComponent<Text>();

        if (percDone == 0.0f)
        {
            text1.text = "";
        }
        else
        {
            text1.text = ((int)(percDone*100)).ToString() + "%";
        }

        if (numActive == 0)
        {
            text2.text = "";
        }
        else
        {
            text2.text = numActive.ToString();
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
