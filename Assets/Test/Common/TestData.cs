using UnityEngine;

public class TestData : MonoBehaviour
{
    public string Phone;
    public string VerifyCode;
    public int Coin;

    void Start()
    {
        GameObject.DontDestroyOnLoad(this.gameObject);
    }
}
