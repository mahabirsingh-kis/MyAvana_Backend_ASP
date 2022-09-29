using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyAvana.CRM.Api.Contract;
using MyAvana.DAL.Auth;
using MyAvana.Framework.TokenService;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using MyAvanaApi.Models.Entities;
using MyAvanaApi.Models.ViewModels;
using NLog.Web.LayoutRenderers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net.Mail;
using ZendeskApi_v2.Models.Requests;
using System.Web;


namespace MyAvana.CRM.Api.Services
{
    public class QuestionaireService : IQuestionaire
    {
        private readonly AvanaContext _context;
        private readonly ITokenService _tokenService;
        private readonly UserManager<UserEntity> _userManager;
        //private readonly ILogger _logger;



        public QuestionaireService(AvanaContext avanaContext, ITokenService tokenService, UserManager<UserEntity> userManager)
        {
            _context = avanaContext;
            _tokenService = tokenService;
            _userManager = userManager;
            //_logger = logger;
        }

        public UserEntity GetUserEmail(ClaimsPrincipal claims)
        {
            Claim claim = claims.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
            if (claim.Value != "")
            {
                UserEntity user = new UserEntity();
                user.UserName = claim.Value;
                return user;
            }
            return null;
        }

        public async Task<(JsonResult result, bool success, string error)> AuthenticateUser(String email)
        {
            try
            {
                //Claim claim = user.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
                var user = await _userManager.FindByEmailAsync(email);
                UserEntityModel userEntity = new UserEntityModel();
                if (user != null)
                {
                    var loginUser = _context.Users.Where(s => s.Email.ToLower() == email.ToLower()).FirstOrDefault();

                    userEntity.Id = loginUser.Id;
                    userEntity.UserName = loginUser.UserName;
                    userEntity.NormalizedUserName = loginUser.NormalizedUserName;
                    userEntity.Email = loginUser.Email;
                    userEntity.NormalizedEmail = loginUser.NormalizedEmail;
                    userEntity.EmailConfirmed = loginUser.EmailConfirmed;
                    userEntity.PasswordHash = loginUser.PasswordHash;
                    userEntity.SecurityStamp = loginUser.SecurityStamp;
                    userEntity.PhoneNumber = loginUser.PhoneNumber;
                    userEntity.FirstName = loginUser.FirstName;
                    userEntity.LastName = loginUser.LastName;

                    return (new JsonResult(userEntity) { StatusCode = (int)HttpStatusCode.OK }, true, "");
                }
                return (new JsonResult("") { StatusCode = (int)HttpStatusCode.OK }, false, "");
            }
            catch (Exception ex)
            {
                return (new JsonResult("") { StatusCode = (int)HttpStatusCode.OK }, false, "");
            }
        }
        public IEnumerable<Questionaire> SaveSurvey1(IEnumerable<Questionaire> questionaires)
        {
            List<Questionaire> objQuestionaire = new List<Questionaire>();
            string userId = questionaires.Select(x => x.UserId).LastOrDefault();
            try
            {
                var questions1 = _context.Questionaires.Where(x => x.UserId == userId && x.QA == 2).ToList();
                if (questions1 != null && questions1.Count() != 0)
                {
                    _context.Questionaires.RemoveRange(questions1);
                    _context.SaveChanges();
                    foreach (var ques in questionaires)
                    {
                        if (ques.QuestionId == 22)
                        {
                            Questionaire questionaire = new Questionaire();
                            questionaire.QuestionId = ques.QuestionId;
                            questionaire.AnswerId = ques.AnswerId == 0 ? null : ques.AnswerId;
                            questionaire.DescriptiveAnswer = "http://customer.myavana.com/questionnaireFile/" + ques.DescriptiveAnswer;
                            questionaire.CreatedOn = DateTime.Now;
                            questionaire.IsActive = true;
                            questionaire.UserId = userId;
                            questionaire.QA = 2;
                            objQuestionaire.Add(questionaire);
                        }
                        else
                        {
                            Questionaire questionaire = new Questionaire();
                            questionaire.QuestionId = ques.QuestionId;
                            questionaire.AnswerId = ques.AnswerId == 0 ? null : ques.AnswerId;
                            questionaire.DescriptiveAnswer = ques.DescriptiveAnswer;
                            questionaire.CreatedOn = DateTime.Now;
                            questionaire.IsActive = true;
                            questionaire.UserId = userId;
                            questionaire.QA = 2;

                            objQuestionaire.Add(questionaire);
                        }
                    }
                    _context.AddRange(objQuestionaire);

                    _context.SaveChanges();
                }
                else
                {
                    var questions = _context.Questionaires.Where(x => x.UserId == userId && x.QA == 1).ToList();
                    if (questions != null && questions.Count() != 0)
                    {

                        foreach (var ques in questionaires)
                        {
                            if (ques.QuestionId == 22)
                            {
                                Questionaire questionaire = new Questionaire();
                                questionaire.QuestionId = ques.QuestionId;
                                questionaire.AnswerId = ques.AnswerId == 0 ? null : ques.AnswerId;
                                questionaire.DescriptiveAnswer = "http://customer.myavana.com/questionnaireFile/" + ques.DescriptiveAnswer;
                                questionaire.CreatedOn = DateTime.Now;
                                questionaire.IsActive = true;
                                questionaire.UserId = userId;
                                questionaire.QA = 2;
                                objQuestionaire.Add(questionaire);
                            }
                            else
                            {
                                Questionaire questionaire = new Questionaire();
                                questionaire.QuestionId = ques.QuestionId;
                                questionaire.AnswerId = ques.AnswerId == 0 ? null : ques.AnswerId;
                                questionaire.DescriptiveAnswer = ques.DescriptiveAnswer;
                                questionaire.CreatedOn = DateTime.Now;
                                questionaire.IsActive = true;
                                questionaire.UserId = userId;
                                questionaire.QA = 2;

                                objQuestionaire.Add(questionaire);
                            }
                        }
                        _context.AddRange(objQuestionaire);

                        _context.SaveChanges();
                    }

                    else
                    {
                        foreach (var ques in questionaires)
                        {
                            if (ques.QuestionId == 22)
                            {
                                Questionaire questionaire = new Questionaire();
                                questionaire.QuestionId = ques.QuestionId;
                                questionaire.AnswerId = ques.AnswerId == 0 ? null : ques.AnswerId;
                                questionaire.DescriptiveAnswer = "http://customer.myavana.com/questionnaireFile/" + ques.DescriptiveAnswer;
                                questionaire.CreatedOn = DateTime.Now;
                                questionaire.IsActive = true;
                                questionaire.UserId = userId;
                                questionaire.QA = 1;
                                objQuestionaire.Add(questionaire);
                            }
                            else
                            {
                                Questionaire questionaire = new Questionaire();
                                questionaire.QuestionId = ques.QuestionId;
                                questionaire.AnswerId = ques.AnswerId == 0 ? null : ques.AnswerId;
                                questionaire.DescriptiveAnswer = ques.DescriptiveAnswer;
                                questionaire.CreatedOn = DateTime.Now;
                                questionaire.IsActive = true;
                                questionaire.UserId = userId;
                                questionaire.QA = 1;
                                objQuestionaire.Add(questionaire);
                            }
                        }
                        _context.AddRange(objQuestionaire);

                        _context.SaveChanges();
                    }
                }


            }
            catch (Exception ex)
            {
            }

            return questionaires;
        }
        public IEnumerable<Questionaire> SaveSurvey(IEnumerable<Questionaire> questionaires)
        {
            List<Questionaire> objQuestionaire = new List<Questionaire>();
            //int QaInput=3;//= objQuestionaire[30].QA;
            int QAvalEdit = questionaires.Where(x => x.QuestionId == 22).Select(q => q.QA).FirstOrDefault();

            if (QAvalEdit > 0)
            {
                EditSurvey(questionaires);
            }
            else
            {
                int QAval;
                string userId = questionaires.Select(x => x.UserId).LastOrDefault();
                try
                {
                    var questions = _context.Questionaires.Where(x => x.UserId == userId).ToList();
                    if (questions == null || questions.Count() == 0)
                    {
                        QAval = 1;
                    }
                    else
                    {
                        QAval = questions.Select(x => x.QA).Distinct().Max();
                    }

                    foreach (var ques in questionaires)
                    {
                        if (ques.QuestionId == 22)
                        {
                            Questionaire questionaire = new Questionaire();
                            questionaire.QuestionId = ques.QuestionId;
                            questionaire.AnswerId = ques.AnswerId == 0 ? null : ques.AnswerId;
                            questionaire.DescriptiveAnswer = "http://customer.myavana.com/questionnaireFile/" + ques.DescriptiveAnswer;
                            questionaire.CreatedOn = DateTime.Now;
                            questionaire.IsActive = true;
                            questionaire.QA = QAval + 1;
                            questionaire.UserId = userId;

                            objQuestionaire.Add(questionaire);
                        }
                        else
                        {
                            Questionaire questionaire = new Questionaire();
                            questionaire.QuestionId = ques.QuestionId;
                            questionaire.AnswerId = ques.AnswerId == 0 ? null : ques.AnswerId;
                            questionaire.DescriptiveAnswer = ques.DescriptiveAnswer;
                            questionaire.CreatedOn = DateTime.Now;
                            questionaire.IsActive = true;
                            questionaire.QA = QAval + 1;
                            questionaire.UserId = userId;

                            objQuestionaire.Add(questionaire);
                        }
                    }
                    _context.AddRange(objQuestionaire);

                    _context.SaveChanges();

                }
                catch (Exception ex)
                {
                }
            }
            return questionaires;
        }

