using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleGizmo : MonoBehaviour {
    [SerializeField]
    Color color = Color.blue;

    [SerializeField]
    float radius = 1f;

    private void OnDrawGizmos() {
        Color oldColor = Gizmos.color;
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, radius);
        Gizmos.color = oldColor;
    }
}
