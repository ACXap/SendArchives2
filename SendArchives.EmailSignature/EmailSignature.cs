using SendArchives.EmailSignature.Enumerations;

namespace SendArchives.EmailSignature
{
    public class EmailSignature
    {
        public string Name { get; set; }
        public TypeSignature TypeSignature { get; set; }
        public string Path { get; set; }
    }
}