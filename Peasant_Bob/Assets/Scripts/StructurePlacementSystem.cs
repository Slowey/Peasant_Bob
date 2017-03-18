using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructurePlacementSystem : MonoBehaviour {

    public Transform gridCenter;
    static public int gridHalfLength = 50;
    float cellWidth = 1.0f;
    
    int[,] grid = new int[gridHalfLength*2, gridHalfLength*2];


    bool placing = false;
    GameObject currentStructurePrefab;
    GameObject currentStructure;
    public GameObject camera;

    public GameObject placeStructurePrefab;
    public GameObject buildingStructurePrefab;

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
	void Update ()
    {
        if (placing)
        {
            bool place = false;
            if (Input.GetKeyDown(KeyCode.E))
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
                            Debug.Log("outside1" + " " + gridPosX +" " + gridPosY);
                            occopied = true;
                            continue;
                        }
                        if (gridPosX > gridHalfLength*2 - 1 || gridPosY > gridHalfLength*2 - 1)
                        {
                            Debug.Log("outside2" + " " + gridPosX + " " + gridPosY);
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

                if (occopied)
                {
                    currentStructure.GetComponent<Renderer>().material = invalidMaterial;
                }
                else
                {
                    currentStructure.GetComponent<Renderer>().material = validMaterial;
                }

                if (place && occopied == false)
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
                    placing = false;
                    Destroy(currentStructure);
                    currentStructure = null;

                }
            }
        }
	}

    void CreateBuildingStructure()
    {
        GameObject obj = Instantiate(buildingStructurePrefab, currentStructure.transform.position, Quaternion.identity);

        MeshFilter filter = obj.GetComponent<MeshFilter>();
        MeshFilter filterObj = currentStructurePrefab.GetComponent<MeshFilter>();
        if (filter == null || filterObj == null)
            Debug.LogWarning("No filter on structure");

        filter.mesh = filterObj.sharedMesh;
    }

    public void CancelPlace()
    {
        if (currentStructure)
        {
            Destroy(currentStructure);
        }
    }

    public void StartPlacing(GameObject obj)
    {
        // Cancel any previous
        CancelPlace();

        currentStructurePrefab = obj;
        currentStructure = Instantiate(placeStructurePrefab, transform.position, Quaternion.identity);

        // Get data
        MeshFilter filter = currentStructure.GetComponent<MeshFilter>();
        MeshFilter filterObj = currentStructurePrefab.GetComponent<MeshFilter>();
        if (filter == null || filterObj == null)
            Debug.LogWarning("No filter on structure");

        filter.mesh = filterObj.sharedMesh;


        StructureInformation info = obj.GetComponent<StructureInformation>();
        currentStructureGridSize.x = info.gridSizeX;
        currentStructureGridSize.y = info.gridSizeY;


        placing = true;
    }
}
