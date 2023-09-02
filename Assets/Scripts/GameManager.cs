using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
    }
}
