using UnityEngine;
using System;

namespace UltimateAttributesPack
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false)]
    public class DoOnValueChangedAttribute : PropertyAttribute
    {
        readonly public string FunctionName;
        readonly public Type ClassType;

        /// <summary>
        /// Execute a function if the variable change.
        /// </summary>
        /// <param name="functionName">The name of the function that will be executed if the variable change</param>
        public DoOnValueChangedAttribute(string functionName, Type classType)
        {
            FunctionName = functionName;
            ClassType = classType;
        }
    }
}