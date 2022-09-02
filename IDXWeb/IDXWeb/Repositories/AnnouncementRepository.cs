using IDXWeb.Models;
using System;
using System.Data;
using System.Data.Entity.Core;
using System.Linq;
using Dapper;

namespace IDXWeb.Repositories
{
    public class AnnouncementRepository : RepositoryBase, IAnnouncementRepository
    {
        public AnnouncementRepository(IDbTransaction transaction) : base(transaction)
        {
        }

        public void Save(Announcement item)
        {
            if (item == null)
                throw new ArgumentNullException("Announcement");

            var announcementDb = GetByUmbracoId(item.ItemID);

            if (announcementDb != null)
            {
                Update(item);
            }
            else
            {
                Add(item);
            }
        }

        public void Add(Announcement item)
        {
            if (item == null)
                throw new ArgumentNullException("Announcement");

            item.Id = Connection.ExecuteScalar<int>(
                "INSERT INTO dbo.pengumuman_cms(PKId,PKIdOri,FinalID,param,ItemID,NoPengumuman,DATE,TITLE,JenisPengumuman,CODE,FormID,Lang,PerihalPengumuman,KodeDivisi,Divisi,JenisEmiten,IsHidden,JMSXGroupId,PDF_PATH,CreatedDate) " +
                "VALUES(0,0,0,0,@ItemID,@NoPengumuman,@DATE,@TITLE,@JenisPengumuman,@CODE,@FormID,@Lang,@PerihalPengumuman,@KodeDivisi,@Divisi,@JenisEmiten,@IsHidden,@JMSXGroupId,@PDF_PATH,@CreatedDate); " +
                "SELECT SCOPE_IDENTITY();",
                new
                {
                    item.ItemID,
                    item.NoPengumuman,
                    item.DATE,
                    item.TITLE,
                    item.JenisPengumuman,
                    item.CODE,
                    item.FormID,
                    item.Lang,
                    item.PerihalPengumuman,
                    item.KodeDivisi,
                    item.Divisi,
                    item.JenisEmiten,
                    item.IsHidden,
                    item.JMSXGroupId,
                    item.PDF_PATH,
                    item.CreatedDate
                },
                Transaction
            );
        }

        public void Update(Announcement item)
        {
            if (item == null)
                throw new ArgumentNullException("Announcement");

            Connection.Execute(
                "UPDATE dbo.pengumuman_cms " +
                "SET  NoPengumuman=@NoPengumuman, DATE=@DATE, TITLE=@TITLE, JenisPengumuman=@JenisPengumuman, CODE=@CODE, FormID=@FormID, Lang=@Lang, PerihalPengumuman=@PerihalPengumuman, KodeDivisi=@KodeDivisi, Divisi=@Divisi, JenisEmiten=@JenisEmiten, IsHidden=@IsHidden, JMSXGroupId=@JMSXGroupId, PDF_PATH=@PDF_PATH " +
                "WHERE ItemID = @ItemID",
                new
                {
                    item.NoPengumuman,
                    item.DATE,
                    item.TITLE,
                    item.JenisPengumuman,
                    item.CODE,
                    item.FormID,
                    item.Lang,
                    item.PerihalPengumuman,
                    item.KodeDivisi,
                    item.Divisi,
                    item.JenisEmiten,
                    item.IsHidden,
                    item.JMSXGroupId,
                    item.PDF_PATH,
                    item.ItemID
                    
                },
                Transaction
            );
        }

        public void Remove(int id)
        {
            Connection.Execute(
                "DELETE FROM dbo.pengumuman_cms WHERE id = @id",
                new { id },
                Transaction
            );
        }

        public void Remove(Announcement item)
        {
            if (item == null)
                throw new ArgumentNullException("Announcement");

            Remove(item.Id);
        }

        public Announcement GetByUmbracoId(string itemId)
        {
            return Connection.Query<Announcement>(
                "SELECT * FROM dbo.pengumuman_cms WHERE ItemID = @itemId",
                new { itemId },
                Transaction
            ).FirstOrDefault();
        }

        public void RemoveByUmbracoId(string itemId)
        {
            var item = GetByUmbracoId(itemId);

            if (item == null) throw new ObjectNotFoundException("Entity not found with Umbraco Id : " + itemId);

            Remove(item);
        }
    }
}