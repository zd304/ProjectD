﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UILogin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickLogin()
    {
        Operation.UserInfo userInfo = new Operation.UserInfo();
        userInfo.username = username.text;
        userInfo.password = password.text;
        PhotonEngine.Instance.DoRequest<Operation.UserInfo>(Operation.OperationCode.Login, userInfo);
    }

    public void JumpToRegist()
    {
        SceneManager.LoadScene("Regist");
    }

    public InputField username;
    public InputField password;
}
