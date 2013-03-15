using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.Testing
{
    public static class SerializedAssert
    {
        public static void AreEqual(object expected, object actual)
        {
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            var actualSerialized = serializer.Serialize(actual);
            var expectedSerialized = serializer.Serialize(expected);

            Assert.AreEqual(expectedSerialized, actualSerialized);
        }
    }
}
