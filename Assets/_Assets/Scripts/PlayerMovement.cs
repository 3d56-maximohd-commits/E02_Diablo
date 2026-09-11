using JetBrains.Annotations;
using System.Text.Json.Serialization;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 10f;
    public Transform model;
    public float rotationSpeed = 100f;

    public float tiltLimit = 0;
    public GameObject aimObject;
    [SerializeField] InputActionReference moveAction;

 
    private void Update()
    {

        Vector2 movement = moveAction.action.ReadValue<Vector2>();
        LocalMove(movement.x, movement.y, movementSpeed);
        ClampPosition();
        LookRotation(movement.x, movement.y, rotationSpeed );
        HorizontalTilt(model, -movement.x, .1f, tiltLimit);
    }

    void LocalMove(float x, float y, float speed)
    {
        transform.localPosition += new Vector3(x, y, 0) * speed * Time.deltaTime;
    }

    void LookRotation(float x, float y, float speed)
    {
        aimObject.transform.localPosition = new Vector3(x, y, 1);
        gameObject.transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(aimObject.transform.position), Mathf.Deg2Rad * speed * Time.deltaTime);
    }

    void ClampPosition()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(aimObject.transform.position, .5f);
        Gizmos.DrawSphere(aimObject.transform.position, .15f);
    }

    void HorizontalTilt(Transform target, float axis, float lerpTime, float tiltLimit)
    {
        Vector3 targetEulerAngles = target.localEulerAngles;
        target.localEulerAngles = new Vector3(targetEulerAngles.x, targetEulerAngles.y, Mathf.LerpAngle(targetEulerAngles.z, axis * tiltLimit, lerpTime));
    }


}
