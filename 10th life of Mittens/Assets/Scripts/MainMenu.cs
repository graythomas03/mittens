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
        SoundManager.Instance.ToggleCredit(false);
        SoundManager.Instance.PlayOnce(SoundFX.SFXButton);
        SoundManager.Instance.ToggleTitle(true);
        ShowMainPanel();
    }

    private void StarGame()
    {
        SoundManager.Instance.PlayOnce(SoundFX.SFXButton);
        //SceneManager.LoadScene("GameSceneName");
        // or
        //GameManager.StartGame();
        //Hide();
    }

    private void ExitGame()
    {
        SoundManager.Instance.PlayOnce(SoundFX.SFXButton);
        Application.Quit();
    }

    private void ShowMainPanel()
    {
        MainPanel.SetActive(true);
        CreditsPanel.SetActive(false);
    }

    private void ShowCreditPanel()
    {
        SoundManager.Instance.ToggleTitle(false);
        SoundManager.Instance.ToggleCredit(true);
        SoundManager.Instance.PlayOnce(SoundFX.SFXButton);
        MainPanel.SetActive(false);
        CreditsPanel.SetActive(true);
    }
}
