using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructurePlacement : MonoBehaviour {

    public Transform gridCenter;
    static public int gridHalfLength = 50;
    float cellWidth = 1.0f;
    
    int[,] grid = new int[gridHalfLength*2, gridHalfLength*2];

    bool placing = true;
    public Transform currentStructure;

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

        grid[0, 0] = 1;

        currentStructureGridSize.x = 4;
        currentStructureGridSize.y = 8;
    }
	
	// Update is called once per frame
	void Update ()
    {
        bool place = false;
        if (Input.GetKeyDown(KeyCode.E))
        {
            place = true;
        }

        if (placing)
        {
            Vector3 direction = transform.forward; // Get from camera

            Ray ray = new Ray(transform.position + direction*0.2f, direction);
            //Ray ray = new Ray(new Vector3(0, 5, 1), Vector3.down);
            RaycastHit hitInfo;
            if(Physics.Raycast(ray, out hitInfo, 1000.0f))
            {
                Vector3 hitPosition = hitInfo.point;

                Vector3 formatPos = (hitPosition - gridCenter.transform.position) / cellWidth;
                Vector2 cornerPos = new Vector2(Mathf.Floor(formatPos.x + 0.5f), Mathf.Floor(formatPos.z + 0.5f));

                cornerPos.x = Mathf.Max(0, cornerPos.x);
                cornerPos.y = Mathf.Max(0, cornerPos.y);

                currentStructure.position = new Vector3(cornerPos.x* cellWidth, hitPosition.y, cornerPos.y* cellWidth) + gridCenter.transform.position;

                // Check collision
                int cornerPosX = (int)cornerPos.x;
                int cornerPosY = (int)cornerPos.y;

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
                            continue;

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
                    Debug.Log("occopied");
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

                    Transform obj = Instantiate(currentStructure);
                    currentStructure = obj.transform;
                }

            }

            // Check collision

        }


	}

    void AttemptPlace()
    {

    }

    void StartPlacing()
    {
        placing = true;
    }
}
