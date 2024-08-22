using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runaway : MonoBehaviour
{
    public Transform player;  // 플레이어의 위치
    public float speed = 3.0f;  // 동물의 이동 속도
    public float detectionRange = 10.0f;  // 플레이어를 인식하는 범위
    public float wallDetectionRange = 5.0f;  // 벽을 감지하는 범위
    public float turnSpeed = 1.5f;  // 방향 전환 속도
    public float wallAvoidanceStrength = 10.0f;  // 벽을 회피하는 정도

    private Vector3 currentDirection;  // 현재 이동 방향
    private bool isDetectWall = false;
    private Rigidbody rb;
    private Victim victim;

    void Start()
    {
        player = BF_PlayerSnow.Instance.transform;

        victim = GetComponent<Victim>();    

        // Rigidbody 컴포넌트를 가져옴
        rb = GetComponent<Rigidbody>();

        // Rigidbody의 속도 조절 (중력은 켜져 있으므로 Y축으로 떨어지지 않도록 설정)
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        // 시작할 때 랜덤한 방향으로 설정
        AvoidPlayer();
    }

    void FixedUpdate()
    {
        if (victim.IsCaught)
        {
            enabled = false;
            return;
        }

        // 벽 감지
        isDetectWall = DetectWalls();
        //if (isDetectWall == false)
        //{
        //    // 플레이어 반대 방향으로 이동 방향 업데이트
        //    Vector3 directionFromPlayer = (transform.position - player.position).normalized;
        //    //currentDirection = Vector3.Lerp(currentDirection, directionFromPlayer, Time.deltaTime * turnSpeed);
        //    currentDirection = directionFromPlayer;
        //}

        // Rigidbody를 사용해 이동
        rb.MovePosition(rb.position + currentDirection * speed * Time.fixedDeltaTime);


        // 이동 방향으로 회전
        //Quaternion targetRotation = Quaternion.LookRotation(currentDirection);
        //rb.MoveRotation(targetRotation);
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
        RaycastHit hit;

        // 현재 이동 방향으로 Raycast를 쏘아서 벽 감지
        if (Physics.Raycast(transform.position, currentDirection, out hit, wallDetectionRange))
        {
            if (hit.collider.CompareTag("Wall"))
            {
                Debug.Log("sd");

                // 벽을 감지했을 때 벽을 피하는 방향으로 회전
                Vector3 avoidanceDirection = Quaternion.Euler(0, 90, 0) * currentDirection;
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