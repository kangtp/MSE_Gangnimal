using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Unity.Services.Authentication;
using Unity.Services.Core;

public class AccountManager : MonoBehaviour
{
    //url for sign up
    private string signUpUrl = "http://localhost:7755/gangnimal/signup";

    //url for sign in
    private string signInUrl = "http://localhost:7755/gangnimal/signin";

    //url for update record
    private string updateRecordUrl = "http://localhost:7755/gangnimal/record/update/";

    //url for find record
    private string findRecordUrl = "http://localhost:7755/gangnimal/record/";

    //Input field for Sign In
    public TMP_InputField nickNameInput;
    public TMP_InputField passwordInput;

    //Input field for Sign Up
    public TMP_InputField nickNameInputSU;
    public TMP_InputField passwordInputSU;

    //Text for show record
    public TMP_Text name;
    public TMP_Text win;
    public TMP_Text lose;
    public TMP_Text winRate;

    //message ui. ex)invalid account, fail sign up, etc...
    public GameObject message;

    private async void start() {
        await UnityServices.InitializeAsync(); //request

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("sigend in  " + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public void SignUp()
    {
        StartCoroutine(signUpRequest());
    }

    public void SignIn()
    {
        StartCoroutine(signInRequest());
    }

    //Update wins and losses based on results.
    //result : "win" or "lose"
    public void UpdateBattleRecord(string result)
    {
        updateRecordUrl += UserInfo.Instance.userName + "/" + result;
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
                //sign up success
                if (www.downloadHandler.text == "true")
                {
                    Debug.Log("Create Account Success!");
                    StartCoroutine(MessageWindow(1));   //show success message ui
                }
                //sign up fail
                else
                {
                    Debug.Log("Account creation Failed! The nickname you entered already exists.");
                    StartCoroutine(MessageWindow(2));   //show fail message ui
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
                //sign in success
                if (www.downloadHandler.text == "true")
                {
                    Debug.Log("Success! Login...");
                    start();
                    UserInfo.Instance.userName = a.nickName;
                    PlayerPrefs.SetString("name",UserInfo.Instance.userName);
                    Debug.Log("Hi! "+ UserInfo.Instance.userName);
                    SceneManager.LoadScene("MainMenu");
                }
                //sign in fail
                else
                {
                    Debug.Log("Failed! Invalid Account");
                    StartCoroutine(MessageWindow(0));   //show fail message ui
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
                ParseResult(json);  //show user's battle record in UI
                break;
        }
    }

    public void ParseResult(string json)
    {
        Account a = JsonUtility.FromJson<Account>(json);
        int winCount = int.Parse(a.win);
        int loseCount = int.Parse(a.lose);

        Debug.Log(UserInfo.Instance.userName + " battle record: " + "win: " + a.win + " / lose: " + a.lose);

        name.text = UserInfo.Instance.userName;

        //If there is no record, return
        if (winCount == 0 && loseCount == 0)
        {
            Debug.Log("No record");
            return;
        }

        //If there is record, show the record
        win.text = a.win;
        lose.text = a.lose;
        winRate.text = ""+(int.Parse(a.win) * 100.0 / (int.Parse(a.win) + int.Parse(a.lose))) + "%";
    }

    //Show message in UI
    //num == 0, sign in fail / num == 1, sign up success / num == 2, sign up fail
    IEnumerator MessageWindow(int num)
    {
        message.transform.GetChild(num).gameObject.SetActive(true);
        yield return new WaitForSeconds(1.2f);
        message.transform.GetChild(num).gameObject.SetActive(false);
    }
}
