using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RandomColors : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GenerateColor();
    }
    
    public void GenerateColor()
    {
        Color newColor = Random.ColorHSV();
        
        Debug.Log(newColor.ToString());
        
        ApplyMaterial(newColor, 0);
    }

    void ApplyMaterial(Color color, int targetMaterialIndex)
    {
        Debug.Log(targetMaterialIndex);
        Material localMaterial = GetComponent<Renderer>().materials[targetMaterialIndex];
        Debug.Log(localMaterial.name);
        localMaterial.color = color; 
    }
}
