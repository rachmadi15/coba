using System;
using System.Data;
using System.Data.Entity.Core;
using System.Linq;
using Dapper;
using IDXWeb.Models;

namespace IDXWeb.Repositories
{
    public class SuspensiRepository : RepositoryBase, ISuspensiRepository
    {
        public SuspensiRepository(IDbTransaction transaction) : base(transaction)
        {
        }

        public void Save(Suspensi suspensi)
        {
            if (suspensi == null)
                throw new ArgumentNullException("suspensi");

            var newsDb = GetByUmbracoId(suspensi.ItemID);

            if (newsDb != null)
            {
                Update(suspensi);
            }
            else
            {
                Add(suspensi);
            }
        }

        public void Add(Suspensi suspensi)
        {
            if (suspensi == null)
                throw new ArgumentNullException("suspensi");

            var info_type = "SPT";

            suspensi.Id = Connection.ExecuteScalar<int>(
                "INSERT INTO dbo.suspensi_cms(ItemId,IsHidden,IsAttachmentHidden,Judul,Pengumuman,Date,info_type,data_download,LangID) " +
                "VALUES(@itemId, @IsHidden, @IsAttachmentHidden, @Judul, @Pengumuman, @Date, @info_type, @data_download,@langID); " +
                "SELECT SCOPE_IDENTITY();",
                new
                {
                    suspensi.ItemID,
                    suspensi.IsHidden,
                    suspensi.IsAttachmentHidden,
                    suspensi.Judul,
                    suspensi.Pengumuman,
                    suspensi.Date,
                    info_type,
                    suspensi.data_download,
                    suspensi.LangID
                },
                Transaction
            );
        }

        public void Update(Suspensi suspensi)
        {
            if (suspensi == null)
                throw new ArgumentNullException("suspensi");

            var info_type = "SPT";

            Connection.Execute(
                "UPDATE dbo.suspensi_cms " +
                "SET Judul = @Judul, IsHidden = @IsHidden, IsAttachmentHidden = @IsAttachmentHidden, Pengumuman = @pengumuman, Date = @Date, info_type = @info_type, data_download = @data_download, LangID = @LangID " +
                "WHERE ItemID = @ItemID",
                new
                {
                    suspensi.Judul,
                    suspensi.IsHidden,
                    suspensi.IsAttachmentHidden,
                    suspensi.Pengumuman,
                    suspensi.Date,
                    info_type,
                    suspensi.data_download,
                    suspensi.LangID,
                    suspensi.ItemID
                },
                Transaction
            );
        }

        public void Remove(int id)
        {
            Connection.Execute(
                "DELETE FROM dbo.suspensi_cms WHERE id = @id",
                new { id },
                Transaction
            );
        }

        public void Remove(Suspensi suspensi)
        {
            if (suspensi == null)
                throw new ArgumentNullException("entity");

            Remove(suspensi.Id);
        }

        public Suspensi GetByUmbracoId(string itemId)
        {
            return Connection.Query<Suspensi>(
                "SELECT * FROM dbo.suspensi_cms WHERE ItemID = @itemId",
                new { itemId },
                Transaction
            ).FirstOrDefault();
        }

        public void RemoveByUmbracoId(string itemId)
        {
            var suspensi = GetByUmbracoId(itemId);

            if (suspensi == null) throw new ObjectNotFoundException("Entity not found with Umbraco Id : " + itemId);

            Remove(suspensi);
        }
    }
}