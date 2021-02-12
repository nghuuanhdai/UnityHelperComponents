using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CommonCoroutine
{
    public static IEnumerator LerpFactor(float duration, Action<float> callback)
    {
        float t = 0.0f;
        callback(t/duration);
        while (t < duration)
        {
            yield return null;
            t += Time.deltaTime;
            callback(t/duration);
        }
        callback(1);
    }

    public static IEnumerator LerpAnimation(float duration, AnimationCurve timeCurve, Action<float> callback)
    {
        return LerpFactor(duration, f => callback(timeCurve.Evaluate(f)));
    }
}