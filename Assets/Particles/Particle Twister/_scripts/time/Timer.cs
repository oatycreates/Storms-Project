
// =================================	
// Namespaces.
// =================================

using UnityEngine;
//using System.Collections;

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

        public class Timer
        {
            // =================================	
            // Nested classes and structures.
            // =================================

            // ...

            // =================================	
            // Variables.
            // =================================

            // Current time and target max.

            public float time { get; private set; }
            public float target = 1.0f;

            // Loop? Restart after every complete.

            public bool loop = true;

            // Completed a cycle?

            public bool complete { get; private set; }

            // Completed on this frame?
            // Useful for checking when NOT looping.

            // Err...actually, it might be useless after all.

            //public bool frameComplete { get; private set; }

            // =================================	
            // Functions.
            // =================================

            // ...

            //public Timer(float time = 0.0f, float target = 1.0f)
            //{
            //    this.time = time;
            //    this.target = target;

            //    complete = false;
            //}

            // ...

            public void reset()
            {
                time = 0.0f;
                //time = time - target;

                complete = false;
            }

            // ...

            public void setToComplete()
            {
                time = target;
                complete = true;
            }

            // Returns true on every complete.

            public bool update()
            {
                //frameComplete = false;

                if (!complete || loop)
                {
                    complete = false;
                    time += Time.deltaTime;

                    if (time >= target)
                    {
                        if (loop)
                        {
                            reset();
                        }
                        else
                        {
                            time = target;
                            complete = true;
                        }

                        //frameComplete = true;

                        return true;
                    }
                }

                return false;
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
