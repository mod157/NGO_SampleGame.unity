using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button btn_Host;
    [SerializeField] private Button btn_Client;
    [SerializeField] private Button btn_Cencel;
    [SerializeField] private Button btn_Exit;
    [SerializeField] private ProgressRotate progressRotate;
    
    private void Awake() {
        btn_Host.onClick.AddListener(() => {
            NetworkManager.Singleton.StartHost();
            ReadyConnected();
        });
        btn_Client.onClick.AddListener(() => {
            NetworkManager.Singleton.StartClient();
            ReadyConnected();
        });
        btn_Cencel.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            ResetUI();
        });
        btn_Exit.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            Application.Quit();
        });
    }

    public void ReadyConnected()
    {
        btn_Host.gameObject.SetActive(false);
        btn_Client.gameObject.SetActive(false);
        btn_Cencel.gameObject.SetActive(true);
        progressRotate.OnProgressRotate(ProgressEnd);
    }

    private void GameStartUI()
    {
        progressRotate.gameObject.SetActive(false);
        btn_Exit.gameObject.SetActive(false);
        btn_Cencel.gameObject.SetActive(false);
    }

    public void ResetUI()
    {
        btn_Host.gameObject.SetActive(true);
        btn_Client.gameObject.SetActive(true);
        btn_Exit.gameObject.SetActive(true);
        btn_Cencel.gameObject.SetActive(false);
        progressRotate.gameObject.SetActive(false);
    }

    private void ProgressEnd()
    {
        if (progressRotate.IsExit)
        {
            GameStartUI();

        }else
        {
            ResetUI();
        }
    }

    public ProgressRotate Progress => progressRotate;
}
