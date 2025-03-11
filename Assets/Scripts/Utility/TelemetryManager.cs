using System;
using System.Collections;
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

    private void Awake()
    {
        sessionID = Guid.NewGuid();
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
        public string eventName;
        public string eventData;
        public double timestamp;
        public string levelName;
        public string sessionURL;
        public string sessionID;
    }

    private IEnumerator SendTelemetry(TelemetryEvent telemetryEvent)
    {
        string json = JsonUtility.ToJson(telemetryEvent);
        using (UnityWebRequest www = UnityWebRequest.Post(databaseURL, json, "application/json"))
        {
            yield return www.SendWebRequest();

            if (www.isDone)
            {
                Debug.Log("Telemetry sent successfully");
            }
        }
    }

    public void Log(EventName eventName, string eventData)
    {
        if (Application.isEditor) return;

        double timestamp = Time.time;
        GameObject currentLevelObj = LevelManagerRef.GetComponent<LevelManager>().CurrentLevelObj;

        string levelName = "";

        if (currentLevelObj != null)
        {
            levelName = LevelManagerRef.GetComponent<LevelManager>().CurrentLevelObj?.name;
        }

        StartCoroutine(SendTelemetry(new TelemetryEvent
        {
            eventName = eventName.ToString(),
            eventData = eventData,
            timestamp = timestamp,
            levelName = levelName,
            sessionURL = sessionURL,
            sessionID = sessionID.ToString()
        }));
    }
}
