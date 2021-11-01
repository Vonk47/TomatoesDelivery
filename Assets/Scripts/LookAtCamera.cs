using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] Transform cam;

    private void LateUpdate()
    {
        transform.LookAt(cam);
        transform.Rotate(0, 180, 0);
    }

}
