using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Slider Tomato;
    int TOMATO;
  
  
    private void FixedUpdate()
    {
        Tomato.value = TOMATO;
    }

    public void AddTomato()
    {

        TOMATO++;
        
    }
}
