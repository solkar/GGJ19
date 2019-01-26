using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Runtime.CompilerServices;

public class GameEnterHouseState : IFSMState
{
    [Serializable]
    public struct Settings
    {
        [SerializeField]
        public string houseScene;

        [SerializeField] 
        public string hudScene;
    }

    private Settings settings;
    private bool _maxHealth; 

    public GameEnterHouseState(
        Settings settings
    )
    {
        this.settings = settings;
    }

    public IEnumerator Enter()
    {
        SceneManager.LoadScene(settings.houseScene);
        
        
        SceneManager.LoadScene(settings.hudScene, LoadSceneMode.Additive);
        
        EventBus.OnPlayerHealthMax.evt += () => { _maxHealth = true; };

        yield return AutoHealCoroutine();

        yield break;
    }

    public void Exit()
    {
        SceneManager.UnloadSceneAsync(settings.houseScene);
    }

    private IEnumerator AutoHealCoroutine()
    {
        yield return new WaitForSeconds(2.0f);

        _maxHealth = false;
        while (_maxHealth == false)
        {
            EventBus.OnPlayerHeal.Invoke(1);

            yield return new WaitForSeconds(0.5f);
        }

        yield return null;
    }

}