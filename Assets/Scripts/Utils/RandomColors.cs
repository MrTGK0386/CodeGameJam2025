using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RandomColors : MonoBehaviour
{
    // Start is called before the first frame update
    
    public Color[] colors;
    private Color currentColor;
    void Start()
    {
        PopulateColorArray();
        GenerateColor();
    }
    
    public void GenerateColor()
    {
        int rand = Random.Range(0, colors.Length);
        Color newColor = colors[rand];
        currentColor = newColor;
        
        ApplyMaterial(newColor, 0);
    }

    void ApplyMaterial(Color color, int targetMaterialIndex)
    {
        Debug.Log(targetMaterialIndex);
        Material localMaterial = GetComponent<Renderer>().materials[targetMaterialIndex];
        Debug.Log(localMaterial.name);
        localMaterial.color = color; 
    }

    void PopulateColorArray()
    { 
        colors[0] = new Color(94f / 255f, 148f / 255f, 224f / 255f);
        colors[1] = new Color(243f / 255f, 155f / 255f, 186f / 255f);
        colors[2] = new Color(125f / 255f, 226f / 255f, 245f / 255f);
        colors[3] = new Color(255f / 255f, 203f / 255f, 134f / 255f);
        colors[4] = new Color(201f / 255f, 154f / 255f, 248f / 255f);
        colors[5] = new Color(169f / 255f, 227f / 255f, 90f / 255f);

    }
    
    
}
