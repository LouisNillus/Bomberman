using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class EndMenu : MonoBehaviour
{
    public static EndMenu instance;

    [SerializeField] private GameObject _endPanel;
    [SerializeField] private TextMeshProUGUI _winText;

    private void Awake()
    {
        instance = this;
    }

    public void ShowEndMenu(string p_winner)
    {
        _endPanel.SetActive(true);
        _winText.text = $"The winner is : {p_winner}";
        Time.timeScale = 0.0f;
    }

    public void Restart()
    {
        Time.timeScale = 1.0f;
        _endPanel.SetActive(false);

        GridHandler.instance.ReadMap();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
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
