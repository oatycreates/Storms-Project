
// =================================	
// Namespaces.
// =================================

using UnityEngine;
using System.Collections;

using System.Collections.Generic;

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

        public class ParticleManager : MonoBehaviour
        {
            // =================================	
            // Nested classes and structures.
            // =================================



            // =================================	
            // Variables.
            // =================================

            public ParticlePrefab[] particlePrefabs;
            public int currentParticlePrefab { get; set; }

            // Since I may have prefabs as children I was using to set values.
            // But I don't want to disable/enable them each time I want to run
            // the build or change values. This will auto-disable all at start.

            public bool disableChildrenAtStart = true;

            // =================================	
            // Functions.
            // =================================

            // ...

            public void Awake()
            {
                // Default.

                currentParticlePrefab = 0;

                // Find and disable all particle prefab object children.

                if (disableChildrenAtStart)
                {
                    ParticlePrefab[] particlePrefabChildren = gameObject.GetComponentsInChildren<ParticlePrefab>();

                    for (int i = 0; i < particlePrefabChildren.Length; i++)
                    {
                        particlePrefabChildren[i].gameObject.SetActive(false);
                    }
                }
            }

            // ...

            public void Start()
            {

            }

            // ...

            public void next()
            {
                currentParticlePrefab++;

                if (currentParticlePrefab > particlePrefabs.Length - 1)
                {
                    currentParticlePrefab = 0;
                }
            }

            public void previous()
            {
                currentParticlePrefab--;

                if (currentParticlePrefab < 0)
                {
                    currentParticlePrefab = particlePrefabs.Length - 1;
                }
            }

            // ...

            public string getCurrentPrefabName(bool shorten = false)
            {
                // Save object name for clarity.

                string particleSystemName = particlePrefabs[currentParticlePrefab].name;

                // Only take name from after the last underscore to the end.

                if (shorten)
                {
                    int lastIndexOfUnderscore = particleSystemName.LastIndexOf("_") + 1;
                    particleSystemName = particleSystemName.Substring(lastIndexOfUnderscore, particleSystemName.Length - lastIndexOfUnderscore);
                }

                // Return display text.

                return "PARTICLE SYSTEM: #" + (currentParticlePrefab + 1).ToString("00") +
                    " / " + particlePrefabs.Length.ToString("00") + " (" + particleSystemName + ")";
            }

            // ...

            public virtual int getParticleCount()
            {
                return 0;
            }

            // ...

            public void Update()
            {

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