        public IEnumerable<Questionaire> EditSurvey(IEnumerable<Questionaire> questionaires)
        {
            List<Questionaire> objQuestionaire = new List<Questionaire>();

            int QAval = questionaires.Where(x => x.QuestionId == 22).Select(q => q.QA).FirstOrDefault();
            string userId = questionaires.Select(x => x.UserId).LastOrDefault();
            try
            {
                var questions = _context.Questionaires.Where(x => x.UserId == userId && x.QA == QAval).ToList();

                _context.Questionaires.RemoveRange(questions);

                foreach (var ques in questionaires)
                {
                    if (ques.QuestionId == 22)
                    {
                        Questionaire questionaire = new Questionaire();
                        questionaire.QuestionId = ques.QuestionId;
                        questionaire.AnswerId = ques.AnswerId == 0 ? null : ques.AnswerId;
                        questionaire.DescriptiveAnswer = "http://customer.myavana.com/questionnaireFile/" + ques.DescriptiveAnswer;
                        questionaire.CreatedOn = DateTime.Now;
                        questionaire.IsActive = true;
                        questionaire.QA = QAval;
                        questionaire.UserId = userId;

                        objQuestionaire.Add(questionaire);
                    }
                    else
                    {
                        Questionaire questionaire = new Questionaire();
                        questionaire.QuestionId = ques.QuestionId;
                        questionaire.AnswerId = ques.AnswerId == 0 ? null : ques.AnswerId;
                        questionaire.DescriptiveAnswer = ques.DescriptiveAnswer;
                        questionaire.CreatedOn = DateTime.Now;
                        questionaire.IsActive = true;
                        questionaire.QA = QAval;
                        questionaire.UserId = userId;

                        objQuestionaire.Add(questionaire);
                    }
                }
                _context.AddRange(objQuestionaire);

                _context.SaveChanges();

            }
            catch (Exception ex)
            {
            }

            return questionaires;
        }

