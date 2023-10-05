using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltimateAttributesPack;

namespace Romain
{
    public class TongueScript : MonoBehaviour
    {
        [SerializeField] Camera mainCamera;
        [SerializeField] GameObject tongueAimObj;

        [Title("Tongue params", "light blue", "white")]
        [SerializeField] LineRenderer lineRend;
        [SerializeField] Transform tongueStartTransform;
        [SerializeField] Transform tongueEndTransform;
        [SerializeField] GrabType grabType;
        [SerializeField] float maxTonguelenght;

        [ShowIf("grabType", GrabType.Propulser)]
        [SerializeField] float grabForce = 50f;
        public enum GrabType
        {
            Tirer,
            Propulser,
        }

        [Line("light blue")]
        [SerializeField] AnimationCurve tongueOutCurve;
        [SerializeField] float tongueOutTime;
        [SerializeField] AnimationCurve tongueInCurve;
        [SerializeField] float tongueInTime;

        [Title("Debug", "blue", "white")]
        [SerializeField] bool showDebug = false;
        [SerializeField] Color debugColor = Color.red;


        GameObject targetObject;

        float tongueOutTimer, tongueInTimer;
        bool tongueOut, tongueIn;

        bool tongueAimMode;
        bool grabbing;

        void Update()
        {
            // Détection des inputs
            if (Input.GetKey(KeyCode.E) && !grabbing)
            {
                // Stopper les mouvements du joueur
                tongueAimMode = true;
            }
            else if (Input.GetKeyUp(KeyCode.E) && !grabbing)
            {
                float objectDistanceFromPlayer = Vector3.Distance(tongueStartTransform.position, targetObject.transform.position);
                if(objectDistanceFromPlayer <= maxTonguelenght)
                {
                    TryGrabTargetedObject(targetObject);
                }

                // Redonner les mouvements au joueur
                tongueAimMode = false;
            }

            if(tongueAimMode && !grabbing)
            {
                if (!tongueAimObj.activeInHierarchy)
                {
                    tongueAimObj.SetActive(true);
                }

                Vector3 mousePos = Input.mousePosition;
                mousePos.z = 1f;
                tongueAimObj.transform.position = mainCamera.ScreenToWorldPoint(mousePos);
            }
            else
            {
                if (tongueAimObj.activeInHierarchy)
                {
                    tongueAimObj.SetActive(false);
                }
            }

            if (grabbing)
            {
                if (!lineRend.enabled)
                {
                    lineRend.enabled = true;
                }
                lineRend.SetPosition(0, tongueStartTransform.position);
                lineRend.SetPosition(1, tongueEndTransform.position);
            }
            else
            {
                lineRend.enabled = false;
            }


        }

        private void FixedUpdate()
        {
            if (tongueAimMode && !grabbing)
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 500))
                {
                    if (showDebug)
                    {
                        Debug.Log("Tongue target object : " + hit.transform.name);
                        Debug.DrawLine(tongueStartTransform.position, hit.transform.position, debugColor);                       
                    }
                    targetObject = hit.transform.gameObject;
                }
            }
        }

        void TryGrabTargetedObject(GameObject targetedObject)
        {
            if(targetedObject != null && !grabbing)
            {
                tongueInTimer = 0;
                tongueOutTimer = 0;
                tongueOut = true;
                tongueIn = false;

                StartCoroutine(GrabTargetedObject(targetedObject));
                
            }
        }

        IEnumerator GrabTargetedObject(GameObject targetedObject)
        {
            grabbing = true;
            targetObject = null;
            switch (grabType)
            {
                case GrabType.Propulser:

                    while (tongueOut)
                    {
                        if(tongueOutTimer < tongueOutTime)
                        {
                            tongueOutTimer += Time.deltaTime;

                            tongueEndTransform.position = Vector3.Lerp(tongueStartTransform.position, targetedObject.transform.position, tongueOutCurve.Evaluate(tongueOutTimer / tongueOutTime));
                        }
                        else
                        {                            
                            tongueEndTransform.position = targetedObject.transform.position;
                            tongueIn = true;
                            tongueOut = false;
                        }
                        yield return null;
                    }
                    while (tongueIn)
                    {
                        if(tongueInTimer < tongueInTime)
                        {
                            tongueInTimer += Time.deltaTime;

                            tongueEndTransform.position = Vector3.Lerp(targetedObject.transform.position, tongueStartTransform.position, tongueOutCurve.Evaluate(tongueInTimer / tongueInTime));
                        }
                        else
                        {
                            tongueEndTransform.position = tongueStartTransform.position;
                            tongueIn = false;

                            grabbing = false;
                        }
                        yield return null;
                    }
                    break;
                case GrabType.Tirer:






                    break;
            }
            


        }

        private void OnDrawGizmos()
        {
            if (showDebug)
            {
                Gizmos.color = debugColor;
                Gizmos.DrawWireSphere(tongueStartTransform.position, maxTonguelenght);
            }
        }
    }
}
