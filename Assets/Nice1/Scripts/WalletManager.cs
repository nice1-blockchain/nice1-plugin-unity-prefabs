using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class WalletManager : Singleton<WalletManager>
{
    [Header("License parameters")]
    public string author;
    public string category;
    public string licenseName;

    // Create a const string for each NFT with the idata name.
    // Use method CheckNFT to implement the corresponding behaviour.
    public const string NFT_EXAMPLE = "EXAMPLE";

    // Add NFTs to list to view in inspector.
    [HideInInspector]
    public List<string> nftList = new List<string>()
    {
        NFT_EXAMPLE
    };

    [HideInInspector] public bool checkLicense;

    private WebRequestResults lastRequest;

    public bool TryingLogin { get; private set; }
    public WalletAccount CurrentAccount { get; private set; }

    private void Awake()
    {
        CurrentAccount = new WalletAccount();
    }

    public void SetAccount(string name)
    {
        if (!GameManager.Instance.IsLoggedIn)
        {
            TryingLogin = false;

            CurrentAccount.Initialize(name, null, null, null, false, null);

            Debug.Log("Login successful");

            GameManager.Instance.IsLoggedIn = true;

            MainMenuManager.Instance.SetLoggedInMenu();

            /*if (checkLicense)*/
            StartCoroutine(SearchAssetsByOwner(name));
            //else MainMenuManager.Instance.SetF2PMenu();
        }
    }

    public void CheckNewNFTs()
    {
        StartCoroutine(SearchAssetsByOwner(CurrentAccount.name));
    }

    /// <summary>
    /// Searches the assets by owner.
    /// </summary>
    /// <param name="owner">The owner.</param>
    private IEnumerator SearchAssetsByOwner(string owner)
    {
        // https://jungle3.api.simpleassets.io/doc/
        string url = "https://jungle3.api.simpleassets.io/v1/assets/search?author=" + author + "&owner=" + owner + "&category=" + category + "&page=1&limit=1000&sortField=assetId&sortOrder=asc";

        Debug.Log("Waiting for request...");

        yield return StartCoroutine(GetRequest(url));

        if (lastRequest.results.Count > 0)
        {
            for (int i = 0; i < lastRequest.results.Count; i++)
            {
                //Debug.Log(lastRequest.results[i].idata.name);
                if (checkLicense) CheckLicense(lastRequest.results[i].idata.name);
                CheckNFT(lastRequest.results[i].idata.name);
            }
        }

        MainMenuManager.Instance.SetNFTLoadedMenu(checkLicense);
    }

    /// <summary>
    /// Gets the request.
    /// </summary>
    /// <param name="uri">The uri.</param>
    /// <returns>An IEnumerator.</returns>
    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    lastRequest = JsonUtility.FromJson<WebRequestResults>(webRequest.downloadHandler.text);

                    break;

                default:
                    break;
            }
        }
    }

    #region Errors
    public void ShowLoginError(string errorMessage)
    {
        TryingLogin = false;

        string errorText = "There was a problem logging into your account. Make sure you are logged into Scatter."
                         + "\nError: " + errorMessage;

        Debug.Log("There was a problem logging into your account. Make sure you are logged into Scatter.");
        Debug.Log("Error: " + errorMessage);
    }

    public void ShowApiError(string errorMessage)
    {
        TryingLogin = false;

        string errorText = "There was a problem communicating with the API. Please try again."
                         + "\nError: " + errorMessage;

        Debug.Log("There was a problem communicating with the API. Please try again.");
        Debug.Log("Error: " + errorMessage);
    }
    #endregion

    #region NFT 
    public void CheckLicense(string NFT_idata_name)
    {
        string key = NFT_idata_name;

        if (key == licenseName)
        {
            GameManager.Instance.UnlockGame();
            Debug.Log("License");
        }
    }

    public void CheckNFT(string NFT_idata_name)
    {
        string key = NFT_idata_name;

        switch (key)
        {
            // Implement a case for each NFT
            case NFT_EXAMPLE:
                Debug.Log("NFT example found.");
                break;

            default:
                break;
        }
    }

    #endregion
}

#region Data Model

[System.Serializable]
public class WalletAccount
{
    public void Initialize(string name, string authority, string publicKey, string blockChain, bool isHardware, string chainID)
    {
        this.name = name;
        this.authority = authority;
        this.publicKey = publicKey;
        this.blockChain = blockChain;
        this.isHardware = isHardware;
        this.chainID = chainID;
    }

    public string name;
    public string authority;
    public string publicKey;
    public string blockChain;
    public bool isHardware;
    public string chainID;
}

[System.Serializable]
public class WebRequestResults
{
    public List<WebResultContainer> results;
}

[System.Serializable]
public class WebResultContainer
{
    public string assetId;
    public string author;
    public string owner;
    public string category;
    public string control;
    public ImmutableData idata;
    public MutableData mdata;
}

[System.Serializable]
public class ImmutableData
{
    public string name;
    public string img;
}

[System.Serializable]
public class MutableData
{
    public string name;
}

#endregion