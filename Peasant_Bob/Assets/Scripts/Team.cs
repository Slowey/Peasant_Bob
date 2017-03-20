using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour {

    public enum Teams
    {
        Blue,
        Red,
        NumberOfTeams,
    }

    public Teams team  = Teams.Blue;
}
