using UnityEngine;
using System.Collections;

public abstract class GameLoader : MonoBehaviour
{
    static public void CallAfterCompletion(System.Action callback)
    {
        if (s_instance != null && s_instance.m_completed)
        {
            callback.Invoke();
        }
        else
        {
            s_callAfterCompletion += callback;
        }

        if (s_instance == null)
        {
            GameObject.Instantiate(Resources.Load("GameLoader"));
        }
    }

    static protected System.Action s_callAfterCompletion = null;
    static protected GameLoader s_instance = null;

    private bool m_completed = false;
    private int m_loadedSceneIndex = -1;


    public bool loadedFromEntryScene
    {
#if UNITY_EDITOR
        get { return m_loadedSceneIndex == 0; }
#else
        get{ return false; }
#endif
    }



    private void Awake()
    {
        if (s_instance != null)
        {
            GameObject.Destroy(this.gameObject);
            return;
        }

        s_instance = this;
        GameObject.DontDestroyOnLoad(this.gameObject);
        this.StartCoroutine(Load());
    }

    private IEnumerator Load()
    {
        yield return StartCoroutine(LoadingSequence());

        m_loadedSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        if (s_callAfterCompletion != null)
        {
            s_callAfterCompletion.Invoke();
            s_callAfterCompletion = null;
        }
    }

    protected abstract IEnumerator LoadingSequence();

}
