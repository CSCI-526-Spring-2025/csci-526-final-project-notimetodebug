using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextDisappear : MonoBehaviour
{
    private TextMeshPro textMesh; 

    private void Start()
    {
        textMesh = GetComponent<TextMeshPro>();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Player"))
        {
            HideText();
        }
    }

    private void HideText()
    {
        if (textMesh != null)
        {
            textMesh.gameObject.SetActive(false); 
        }
    }
}
