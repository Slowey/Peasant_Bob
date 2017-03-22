using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeShader : MonoBehaviour {
    Shader standard;
    List<Material> allMaterials = new List<Material>();
    // Use this for initialization
    void Start () {
        standard = GetComponentInChildren<Renderer>().material.shader;
        Renderer[] allRenderers = GetComponentsInChildren<Renderer>();
        foreach (var item in allRenderers)
        {
            allMaterials.AddRange(item.materials);        
        }
    }
	
    public void OutlineObject()
    {
        foreach (var item in allMaterials)
        {
            item.shader = Shader.Find("Outlined/Diffuse");
        }
    }

    public void ReturnObjectToNormal()
    {
        foreach (var item in allMaterials)
        {
            item.shader = standard;
        }
    }
}
