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
        foreach (Renderer rend in obj.transform.GetComponentsInChildren<MeshRenderer>())
        {
            List<Material> mats = new List<Material>();

            for (int i = 0; i < rend.materials.Length; i++)
            {
                if (rend.materials[i].name == "TeamColorMat")
                {
                    switch (teamCol)
                    {
                        case Team.Teams.Blue:
                            mats.Add(teamColBlue);
                            break;
                        case Team.Teams.Red:
                            mats.Add(teamColBlue);
                            break;
                        default:
                            mats.Add(rend.materials[i]);
                            break;
                    }
                }
                else
                {
                    mats.Add(rend.materials[i]);
                }
            }

            rend.materials = mats.ToArray();
        }
    }
}
