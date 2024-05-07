using UnityEngine;

namespace UltimateAttributesPack
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false)]
    public class GetComponentInObjectAttribute : PropertyAttribute
    {
        public readonly bool CreateOneIfEmpty;

        /// <summary>
        /// Automatically takes the component in the game object and set the variable with it (works with object reference, exposed reference and managed reference). <br></br>
        /// /!\ Can't be used on arrays or lists /!\
        /// </summary>
        /// <param name="createOneIfEmpty">Is the component created and added to the game object automatically if there is none</param>
        public GetComponentInObjectAttribute(bool createOneIfEmpty = false)
        {
            CreateOneIfEmpty = createOneIfEmpty;
        }
    }
}