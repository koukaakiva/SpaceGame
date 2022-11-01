using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DataStructures;

//TODO seperate logic from the monobehaviour
public class GameTime : MonoBehaviour {
    public static GameTime instance { get; private set; }
    public static DateTime gameTime = new DateTime(3000, 1, 1, 0, 0, 0);
    private static bool isPaused = true;
    private static float timeScale = 1f;
    public static PriorityQueue<Action, DateTime> timedEvents = new PriorityQueue<Action, DateTime>();

    public static Action<float> onGameTick;
    public static Action onGamePaused;
    public static Action onGameUnpaused;

    private void Awake(){ 
        if (instance != null && instance != this) Destroy(this); 
        else instance = this;
        DontDestroyOnLoad(FindObjectOfType<GameTime>().gameObject.transform.root);
    }

    public void Update(){
        if(isPaused == false){
            gameTime += TimeSpan.FromSeconds(Time.deltaTime * timeScale);
            onGameTick?.Invoke(Time.deltaTime * timeScale);
            while(timedEvents.Count > 0 && timedEvents.PeekPriority() <= gameTime){
                timedEvents.Dequeue().Invoke();
            }
        }
    }

    public static void Pause(){
        isPaused = true;
        onGamePaused?.Invoke();
    }

    public static void Unpause(){
        isPaused = false;
        onGameUnpaused?.Invoke();
    }

    public static void SetTimeScale(float multiplier){
        timeScale = multiplier;
    }

    public static void RunEventAtTime(Action action, float timeSpan){
        timedEvents.Enqueue(action, gameTime + TimeSpan.FromSeconds(timeSpan));
    }

    public static void RunEventAtTime(Action action, DateTime time){
        timedEvents.Enqueue(action, time);
    }
}
