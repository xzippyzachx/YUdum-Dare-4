using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    private static GameUI _singleton;
    public static GameUI Singleton
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
                Debug.Log($"{nameof(GameUI)} instance already exists, destroying object!");
                Destroy(value.gameObject);
            }
        }
    }

    [SerializeField] private RectTransform transitionMask;

    [SerializeField] private TextMeshProUGUI tetherCountText;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        ShowCursor(false);
        TransitionIn();
    }

    public void ShowCursor(bool show)
    {
        Cursor.visible = show;
        Cursor.lockState = show ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void UpdateTetherCountText(int currentAmount, int maxAmount)
    {
        tetherCountText.text = $"{maxAmount - currentAmount}/{maxAmount}";
    }
    
    public void TransitionOut()
    {
        transitionMask.gameObject.SetActive(true);
        transitionMask.DOScale(1, 0.5f).OnComplete(() => {
            transitionMask.GetChild(1).gameObject.SetActive(true);
        });
    }

    public void TransitionIn()
    {
        transitionMask.gameObject.SetActive(true);
        transitionMask.DOScale(1, 0);
        transitionMask.DOScale(600, 0.5f).OnComplete(() => {
            transitionMask.gameObject.SetActive(false);
        });
    }
}
