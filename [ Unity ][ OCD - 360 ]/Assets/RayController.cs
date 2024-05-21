using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayController : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit)) ;
        {
            Gizmos.DrawSphere(hit.point, 0.25f);
        }
    }
}
