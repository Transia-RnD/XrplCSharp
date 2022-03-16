using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Ripple.Core.Tests
{
    public class Utils
    {
        public static JToken ParseJson(byte[] testBytes)
        {
            var utf8 = Encoding.UTF8.GetString(testBytes);
            var obj = JToken.Parse(utf8);
            return obj;
        }
        public static JObject ParseJObject(byte[] testBytes)
        {
            return (JObject)ParseJson(testBytes);
        }

        public static JArray ParseJArray(byte[] testBytes)
        {
            return (JArray)ParseJson(testBytes);
        }

        public static byte[] FileToByteArray(string fileName)
        {
            var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            var br = new BinaryReader(fs);
            var numBytes = new FileInfo(fileName).Length;
            var buff = br.ReadBytes((int)numBytes);
            return buff;
        }
    }
}
