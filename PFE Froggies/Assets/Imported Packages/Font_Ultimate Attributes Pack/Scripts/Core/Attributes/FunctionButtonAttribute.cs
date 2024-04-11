using UnityEngine;

namespace UltimateAttributesPack
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
    public class FunctionButtonAttribute : PropertyAttribute
    {
        readonly public string Name;
        readonly public string FunctionName;
        readonly public System.Type ClassType;

        /// <summary>
        /// <param name="name"></param>
        /// <param name="functionName"></param>
        /// <param name="classType"></param>
        /// Displays a button in the inspector to open call a specific function with no parameters (only when object selected). 
        /// <br></br>
        /// Use typeof(your class) for ClassType parameter.
        /// </summary>
        public FunctionButtonAttribute(string name, string functionName, System.Type classType)
        {        
            Name = name;       
            FunctionName = functionName;
            ClassType = classType;
        }
    }
}
