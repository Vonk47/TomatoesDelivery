using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropResource : MonoBehaviour
{
    CarController car;
    bool doMove=false;
    public static  int currentCounter;
    Vector3 scale;

    public  enum resourceState
    {
        isTomato,
        isMetal
    }
    public resourceState resState;

    private void Start()
    {
        currentCounter = 0;
        scale = transform.localScale;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            currentCounter++;
            car = other.gameObject.GetComponent<CarController>();
            if (car.stackPointer < car.stackSize && currentCounter<28 && !car.isDelivering)
            {
                car.DoShake();
                doMove = true;
            }
     
        }
    }

    private void FixedUpdate()
    {
        if (car != null && car.stackPointer<car.stackSize && doMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, car.transform.position, Time.deltaTime*19f);
            transform.localScale -= new Vector3(0.01f, 0.01f,0.01f);
            if (Vector3.Distance(transform.position, car.transform.position) < 1f)
            {
               

                car.AddResource(this);
                if (car.stackPointer <= car.stackSize)
                {
                    SpawnItem();
                    Destroy(gameObject);
                }
            }

        }
        else
        {
            doMove = false;
            transform.localScale = Vector3.Lerp(transform.localScale, scale, Time.deltaTime * 2f);
        }
    }

    public void SpawnItem()
    {
        Instantiate(this, new Vector3(transform.position.x, transform.position.y + 100, transform.position.z), transform.rotation);
    }



}
