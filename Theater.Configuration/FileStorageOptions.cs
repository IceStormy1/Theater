using System;

namespace Theater.Configuration
{
    public class FileStorageOptions
    {
        public bool ForcePathStyle { get; set; }
        public bool UseHttp { get; set; }
        public string ServiceUrl { get; set; }
        public string ServiceInnerUrl { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string Region { get; set; }
        public ushort MaximumNameLength { get; set; }
    }
}
