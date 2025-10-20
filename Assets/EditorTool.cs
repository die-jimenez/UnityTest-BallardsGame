using System;
using System.Collections;
using System.Collections.Generic;
using EasyButtons;
using UnityEngine;

public class EditorTool : MonoBehaviour
{
    [SerializeField] MeshRenderer cubeRenderer;
    [SerializeField] Color newColor;


    [Button]
    public void ChangeColor()
    {
        if (cubeRenderer == null) return;
        cubeRenderer.material.color = newColor;
    }

    public void ChangeColor(Color _color)
    {
        if (cubeRenderer == null) return;
        newColor = _color;
        cubeRenderer.material.color = newColor;
    }
}
