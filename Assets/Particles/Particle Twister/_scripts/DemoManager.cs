
// =================================	
// Namespaces.
// =================================

using UnityEngine;
using System.Collections;

using UnityEngine.UI;

// =================================	
// Define namespace.
// =================================

namespace MirzaBeig
{

    namespace ParticleTwister
    {

        // =================================	
        // Classes.
        // =================================

        //[ExecuteInEditMode]
        [System.Serializable]

        public class DemoManager : MonoBehaviour
        {
            // =================================	
            // Nested classes and structures.
            // =================================



            // =================================	
            // Variables.
            // =================================

            public Vector3 cameraPosition;
            public Vector3 cameraPosition2;

            Vector3 targetCameraPosition;
            Vector3 targetCameraContainerRotation;

            // Because Euler angles wrap around 360, I use
            // a separate value to store the full rotation.

            Vector3 cameraContainerRotation;

            public float cameraSpeed = 2.0f;

            public float cameraLerpTime = 0.2f;
            public float cameraContainerLerpTime = 0.2f;

            public Vector2 cameraAngleLimits = new Vector2(-5.0f, 65.0f);

            public GameObject field;

            public bool interactiveCameraMode = false;
            public bool instantiateParticleMode = false;

            public PerpetualParticleManager perpetualParticleSystems;
            public InstantiatedParticleManager instantiatedParticleSystems;

            public Text particleCountText;
            public Text currentParticleSystemText;

            public Text particleSpawnInstructionText;

            // =================================	
            // Functions.
            // =================================

            // ...

            void Awake()
            {

            }

            // ...

            void Start()
            {
                // Toggle functions will reverse these. So reverse them to start with.

                interactiveCameraMode = !interactiveCameraMode;
                instantiateParticleMode = !instantiateParticleMode;

                toggleParticleSystems();
                toggleInteractiveCameraMode();

                updateCurrentParticleSystemNameText();
            }

            // ...

            public void toggleInteractiveCameraMode()
            {
                if (interactiveCameraMode)
                {
                    interactiveCameraMode = false;

                    field.SetActive(false);
                    targetCameraPosition = cameraPosition;

                    targetCameraContainerRotation = Vector3.zero;
                }
                else
                {
                    interactiveCameraMode = true;

                    field.SetActive(true);
                    targetCameraPosition = cameraPosition2;
                }

                cameraContainerRotation = Camera.main.transform.parent.localEulerAngles;
            }

            // ...

            public void toggleParticleSystems()
            {
                if (!instantiateParticleMode)
                {
                    instantiateParticleMode = true;

                    perpetualParticleSystems.gameObject.SetActive(false);
                    instantiatedParticleSystems.gameObject.SetActive(true);

                    particleSpawnInstructionText.gameObject.SetActive(true);
                }
                else
                {
                    instantiateParticleMode = false;
                    instantiatedParticleSystems.clear();

                    perpetualParticleSystems.gameObject.SetActive(true);
                    instantiatedParticleSystems.gameObject.SetActive(false);

                    particleSpawnInstructionText.gameObject.SetActive(false);
                }

                updateCurrentParticleSystemNameText();
            }

            void updateCamera()
            {

            }

            // ...

            void Update()
            {
                // Get targets.

                if (interactiveCameraMode)
                {
                    if (!Input.GetKey(KeyCode.LeftShift))
                    {
                        targetCameraPosition.y += Input.GetAxis("Vertical") * cameraSpeed;
                        targetCameraPosition.y = Mathf.Clamp(targetCameraPosition.y, cameraAngleLimits.x, cameraAngleLimits.y);
                    }
                    else
                    {
                        targetCameraPosition.z += Input.GetAxis("Vertical") * cameraSpeed;
                    }

                    targetCameraContainerRotation.y += Input.GetAxis("Horizontal") * cameraSpeed;
                }

                // Camera position.

                Camera.main.transform.localPosition = Vector3.Lerp(
                    Camera.main.transform.localPosition, targetCameraPosition, Time.deltaTime / cameraLerpTime);

                // Camera container rotation.

                cameraContainerRotation = Vector3.Lerp(
                    cameraContainerRotation, targetCameraContainerRotation, Time.deltaTime / cameraContainerLerpTime);

                Camera.main.transform.parent.localEulerAngles = cameraContainerRotation;

                // Look at origin.

                Camera.main.transform.LookAt(Vector3.zero);

                // Switch modes.

                if (Input.GetKeyDown(KeyCode.E))
                {
                    toggleInteractiveCameraMode();
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    toggleParticleSystems();
                }

                // Scroll through systems.

                if (Input.GetAxis("Mouse ScrollWheel") < 0)
                {
                    next();
                }
                else if (Input.GetAxis("Mouse ScrollWheel") > 0)
                {
                    previous();
                }

                // Random prefab while holding key.

                else if (Input.GetKey(KeyCode.R))
                {
                    if (instantiateParticleMode)
                    {
                        instantiatedParticleSystems.randomize();
                        updateCurrentParticleSystemNameText();

                        // If also holding down, auto-spawn at random point.

                        if (Input.GetKey(KeyCode.T))
                        {
                            instantiatedParticleSystems.instantiateParticlePrefabRandom();
                        }
                    }
                }

                // Left-click to spawn.

                if (Input.GetMouseButtonDown(0))
                {
                    if (instantiateParticleMode)
                    {
                        instantiatedParticleSystems.instantiateParticlePrefab(Input.mousePosition);
                    }
                }
            }

            // ...

            void LateUpdate()
            {
                // Update particle count display.

                particleCountText.text = "PARTICLE COUNT: ";

                if (!instantiateParticleMode)
                {
                    particleCountText.text += perpetualParticleSystems.getParticleCount().ToString();
                }
                else
                {
                    particleCountText.text += instantiatedParticleSystems.getParticleCount().ToString();
                }
            }

            // ...

            void updateCurrentParticleSystemNameText()
            {
                if (!instantiateParticleMode)
                {
                    currentParticleSystemText.text = perpetualParticleSystems.getCurrentPrefabName(true);
                }
                else
                {
                    currentParticleSystemText.text = instantiatedParticleSystems.getCurrentPrefabName(true);
                }
            }

            // ...

            public void next()
            {
                if (!instantiateParticleMode)
                {
                    perpetualParticleSystems.next();
                }
                else
                {
                    instantiatedParticleSystems.next();
                }

                updateCurrentParticleSystemNameText();
            }

            public void previous()
            {
                if (!instantiateParticleMode)
                {
                    perpetualParticleSystems.previous();
                }
                else
                {
                    instantiatedParticleSystems.previous();
                }

                updateCurrentParticleSystemNameText();
            }

            // =================================	
            // End functions.
            // =================================

        }

        // =================================	
        // End namespace.
        // =================================

    }

}

// =================================	
// --END-- //
// =================================
