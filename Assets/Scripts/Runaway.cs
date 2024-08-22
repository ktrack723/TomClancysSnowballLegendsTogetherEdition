using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runaway : MonoBehaviour
{
    public Transform player;  // ?????????? ????
    public float speed = 3.0f;  // ?????? ???? ????
    public float detectionRange = 10.0f;  // ?????????? ???????? ????
    public float wallDetectionRange = 5.0f;  // ???? ???????? ????
    public float turnSpeed = 1.5f;  // ???? ???? ????
    public float wallAvoidanceStrength = 10.0f;  // ???? ???????? ????

    private Vector3 currentDirection;  // ???? ???? ????
    private bool isDetectWall = false;
    private Rigidbody rb;
    private Victim victim;

    public bool RandomJump;
    public float runawayInterval;

    private float runawayTimer;

    private int randomDirection = 1;

    void Start()
    {
        player = BF_PlayerSnow.Instance.transform;

        victim = GetComponent<Victim>();    

        // Rigidbody ?????????? ??????
        rb = GetComponent<Rigidbody>();

        // Rigidbody?? ???? ???? (?????? ???? ???????? Y?????? ???????? ?????? ????)

        // ?????? ?? ?????? ???????? ????
        AvoidPlayer();

        if (Random.Range(0f, 1f) > 0.5f)
        {
            randomDirection *= -1;
        }
    }

    void FixedUpdate()
    {
        if (victim.IsCaught)
        {
            enabled = false;
            return;
        }

        runawayTimer += Time.deltaTime;

        // ?? ????
        isDetectWall = DetectWalls();
        //if (isDetectWall == false)
        //{
        //    // ???????? ???? ???????? ???? ???? ????????
        //    Vector3 directionFromPlayer = (transform.position - player.position).normalized;
        //    //currentDirection = Vector3.Lerp(currentDirection, directionFromPlayer, Time.deltaTime * turnSpeed);
        //    currentDirection = directionFromPlayer;
        //}

        if (runawayTimer > runawayInterval)
        {
            AvoidPlayer();
            runawayTimer = 0;
        }

        // Rigidbody?? ?????? ????
        rb.MovePosition(rb.position + currentDirection * speed * Time.fixedDeltaTime);


        // ???? ???????? ????
        Quaternion targetRotation = Quaternion.LookRotation(currentDirection);
        transform.rotation = targetRotation;
    }

    void AvoidPlayer()
    {
        Vector3 directionFromPlayer = (transform.position - player.position);
        directionFromPlayer.y = 0;
        directionFromPlayer.Normalize();
        currentDirection = directionFromPlayer;
    }

    bool DetectWalls()
    {
        RaycastHit[] hits;

        // ???? ???? ???????? Raycast?? ?????? ?? ????
        hits = Physics.RaycastAll(transform.position, currentDirection, wallDetectionRange);

        foreach(RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Wall"))
            {
                Debug.Log("sd");

                // ???? ???????? ?? ???? ?????? ???????? ????
                Vector3 avoidanceDirection = Quaternion.Euler(0, 90 * randomDirection, 0) * currentDirection;
                //currentDirection = Vector3.Lerp(currentDirection, avoidanceDirection.normalized, Time.deltaTime * wallAvoidanceStrength);
                currentDirection = avoidanceDirection;

                return true;
            }
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + wallDetectionRange * currentDirection);
    }
}