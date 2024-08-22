using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runaway : MonoBehaviour
{
    public Transform player;  // �÷��̾��� ��ġ
    public float speed = 3.0f;  // ������ �̵� �ӵ�
    public float detectionRange = 10.0f;  // �÷��̾ �ν��ϴ� ����
    public float wallDetectionRange = 5.0f;  // ���� �����ϴ� ����
    public float turnSpeed = 5.0f;  // ���� ��ȯ �ӵ�

    private Vector3 currentDirection;  // ���� �̵� ����
    private bool isEscaping = false;
    private Rigidbody rb;
    private Victim victim;

    void Start()
    {
        player = BF_PlayerSnow.Instance.transform;

        victim = GetComponent<Victim>();

        // Rigidbody ������Ʈ�� ������
        rb = GetComponent<Rigidbody>();

        // Rigidbody�� �ӵ� ���� (�߷��� ���� �����Ƿ� Y������ �������� �ʵ��� ����)
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        // ������ �� ������ �������� ����
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

        // �÷��̾ ���� �ȿ� ���� �� �������� ����
        if (distanceToPlayer < detectionRange)
        {
            isEscaping = true;
        }

        // �÷��̾�κ��� �������� ���� ��
        if (isEscaping)
        {
            // �� ����
            DetectWalls();

            // �÷��̾� �ݴ� �������� �̵� ���� ������Ʈ
            Vector3 directionFromPlayer = (transform.position - player.position).normalized;
            currentDirection = Vector3.Lerp(currentDirection, directionFromPlayer, Time.deltaTime * turnSpeed);
        }
    }

    void FixedUpdate()
    {
        if (isEscaping)
        {
            // Rigidbody�� ����� �̵�
            rb.MovePosition(rb.position + currentDirection * speed * Time.fixedDeltaTime);

            // �̵� �������� ȸ��
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

        // �� ����(��, ��, ��, ��)���� Raycast�� ��Ƽ� ���� �ִ��� ����
        if (Physics.Raycast(transform.position, transform.forward, out hit, wallDetectionRange))
        {
            if (hit.collider.CompareTag("Wall"))
            {
                // ���� �����Ǹ� �÷��̾��� �ݴ� �������� ȸ��
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