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
        public string worldScene;
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
        SceneManager.LoadScene(settings.worldScene);

        yield break;
    }

    public void Exit()
    {
        SceneManager.UnloadSceneAsync(settings.worldScene);
    }
}