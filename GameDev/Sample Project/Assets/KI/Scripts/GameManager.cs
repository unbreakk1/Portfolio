using UnityEngine;
using UnityEngine.InputSystem;

namespace KI
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private LayerMask playerMask;
        [SerializeField] private GameObject rainParent;
        
        private Camera mainCam;
        private Mouse currentMouse;
        private PlayerController currentPlayer;
        private LightManager lightManager;

        private void Awake()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            mainCam = Camera.main;
            currentMouse = Mouse.current;
        }

        public void OnLeftClick()
        {
            Ray ray = mainCam.ScreenPointToRay(currentMouse.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, playerMask))
            {
                if (hit.collider.TryGetComponent(out PlayerController player))
                {
                    if (currentPlayer != null)
                    {
                        currentPlayer.Deactivate();
                    }

                    currentPlayer = player;
                    player.Activate();
                }
            }
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}