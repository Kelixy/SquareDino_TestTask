using Models;
using UnityEngine;

namespace Controllers
{
    public class ControllersManager : ComponentSingleton<ControllersManager>
    {
        [SerializeField] private HeroController heroController;
        [SerializeField] private GameController gameController;

        public HeroController HeroController => heroController;
        public GameController GameController => gameController;
    }
}
