using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UILogin : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        GetTabDown();
    }

    private void GetTabDown()
    {
#if UNITY_STANDALONE_WIN
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (username.isFocused)
            {
                password.Select();
            }
            else if (password.isFocused)
            {
                username.Select();
            }
        }
#endif
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
        EventSystem.Dispatch(EventID.ClickRegist);
    }

    public InputField username;
    public InputField password;
}
