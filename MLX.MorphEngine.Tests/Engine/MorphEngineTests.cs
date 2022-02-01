using Microsoft.VisualStudio.TestTools.UnitTesting;
using MLX.MorphEngine.Attribute;
using MLX.MorphEngine.Engine;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace MLX.MorphEngine.Tests.Engine
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    [DataContract]
    public class Robot
    {
        [ClassMorph(TargetField = "Name", TargetType = typeof(Cyborg))]
        [DataMember(Name = "model")]
        public string Model { get; set; }

        [DataMember(Name = "serialNumber")]
        public string SerialNumber { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Cyborg
    {
        [ClassMorph(TargetField = "Model", TargetType = typeof(Robot))]
        public string Name { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Humanoid
    {
        public string Name { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Mechatron
    {
        [ClassMorph(TargetField = "ParentMechatron", TargetType = typeof(MechatronWorker))]
        [ClassMorph(TargetField = "Model", TargetType = typeof(Robot))]
        public string Name;
    }

    [ExcludeFromCodeCoverage]
    public class MechatronWorker
    {
        [ClassMorph(TargetField = "Name", TargetType = typeof(Humanoid))]
        public string Name;

        [ClassMorph(TargetField = "Name", TargetType = typeof(Mechatron))]
        public string ParentMechatron;
    }

    [ExcludeFromCodeCoverage]
    [Serializable]
    [DataContract]
    public class AttackDroid
    {
        [DataMember(Name = "callSign", Order = 1)] public string CallSign;
    }

    [ExcludeFromCodeCoverage]
    [Serializable]
    [DataContract]
    public class Gundam
    {
        [DataMember(Name = "callSign", Order = 1)]
        public string CallSign;
    }

    [TestClass]
    [ExcludeFromCodeCoverage]
    public class MorphEngineTests
    {
        [TestMethod]
        public void MorphToCyborgTest()
        {
            var actual = new Robot { Model = "XYZ" };

            var expected = actual.Morph<Cyborg>();

            Assert.AreEqual(expected.Name, actual.Model);
        }

        [TestMethod]
        public void MorhpToHumanoidNoClassMorph()
        {
            var actual = new Robot { Model = "MD-112" };
            var expected = actual.Morph<Humanoid>();

            Assert.AreNotEqual(expected.Name, actual.Model);
        }

        [TestMethod]
        public void MorhpToAttackDroidNoPropMorph()
        {
            var actual = new Humanoid { Name = "Melroy" };
            var expected = actual.Morph<AttackDroid>();

            Assert.AreNotEqual(expected.CallSign, actual.Name);
        }

        [TestMethod]
        public void MorhpFieldsTest()
        {
            var actual = new Mechatron { Name = "Melroy" };
            var expected = actual.Morph<MechatronWorker>();

            Assert.AreEqual(expected.ParentMechatron, actual.Name);
        }

        [TestMethod]
        public void MorphNonCompatibleTest()
        {
            var actual = new Robot { Model = "T20X" };
            var expected = actual.Morph<Gundam>();

            Assert.AreNotEqual(expected.CallSign, actual.Model);
        }
    }
}
