using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUIHandler : MonoBehaviour
{

    

    public void StartNew()
    {
        StartCoroutine(DelayReload());
       
    }

    IEnumerator DelayReload() //Delay coroutine to allow UI to update before reloading the scene so that name is captured.
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(1); // main scene is indexed at 1 in build profile
    }
}
