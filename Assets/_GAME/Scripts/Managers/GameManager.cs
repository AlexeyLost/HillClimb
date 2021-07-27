using Cinemachine;
using GAME.Bikes;
using GAME.Grounds;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GAME.Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private PrefabsDB prefabs;
        [SerializeField] private CinemachineVirtualCamera cam;

        private Bike bike;
        private Ground ground;
        private float screenWidht;

        void Start()
        {
            InitLevel();
        }

        private void InitLevel()
        {
            screenWidht = Screen.width;
            SpawnManager.Instance.Prefabs = prefabs;
            bike = new Bike();
            ground = new Ground();
            cam.Follow = bike.view.bodyTransform;
            cam.LookAt = bike.view.bodyTransform;
            EventsManager.Instance.BikeEvents.BikeUpsideDown += RestartGame;
            EventsManager.Instance.MainEvents.UnsubscribeAll += UnsubscribeAll;
            EventsManager.Instance.MainEvents.GameStarted?.Invoke();
        }

        private void RestartGame()
        {
            EventsManager.Instance.MainEvents.UnsubscribeAll?.Invoke();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void UnsubscribeAll()
        {
            EventsManager.Instance.BikeEvents.BikeUpsideDown -= RestartGame;
            EventsManager.Instance.MainEvents.UnsubscribeAll -= UnsubscribeAll;
        }

        private void Update()
        {
            EventsManager.Instance.MainEvents.Update?.Invoke();
            if (Input.GetMouseButton(0))
            {
                if (Input.mousePosition.x > screenWidht / 2)
                    EventsManager.Instance.ControlEvents.PlayerTappedRight?.Invoke();
                else 
                    EventsManager.Instance.ControlEvents.PlayerTappedLeft?.Invoke();
            }

            if (Input.GetMouseButtonUp(0))
            {
                EventsManager.Instance.ControlEvents.PlayerTappedUp?.Invoke();
            }
        }

        private void OnApplicationQuit()
        {
            EventsManager.Instance.MainEvents.UnsubscribeAll?.Invoke();
        }
    }
}