using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Base : MonoBehaviour
{


    [SerializeField] CarController car;
    [SerializeField] Slider HP;
    [SerializeField] bool isEnemy;
    [SerializeField] UIManager uIManager;
    [SerializeField] CamMove camMove;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            if(!car.isDelivering)
            car.TakePlanks(this.gameObject,false);
          
          
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            car.CancelDeliver();
        }
    }



}
