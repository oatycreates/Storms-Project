/**
 * File: MethodExtensionForMonoBehaviourTransform.cs
 * Author: http://wiki.unity3d.com/index.php/Singleton
 * Maintainers: Andrew Barbour
 * Created: 15/10/2015
 * Copyright: Attribution-ShareAlike 3.0 Unported - http://creativecommons.org/licenses/by-sa/3.0/
 * Description: Neccessary extension to use Singleton class.
 *  No changes have been made from original file.
 **/

using UnityEngine;

static public class MethodExtensionForMonoBehaviourTransform
{
    /// <summary>
    /// Gets or add a component. Usage example:
    /// BoxCollider boxCollider = transform.GetOrAddComponent<BoxCollider>();
    /// </summary>
    static public T GetOrAddComponent<T>(this Component child) where T : Component
    {
        T result = child.GetComponent<T>();
        if (result == null)
        {
            result = child.gameObject.AddComponent<T>();
        }
        return result;
    }
}
