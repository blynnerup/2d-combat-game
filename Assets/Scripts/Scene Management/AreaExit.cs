using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scene_Management
{
    public class AreaExit : MonoBehaviour
    {
        [SerializeField] private string sceneToLoad;
        [SerializeField] private string sceneTransitionName;
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.GetComponent<PlayerController>())
            {
                SceneManager.LoadScene(sceneToLoad);
                SceneManagement.Instance.SetTransitionName(sceneTransitionName);
            }
        }
    }
}
