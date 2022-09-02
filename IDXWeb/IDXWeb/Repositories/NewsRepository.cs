using System;
using System.Data;
using System.Data.Entity.Core;
using System.Linq;
using Dapper;
using IDXWeb.Entities;

namespace IDXWeb.Repositories
{
    public class NewsRepository : RepositoryBase, INewsRepository
    {
        public NewsRepository(IDbTransaction transaction) : base(transaction)
        {
        }

        public void Save(News entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var newsDb = GetByUmbracoId(entity.ItemID);

            if (newsDb != null)
            {
                Update(entity);
            }
            else
            {
                Add(entity);
            }
        }

        public void Add(News entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            entity.ID = Connection.ExecuteScalar<int>(
                "INSERT INTO dbo.BEJ_News(ItemID, TanggalPublish, ImageURL, Locale, Judul, Ringkasan, Isi, Folder, Files, Tags, isHeadLine) " +
                "VALUES(@ItemID, @TanggalPublish, @ImageURL, @Locale, @Judul, @Ringkasan, @Isi, @Folder, @File, @Tags, @IsHeadLine); SELECT SCOPE_IDENTITY()",
                new
                {
                    entity.ItemID,
                    entity.TanggalPublish,
                    entity.ImageURL,
                    entity.Locale,
                    entity.Judul,
                    entity.Ringkasan,
                    entity.Isi,
                    entity.Folder,
                    entity.File,
                    entity.Tags,
                    entity.IsHeadLine
                },
                Transaction
            );
        }

        public void Remove(int id)
        {
            Connection.Execute(
                "DELETE FROM dbo.BEJ_News WHERE ID = @id",
                new { id },
                Transaction
            );
        }

        public void Remove(News entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Remove(entity.ID);
        }

        public void Update(News entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Connection.Execute(
                "UPDATE dbo.BEJ_News " +
                "SET ItemID = @ItemID, TanggalPublish = @TanggalPublish, ImageURL = @ImageURL, Locale = @Locale, Judul = @Judul, Ringkasan = @Ringkasan, " +
                "Isi = @Isi, Folder = @Folder, Files = @File, Tags = @Tags, isHeadLine = @IsHeadLine " +
                "WHERE ItemID = @ItemID",
                new
                {
                    entity.ItemID,
                    entity.TanggalPublish,
                    entity.ImageURL,
                    entity.Locale,
                    entity.Judul,
                    entity.Ringkasan,
                    entity.Isi,
                    entity.Folder,
                    entity.File,
                    entity.Tags,
                    entity.IsHeadLine
                },
                Transaction
            );
        }

        public News GetByUmbracoId(string itemId)
        {
            return Connection.Query<News>(
                "SELECT * FROM dbo.BEJ_News WHERE ItemID = @itemId",
                new { itemId },
                Transaction
            ).FirstOrDefault();
        }

        public void RemoveByUmbracoId(string itemId)
        {
            var entity = GetByUmbracoId(itemId);

            if (entity == null) throw new ObjectNotFoundException("Entity not found with Umbraco Id : " + itemId);

            Remove(entity);
        }
    }
}