        public IEnumerable<Questionaire> SaveSurveyAdmin(IEnumerable<Questionaire> questionaires)
        {
            List<Questionaire> objQuestionaire = new List<Questionaire>();
            string userId = questionaires.Select(x => x.UserId).LastOrDefault();
            try
            {
                var questions = _context.Questionaires.Where(x => x.UserId == userId).ToList();
                if (questions != null && questions.Count() != 0)
                {
                    _context.Questionaires.RemoveRange(questions);
                    _context.SaveChanges();
                }
                foreach (var ques in questionaires)
                {
                    if (ques.QuestionId == 22)
                    {
                        Questionaire questionaire = new Questionaire();
                        questionaire.QuestionId = ques.QuestionId;
                        questionaire.AnswerId = ques.AnswerId == 0 ? null : ques.AnswerId;
                        questionaire.DescriptiveAnswer = "http://admin.myavana.com/questionnaireFile/" + ques.DescriptiveAnswer;
                        questionaire.CreatedOn = DateTime.Now;
                        questionaire.IsActive = true;
                        questionaire.UserId = userId;

                        objQuestionaire.Add(questionaire);
                    }
                    else
                    {
                        Questionaire questionaire = new Questionaire();
                        questionaire.QuestionId = ques.QuestionId;
                        questionaire.AnswerId = ques.AnswerId == 0 ? null : ques.AnswerId;
                        questionaire.DescriptiveAnswer = ques.DescriptiveAnswer;
                        questionaire.CreatedOn = DateTime.Now;
                        questionaire.IsActive = true;
                        questionaire.UserId = userId;

                        objQuestionaire.Add(questionaire);
                    }

                }
                _context.AddRange(objQuestionaire);

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
            }

            return questionaires;
        }


