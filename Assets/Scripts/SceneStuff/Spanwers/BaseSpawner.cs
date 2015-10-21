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

        public enum BaseType
        {
            TEAM_ALPHA,
            TEAM_OMEGA,
            FFA_ONLY
        }

        [Header("Base Settings")]
        public BaseType baseType;

#if UNITY_EDITOR
        [Header("Editor Only")]
        public bool overrideBaseType = false;

        public enum Faction
        {
            NONE,
            NAVY,
            PIRATES,
            TINKERERS,
            VIKINGS,
        }
        public Faction overrideFaction = Faction.NONE;
#endif

        private ScoreManager m_scoreManager;

        public void Awake()
        {
            // Ensure prefabs are set
            if (navyBase        == null ||
                piratesBase     == null ||
                tinkerersBase   == null ||
                vikingsBase     == null)
            {
                Debug.LogError("Prefabs not correctly set!");
            }

            // Get reference to score manager within the scene
            m_scoreManager = FindObjectOfType<ScoreManager>();

            if (m_scoreManager == null)
            {
                Debug.LogError("Unable to find the Score manager within the scene!");
            }
        }

        void Start()
        {
#if UNITY_EDITOR
            // Spawn overrided base type
            if (overrideBaseType)
            {
                SpawnBase(overrideFaction);
            }
#endif

            // TODO: Spawn base using settings from main menu
            DestroyImmediate(this.gameObject);
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
            if (baseType == BaseType.FFA_ONLY &&
                m_scoreManager.gameType != EGameType.FreeForAll)
            {
                // Don't spawn FFA base in teams gamemode
                return;
            }
            else if ((baseType == BaseType.TEAM_ALPHA || baseType == BaseType.TEAM_OMEGA) &&
                     m_scoreManager.gameType != EGameType.TeamGame)
            {
                // Don't spawn Team base in FFA gamemode
                return;
            }

            GameObject baseObject = Instantiate(prefab, transform.position, transform.rotation) as GameObject;
            baseObject.tag = this.tag;

            switch (m_scoreManager.gameType)
            {
                case EGameType.FreeForAll:
                    SetupBaseForFFA(baseObject);
                    break;

                case EGameType.TeamGame:
                    SetupBaseForTeams(baseObject);
                    break;
            }
        }

        private void SetupBaseForTeams(GameObject a_base)
        {
            switch (baseType)
            {
                case BaseType.FFA_ONLY:
                    Debug.LogWarning(string.Format("SetupBaseForTeams() was called, but base setting for {0} is for FFA", name));
                    return;

                case BaseType.TEAM_ALPHA:
                    // Log any extra team bases
                    if (m_scoreManager.teamBaseAlpha != null)
                    {
                        Debug.LogWarning("Potentially multiple Team Alpha bases within scene...");
                        return;
                    }

                    // Let score manager know of spawned base
                    m_scoreManager.teamBaseAlpha = a_base.GetComponent<PirateBaseIdentity>();

                    break;

                case BaseType.TEAM_OMEGA:
                    // Log any extra team bases
                    if (m_scoreManager.teamBaseOmega != null)
                    {
                        Debug.LogWarning("Potentially multiple Team Omega bases within scene...");
                        return;
                    }

                    // Let score manager know of spawned base
                    m_scoreManager.teamBaseOmega = a_base.GetComponent<PirateBaseIdentity>();

                    break;
            }
        }

        private void SetupBaseForFFA(GameObject a_base)
        {
#if UNITY_EDITOR
            bool tagSet = false;
            if (CompareTag("Player1_"))
            {
                tagSet = true;
            }
            else if (CompareTag("Player2_"))
            {
                tagSet = true;
            }
            else if (CompareTag("Player3_"))
            {
                tagSet = true;
            }
            else if (CompareTag("Player4_"))
            {
                tagSet = true;
            }

            if (!tagSet)
            {
                Debug.LogError(string.Format("Tag not set for base spawner! ({0})", this.name));
                return;
            }
#endif

            if (m_scoreManager.pirateBase1 == null)
            {
                m_scoreManager.pirateBase1 = a_base.GetComponent<PirateBaseIdentity>();
            }
            else if (m_scoreManager.pirateBase2 == null)
            {
                m_scoreManager.pirateBase2 = a_base.GetComponent<PirateBaseIdentity>();
            }
            else if (m_scoreManager.pirateBase3 == null)
            {
                m_scoreManager.pirateBase3 = a_base.GetComponent<PirateBaseIdentity>();
            }
            else if (m_scoreManager.pirateBase4 == null)
            {
                m_scoreManager.pirateBase4 = a_base.GetComponent<PirateBaseIdentity>();
            }
            else
            {
                Debug.LogWarning("Potentially too many bases within scene for FFA");
            }
        }
    }
}
