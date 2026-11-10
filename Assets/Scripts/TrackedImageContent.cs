
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TrackedImageContent : MonoBehaviour
{
    [SerializeField] private ARTrackedImageManager imageManager;
    [SerializeField] private GameObject contentPrefab;

    private readonly Dictionary<TrackableId, GameObject> content = new();

    void OnEnable()
    {
        imageManager.trackablesChanged.AddListener(OnImagesChanged);
    }

    void OnDisable()
    {
        imageManager.trackablesChanged.RemoveListener(OnImagesChanged);
    }

    void OnImagesChanged(ARTrackablesChangedEventArgs<ARTrackedImage> changes)
    {
        foreach (var image in changes.added)
            ShowContent(image);

        foreach (var image in changes.updated)
            ShowContent(image);

        foreach (var removed in changes.removed)
        {
            if (content.Remove(removed.Key, out var instance))
                Destroy(instance);
        }
    }

    void ShowContent(ARTrackedImage image)
    {
        bool tracked = image.trackingState == TrackingState.Tracking;

        if (!content.TryGetValue(image.trackableId, out var instance))
        {
            if (!tracked) return;

            instance = Instantiate(contentPrefab, image.transform, false);
            instance.transform.localPosition = Vector3.zero;
            instance.transform.localRotation = Quaternion.identity;

            content.Add(image.trackableId, instance);
        }

        instance.SetActive(tracked);
    }
}
