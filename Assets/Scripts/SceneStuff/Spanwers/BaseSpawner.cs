/**
 * File: BaseSpawner.cs
 * Author: Andrew Barbour
 * Maintainers: Andrew Barbour
 * Created: 15/10/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Spawns a player into the scene, using
 *  the settings from the Main Menu scene
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
    public class BaseSpawner : MonoBehaviour
    {
        [Header("Prefabs")]
        public GameObject navyBase;
        public GameObject piratesBase;
        public GameObject tinkerersBase;
        public GameObject vikingsBase;

#if UNITY_EDITOR
        [Header("Editor Only")]
        public bool overrideBaseType = false;

        public Faction overrideFaction = Faction.NONE;
#endif

        public enum Faction
        {
            NONE,
            NAVY,
            PIRATES,
            TINKERERS,
            VIKINGS,
        }

        public void Awake()
        {
            if (navyBase == null ||
                piratesBase == null ||
                tinkerersBase == null ||
                vikingsBase == null)
            {
                Debug.LogError("Prefabs not correctly set!");
            }
        }

        // Use this for initialization
        void Start()
        {
#if UNITY_EDITOR
            if (tag != "Player1_" ||
                tag != "Player2_" ||
                tag != "Player3_" ||
                tag != "Player4_")
            {
                Debug.LogError(string.Format("Tag not set for base spawner! ({0})", this.name));
                return;
            }

            if (overrideBaseType)
            {
                SpawnBase(overrideFaction);
            }
#endif

            DestroyImmediate(this.gameObject);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void SpawnBase(Faction a_faction)
        {
            GameObject prefab = null;

            switch (a_faction)
            {
                case Faction.NONE:
                    Debug.LogError("Can't load 'NONE' faction!");
                    return;

                case Faction.NAVY:
                    prefab = navyBase;
                    break;

                case Faction.PIRATES:
                    prefab = piratesBase;
                    break;

                case Faction.TINKERERS:
                    prefab = tinkerersBase;
                    break;

                case Faction.VIKINGS:
                    prefab = vikingsBase;
                    break;
            }

            // Spawn base and setup base
            GameObject baseObject = Instantiate(prefab, transform.position, transform.rotation) as GameObject;
            baseObject.tag = this.tag;

            // TODO: Setup bases for teamplay
        }
    }
}
