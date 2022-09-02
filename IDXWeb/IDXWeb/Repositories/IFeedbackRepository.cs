using IDXWeb.Entities;
using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace IDXWeb.Repositories
{
    public interface IFeedbackRepository : IRepositoryBase<Feedback>
    {
        IEnumerable<Feedback> Get(IContentService ContentService, IEnumerable<IPublishedContent> roots, int parentId, string start, string end);

        object GetTimelineList(IContentService ContentService, IEnumerable<IPublishedContent> roots, int parentId, string start, string end);
        object GetTodayFeedbackList(IContentService ContentService, IEnumerable<IPublishedContent> roots, int parentId, string start, string end);
        double GetOverallRating(int parentId, string start, string end);
    }
}
