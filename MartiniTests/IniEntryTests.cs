using Martini;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MartiniTests
{
    [TestClass]
    public class IniEntryTests
    {
        [TestMethod]
        public void EmptyEntry()
        {
            var sut = new IniEntry("", "", "", "", new string[] { });
            Assert.IsFalse(sut.HasNote);
            Assert.IsFalse(sut.HasChanged);
            Assert.IsFalse(sut.IsRestricted);
            Assert.IsFalse(sut.IsBoolean);
            Assert.IsTrue(sut.IsGlobal);
            Assert.AreEqual(sut.CurrentOrDefault, "");
        }

        [TestMethod]
        public void InvalidRestrictedDefault()
        {
            var sut = new IniEntry("Section", "Key", "DefaultValue", "Note", new[] { "A", "B", "C" });
            Assert.AreEqual("A", sut.CurrentOrDefault);
        }

        [TestMethod]
        public void InvalidRestrictedDefaultAndValue()
        {
	        var sut = new IniEntry("Section", "Key", "DefaultValue", "Note", new[] { "A", "B", "C" });
	        sut.CurrentValue = "InvalidValue";
	        Assert.AreEqual("A", sut.CurrentOrDefault);
        }
    }
}
