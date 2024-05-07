using UnityEngine;
using System;

namespace UltimateAttributesPack
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
    public class FunctionButtonAttribute : PropertyAttribute
    {
        readonly public string Text;
        readonly public string FunctionName;
        readonly public Type ClassType;

        /// <summary>
        /// Displays a button in the inspector to execute a specific function with no parameters (only when object is selected in hierarchy). <br></br>
        /// </summary>
        /// <param name="text">The text that will be displayed on the button</param>
        /// <param name="functionName">The name of the function that will be executed</param>
        /// <param name="classType">The class type where the function is</param>
        public FunctionButtonAttribute(string text, string functionName, Type classType)
        {        
            Text = text;       
            FunctionName = functionName;
            ClassType = classType;
        }
    }
}
