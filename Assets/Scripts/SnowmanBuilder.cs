using AutoLetterbox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class SnowmanBuilder : MonoBehaviour
{
    public static SnowmanBuilder Instance;

    public GameObject PlayerPrefab;
    public GameObject PlayerSpawnEffect;
    public Transform PlayerSpawnParent;

    public GameObject MainCamera;
    public GameObject BuilderCamera;

    public Transform SnowmanDropTransformLeft;
    public Transform SnowmanDropTransformRight;
    public Transform SnowmanFixedTransform;
    public GameObject SnowmanDropper;

    public List<BF_PlayerSnow> BuiltSnowballList = new List<BF_PlayerSnow>();

    public SmoothCameraFollow cameraFollow;

    public float DropperMoveSpeed;

    public float HighestSnowmanHeight;

    private float OriginalDropperMoveSpeed;

    private bool InBuildMode = true;
    private bool IsDropped = false;

    private Vector3 inputDirection;

    private void Awake()
    {
        Instance = this;
        BackToGame();

        HighestSnowmanHeight = 0f;
    }

    public void BackToGame()
    {
        if (!InBuildMode)
        {
            return;
        }

        MainCamera.SetActive(true);
        BuilderCamera.SetActive(false);

        InBuildMode = false;

        var player = Instantiate(PlayerPrefab);
        var effect = Instantiate(PlayerSpawnEffect);
        player.transform.position = PlayerSpawnParent.transform.position;
        effect.transform.position = PlayerSpawnParent.transform.position;

        cameraFollow.target = player.transform;

        foreach (var obj in AnimalManager.Instance.VictimList)
        {
            obj.runaway.player = player.transform;
        }
    }

    public void EnterBuilderMode()
    {
        if (InBuildMode)
        {
            return;
        }

        MainCamera.SetActive(false);
        BuilderCamera.SetActive(true);

        InBuildMode = true;
        IsDropped = false;

        var newPos = transform.position;
        newPos.y = HighestSnowmanHeight;
        transform.position = newPos;

        BF_PlayerSnow.Instance.rB.isKinematic = true;
        BF_PlayerSnow.Instance.transform.SetParent(SnowmanDropper.transform, false);
        BF_PlayerSnow.Instance.transform.localPosition = Vector3.zero;

        BF_PlayerSnow.Instance.enabled = false;
        BF_PlayerMovement.Instance.enabled = false;

        OriginalDropperMoveSpeed = DropperMoveSpeed;

        if (BuiltSnowballList.Count == 0)
        {
            DropperMoveSpeed = 0;
        }
    }

    public void Update()
    {
        float t = (Mathf.Sin(Time.time * DropperMoveSpeed) + 1.0f) / 2.0f;
        SnowmanDropper.transform.position = Vector3.Lerp(SnowmanDropTransformLeft.position, SnowmanDropTransformRight.position, t);

        if (InBuildMode && !IsDropped)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                DropSnowball();
            }

            if (Input.GetKey(KeyCode.A))
            {
                inputDirection += Vector3.left;
            }
            if (Input.GetKey(KeyCode.D))
            {
                inputDirection += Vector3.right;
            }
            if (Input.GetKey(KeyCode.W))
            {
                inputDirection += Vector3.forward;
            }
            if (Input.GetKey(KeyCode.S))
            {
                inputDirection += Vector3.back;
            }
            MoveBall();
        }


        if (Input.GetKeyDown(KeyCode.O))
        {
            EnterBuilderMode();
        }
    }

    private void MoveBall()
    {
        // Create a rotation vector from input directions
        Vector3 rotationVector = new Vector3(inputDirection.z, 0, -inputDirection.x);

        // Convert rotation vector to a Quaternion using Euler angles
        Quaternion rotation = Quaternion.Euler(rotationVector);

        // Rotate around the world Y axis and adjust pitch and roll in world space
        BF_PlayerSnow.Instance.transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0) * BF_PlayerSnow.Instance.transform.rotation;
        BF_PlayerSnow.Instance.transform.Rotate(rotationVector * 1, Space.World);

        // Reset input direction
        inputDirection = Vector3.zero;
    }

    private void DropSnowball()
    {
        IsDropped = true;

        BF_PlayerSnow.Instance.transform.SetParent(SnowmanFixedTransform, true);
        BF_PlayerSnow.Instance.rB.isKinematic = false;

        BF_PlayerSnow.Instance.ddb.enabled = true;

        if (BuiltSnowballList.Count == 0)
        {
            DropperMoveSpeed = OriginalDropperMoveSpeed;
        }
    }

    public void BackToGameAfterSeconds(float sleepTimes)
    {
        StartCoroutine(internalCoroutine(sleepTimes));
    }

    private IEnumerator internalCoroutine(float sleepTimes)
    {
        yield return new WaitForSeconds(sleepTimes);

        BackToGame();
    }

    public void BoomAllSnowball(BF_PlayerSnow start)
    {
        StartCoroutine(boomallcoroutine(start));
        StartCoroutine(cameramovecoroutine());
    }

    private IEnumerator boomallcoroutine(BF_PlayerSnow start)
    {
        yield return new WaitForSeconds(0.5f);

        start.BoomSnowball();

        yield return new WaitForSeconds(1.1f);

        while (BuiltSnowballList.Count > 0)
        {
            var ball = BuiltSnowballList[BuiltSnowballList.Count - 1];
            BuiltSnowballList.Remove(ball);
            ball.BoomSnowball();
            yield return new WaitForSeconds(0.25f);
        }

        yield return new WaitForSeconds(2.5f);

        HighestSnowmanHeight = 0;
        BackToGame();
    }

    private IEnumerator cameramovecoroutine()
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition - new Vector3(0, HighestSnowmanHeight, 0);

        // 이동에 걸리는 시간
        float elapsedTime = 0f;
        float duration = 0.25f * BuiltSnowballList.Count;

        yield return new WaitForSeconds(1.6f);

        while (elapsedTime < duration)
        {
            // 비율 계산
            float t = elapsedTime / duration;
            // 위치 보간
            transform.position = Vector3.Lerp(startPosition, endPosition, t);

            // 시간 증가
            elapsedTime += Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }

        // 최종 위치 설정 (정확한 위치 도달을 보장)
        transform.position = endPosition;
    }
}
