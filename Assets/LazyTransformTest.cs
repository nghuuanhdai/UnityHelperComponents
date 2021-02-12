using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazyTransformTest : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(TestPositionCR());
        StartCoroutine(TestRotationCR());
        StartCoroutine(TestScaleCR());
    }

    private IEnumerator TestScaleCR()
    {
        yield return new WaitForSeconds(1);
        transform.GetLazyTransform().lossyScale = Vector3.zero;
        yield return new WaitForSeconds(0.5f);
        transform.GetLazyTransform().lossyScale = Vector3.one*2;
    }

    private IEnumerator TestRotationCR()
    {
        yield return new WaitForSeconds(1);
        transform.GetLazyTransform().rotation = Quaternion.Euler(45,0,0);
        yield return new WaitForSeconds(0.5f);
        transform.GetLazyTransform().rotation = Quaternion.Euler(45,0,0);
    }

    private IEnumerator TestPositionCR()
    {
        yield return new WaitForSeconds(1);
        transform.GetLazyTransform().position = new Vector3(1,0,0);
        yield return new WaitForSeconds(0.5f);
        transform.GetLazyTransform().position = new Vector3(1,1,0);
    }
}
