using UnityEngine;
using System.Collections;

public class CamMove : MonoBehaviour
{
    public GameObject Player;
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private float cameraSpeed = 1.5f;
    [SerializeField]
    public Vector3 offset;
    float cameraY;
    float rotationSpeed = 3f;
    public float rotateValue=0f;
    [SerializeField] GameObject playerBase;
    public bool isOrbital=true;
    Vector3 offsetScaler;

    GameObject startPlayer;
    bool won;

    public void Start()
    {

        if (offset==null)
        offset = transform.position - Player.transform.position;
        cameraY = Player.transform.position.y+offset.y;
        startPlayer = Player;
        offsetScaler = new Vector3(0, 0, 0);
    }

    private void FixedUpdate()
    {
        if (Player)
        {


            Vector3 target;
            target = new Vector3((Player.transform.position.x+offset.x) , (Player.transform.position.y + offset.y) * (1+offsetScaler.y), (Player.transform.position.z + offset.z)*(1+offsetScaler.z));
            Vector3 currentPosition = Vector3.Lerp(transform.position, target, cameraSpeed/2 * Time.deltaTime);
            cam.transform.position = currentPosition;
            if(isOrbital)
            transform.LookAt(Player.transform);



        }
    }
    public void CameraOn()
    {
        offset = new Vector3(0, 10, -9);
        offsetScaler = new Vector3(0, 0, 0);
        Invoke("SetFirstPlayer", 2f);
        won = true;
    }

    public void CameraOnEnemy()
    {
        offset = new Vector3(0, 10, -9);
        offsetScaler = new Vector3(0, 0, 0);
        Invoke("SetFirstPlayer", 2f);
        won = true;
    }


    public void SetPlayer(GameObject _player)
    {
        Player = _player;
    }

    private void SetFirstPlayer()
    {
        Player = startPlayer;
    }
}
