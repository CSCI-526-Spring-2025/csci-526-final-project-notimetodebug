using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Networking;

public class TelemetryManager : MonoBehaviour
{
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

    private class CumulativeEventLogItem
    {
        public string LevelName { get; set; }
        public EventName EventName { get; set; }
        public string EventData { get; set; }
        public int Count { get; set; }
    }

    public static TelemetryManager Instance;
    public GameObject LevelManagerRef;

    private Guid sessionID;
    private string sessionURL;
    private const string databaseURL = "https://game-telemetry-default-rtdb.firebaseio.com/telemetry_sodifaposid93roj94ek9923oij.json";
    private static List<CumulativeEventLogItem> cumulativeEventLog = new List<CumulativeEventLogItem>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(gameObject);
        }

        sessionID = Guid.NewGuid();
        sessionURL = Application.absoluteURL;
        Log(EventName.GAME_START, $"{Time.time}");
    }

    // For builds like windows
    void OnApplicationQuit()
    {
        OnGameExit();
    }

    // Called directly by the WebGL build when the browser closes
    public void OnGameExit()
    {
        DumpCumulativeEventLog();
        Log(EventName.GAME_END, $"{Time.time}");
    }

    private static IEnumerator SendTelemetry(TelemetryEvent telemetryEvent)
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

    private static string GetLevelName()
    {
        GameObject currentLevelObj = Instance.LevelManagerRef.GetComponent<LevelManager>().CurrentLevelObj;

        if (currentLevelObj != null)
        {
            return currentLevelObj.name;
        }
        else
        {
            return "";
        }
    }

    private static TelemetryEvent GenerateTelemetryEvent(EventName eventName, string eventData, string levelName = null)
    {
        double timestamp = Time.time;
        levelName = levelName == null ? GetLevelName() : levelName;

        return new TelemetryEvent
        {
            eventName = eventName.ToString(),
            eventData = eventData,
            timestamp = timestamp,
            levelName = levelName,
            sessionURL = Instance.sessionURL,
            sessionID = Instance.sessionID.ToString()
        };
    }

    public static void Log(EventName eventName, string eventData)
    {
        if (Application.isEditor) return;

        Instance.StartCoroutine(SendTelemetry(GenerateTelemetryEvent(eventName, eventData)));
    }

    // Logs the number of times an event occurs per level instead of logging each occurrence.
    public static void LogCumulative(EventName eventName, string eventData)
    {
        if (Application.isEditor) return;

        string levelName = GetLevelName();

        CumulativeEventLogItem existingItem = cumulativeEventLog.Find(item =>
            item.LevelName == levelName &&
            item.EventName == eventName &&
            item.EventData == eventData
        );

        if (existingItem != null)
        {
            existingItem.Count++;
        }
        else
        {
            cumulativeEventLog.Add(new CumulativeEventLogItem
            {
                LevelName = levelName,
                EventName = eventName,
                EventData = eventData,
                Count = 1
            });
        }
    }

    private static void DumpCumulativeEventLog()
    {
        List<TelemetryEvent> events = cumulativeEventLog
            .Select(item => {
                return GenerateTelemetryEvent(
                    item.EventName,
                    $"{item.EventData};{item.Count}",
                    item.LevelName);
                })
            .ToList();

        foreach (var telemetryEvent in events)
        {
            Instance.StartCoroutine(SendTelemetry(telemetryEvent));
        }
    }
}