        public async Task<List<QuestionAnswerModel>> GetQuestionnaire()
        {
            List<QuestionAnswerModel> questionnaireModels = new List<QuestionAnswerModel>();
            try
            {
                this._context.Database.SetCommandTimeout(280);
                List<string> userIds = _context.Questionaires.Where(z => z.IsActive == true).Select(x => x.UserId).Distinct().ToList();

                foreach (var user in userIds)
                {
                    if (user != null && user != "")
                    {
                        int i = 0;
                        int[] elements = new int[500];

                        QuestionAnswerModel questionAnswerModel = new QuestionAnswerModel();

                        string userId = user.ToString();
                        var userName = await _userManager.FindByIdAsync(userId);
                        questionAnswerModel.IsDraft = _context.HairProfiles.Where(x => x.UserId == userName.UserName).Select(x => x.IsDrafted).LastOrDefault();
                        questionAnswerModel.UserId = userId;
                        questionAnswerModel.UserName = userName.FirstName + " " + userName.LastName;
                        questionAnswerModel.UserEmail = userName.UserName;
                        questionAnswerModel.CustomerType = userName.CustomerType;
                        questionAnswerModel.CreatedOn = _context.Questionaires.FirstOrDefault(x => x.UserId == user).CreatedOn;

                        int latestQA = _context.Questionaires.Where(x => x.UserId == userId && x.IsActive == true).Max(x => x.QA);
                        var QuestionList = _context.Questionaires.Include(x => x.Questions).Where(x => x.UserId == user && x.QA == latestQA).Select(x => x.Questions).ToList();
                        List<QuestionModels> questionModels = new List<QuestionModels>();

                        foreach (var question in QuestionList)
                        {
                            if (!elements.Contains(question.QuestionId))
                            {
                                elements[i] = question.QuestionId;
                                i++;
                                QuestionModels questions = new QuestionModels();
                                questions.QuestionId = question.QuestionId;
                                questions.Question = question.Question;
                                questions.SerialNo = question.SerialNo;
                                questions.AnswerList = (from m in _context.Questionaires
                                                        join ans in _context.Answers
                                                        on m.AnswerId equals ans.AnswerId into answer
                                                        from qAns in answer.DefaultIfEmpty()
                                                        where m.QuestionId == question.QuestionId
                                                        && m.UserId == userId && m.QA == latestQA
                                                        select new AnswerModel()
                                                        {
                                                            AnswerId = qAns.AnswerId,
                                                            Answer = (m.AnswerId == null || m.AnswerId == 112) ? m.DescriptiveAnswer : qAns.Description
                                                        }).ToList();
                                questionModels.Add(questions);
                            }

                        }

                        questionAnswerModel.questionModel = questionModels;

                        questionnaireModels.Add(questionAnswerModel);
                    }
                }

            }
            catch (Exception ex)
            {

            }
            return questionnaireModels;
        }

