
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

        public class MouseFollow : MonoBehaviour
        {
            // =================================	
            // Nested classes and structures.
            // =================================

            // ...

            // =================================	
            // Variables.
            // =================================

            public float lerpTime = 0.0f;

            Vector3 position;

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
                Vector3 mousePosition = Input.mousePosition;
                mousePosition.z = -Camera.main.transform.localPosition.z;

                position = Vector3.Lerp(
                    position, Camera.main.ScreenToWorldPoint(mousePosition), Time.deltaTime / lerpTime);

                transform.position = position;
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
