using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colorPicker : MonoBehaviour
{
    //[SerializeField] GameObject furniture;
    FlexibleColorPicker fcp;
    public Material newmat;
    [SerializeField] private ModelManager modelManager;
    //[SerializeField] private Material[] mats;

    private void Start()    
    {
        modelManager = GameObject.Find("ModelManager").GetComponent<ModelManager>();
        //mats = furniture.GetComponent<MeshRenderer>().materials;
        //newmat = mats[0];
        newmat = new Material(Shader.Find("Transparent/Diffuse"));
    }
    // Update is called once per frame
    void Update()
    {
        if (modelManager.selectedModel != null)
        {
            fcp = GameObject.FindWithTag("colorPicker").GetComponent<FlexibleColorPicker>();
            GameObject furniture = modelManager.selectedModel;
            var mats = furniture.GetComponent<MeshRenderer>().materials;

            if (furniture.name == "bed1" || furniture.name == "bed1(Clone)")
            {
                newmat = mats[2];
            }
            else if(furniture.name == "chair" || furniture.name == "chair(Clone)")
            {
                newmat = mats[1];
                //Debug.Log("get chair");
            }
            else
            {
                newmat = mats[1];
            }
            newmat.color = fcp.color;
        }
    }
}
