using System.Collections;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scene_Management
{
    public class AreaExit : MonoBehaviour
    {
        [SerializeField] private string sceneToLoad;
        [SerializeField] private string sceneTransitionName;

        private readonly float _waitToLoadTime = 1f;
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.GetComponent<PlayerController>())
            {
                SceneManagement.Instance.SetTransitionName(sceneTransitionName);
                
                UiFade.Instance.FadeToBlack();
                StartCoroutine(LoadSceneRoutine());
            }
        }

        private IEnumerator LoadSceneRoutine()
        {
            yield return new WaitForSeconds(_waitToLoadTime);
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
