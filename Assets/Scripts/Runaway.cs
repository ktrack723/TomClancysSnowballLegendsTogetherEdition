using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runaway : MonoBehaviour
{
    public Transform player;  // 플레이어의 위치
    public float speed = 3.0f;  // 동물의 이동 속도
    public float detectionRange = 10.0f;  // 플레이어를 인식하는 범위
    public float wallDetectionRange = 5.0f;  // 벽을 감지하는 범위
    public float turnSpeed = 5.0f;  // 방향 전환 속도

    private Vector3 currentDirection;  // 현재 이동 방향
    private bool isEscaping = false;
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
        SetRandomDirection();
    }

    void Update()
    {
        if (victim.IsCaught)
        {
            enabled = false;
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // 플레이어가 범위 안에 있을 때 도망가기 시작
        if (distanceToPlayer < detectionRange)
        {
            isEscaping = true;
        }

        // 플레이어로부터 도망가는 중일 때
        if (isEscaping)
        {
            // 벽 감지
            DetectWalls();

            // 플레이어 반대 방향으로 이동 방향 업데이트
            Vector3 directionFromPlayer = (transform.position - player.position).normalized;
            currentDirection = Vector3.Lerp(currentDirection, directionFromPlayer, Time.deltaTime * turnSpeed);
        }
    }

    void FixedUpdate()
    {
        if (isEscaping)
        {
            // Rigidbody를 사용해 이동
            rb.MovePosition(rb.position + currentDirection * speed * Time.fixedDeltaTime);

            // 이동 방향으로 회전
            Quaternion targetRotation = Quaternion.LookRotation(currentDirection);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * turnSpeed));
        }
    }

    void SetRandomDirection()
    {
        currentDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
    }

    void DetectWalls()
    {
        RaycastHit hit;

        // 네 방향(앞, 뒤, 좌, 우)으로 Raycast를 쏘아서 벽이 있는지 감지
        if (Physics.Raycast(transform.position, transform.forward, out hit, wallDetectionRange))
        {
            if (hit.collider.CompareTag("Wall"))
            {
                // 벽이 감지되면 플레이어의 반대 방향으로 회전
                Vector3 directionFromWall = transform.position - hit.point;
                currentDirection = Vector3.Lerp(currentDirection, directionFromWall.normalized, Time.deltaTime * turnSpeed);
            }
        }
        else if (Physics.Raycast(transform.position, -transform.forward, out hit, wallDetectionRange))
        {
            if (hit.collider.CompareTag("Wall"))
            {
                Vector3 directionFromWall = transform.position - hit.point;
                currentDirection = Vector3.Lerp(currentDirection, directionFromWall.normalized, Time.deltaTime * turnSpeed);
            }
        }
        else if (Physics.Raycast(transform.position, transform.right, out hit, wallDetectionRange))
        {
            if (hit.collider.CompareTag("Wall"))
            {
                Vector3 directionFromWall = transform.position - hit.point;
                currentDirection = Vector3.Lerp(currentDirection, directionFromWall.normalized, Time.deltaTime * turnSpeed);
            }
        }
        else if (Physics.Raycast(transform.position, -transform.right, out hit, wallDetectionRange))
        {
            if (hit.collider.CompareTag("Wall"))
            {
                Vector3 directionFromWall = transform.position - hit.point;
                currentDirection = Vector3.Lerp(currentDirection, directionFromWall.normalized, Time.deltaTime * turnSpeed);
            }
        }
    }
}