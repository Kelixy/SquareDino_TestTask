using Controllers;
using UnityEngine;

namespace SceneObjects
{
    public class Finish : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Player")
                ControllersManager.Instance.GameController.EndGame();
        }
    }
}
