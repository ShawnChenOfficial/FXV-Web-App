using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FXV.Data;
using FXV.JwtManager;
using FXV.Models;
using FXV.TestsFormula;
using FXV.ViewModels;
using FXV_App.CustomizeControllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FXV_App.Controllers
{
    public class ResultCollectionController : CustomizeController
    {
        public ResultCollectionController(IConfiguration configuration, IHttpContextAccessor accessor, UserManager<AppUser> userManager, ApplicationDbContext dbContext, IHostingEnvironment hostingEnvironment)
            : base(configuration, accessor, userManager, dbContext, hostingEnvironment)
        {
        }

        [Authorize("Admin")]
        [Authorize("Permission_All")]
        [HttpGet]
        public IActionResult ResultCollection(int id)
        {
            var e_id = id;


            //var list_tc_id = _dbContext.Combine_Categories_Weight.Where(x => x.C_ID == _dbContext.Event_Builder.Where(y => y.E_ID == id).First().C_ID).Select(z => z.TC_ID);
            //var categories = (from t1 in _dbContext.Test_Category
            //                  join t2 in list_tc_id on t1.TC_id equals t2
            //                  select t1).ToList();

            var categories = _dbContext.Combine_Categories_Weight.Where(w => w.C_ID == _dbContext.Event_Builder.Where(ww => ww.E_ID == id).FirstOrDefault().C_ID).Include(inc => inc.Test_Category).Select(s=>s.Test_Category).ToList(); 

            ViewData["Categories"] = categories;
            TempData["Event_ID"] = e_id;

            return View();
        }

        [Authorize("Admin")]
        [Authorize("Permission_All")]
        [HttpPost]
        public string GetSpeedTests(int e_id)
        {
            //var speed_Cate_ID = _dbContext.Test_Category.Where(x => x.Category == "Speed").First().TC_id;
            //var combine_builder = _dbContext.Combine_Builder.Where(x => x.C_ID == _dbContext.Event_Builder.Where(y => y.E_ID == e_id).First().C_ID).ToList();
            //var tests = (from t1 in combine_builder
            //             join t2 in _dbContext.Test.Where(x => x.TC_id == speed_Cate_ID) on t1.Test_ID equals t2.Test_ID
            //             select t2).ToList();

            var tests = _dbContext.Combine_Builder.Where(w => w.C_ID == _dbContext.Event_Builder.Where(ww => ww.E_ID == e_id).First().C_ID)
                    .Include(inc => inc.Test).Select(s => s.Test)
                    .Include(iinc => iinc.Test_Category).Where(www => www.Test_Category.Category == "Speed").ToList();

            return JsonConvert.SerializeObject(tests);
        }

        [Authorize("Admin")]
        [Authorize("Permission_All")]
        [HttpPost]
        public string ResultCollectionSave(EventResultCollection EventResultCollection,int e_id)
        {

            if (!ModelState.IsValid && (EventResultCollection.AttendeeFullName == null || EventResultCollection.AttendeeFullName == "" || EventResultCollection.AttendeeId == 0))
            {
                return "Must have an athlete, or the athlete must be selected from the search list.";
            }

            else
            {
                if (EventResultCollection.Test_Results != null && EventResultCollection.Test_Result == null)
                {
                    bool missing_result = true;

                    foreach (var x in EventResultCollection.Test_Results)
                    {
                        if (x.Result != 0)
                        {
                            missing_result = false;
                        }
                    }

                    if (missing_result)
                    {
                        return "There is no result entered.";
                    }
                    else
                    {
                        try
                        {
                            using (var transaction = _dbContext.Database.BeginTransaction())
                            {
                                foreach (var x in EventResultCollection.Test_Results)
                                {
                                    if (x.Result != 0)
                                    {
                                        var test = _dbContext.Test.Find(x.Test_ID);

                                        var point = (x.Result == 0) ? 0 :
                                            (test.Reverse ? new TestFormula(test.HigherResult, test.LowerResult, test.LowerScore, test.HigherScore, x.Result).GetFinalScore()
                                                            : new TestFormula(test.LowerResult, test.HigherResult, test.LowerScore, test.HigherScore, x.Result).GetFinalScore());

                                        Test_Result test_Result = new Test_Result
                                        {
                                            E_ID = e_id,
                                            Date = DateTime.Now,
                                            Id = EventResultCollection.AttendeeId,
                                            Result = x.Result,
                                            Test_ID = x.Test_ID,
                                            Point = (point < 0) ? 0 : point
                                        };

                                        _dbContext.Test_Result.Update(test_Result);
                                        _dbContext.SaveChanges();
                                    }
                                }

                                UpdatingCombine_Event_Result(EventResultCollection.E_ID, EventResultCollection.AttendeeId);
                                transaction.Commit();
                            }
                        }
                        catch (Exception e)
                        {
                            return e.Message;
                        }

                        return "Done!";
                    }
                }
                else
                {
                    if (EventResultCollection.Test_Result.Test_ID == 0)
                    {
                        return "Must have a test, or the test must be selected from the search list.";
                    }
                    else if (EventResultCollection.Test_Result.Result == 0)
                    {
                        return "There is no result entered.";
                    }
                    else
                    {
                        try
                        {
                            using (var transaction = _dbContext.Database.BeginTransaction())
                            {
                                var test = _dbContext.Test.Find(EventResultCollection.Test_Result.Test_ID);

                                var point = (EventResultCollection.Test_Result.Result == 0) ? 0 : (test.Reverse ? new TestFormula(test.HigherResult, test.LowerResult, test.LowerScore, test.HigherScore, EventResultCollection.Test_Result.Result).GetFinalScore() : new TestFormula(test.LowerResult, test.HigherResult, test.LowerScore, test.HigherScore, EventResultCollection.Test_Result.Result).GetFinalScore());

                                Test_Result test_Result = new Test_Result
                                {
                                    E_ID = EventResultCollection.E_ID,
                                    Date = DateTime.Now,
                                    Id = EventResultCollection.AttendeeId,
                                    Result = EventResultCollection.Test_Result.Result,
                                    Test_ID = EventResultCollection.Test_Result.Test_ID,
                                    Point = (point < 0) ? 0 : point
                                };

                                _dbContext.Test_Result.Update(test_Result);
                                _dbContext.SaveChanges();

                                UpdatingCombine_Event_Result(EventResultCollection.E_ID, EventResultCollection.AttendeeId);

                                transaction.Commit();
                            }
                        }
                        catch (Exception e)
                        {
                            return e.Message;
                        }
                        return "Done!";
                    }
                }
            }
        }

        //updating combine result and event result
        private void UpdatingCombine_Event_Result(int e_id, int Id)
        {
            var temtable = _dbContext.TempEventTable.Where(x => x.Event_ID == e_id);

            if (temtable.ToList().Count == 0)
            {
                TempEventTable tempEventTable = new TempEventTable
                {
                    Event_ID = e_id,
                    Athlete_ID = Id,
                    IsFinished = true
                };

                _dbContext.TempEventTable.Update(tempEventTable);
                _dbContext.SaveChanges();
            }
            else
            {
                temtable.First().Athlete_ID = Id;
                temtable.First().IsFinished = true;

                _dbContext.TempEventTable.Update(temtable.First());
                _dbContext.SaveChanges();
            }

            var c_id = _dbContext.Event_Builder.Where(x => x.E_ID == e_id).First().C_ID;

            var existing_combine_result = _dbContext.Combine_Result.Where(x => x.C_ID == c_id && x.E_ID == e_id && x.Id == Id);

            var combine_cate = _dbContext.Combine_Categories_Weight.Where(x => x.C_ID == c_id).ToList();


            //var test_list = _dbContext.Combine_Builder.Where(x => x.C_ID == c_id).ToList();

            //var test_result_list = (from t1 in _dbContext.Test_Result
            //                        join t2 in test_list on t1.Test_ID equals t2.Test_ID
            //                        select t1).Where(x => x.E_ID == e_id && x.Id == Id).ToList();

            var test_result_list = _dbContext.Test_Result.Where(w => _dbContext.Combine_Builder.Where(ww => ww.C_ID == c_id).Select(s => s.Test_ID).Contains(w.Test_ID) && w.E_ID == e_id && w.Id == Id).ToList();

            float point = 0.00f;

            foreach (var item in test_result_list)
            {
                var TC_id = _dbContext.Test.Find(item.Test_ID).TC_id;

                //var countx = (from t1 in _dbContext.Combine_Builder.Where(x => x.C_ID == c_id && x.IsWeighted)
                //              join t2 in _dbContext.Test on t1.Test_ID equals t2.Test_ID
                //              select t2).Where(y => y.TC_id == TC_id).ToList().Count;
                //

                var countx = _dbContext.Combine_Builder.Include(inc => inc.Test).Where(w => w.C_ID == c_id && w.IsWeighted && w.Test.TC_id == TC_id).Select(s => s.Test).ToList().Count;

                float percentage_of_cate = (100f / (float)countx) / 100f;
                float percentage_of_combine = (float)combine_cate.Where(x => x.TC_ID == TC_id).First().Weight;

                point += ((float)item.Point * percentage_of_cate * percentage_of_combine);
            }

            if (existing_combine_result.ToList().Count > 0)
            {
                var combine_result = existing_combine_result.First();

                combine_result.Point = (int)Math.Round(point);
                _dbContext.Combine_Result.Update(combine_result);
                _dbContext.SaveChanges();
            }
            else
            {
                Combine_Result new_combine_Result = new Combine_Result
                {
                    C_ID = c_id,
                    E_ID = e_id,
                    Id = Id,
                    Point = (int)Math.Round(point)
                };

                _dbContext.Combine_Result.Update(new_combine_Result);
                _dbContext.SaveChanges();
            }

            var event_Result = _dbContext.Event_Result.Where(x => x.E_ID == e_id && x.Id == Id);

            if (event_Result.ToList().Count > 0)
            {
                event_Result.First().Final_Point = (int)Math.Round(point);
                _dbContext.Event_Result.Update(event_Result.First());
                _dbContext.SaveChanges();
            }
            else
            {
                Event_Result new_event_Result = new Event_Result
                {
                    E_ID = e_id,
                    Id = Id,
                    Final_Point = (int)Math.Round(point)
                };
                _dbContext.Event_Result.Update(new_event_Result);
                _dbContext.SaveChanges();
            }
        }
    }
}
