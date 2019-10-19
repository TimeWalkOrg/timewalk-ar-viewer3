using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/// <summary>
/// Moves the ARSessionOrigin in such a way that it makes the given content appear to be
/// at a given location acquired via a raycast.
/// </summary>
[RequireComponent(typeof(ARSessionOrigin))]
[RequireComponent(typeof(ARRaycastManager))]
public class MakeAppearOnPlane : MonoBehaviour
{
    [SerializeField]
    [Tooltip("A transform which should be made to appear to be at the touch point.")]
    Transform m_Content;
    public Text debugText;

    // Good examples of tracking here: https://github.com/google-ar/arcore-unity-sdk/blob/master/Assets/GoogleARCore/Examples/HelloAR/Scripts/HelloARController.cs


    /// <summary>
    /// A transform which should be made to appear to be at the touch point.
    /// </summary>
    public Transform content
    {
        get { return m_Content; }
        set { m_Content = value; }
    }

    [SerializeField]
    [Tooltip("The rotation the content should appear to have.")]
    Quaternion m_Rotation;

    /// <summary>
    /// The rotation the content should appear to have.
    /// </summary>
    public Quaternion rotation
    {
        get { return m_Rotation; }
        set
        {
            m_Rotation = value;
            if (m_SessionOrigin != null)
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    m_SessionOrigin.MakeContentAppearAt(content, content.transform.position, m_Rotation);
                }
            }
        }
    }

    void Awake()
    {
        m_SessionOrigin = GetComponent<ARSessionOrigin>();
        m_RaycastManager = GetComponent<ARRaycastManager>();

        foreach (Transform child in content)
        {
            child.gameObject.SetActive(false); // hide children of the content object
        }

    }

    void Update()
    {
        Touch touch; // per ARCore example (compare to below)
        //var touch = Input.GetTouch(0);

        //if (Input.touchCount == 0 || m_Content == null) // My OLD way
        if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)

        {
            return; // don't update if just touched or m_Content is null (not set yet)
        }

        if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
        {
            debugText.text = "Clicked in UI";
            return;
        }



        if (m_RaycastManager.Raycast(touch.position, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            // Raycast hits are sorted by distance, so the first one
            // will be the closest hit.
            var hitPose = s_Hits[0].pose;

            // This does not move the content; instead, it moves and orients the ARSessionOrigin
            // such that the content appears to be at the raycast hit position.
            m_SessionOrigin.MakeContentAppearAt(content, hitPose.position, m_Rotation);

            debugText.text = "New hitPose in MakeAppear";

            // DELETE THIS BELOW?
            foreach (Transform child in content)
            {
                child.gameObject.SetActive(true); // SHOW children of the content object
            }
        }
    }

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    ARSessionOrigin m_SessionOrigin;

    ARRaycastManager m_RaycastManager;
}
