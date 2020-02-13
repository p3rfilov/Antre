using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourChanger : MonoBehaviour
{
    public Color targetColor;

    private Color sourceColor;
    private Renderer rend;

    private void Awake ()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
        {
            sourceColor = rend.material.color;
        }
    }

    private void OnMouseDown ()
    {
        if (rend != null)
        {
            if (rend.material.color != targetColor)
            {
                SetColour(targetColor);
            }
            else
            {
                SetColour(sourceColor);
            }
        }
    }

    private void SetColour (Color colour)
    {
        rend.material.color = colour;
        rend.material.SetColor("_EmissionColor", colour);
    }
}
