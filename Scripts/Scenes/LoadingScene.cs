using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    static string nextScene;

    Slider progressBar;
    Text progressText;
    TextMesh text;
    
    public static void LoadScene(string sceneName){
        nextScene = sceneName;
        SceneManager.LoadScene("Loading");
    }
    
    void Start()
    {
       progressBar = gameObject.GetComponentInChildren<Slider>();
       progressText = transform.GetChild(2).GetComponent<Text>();
       text = transform.GetChild(3).GetComponent<TextMesh>();
    
        StartCoroutine(LoadSceneProcess());
    }

    IEnumerator LoadSceneProcess(){
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;
        
        float timer = 0f;
        while(!op.isDone){
            yield return null;

            if(op.progress < 0.9f){
                progressBar.value = op.progress;
                progressText.text = op.progress.ToString() + "%";
            }else{
                timer += Time.unscaledDeltaTime;
                progressBar.value = Mathf.Lerp(0.9f,1f,timer);
                progressText.text = (Mathf.Lerp(0.9f,1f,timer)*100).ToString() + "%";
                if(progressBar.value>=1f){
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
