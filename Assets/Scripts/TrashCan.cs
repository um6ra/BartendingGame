using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MonoBehaviour
{
    Collider[] colliders;

    private void FixedUpdate()
    {

        colliders = Physics.OverlapSphere(transform.position, 1.5f);

        foreach (Collider collider in colliders)
        {
            LiquorBottle bottle = collider.GetComponent<LiquorBottle>();
            if (bottle != null) Destroy(bottle.gameObject);

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 1.5f);
    }

}
