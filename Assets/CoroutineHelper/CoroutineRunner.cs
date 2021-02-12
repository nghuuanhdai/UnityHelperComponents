using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineRunner : MonoBehaviour
{
    public static CoroutineRunner CreateCoroutineRunner(
        bool dontDestroyOnLoad
    ){
        var gameObjectHolder = new GameObject("Coroutine-Runner");
        if(dontDestroyOnLoad)
            DontDestroyOnLoad(gameObjectHolder);
        return gameObjectHolder.AddComponent<CoroutineRunner>();
    }
}