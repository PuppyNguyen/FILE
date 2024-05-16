using Ezbuy.Cdn.Model;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using Microsoft.Extensions.Options;
using EA.Infra.FILE.FileConfig;
using EA.NetDevPack.Mediator;
using Microsoft.EntityFrameworkCore;
using EA.Application.FILE.Commands;
using EA.NetDevPack.Context;
using FluentValidation.Results;

namespace ES.Api.FILE.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected static readonly FormOptions DefaultFormOptions = new FormOptions();
        protected static object lockFile = new object();

        protected readonly ILogger logger;
        protected readonly IOptions<FileUploadConfig> fileUploadConfig;
        protected readonly IContextUser _context;
        protected readonly IMediatorHandler _mediator;
        public BaseController(IMediatorHandler mediator, IContextUser context, ILogger logger, IOptions<FileUploadConfig> fileUploadConfig)
        {
            this.logger = logger;
            _context = context;
            _mediator = mediator;
            this.fileUploadConfig = fileUploadConfig;
        }

        protected async Task<IActionResult> Upload(string path, params string[] allowExtensions)
        {
            try
            {
                UploadResponse response = new UploadResponse();
                DateTime dtNow = DateTime.UtcNow;
                var boundary = MultipartRequestHelper.GetBoundary(MediaTypeHeaderValue.Parse(Request.ContentType), DefaultFormOptions.MultipartBoundaryLengthLimit);
                var reader = new MultipartReader(boundary, HttpContext.Request.Body);
                var section = reader.ReadNextSectionAsync().Result;

                while (section != null)
                {
                    ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition);

                    if (MultipartRequestHelper.HasFileContentDisposition(contentDisposition))
                    {
                        var createdUid = HeaderUtilities.RemoveQuotes(contentDisposition.Name).Value;
                        if (string.IsNullOrEmpty(createdUid))
                        {
                            response.Fail(new List<DetailError>() { new DetailError(ErrorCodeDefine.FILE_CREATED_USER_ID_IS_NULL_OR_EMPTY, null) });
                            return Ok(response);
                        }

                        var fileNameUpload = HeaderUtilities.RemoveQuotes(contentDisposition.FileName).Value;
                        if (!Validate(fileNameUpload, allowExtensions))
                        {
                            response.Fail(new List<DetailError>() { new DetailError(ErrorCodeDefine.FILE_FILE_NAME_IS_NULL_OR_EMPTY, null) });
                            return Ok(response);
                        }

                        string extension = Path.GetExtension(fileNameUpload);
                        string fileName = dtNow.Ticks.ToString() + "_" + createdUid;
                        string filePath = Path.Combine(fileUploadConfig.Value.OriginalFolder, _context.Product, dtNow.Year.ToString() + dtNow.Month.ToString("00"), path);
                        if (!Directory.Exists(filePath))
                        {
                            Directory.CreateDirectory(filePath);
                        }
                        string fullFileName = Path.Combine(filePath, string.Format("{0}{1}", fileName, extension));

                        lock (lockFile)
                        {
                            int i = 0;
                            while (System.IO.File.Exists(fullFileName))
                            {
                                i++;
                                fileName += $"_{i}";
                                fullFileName = Path.Combine(filePath, string.Format("{0}{1}", fileName, extension));
                            }
                        }

                        using (var targetStream = System.IO.File.Create(fullFileName))
                        {
                            await section.Body.CopyToAsync(targetStream);
                        }

                        var itemAddCommand = new ItemAddCommand(
                             Guid.NewGuid(),
                             fullFileName,
                             fileName,
                             fileName,
                             Convert.ToInt32(section.Body.Length),
                             true,
                             Guid.Empty,
                             extension,
                             false,
                             fullFileName,
                             "",
                             _context.Product,
                             1,
                             "",
                             "",
                             _context.Tenant,
                             _context.GetUserId(),
                             DateTime.Now
                         );

                        var result = await _mediator.SendCommand(itemAddCommand);

                        if (result.Errors.Count > 0)
                        {
                            return Ok(result);
                        }
                        else
                        {
                            response.Status = true;
                            response.Name = fileNameUpload;
                            response.Path = $"{path}/{itemAddCommand.Id}";
                            response.Type = extension;
                            return Ok(response);
                        }
                    }
                    section = await reader.ReadNextSectionAsync();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
        protected async Task<IActionResult> UploadMutil(string path, params string[] allowExtensions)
        {
            try
            {
                UploadMultiResponse response = new UploadMultiResponse();
                List<InfoFileResponse> infoFileResponses = new List<InfoFileResponse>();
                DateTime dtNow = DateTime.UtcNow;
                var boundary = MultipartRequestHelper.GetBoundary(MediaTypeHeaderValue.Parse(Request.ContentType), DefaultFormOptions.MultipartBoundaryLengthLimit);
                var reader = new MultipartReader(boundary, HttpContext.Request.Body);
                var section = reader.ReadNextSectionAsync().Result;

                while (section != null)
                {
                    ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition);

                    var createdUid = HeaderUtilities.RemoveQuotes(contentDisposition.Name).Value;
                    if (string.IsNullOrEmpty(createdUid))
                    {
                        response.Fail(new List<DetailError>() { new DetailError(ErrorCodeDefine.FILE_CREATED_USER_ID_IS_NULL_OR_EMPTY, null) });
                        return Ok(response);
                    }

                    var fileNameUpload = HeaderUtilities.RemoveQuotes(contentDisposition.FileName).Value;
                    if (!Validate(fileNameUpload, allowExtensions))
                    {
                        response.Fail(new List<DetailError>() { new DetailError(ErrorCodeDefine.FILE_FILE_NAME_IS_NULL_OR_EMPTY, null) });
                        return Ok(response);
                    }

                    string extension = Path.GetExtension(fileNameUpload);
                    string fileName = dtNow.Ticks.ToString() + "_" + createdUid;
                    string filePath = Path.Combine(fileUploadConfig.Value.OriginalFolder, _context.Product, dtNow.Year.ToString() + dtNow.Month.ToString("00"), path);
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    string fullFileName = Path.Combine(filePath, string.Format("{0}{1}", fileName, extension));

                    lock (lockFile)
                    {
                        int i = 0;
                        while (System.IO.File.Exists(fullFileName))
                        {
                            i++;
                            fileName += $"_{i}";
                            fullFileName = Path.Combine(filePath, string.Format("{0}{1}", fileName, extension));
                        }
                    }

                    using (var targetStream = System.IO.File.Create(fullFileName))
                    {
                        await section.Body.CopyToAsync(targetStream);
                    }

                    var itemAddCommand = new ItemAddCommand(
                         Guid.NewGuid(),
                         fullFileName,
                         fileName,
                         fileName,
                         Convert.ToInt32(section.Body.Length),
                         true,
                         Guid.Empty,
                         extension,
                         false,
                         fullFileName,
                         "",
                         _context.Product,
                         1,
                         "",
                         "",
                         _context.Tenant,
                         _context.GetUserId(),
                         DateTime.Now
                     );

                    var result = await _mediator.SendCommand(itemAddCommand);

                    if (result.Errors.Count > 0)
                    {
                        return Ok(result);
                    }
                    else
                    {
                        infoFileResponses.Add(new InfoFileResponse()
                        {
                            Name = fileNameUpload,
                            Path = $"{path}/{itemAddCommand.Id}",
                            Type = extension,
                        });
                    }
                section = await reader.ReadNextSectionAsync();
                }
                response.infoFiles = infoFileResponses;
                response.Status = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);

                return StatusCode(500, ex.Message);
            }
        }

        private bool Validate(string fileName, params string[] extensions)
        {
            string extension = Path.GetExtension(fileName).ToLower().Trim('.');

            return extensions.Any(m => m.Equals(extension));
        }
    }
}