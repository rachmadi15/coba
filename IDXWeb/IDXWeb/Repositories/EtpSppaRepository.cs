using System;
using System.Data;
using System.Data.Entity.Core;
using System.Linq;
using Dapper;
using IDXWeb.Entities;
using IDXWeb.Entities.EtpSppa;

namespace IDXWeb.Repositories {
    public class SppaProfileRepository : RepositoryBase, ISppaProfileRepository {
        public SppaProfileRepository(IDbTransaction transaction) : base(transaction) {}

        public void Save(SppaProfile entity) {
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

        public void Add(SppaProfile entity) {
            if (entity == null)
                throw new ArgumentNullException("entity");

            
            entity.Id = Connection.ExecuteScalar<int>(
                @"INSERT INTO 
SPPA.Profile
(
[ContentId],
[Code],
[Name],
[Address],
[City],
[PostalCode],
[Phone],
[Fax],
[Website],
[Email],
[STDP],
[User],
[UpdatedDate],
[CreatedDate],
[ImageUrl]
)
VALUES(
@ContentId,
@Code,
@Name,
@Address,
@City,
@PostalCode,
@Phone,
@Fax,
@Website,
@Email,
@STDP,
@User,
@UpdatedDate,
@CreatedDate,
@ImageUrl
); SELECT SCOPE_IDENTITY()",
                new {
                    entity.ContentId,
                    entity.Code,
                    entity.Name,
                    entity.Address,
                    entity.City,
                    entity.PostalCode,
                    entity.Phone,
                    entity.Fax,
                    entity.Website,
                    entity.Email,
                    entity.STDP,
                    entity.User,
                    entity.UpdatedDate,
                    entity.CreatedDate,
                    entity.ImageUrl
                },
                Transaction
            );
        }

        public void Remove(int ContentId) {
            Connection.Execute(
                "DELETE FROM [SPPA].[Profile] WHERE [ContentId] = @ContentId",
                new { ContentId },
                Transaction
            );
        }

        public void Remove(SppaProfile entity) {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Remove(entity.ContentId);
        }

        public void Update(SppaProfile entity) {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Connection.Execute(
                $@"UPDATE [SPPA].[Profile] SET 
[Code] = @Code,
[Name] = @Name,
[City] = @City,
[Address] = @Address,
[PostalCode] = @PostalCode,
[Phone] = @Phone,
[Fax] = @Fax,
[Website] = @Website,
[Email] = @Email,
[User] = @User,
[STDP] = @STDP,
[UpdatedDate] = @UpdatedDate,
[ImageUrl] = @ImageUrl
WHERE [ContentId] = @ContentId",
                new {
                    entity.Code,
                    entity.Name,
                    entity.City,
                    entity.Address,
                    entity.PostalCode,
                    entity.Phone,
                    entity.Fax,
                    entity.Website,
                    entity.Email,
                    entity.User,
                    entity.STDP,
                    entity.UpdatedDate,
                    entity.ImageUrl,
                    entity.ContentId
                },
                Transaction
            );
        }

        public SppaProfile GetByUmbracoId(int ContentId) {
            return Connection.Query<SppaProfile>(
                "SELECT * FROM [SPPA].[Profile] WHERE [ContentId] = @ContentId",
                new { ContentId },
                Transaction
            ).FirstOrDefault();
        }

        public void RemoveByUmbracoId(int ContentId) {
            var entity = GetByUmbracoId(ContentId);

            if (entity == null) throw new ObjectNotFoundException("Entity not found with Umbraco Id : " + ContentId);

            Remove(entity);
        }

        public SppaProfile GetByUmbracoId(string itemId) {
            throw new NotImplementedException();
        }

        public void RemoveByUmbracoId(string itemId) {
            throw new NotImplementedException();
        }
    }
}