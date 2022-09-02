using IDXWeb.Entities.EtpSppa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace IDXWeb.Repositories {
    public interface ISppaProfileRepository : IRepositoryBase<SppaProfile> {
         void Add(SppaProfile entity);

         void Remove(SppaProfile entity);

         void Remove(int ContentId);

         void RemoveByUmbracoId(int ContentId);

         void Save(SppaProfile entity);

         void Update(SppaProfile entity);

    }

    public interface ISppaPengumumanRepository : IRepositoryBase<SppaAnnouncement> {
        void Add(SppaAnnouncement entity);

        void Remove(SppaAnnouncement entity);

        void Remove(int ContentId);

        void RemoveByUmbracoId(int ContentId);

        void Save(SppaAnnouncement entity);

        void Update(SppaAnnouncement entity);
    }
}