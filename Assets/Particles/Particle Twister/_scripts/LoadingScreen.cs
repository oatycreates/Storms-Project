
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

        //[RequireComponent(typeof(TrailRenderer))]

        public class LoadingScreen : MonoBehaviour
        {
            // =================================	
            // Nested classes and structures.
            // =================================

            // ...

            // =================================	
            // Variables.
            // =================================

            public Text loadingText;
            public Transform loadingRotator;

            public Vector2 loadingRotatorYRange = new Vector2(10.0f, 6.0f);

            // Just for quick testing from the editor.

            public bool autoLoad = true;

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

            }

            // ...

            void Update()
            {
                if (Application.CanStreamedLevelBeLoaded(1) && autoLoad)
                {
                    Application.LoadLevel(1);
                }
                else
                {
                    Vector2 loadingRotatorPosition = loadingRotator.localPosition;
                    float progress = Application.GetStreamProgressForLevel(1) * 100.0f;

                    loadingRotatorPosition.y = MathUtility.remap(
                        progress, 0.0f, 100.0f, loadingRotatorYRange.x, loadingRotatorYRange.y);

                    loadingText.text = "LOADING: " + progress.ToString("00.00") + "%";

                    loadingRotator.localPosition = loadingRotatorPosition;
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
