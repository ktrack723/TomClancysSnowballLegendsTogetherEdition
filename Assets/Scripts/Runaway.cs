using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runaway : MonoBehaviour
{
    public Transform player;  // �÷��̾��� ��ġ
    public float speed = 3.0f;  // ������ �̵� �ӵ�
    public float detectionRange = 10.0f;  // �÷��̾ �ν��ϴ� ����
    public float wallDetectionRange = 5.0f;  // ���� �����ϴ� ����
    public float turnSpeed = 1.5f;  // ���� ��ȯ �ӵ�
    public float wallAvoidanceStrength = 10.0f;  // ���� ȸ���ϴ� ����

    private Vector3 currentDirection;  // ���� �̵� ����
    private bool isDetectWall = false;
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
        AvoidPlayer();
    }

    void FixedUpdate()
    {
        if (victim.IsCaught)
        {
            enabled = false;
            return;
        }

        // �� ����
        isDetectWall = DetectWalls();
        //if (isDetectWall == false)
        //{
        //    // �÷��̾� �ݴ� �������� �̵� ���� ������Ʈ
        //    Vector3 directionFromPlayer = (transform.position - player.position).normalized;
        //    //currentDirection = Vector3.Lerp(currentDirection, directionFromPlayer, Time.deltaTime * turnSpeed);
        //    currentDirection = directionFromPlayer;
        //}

        // Rigidbody�� ����� �̵�
        rb.MovePosition(rb.position + currentDirection * speed * Time.fixedDeltaTime);


        // �̵� �������� ȸ��
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

        // ���� �̵� �������� Raycast�� ��Ƽ� �� ����
        if (Physics.Raycast(transform.position, currentDirection, out hit, wallDetectionRange))
        {
            if (hit.collider.CompareTag("Wall"))
            {
                Debug.Log("sd");

                // ���� �������� �� ���� ���ϴ� �������� ȸ��
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