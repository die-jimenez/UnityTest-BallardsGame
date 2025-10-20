using NativeWebSocket;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class NetworkManager : MonoBehaviour
{
    WebSocket websocket;

    [Header("Connection")]
    public string nombre;
    public string serverUrl;

    [Header("Interface")]
    public GameObject connectButon;
    public TextMeshProUGUI inputName;
    public TextMeshProUGUI inputIP;

    [Header("ServerEvents")]
    [SerializeField] UnityEvent<Color> OnColorChanged = new UnityEvent<Color>();


    //Classes for parse JSON
    [Serializable]
    public class ServerMessage
    {
        public string command;
        public ColorData color;
    }

    [Serializable]
    public class ColorData
    {
        public string r;
        public string g;
        public string b;
    }




    void Start()
    {

    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        if (websocket != null)
        {
            websocket.DispatchMessageQueue();
        }
#endif
    }



    async public void ConectToServer()
    {
        if (serverUrl == "ws://:3000" || serverUrl == "")
        {
            Debug.LogWarning("You didn't set any IP");
            return;
        }
        websocket = new WebSocket(serverUrl);

        websocket.OnOpen += () =>
        {
            Debug.Log("Connection open");
            HideConnectButton();
        };

        websocket.OnError += (e) =>
        {
            Debug.Log("Error! " + e);
        };

        websocket.OnClose += (e) =>
        {
            Debug.Log("Connection closed");
            ShowConnectButton();
        };

        websocket.OnMessage += (bytes) =>
        {
            string message = System.Text.Encoding.UTF8.GetString(bytes);
            Debug.Log(message);
            ServerEvents(message);
        };

        // waiting for messages
        await websocket.Connect();
    }

    void ServerEvents(string json)
    {
        try
        {
            ServerMessage message = JsonUtility.FromJson<ServerMessage>(json);

            switch (message.command)
            {
                case "ChangeColor":
                    // Parsear de string a float
                    float r = float.Parse(message.color.r) / 255f;
                    float g = float.Parse(message.color.g) / 255f;
                    float b = float.Parse(message.color.b) / 255f;

                    Color color = new Color(r, g, b);
                    OnColorChanged?.Invoke(color);
                    break;
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Parse error: " + e.Message);
        }
    }

    #region Interface
    public void ReadTextInputIP(string _inputData)
    {
        serverUrl = "ws://" + _inputData + ":3000";
        Debug.Log("The connection will connect to: " + _inputData);
    }

    public void ReadTextInputName(string _inputData)
    {
        nombre = _inputData;
        Debug.Log("The ID of this player will be: " + _inputData);
    }

    public void DisconnectToServer()
    {
        if (websocket != null)
        {
            OnApplicationQuit();
        }
    }

    void HideConnectButton()
    {
        connectButon.SetActive(false);
    }

    void ShowConnectButton()
    {
        connectButon.SetActive(true);
    }
    #endregion



    private async void OnApplicationQuit()
    {
        await websocket.Close();
    }
}
