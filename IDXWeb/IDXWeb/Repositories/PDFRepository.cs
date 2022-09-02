using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core;
using System.Linq;
using Dapper;
using IDXWeb.Models;

namespace IDXWeb.Repositories
{
    public class PDFRepository : RepositoryBase, IPDFRepository
    {
        public PDFRepository(IDbTransaction transaction) : base(transaction)
        {
        }

        public void Save(PDF_CMS item)
        {
            if (item == null)
                throw new ArgumentNullException("PDF_CMS");

            //var pdf = GetByUmbracoId(item.JMSXGroupId);

            if (item.id != null)
            {
                Update(item);
            }
            else
            {
                Add(item);
            }
        }

        public void Add(PDF_CMS item)
        {
            if (item == null)
                throw new ArgumentNullException("PDF_CMS");

            item.id = Connection.ExecuteScalar<int>(
                "INSERT INTO dbo.PDF_CMS(PDFFilename, IsHidden, CMSPosition, FullSavePath, JMSXGroupId, CorrelationID, IsAttachment, OriginalFilename) " +
                "VALUES(@PDFFilename, @IsHidden, @CMSPosition, @FullSavePath, @JMSXGroupId, @CorrelationID, @IsAttachment, @OriginalFilename); " +
                "SELECT SCOPE_IDENTITY();",
                new
                {
                    item.PDFFilename,
                    item.IsHidden,
                    item.CMSPosition,
                    item.FullSavePath,
                    item.JMSXGroupId,
                    item.CorrelationID,
                    item.IsAttachment,
                    item.OriginalFilename
                },
                Transaction
            );
        }

        public void Update(PDF_CMS item)
        {
            if (item == null)
                throw new ArgumentNullException("PDF_CMS");

            Connection.Execute(
                "UPDATE dbo.PDF_CMS " +
                "SET PDFFilename = @PDFFilename, IsHidden = @IsHidden, FullSavePath = @FullSavePath, CorrelationID = @CorrelationID, IsAttachment = @IsAttachment, OriginalFilename = @OriginalFilename " +
                "WHERE JMSXGroupId = @JMSXGroupId AND id= @id",
                new
                {
                    item.PDFFilename,
                    item.IsHidden,
                    item.FullSavePath,
                    item.CorrelationID,
                    item.IsAttachment,
                    item.OriginalFilename,
                    item.JMSXGroupId,
                    item.id
                },
                Transaction
            );
        }

        public void Remove(int id)
        {
            Connection.Execute(
                "DELETE FROM dbo.PDF_CMS WHERE id = @id",
                new { id },
                Transaction
            );
        }

        public void Remove(PDF_CMS item)
        {
            Connection.Execute(
                "DELETE FROM dbo.PDF_CMS WHERE id = @id",
                new { item.id },
                Transaction
            );
        }

        public PDF_CMS GetByUmbracoId(string itemId)
        {
            return Connection.Query<PDF_CMS>(
                "SELECT * FROM dbo.PDF_cms WHERE JMSXGroupId = @itemId",
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

        public List<PDF_CMS> GetListByUmbracoId(string itemId)
        {
            return Connection.Query<PDF_CMS>(
                "SELECT * FROM dbo.PDF_cms WHERE JMSXGroupId = @itemId",
                new { itemId },
                Transaction
            ).ToList();
        }

        public void RemoveListByUmbracoId(string itemId)
        {
            var items = GetListByUmbracoId(itemId);

            if (items == null) throw new ObjectNotFoundException("Entity not found with Umbraco Id : " + itemId);
            foreach (var item in items)
            {
                Remove(item);
            }
        }
    }
}