using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace _Scripts {
    public class SlerpySlerp : MonoBehaviour {
        [SerializeField]private bool isLooping;
        [SerializeField] private List<Transform> points;
        [SerializeField] private List<Transform[]> steps = new List<Transform[]>();

        private Transform currentStart, currentEnd, currentCenter;
        [SerializeField] int duration;
        [SerializeField] float BallSize = 1f;
        [SerializeField, Range(1,100)] private int _count = 15;
        private int nbSteps;
        private int lNbPoints;


        private void Start()
        {
            lNbPoints = transform.childCount;
            if (lNbPoints <= 1) return;
            points.Clear();
            for (int i = 0; i < transform.childCount; i++)
            {
                points.Add(transform.GetChild(i));
            }
            currentStart = points[0];
            if (isLooping)
            {
                points.Add(currentStart);
                lNbPoints++;
            }
            for (int i = 0; i < points.Count; i++)
            {
                Transform t = points[i];
                if (t.CompareTag("Center")) continue;
                nbSteps++;
            }

            for (int i = 1; i < lNbPoints; i++)
            {
                if (points[i].CompareTag("Center"))
                {
                    currentCenter = points[i];
                    continue;
                }
                else
                {
                    currentEnd = points[i];
                }
                if (currentCenter != null)
                {
                    steps.Add(new Transform[3] {currentStart,currentCenter,currentEnd});
                    currentCenter = null;
                }
                else
                {
                    steps.Add(new Transform[2] { currentStart,  currentEnd });
                }
                currentStart = currentEnd;
            }

            nbSteps--;
        }


        private void OnDrawGizmos() {
            lNbPoints = transform.childCount;
            if (lNbPoints <= 1) return;

            if (!Application.isPlaying)
            {
                points.Clear();
                for (int i = 0; i < transform.childCount; i++)
                {
                    points.Add(transform.GetChild(i));
                }
                currentStart = points[0];
                if (isLooping)
                {
                    points.Add(currentStart);
                    lNbPoints++;
                }
            }
            else
            {
                currentStart = points[0];
                if (isLooping)
                {
                    lNbPoints++;
                }
            }

            for (int i = 1; i < lNbPoints ; i++)
            {
                if (points[i].CompareTag("Center"))
                {
                    if (currentCenter != null)
                    {
                        Gizmos.color = Color.black;
                        Gizmos.DrawSphere(currentCenter.position, 0.15f * BallSize);
                    }
                    currentCenter = points[i];
                    continue;
                }
                else
                {
                    currentEnd = points[i];
                }
                if (currentCenter != null)
                {
                    DrawPath(currentStart, currentEnd, currentCenter) ;
                    currentCenter = null;
                }
                else
                {
                    DrawPath(currentStart, currentEnd);
                }
                currentStart = currentEnd;
            }
            if (currentCenter != null)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawSphere(currentCenter.position, 0.15f * BallSize);
            }
            
            currentStart = null;
            currentEnd = null;
            currentCenter = null;
        }

        private void DrawPath(Transform start, Transform end)
        {
            Gizmos.color = Color.white;

            foreach (var point in EvaluateLerpPoints(start.position, end.position, _count))
            {
                Gizmos.DrawSphere(point, 0.1f * BallSize);
            }
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(start.position, 0.15f * BallSize);
            Gizmos.DrawSphere(end.position, 0.15f * BallSize);
        }

        private void DrawPath(Transform start, Transform end, Transform center)
        {
            Gizmos.color = Color.white;
            foreach (var point in EvaluateSlerpPoints(start.position, end.position, center.position, _count))
            {
                Gizmos.DrawSphere(point, 0.1f * BallSize);
            }
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(center.position, 0.2f * BallSize);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(start.position, 0.15f * BallSize);
            Gizmos.DrawSphere(end.position, 0.15f * BallSize);
        }
        IEnumerable<Vector3> EvaluateSlerpPoints(Vector3 start, Vector3 end, Vector3 center,int count = 10) {
            var startRelativeCenter = start - center;
            var endRelativeCenter = end - center;

            var f = 1f / count;

            for (var i = 0f; i < 1 + f; i += f) {
                yield return Vector3.Slerp(startRelativeCenter, endRelativeCenter, i) + center;
            }
        }

        IEnumerable<Vector3> EvaluateLerpPoints(Vector3 start, Vector3 end, int count = 10)
        {
            var f = 1f / count;
            for (var i = 0f; i < 1 + f; i += f)
            {
                yield return Vector3.Lerp(start, end, i);
            }
        }

        public void ApplyMovement(Transform pTransform, float pCurrentTime, float pTotalDuration, Easing.EaseType easing = Easing.EaseType.None)
        {
            float lStepDuration = pTotalDuration / nbSteps;


            float easedTime = pCurrentTime;
            if (easing != Easing.EaseType.None) 
            {
                easedTime = Easing.Ease(pCurrentTime / pTotalDuration, easing);
                easedTime *= pTotalDuration;
            }
            int lCurrentStepIndex = (int)MathF.Floor(easedTime / lStepDuration);
            float currentStepTime = 0;
            if (lCurrentStepIndex < 0) 
            {
                lCurrentStepIndex = 0;
                currentStepTime = easedTime;
            };
            if (lCurrentStepIndex > nbSteps - 1)
            {
                lCurrentStepIndex = nbSteps - 1;
                currentStepTime = easedTime - pTotalDuration + 1;
            }
            else 
            {
                currentStepTime = easedTime % lStepDuration / lStepDuration ;
            }


            Transform[] currentStep = steps[lCurrentStepIndex];

            if (currentStep == null || currentStep.Length <= 1 || currentStep.Length >= 4) return;
            //Lerp movement
            if (currentStep.Length == 2) { 
                pTransform.position = Vector3.LerpUnclamped(currentStep[0].position, currentStep[1].position, currentStepTime); 
                pTransform.rotation = Quaternion.SlerpUnclamped(currentStep[0].rotation, currentStep[1].rotation, currentStepTime);
            }
            //Slerp Movement
            else if (currentStep.Length == 3) {
                var startRelativeCenter = currentStep[0].position - currentStep[1].position;
                var endRelativeCenter = currentStep[2].position - currentStep[1].position;

                pTransform.position = Vector3.SlerpUnclamped(startRelativeCenter, endRelativeCenter, currentStepTime) + currentStep[1].position;
                pTransform.rotation = Quaternion.SlerpUnclamped(currentStep[0].rotation, currentStep[2].rotation, currentStepTime);
            }
        }

        public void ApplyMovementInverse(Transform pTransform, float pCurrentTime, float pTotalDuration, Easing.EaseType easing = Easing.EaseType.None)
        {
            if (pCurrentTime == Time.deltaTime) 
            {
                InvertPoints((int)MathF.Floor(pCurrentTime / pTotalDuration / nbSteps), pTotalDuration);
            }
            ApplyMovement(pTransform, pCurrentTime, pTotalDuration,easing);
        }

        private IEnumerator InvertPoints(int currentStep,float pDuration)
        {
            steps[currentStep].Reverse();
            yield return new WaitForSeconds(pDuration);
            steps[currentStep].Reverse();
        }
    }
}