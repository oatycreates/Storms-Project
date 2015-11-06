
// =================================	
// Namespaces.
// =================================

using UnityEngine;
using System.Collections;

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

        public class Rotate : MonoBehaviour
        {
            // =================================	
            // Nested classes and structures.
            // =================================

            // ...

            // =================================	
            // Variables.
            // =================================

            // Rotation speeds.

            public Vector3 rotation;
            public UpdateCallback updateCallback;

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

            void update()
            {
                transform.Rotate(rotation);
            }

            // ...

            void Update()
            {
                if (updateCallback == UpdateCallback.update)
                {
                    update();
                }
            }
            void FixedUpdate()
            {
                if (updateCallback == UpdateCallback.fixedUpdate)
                {
                    update();
                }
            }
            void LateUpdate()
            {
                if (updateCallback == UpdateCallback.lateUpdate)
                {
                    update();
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
