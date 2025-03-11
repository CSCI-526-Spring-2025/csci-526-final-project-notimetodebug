using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class TelemetryManager : MonoBehaviour
{
    public enum EventName
    {
        PLAYER_DAMAGED,
        PLAYER_SHOT_BULLET,
        COLLECTIBLE_PICKED_UP,
        MOVEMENT_STATE_CHANGE,
        GAME_START,
        LEVEL_START,
        GAME_END,
        PLAYER_DIED
    }

    private Guid sessionID;
    private string sessionURL;

    private const string databaseURL = "https://game-telemetry-default-rtdb.firebaseio.com/telemetry_sodifaposid93roj94ek9923oij.json";

    public GameObject LevelManagerRef;

    void Start()
    {
        sessionID = Guid.NewGuid();
        //LevelManagerRef = GameObject.FindGameObjectsWithTag("LevelManager").FirstOrDefault();
        sessionURL = Application.absoluteURL;

        Log(EventName.GAME_START, $"{Time.time}");
    }

    void OnApplicationQuit()
    {
        Log(EventName.GAME_END, $"{Time.time}");
    }

    [Serializable]
    public class TelemetryEvent
    {
        public EventName eventName;
        public string eventData;
        public double timestamp;
        public string levelName;
        public string sessionURL;
        public string sessionID;
    }

    public void Log(EventName eventName, string eventData)
    {
        //if (Application.isEditor) return;

        double timestamp = Time.time;
        string levelName = LevelManagerRef.GetComponent<LevelManager>().CurrentLevelObj?.name;

        // Stringify the event data

        TelemetryEvent body = new TelemetryEvent
        {
            eventName = eventName,
            eventData = eventData,
            timestamp = timestamp,
            levelName = levelName,
            sessionURL = sessionURL,
            sessionID = sessionID.ToString()
        };

        string json = JsonUtility.ToJson(body);

        // Send network request to database

        using (UnityWebRequest www = UnityWebRequest.Post(databaseURL, json, "application/json"))
        {
            var request = www.SendWebRequest();
            request.completed += (result) =>
            {
                Debug.Log("Telemetry event sent");
            };
        }
    }
}
