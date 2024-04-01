using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ServerButton : MonoBehaviour
{
    [SerializeField] private Button btn_Host;
    [SerializeField] private Button btn_Client;

    private void Awake() {
        btn_Host.onClick.AddListener(() => {
            NetworkManager.Singleton.StartHost();
        });
        btn_Client.onClick.AddListener(() => {
            NetworkManager.Singleton.StartClient();
        });
    }

}
