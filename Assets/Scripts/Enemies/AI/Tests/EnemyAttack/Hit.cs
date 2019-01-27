using UnityEngine;
using System.Collections;

public class Hit : MonoBehaviour
{
    public float distance = 10;
    public float range = 90;

    public Transform target;

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        var originalColor = UnityEditor.Handles.color;

        UnityEditor.Handles.color = new Color(1, 0, 0, .1f);

        UnityEditor.Handles.DrawLine(
            transform.position,
            transform.position + transform.forward
        );

        UnityEditor.Handles.DrawLine(
            transform.position,
            transform.position + (Quaternion.Euler(0, -range / 2, 0) * Vector3.forward)
        );

        UnityEditor.Handles.DrawSolidArc(
            transform.position,
            transform.up,
            Quaternion.Euler(0, -range / 2, 0) * Vector3.forward,
            range,
            distance);
        
        UnityEditor.Handles.color = originalColor;
#endif
    }

    private void Update()
    {
        if (HitCheck(target, transform, distance, range))
        {
            Debug.Log("Damageeeeee!!");
        }
    }

    public static bool HitCheck(Transform target, Transform origin, float distance, float range)
    {
        var vectorToCollider = (target.position - origin.position);
        if (vectorToCollider.sqrMagnitude < distance * distance)
        {
            var angle = Mathf.Cos(range / 2 * Mathf.Deg2Rad);
            if (Vector3.Dot(vectorToCollider.normalized, origin.forward) > angle)
            {
                return true;
            }
        }
        return false;
    }
}
