using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class BF_PlayerSnow : MonoBehaviour
{
    public static BF_PlayerSnow Instance;
    private static int ID_Generator = 0;

    public int index;

    public Collider playerCollider;
    public SphereCollider sphereColider;
    public ParticleSystem particleSys;

    public PhysicMaterial playerMatDefault;
    public PhysicMaterial playerMatSnow;
    public PhysicMaterial playerMatIce;

    public Transform StiatcVictimTransformParent;
    public Transform GrowthVictimTransformParent;

    public Drop_The_Ball ddb;

    public GameObject boomEffect;

    // (GameObject, InitialDepth)
    private List<(Victim, float)> StaticVictimList = new List<(Victim, float)>();
    private List<(Victim, float)> PendingRemoveStaticVictimList = new List<(Victim, float)>();
    // (GameObject, originalScale)
    private List<(Victim, Vector3)> GrowthVictimList = new List<(Victim, Vector3)>();

    public Rigidbody rB;
    private float speedMult = 1;
    private float lerpIce = 0;
    private MeshCollider oldMC = null;
    private Mesh mesh = null;
    private ParticleSystem.MainModule pSMain;

    public AudioClip Yum;

    public List<TextMeshPro> counters;

    public enum EAnimals
    {
        Gecko = 0,
        Monkey,
        Fish,
        Bird,
        Deer,
        Rat,
        Snake
    }



    private void Awake()
    {
        Instance = this;
        index = ID_Generator;
        ID_Generator++;
    }

    // Start is called before the first frame update
    void Start()
    {
        oldMC = null;
        mesh = null;
        rB = this.GetComponent<Rigidbody>();
        pSMain = particleSys.main;

        for (int i = 0; i < System.Enum.GetNames(typeof(EAnimals)).Count(); i++)
        {
            string textName = ((EAnimals)i).ToString() + "Text";
            counters.Add(GameObject.Find(textName).GetComponent<TextMeshPro>());
        }
    }

    private void CheckIceCols(float snowCol)
    {
        lerpIce = snowCol / 255f;

        if (snowCol == -1)
        {
            if (playerCollider.material != playerMatDefault)
            {
                playerCollider.material = playerMatDefault;
            }
            return;
        }


        if (lerpIce <= 0.925f && playerCollider.material != playerMatIce)
        {
            playerCollider.material = playerMatIce;
            rB.angularDrag = 0.25f;
        }
        else if(lerpIce >= 0.925f && playerCollider.material != playerMatSnow)
        {
            playerCollider.material = playerMatSnow;
            rB.angularDrag = 5f;
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.impulse.magnitude>10)
        {
           // RemoveSnow(20);
        }

        Victim victim = collision.gameObject.GetComponent<Victim>();
        if (victim != null && victim.IsCaught == false)
        {
            victim.IsCaught = true;
            Destroy(victim.rigidbody);
            //victim.rigidbody.isKinematic = true;
            StaticVictimList.Add((victim, sphereColider.radius * sphereColider.transform.localScale.x));
            //GrowthVictimList.Add((victim, victim.transform.localScale));
            //collision.gameObject.transform.SetParent(GrowthVictimTransformParent, true);

            Vector3 directionToContact = (collision.transform.position - transform.position).normalized;
            Vector3 newVictimPosition = transform.position + directionToContact * sphereColider.transform.localScale.x / 1.75f;

            // Set victim's position to the calculated position
            victim.transform.position = newVictimPosition;

            collision.gameObject.transform.SetParent(sphereColider.transform, true);

            int index = (int)System.Enum.Parse(typeof(EAnimals), collision.gameObject.name);
            counters[index].fontStyle |= FontStyles.Strikethrough;

            GetComponent<AudioSource>().PlayOneShot(Yum, 4.5f);
        }
    }

    private void AddSnow(float multiplier)
    {
        if (playerCollider.transform.localScale.x < 5f)
        {
            speedMult = Mathf.Clamp(rB.velocity.magnitude * 0.02f,0,1);
            playerCollider.transform.localScale += Vector3.zero + Vector3.one * 0.005f * 2 * multiplier* speedMult;
            transform.position = new Vector3(transform.position.x,
                transform.position.y + 0.005f * 8 * multiplier * speedMult,
                transform.position.z);
        }
    }
    private void RemoveSnow(float multiplier)
    {
        if (playerCollider.transform.localScale.x >= 1.1f)
        {
            if (!playerCollider.transform.gameObject.activeInHierarchy)
            {
               // SnowPlayer.gameObject.SetActive(true);
            }
            playerCollider.transform.localScale -= Vector3.zero + Vector3.one * 0.005f * 4 * multiplier;
        }
        if (playerCollider.transform.localScale.x < 1.1f)
        {
            if (playerCollider.transform.gameObject.activeInHierarchy)
            {
               // SnowPlayer.gameObject.SetActive(false);
            }
            playerCollider.transform.localScale = Vector3.one * 1.1f;
        }
    }

    private void FixedUpdate()
    {
        //ChangePlayerMass();
        if (transform.position.y - (sphereColider.transform.localScale.x / 2f) < 0.25f)
        {
            particleSys.Play();
        }
        else
        {
            particleSys.Stop();
        }

        CheckSnowUnderneath();

        if (rB.velocity.magnitude < 0.5f)
        {
            RemoveSnow(0.2f);
        }
        else
        {
            AddSnow(4);
        }
    }

    private void CheckSnowUnderneath()
    {
        RaycastHit hit;

        int layerMask = 1 << 4;
        if (Physics.Raycast(transform.position+(Vector3.down*(playerCollider.transform.localScale.x/2)+Vector3.up*0.5f), Vector3.down, out hit, 5, layerMask,QueryTriggerInteraction.Ignore))
        {
            MeshCollider meshCollider = hit.collider as MeshCollider;
            if (oldMC != meshCollider || mesh == null)
            {
                mesh = meshCollider.GetComponent<MeshFilter>().sharedMesh;
            }
            oldMC = meshCollider;
            if (meshCollider == null || meshCollider.sharedMesh == null)
            {
                CheckIceCols(255f);
                return;
            }

            //Mesh mesh = meshCollider.sharedMesh;

            int[] triangles = mesh.triangles;
            Color32[] colorArray;
            colorArray = mesh.colors32;

            var vertIndex1 = triangles[hit.triangleIndex * 3 + 0];
            CheckIceCols(((float)colorArray[vertIndex1].g) / 1);
        }
        else
        {
            if (playerCollider.material != playerMatDefault)
            {
                playerCollider.material = playerMatDefault;
            }
        }
    }

    private void ChangePlayerMass()
    {
        rB.mass = Mathf.Lerp(1.95f, 2.5f, (playerCollider.transform.localScale.x-1.2f) / 7);
        pSMain.startSize = playerCollider.transform.localScale.x+0.5f;

        GrowthVictimTransformParent.localScale = playerCollider.transform.localScale;
        foreach (var obj in GrowthVictimList)
        {
            obj.Item1.transform.localScale = obj.Item2 / GrowthVictimTransformParent.localScale.x;
        }

        //var rad = sphereColider.radius * sphereColider.transform.localScale.x;
        //foreach (var obj in StaticVictimList)
        //{
        //    if (rad >= obj.Item2 + obj.Item1.collider.bounds.size.x / 3)
        //    {
        //        GrowthVictimList.Add((obj.Item1, obj.Item1.gameObject.transform.localScale));
        //        obj.Item1.gameObject.transform.SetParent(GrowthVictimTransformParent, true);

        //        PendingRemoveStaticVictimList.Add(obj);
        //    }
        //}

        //foreach (var obj in PendingRemoveStaticVictimList)
        //{
        //    StaticVictimList.Remove(obj);
        //}
        //PendingRemoveStaticVictimList.Clear();
    }

    public List<Victim> GetAllAttachedVictims()
    {
        var res = new List<Victim>(StaticVictimList.Count + GrowthVictimList.Count);
        res.AddRange(StaticVictimList.Select(obj => obj.Item1));
        //res.AddRange(GrowthVictimList.Select(obj => obj.Item1));

        return res;
    }

    public void BoomSnowball()
    {
        var effect = Instantiate(boomEffect, transform.position, Quaternion.identity);

        var list = GetAllAttachedVictims();
        foreach (var obj in list)
        {
            obj.transform.SetParent(transform.parent, true);
            obj.Kill();
            var rb = obj.AddComponent<Rigidbody>();

            var direction = obj.transform.position - transform.position;
            direction.Normalize();

            rb.AddForce(direction * 500f);
            rb.AddForce(Vector3.up * 1000f);
        }

        Destroy(gameObject);
    }
}
