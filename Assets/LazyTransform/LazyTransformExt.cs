using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LazyTransformExt
{
    public class Behaviour : MonoBehaviour
    {
        public LazyTransformConfiguration Conf{
            get
            {
                var _conf = GetComponent<LazyTransformConfiguration>();
                return _conf??gameObject.AddComponent<LazyTransformConfiguration>();
            }
        }
        private Vector3 _targetPosition;
        private Vector3 _targetScale;
        private Quaternion _targetRotation;
        private Vector3 _targetLocalPosition;
        private Vector3 _targetLocalScale;
        private Quaternion _targetLocalRotation;
        private bool positionWorldMode = false;
        private bool rotationWorldMode = false;
        private bool scaleWorldMode = false;

        public Vector3 position{
            set{
                _targetPosition = value;
                positionWorldMode = true;
            }
            get => _targetPosition;
        }
        public Vector3 lossyScale{
            set{
                _targetScale = value;
                scaleWorldMode = true;
            }
            get => _targetScale;
        }
        public Quaternion rotation{
            set{
                _targetRotation = value;
                rotationWorldMode = true;
            }
            get => _targetRotation;
        }
        public Vector3 localPosition{
            set{
                _targetLocalPosition = value;
                positionWorldMode = false;
            }
            get => _targetLocalPosition;
        }
        public Vector3 localScale{
            set{
                _targetLocalScale = value;
                scaleWorldMode = false;
            }
            get => _targetLocalScale;
        }
        public Quaternion localRotation{
            set{
                _targetLocalRotation = value;
                rotationWorldMode = false;
            }
            get => _targetLocalRotation;
        }

        private void OnEnable() {
            StartCoroutine(CrUpdate());
        }

        private IEnumerator CrUpdate()
        {
            while (true)
            {
                yield return null;
                if(Conf.Mode == LazyTransformConfiguration.UpdateMode.coroutine)
                    InternalUpdate(Time.deltaTime);
            }
        }

        private void Update() {
            if(Conf.Mode == LazyTransformConfiguration.UpdateMode.update)
                InternalUpdate(Time.deltaTime);
        }

        private void FixedUpdate() {
            if(Conf.Mode == LazyTransformConfiguration.UpdateMode.fixedUpdate)
                InternalUpdate(Time.fixedDeltaTime);
        }

        private void InternalUpdate(float deltaTime)
        {
            bool updated = false;
            if(positionWorldMode)
                updated |= UpdateWorldPosition(deltaTime);
            else
                updated |= UpdateLocalPosition(deltaTime);
            if(rotationWorldMode)
                updated |= UpdateWorldRotation(deltaTime);
            else
                updated |= UpdateLocalRotation(deltaTime);
            if(scaleWorldMode)
                updated |= UpdateWorldScale(deltaTime);
            else
                updated |= UpdateLocalScale(deltaTime);
            if(!updated)
                Destroy(this);
        }

        private bool UpdateLocalScale(float deltaTime)
        {
            var lastScale = transform.localScale;
            var newScale = Vector3.MoveTowards(lastScale, localScale, deltaTime * Conf.ScaleLazySpeed);
            transform.localScale = newScale;
            return (newScale - localScale).magnitude > Mathf.Epsilon;
        }

        private bool UpdateWorldScale(float deltaTime)
        {
            var localTargetScale = lossyScale;
            var objectParent = transform.parent;
            var speedCompensate = 1.0f;
            if(objectParent != null)
            {
                localTargetScale = objectParent.InverseTransformVector(lossyScale);
                speedCompensate = objectParent.InverseTransformVector(Vector3.one).magnitude;
            }
            if(speedCompensate == 0)
                speedCompensate = 1;
            var lastScale = transform.localScale;
            var newScale = Vector3.MoveTowards(lastScale, localTargetScale, deltaTime * Conf.ScaleLazySpeed * speedCompensate);
            transform.localScale = newScale;
            return (newScale - lastScale).magnitude > Mathf.Epsilon;
        }

        private bool UpdateLocalRotation(float deltaTime)
        {
            var lastRotation = transform.localRotation;
            var newRotation = Quaternion.RotateTowards(lastRotation, localRotation, Conf.RotationLazySpeed*deltaTime);
            transform.localRotation = newRotation;
            return Quaternion.Angle(newRotation, lastRotation) > Mathf.Epsilon;
        }

        private bool UpdateWorldRotation(float deltaTime)
        {
            var lastRotation = transform.rotation;
            var newRotation = Quaternion.RotateTowards(lastRotation, rotation, Conf.RotationLazySpeed*deltaTime);
            transform.rotation = newRotation;
            return Quaternion.Angle(newRotation, lastRotation) > Mathf.Epsilon;
        }

        private bool UpdateLocalPosition(float deltaTime)
        {
            var lastPosition = transform.localPosition;
            var newPosition = Vector3.MoveTowards(lastPosition, localPosition, Conf.PositionLazySpeed*deltaTime);
            transform.localPosition = newPosition;
            return (newPosition-lastPosition).magnitude > Mathf.Epsilon;
        }

        private bool UpdateWorldPosition(float deltaTime)
        {
            var lastPosition = transform.position;
            var newPosition = Vector3.MoveTowards(lastPosition, position, Conf.PositionLazySpeed*deltaTime);
            transform.position = newPosition;
            return (newPosition-lastPosition).magnitude > Mathf.Epsilon;
        }
    }

    public static Behaviour GetLazyTransform(this Transform transform)
    {
        var lazyTransform = transform.GetComponent<Behaviour>();
        return lazyTransform ?? transform.gameObject.AddComponent<Behaviour>();
    }
}