using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private GameObject panel_GameOver;
    [SerializeField] private TextMeshProUGUI tmp_GameOver;
    
    private void Awake() {
        btn_Host.onClick.AddListener(() => {
            NetworkManager.Singleton.StartHost();
            ReadyConnectedUI();
        });
        btn_Client.onClick.AddListener(() => {
            NetworkManager.Singleton.StartClient();
            ReadyConnectedUI();
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

    public void ReadyConnectedUI()
    {
        Debug.Log("ReadyUI");
        btn_Host.gameObject.SetActive(false);
        btn_Client.gameObject.SetActive(false);
        panel_GameOver.SetActive(false);
        
        progressRotate.OnProgressRotate(ProgressEnd);
        btn_Cencel.gameObject.SetActive(true);
    }

    private void GameStartUI()
    {
        panel_GameOver.SetActive(false);
        progressRotate.gameObject.SetActive(false);
        btn_Exit.gameObject.SetActive(false);
        btn_Cencel.gameObject.SetActive(false);
    }

    public void GameEndUI(string text, Action action)
    {
        panel_GameOver.SetActive(true);
        tmp_GameOver.text = text;
        StartCoroutine(AutoReset(action));
    }

    public void ResetUI()
    {
        btn_Host.gameObject.SetActive(true);
        btn_Client.gameObject.SetActive(true);
        btn_Exit.gameObject.SetActive(true);
        btn_Cencel.gameObject.SetActive(false);
        progressRotate.gameObject.SetActive(false);
        panel_GameOver.SetActive(false);
    }

    private void ProgressEnd()
    {
        if (GameManager.Instance.IsGameStart)
        {
            GameStartUI();

        }else
        {
            ResetUI();
        }
    }

    IEnumerator AutoReset(Action action)
    {
        yield return new WaitForSeconds(5f);
        if(action != null)
            action.Invoke();
    }

    public ProgressRotate Progress => progressRotate;
}
