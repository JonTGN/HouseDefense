using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugGizmo : MonoBehaviour
{

    enum GizmoType
    {
        sphere = 0,
        cube = 1,
    }

    [SerializeField]
    GizmoType type;

    [SerializeField]
    float size = 1f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;

        if(type == GizmoType.sphere)
            Gizmos.DrawSphere(transform.position, size);
        else
            Gizmos.DrawCube(transform.position, new Vector3(size, size, size));
    }
}
