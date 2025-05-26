using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Firebase;
using Firebase.Database;

public class RealtimeDatabaseManager : MonoBehaviour
{
    public static RealtimeDatabaseManager instance;

    public string firebaseRealTimeDatabaseURL = "";

    // DatabaseReference databaseReference;

    void Awake()
    {
        if (RealtimeDatabaseManager.instance == null)
		{
			RealtimeDatabaseManager.instance = this;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
        
    }
    // Start is called before the first frame update
    void Start()
    {
        // AppOptions options = new AppOptions();
        // options.DatabaseUrl = new Uri(firebaseRealTimeDatabaseURL);
        // FirebaseApp app = FirebaseApp.Create(options);
        Debug.Log("Nik Log Is the the firebase Url call");

        // databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                //   app = Firebase.FirebaseApp.DefaultInstance;
                // Set a flag here to indicate whether Firebase is ready to use by your app.


            }
            else
            {
                Debug.LogError(System.String.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
