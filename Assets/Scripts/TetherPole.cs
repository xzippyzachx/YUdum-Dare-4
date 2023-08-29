using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherPole : MonoBehaviour
{
    [field:SerializeField] public TetherLine tetherLine { get; private set; }

    public TetherPole attachedPole;

    [field:SerializeField] public bool autoTether { get; private set; }

    private void Start()
    {
        if (autoTether)
        {
            tetherLine.RemoveConnection();
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (autoTether && col.CompareTag("Player"))
        {
            tetherLine.SetConnection(col.GetComponent<Rigidbody>(), new Vector3(0, 0.5f, 0));
            TetherManager.Singleton.SetLastPlaced(this);
        }
    }
    
}
