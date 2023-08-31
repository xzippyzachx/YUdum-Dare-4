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

    private TetherPole lastPlaced;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        GameUI.Singleton.UpdateTetherCountText(tetherPoles.Count, maxTetherPoleAmount);
    }

    public void AttemptPlaceTetherPole(Vector3 position)
    {
        if (AttemptRemoveTetherPole(position) || tetherPoles.Count >= maxTetherPoleAmount)
        {
            return;
        }

        TetherPole newTetherPole = Instantiate(tetherPolePrefab, position, Quaternion.identity).GetComponent<TetherPole>();
        tetherPoles.Add(newTetherPole);
        newTetherPole.PlayPlaceTween();

        newTetherPole.tetherLine.SetConnection(Player.player.playerMovement.rb, new Vector3(0, 0.5f, 0));

        if (tetherPoles.Count > 0 && lastPlaced != null)
        {
            lastPlaced.tetherLine.SetConnection(newTetherPole.tetherLine.GetRootPoint());
            newTetherPole.attachedFromPole = lastPlaced;
            lastPlaced.attachedToPole = newTetherPole;
        }

        lastPlaced = newTetherPole;
        CheckOxygenConnection();
        GameUI.Singleton.UpdateTetherCountText(tetherPoles.Count, maxTetherPoleAmount);
    }                                                                
    public bool AttemptRemoveTetherPole(Vector3 position)
    {
        if (tetherPoles.Count == 0)
        {
            return false;
        }

        TetherPole closestPole = GetClosestTetherPole(position);
        if (Vector3.Distance(closestPole.transform.position, position) < 1f)
        {
            TetherPole attachedFromPole = closestPole.attachedFromPole;

            if (attachedFromPole != null)
            {
                if (lastPlaced == closestPole)
                {
                    attachedFromPole.tetherLine.SetConnection(Player.player.playerMovement.rb, new Vector3(0, 0.5f, 0));
                    lastPlaced = attachedFromPole;
                }
                else
                {
                    attachedFromPole.tetherLine.RemoveConnection();
                    if (closestPole.attachedToPole != null)
                    {
                        closestPole.attachedToPole.attachedFromPole = null;
                    }
                }
            }
            
            tetherPoles.Remove(closestPole);
            Destroy(closestPole.gameObject);

            CheckOxygenConnection();
            GameUI.Singleton.UpdateTetherCountText(tetherPoles.Count, maxTetherPoleAmount);

            return true;
        }
        return false;
    }

    public void SetLastPlaced(TetherPole pole)
    {
        lastPlaced = pole;
    }

    public void UntetherLastPlaced()
    {
        if (lastPlaced == null)
        {
            return;
        }

        lastPlaced.tetherLine.RemoveConnection();
        lastPlaced.attachedToPole = null;
    }

    public void CheckOxygenConnection()
    {
        if (!Check(lastPlaced))
        {
            Player.player.Die();
        }
    }

    private bool Check(TetherPole pole)
    {
        if (!pole.tetherLine.HasConnection())
        {
            return false;
        }
        if (pole.oxygenSupply)
        {
            return true;
        }
        else if (pole.attachedFromPole == null)
        {
            pole.tetherLine.SetHasOxygen(false);
            return false;
        }
        bool check = Check(pole.attachedFromPole);
        if (!check)
        {
            pole.tetherLine.SetHasOxygen(false);
        }

        return check;
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
