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

    private void Awake()
    {
        Singleton = this;
    }

    public void AttemptPlaceTetherPole(Vector3 position, Player player)
    {
        if (AttemptRemoveTetherPole(position, player) || tetherPoles.Count >= maxTetherPoleAmount)
        {
            return;
        }

        TetherPole newTetherPole = Instantiate(tetherPolePrefab, position, Quaternion.identity).GetComponent<TetherPole>();
        tetherPoles.Add(newTetherPole);

        newTetherPole.tetherLine.SetConnection(player.GetComponent<Rigidbody>(), new Vector3(0, 0.5f, 0));

        if (tetherPoles.Count > 1)
        {
            tetherPoles[tetherPoles.Count - 2].tetherLine.SetConnection(newTetherPole.tetherLine.GetRootPoint());
        }
    }

    public bool AttemptRemoveTetherPole(Vector3 position, Player player)
    {
        if (tetherPoles.Count == 0)
        {
            return false;
        }

        TetherPole closestPole = GetClosestTetherPole(position);
        if (closestPole == tetherPoles[tetherPoles.Count - 1] && Vector3.Distance(closestPole.transform.position, position) < 0.5f)
        {
            tetherPoles.Remove(closestPole);
            Destroy(closestPole.gameObject);

            if (tetherPoles.Count > 0)
            {
                tetherPoles[tetherPoles.Count - 1].tetherLine.SetConnection(player.GetComponent<Rigidbody>(), new Vector3(0, 0.5f, 0));
            }
            return true;
        }
        return false;
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
