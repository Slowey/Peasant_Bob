using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTeamColor : MonoBehaviour {

    public Material teamColBluePub;
    public Material teamColRedPub;

    public static Material teamColBlue;
    public static Material teamColRed;

    void Awake()
    {
        teamColBlue = teamColBluePub;
        teamColRed = teamColRedPub;
    }

    public static void SetColorForObject(GameObject obj, Team.Teams teamCol)
    {
        foreach (Renderer rend in obj.transform.GetComponentsInChildren<Renderer>())
        {
            Material[] mats = new Material[rend.materials.Length];

            for (int i = 0; i < rend.materials.Length; i++)
            {
                if (rend.materials[i].name.CompareTo("TeamColorMat (Instance)") == 0)
                {
                    switch (teamCol)
                    {
                        case Team.Teams.Blue:
                            mats[i] = (teamColBlue);
                            break;
                        case Team.Teams.Red:
                            mats[i] = (teamColRed);
                            break;
                        default:
                            mats[i] = (rend.materials[i]);
                            break;
                    }
                }
                else
                {
                    mats[i] = (rend.materials[i]);
                }
            }

            rend.materials = mats;
        }
    }
}
