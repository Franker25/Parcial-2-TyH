using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWithDelay : MonoBehaviour
{
    
    void Start()
    {
        
        Invoke(nameof(DestroyObject), 5);
    }

    private void DestroyObject()
    {
        
        Destroy(gameObject);
    }
}
