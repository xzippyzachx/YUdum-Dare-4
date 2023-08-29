using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherManager : MonoBehaviour
{
    private static TetherManager _singleton;
    public static TetherManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
            {
                _singleton = value;
            }
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(TetherManager)} instance already exists, destroying object!");
                Destroy(value.gameObject);
            }
        }
    }

    [SerializeField] private GameObject tetherPolePrefab;

    private List<TetherPole> tetherPoles = new List<TetherPole>();
    [SerializeField] private int maxTetherPoleAmount;

    [SerializeField] private Player player;

    private TetherPole lastPlaced;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        AttemptPlaceTetherPole(player.transform.position);
    }

    public void AttemptPlaceTetherPole(Vector3 position)
    {
        if (AttemptRemoveTetherPole(position) || tetherPoles.Count >= maxTetherPoleAmount)
        {
            return;
        }

        TetherPole newTetherPole = Instantiate(tetherPolePrefab, position, Quaternion.identity).GetComponent<TetherPole>();
        tetherPoles.Add(newTetherPole);

        newTetherPole.tetherLine.SetConnection(player.GetComponent<Rigidbody>(), new Vector3(0, 0.5f, 0));

        if (tetherPoles.Count > 1 && lastPlaced != null)
        {
            lastPlaced.tetherLine.SetConnection(newTetherPole.tetherLine.GetRootPoint());
            newTetherPole.attachedPole = lastPlaced;
        }

        lastPlaced = newTetherPole;
    }

    public bool AttemptRemoveTetherPole(Vector3 position)
    {
        if (tetherPoles.Count <= 1)
        {
            return false;
        }

        TetherPole closestPole = GetClosestTetherPole(position);
        if (Vector3.Distance(closestPole.transform.position, position) < 0.5f)
        {
            TetherPole attachedPole = closestPole.attachedPole;

            if (tetherPoles.Count > 1 && attachedPole != null)
            {
                if (lastPlaced == closestPole)
                {
                    attachedPole.tetherLine.SetConnection(player.GetComponent<Rigidbody>(), new Vector3(0, 0.5f, 0));
                    lastPlaced = attachedPole;
                }
                else
                {
                    attachedPole.tetherLine.RemoveConnection();
                }
            }
            
            tetherPoles.Remove(closestPole);
            Destroy(closestPole.gameObject);

            return true;
        }
        return false;
    }

    public void SetLastPlaced(TetherPole pole)
    {
        lastPlaced = pole;
    }

    public TetherPole GetClosestTetherPole(Vector3 position)
    {
        int closestIndex = 0;
        float closestDist = Mathf.Infinity;
        for (int i = 0; i < tetherPoles.Count; i++)
        {
            float dist = Vector3.Distance(tetherPoles[i].transform.position, position);
            if (dist < closestDist)
            {
                closestIndex = i;
                closestDist = dist;
            }
        }

        return tetherPoles[closestIndex];
    }

}
