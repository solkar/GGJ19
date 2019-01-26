using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class GameEnterHouseState : IFSMState
{
    [Serializable]
    public struct Settings
    {
        [SerializeField]
        public string houseScene;
    }

    private Settings settings;

    public GameEnterHouseState(
        Settings settings
    )
    {
        this.settings = settings;
    }

    public IEnumerator Enter()
    {
        SceneManager.LoadScene(settings.houseScene);

        yield break;
    }

    public void Exit()
    {
        SceneManager.UnloadSceneAsync(settings.houseScene);
    }
}