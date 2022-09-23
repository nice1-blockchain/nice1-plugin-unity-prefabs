using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour
{
    public Transform InitialSpawn;
    public GameObject CharacterSelectionPanel;
    public GameObject SkinSelectionPanel;

    [Space(10)]
    public List<GameObject> CharacterPrefabs;
    public List<Button> CharacterButtonList;
    public List<Button> SkinButtonList;

    private GameObject _currentCharacter;
    private int _currentCharacterIndex;
    private Vector3 _currentPosition;
    private bool _facingRight;
    public UIPanel nftPanel;

    private void Start()
    {
        if (GameManager.Instance.CharacterSelected)
        {
            if (GameManager.Instance.Character1Selected)
            {
                SelectCharacter(0);
            }
            else if (GameManager.Instance.Character2Selected)
            {
                SelectCharacter(1);
            }
            else if (GameManager.Instance.Character3Selected)
            {
                SelectCharacter(2);
            }
            else if (GameManager.Instance.Character4Selected)
            {
                SelectCharacter(3);
            }
        }

        if (!GameManager.Instance.Character1Unlocked)
        {
            CharacterButtonList[0].interactable = false;
        }

        if (!GameManager.Instance.Character2Unlocked)
        {
            CharacterButtonList[1].interactable = false;
        }

        if (!GameManager.Instance.Character3Unlocked)
        {
            CharacterButtonList[2].interactable = false;
        }

        if (!GameManager.Instance.Character4Unlocked)
        {
            CharacterButtonList[3].interactable = false;
        }
    }

    public void SelectCharacter(int characterIndex)
    {
        if (_currentCharacter != null)
        {
            // Save position and deactivate previous character
            _currentPosition = _currentCharacter.transform.position;

            CharacterButtonList[_currentCharacterIndex].interactable = true;
            _currentCharacter.gameObject.SetActive(false);
        }
        else
        {
            //_currentPosition = InitialSpawn.position;
            //_facingRight = true;
        }

        // Select new character
        _currentCharacter = CharacterPrefabs[characterIndex];
        CharacterButtonList[characterIndex].interactable = false;
        _currentCharacterIndex = characterIndex;

        GameManager.Instance.SelectCharacter(characterIndex);

        // Set position and activate new character
        _currentCharacter.transform.position = _currentPosition;
        _currentCharacter.transform.rotation = Quaternion.identity;

        //_currentCharacter.gameObject.SetActive(true);

        if (!GameManager.Instance.CharacterSelected)    // Enters when starting the game
        {
            //TransitionManager.Instance.DoEnterTransition();
            //PauseManager.Instance.HideCharacterSelector();
            //TimerManager.Instance.StartTimer();
            CharacterSelectionPanel.SetActive(false);
            SkinSelectionPanel.SetActive(true);
            SetSkinSprites(_currentCharacter.GetComponent<SpriteRenderer>().sprite);
            GameManager.Instance.CharacterSelected = true;
        }

        //PauseManager.Instance.HidePausePanel();
    }

    public void SelectSkin(int skinIndex)
    {


        SkinSelectionPanel.SetActive(false);
    }

    public void SetSkinSprites(Sprite sprite)
    {
        foreach (Button button in SkinButtonList)
        {
            Image image = button.GetComponent<Image>();
            image.sprite = sprite;
        }
    }


}
