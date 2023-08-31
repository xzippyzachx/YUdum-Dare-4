using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SpringJoint))]
public class TetherLinePoint : MonoBehaviour
{

    public Rigidbody rb { get; private set; }
    public SpringJoint springJoint { get; private set; }
    public CapsuleCollider capsuleCollider { get; private set; }

    private TetherLine tetherLine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        springJoint = GetComponent<SpringJoint>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        tetherLine = GetComponentInParent<TetherLine>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Sharp") && tetherLine.HasConnection())
        {
            tetherLine.RemoveConnection();
            TetherManager.Singleton.CheckOxygenConnection();
        }
    }

}
