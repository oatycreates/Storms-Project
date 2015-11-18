
// =================================	
// Namespaces.
// =================================

using UnityEngine;

using System.Linq;
using System.Reflection;

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

        public class ParticlePrefab : MonoBehaviour
        {
            // =================================	
            // Nested classes and structures.
            // =================================



            // =================================	
            // Variables.
            // =================================

            public ParticleSystem[] particleSystems { get; set; }

            public bool autoDestroy = false;

            // Wait for all lights to complete their animation cycles before destroying,
            // even if particles have all died out. Else, just wait for all particles to die
            // and destroy even if there are lights completing their animation cycles.

            public bool waitForLightAnimationsBeforeDestroy = false;

            ParticleLight[] lights;

            // =================================	
            // Functions.
            // =================================

            // ...

            void Awake()
            {
                particleSystems =
                    gameObject.GetComponentsInChildren<ParticleSystem>();

                lights = gameObject.GetComponentsInChildren<ParticleLight>();
            }

            // ...

            void Start()
            {

            }

            // ...

            void Update()
            {

            }

            // ...

            void LateUpdate()
            {
                if (autoDestroy)
                {
                    if (!isAlive())
                    {
                        bool lightsComplete = true;

                        if (waitForLightAnimationsBeforeDestroy)
                        {
                            for (int i = 0; i < lights.Length; i++)
                            {
                                if (!lights[i].complete)
                                {
                                    lightsComplete = false; break;
                                }
                            }
                        }

                        if (lightsComplete)
                        {
                            Destroy(gameObject);
                        }
                    }
                }
            }

            // ...

            public bool isAlive()
            {
                for (int i = 0; i < particleSystems.Length; i++)
                {
                    if (particleSystems[i])
                    {
                        if (particleSystems[i].IsAlive())
                        {
                            return true;
                        }
                    }
                }

                return false;
            }

            // ...

            public int getParticleCount()
            {
                int pcount = 0;

                for (int i = 0; i < particleSystems.Length; i++)
                {
                    if (particleSystems[i])
                    {
                        pcount += particleSystems[i].particleCount;
                    }
                }

                return pcount;
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
