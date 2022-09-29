using Microsoft.AspNetCore.Mvc;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using MyAvanaApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Contract
{
    public interface IQuestionaire
    {
        Task<(JsonResult result, bool success, string error)> AuthenticateUser(string email);
        IEnumerable<Questionaire> SaveSurvey(IEnumerable<Questionaire> questionaires);
        IEnumerable<Questionaire> SaveSurveyAdmin(IEnumerable<Questionaire> questionaires);
        Task<List<QuestionAnswerModel>> GetQuestionnaire();
        Task<List<QuestionAnswerModel>> GetQuestionnaireAbsenceUserList();
        List<QuestionnaireCustomerList> GetQuestionnaireCustomerList();
        List<CustomerMessageList> GetCustomerMessagesList(Guid userId);
        Task<QuestionAnswerModel> GetQuestionnaireCustomer(string userId);
        bool DeleteQuest(QuestModel quest);
        List<Questionaire> GetFilledSurvey(ClaimsPrincipal user);
        List<QuestionGraph> GetQuestionsForGraph();
        CustomerMessageModel SaveCustomerMessage(CustomerMessageModel customerMessageModel);
        EmailTemplate GetCustomerEmailTemplate();
    }
}
