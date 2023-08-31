using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TetherPole : MonoBehaviour
{
    [field:SerializeField] public TetherLine tetherLine { get; private set; }

    [SerializeField] private GameObject model;

    public TetherPole attachedFromPole;
    public TetherPole attachedToPole;

    [field:SerializeField] public bool oxygenSupply { get; private set; }

    private void Start()
    {
        if (oxygenSupply)
        {
            tetherLine.RemoveConnection();
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (oxygenSupply && !tetherLine.HasConnection() && col.CompareTag("Player"))
        {
            TetherManager.Singleton.UntetherLastPlaced();
            tetherLine.SetConnection(col.GetComponent<Rigidbody>(), new Vector3(0, 0.5f, 0));
            TetherManager.Singleton.SetLastPlaced(this);
        }
    }

    public void PlayPlaceTween()
    {
        model.transform.DOScale(0.5f, 0);
        model.transform.DOMoveY(0.5f, 0);
        model.transform.DOScale(1, 0.5f);
        model.transform.DOMoveY(0, 0.5f);
    }
    
}
