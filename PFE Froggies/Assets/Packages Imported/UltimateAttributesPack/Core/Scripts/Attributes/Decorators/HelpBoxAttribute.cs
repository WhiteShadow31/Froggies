using UnityEngine;

namespace UltimateAttributesPack
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
    public class HelpBoxAttribute : PropertyAttribute
    {
        public readonly string HelpText;
        public readonly HelpBoxMessageType MessageType;

        /// <summary>
        /// Displays an help box with custom text in the inspector. 
        /// </summary>
        /// <param name="helpText">The text displayed in the help box</param>
        /// <param name="messageType">The message type of the help box</param>
        public HelpBoxAttribute(string helpText, HelpBoxMessageType messageType = HelpBoxMessageType.Info)
        {
            MessageType = messageType;
            HelpText = helpText;
        }
    }

    public enum HelpBoxMessageType
    {
        None,
        Info,
        Warning,
        Error
    }
}