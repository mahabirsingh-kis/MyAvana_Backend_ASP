using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Contract
{
    public interface IStylistService
    {
        StylistModel AddUpdateStylist(StylistModel stylist);
        //List<StylistModel> GetStylists();
        bool DeleteStylist(StylistModel stylist);
        List<StylistSpecialty> GetStylistSpecialty();
        StylishAdminModel GetStylishAdmin(StylishAdminModel stylishAdminModel);

        List<StylistListModel> GetStylistList();
		IEnumerable<StylistListModel> AddStylistList(IEnumerable<StylistListModel> stylistData);

	}
}
