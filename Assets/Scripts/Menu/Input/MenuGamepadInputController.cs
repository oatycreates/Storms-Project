using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace ProjectStorms
{
    public class MenuGamepadInputController : MonoBehaviour
    {
        [Header("Configuration")]
        [Range(1, 4)]
        public int playerControlling = 1;

        [Range(0.0f, 1.0f)]
        public float deadZone = 0.5f;

        public Button backButton;

        Button[] m_buttons;

        Button m_selectedButton;

        Vector2 m_lastThumbstickState;

        public void Awake()
        {
            m_buttons = GetComponentsInChildren<Button>();
        }

        void Start()
        {
            
        }

        void Update()
        {
            UpdateButtons();

            if (!IsThumbstickCentered())
            {
                return;
            }

            Vector2 thumbstickState = GetThumbstickState();

            Selectable selectable = null;

            // Horizontal input
            if (thumbstickState.x < -deadZone)
            {
                Debug.Log("Thumbstick Left");
                selectable = m_selectedButton.FindSelectableOnLeft();
            }
            else if (thumbstickState.x > deadZone)
            {
                Debug.Log("Thumbstick Right");
                selectable = m_selectedButton.FindSelectableOnRight();
            }
            // Vertical input
            else if (thumbstickState.y < -deadZone)
            {
                Debug.Log("Thumbstick Up");
                selectable = m_selectedButton.FindSelectableOnUp();
            }
            else if (thumbstickState.y > deadZone)
            {
                Debug.Log("Thumbstick Down");
                selectable = m_selectedButton.FindSelectableOnDown();
            }

            // Select button if appropriate selectable was found
            if (selectable != null)
            {
                m_selectedButton.OnDeselect(null);

                selectable.OnSelect(null);
                m_selectedButton = selectable.GetComponent<Button>();
            }
        }
        
        //A quick fix
        public void ResetScene()
        {
        	Application.LoadLevel(Application.loadedLevelName);
        }

        public void OnEnable()
        {
            m_selectedButton = m_buttons[0];
            m_selectedButton.OnSelect(null);
        }

        public void LateUpdate()
        {
            m_lastThumbstickState = GetThumbstickState();
        }

        void UpdateButtons()
        {
            string playerID = "";
            switch (playerControlling)
            {
                case 1:
                    playerID = "Player1_";
                    break;

                case 2:
                    playerID = "Player2_";
                    break;

                case 3:
                    playerID = "Player3_";
                    break;

                case 4:
                    playerID = "Player4_";
                    break;

                default:
                    break;
            }

            // Ensure both buttons aren't down at the same time
            if (Input.GetButton(playerID + "FaceDown") && 
                Input.GetButtonUp(playerID + "FaceRight"))
            {
                return;
            }

            // Update select button
            if (Input.GetButtonDown(playerID + "FaceDown") &&
                m_selectedButton != null)
            {
                m_selectedButton.onClick.Invoke();
            }

            // Update deselect button
            else if (Input.GetButtonDown(playerID + "FaceRight") &&
                backButton != null)
            {
                backButton.onClick.Invoke();
            }
            
            //Trigger stuff using ESC
            if (Input.GetKeyDown(KeyCode.Escape))
            {
				if ( backButton != null)
				{
					backButton.onClick.Invoke();
				}
            }
        }

        public void SelectButton(Button a_button)
        {
            if (!this.isActiveAndEnabled)
            {
                return;
            }

            m_selectedButton.OnDeselect(null);

            m_selectedButton = a_button;
            m_selectedButton.OnSelect(null);
        }

        public void UnselectButton(Button a_button)
        {
            a_button.OnDeselect(null);
        }

        public void SetBackButton(Button a_button)
        {
            backButton = a_button;
        }

        bool IsThumbstickCentered()
        {
            return m_lastThumbstickState.x > -deadZone &&
                m_lastThumbstickState.x < deadZone &&
                m_lastThumbstickState.y > -deadZone &&
                m_lastThumbstickState.y < deadZone;
        }

        Vector2 GetThumbstickState()
        {
            // Determine which controller we shall gather
            // input from
            string playerAxis;
            switch (playerControlling)
            {
                case 1:
                    playerAxis = "Player1_";
                    break;

                case 2:
                    playerAxis = "Player2_";
                    break;

                case 3:
                    playerAxis = "Player3_";
                    break;

                case 4:
                    playerAxis = "Player4_";
                    break;

                default:
                    playerAxis = "Player1_";
                    break;
            }

            // Return input for specified controller
            return new Vector2(Input.GetAxisRaw(playerAxis + "Horizontal"), 
                Input.GetAxisRaw(playerAxis + "Vertical"));
        }
    }
}
