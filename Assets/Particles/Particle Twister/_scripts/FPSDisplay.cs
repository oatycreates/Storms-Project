
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

        [ExecuteInEditMode]
        [System.Serializable]

        public class FPSDisplay : MonoBehaviour
        {
            // =================================	
            // Nested classes and structures.
            // =================================

            // ...

            // =================================	
            // Variables.
            // =================================

            // Singleton.

            static FPSDisplay instance;

            // Display.

            Text fpsText;

            // Count and timer.

            int frames = 0;
            float time = 0.0f;

            public int targetFrameRate = 60;

            // Length in seconds of time "buffer" to average.

            public float updateTime = 1.0f;

            // =================================	
            // Functions.
            // =================================

            // ...

            void Awake()
            {
                if (instance)
                {
                    Destroy(gameObject);
                }
                else
                {
                    instance = this;
                }

                Application.targetFrameRate = targetFrameRate;
            }

            // ...

            void Start()
            {
                fpsText = GetComponent<Text>();
            }

            // ...

            void Update()
            {
                time += Time.deltaTime;

                frames++;

                if (time > updateTime)
                {
                    fpsText.text = "FPS (1/4s-AVG): " + (1.0f / (time / frames)).ToString("F2");

                    time = 0.0f;
                    frames = 0;
                }
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
