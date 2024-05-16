using System;
namespace EA.Infra.FILE.FileConfig
{
    public class FileUploadConfig
    {
        public string CdnImagePath { get; set; }
        public string OriginalFolder { get; set; }

        public string ImageAllowExtension { get; set; }

        public string[] ImageAllowExtensions
        {
            get
            {
                return SplitExtensions(ImageAllowExtension);
            }
        }

        public string CdnFilePath { get; set; }
        public string FileAllowExtension { get; set; }

        public string[] FileAllowExtensions
        {
            get
            {
                return SplitExtensions(FileAllowExtension);
            }
        }

        private string[] SplitExtensions(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return new string[0];
            }

            return input.Split(';', StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
