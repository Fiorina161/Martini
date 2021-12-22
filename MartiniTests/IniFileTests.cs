using System;
using System.Linq;
using Martini;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MartiniTests
{
    [TestClass]
    public class IniFileTests
    {
        private const string INI_CONTENT = @"
                      ################################
                      # key0=value0
                      # [sectionA] 
                      # key1=value1
                      # [sectionB] 
                      # key1=value1
                      # key2=value2
                      #
                      # [sectionC]
                      # {This is a comment}
                      # key1=value1
                      #
                      # {This is another comment}
                      # <udp|tcp|icmp>
                      # protocol=tcp
                      ################################
                      
                      key0=banana

                      [sectionC]
                      key1=kiwi
                      protocol=nil";

        [TestMethod]
        public void GetSectionNames() {
	        var sut = new IniParser();
	        sut.Parse(INI_CONTENT);

	        var sectionNames = sut.GetSectionNames().ToArray();
            CollectionAssert.AreEquivalent(new []{"","sectionA","sectionB","sectionC"},sectionNames);
        }

        [TestMethod]
        public void GetKeyNames() {
	        var sut = new IniParser();
	        sut.Parse(INI_CONTENT);

	        var keyNames = sut.GetKeyNames("sectionB").ToArray();
	        CollectionAssert.AreEquivalent(new []{"key1","key2"},keyNames);
        }

        [TestMethod]
        public void GetSpecificKey() {
	        var sut = new IniParser();
	        sut.Parse(INI_CONTENT);
            var entry = sut.GetIniEntry("","key0");
            Assert.AreEqual("value0", entry.DefaultValue);
            Assert.AreEqual("banana", entry.CurrentValue);
            Assert.AreEqual("banana", entry.CurrentOrDefault);
            Assert.AreEqual("", entry.Note);
            Assert.AreEqual(0, entry.AllowedValues.Length);
        }

        [TestMethod]
        public void WithNoEffectiveValue()
        {
	        var sut = new IniParser();
	        sut.Parse(INI_CONTENT);
	        var entry = sut.GetIniEntry("sectionA","key1");
	        Assert.AreEqual("value1", entry.DefaultValue);
	        Assert.IsNull(entry.CurrentValue);
	        Assert.AreEqual("value1", entry.CurrentOrDefault);
	        Assert.AreEqual("", entry.Note);
	        Assert.AreEqual(0, entry.AllowedValues.Length);
        }

        [TestMethod]
        public void WithNote()
        {
	        var sut = new IniParser();
	        sut.Parse(INI_CONTENT);
	        var entry = sut.GetIniEntry("sectionC","key1");
	        Assert.AreEqual("value1", entry.DefaultValue);
	        Assert.AreEqual("kiwi", entry.CurrentValue);
	        Assert.AreEqual("kiwi", entry.CurrentOrDefault);
	        Assert.AreEqual("This is a comment", entry.Note);
	        Assert.AreEqual(0, entry.AllowedValues.Length);
        }

        [TestMethod]
        public void WithAllowedValues()
        {
	        var sut = new IniParser();
	        sut.Parse(INI_CONTENT);
	        var entry = sut.GetIniEntry("sectionC","protocol");
	        Assert.AreEqual("tcp", entry.DefaultValue);
	        Assert.AreEqual("nil", entry.CurrentValue);
	        Assert.AreEqual("udp", entry.CurrentOrDefault);
	        Assert.AreEqual(3, entry.AllowedValues.Length);
        }
    }
}
