using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using UnityEngine.UI;

public class DatabaseManager : MonoBehaviour
{
    public InputField Name;
    private string userID;

    private DatabaseReference dbReference;
    void Start()
    {
        userID = SystemInfo.deviceUniqueIdentifier;
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void CreateUser()
    {
        User newUser = new User(Name.text);
        string json = JsonUtility.ToJson(newUser);

        dbReference.Child("users").Child(userID).SetRawJsonValueAsync(json);
    }
}
