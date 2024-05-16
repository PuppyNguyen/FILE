using EA.Domain.FILE.Interfaces;
using EA.Domain.FILE.Models;
using EA.Infra.FILE.Context;
using EA.NetDevPack.Data;
using Microsoft.EntityFrameworkCore;

namespace EA.Infra.FILE.Repository
{
    public class ItemRepository : IItemRepository
    {
        protected readonly SqlCoreContext Db;
        protected readonly DbSet<Item> DbSet;
        public IUnitOfWork UnitOfWork => Db;
        public ItemRepository(SqlCoreContext context)
        {
            Db = context;
            DbSet = Db.Set<Item>();
        }

        public void Add(Item ward)
        {
            DbSet.Add(ward);
        }

        public void Dispose()
        {
            Db.Dispose();
        }

        public async Task<IEnumerable<Item>> GetAll()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<Item> GetById(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public void Remove(Item ward)
        {
            DbSet.Remove(ward);
        }

        public void Update(Item ward)
        {
            DbSet.Update(ward);
        }
        public async Task<IEnumerable<Item>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex)
        {
            var query = DbSet.AsQueryable();
            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword) || x.Title.Contains(keyword));
            }
            foreach (var item in filter)
            {
                if (item.Key.Equals("parentId"))
                {
                    query = query.Where(x => x.ParentId.Equals(new Guid(item.Value + "")));
                }
                if (item.Key.Equals("workspace"))
                {
                    query = query.Where(x => x.Workspace.Equals(item.Value + ""));
                }
                if (item.Key.Equals("tenant"))
                {
                    query = query.Where(x => x.Tenant.Equals(item.Value + ""));
                }
                if (item.Key.Equals("product"))
                {
                    query = query.Where(x => x.Product.Equals(item.Value + ""));
                }
                if (item.Key.Equals("status"))
                {
                    query = query.Where(x => x.Status == Convert.ToInt32(item.Value));
                }
            }
            return await query.Skip((pageindex - 1) * pagesize).Take(pagesize).ToListAsync();
        }

        public async Task<int> FilterCount(string? keyword, Dictionary<string, object> filter)
        {
            var query = DbSet.AsQueryable();
            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword) || x.Title.Contains(keyword));
            }
            foreach (var item in filter)
            {
                if (item.Key.Equals("parentId"))
                {
                    query = query.Where(x => x.ParentId.Equals(new Guid(item.Value + "")));
                }
                if (item.Key.Equals("workspace"))
                {
                    query = query.Where(x => x.Workspace.Equals(item.Value + ""));
                }
                if (item.Key.Equals("tenant"))
                {
                    query = query.Where(x => x.Tenant.Equals(item.Value + ""));
                }
                if (item.Key.Equals("product"))
                {
                    query = query.Where(x => x.Product.Equals(item.Value + ""));
                }
                if (item.Key.Equals("status"))
                {
                    query = query.Where(x => x.Status == Convert.ToInt32(item.Value));
                }
            }
            return await query.CountAsync();
        }

        public async Task<IEnumerable<Item>> GetListCbx(Dictionary<string, object> filter)
        {
            var query = DbSet.AsQueryable();
            foreach (var item in filter)
            {
                if (item.Key.Equals("parentId"))
                {
                    query = query.Where(x => x.ParentId.Equals(new Guid(item.Value + "")));
                }
                if (item.Key.Equals("workspace"))
                {
                    query = query.Where(x => x.Workspace.Equals(item.Value + ""));
                }
                if (item.Key.Equals("tenant"))
                {
                    query = query.Where(x => x.Tenant.Equals(item.Value + ""));
                }
                if (item.Key.Equals("product"))
                {
                    query = query.Where(x => x.Product.Equals(item.Value + ""));
                }
                if (item.Key.Equals("status"))
                {
                    query = query.Where(x => x.Status == Convert.ToInt32(item.Value));
                }
            }
            return await query.ToListAsync();
        }
        public async Task<bool> CheckExistById(Guid id)
        {
            return await DbSet.AnyAsync(x => x.Id == id);
        }
    }
}
