using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MenuActionBase : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public abstract void ClickFunction();
    public abstract void CancelFunction();
    public abstract float PercentageDoneFunction();
    public abstract int NumberActiveFunction();
    public abstract bool AvaliableFunction();
}
