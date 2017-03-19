using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetColorOnStart : MonoBehaviour {

	// Use this for initialization
	void Start () {
        SetTeamColor.SetColorForObject(gameObject, GetComponent<Team>().team);
	}
}
