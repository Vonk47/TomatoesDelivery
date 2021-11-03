using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{

    [SerializeField] float carSpeed = 1f;
    [SerializeField] float rotationSpeed = 1f;
    [SerializeField] float acceleration = 2f;
    [SerializeField] List<Resource> Stack = new List<Resource>();
    [SerializeField] List<Transform> wheels = new List<Transform>();
    [SerializeField] List<GameObject> dust = new List<GameObject>();
    public float stackSize = 56;
    public int stackPointer = 0;
    int count = 0;

    [SerializeField] List<Vector3> newPaths = new List<Vector3>();
    [SerializeField] List<Resource> FalseStack = new List<Resource>();
    public Action onPlankGive;
    public Action<int> onMetalGive;

    [SerializeField] bool isEnemy;
    [SerializeField] GameObject StackObj;

    [SerializeField] UIManager uiManager;

    bool delay = true;
    bool checkInvoke;
    bool tornadoDelivering;
    public  bool isDelivering;
    public bool vacuumed;

    bool doShake;
    float shakeCounter;

    public  bool isDED;
    bool wactive;


    [SerializeField] GameObject panel;

    GameObject currentBase;

    void Start()
    {

        checkInvoke = true;
        Invoke("ResetDelay", 1f);
        for(int i = 0; i < Stack.Capacity; i++)
        {
            Stack[i] = StackObj.transform.GetChild(i).gameObject.GetComponent<Resource>();
        }
    }

    private void ResetDelay()
    {
        delay = false;
    }

    void Update()
    {
        if (!isEnemy)    //if player controlls the car
        {
            if (Input.GetKey(KeyCode.W))
            {
                if (checkInvoke == false)
                {
                    CancelInvoke("DisableMovement");
                    checkInvoke = true;
                }

                wactive = true;
                carSpeed = Mathf.Lerp(carSpeed, 17, Time.deltaTime* acceleration);

                foreach (Transform wheel in wheels)
                {
                    wheel.eulerAngles += new Vector3(0f, 0, 0);
                }
                foreach(GameObject obj in dust)
                {
                    obj.SetActive(true);
                    ParticleSystem pS = obj.GetComponent<ParticleSystem>();
                    pS.startLifetime = Mathf.Lerp(pS.startLifetime, 0.6f, Time.deltaTime);
                    
                }   
               
            }
            else   
            {
                if (wactive && checkInvoke)
                {
                    Invoke("DisableMovement", 1f);
                    checkInvoke = false;
                }
                carSpeed = Mathf.Lerp(carSpeed, 7, Time.deltaTime* acceleration);
                foreach (GameObject obj in dust)
                {
                    ParticleSystem pS = obj.GetComponent<ParticleSystem>();
                    pS.startLifetime =Mathf.Lerp(pS.startLifetime,0.1f,Time.deltaTime*5f);
                    if (pS.startLifetime < 0.15f)
                    {
                        obj.SetActive(false);
                    }
                  
                }
            }
        }
        else // if bot controlls the car
        {
           
            if (!delay && count != 10)
            {
                Vector3 targetDirection = (newPaths[count] - transform.position).normalized;
                Quaternion _lookRotation = Quaternion.LookRotation(targetDirection);
                Quaternion LookAtRotationOnly_Y = Quaternion.Euler(_lookRotation.eulerAngles.x, _lookRotation.eulerAngles.y, _lookRotation.eulerAngles.z);
                transform.rotation = Quaternion.Slerp(transform.rotation, LookAtRotationOnly_Y, Time.deltaTime * carSpeed / 2);

            }
        }
        if (isDED)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 180);
        }


    }

    private void FixedUpdate()
    {   if (wactive)   // rotating - left right
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward * 3, Time.deltaTime * carSpeed);
            if (Input.GetKey(KeyCode.A))
            {
                Vector3 _targetRotation = Vector3.zero;
                _targetRotation.y = transform.eulerAngles.y - 2f;
                transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.right, Time.deltaTime * carSpeed / 5f);
                transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, _targetRotation, Time.deltaTime * rotationSpeed);
                carSpeed = Mathf.Lerp(carSpeed, 5, Time.deltaTime);
            }
            else
               if (Input.GetKey(KeyCode.D))
            {
                Vector3 _targetRotation = Vector3.zero;
                _targetRotation.y = transform.eulerAngles.y + 2f;

                transform.position = Vector3.MoveTowards(transform.position, transform.position + -transform.right, Time.deltaTime * carSpeed / 5f);
                transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, _targetRotation, Time.deltaTime * rotationSpeed);
                carSpeed = Mathf.Lerp(carSpeed, 5, Time.deltaTime);
            }
        }

         if(vacuumed)   // getting vacuumed by tornado
        {
            wactive = false;
            Vector3 _targetRotation = Vector3.zero;
            _targetRotation.y = transform.eulerAngles.y - 5f;
           // _targetRotation.x = 179f;
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, _targetRotation, Time.deltaTime * rotationSpeed);
           
        }

        if (isEnemy && !delay && count != 10)   
        {
            if (Vector3.Distance(transform.position, newPaths[count]) < 3f)
            {
                count++;
                if (count > 4)
                {
                    Invoke("ResetCount", 2f);
                    count = 10;

                }

            }
        }
        if (doShake)   // shaking car animation
        {
            shakeCounter += Time.deltaTime;

            if (shakeCounter >= 0.16)
            {
                transform.localScale = new Vector3(120, 120, 120);
                doShake = false;
                shakeCounter = 0;
            }

            if (shakeCounter >= 0.08)
            {
                transform.localScale += new Vector3(0, 5f, 0);
            }
            else
            {
                transform.localScale -= new Vector3(0, 5f, 0);
            }

        }
    }

    private void ResetCount()
    {
        count = 0;
    }

    public void EndOfVacuum()
    {
        transform.GetChild(0).eulerAngles = new Vector3(180, transform.GetChild(0).eulerAngles.y, transform.GetChild(0).eulerAngles.z);
        Invoke("Lose",1.4f);
    }

    void Lose()
    {
        vacuumed = false;
        panel.SetActive(true);
    }

    public void AddResource(PropResource _resource)
    {
        if (stackPointer < stackSize && !isDelivering)
        {  
            if (_resource.resState == PropResource.resourceState.isTomato )
            {
                Stack[stackPointer].resState = Resource.resourceState.isTomato;
                Stack[stackPointer].gameObject.SetActive(true);
                stackPointer++;

            }
           
        }

    }
    void DisableMovement()
    {
        wactive = false;
    }

    public void TakePlanks(GameObject _base,bool isTornado)
    {
        if (isTornado)
            tornadoDelivering = true;
        else tornadoDelivering = false;

        FalseStack = Stack;
        currentBase = _base;
        InvokeRepeating("LoopTakePlanks", 0.01f, 0.02f);
        isDelivering = true;
    }

    void LoopTakePlanks()
    {
        if (stackPointer == 0)
        {
            CancelInvoke("LoopTakePlanks");
            isDelivering = false;
        }
        else
        {
            if (FalseStack[stackPointer - 1].gameObject.activeInHierarchy)
            {
                if(tornadoDelivering)
                FalseStack[stackPointer-1].isVacuumed=true;
                else FalseStack[stackPointer - 1].isVacuumed = false;

                FalseStack[stackPointer - 1].MoveToTheBase(currentBase, this);
            }
        }
        
    }

    public void CancelDeliver()
    {
        CancelInvoke("LoopTakePlanks");
        isDelivering = false;
    }

    private void ReverseStack()
    {
        FalseStack.Reverse();
    }

    public void RecieveCallback(Resource res)
    {
        uiManager.AddTomato();
    }

    void RecalculateStack()
    {
        PropResource.currentCounter = 0;
    }
    public void DoShake()
    {
       // doShake = true;
        shakeCounter = 0;
    }

    public void DoBoom()
    {
        StackObj.SetActive(false);
        isDED = true;
        isEnemy = false;
        transform.eulerAngles = new Vector3 (transform.eulerAngles.x, transform.eulerAngles.y, 180);
    }
}
