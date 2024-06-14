using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class AccountManager : MonoBehaviour
{
    private string signUpUrl = "http://localhost:7755/gangnimal/signup";
    private string signInUrl = "http://localhost:7755/gangnimal/signin";
    private string updateRecordUrl = "http://localhost:7755/gangnimal/record/update/";
    private string findRecordUrl = "http://localhost:7755/gangnimal/record/";

    //for Sign In
    public TMP_InputField nickNameInput;
    public TMP_InputField passwordInput;

    //for Sign Up
    public TMP_InputField nickNameInputSU;
    public TMP_InputField passwordInputSU;

    //for record
    public TMP_Text win;
    public TMP_Text lose;
    public TMP_Text winRate;


    public void SignUp()
    {
        StartCoroutine(signUpRequest());
    }

    public void SignIn()
    {
        StartCoroutine(signInRequest());
    }

    public void UpdateBattleRecord(string result)
    {
        updateRecordUrl += UserInfo.Instance.userName + "/" + result;
        Debug.Log(updateRecordUrl);
        StartCoroutine(RecordUpdateRequest());
        updateRecordUrl = "http://localhost:7755/gangnimal/record/update/";
    }

    public void FindMyBattleRecord()
    {
        findRecordUrl += UserInfo.Instance.userName;
        StartCoroutine(FindRecordRequest());
        findRecordUrl = "http://localhost:7755/gangnimal/record/";
    }

    IEnumerator signUpRequest()
    {
        Account a = new Account();
        a.nickName = nickNameInputSU.text;
        a.password = passwordInputSU.text;
        string json = JsonUtility.ToJson(a);

        UnityWebRequest www = UnityWebRequest.Post(signUpUrl, json, "application/json");

        yield return www.SendWebRequest();

        switch (www.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
                Debug.Log("Error: " + www.error);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.Log("HTTP Error: " + www.error);
                break;
            case UnityWebRequest.Result.Success:
                if (www.downloadHandler.text == "true")
                {
                    Debug.Log("Create Account Success!");
                    //UserInfo.Instance.userName = a.nickName;
                }
                else
                {
                    Debug.Log("Account creation Failed! The nickname you entered already exists.");
                }
                    
                break;
        }
    }

    IEnumerator signInRequest()
    {
        Account a = new Account();
        a.nickName = nickNameInput.text;
        a.password = passwordInput.text;
        string json = JsonUtility.ToJson(a);

        UnityWebRequest www = UnityWebRequest.Post(signInUrl, json, "application/json");

        yield return www.SendWebRequest();

        switch (www.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
                Debug.Log("Error: " + www.error);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.Log("HTTP Error: " + www.error);
                break;
            case UnityWebRequest.Result.Success:
                if (www.downloadHandler.text == "true")
                {
                    Debug.Log("Success! Login...");
                    UserInfo.Instance.userName = a.nickName;
                    Debug.Log("Hi! "+ UserInfo.Instance.userName);
                    SceneManager.LoadScene("MainMenu");
                }
                else
                {
                    Debug.Log("Failed! Invalid Account");
                }
                    
                break;
        }
    }

    IEnumerator RecordUpdateRequest()
    {
        UnityWebRequest www = UnityWebRequest.Get(updateRecordUrl);
        www.SetRequestHeader("Accept", "application/json");
        yield return www.SendWebRequest();

        switch (www.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
                Debug.Log("Error: " + www.error);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.Log("HTTP Error: " + www.error);
                break;
            case UnityWebRequest.Result.Success:
                Debug.Log("Update Record...");
                break;
        }
    }

    IEnumerator FindRecordRequest()
    {
        UnityWebRequest www = UnityWebRequest.Get(findRecordUrl);
        www.SetRequestHeader("Accept", "application/json");
        yield return www.SendWebRequest();

        switch (www.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
                Debug.Log("Error: " + www.error);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.Log("HTTP Error: " + www.error);
                break;
            case UnityWebRequest.Result.Success:
                string json = www.downloadHandler.text;
                ParseResult(json);
                break;
        }
    }

    public void ParseResult(string json)
    {
        Account a = JsonUtility.FromJson<Account>(json);
        int winCount = int.Parse(a.win);
        int loseCount = int.Parse(a.lose);

        Debug.Log(UserInfo.Instance.userName + " battle record: " + "win: " + a.win + " / lose: " + a.lose);

        if(winCount == 0 && loseCount == 0)
        {
            Debug.Log("No record");
            return;
        }

        win.text = a.win;
        lose.text = a.lose;
        winRate.text = ""+(int.Parse(a.win) * 100.0 / (int.Parse(a.win) + int.Parse(a.lose))) + "%";
    }

}
