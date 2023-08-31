using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    
    [SerializeField] private string LoadSceneName;

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            GameUI.Singleton.TransitionOut();
            DOTween.Sequence().AppendInterval(0.5f).OnComplete(() => {
                SceneManager.LoadScene(LoadSceneName);
            });
        }
    }

}
