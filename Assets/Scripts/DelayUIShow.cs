using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayUIShow : MonoBehaviour
{
    [SerializeField] List<GameObject> gameObjects = new List<GameObject>();

    private void OnEnable()
    {
        Invoke("Delayed", 0.5f);
    }

    private void Delayed()
    {
        foreach(GameObject gameObject in gameObjects)
        {
            gameObject.SetActive(true);
        }
    }
}
