using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    public Transform farBackground, backBackground, middleBackground, frontBackground;

    public float minHeight, maxHeight;

    //private float lastXPos;
    private Vector2 lastPos;

    // Start is called before the first frame update
    void Start()
    {
        //lastXPos = transform.position.x;
        lastPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);

        //Clamp (if needed) 
        //transform.position = new Vector3(target.position.x, Mathf.Clamp(target.position.y, minHeight, maxHeight), transform.position.z);

        //float amountToMoveX = transform.position.x - lastXPos;
        Vector2 amountToMove = new Vector2(transform.position.x - lastPos.x, transform.position.y - lastPos.y);

        farBackground.position = farBackground.position + new Vector3(amountToMove.x, amountToMove.y, 0f);
        backBackground.position += new Vector3(amountToMove.x, 0f) * -.3f;
        backBackground.position += new Vector3(amountToMove.y, 0f) * -.3f;
        middleBackground.position += new Vector3(amountToMove.x, 0f) * -.2f;
        middleBackground.position += new Vector3(amountToMove.y, 0f) * -.2f;
        frontBackground.position += new Vector3(amountToMove.x, 0f) * -.1f;
        frontBackground.position += new Vector3(amountToMove.y, 0f) * -.1f;

        //lastXPos = transform.position.x;
        lastPos = transform.position;
    }
}
