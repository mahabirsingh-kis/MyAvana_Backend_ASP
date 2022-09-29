using Microsoft.Extensions.Logging;
using MyAvana.CRM.Api.Contract;
using MyAvana.DAL.Auth;
using MyAvana.Models.ViewModels;
using MyAvanaApi.Models.Entities;
using MyAvanaApi.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Services
{
    public class PromoCodeService : IPromoCodeService
    {
        private readonly AvanaContext _context;
        public PromoCodeService(AvanaContext avanaContext)
        {
            _context = avanaContext;
        }
        public bool SavePromoCode(PromoCodeModel codeModel)
        {
            try
            {
                _context.PromoCodes.Add(new PromoCode()
                {
                    Code = codeModel.PromoCode,
                    CreatedDate = DateTime.UtcNow,
                    ExpireDate = codeModel.ExpireDate,
                    Active = true,
					StripePlanId = codeModel.StripePlanId,
					CreatedBy = "Admin",
                });
                _context.SaveChanges();
                return true;
            }

            catch (Exception Ex)
            {
                return false;
            }
        }

        public List<PromoCodeModel> GetPromoCodes()
        {
            try
            {
                List<PromoCodeModel> promoCodes = _context.PromoCodes.Where(x => x.Active == true).Select(x => new PromoCodeModel
                {
                    PromoCode = x.Code,
                    CreatedDate = x.CreatedDate,
                    ExpireDate = x.ExpireDate,
                    IsActive = x.Active
                }).OrderByDescending(x => x.CreatedDate).ToList();


                return promoCodes;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool DeletePromoCode(PromoCodeModel codeModel)
        {
            try
            {
                var objCode = _context.PromoCodes.FirstOrDefault(x => x.Code == codeModel.PromoCode);
                {
                    if (objCode != null)
                    {
                        objCode.Active = false;
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
