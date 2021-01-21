using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Test.AddtiveScene
{
    public class SceneLoader : MonoBehaviour
    {
        /************************************************属性与变量命名************************************************/
        [SerializeField]
        private Text sceneName;
        [SerializeField]
        private Button button;
        [SerializeField]
        /************************************************Unity方法与事件***********************************************/
        private void Awake()
        {
        }
        private void Start()
        {
        }
        private void Update()
        {
        }
        /************************************************自 定 义 方 法************************************************/
        public void LoadScene(string sceneName)
        {
            Scene scene = SceneManager.GetSceneByName(sceneName);
            if (scene != null)
            {
                if (scene.isLoaded)
                {
                    AsyncOperation result = SceneManager.UnloadSceneAsync(scene);
                    this.StartCoroutine(this.UnlockButton(result, this.button));
                }
                SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
                this.sceneName.text = sceneName;
            }
        }

        private IEnumerator UnlockButton(AsyncOperation result, Button button)
        {
            button.enabled = false;
            while (!result.isDone) yield return null;
            button.enabled = true;
        }
    }
}