using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorChangeTestScript : MonoBehaviour
{
    public InputField[] inputFields;
    public Shader shader;
    public Material[] materials;

    public void updateColor(int index)
    {
        ColorUtility.TryParseHtmlString(inputFields[index].text, out Color color);
        Debug.Log(color);
        Renderer renderer = GetComponent<Renderer>();
        materials = renderer.materials;
        materials[index] = new Material(shader);
        materials[index].SetColor("Color_Base_Color", color);
        renderer.materials = materials;
    }
}
