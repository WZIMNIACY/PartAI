using System.Text;

namespace FileOperations
{
    static public class FileOp
    {
        //Basic path for easer operating
        static public string basic = "";
        public static string Read(string path)
        {

            if(!File.Exists(basic + path))
            {
                throw new FileOpException("File not found: " + basic + path);
            }

            path = basic + path;
            
            string result = File.ReadAllText(path, Encoding.UTF8);
            return result;

            
        }

        public static void Write(string path, string text, bool ov = false)
        {
            path = basic + path;

            //ov = false -> override on
            //ov = true -> ovveride off

            using (StreamWriter SW = new StreamWriter(path, ov))
            {
                if (!File.Exists(path))
                    File.Create(path);
                SW.Write(text);
            }
        }


        [Serializable]
        public class FileOpException : Exception
        {
            public FileOpException() { }
            public FileOpException(string message) : base(message) { }
            public FileOpException(string message, Exception inner) : base(message, inner) { }
        }
    }
}