using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static void loadSLR() {
        SceneManager.LoadSceneAsync("Custom/Scenes/WRLDSLR", LoadSceneMode.Single);
    }
    


}
