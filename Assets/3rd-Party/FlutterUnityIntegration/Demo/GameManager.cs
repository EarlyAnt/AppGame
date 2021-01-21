using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Text messageBox;
    private UnityMessageManager unityMessageManager;

    private void Awake()
    {
        GameObject go = new GameObject("UnityMessageManager");
        DontDestroyOnLoad(go);
        this.unityMessageManager = go.AddComponent<UnityMessageManager>();
        this.unityMessageManager.SceneLoaded += this.OnSceneLoaded;
        this.unityMessageManager.OnMessageReceive += this.OnReceiveMessage;
    }

    private void Start()
    {
    }

    private void OnDestroy()
    {
        this.unityMessageManager.SceneLoaded += this.OnSceneLoaded;
        this.unityMessageManager.OnMessageReceive -= this.OnReceiveMessage;
    }

    private void Update()
    {
    }

    void OnSceneLoaded()
    {
        Debug.Log("<><GameManager.OnSceneLoaded>");
        this.unityMessageManager.SendMessageToFlutter("ha ha, unity loaded ! ! !");
    }

    private void OnReceiveMessage(string message)
    {
        Debug.LogFormat("<><GameManager.OnReceiveMessage>message: {0}", message);
        this.messageBox.text = message;
    }
}
