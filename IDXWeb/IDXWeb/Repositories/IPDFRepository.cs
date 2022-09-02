using System.Collections.Generic;
using IDXWeb.Models;

namespace IDXWeb.Repositories
{
    public interface IPDFRepository : IRepositoryBase<PDF_CMS>
    {
        List<PDF_CMS> GetListByUmbracoId(string itemId);

        void RemoveListByUmbracoId(string itemId);
    }
}