
// =================================	
// Namespaces.
// =================================

using UnityEngine;
using UnityEditor;

using System.Linq;

using System.Collections;
using System.Collections.Generic;

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

        //[CustomEditor(typeof(ParticlePrefab))]
        //[CustomEditor(typeof(Transform))]
        //[CustomEditor(typeof(ParticleSystem))]
        //[CanEditMultipleObjects]

        public class ParticleTwister : EditorWindow
        {
            // =================================	
            // Nested classes and structures.
            // =================================

            // All values in the editor that can be changed
            // by the user and can be restored to default go here.

            public class ParticleTwisterEditorValues
            {
                // ...

                public ParticleTwisterEditorValues()
                {
                    reset();
                }

                // ...

                public void reset()
                {
                    scale = 50.0f;
                    velocity = 200.0f;
                    resolution = 64;

                    fastForwardTime = 8.0f;
                    fastForwardOnApply = true;

                    autoUpdate = true;
                    invert = false;
                }

                // Twister.

                public float scale;
                public float velocity;
                public int resolution; // Keyframe count.

                // Options.

                public float fastForwardTime;
                public bool fastForwardOnApply;

                public bool autoUpdate;

                public bool invert;

            }

            // =================================	
            // Variables.
            // =================================

            // Selected objects in editor and all the particle systems components.

            List<GameObject> selectedGameObjects = new List<GameObject>();
            List<ParticleSystem> particleSystems = new List<ParticleSystem>();

            // Scrolling view position for selected objects.

            Vector2 scrollPosition;

            ParticleTwisterEditorValues particleTwisterEditorValues;

            // For labeling and tooltips.

            GUIContent content;

            // =================================	
            // Functions.
            // =================================

            // ...

            [MenuItem("Window/Mirza Beig/Particle Twister")]
            static void showEditor()
            {
                EditorWindow.GetWindow<ParticleTwister>(false, "Mirza Beig - Particle Twister");
            }

            // ...

            void OnEnable()
            {
                particleTwisterEditorValues = new ParticleTwisterEditorValues();
            }

            // ...

            void OnSelectionChange()
            {
                // Clear for updated selection.

                particleSystems.Clear();

                // SelectedGameObjects == all objects which either have a particle system on them, or in at least one child.

                selectedGameObjects = Selection.gameObjects.Where(
                    selected => selected.GetComponentInChildren<ParticleSystem>() != null).ToList();

                // All the particle systems in all selected.

                for (int i = 0; i < selectedGameObjects.Count; i++)
                {
                    // Merge to self while ignoring duplicate components.

                    particleSystems = particleSystems.Union(
                        selectedGameObjects[i].GetComponentsInChildren<ParticleSystem>()).ToList();
                }

                Repaint();
            }

            // ...

            void applyTwisterToAll()
            {
                float scale = !particleTwisterEditorValues.invert ?
                    particleTwisterEditorValues.scale : -particleTwisterEditorValues.scale;

                for (int i = 0; i < particleSystems.Count; i++)
                {
                    particleSystems[i].applyTwister(

                        scale,
                        particleTwisterEditorValues.velocity,
                        particleTwisterEditorValues.resolution);

                    if (particleTwisterEditorValues.fastForwardOnApply)
                    {
                        simulateAndPlay(particleSystems[i]);
                    }
                }
            }

            // ...

            void simulateAndPlay(ParticleSystem particleSystem)
            {
                particleSystem.Simulate(
                    particleTwisterEditorValues.fastForwardTime, true, false);

                particleSystem.Play();
            }

            // ...

            void OnGUI()
            {
                // Twister settings.

                EditorGUILayout.Separator();

                EditorGUILayout.LabelField("- Twister Settings:", EditorStyles.boldLabel);
                EditorGUILayout.Separator();

                content = new GUIContent("Scale", "Time scale: higher values == more curves / twists.");
                particleTwisterEditorValues.scale = EditorGUILayout.Slider(content, particleTwisterEditorValues.scale, 1.0f, 100.0f);

                content = new GUIContent("Velocity", "Velocity scale: higher values == faster particles.");
                particleTwisterEditorValues.velocity = EditorGUILayout.Slider(content, particleTwisterEditorValues.velocity, 0.25f, 1000.0f);

                content = new GUIContent("Resolution", "Keyframes used per axis. Higher values == smoother curves. 64 is a decent maximum.");
                particleTwisterEditorValues.resolution = EditorGUILayout.IntSlider(content, particleTwisterEditorValues.resolution, 8, 1024);

                EditorGUILayout.Separator();

                // If any previous GUI elements were modified and set to auto-update, apply twister to all.

                if (particleTwisterEditorValues.autoUpdate)
                {
                    if (GUI.changed)
                    {
                        applyTwisterToAll();
                    }
                }

                // Extension options.

                EditorGUILayout.LabelField("- Options:", EditorStyles.boldLabel);
                EditorGUILayout.Separator();

                content = new GUIContent("Auto-Update", "Update particles on setting changes.");
                particleTwisterEditorValues.autoUpdate = EditorGUILayout.Toggle(content, particleTwisterEditorValues.autoUpdate);

                content = new GUIContent("Auto Fast-Forward", "Simulate particles forward on updates.");
                particleTwisterEditorValues.fastForwardOnApply = EditorGUILayout.Toggle(content, particleTwisterEditorValues.fastForwardOnApply);

                EditorGUILayout.Separator();

                content = new GUIContent("Fast-Forward Time (s)", "Time to simulate particles forward (in seconds).");
                particleTwisterEditorValues.fastForwardTime = EditorGUILayout.Slider(content, particleTwisterEditorValues.fastForwardTime, 0.5f, 1024.0f);

                EditorGUILayout.Separator();

                content = new GUIContent("Invert", "Invert scale factor (reverse direction).");
                particleTwisterEditorValues.invert = EditorGUILayout.Toggle(content, particleTwisterEditorValues.invert);

                EditorGUILayout.Separator();

                // Buttons to apply.

                EditorGUILayout.BeginHorizontal();
                {
                    content = new GUIContent("Apply Twister",
                        "Apply twister settings to all ParticleSystem components in select GameObjects (copy values).");

                    if (GUILayout.Button(content))
                    {
                        applyTwisterToAll();
                    }

                    content = new GUIContent("Fast-Forward",
                        "Simulate particles forward in time.");

                    if (GUILayout.Button(content))
                    {
                        for (int i = 0; i < particleSystems.Count; i++)
                        {
                            simulateAndPlay(particleSystems[i]);
                        }
                    }
                } EditorGUILayout.EndHorizontal();

                // Button for creating a new system.

                content = new GUIContent("Create New",
                    "Create a new default ParticleSystem as a twister.");

                if (GUILayout.Button(content))
                {
                    GameObject gameObject = new GameObject("Twister", typeof(ParticleSystem));
                    ParticleSystem particleSystem = gameObject.GetComponent<ParticleSystem>();

                    particleSystem.setNewParticleTwisterValues();

                    // particleSystem.applyTwister();

                    particleSystem.applyTwister(

                        particleTwisterEditorValues.scale,
                        particleTwisterEditorValues.velocity,
                        particleTwisterEditorValues.resolution);

                    //simulateAndPlay(particleSystem);
                }

                // Button for resetting editor values.

                content = new GUIContent("Reset Editor",
                    "Resets editor values to their original defaults.");

                if (GUILayout.Button(content))
                {
                    particleTwisterEditorValues.reset();
                }

                EditorGUILayout.Separator();

                // Selected objects.

                content = new GUIContent("- Selected:",
                   "Selected GameObjects must be active and contain at least one active ParticleSystem component in their hierarchy.");

                EditorGUILayout.LabelField(content, EditorStyles.boldLabel);
                EditorGUILayout.Separator();

                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
                {
                    if (selectedGameObjects.Count == 0)
                    {
                        EditorGUILayout.LabelField("Please select GameObjects with at least one ParticleSystem component.", EditorStyles.miniBoldLabel);
                    }
                    else
                    {
                        for (int i = 0; i < selectedGameObjects.Count; i++)
                        {
                            EditorGUILayout.BeginHorizontal();
                            {
                                EditorGUILayout.LabelField(">   " + selectedGameObjects[i].name, EditorStyles.miniBoldLabel);
                            }

                            EditorGUILayout.EndHorizontal();
                        }
                    }
                }

                EditorGUILayout.EndScrollView();
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
