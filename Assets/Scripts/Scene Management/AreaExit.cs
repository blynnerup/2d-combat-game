using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scene_Management
{
    public class AreaExit : MonoBehaviour
    {
        [SerializeField] private string sceneToLoad;
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.GetComponent<PlayerController>())
            {
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }
}
