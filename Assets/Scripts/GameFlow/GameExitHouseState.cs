using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class GameExitHouseState : IFSMState
{
    [Serializable]
    public struct Settings
    {
        [SerializeField]
        public string houseScene;
    }

    private Settings settings;

    public GameExitHouseState(
        Settings settings
    )
    {
        this.settings = settings;
    }

    public IEnumerator Enter()
    {
        yield break;
    }

    public void Exit()
    {
    }
}