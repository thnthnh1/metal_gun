using System.Collections;
using UnityEngine;

namespace Unity.Advertisement.IosSupport
{
    /// <summary>
    /// This component will trigger the context screen to appear when the scene starts,
    /// if the user hasn't already responded to the iOS tracking dialog.
    /// </summary>
    public class ATTManager : MonoBehaviour
    {
        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        void Start()
        {
            Debug.Log("[DGAME] ATTManager - START");
#if UNITY_IOS
            // check with iOS to see if the user has accepted or declined tracking
            var status = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();

            Debug.Log("[DGAME] ATTManager, status = " + status.ToString());
            if (status == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
            {
                RequestAuthorizationTracking();
            }
#else
            Debug.Log("Unity iOS Support: App Tracking Transparency status not checked, because the platform is not iOS.");
#endif
        }

        public void RequestAuthorizationTracking()
        {
#if UNITY_IOS
            Debug.Log("Unity iOS Support: Requesting iOS App Tracking Transparency native dialog.");

            ATTrackingStatusBinding.RequestAuthorizationTracking();
#else
            Debug.LogWarning("Unity iOS Support: Tried to request iOS App Tracking Transparency native dialog, " +
                             "but the current platform is not iOS.");
#endif
        }
    }
}
