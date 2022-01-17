using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehavior : MonoBehaviour
{
    [Header("‘¬“x")]
    public float _FlySpeed;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (transform.position.y < -5) Destroy(gameObject);
        if (rb.velocity.magnitude <= 1.2f) rb.velocity = Vector3.zero;
    }

    public void Throw(Vector3 forward_)
    {
        rb.AddForce(forward_.normalized * _FlySpeed, ForceMode.Impulse);
    }

    public void PickUp()
    {
        Destroy(gameObject);
    }
}
