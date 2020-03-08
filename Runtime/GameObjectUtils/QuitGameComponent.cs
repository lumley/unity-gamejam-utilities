using UnityEngine;

namespace Lumley.GameObjectUtils
{
    public class QuitGameComponent : MonoBehaviour
    {
        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit(0);
        }
    }
}