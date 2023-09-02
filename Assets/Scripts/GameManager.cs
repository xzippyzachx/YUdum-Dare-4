using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _singleton;
    public static GameManager Singleton
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
                Destroy(value.gameObject);
            }
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        Singleton = this;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            GameUI.Singleton.ShowCursor(!Cursor.visible);
        }

        if (Input.GetKey(KeyCode.Tab))
        {
            GameUI.Singleton.TransitionOut();
            DOTween.Sequence().AppendInterval(0.5f).OnComplete(() => {
                SceneManager.LoadScene("LevelSelect");
            });
        }
    }
}
