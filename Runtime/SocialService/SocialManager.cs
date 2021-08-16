using System;
using UnityEngine;
using Drolegames.Utils;
using Drolegames.GameMenu.ScriptableObjects;
using Drolegames.Save;
using UnityEngine.SocialPlatforms;

namespace Drolegames.SocialService
{
    public class SocialManager : Singleton<SocialManager>
    {
        public GameData_SO gameSaveData;
        private ISocialService socialService;
        public static event EventHandler<SocialManagerArgs> LoggedInChanged;
        public static event EventHandler<bool> LoggingInPendingChanged;
        public static event EventHandler<bool> OnUploadChanged;
        public static event EventHandler<OnUploadCompleteArgs> OnUploadComplete;

        private bool _loggingInPending = false;
        public bool LoggingInPending
        {
            get => _loggingInPending;
            private set
            {
                if (_loggingInPending != value)
                {
                    _loggingInPending = value;
                    LoggingInPendingChanged?.Invoke(this, _loggingInPending);
                }
            }
        }
        private bool _uploadPending = false;
        public bool UploadPending
        {
            get => _uploadPending;
            private set
            {
                if (_uploadPending != value)
                {
                    _uploadPending = value;
                    OnUploadChanged?.Invoke(this, _uploadPending);
                }
            }
        }
        public string Name => socialService.Name;
        public string Greeting => socialService.Greeting;
        public bool IsLoggedIn => socialService.IsLoggedIn;
        public bool UserCanSign => socialService.UserCanSign;
        public string StoreName => socialService.StoreName;
        public bool CloudSaveEnabled => socialService.CloudSaveEnabled;
        public RuntimePlatform Platform => socialService.Platform;
        protected override void Awake()
        {
            base.Awake();
#if UNITY_EDITOR
            socialService = new MockSocial();
#elif UNITY_ANDROID
            socialService = new GooglePlaySocial();
#elif UNITY_IOS
            socialService = new IOSSocial();
#else
#endif

            socialService.Initialize();
        }

        private void Start()
        {
            Login();
        }

        public void Login()
        {
            LoggingInPending = true;
            socialService.Login((bool success) =>
            {
                if (success && CloudSaveEnabled)
                    LoadFromCloud();

                LoggingInPending = false;
                LoggedInChanged?.Invoke(this, new SocialManagerArgs()
                {
                    IsLoggedIn = socialService.IsLoggedIn,
                    Platform = socialService.Platform,
                    Name = socialService.Name
                });
            }
            );
        }
        public void SaveGame(bool manual = false)
        {
            if (gameSaveData && CloudSaveEnabled)
            {
                UploadPending = true;
                var saveData = gameSaveData.GetSaveData();
                TimeSpan timePlayed;
                try
                {
                    timePlayed = TimeSpan.FromSeconds(saveData.totalPlayingTime);
                }
                catch
                {
                    timePlayed = TimeSpan.FromSeconds(3600);
                }

                socialService.SaveGame(saveData.ToBytes(), timePlayed, (bool s) =>
                {
                    UploadPending = false;
                    if (s)
                    {
                        Debug.LogWarning("SocialManager SaveGame Sucess");
                        OnUploadComplete?.Invoke(this, new OnUploadCompleteArgs() { Manual = manual });
                    }
                    else
                    {
                        Debug.LogWarning("SocialManager SaveGame FAIL");
                    }
                });
            }
        }
        public void LoadFromCloud()
        {
            if (!CloudSaveEnabled) return;
            Debug.LogWarning("SocialManager LoadFromCloud");
            socialService.LoadFromCloud((bool success) =>
            {
                Debug.LogWarning("SocialManager LoadFromCloud success? " + success);
                if (success)
                {
                    if (socialService != null)
                    {
                        Debug.LogWarning("SocialManager LoadFromCloud CloudData? " + socialService.CloudData + "  " + GameSaveData.FromBytes(socialService.CloudData).ToString());
                        if (gameSaveData)
                            gameSaveData.MergeSaveData(GameSaveData.FromBytes(socialService.CloudData));
                    }
                    else
                    {
                        Debug.LogError("SocialManager LoadFromCloud socialService is null wtf?");
                    }
                }
            });
        }
        public void LogOut()
        {
            LoggingInPending = true;
            socialService.Logout((bool success) =>
            {
                LoggingInPending = false;
                LoggedInChanged?.Invoke(this, new SocialManagerArgs()
                {
                    IsLoggedIn = false,
                    Platform = socialService.Platform,
                    Name = socialService.Name
                });
            });
        }
        public void UnlockAchievement(string achivementId, Action<bool> callback) => socialService.UnlockAchievement(achivementId, callback);
        public void IncrementAchievement(string achivementId, double steps, Action<bool> callback) => socialService.IncrementAchievement(achivementId, steps, callback);
        public void LoadAchievements(Action<IAchievement[]> callback) => socialService.LoadAchievements(callback);
        public void ShowAchievementsUI() => socialService.ShowAchievementsUI();
        private void OnEnable()
        {
            if (gameSaveData)
            {
                gameSaveData.SavedToDisk += GameSaveData_SavedToDisk;
            }
        }

        private void GameSaveData_SavedToDisk(object sender, EventArgs e)
        {
            SaveGame();
        }
        private void OnDisable()
        {
            if (gameSaveData)
            {
                gameSaveData.SavedToDisk -= GameSaveData_SavedToDisk;
            }
        }
    }
    public class SocialManagerArgs : EventArgs
    {
        public bool IsLoggedIn { get; set; }
        public RuntimePlatform Platform { get; set; }
        public string Name { get; internal set; }
    }
    public class OnUploadCompleteArgs : EventArgs
    {
        public bool Manual { get; set; }
    }
}