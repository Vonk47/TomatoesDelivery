using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoController : MonoBehaviour
{
    [SerializeField] Tornado tornado;
    [SerializeField] float torandoEnableTime = 7f;

    void Start()
    {
        InvokeRepeating("EnableTornado", 0.1f, torandoEnableTime);
    }

    private void EnableTornado()
    {
        tornado.gameObject.SetActive(true);
    }
    
}
