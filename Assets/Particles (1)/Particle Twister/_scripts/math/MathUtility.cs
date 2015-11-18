
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

        public static class MathUtility
        {
            // =================================	
            // Nested classes and structures.
            // =================================

            [System.Serializable]
            public class DualContainer<T>
            {
                public T a;
                public T b;
            }

            // Int range.

            [System.Serializable]
            public class RangeInt
            {
                public RangeInt()
                {
                    min = 0;
                    max = 1;
                }
                public RangeInt(int min, int max)
                {
                    this.min = min;
                    this.max = max;
                }

                public int min;
                public int max;
            }

            // Enumeration of interpolation types.

            public enum LerpType
            {
                lerpLinear,

                lerpEaseInSine, lerpEaseOutSine, lerpEaseInOutSine,
                lerpEaseInQuad, lerpEaseOutQuad, lerpEaseInOutQuad
            }


            // =================================	
            // Variables.
            // =================================

            // Pre-calcs.

            public const float PI_DIV2 = Mathf.PI / 2.0f;
            public const float PI_MULT2 = Mathf.PI * 2.0f;

            // =================================	
            // Functions.
            // =================================

            // Clamp angle.

            public static float clampAngle(float angle, float min = -360.0f, float max = 360.0f)
            {
                if (angle < min)
                {
                    angle += 360.0f;
                }
                else if (angle > max)
                {
                    angle -= 360.0f;
                }

                //angle = Mathf.Clamp(angle, -360.0f, 360.0f);

                return angle;
            }

            // Easing equations adapted from Robert Penner's easing page.
            // http://www.robertpenner.com/easing/

            public static float lerp(float from, float to, float time)
            {
                return ((to - from) * time) + from;
            }
            public static Vector3 lerp(Vector3 from, Vector3 to, float time)
            {
                return ((to - from) * time) + from;
            }

            // Pick an interpolation method.

            public static float lerp(float from, float to, float time, LerpType mode)
            {
                switch (mode)
                {
                    case LerpType.lerpLinear:
                        {
                            return (lerp(from, to, time));
                        }
                    case LerpType.lerpEaseInSine:
                        {
                            return (lerpEaseInSine(from, to, time));
                        }
                    case LerpType.lerpEaseOutSine:
                        {
                            return (lerpEaseOutSine(from, to, time));
                        }
                    case LerpType.lerpEaseInOutSine:
                        {
                            return (lerpEaseInOutSine(from, to, time));
                        }
                    case LerpType.lerpEaseInQuad:
                        {
                            return (lerpEaseInQuad(from, to, time));
                        }
                    case LerpType.lerpEaseOutQuad:
                        {
                            return (lerpEaseOutQuad(from, to, time));
                        }
                    case LerpType.lerpEaseInOutQuad:
                        {
                            return (lerpEaseInOutQuad(from, to, time));
                        }
                    default:
                        {
                            throw new System.ArgumentException("Unknown case.");
                        }
                }
            }
            public static Vector3 lerp(Vector3 from, Vector3 to, float time, LerpType mode)
            {
                switch (mode)
                {
                    case LerpType.lerpLinear:
                        {
                            return (lerp(from, to, time));
                        }
                    case LerpType.lerpEaseInSine:
                        {
                            return (lerpEaseInSine(from, to, time));
                        }
                    case LerpType.lerpEaseOutSine:
                        {
                            return (lerpEaseOutSine(from, to, time));
                        }
                    case LerpType.lerpEaseInOutSine:
                        {
                            return (lerpEaseInOutSine(from, to, time));
                        }
                    case LerpType.lerpEaseInQuad:
                        {
                            return (lerpEaseInQuad(from, to, time));
                        }
                    case LerpType.lerpEaseOutQuad:
                        {
                            return (lerpEaseOutQuad(from, to, time));
                        }
                    case LerpType.lerpEaseInOutQuad:
                        {
                            return (lerpEaseInOutQuad(from, to, time));
                        }
                    default:
                        {
                            throw new System.ArgumentException("Unknown case.");
                        }
                }
            }

            // ...

            public static float lerpEaseInSine(float from, float to, float time)
            {
                to -= from;
                return ((-to * Mathf.Cos(time * PI_DIV2)) + to) + from;
            }
            public static float lerpEaseOutSine(float from, float to, float time)
            {
                to -= from;
                return (to * Mathf.Sin(time * PI_DIV2)) + from;
            }
            public static float lerpEaseInOutSine(float from, float to, float time)
            {
                to -= from;
                return ((-to / 2.0f) * (Mathf.Cos(Mathf.PI * time) - 1.0f)) + from;
            }

            public static Vector3 lerpEaseInSine(Vector3 from, Vector3 to, float time)
            {
                to -= from;
                return ((-to * Mathf.Cos(time * PI_DIV2)) + to) + from;
            }
            public static Vector3 lerpEaseOutSine(Vector3 from, Vector3 to, float time)
            {
                to -= from;
                return (to * Mathf.Sin(time * PI_DIV2)) + from;
            }
            public static Vector3 lerpEaseInOutSine(Vector3 from, Vector3 to, float time)
            {
                to -= from;
                return ((-to / 2.0f) * (Mathf.Cos(Mathf.PI * time) - 1.0f)) + from;
            }

            // ...

            public static float lerpEaseInQuad(float from, float to, float time)
            {
                return ((to - from) * (time * time)) + from;
            }
            public static float lerpEaseOutQuad(float from, float to, float time)
            {
                return (-(to - from) * (time * (time - 2.0f))) + from;
            }
            public static float lerpEaseInOutQuad(float from, float to, float time)
            {
                if ((time *= 2.0f) < 1.0f)
                {
                    return (((to - from) / 2.0f) * (time * time)) + from;
                }

                return ((-(to - from) / 2.0f) * (((--time) * (time - 2.0f)) - 1.0f)) + from;
            }

            public static Vector3 lerpEaseInQuad(Vector3 from, Vector3 to, float time)
            {
                return ((to - from) * (time * time)) + from;
            }
            public static Vector3 lerpEaseOutQuad(Vector3 from, Vector3 to, float time)
            {
                return (-(to - from) * (time * (time - 2.0f))) + from;
            }
            public static Vector3 lerpEaseInOutQuad(Vector3 from, Vector3 to, float time)
            {
                if ((time *= 2.0f) < 1.0f)
                {
                    return (((to - from) / 2.0f) * (time * time)) + from;
                }

                return ((-(to - from) / 2.0f) * (((--time) * (time - 2.0f)) - 1.0f)) + from;
            }

            // ...

            //public static Vector3 lerpEaseInQuad(Vector3 from, Vector3 to, float time)
            //{
            //    time = time * time;
            //    return lerp(from, to, time);
            //}
            //public static Vector3 lerpEaseOutQuad(Vector3 from, Vector3 to, float time)
            //{
            //    time = time * time * time;
            //    return lerp(from, to, time);
            //}
            //public static Vector3 lerpEaseInOutQuad(Vector3 from, Vector3 to, float time)
            //{
            //    time = (3 * (time * time)) - (2 * (time * time * time));
            //    return lerp(from, to, time);
            //}

            // ...

            // Remap to range.

            public static float remap(float value, float min1, float max1, float min2, float max2)
            {
                return (value - min1) / (max1 - min1) * (max2 - min2) + min2;
            }

            // Return a spline point based on catmull-rom interpolation and time(t).

            public static float catmullRom(float p1, float p2, float p3, float p4, float time)
            {
                float t2 = time * time;
                float t3 = t2 * time;

                return
                    0.5f * ((2 * p2) + (-p1 + p3) * time +
                    (2 * p1 - 5 * p2 + 4 * p3 - p4) * t2 +
                    (-p1 + 3 * p2 - 3 * p3 + p4) * t3);
            }

            // For floating point inaccuracies. Use this for comparison instead.

            public static bool floatEquals(float a, float b, float epsilon = 0.02f)
            {
                return Mathf.Abs(a - b) < epsilon;
            }

            // Rotate around from a point around another point (pivot).

            public static Vector3 rotatePointAroundPivot(
                Vector3 point, Vector3 pivot, Quaternion angle)
            {
                return angle * (point - pivot) + pivot;
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
