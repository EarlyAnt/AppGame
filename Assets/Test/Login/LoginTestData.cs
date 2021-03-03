using UnityEngine;

public class LoginTestData : MonoBehaviour
{
    public string Phone;
    public string VerifyCode;

    void Start()
    {
        GameObject.DontDestroyOnLoad(this.gameObject);
    }
}
