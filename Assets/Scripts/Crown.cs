using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crown : MonoBehaviour
{

    private void FixedUpdate()
    {
        transform.eulerAngles += new Vector3(0, 0, -2f);
    }

}
