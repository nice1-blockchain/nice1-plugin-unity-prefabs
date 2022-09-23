using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WalletManager))]
public class WalletManagerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("NFTs", EditorStyles.boldLabel);

        GUI.enabled = false;

        foreach(string nftName in WalletManager.Instance.nftList)
        {
            EditorGUILayout.TextField("", nftName);
        }

        GUI.enabled = true;
    }
}
