using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class DatabaseMgr
{
    private string userID;
    public bool DBLogin { get; set; } = false;
    public Dictionary<string, object> jsStrList = new Dictionary<string, object>();
    private static DatabaseMgr instance;
    public static DatabaseMgr Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DatabaseMgr();
                return instance;
            }
            return instance;
        }
    }
    public DatabaseReference Reference { get; set; }
    public void CheckUserData(string userID)
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://testproject-60247446.firebaseio.com/");
        Reference = FirebaseDatabase.DefaultInstance.GetReference("Users");
        // ID 확인    
        Reference.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                this.userID = userID;
                if (!snapshot.HasChild(userID))
                {
                    //db에 추가
                    Reference.SetValueAsync(userID);
                }
                else
                {
                    DBLoadData();
                }
            }
            DBLogin = true;
        });
    }
    public bool CheckDBLogin()
    {
       return !string.IsNullOrEmpty(userID);
    }
    public void DBLoadData()
    {
        Reference.Child(userID).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            { // 성공적으로 데이터를 가져왔으면
                DataSnapshot snapshot = task.Result;
                foreach(DataSnapshot data in snapshot.Children)
                {
                    jsStrList.Add(data.Key, data.Value);
                }
                /* 불러온 데이터 체크용
                foreach (string data in jsStrList.Keys)
                {
                    Debug.Log(data);
                }
                */
            }
            else
            {
                Debug.Log("데이터 불러오기 오류");
            }
        });
    }
    public void DBSaveData<T>(string category, T obj)
    {
        string _jsStr = JsonConvert.SerializeObject(obj);
        Reference.Child(userID).Child(category).SetRawJsonValueAsync(_jsStr);
    }
    public T DBLoadData<T>(string category)
    {
        string _convertToJs = JsonConvert.SerializeObject(jsStrList[category]);
        return JsonConvert.DeserializeObject<T>(_convertToJs);
    }
    public bool CheckDBExistUser(string category)
    {
        if (jsStrList.ContainsKey(category))
            return true;
        else
            return false;
    }
    public void LocalSaveData<T>(string fileName, T obj)
    {
        string _jsStr = JsonConvert.SerializeObject(obj);
        File.WriteAllText($"{Application.persistentDataPath}/{fileName}.json", _jsStr);
    }
    public T LocalLoadData<T>(string fileName)
    {
        string _jsStr = File.ReadAllText($"{Application.persistentDataPath}/{fileName}.json");
        return JsonConvert.DeserializeObject<T>(_jsStr);
    }
    public bool CheckLocalExistFile(string fileName)
    {
        return File.Exists($"{Application.persistentDataPath}/{fileName}.json");
    }
}