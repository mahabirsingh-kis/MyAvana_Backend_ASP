using Flurl;
using MyAvana.CRM.Api.Contract;
using MyAvana.DAL.Auth;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Services
{
    public class ArticleService : IArticleService
    {
        private readonly AvanaContext _context;
        public ArticleService(AvanaContext avanaContext)
        {
            _context = avanaContext;
        }
        public BlogArticleModel GetArticles()
        {
            try
            {
                List<BlogArticle> blogArticle = _context.BlogArticles.Where(x => x.IsActive == true).OrderByDescending(x => x.CreatedOn).ToList();


                BlogArticleModel blogArticleModel = new BlogArticleModel();
                blogArticleModel.Article = blogArticle;
                return blogArticleModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public BlogArticle UploadArticles(BlogArticle blogArticle)
        {
            try
            {
                if (blogArticle.BlogArticleId != 0)
                {
                    var objArticle = _context.BlogArticles.Where(x => x.BlogArticleId == blogArticle.BlogArticleId).FirstOrDefault();
                    objArticle.HeadLine = blogArticle.HeadLine;
                    objArticle.Details = blogArticle.Details;
                    objArticle.ImageUrl = blogArticle.ImageUrl;
                    objArticle.Url = blogArticle.Url;
                }
                else
                {
                    _context.BlogArticles.Add(new BlogArticle()
                    {
                        HeadLine = blogArticle.HeadLine,
                        Details = blogArticle.Details,
                        ImageUrl = blogArticle.ImageUrl,
                        Url = blogArticle.Url,
                        IsActive = true,
                        CreatedOn = DateTime.UtcNow

                    });
                }
                _context.SaveChanges();
                return blogArticle;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public BlogArticle GetArticleById(BlogArticle blogArticle)
        {
            try
            {
                BlogArticle blogArticleModel = _context.BlogArticles.Where(x => x.BlogArticleId == blogArticle.BlogArticleId).FirstOrDefault();
                return blogArticleModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool DeleteArticle(BlogArticle articleModel)
        {
            try
            {
                var objCode = _context.BlogArticles.FirstOrDefault(x => x.BlogArticleId == articleModel.BlogArticleId);
                {
                    if (objCode != null)
                    {
                        objCode.IsActive = false;
                    }
                }
                _context.SaveChanges();
                return true;
            }

            catch (Exception Ex)
            {
                return false;
            }
        }
    }
}
