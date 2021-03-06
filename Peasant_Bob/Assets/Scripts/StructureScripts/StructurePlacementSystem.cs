﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructurePlacementSystem : MonoBehaviour {

    public Transform gridCenter;
    static public int gridHalfLength = 1000;
    float cellWidth = 1.0f;
    
    int[,] grid = new int[gridHalfLength*2, gridHalfLength*2];


    bool placing = false;
    GameObject currentStructurePrefab;
    GameObject currentStructure;
    public GameObject camera;

    public GameObject placeStructurePrefab;
    public GameObject buildingStructurePrefab;
    public GameObject[] constructionSites;

    public Material validMaterial;
    public Material invalidMaterial;

    struct StructureGridSize
    {
        public int x, y;
    }

    StructureGridSize currentStructureGridSize;

	// Use this for initialization
	void Start () {
        for (int y = 0; y < gridHalfLength*2; y++)
        {
            for (int x = 0; x < gridHalfLength*2; x++)
            {
                grid[x,y] = 0;
            }
        }
    }
	
	// Update is called once per frame
	public bool UpdateSystem ()
    {
        if (placing)
        {
            bool place = false;
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                place = true;
            }


            Vector3 direction;
            if (camera == null)
            {
                direction = transform.forward; // Get from camera
            }
            else
            {
                direction = camera.transform.forward;
            }


            Ray ray = new Ray(transform.position + direction*0.2f, direction);
            //Ray ray = new Ray(new Vector3(0, 5, 1), Vector3.down);
            RaycastHit hitInfo;
            if(Physics.Raycast(ray, out hitInfo, 1000.0f))
            {
                Vector3 hitPosition = hitInfo.point;
                Vector3 formatPos = (hitPosition - gridCenter.transform.position) / cellWidth;
                Vector2 cornerPos = new Vector2(Mathf.Floor(formatPos.x + 0.5f), Mathf.Floor(formatPos.z + 0.5f));

                //cornerPos.x = Mathf.Max(0, cornerPos.x);
                //cornerPos.y = Mathf.Max(0, cornerPos.y);

                currentStructure.transform.position = new Vector3(cornerPos.x* cellWidth, hitPosition.y, cornerPos.y* cellWidth) + gridCenter.transform.position;

                // Check collision
                int cornerPosX = (int)cornerPos.x + gridHalfLength;
                int cornerPosY = (int)cornerPos.y + gridHalfLength;

                bool occopied = false;

                for (int y = -(currentStructureGridSize.y/2); y < (currentStructureGridSize.y/2); y++)
                {
                    for (int x = -(currentStructureGridSize.x / 2); x < (currentStructureGridSize.x / 2); x++)
                    {
                        // If 2 in gridSize x, x = -1 => x = 1
                        // where if cornerPosX = 1,1, gridPos = 
                        int gridPosX = cornerPosX + x;
                        int gridPosY = cornerPosY + y;

                        if (gridPosX < 0 || gridPosY < 0)
                        {
                            //Debug.Log("outside1" + " " + gridPosX +" " + gridPosY);
                            occopied = true;
                            continue;
                        }
                        if (gridPosX > gridHalfLength*2 - 1 || gridPosY > gridHalfLength*2 - 1)
                        {
                            //Debug.Log("outside2" + " " + gridPosX + " " + gridPosY);
                            occopied = true;
                            continue;
                        }



                        //Debug.Log(gridPosX + " " +  gridPosY);

                        if (grid[gridPosX, gridPosY] == 1)
                        {
                            occopied = true;
                            break;
                        }
                    }
                }

                if (occopied == false)
                {
                    if(GameObject.Find("ResourceManager").GetComponent<ResourceManager>().ResourceExists(currentStructurePrefab.GetComponent<StructureInformation>().resourceCost, ResourceType.Wood) == false)
                    {
                        occopied = true;
                    }
                }


                if (occopied)
                {
                    foreach (Renderer rend in currentStructure.GetComponentsInChildren<Renderer>())
                    {

                        rend.enabled = true;
                        Material[] tempMatArr = new Material[rend.materials.Length];
                        for (int i = 0; i < tempMatArr.Length; i++)
                        {
                            tempMatArr[i] = invalidMaterial;
                        }

                        rend.materials = tempMatArr;

                    }
                }
                else
                {
                    foreach (Renderer rend in currentStructure.GetComponentsInChildren<Renderer>())
                    {

                        rend.enabled = true;
                        Material[] tempMatArr = new Material[rend.materials.Length];
                        for (int i = 0; i < tempMatArr.Length; i++)
                        {
                            tempMatArr[i] = validMaterial;
                        }

                        rend.materials = tempMatArr;

                    }

                }

                if (place && occopied == false && GameObject.Find("ResourceManager").GetComponent<ResourceManager>().ResourceExists(currentStructurePrefab.GetComponent<StructureInformation>().resourceCost, ResourceType.Wood))
                {
                    for (int y = -(currentStructureGridSize.y / 2); y < (currentStructureGridSize.y / 2); y++)
                    {
                        for (int x = -(currentStructureGridSize.x / 2); x < (currentStructureGridSize.x / 2); x++)
                        {
                            // If 2 in gridSize x, x = -1 => x = 1
                            // where if cornerPosX = 1,1, gridPos = 
                            int gridPosX = cornerPosX + x;
                            int gridPosY = cornerPosY + y;

                            if (gridPosX < 0 || gridPosY < 0)
                                continue;

                            //Debug.Log(gridPosX + " " +  gridPosY);

                            grid[gridPosX, gridPosY] = 1;

                        }
                    }

                    CreateBuildingStructure();


                    if (Input.GetKey(KeyCode.LeftShift) == false)
                    {
                        CancelPlace();
                    }
                }
            }
        }
        return false;
	}

    void CreateBuildingStructure()
    {
        GameObject obj = Instantiate(buildingStructurePrefab, currentStructure.transform.position, Quaternion.identity);
        obj.GetComponent<ConstructionAgentSystem>().m_finalBuildingPrefab = currentStructurePrefab;

        if (constructionSites.Length == 0)
            return;

        int numSitesX = currentStructureGridSize.x / 4;
        int numSitesY = currentStructureGridSize.y / 4;

        float offSetX = -cellWidth * 2 *(numSitesX - 1);
        float offSetY = -cellWidth * 2 * (numSitesY - 1);

        //offSetX = 0;
        //offSetY = 0;

        for (int y = 0; y < numSitesY; y++)
        {
            for (int x = 0; x < numSitesX; x++)
            {
                Quaternion rotation = Quaternion.identity * Quaternion.Euler(Vector3.up * 90 * Random.Range(0, 3));
                Vector3 position = obj.transform.position + new Vector3(offSetX + x * 4 * cellWidth, 0, offSetY + y * 4 * cellWidth);
                Instantiate(constructionSites[Random.Range(0, constructionSites.Length)], position, rotation, obj.transform);
            }
        }

        // Set health
        if(obj.GetComponent<Health>() != null)
        {
            Health health = obj.GetComponent<Health>();
            StructureInformation structInf = currentStructurePrefab.GetComponent<StructureInformation>();
            float sethealth = structInf.maxHealth;
            health.SetMaxHealth(sethealth);
            health.SetHealth(sethealth);
        }

        GameObject.Find("ResourceManager").GetComponent<ResourceManager>().TakeResource(currentStructurePrefab.GetComponent<StructureInformation>().resourceCost, ResourceType.Wood);

        //MeshFilter filter = obj.GetComponent<MeshFilter>();
        //MeshFilter filterObj = currentStructurePrefab.GetComponent<MeshFilter>();

        //Renderer renderer = obj.GetComponent<Renderer>();
        //Renderer rendererObj = currentStructurePrefab.GetComponent<Renderer>();

        //if (filter == null || filterObj == null)
        //    Debug.LogWarning("No filter on structure");

        //if (renderer == null || rendererObj == null)
        //    Debug.LogWarning("No renderer on structure");


        //filter.mesh = filterObj.sharedMesh;
        //renderer.materials = rendererObj.sharedMaterials;


    }

    public void CancelPlace()
    {
        placing = false;
        GameObject.Find("BuildingGUI").GetComponent<ShowThisMenu>().ShowThis(false);
        if (currentStructure)
        {
            Destroy(currentStructure);
        }
        currentStructure = null;
    }

    public void StartPlacing(GameObject obj)
    {
        // Cancel any previous
        CancelPlace();
        GameObject.Find("BuildingGUI").GetComponent<ShowThisMenu>().ShowThis(true);

        currentStructurePrefab = obj;
        currentStructure = Instantiate(placeStructurePrefab, transform.position, Quaternion.identity);

        // Get data
        int numChild = currentStructurePrefab.transform.childCount;
        for (int i = 0; i < numChild; i++)
        {
            Transform child = currentStructurePrefab.transform.GetChild(i);
            GameObject newChild = new GameObject("child");
            
            newChild.transform.SetParent(currentStructure.transform);
            newChild.transform.localPosition = child.transform.localPosition;
            newChild.transform.localRotation = child.transform.localRotation;

            MeshFilter filterObj = child.GetComponent<MeshFilter>();
            Renderer rendererObj = child.GetComponent<Renderer>();

            if (filterObj)
            {
                MeshFilter filter = newChild.AddComponent<MeshFilter>();
                Renderer renderer = newChild.AddComponent<MeshRenderer>();

                renderer.enabled = false;
                filter.mesh = filterObj.sharedMesh;
                renderer.materials = rendererObj.sharedMaterials;
            }
        }

        MeshFilter filterp = currentStructure.GetComponent<MeshFilter>();
        MeshFilter filterObjp = currentStructurePrefab.GetComponent<MeshFilter>();

        Renderer rendererp = currentStructure.GetComponent<Renderer>();
        Renderer rendererObjp = currentStructurePrefab.GetComponent<Renderer>();


        if (filterp != null && filterObjp != null && rendererp != null && rendererObjp != null)
        {
            rendererp.enabled = false;
            filterp.mesh = filterObjp.sharedMesh;
            rendererp.materials = rendererObjp.sharedMaterials;

        }


        StructureInformation info = obj.GetComponent<StructureInformation>();
        currentStructureGridSize.x = info.gridSizeX;
        currentStructureGridSize.y = info.gridSizeY;


        placing = true;
    }
}
