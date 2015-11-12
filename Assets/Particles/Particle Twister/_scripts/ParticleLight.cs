
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

        public class ParticleLight : MonoBehaviour
        {
            // =================================	
            // Nested classes and structures.
            // =================================

            // ...

            [System.Serializable]
            public class LightIntensityLerp
            {
                public LightIntensityLerp()
                {
                    targetIntensity = 1.0f;

                    timer = new Timer();

                    timer.target = 1.0f;
                    timer.loop = false;
                }

                public float targetIntensity;
                public Timer timer;
            }

            // =================================	
            // Variables.
            // =================================

            public LightIntensityLerp[] intensityAnimations;
            public MathUtility.LerpType intensityLerpType;

            int currentAnimation = 0;

            public bool complete
            {
                get
                {
                    return currentAnimation == intensityAnimations.Length;
                }
            }

            float fromIntensity = 0.0f;

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
                fromIntensity = GetComponent<Light>().intensity;
            }

            // ...

            void Update()
            {
                if (!complete)
                {
                    GetComponent<Light>().intensity = MathUtility.lerp(fromIntensity, intensityAnimations[currentAnimation].targetIntensity,
                        intensityAnimations[currentAnimation].timer.time / intensityAnimations[currentAnimation].timer.target, intensityLerpType);

                    if (intensityAnimations[currentAnimation].timer.update())
                    {
                        currentAnimation++;
                        fromIntensity = GetComponent<Light>().intensity;
                    }
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
