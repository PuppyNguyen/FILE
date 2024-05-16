using System;
using System.Collections.Generic;

namespace Ezbuy.Cdn.Model
{
    public class InfoFileResponse { 
        public string Name { get; set; }
        public string Path { get; set; }
        public string Type { get; set; }
    }
    public class UploadResponse : BaseResponse
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Type { get; set; }
    }

    public class UploadMultiResponse : BaseResponse
    {
        public List<InfoFileResponse> infoFiles { get; set; }
    }
}