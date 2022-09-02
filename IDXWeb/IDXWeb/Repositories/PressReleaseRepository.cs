using System;
using System.Data;
using System.Data.Entity.Core;
using System.Linq;
using Dapper;
using IDXWeb.Entities;

namespace IDXWeb.Repositories
{
    public class PressReleaseRepository : RepositoryBase, IPressReleaseRepository
    {
        public PressReleaseRepository(IDbTransaction transaction) : base(transaction)
        {
        }

        public void Save(PressRelease entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var pressReleaseDb = GetByUmbracoId(entity.ItemID);

            if (pressReleaseDb != null)
            {
                Update(entity);
            }
            else
            {
                Add(entity);
            }
        }

        public void Add(PressRelease entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            entity.ID = Connection.ExecuteScalar<int>(
                "INSERT INTO dbo.BEJ_PressRelease(ItemID, TanggalPublish, ImageURL, Locale, Judul, Ringkasan, Isi, Folder, Files) " +
                "VALUES(@ItemID, @TanggalPublish, @ImageURL, @Locale, @Judul, @Ringkasan, @Isi, @Folder, @File); SELECT SCOPE_IDENTITY()",
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
                    entity.File
                },
                Transaction
            );
        }

        public void Remove(int id)
        {
            Connection.Execute(
                "DELETE FROM dbo.BEJ_PressRelease WHERE ID = @id",
                new { id },
                Transaction
            );
        }

        public void Remove(PressRelease entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Remove(entity.ID);
        }

        public void Update(PressRelease entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Connection.Execute(
                "UPDATE dbo.BEJ_PressRelease " +
                "SET ItemID = @ItemID, TanggalPublish = @TanggalPublish, ImageURL = @ImageURL, Locale = @Locale, Judul = @Judul, Ringkasan = @Ringkasan, " +
                "Isi = @Isi, Folder = @Folder, Files = @File " +
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
                    entity.File
                },
                Transaction
            );
        }

        public PressRelease GetByUmbracoId(string itemId)
        {
            return Connection.Query<PressRelease>(
                "SELECT * FROM dbo.BEJ_PressRelease WHERE ItemID = @itemId",
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