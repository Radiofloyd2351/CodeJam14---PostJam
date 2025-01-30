using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScenes : MonoBehaviour
{

    public void SwitchScenes1() {
        PlayerStats.player = DefaultValues.player;
        DefaultValues.grid.SetActive(false);
        PlayerStats.heldInstrument = DefaultValues.Current.type;
        PlayerStats.playerPos = PlayerStats.player.transform.position;
        SceneManager.LoadScene(sceneBuildIndex:1, LoadSceneMode.Additive);
        PlayerStats.transitionning = true;
    }

    public void SwitchScenes2() {
        SceneManager.UnloadSceneAsync(sceneBuildIndex:1);
        PlayerStats.transitionning = false;
    }
}
