using EA.Application.FILE.Queries;
using EA.Domain.FILE.Interfaces;
using EA.Infra.FILE.FileConfig;
using EA.NetDevPack.Context;
using EA.NetDevPack.Mediator;
using MassTransit.Mediator;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;

namespace EA.Api.FILE.Controllers
{
    [Route("")]
    public class LinkFileController : ControllerBase
    {
        protected readonly IOptions<FileUploadConfig> _fileUploadConfig;
        protected readonly IMediatorHandler _mediator;
        public LinkFileController(IOptions<FileUploadConfig> fileUploadConfig, IItemRepository itemRepository, IMediatorHandler mediator)
        {
            _fileUploadConfig = fileUploadConfig;
            _mediator = mediator;
        }

        [AllowAnonymous]
        [ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 7200)]
        [HttpGet("image/{id}")]
        public async Task<IActionResult> GetImageAsync(Guid id)
        {
            try
            {
                var item = await _mediator.Send(new ItemQueryById(id));

                if (item == null)
                {
                    return NotFound();
                }
                else
                {
                    var stream = new FileStream(item.LocalPath, FileMode.Open);
                    var extentsion = Path.GetExtension(item.LocalPath).ToLower().Trim('.');
                    var contentType = $"image/{extentsion}";

                    return new FileStreamResult(stream, contentType);
                }
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [AllowAnonymous]
        [ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 7200)]
        [HttpGet("file/{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            try
            {
                var item = await _mediator.Send(new ItemQueryById(id));

                if (item == null)
                {
                    return NotFound();
                }
                else
                {
                    var stream = new FileStream(item.LocalPath, FileMode.Open);
                    var extentsion = Path.GetExtension(item.LocalPath).ToLower().Trim('.');
                    new FileExtensionContentTypeProvider().TryGetContentType(item.LocalPath, out var contentType);

                    return new FileStreamResult(stream, contentType);
                }
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
    }
}
