using UnityEngine;

namespace Controllers
{
    public class GameController : MonoBehaviour
    {
        void Start()
        {
            
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                ControllersManager.Instance.HeroController.Shoot();
            }
        }
    }
}
