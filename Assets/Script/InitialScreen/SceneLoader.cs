using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리를 위해 추가

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName); // 씬 로드
    }

    public void ExitGame()
    {
        Application.Quit(); // 게임 종료
    }
}