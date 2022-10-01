using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject MainPanel;
    public GameObject CreditsPanel;

    [Header("Buttons")]
    public Button BackBtn;
    public Button StartGameBtn;
    public Button ExitGameBtn;
    public Button CreditsBtn;

    private void Awake()
    {
        BackBtn.onClick.AddListener(GoBack);
        StartGameBtn.onClick.AddListener(StarGame);
        ExitGameBtn.onClick.AddListener(ExitGame);
        CreditsBtn.onClick.AddListener(ShowCreditPanel);
    }

    private void Start()
    {
        ShowMainPanel();
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
        ShowMainPanel();
    }

    public void Hide()
    {
        this.gameObject.SetActive(true);
    }

    private void GoBack()
    {
        ShowMainPanel();
    }

    private void StarGame()
    {
        //SceneManager.LoadScene("GameSceneName");
        // or
        //GameManager.StartGame();
        //Hide();
    }

    private void ExitGame()
    {
        Application.Quit();
    }

    private void ShowMainPanel()
    {
        MainPanel.SetActive(true);
        CreditsPanel.SetActive(false);
    }

    private void ShowCreditPanel()
    {
        MainPanel.SetActive(false);
        CreditsPanel.SetActive(true);
    }
}
