﻿using Assets.Scripts.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Components
{
    public class ReloadLevelComponent : MonoBehaviour
    {
        public void Reload()
        {
            var session = FindObjectOfType<GameSession>();
            Destroy(session.gameObject);

            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
        
    }
}

