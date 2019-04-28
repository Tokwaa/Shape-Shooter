using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCLS : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if (Cursor.lockState != CursorLockMode.Confined)
            Cursor.lockState = CursorLockMode.Confined;
    }
}
