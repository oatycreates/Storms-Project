/**
 * File: FactionIndentifier.cs
 * Author: Andrew Barbour
 * Maintainers: Andrew Barbour
 * Created: 23/09/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Used to help indentify object by their faction
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
	public class FactionIndentifier : MonoBehaviour 
	{
		public enum Faction
        {
            NAVY,
            PIRATES,
            TINKERERS,
            VIKINGS
        }

        public Faction faction;
	}
}
