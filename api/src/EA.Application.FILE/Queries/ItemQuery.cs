using EA.Application.FILE.DTOs;
using EA.Domain.FILE.Interfaces;
using EA.NetDevPack.Queries;
using Microsoft.IdentityModel.Tokens;

namespace EA.Application.FILE.Queries
{

    public class ItemQueryById : IQuery<ItemDto>
    {
        public ItemQueryById()
        {
        }

        public ItemQueryById(Guid itemId)
        {
            ItemId = itemId;
        }

        public Guid ItemId { get; set; }
    }
    public class ItemQueryCheckExist : IQuery<bool>
    {

        public ItemQueryCheckExist(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
    public class ItemPagingQuery : ListQuery, IQuery<PagingResponse<ItemDto>>
    {
        public ItemPagingQuery(string? keyword, ItemQueryParams itemQueryParams, int pageSize, int pageIndex) : base(pageSize, pageIndex)
        {
            Keyword = keyword;
            QueryParams = itemQueryParams;
        }

        public ItemPagingQuery(string? keyword, ItemQueryParams itemQueryParams, int pageSize, int pageIndex, SortingInfo[] sortingInfos) : base(pageSize, pageIndex, sortingInfos)
        {
            Keyword = keyword;
            QueryParams = itemQueryParams;
        }

        public string? Keyword { get; set; }
        public ItemQueryParams QueryParams { get; set; }
    }

    public class ItemQueryHandler : IQueryHandler<ItemQueryCheckExist, bool>,
                                    IQueryHandler<ItemQueryById, ItemDto>,
                                    IQueryHandler<ItemPagingQuery, PagingResponse<ItemDto>>
    {
        private readonly IItemRepository _itemRepository;
        public ItemQueryHandler(IItemRepository itemRespository)
        {
            _itemRepository = itemRespository;
        }
        public async Task<bool> Handle(ItemQueryCheckExist request, CancellationToken cancellationToken)
        {
            return await _itemRepository.CheckExistById(request.Id);
        }
        public async Task<ItemDto> Handle(ItemQueryById request, CancellationToken cancellationToken)
        {
            var item = await _itemRepository.GetById(request.ItemId);
            var result = new ItemDto()
            {
                Id = item.Id,
                Name = item.Name,
                LocalPath = item.LocalPath,
                Size = item.Size,
                Status = item.Status,
                Product = item.Product,
                Cdn = item.Cdn,
                Content = item.Content,
                Description = item.Description,
                HasChild = item.HasChild,
                IsFile = item.IsFile,
                MimeType = item.MimeType,
                ParentId = item.ParentId,
                Tenant = item.Tenant,
                Title = item.Title,
                Workspace = item.Workspace,
                UpdatedDate = item.UpdatedDate,
                UpdatedBy = item.UpdatedBy,
                CreatedBy = item.CreatedBy,
                CreatedDate = item.CreatedDate
            };
            return result;
        }

        public async Task<PagingResponse<ItemDto>> Handle(ItemPagingQuery request, CancellationToken cancellationToken)
        {
            var response = new PagingResponse<ItemDto>();
            var filter = new Dictionary<string, object>();
            if (!String.IsNullOrEmpty(request.QueryParams.ParentId))
            {
                filter.Add("parentId", request.QueryParams.ParentId);
            }
            if (!String.IsNullOrEmpty(request.QueryParams.Tenant))
            {
                filter.Add("tenant", request.QueryParams.Tenant);
            }
            if (!String.IsNullOrEmpty(request.QueryParams.Workspace))
            {
                filter.Add("workspace", request.QueryParams.Workspace);
            }
            if (!String.IsNullOrEmpty(request.QueryParams.Product))
            {
                filter.Add("product", request.QueryParams.Product);
            }
            if (request.QueryParams.Status != null)
            {
                filter.Add("status", request.QueryParams.Status);
            }
            var count = await _itemRepository.FilterCount(request.Keyword, filter);
            var items = await _itemRepository.Filter(request.Keyword, filter, request.PageSize, request.PageIndex);
            var data = items.Select(item => new ItemDto()
            {
                Id = item.Id,
                Name = item.Name,
                LocalPath = item.LocalPath,
                Size = item.Size,
                Status = item.Status,
                Product = item.Product,
                Cdn = item.Cdn,
                Content = item.Content,
                Description = item.Description,
                HasChild = item.HasChild,
                IsFile = item.IsFile,
                MimeType = item.MimeType,
                ParentId = item.ParentId,
                Tenant = item.Tenant,
                Title = item.Title,
                Workspace = item.Workspace,
                UpdatedDate = item.UpdatedDate,
                UpdatedBy = item.UpdatedBy,
                CreatedBy = item.CreatedBy,
                CreatedDate = item.CreatedDate
            });
            response.Items = data;
            response.Total = count; response.Count = count;
            response.PageIndex = request.PageIndex;
            response.PageSize = request.PageSize;
            response.Successful();
            return response;
        }
    }
}
