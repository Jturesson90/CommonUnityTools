using DG.Tweening;
using Drolegames.Save;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.Impl;

namespace Drolegames.SocialService
{
    public class MockSocial : ISocialService
    {
        public RuntimePlatform Platform => RuntimePlatform.WindowsEditor;

        public bool UserCanSign => true;

        public bool IsLoggedIn { get; private set; }

        public string Name => IsLoggedIn ? "Jesper" : string.Empty;
        public string Greeting => $"Welcome {Name}";
        public string StoreName => "Mock";

        public byte[] CloudData { get; private set; }

        public bool CloudSaveEnabled => true;

        public void Initialize()
        {
            IsLoggedIn = false;
        }

        public void Login(Action<bool> callback)
        {
            Sequence mySequence = DOTween.Sequence();
            mySequence
                .AppendInterval(1.5f)
                .AppendCallback(() => callback?.Invoke(IsLoggedIn = true));
        }

        public void Logout(Action<bool> callback)
        {
            IsLoggedIn = false;
            callback?.Invoke(true);
        }

        public void SaveGame(byte[] data, TimeSpan playedTime, Action<bool> callback)
        {
            CloudData = data;
            var path = $"{Application.persistentDataPath}/mock.txt";
            File.WriteAllText(path, GameSaveData.FromBytes(data).ToString());

            Sequence mySequence = DOTween.Sequence();
            mySequence
              .AppendInterval(1.5f)
              .AppendCallback(() => callback?.Invoke(true));
        }

        public void LoadFromCloud(Action<bool> callback)
        {
            bool success = false;
            var path = $"{Application.persistentDataPath}/mock.txt";
            if (!File.Exists(path))
            {
                File.Create(path);
            }

            var jsonFromCloud = File.ReadAllText(path);

            GameSaveData a = GameSaveData.FromString(jsonFromCloud);
            if (a != null)
            {
                CloudData = a.ToBytes();
                success = true;
            }
            Debug.Log("Mock Cloud Load Success? " + success);
            Sequence mySequence = DOTween.Sequence();
            mySequence
              .AppendInterval(1.5f)
              .AppendCallback(() => callback?.Invoke(success));
        }

        public void UnlockAchievement(string achievementId, Action<bool> callback)
        {
            callback(true);
        }

        public void IncrementAchievement(string achievementId, double steps, Action<bool> callback)
        {
            callback(true);
        }

        public void LoadAchievements(Action<IAchievement[]> callback)
        {
            callback(new IAchievement[0]);
        }

        public void ShowAchievementsUI()
        {
            Debug.Log("MockSocial ShowAchievementsUI");

        }
    }
}
