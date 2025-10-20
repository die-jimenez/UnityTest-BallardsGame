using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    [Range(0f, 90f)]
    [SerializeField] float speed;

    void Start()
    {
        
    }

    void Update()
    {
        transform.Rotate(Vector3.up * speed * Time.deltaTime);  
    }
}
