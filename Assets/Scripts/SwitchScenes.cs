using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScenes : MonoBehaviour
{

    public static void SwitchScenes1() {
        PlayerStats.transitionning = true;
        PlayerStats.player = DefaultValues.player;
        DefaultValues.grid.SetActive(false);
        PlayerStats.playerPos = PlayerStats.player.transform.position;
        PlayerStats.player.transform.position = new Vector3(0f, 0f, 0f);
        SceneManager.LoadScene(sceneBuildIndex:1, LoadSceneMode.Additive);
    }

    public static void SwitchScenes2() {
        PlayerStats.player.transform.position = PlayerStats.playerPos;
        PlayerStats.transitionning = false;
        DefaultValues.grid.SetActive(true);
        SceneManager.UnloadSceneAsync(sceneBuildIndex:1);
    }
}
