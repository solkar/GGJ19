using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartState : IFSMState
{
    [Serializable]
    public struct Settings
    {
        [SerializeField]
        public string opening;

        [SerializeField]
        public string gameScene;

        [SerializeField]
        public string hudScene;
    }

    private Settings settings;

    public GameStartState(
        Settings settings
    )
    {
        this.settings = settings;
    }

    public IEnumerator Enter()
    {
        SceneManager.LoadScene(settings.opening);

        yield return new WaitForSeconds(2);

        SceneManager.LoadScene(settings.gameScene);

        yield return new WaitForSeconds(2);

        SceneManager.LoadScene(settings.hudScene);
    }

    public void Exit()
    {
    }
}
