
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

        public class PerpetualParticleManager : ParticleManager
        {
            // =================================	
            // Nested classes and structures.
            // =================================

            // ...

            // =================================	
            // Variables.
            // =================================

            // ...

            // =================================	
            // Functions.
            // =================================

            // ...

            new void Awake()
            {
                base.Awake();
                particlePrefabs[currentParticlePrefab].gameObject.SetActive(true);
            }

            // ...

            new void Start()
            {
                base.Start();
            }

            // ...

            public new void next()
            {
                particlePrefabs[currentParticlePrefab].gameObject.SetActive(false);

                base.next();
                particlePrefabs[currentParticlePrefab].gameObject.SetActive(true);
            }
            public new void previous()
            {
                particlePrefabs[currentParticlePrefab].gameObject.SetActive(false);

                base.previous();
                particlePrefabs[currentParticlePrefab].gameObject.SetActive(true);
            }

            // ...

            new void Update()
            {
                base.Update();
            }

            // ...

            public override int getParticleCount()
            {
                // Return particle count from active prefab.

                return particlePrefabs[currentParticlePrefab].getParticleCount();
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

// =================================	
// --END-- //
// =================================
