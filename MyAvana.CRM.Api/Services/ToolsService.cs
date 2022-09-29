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
    public class ToolsService : IToolsService
    {
        private readonly AvanaContext _context;
        public ToolsService(AvanaContext context)
        {
            _context = context;
        }
        public List<ToolsModel> GetTools()
        {
            try
            {
                List<ToolsModel> lstProductModel = _context.Tools.Where(x => x.IsActive == true).OrderByDescending(x => x.CreatedOn).
                    Select(x => new ToolsModel
                    {
                        Id = x.Id,
                        ToolName = x.ToolName,
                        ActualName = x.ActualName,
                        BrandName = x.BrandName,
                        Image = x.Image,
                        ToolLink = x.ToolLink,
                        ToolDetails = x.ToolDetails,
                        IsActive = x.IsActive,
                        CreatedOn = x.CreatedOn,
                        Price = x.Price
                    }).ToList();

                return lstProductModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ToolsModel SaveTools(ToolsModel toolsEntity)
        {
            try
            {
                Tools tool = _context.Tools.Where(x => x.Id == toolsEntity.Id).FirstOrDefault();
                if(tool != null)
                {
                    tool.ToolName = toolsEntity.ToolName;
                    tool.ActualName = toolsEntity.ActualName;
                    tool.BrandName = toolsEntity.BrandName;
                    tool.Image = toolsEntity.Image;
                    tool.ToolLink = toolsEntity.ToolLink;
                    tool.ToolDetails = toolsEntity.ToolDetails;
                    tool.Price = toolsEntity.Price;
                }
                else
                {
                    Tools toolModel = new Tools();
                    toolModel.ToolName = toolsEntity.ToolName;
                    toolModel.ActualName = toolsEntity.ActualName;
                    toolModel.BrandName = toolsEntity.BrandName;
                    toolModel.Image = toolsEntity.Image;
                    toolModel.ToolLink = toolsEntity.ToolLink;
                    toolModel.ToolDetails = toolsEntity.ToolDetails;
                    toolModel.IsActive = true;
                    toolModel.CreatedOn = DateTime.Now;
                    toolModel.Price = toolsEntity.Price;
                    _context.Add(toolModel);

                }
                _context.SaveChanges();
                return toolsEntity;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public ToolsModel GetToolsById(ToolsModel toolsModel)
        {
            try
            {
                ToolsModel toolsEntity = _context.Tools.Where(x => x.Id == toolsModel.Id).
                    Select(x => new ToolsModel {
                        Id = x.Id,
                        ToolName = x.ToolName,
                        ActualName = x.ActualName,
                        BrandName = x.BrandName,
                        Image = x.Image,
                        ToolLink = x.ToolLink,
                        ToolDetails = x.ToolDetails,
                        IsActive = x.IsActive,
                        CreatedOn = x.CreatedOn,
                        Price = x.Price,
                        ActualPrice = x.Price.ToString().Substring(0, x.Price.ToString().IndexOf('.')),
                        DecimalPrice = x.Price.ToString().Substring(x.Price.ToString().IndexOf('.') + 1)
                    }).FirstOrDefault();
                _context.SaveChanges();
                return toolsEntity;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool DeleteTool(ToolsModel toolsModel)
        {
            try
            {
                var tool = _context.Tools.FirstOrDefault(x => x.Id == toolsModel.Id);
                {
                    if (tool != null)
                    {
                        tool.IsActive = false;
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