        public async Task<List<QuestionAnswerModel>> GetQuestionnaireAbsenceUserList()
        {
            List<QuestionAnswerModel> questionnaireModels = new List<QuestionAnswerModel>();
            try
            {
                List<Guid> netUserIds = new List<Guid>();
                List<string> userIds = _context.Questionaires.Where(z => z.IsActive == true).Select(x => x.UserId).Distinct().ToList();
                try
                {
                    foreach (string userId in userIds)
                    {
                        if (userId != null && userId != "")
                        {
                            Guid id = new Guid(userId);
                            netUserIds.Add(id);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                var users = _context.Users.Where(x => !netUserIds.Contains(x.Id)).ToList();

                foreach (var user in users)
                {
                    QuestionAnswerModel questionAnswerModel = new QuestionAnswerModel();
                    questionAnswerModel.UserId = user.Id.ToString();
                    questionAnswerModel.UserName = user.FirstName + " " + user.LastName;
                    questionAnswerModel.UserEmail = user.UserName;
                    questionAnswerModel.CustomerType = user.CustomerType;
                    questionAnswerModel.CreatedOn = DateTime.Now;
                    questionnaireModels.Add(questionAnswerModel);
                }



                return questionnaireModels;
            }

            catch (Exception ex) { return null; }
        }
        public async Task<QuestionAnswerModel> GetQuestionnaireCustomer(string userId)
        {
            List<QuestionAnswerModel> questionnaireModels = new List<QuestionAnswerModel>();
            QuestionAnswerModel questionAnswerModel = new QuestionAnswerModel();

            try
            {
                int i = 0;
                int[] elements = new int[500];

                var userName = await _userManager.FindByIdAsync(userId);
                questionAnswerModel.UserId = userId;
                questionAnswerModel.UserName = userName.FirstName;
                questionAnswerModel.UserEmail = userName.UserName;

                int latestQA = _context.Questionaires.Where(x => x.UserId == userId && x.IsActive == true).Max(x => x.QA);
                var QuestionList = _context.Questionaires.Include(x => x.Questions).Where(x => x.UserId == userId && x.IsActive == true && x.QA == latestQA).Select(x => x.Questions).ToList();
                List<QuestionModels> questionModels = new List<QuestionModels>();

                foreach (var question in QuestionList)
                {
                    if (!elements.Contains(question.QuestionId))
                    {
                        elements[i] = question.QuestionId;
                        i++;
                        if (question.QuestionId == 29)
                        {
                            string abc = "ww";
                        }
                        QuestionModels questions = new QuestionModels();
                        questions.QuestionId = question.QuestionId;
                        questions.Question = question.Question;
                        questions.SerialNo = question.SerialNo;

                        questions.AnswerList = (from m in _context.Questionaires
                                                join ans in _context.Answers
                                                on m.AnswerId equals ans.AnswerId into answer
                                                from qAns in answer.DefaultIfEmpty()
                                                where m.QuestionId == question.QuestionId
                                                && m.UserId == userId && m.QA == latestQA
                                                select new AnswerModel()
                                                {
                                                    QA = (m.QA),
                                                    QAdate = (m.CreatedOn),
                                                    AnswerId = qAns.AnswerId,
                                                    Answer = (m.AnswerId == null || m.AnswerId == 112) ? m.DescriptiveAnswer : qAns.Description
                                                }).ToList();
                        questionModels.Add(questions);
                    }

                }
                questionAnswerModel.questionModel = questionModels;

                //questionnaireModels.Add(questionAnswerModel);
            }
            catch (Exception ex)
            {

            }
            return questionAnswerModel;
        }
        public async Task<QuestionAnswerModel> GetQuestionnaireCustomerDetails(QuestionAnswerModel quesModel)
        {
            List<QuestionAnswerModel> questionnaireModels = new List<QuestionAnswerModel>();
            QuestionAnswerModel questionAnswerModel = new QuestionAnswerModel();

            try
            {
                int i = 0;
                int[] elements = new int[50];

                var userName = await _userManager.FindByIdAsync(quesModel.UserId);
                questionAnswerModel.UserId = quesModel.UserId;
                questionAnswerModel.UserName = userName.FirstName;
                questionAnswerModel.UserEmail = userName.UserName;

                var QuestionList = _context.Questionaires.Include(x => x.Questions).Where(x => x.UserId == quesModel.UserId && x.IsActive == true && x.QA == quesModel.QA).Select(x => x.Questions).ToList();
                List<QuestionModels> questionModels = new List<QuestionModels>();

                foreach (var question in QuestionList)
                {
                    if (!elements.Contains(question.QuestionId))
                    {
                        elements[i] = question.QuestionId;
                        i++;
                        if (question.QuestionId == 29)
                        {
                            string abc = "ww";
                        }
                        QuestionModels questions = new QuestionModels();
                        questions.QuestionId = question.QuestionId;
                        questions.Question = question.Question;
                        questions.SerialNo = question.SerialNo;

                        questions.AnswerList = (from m in _context.Questionaires
                                                join ans in _context.Answers
                                                on m.AnswerId equals ans.AnswerId into answer
                                                from qAns in answer.DefaultIfEmpty()
                                                where m.QuestionId == question.QuestionId
                                                && m.UserId == quesModel.UserId
                                                select new AnswerModel()
                                                {
                                                    QA = (m.QA),
                                                    QAdate = (m.CreatedOn),
                                                    AnswerId = qAns.AnswerId,
                                                    Answer = (m.AnswerId == null || m.AnswerId == 112) ? m.DescriptiveAnswer : qAns.Description
                                                }).ToList();
                        questionModels.Add(questions);
                    }

                }
                questionAnswerModel.questionModel = questionModels;

                //questionnaireModels.Add(questionAnswerModel);
            }
            catch (Exception ex)
            {

            }
            return questionAnswerModel;
        }

        public List<QuestionnaireCustomerList> GetQuestionnaireCustomerList()
        {
            List<QuestionnaireCustomerList> questionnaireModels = new List<QuestionnaireCustomerList>();
            List<UserEntity> user = new List<UserEntity>();
            try
            {
                //List<string> userIds = _context.Questionaires.Select(x => x.UserId).Distinct().ToList();
                //user = _context.Users.Where(x => !userIds.Contains(x.Id.ToString())).ToList();       

                user = _context.Users.OrderByDescending(x => x.CreatedAt).ToList();
                foreach (var us in user)
                {
                    QuestionnaireCustomerList questionnaireCustomerList = new QuestionnaireCustomerList();

                    questionnaireCustomerList.UserId = us.Id.ToString();
                    questionnaireCustomerList.UserName = us.FirstName + " " + us.LastName;
                    questionnaireCustomerList.UserEmail = us.UserName;
                    questionnaireCustomerList.CreatedAt = us.CreatedAt;
                    questionnaireCustomerList.CustomerType = us.CustomerType;

                    var findUser = _context.Questionaires.FirstOrDefault(x => x.UserId == us.Id.ToString());
                    if (findUser != null)
                        questionnaireCustomerList.IsQuestionnaire = true;
                    else
                        questionnaireCustomerList.IsQuestionnaire = false;

                    questionnaireModels.Add(questionnaireCustomerList);
                }
            }
            catch (Exception ex)
            {

            }
            return questionnaireModels;
        }

        public List<CustomerMessageList> GetCustomerMessagesList(Guid userId)
        {
            List<CustomerMessageList> messagesModels = new List<CustomerMessageList>();
            try
            {
                messagesModels = (from m in _context.CustomerMessage
                                  where m.UserId == userId
                                  select new CustomerMessageList()
                                  {
                                      Subject = m.Subject,
                                      Message = m.Message,
                                      EmailAddress = m.EmailAddress,
                                      CreatedOn = m.CreatedOn,
                                      AttachmentFile = m.AttachmentFile
                                  }).ToList();
            }
            catch (Exception ex)
            {

            }
            return messagesModels;
        }

        public bool DeleteQuest(QuestModel quest)
        {
            Guid userId = _context.Users.Where(x => x.Email == quest.Email).Select(z => z.Id).FirstOrDefault();
            try
            {
                var objCode = _context.Questionaires.Where(x => x.UserId == userId.ToString()).ToList();
                foreach (var result in objCode)
                {
                    result.IsActive = false;
                }

                _context.SaveChanges();
                return true;
            }

            catch (Exception Ex)
            {
                return false;
            }
        }

        public List<Questionaire> GetFilledSurvey(ClaimsPrincipal claim)
        {
            Questionaire customerQuestionnaire = new Questionaire();
            var usr = _tokenService.GetAccountNo(claim);

            if (usr != null)
            {
                try
                {
                    List<Questionaire> questionaires = _context.Questionaires.Where(x => x.UserId == usr.Id.ToString()).ToList();
                    return questionaires;
                }
                catch (Exception Ex)
                {
                    // _logger.LogError("Error in GetFilledSurvey Questionaire Service" + Ex.Message, Ex);
                    return null;
                }
            }
            return null;
        }


        public List<QuestionGraph> GetQuestionsForGraph()
        {
            var questionaires = _context.Questionaires.Include(x => x.Questions).Where(y => y.IsActive == true).OrderBy(x => x.QuestionId).ToList();
            List<QuestionGraph> questionGraphs = new List<QuestionGraph>();
            List<int> questQuestionIds = questionaires.Select(x => x.QuestionId).Distinct().ToList();
            List<int> questionIds = new List<int>();
            foreach (var questionId in questQuestionIds)
            {
                QuestionGraph questionGraph = new QuestionGraph();
                questionIds.Add(questionId);
                questionGraph.QuestionId = questionId;
                questionGraph.Question = questionaires.Where(x => x.QuestionId == questionId).Select(q => q.Questions.Question).FirstOrDefault();
                var answers = _context.Answers.Where(q => q.QuestionId == questionId).Select(x => new { AnswerId = x.AnswerId, Answer = x.Description }).ToList();

                List<AnswerCount> answerCounts = new List<AnswerCount>();
                foreach (var answer in answers)
                {
                    if (answer.Answer != "Free response")
                    {
                        AnswerCount answerCount = new AnswerCount();
                        answerCount.AnswerId = answer.AnswerId;
                        answerCount.Answer = answer.Answer;
                        answerCount.Count = questionaires.Where(x => x.AnswerId == answer.AnswerId).Count();
                        answerCounts.Add(answerCount);
                    }
                }
                if (answerCounts.Count() > 0)
                {
                    questionGraph.AnswerCounts = answerCounts;
                    questionGraphs.Add(questionGraph);
                }
            }
            return questionGraphs;
        }

        public CustomerMessageModel SaveCustomerMessage(CustomerMessageModel customerMessageModel)
        {
            CustomerMessage customerMessage = new CustomerMessage();
            try
            {
                customerMessage.EmailAddress = customerMessageModel.EmailAddress;
                customerMessage.Message = customerMessageModel.Message;
                customerMessage.UserId = customerMessageModel.UserId;
                customerMessage.Subject = customerMessageModel.Subject;
                customerMessage.AttachmentFile = customerMessageModel.AttachmentFile;
                customerMessage.CreatedOn = DateTime.UtcNow;
                customerMessage.IsActive = true;
                _context.Add(customerMessage);
                _context.SaveChanges();
                customerMessageModel.emailBody = _context.GenericSettings.Where(s => s.SettingName == customerMessageModel.emailTemplate).Select(s => s.DefaultTextMax).FirstOrDefault();
            }
            catch (Exception ex)
            {
                customerMessageModel.ErrorMessage = ex.Message;
            }
            return customerMessageModel;
        }

        public EmailTemplate GetCustomerEmailTemplate()
        {
            try
            {
                EmailTemplate emailTemplateResult = _context.EmailTemplates.Where(p => p.TemplateCode == "CUSTMSG").FirstOrDefault();
                return emailTemplateResult;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }

    }
}
