using System.Collections;
using System.Collections.Generic;
using UnityEditor.Connect;
using UnityEngine;

public class UnityConnectSession {
    
    static UnityConnectSession _instance = new UnityConnectSession();

    public static UnityConnectSession instance {
        get => _instance;
    }

    public string GetAccessToken() {
        return UnityConnect.instance.GetAccessToken();
    }

    public string GetEnvironment() {
        return UnityConnect.instance.GetEnvironment();
    }

    public void ShowLogin() {
        UnityConnect.instance.ShowLogin();
    }
}
