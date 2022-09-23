using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    // Log in
    public bool IsLoggedIn { get; set; }

    // Character selection
    public bool CharacterSelected { get; set; }
    public bool Character1Selected { get; private set; }
    public bool Character2Selected { get; private set; }
    public bool Character3Selected { get; private set; }
    public bool Character4Selected { get; private set; }

    // Game NFT
    public bool GameUnlocked { get; private set; }

    // Character NFT
    public bool Character1Unlocked { get; private set; }
    public bool Character2Unlocked { get; private set; }
    public bool Character3Unlocked { get; private set; }
    public bool Character4Unlocked { get; private set; }

    // Bonus NFT
    public bool Bonus1Unlocked { get; private set; }
    public bool Bonus2Unlocked { get; private set; }

    private void Awake()
    {
        IsLoggedIn = false;

        GameUnlocked = false;

        Character1Unlocked = true;   // First character is always unlocked
        Character2Unlocked = false;
        Character3Unlocked = false;
        Character4Unlocked = false;

        Bonus1Unlocked = false;
        Bonus2Unlocked = false;
    }

    public void ResetCharacter()
    {
        DeselectAll();
        CharacterSelected = false;
    }

    public void ResetBonus()
    {
        Bonus1Unlocked = false;
        Bonus2Unlocked = false;
    }

    public void SelectCharacter(int index)
    {
        DeselectAll();

        switch (index)
        {
            case 0:
                Character1Selected = true;
                break;

            case 1:
                Character2Selected = true;
                break;

            case 2:
                Character3Selected = true;
                break;

            case 3:
                Character4Selected = true;
                break;

            default:
                break;
        }
    }

    public void DeselectAll()
    {
        Character1Selected = false;
        Character2Selected = false;
        Character3Selected = false;
        Character4Selected = false;
    }

    public void LockerAllCharacters()
    {
        Character2Unlocked = false;
        Character3Unlocked = false;
        Character4Unlocked = false;
    }

    public async void QuitGame()
    {
#if VUPLEX_STANDALONE
        await GameObject.FindGameObjectWithTag("canvasWebView").GetComponent<Vuplex.WebView.CanvasWebViewPrefab>().LogOut2();
#endif
        IsLoggedIn = false;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void UnlockCharacter2()
    {
        Character2Unlocked = true;
    }

    public void UnlockCharacter3()
    {
        Character3Unlocked = true;
    }

    public void UnlockCharacter4()
    {
        Character4Unlocked = true;
    }

    public void UnlockBonus1()
    {
        Bonus1Unlocked = true;
    }

    public void UnlockBonus2()
    {
        Bonus2Unlocked = true;
    }

    public void UnlockGame()
    {
        GameUnlocked = true;
    }
}
