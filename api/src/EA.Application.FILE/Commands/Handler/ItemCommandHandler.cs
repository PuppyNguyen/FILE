using EA.Domain.FILE.Interfaces;
using EA.Domain.FILE.Models;
using EA.Infra.FILE.Repository;
using EA.NetDevPack.Messaging;
using FluentValidation.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EA.Application.FILE.Commands
{
    internal class ItemCommandHandler : CommandHandler, IRequestHandler<ItemAddCommand, ValidationResult>, IRequestHandler<ItemDeleteCommand, ValidationResult>, IRequestHandler<ItemEditCommand, ValidationResult>
    {
        private readonly IItemRepository _wardRepository;

        public ItemCommandHandler(IItemRepository wardRepository)
        {
            _wardRepository = wardRepository;
        }
        public void Dispose()
        {
            _wardRepository.Dispose();
        }

        public async Task<ValidationResult> Handle(ItemAddCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_wardRepository)) return request.ValidationResult;
            var ward = new Item
            {
                Id = request.Id,
                Name = request.Name,
                Cdn= request.Cdn,
                Content= request.Content,
                Description= request.Description,
                HasChild= request.HasChild,
                IsFile= request.IsFile,
                LocalPath= request.LocalPath,
                MimeType= request.MimeType,
                ParentId= request.ParentId,
                Product= request.Product,
                Size= request.Size,
                Status= request.Status,
                Tenant= request.Tenant,
                Title= request.Title,
                Workspace= request.Workspace,
                CreatedBy = request.CreatedBy,
                CreatedDate = request.CreatedDate
            };

            //add domain event
            //ward.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _wardRepository.Add(ward);
            return await Commit(_wardRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ItemDeleteCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_wardRepository)) return request.ValidationResult;
            var ward = new Item
            {
                Id = request.Id
            };

            //add domain event
            //ward.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _wardRepository.Remove(ward);
            return await Commit(_wardRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ItemEditCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_wardRepository)) return request.ValidationResult;
            var data = await _wardRepository.GetById(request.Id);
            var ward = new Item
            {
                Id = request.Id,
                Name = request.Name,
                Cdn = request.Cdn,
                Content = request.Content,
                Description = request.Description,
                HasChild = request.HasChild,
                IsFile = request.IsFile,
                LocalPath = request.LocalPath,
                MimeType = request.MimeType,
                ParentId = request.ParentId,
                Product = request.Product,
                Size = request.Size,
                Status = request.Status,
                Tenant = request.Tenant,
                Title = request.Title,
                Workspace = request.Workspace,
                CreatedBy = data.CreatedBy,
                CreatedDate = data.CreatedDate,
                UpdatedBy = request.UpdatedBy,
                UpdatedDate = request.UpdatedDate
            };

            //add domain event
            //ward.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _wardRepository.Update(ward);
            return await Commit(_wardRepository.UnitOfWork);
        }
    }
}
