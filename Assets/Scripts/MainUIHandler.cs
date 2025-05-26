using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUIHandler : MonoBehaviour
{

    

    public void StartNew()
    {
        SceneManager.LoadScene(1); // main scene is indexed at 1 in build profile
    }
}
