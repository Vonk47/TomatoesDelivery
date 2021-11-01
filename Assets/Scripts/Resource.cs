using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
   public  enum resourceState
    {
        isTomato,
        isMetal,
        none
    }
    [SerializeField] bool isEnemy;
    [SerializeField] Transform secondStack;
    public resourceState resState = resourceState.none;
    bool toTheBase;
    GameObject place;
    CarController car;
    Vector3 targetPos;
    Vector3 prePosition;
    static float delay=0.0001f;
    MeshRenderer mrenderer;
    public bool isVacuumed;


    private void OnEnable()
    {

       switch (resState)
        {
            case resourceState.isTomato:
                mrenderer = gameObject.GetComponent<MeshRenderer>();
                mrenderer.material.color = new Color(1, 1, 1);
                transform.localEulerAngles = new Vector3(90, 90, 0);
                prePosition = transform.localPosition;
                transform.localPosition = new Vector3(-0.002f, transform.localPosition.y + 0.02f, transform.localPosition.z);
                break;
        }


    }

    private void FixedUpdate()
    {
   
       if (!toTheBase)
        {

            transform.localPosition = Vector3.Lerp(transform.localPosition, prePosition, Time.deltaTime * 4f);
        }

        else
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * 120f);
            transform.localScale -= new Vector3( 0.0001f, 0.0001f, 0.0001f);
            if (Vector3.Distance(transform.position, targetPos) <1.5f)
            {
                if(resState==resourceState.isTomato)
                transform.SetParent(car.transform.GetChild(0).transform.GetChild(4).transform);
                else
                {
                    transform.SetParent(secondStack);
                }
                if(!isVacuumed)
                car.RecieveCallback(this);
                car.stackPointer--;
                resState = resourceState.none;
                gameObject.SetActive(false);
            }
        }
    }


    private void OnDisable()
    {
        if (resState == resourceState.isTomato)
            transform.localEulerAngles = new Vector3(90, 90, 0);
        else transform.localEulerAngles = new Vector3(0, 90, 90);
        transform.localPosition = prePosition;
        toTheBase = false;
  

    }

    public void MoveToTheBase(GameObject _base,CarController _car)
    {
       
        place = _base;
        car = _car;
        ToTheBase();
    }


    private void ToTheBase()
    {
        toTheBase = true;
        transform.SetParent(null);
        if (resState == resourceState.isMetal)
        {
            float _y = place.transform.position.y;
            _y += 0f;
            if (!isEnemy)
                targetPos = new Vector3(5.44000006f, 1.67999995f, 17.9099998f);
            else
            targetPos = new Vector3(place.transform.position.x, _y, place.transform.position.z);
           
        }
        else if (resState == resourceState.isTomato)
        {
            float _y = place.transform.position.y;
            _y += 0f;
            if (!isEnemy)
                targetPos = new Vector3(place.transform.position.x, _y, place.transform.position.z);
            else
                targetPos = new Vector3(place.transform.position.x, _y, place.transform.position.z);
        }
    }
    private void ResetDelay()
    {
        if(delay>0f)
        delay = 0.000f;
    }

}
