using EA.Infra.FILE.FileConfig;
using EA.NetDevPack.Context;
using EA.NetDevPack.Mediator;
using ES.Api.FILE.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;

namespace EA.Api.FILE.Controllers
{
    [Route("api")]
    public class UploadController : BaseController
    {
        public UploadController(IMediatorHandler mediator, IContextUser context, ILogger<UploadController> logger, IOptions<FileUploadConfig> fileUploadConfig)
            : base(mediator, context, logger, fileUploadConfig)
        {
        }

        [RequestSizeLimit(100000000)]
        [HttpPost("file/upload")]
        public async Task<IActionResult> UploadFile()
        {
            return await Upload(fileUploadConfig.Value.CdnFilePath, fileUploadConfig.Value.FileAllowExtensions);
        }

        [RequestSizeLimit(100000000)]
        [HttpPost("image/upload")]
        public async Task<IActionResult> UploadImage()
        {
            return await Upload(fileUploadConfig.Value.CdnImagePath, fileUploadConfig.Value.ImageAllowExtensions);
        }

        [RequestSizeLimit(100000000)]
        [HttpPost("file/upload-mutil")]
        public async Task<IActionResult> UploadMutilFile()
        {
            return await UploadMutil(fileUploadConfig.Value.CdnFilePath, fileUploadConfig.Value.FileAllowExtensions);
        }

        [RequestSizeLimit(100000000)]
        [HttpPost("image/upload-mutil")]
        public async Task<IActionResult> UploadMutilImage()
        {
            return await UploadMutil(fileUploadConfig.Value.CdnImagePath, fileUploadConfig.Value.ImageAllowExtensions);
        }
    }
}
