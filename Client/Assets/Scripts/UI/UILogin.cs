using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public InputField username;
    public InputField password;
}
