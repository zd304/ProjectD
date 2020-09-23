using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIRegist : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickRegist()
    {
        Operation.UserInfo userInfo = new Operation.UserInfo();
        userInfo.username = username.text;
        userInfo.password = password.text;
        PhotonEngine.Instance.DoRequest<Operation.UserInfo>(Operation.OperationCode.Regist, userInfo);
    }

    public void BackToLogin()
    {
        EventSystem.Dispatch(EventID.BackLogin);
    }

    public InputField username;
    public InputField password;
}
