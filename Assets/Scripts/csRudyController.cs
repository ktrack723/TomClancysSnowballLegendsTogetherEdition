using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class csRudyController : MonoBehaviour
{
    [Header("Fetch on start")]

    [SerializeField] private AudioSource audioSource;

    [SerializeField] private Animator animator;

    [Header("Assigned")]

    [SerializeField] private GameObject Bbaru;

    [SerializeField] private AudioClip dashClip;
    [SerializeField] private AudioClip getClip;
    [SerializeField] private AudioClip hitClip;

    [Header("Parameters")]

    [SerializeField] private float speed;

    [Header("Debug")]

    [SerializeField] private bool isMoving;

    [SerializeField] private bool isHoldingBbaru;

    [SerializeField] private Vector3 velocity;
    [SerializeField] private float dashVelocityMultiplier;



    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        animator = GetComponent<Animator>();
    }



    private void Update()
    {
        ProcessMoveInput();

        Bbaru.SetActive(isHoldingBbaru);
    }



    private void ProcessMoveInput()
    {
        if (LeanTween.isTweening(gameObject) == true)
        {
            Move();

            return;
        }

        velocity = Vector3.zero;

        if (Input.GetKey(KeyCode.A) == true)
        {
            velocity.x -= 1.0f;
        }

        if (Input.GetKey(KeyCode.D) == true)
        {
            velocity.x += 1.0f;
        }

        if (Input.GetKey(KeyCode.W) == true)
        {
            velocity.z += 1.0f;
        }

        if (Input.GetKey(KeyCode.S) == true)
        {
            velocity.z -= 1.0f;
        }

        if (velocity.x != 0 && velocity.z != 0)
        {
            velocity /= 1.414f;
        }

        isMoving = (velocity != Vector3.zero);

        animator.SetBool("isMoving", isMoving);

        if (Input.GetKeyDown(KeyCode.Space) && isMoving == true)
        {
            Dash();
        }

        Move();
    }



    private void Dash()
    {
        audioSource.PlayOneShot(dashClip, 0.55f);

        LeanTween.value(gameObject, UpdateDash, 1, 5, 0.1f).setEaseInOutCubic().setOnComplete(()=>
        {
            LeanTween.value(gameObject, UpdateDash, 5, 1, 0.25f).setEaseInOutCubic();
        });
    }



    private void UpdateDash(float arg)
    {
        dashVelocityMultiplier = arg;
    }



    private void Move()
    {
        if (Mathf.Abs((transform.position + velocity).x) > 11.9f)
        {
            velocity.x = 0;
        }

        if (Mathf.Abs((transform.position + velocity).z) > 5.9f)
        {
            velocity.z = 0;
        }

        transform.Translate((velocity * speed * dashVelocityMultiplier) * Time.deltaTime);
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bbaru") == true && isHoldingBbaru == false)
        {
            isHoldingBbaru = true;

            audioSource.PlayOneShot(getClip, 1.3f);

            Destroy(other.gameObject);
        }

        if (other.CompareTag("House") == true && isHoldingBbaru == true)
        {
            isHoldingBbaru = false;

            audioSource.PlayOneShot(hitClip);

            Destroy(other.gameObject);
        }
    }
}