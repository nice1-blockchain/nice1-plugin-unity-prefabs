using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class MainMenuManager : Singleton<MainMenuManager>
{

    [Header("Login")]
    public TMP_Text usernameText;
    public TMP_Text NFTLoadingText;
    public UIPanel loginPanel;
    public GameObject playButton;
    public GameObject loginButton;
    public GameObject loginButtonF2P;
    public GameObject logoutButton;

    public GameObject mainMenuPanel;
    public GameObject characterSelectionPanel;

    [Header("License")]
    public string errorLicenseText = "You are not licensed to use this game. Get in touch with Nice1 - https://t.me/nice1blockchain";

    [Header("Vuplex - Error message")]
    public string errorVuplexText = "It is necessary to have the Vuplex plugin installed. For more information go to https://docs.nice1.dev/";

    private void Awake()
    {
        GameManager.Instance.ResetCharacter();

        if (GameManager.Instance.IsLoggedIn)
        {
            SetLoggedInMenu();
            SetNFTLoadedMenu(WalletManager.Instance.checkLicense);

            WalletManager.Instance.CheckNewNFTs();
        }
        else
        {
            SetNotLoggedInMenu();
        }
    }

    public void SetLicenseInMenu()
    {
        playButton.SetActive(true);
    }

    public void SetLicenseNotFoundMenu()
    {
        NFTLoadingText.text = "License not found...";
    }

    public void SetLoggedInMenu()
    {
        HideLogin();
        usernameText.text = WalletManager.Instance.CurrentAccount.name;

        //playButton.SetActive(true);
        loginButton.SetActive(false);
        loginButtonF2P.SetActive(false);
        logoutButton.SetActive(true);

        NFTLoadingText.text = "Loading NFT keys...";
        NFTLoadingText.gameObject.SetActive(true);
    }

    private void SetNotLoggedInMenu()
    {
        usernameText.text = "Not logged in";
        NFTLoadingText.text = "";

        playButton.SetActive(false);
        loginButton.SetActive(true);
        loginButtonF2P.SetActive(true);
        logoutButton.SetActive(false);
    }

    public async void LogOut()
    {
#if VUPLEX_STANDALONE
        await GameObject.FindGameObjectWithTag("canvasWebView").GetComponent<Vuplex.WebView.CanvasWebViewPrefab>().LogOut2();
#endif        
        GameManager.Instance.ResetCharacter();
        GameManager.Instance.LockerAllCharacters();
        GameManager.Instance.ResetBonus();
        GameManager.Instance.IsLoggedIn = false;
        SetNotLoggedInMenu();
    }

    public void SetNFTLoadedMenu(bool checkLicense)
    {
        NFTLoadingText.text = "NFT keys loaded";

        if (!checkLicense)
        {
            SetF2PMenu();
        }
        else if (GameManager.Instance.GameUnlocked)
        {
            SetLicenseInMenu();
        }
        else
        {
            SetLicenseNotFoundMenu();
        }
        HideLogin();
    }

    public void SetF2PMenu()
    {
        NFTLoadingText.text = "F2P mode";

        SetLicenseInMenu();
        HideLogin();
    }

    public void ShowCharacterSelection()
    {
        mainMenuPanel.SetActive(false);
        characterSelectionPanel.gameObject.SetActive(true);
    }

    public void ShowLogin(bool checkLicense)
    {
        WalletManager.Instance.checkLicense = checkLicense;
#if VUPLEX_STANDALONE
        if (loginPanel != null) ShowPanel(loginPanel);
#else
        Debug.Log(errorVuplexText);
#endif
    }

    public void HideLogin()
    {
        HidePanel(loginPanel);
    }

    private void ShowPanel(UIPanel panel)
    {
        panel.ShowPanel();
    }

    private void HidePanel(UIPanel panel)
    {
        panel.HidePanel();
    }

    public void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }
}
