using System;
using System.Diagnostics.CodeAnalysis;

namespace MLX.MorphEngine.Attribute
{
    /// <summary>
    /// Class Morph Attribute for creating maps to morph one class to another
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]

    [ExcludeFromCodeCoverage]
    public class ClassMorphAttribute : System.Attribute
    {
        /// <summary>
        /// Target field in the final class
        /// </summary>
        public string TargetField { get; set; }

        /// <summary>
        /// Target Class type for exact matching and morphing
        /// </summary>
        public Type TargetType { get; set; }
    }
}
