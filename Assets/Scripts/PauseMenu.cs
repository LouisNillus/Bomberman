using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] private GameObject _PausePanel;


    public void Show(bool p_show)
    {
        _PausePanel.SetActive(p_show);
    }

    public void Resume()
    {
        // Break pause State

        Show(false);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
