namespace SearchSphere.Integrations.BlobClient.Enum
{
    public class ContentType
    {
        private readonly string _mimeType;

        private ContentType(string mimeType)
        {
            _mimeType = mimeType;
        }

        public override string ToString()
        {
            return _mimeType;
        }

        public static ContentType PLAIN => new ContentType("text/plain");
        public static ContentType PDF => new ContentType("application/pdf");
        public static ContentType DOC => new ContentType("application/msword");
        public static ContentType DOCX => new ContentType("application/vnd.openxmlformats-officedocument.wordprocessingml.document");
    }
}