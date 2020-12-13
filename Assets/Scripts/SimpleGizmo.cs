using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleGizmo : MonoBehaviour {
    [SerializeField]
    Color color = Color.blue;

    [SerializeField]
    float radius = 1f;

    private void OnDrawGizmos() {
        Gizmos.DrawSphere(transform.position, radius);
    }
}
