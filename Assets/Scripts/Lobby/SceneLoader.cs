using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DogGuns_Games
{
    public class SceneLoader : MonoBehaviour
    {
        protected static SceneLoader instance;

        public static SceneLoader Instace
        {
            get
            {
                if (instance == null)
                {
                    var obj = FindAnyObjectByType<SceneLoader>();
                    if (obj != null)
                    {
                        instance = obj;
                    }
                    else
                    {
                        instance = Create();
                    }
                }

                return instance;
            }
            private set { instance = value; }
        }

        [SerializeField] private CanvasGroup sceneLoadferCanvasGroup;
        [SerializeField] private Slider progressbar;


        private string loadSceneName;

        public static SceneLoader Create()
        {
            var SeneLoadprefeb = Resources.Load<SceneLoader>("SceneLoader");
            return Instantiate(SeneLoadprefeb);
        }

        private void Awake()
        {
            if (instance != this)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
        }

        public void LoadScene(string SceneName)
        {
            gameObject.SetActive(true);
            SceneManager.sceneLoaded += LoadSceneEnd;
            loadSceneName = SceneName;
            StartCoroutine(Load(SceneName));
            Debug.Log("Scene change activie");
        }

        private IEnumerator Load(string sceneName)
        {
            progressbar.value = 0f;
            yield return StartCoroutine(Fade(true));

            AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
            op.allowSceneActivation = false;

            float timer = 0.0f;
            while (!op.isDone)
            {
                yield return null;
                timer += Time.unscaledDeltaTime;
                if (op.progress < 0.9f)
                {
                    float targetProgress = Mathf.Lerp(progressbar.value, op.progress, timer);
                    progressbar.value = targetProgress;
                    if (progressbar.value >= op.progress)
                    {
                        timer = 0f;
                    }
                }
                else
                {
                    float targetProgress = Mathf.Lerp(progressbar.value, 1f, timer);
                    progressbar.value = targetProgress;
                    if (progressbar.value >= 0.9999f)
                    {
                        op.allowSceneActivation = true;
                        yield break;
                    }
                }
            }
        }


        private void LoadSceneEnd(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (scene.name == loadSceneName)
            {
                StartCoroutine(Fade(false));
                SceneManager.sceneLoaded -= LoadSceneEnd;
            }
        }

        private IEnumerator Fade(bool isFadeIn)
        {
            float timer = 0f;

            while (timer <= 1f)
            {
                yield return null;
                timer += Time.unscaledDeltaTime * 2f;
                sceneLoadferCanvasGroup.alpha = Mathf.Lerp(isFadeIn ? 0 : 1, isFadeIn ? 1 : 0, timer);
            }

            if (!isFadeIn)
            {
                gameObject.SetActive(false);
            }
        }
    }
}