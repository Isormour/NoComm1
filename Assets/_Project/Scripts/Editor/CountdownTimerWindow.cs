using UnityEditor;
using UnityEngine;
using System;

public class CountdownTimerWindow : EditorWindow
{
    private const string RemainingKey = "CountdownTimer_Remaining";
    private const string RunningKey = "CountdownTimer_Running";
    private const double TotalTime = 20 * 60 * 60; // 20h w sekundach

    private double remaining;
    private bool running;
    private double lastUpdate;

    [MenuItem("Tools/Countdown Timer")]
    public static void ShowWindow()
    {
        GetWindow<CountdownTimerWindow>("Countdown Timer");
    }

    private void OnEnable()
    {
        remaining = EditorPrefs.GetFloat(RemainingKey, (float)TotalTime);
        running = EditorPrefs.GetBool(RunningKey, false);
        lastUpdate = EditorApplication.timeSinceStartup;
        EditorApplication.update += UpdateTimer;
    }

    private void OnDisable()
    {
        SaveState();
        EditorApplication.update -= UpdateTimer;
    }

    private void UpdateTimer()
    {
        if (running)
        {
            double now = EditorApplication.timeSinceStartup;
            double delta = now - lastUpdate;
            lastUpdate = now;

            remaining -= delta;
            if (remaining <= 0)
            {
                remaining = 0;
                running = false;
            }
            SaveState();
            Repaint();
        }
    }

    private void OnGUI()
    {
        GUILayout.Space(10);
        GUILayout.Label("Pozosta³y czas:", EditorStyles.boldLabel);

        TimeSpan t = TimeSpan.FromSeconds(Math.Max(0, remaining));
        GUILayout.Label(string.Format("{0:D2}:{1:D2}:{2:D2}", t.Hours + t.Days * 24, t.Minutes, t.Seconds),
                        new GUIStyle(EditorStyles.label) { fontSize = 24 });

        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Start"))
        {
            if (!running)
            {
                running = true;
                lastUpdate = EditorApplication.timeSinceStartup;
                SaveState();
            }
        }
        if (GUILayout.Button("Stop"))
        {
            if (running)
            {
                running = false;
                SaveState();
            }
        }
        if (GUILayout.Button("Reset"))
        {
            remaining = TotalTime;
            running = false;
            SaveState();
        }
        GUILayout.EndHorizontal();
    }

    private void SaveState()
    {
        EditorPrefs.SetFloat(RemainingKey, (float)remaining);
        EditorPrefs.SetBool(RunningKey, running);
    }
}