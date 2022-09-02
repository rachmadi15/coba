using System;
using System.Data;
using System.Data.Entity.Core;
using System.Linq;
using Dapper;
using IDXWeb.Entities;
using IDXWeb.Entities.EtpSppa;

namespace IDXWeb.Repositories {
    public class SppaPengumumanRepository : RepositoryBase, ISppaPengumumanRepository {
        public SppaPengumumanRepository(IDbTransaction transaction) : base(transaction) { }

        public void Save(SppaAnnouncement entity) {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var SppaProfileDb = GetByUmbracoId(entity.ContentId);

            if (SppaProfileDb != null) {
                Update(entity);
            }
            else {
                Add(entity);
            }
        }

        public void Add(SppaAnnouncement entity) {
            if (entity == null)
                throw new ArgumentNullException("entity");


            entity.Id = Connection.ExecuteScalar<int>(
                @"INSERT INTO 
[SPPA].[Announcement]
(
[ContentId],
[ImageUrl],
[PublishedDate],
[Locale],
[Title],
[Summary],
[Content],
[UpdatedDate],
[CreatedDate],
[ContentPairId]
)
VALUES(
@ContentId,
@ImageUrl,
@TanggalPublish,
@Locale,
@Judul,
@Ringkasan,
@Isi,
@UpdatedDate,
@CreatedDate,
@ContentPairId
); SELECT SCOPE_IDENTITY()",
                new {
                    entity.ContentId,
                    entity.ImageURL,
                    entity.TanggalPublish,
                    entity.Locale,
                    entity.Judul,
                    entity.Ringkasan,
                    entity.Isi,
                    entity.UpdatedDate,
                    entity.CreatedDate,
                    entity.ContentPairId
                },
                Transaction
            );
        }

        public void Remove(int ContentId) {
            Connection.Execute(
                "DELETE FROM [SPPA].[Announcement] WHERE [ContentId] = @ContentId",
                new { ContentId },
                Transaction
            );
        }

        public void Remove(SppaAnnouncement entity) {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Remove(entity.ContentId);
        }

        public void Update(SppaAnnouncement entity) {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Connection.Execute(
                $@"UPDATE [SPPA].[Announcement] SET 
[ImageUrl] = @ImageUrl,
[PublishedDate] = @TanggalPublish,
[Locale] = @Locale,
[Title] = @Judul,
[Summary] = @Ringkasan,
[Content] = @Isi,
[ContentPairId] = @ContentPairId,
[UpdatedDate] = @UpdatedDate
WHERE [ContentId] = @ContentId",
                new {
                    entity.ImageURL,
                    entity.TanggalPublish,
                    entity.Locale,
                    entity.Judul,
                    entity.Ringkasan,
                    entity.Isi,
                    entity.ContentPairId,
                    entity.UpdatedDate,
                    entity.ContentId
                },
                Transaction
            );
        }

        public SppaAnnouncement GetByUmbracoId(int ContentId) {
            return Connection.Query<SppaAnnouncement>(
                "SELECT * FROM [SPPA].[Announcement] WHERE [ContentId] = @ContentId",
                new { ContentId },
                Transaction
            ).FirstOrDefault();
        }

        public void RemoveByUmbracoId(int ContentId) {
            var entity = GetByUmbracoId(ContentId);

            if (entity == null) throw new ObjectNotFoundException("Entity not found with Umbraco Id : " + ContentId);

            Remove(entity);
        }

        public SppaAnnouncement GetByUmbracoId(string itemId) {
            throw new NotImplementedException();
        }

        public void RemoveByUmbracoId(string itemId) {
            throw new NotImplementedException();
        }
    }
}