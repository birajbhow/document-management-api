using System;

namespace DocumentManagementApi.Models
{
    public class AppFile
    {
        public string Id { get; set; }

        public byte[] Content { get; set; }
                
        public string EncodedName { get; set; }
        
        public long Size { get; set; }

        public DateTime UploadDate { get; set; }
    }
}
