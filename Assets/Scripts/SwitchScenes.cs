using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScenes : MonoBehaviour
{

    public static void SwitchScenes1() {
        PlayerInfos.transitionning = true;
        PlayerInfos.player = DefaultValues.player;
        DefaultValues.grid.SetActive(false);
        PlayerInfos.playerPos = PlayerInfos.player.transform.position;
        PlayerInfos.player.transform.position = new Vector3(0f, 0f, 0f);
        SceneManager.LoadScene(sceneBuildIndex:1, LoadSceneMode.Additive);
    }

    public static void SwitchScenes2() {
        PlayerInfos.player.transform.position = PlayerInfos.playerPos;
        PlayerInfos.transitionning = false;
        DefaultValues.grid.SetActive(true);
        SceneManager.UnloadSceneAsync(sceneBuildIndex:1);
    }
}
