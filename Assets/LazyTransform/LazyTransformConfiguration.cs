using UnityEngine;
public class LazyTransformConfiguration : MonoBehaviour
{
    public enum UpdateMode{
    update,
    fixedUpdate,
    coroutine
    }

    public UpdateMode Mode;
    public float PositionLazySpeed = 10.0f;
    public float ScaleLazySpeed = 10.0f;
    public float RotationLazySpeed = 720.0f;
}