namespace Drolegames.Advertisements
{
    using System;
    using Drolegames.Utils;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Advertisements;
    using UnityEngine.Events;
    using UnityEngine.SceneManagement;

    public class AdvertisementsManager : MonoBehaviour, IUnityAdsListener
    {
        public string iOSGameId = string.Empty;
        public string androidGameId = string.Empty;
        public string mockGameId = string.Empty;
        public string rewardVideoID = string.Empty;

        private string gameId = string.Empty;
        private bool _adsIsReady = false;

        public bool AdsIsReady
        {
            get => _adsIsReady;
            set
            {
                if (_adsIsReady != value)
                {
                    _adsIsReady = value;
                    OnAdsIsReadyChanged?.Invoke(this, value);
                }
            }
        }
        public static event EventHandler<bool> OnAdsIsReadyChanged;
        public static event EventHandler<AdsFinishedEventArgs> OnRewardVideoSuccess;

        private string currentId;
        private void OnEnable()
        {
            Advertisement.AddListener(this);
        }
        private void OnDisable()
        {
            Advertisement.RemoveListener(this);
        }
        private void Start()
        {
#if UNITY_IOS
            gameId = iOSGameId;
#elif UNITY_ANDROID
            gameId = androidGameId;
#else 
            gameId = mockGameId;
#endif   
        }
        public void Initialize()
        {
            Advertisement.Initialize(gameId, false);
        }
        public void OnUnityAdsDidError(string message)
        {
            Debug.LogError("AdManager OnUnityAdsDidError " + message);
        }

        public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        {
            if (rewardVideoID.Equals(placementId))
            {
                OnRewardVideoSuccess?.Invoke(this, new AdsFinishedEventArgs(showResult, currentId));
            }
        }

        public void OnUnityAdsDidStart(string placementId)
        {
            if (placementId.Equals(rewardVideoID))
            {
                AdsIsReady = false;
            }
        }

        public void OnUnityAdsReady(string placementId)
        {
            if (placementId.Equals(rewardVideoID))
            {
                AdsIsReady = true;
            }
        }
        public bool GetIsReady() => Advertisement.IsReady(rewardVideoID);
        public bool ShowRewardVideo(string id)
        {
            if (!GetIsReady()) return false;
            Advertisement.Show(rewardVideoID);
            currentId = id;
            return true;
        }
    }
    public class AdsFinishedEventArgs : EventArgs
    {
        public ShowResult showResult;
        public string id;
        public AdsFinishedEventArgs(ShowResult showResult, string id)
        {
            this.showResult = showResult;
            this.id = id;
        }
    }
}