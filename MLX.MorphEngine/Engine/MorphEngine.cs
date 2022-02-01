using MLX.MorphEngine.Attribute;
using System;
using System.Linq;

namespace MLX.MorphEngine.Engine
{
    /// <summary>
    /// Morphing Engine library
    /// </summary>
    public static class MorphEngine
    {
        /// <summary>
        /// Morph from an object to a target class
        /// </summary>
        /// <param name="morphObject">Object to be used for morphing</param>
        /// <typeparam name="T">Final class for morphing</typeparam>
        /// <returns>New instance of target class with empty constructor call and morphing map values used</returns>
        public static T Morph<T>(this object morphObject)
        {
            var returnType = typeof(T);

            var returnObj = Activator.CreateInstance(returnType, false);

            //Check Properties first
            var morphProperties = morphObject.GetType().GetProperties();
            var returnProperties = returnObj.GetType().GetProperties();

            //Properties of one class are set as Fields
            if (!morphProperties.Any() || !returnProperties.Any())
            {
                var morphFields = morphObject.GetType().GetFields();
                var returnFields = returnObj.GetType().GetFields();

                if (!morphFields.Any() || !returnFields.Any()) return (T)returnObj;

                foreach (var morphField in morphFields)
                {
                    var attrs = morphField.GetCustomAttributes(true);

                    foreach (var attr in attrs)
                    {
                        var classMorphAttribute = attr as ClassMorphAttribute;

                        if (classMorphAttribute == null) continue;

                        if (classMorphAttribute.TargetType != returnType) continue;

                        var foundField = returnFields.FirstOrDefault(x => x.Name.Equals(classMorphAttribute.TargetField));

                        if (foundField == null) continue;

                        foundField.SetValue(returnObj, Convert.ChangeType(morphField.GetValue(morphObject), foundField.FieldType));
                    }
                }
            }

            foreach (var morphProperty in morphProperties)
            {
                var attrs = morphProperty.GetCustomAttributes(true);

                foreach (var attr in attrs)
                {
                    var classMorphAttribute = attr as ClassMorphAttribute;

                    if (classMorphAttribute == null) continue;

                    if (classMorphAttribute.TargetType != returnType) continue;

                    var foundProperty = returnProperties.FirstOrDefault(x => x.Name.Equals(classMorphAttribute.TargetField));

                    if (foundProperty == null) continue;

                    foundProperty.SetValue(returnObj, Convert.ChangeType(morphProperty.GetValue(morphObject), foundProperty.PropertyType), null);
                }
            }

            return (T)returnObj;
        }
    }
}
