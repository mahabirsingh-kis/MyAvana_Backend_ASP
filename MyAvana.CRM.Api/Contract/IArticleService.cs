using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Contract
{
	public interface IArticleService
	{
		BlogArticleModel GetArticles();
		BlogArticle UploadArticles(BlogArticle blogArticle);
		BlogArticle GetArticleById(BlogArticle blogArticle);
        bool DeleteArticle(BlogArticle blogArticle);
    }
}
