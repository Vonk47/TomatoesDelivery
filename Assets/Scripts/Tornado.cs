using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : MonoBehaviour
{

    float cooldown;
    [SerializeField] List<Transform> positions = new List<Transform>();
    Vector3 pos;
    [SerializeField] float tornadoSpeed;
    [SerializeField] float tomatoesSpeed=10f;
    [SerializeField] float carSpeed=5f;
    CarController car;
    bool activatedCar;


    private void OnEnable()
    {
        transform.localScale = new Vector3(1, 1, 1);
        cooldown = 0f;
        int random = Random.Range(0, positions.Count );
        transform.position = positions[random].position;
        ChoosePosition();

    }
   
    void Update()
    {
        cooldown += Time.deltaTime;
        if (transform.localScale.x < 3 && cooldown<5)
        {
            transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
        }

        if (cooldown > 2)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * tornadoSpeed);
            if (Vector3.Distance(transform.position, pos) < 1)
            {
                ChoosePosition();
            }
        }

        if (cooldown > 5)
        {
            transform.localScale -= new Vector3(0.05f, 0.05f, 0.05f);

            if (transform.localScale.x<1)
            gameObject.SetActive(false);
        }
    }

    void ChoosePosition()
    {
        int random = Random.Range(0, positions.Count);
        pos = positions[random].position;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
           other.gameObject.transform.position= Vector3.MoveTowards(other.gameObject.transform.position, transform.position, (Time.deltaTime * tomatoesSpeed * 10 )/ Vector3.Distance(other.gameObject.transform.position, transform.position));
            if (Vector3.Distance(other.gameObject.transform.position, transform.position) < 1)
            {
                other.gameObject.GetComponent<PropResource>().SpawnItem();
                Destroy(other.gameObject);
            }
        }
        if (other.gameObject.layer == 6)
        {
            other.gameObject.transform.position = Vector3.MoveTowards(other.gameObject.transform.position, transform.position, (Time.deltaTime * carSpeed * 10) / Vector3.Distance(other.gameObject.transform.position, transform.position));
            if (Vector3.Distance(other.gameObject.transform.position, transform.position) < 0.5f)
            {
                car.vacuumed = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6)
        {

           other.gameObject.GetComponent<CarController>().CancelDeliver();
            activatedCar = false;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            activatedCar = true;
            car = other.gameObject.GetComponent<CarController>();
            if(!car.isDelivering)
            other.gameObject.GetComponent<CarController>().TakePlanks(this.gameObject,true);

        }
    }

    private void OnDisable()
    {
        if (car != null && activatedCar)
        {
            activatedCar = false;
            car.CancelDeliver();
        }
        if (car != null && car.vacuumed == true)
        {
            car.EndOfVacuum();
        }
    }




}
