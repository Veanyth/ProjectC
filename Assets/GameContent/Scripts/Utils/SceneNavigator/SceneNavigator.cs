using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public enum SceneNames
{
    MainMenu,
    Level
}

public class SceneNavigator : SingletonMB<SceneNavigator>
{
    [SerializeField] CanvasGroup loadingTransitionGrpCanvas;
    AsyncOperation asyncLoadIndex;

    public void LoadIndexScene(int index)
    {
        StartCoroutine(waitForLevelSceneLoad(index));
    }

    IEnumerator waitForLevelSceneLoad(int index)
    {
        Transition(true);
        yield return new WaitForSeconds(1);
        asyncLoadIndex = SceneManager.LoadSceneAsync(index, LoadSceneMode.Single); // can call it by scene name or scene index
        while (!asyncLoadIndex.isDone)
        {
            yield return null;
        }
        Transition(false);
    }



    private void Transition(bool show)
    {
        if (show)
        {
            loadingTransitionGrpCanvas.gameObject.SetActive(true);
            loadingTransitionGrpCanvas.DOFade(1, 1);
        }
        else
        {
            loadingTransitionGrpCanvas.gameObject.SetActive(true);
            loadingTransitionGrpCanvas.DOFade(1, 0);
            loadingTransitionGrpCanvas.DOFade(0, 1).OnComplete(() => loadingTransitionGrpCanvas.gameObject.SetActive(false));
        }

    }
}
