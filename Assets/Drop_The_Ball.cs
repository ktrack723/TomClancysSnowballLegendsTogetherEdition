using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop_The_Ball : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            // FIrst SnOWBalL is Safe
            if (GetComponent<BF_PlayerSnow>().index == 0)
            {
                GetComponent<BF_PlayerSnow>().rB.isKinematic = true;
                return;
            }

            // /execute BOOM -CODE -hEre
            return;
        }

        var snow = collision.gameObject.GetComponent<BF_PlayerSnow>();
        if (snow)
        {
            var radius = snow.sphereColider.radius * snow.sphereColider.transform.localScale.x;
            var dropPos = new Vector2(transform.position.x, transform.position.z);
            var underPos = new Vector2(snow.transform.position.x, snow.transform.position.z);

            var distance = Vector2.Distance(dropPos, underPos);
            var isSafe = distance <= radius;

            Debug.Log($"´« ¶³¾îÁü, °á°ú={isSafe}, ¶³¾îÁø¾Ö={dropPos}, ¸ÂÀº¾Ö={underPos}, ¹ÝÁö¸§={radius}, °Å¸®={distance}");

            if (isSafe)
            {
                snow.rB.isKinematic = true;
            }
            else
            {
                // GOM - MUN - GOM - MUN and Boom on collide with floor.
                return;
            }
        }
    }
}
