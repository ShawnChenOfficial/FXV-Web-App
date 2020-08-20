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
using FXV.ViewModels.NewModels;
using FXV_App.CustomizeControllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Serilog;
using Serilog.Formatting.Compact;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FXV_App.Controllers
{
    public class TestsController : CustomizeController
    {
        public TestsController(IConfiguration configuration, IHttpContextAccessor accessor, UserManager<AppUser> userManager, ApplicationDbContext dbContext, IHostingEnvironment hostingEnvironment)
            : base(configuration, accessor, userManager, dbContext, hostingEnvironment)
        {
        }
        //
        //Get Tests methods in FXV_API
        //
        [Authorize("All")]
        [Authorize("Permission_All")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize("Admin")]
        [Authorize("Permission_All")]
        [HttpPost]
        public async Task<IActionResult> RemoveTest(int test_id)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var list_combine = _dbContext.Combine_Builder.Where(x => x.Test_ID == test_id).ToList();
                    var list_test_result = _dbContext.Test_Result.Where(x => x.Test_ID == test_id).ToList();
                    if (list_combine.Count > 0)
                    {
                        return Content(JsonConvert.SerializeObject(new { Success = false, Reason = "Failed! This Test has related to one or more Combines, please remove or edit relevant combines firstly." }), "application/json");
                    }
                    else if (list_test_result.Count > 0)
                    {
                        return Content(JsonConvert.SerializeObject(new { Success = false, Reason = "Failed! This Test has related to one or more Test Result, please remove all relevant result firstly." }), "application/json");
                    }
                    else
                    {
                        _dbContext.Test.Remove(await _dbContext.Test.FindAsync(test_id));
                        _dbContext.SaveChanges();
                        transaction.Commit();

                        return Content(JsonConvert.SerializeObject(new { Success = true, Reason = "Success" }), "application/json");
                    }
                }
                catch (Exception ex)
                {
                    var x = ex.Message;
                    return Content(JsonConvert.SerializeObject(new { Success = false, Reason = "Failed to remove test, please try again. If keeps happen, contact our administrator." }), "application/json");
                }
            }
        }

        [Authorize("Admin")]
        [Authorize("Permission_All")]
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["CategoryList"] = new SelectList(_dbContext.Test_Category, "TC_id", "Category");
            return View();
        }

        [Authorize("Admin")]
        [Authorize("Permission_All")]
        [HttpPost]
        public async Task<IActionResult> Create(ViewModel_Test testBuilder)
        {
            if (!ModelState.IsValid)
            {
                return View(testBuilder);
            }

            else if (testBuilder.LowerScore + testBuilder.HigherScore == 0 && testBuilder.LowerResult + testBuilder.HigherResult != 0)
            {
                ModelState.AddModelError("", "Unable to save changes with only expected Scores are 0. Please make sure both expected Scores and Results are have value or keep them all 0.");

                return View(testBuilder);
            }

            else if (testBuilder.HigherResult + testBuilder.HigherScore != 0 && testBuilder.LowerResult + testBuilder.LowerScore == 0)
            {
                ModelState.AddModelError("", "Unable to save changes with only expected Results are 0. Please make sure both expected Scores and Results are have value or keep them all 0.");

                return View(testBuilder);
            }

            else
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        // requires Database update
                        var test = new Test
                        {
                            Name = testBuilder.Name,
                            Gender = testBuilder.Gender,
                            Description = testBuilder.Description,
                            Unit = testBuilder.Unit,
                            LowerResult = testBuilder.LowerResult,
                            HigherResult = testBuilder.HigherResult,
                            LowerScore = testBuilder.LowerScore,
                            HigherScore = testBuilder.HigherScore,
                            Reverse = testBuilder.Reverse,
                            Public = testBuilder.Public,
                            IsVerified = testBuilder.IsVerified,
                            IsSplittable = testBuilder.IsSplittable,
                            UsedAsSplit = testBuilder.UsedAsSplit,
                            TC_id = testBuilder.CategoryId,
                        };

                        _dbContext.Test.Update(test);
                        _dbContext.SaveChanges();

                        transaction.Commit();

                        return RedirectToAction("Index");
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", "Unable to save changes. " +
                                    "Try again, and if the problem persists, " +
                                    "see your system administrator.");

                        return View(testBuilder);
                    }
                }
            }
        }
        [Authorize("Admin")]
        [Authorize("Permission_All")]
        [HttpGet]
        public async Task<IActionResult> Edit(int test_id)
        {

            var test = await _dbContext.Test.Where(w => w.Test_ID == test_id).Include(inc => inc.Test_Category).FirstOrDefaultAsync();

            if (test_id == 0 || test == null)
            {
                return Content(JsonConvert.SerializeObject(new { Result = false, Reason = "Test has been deleted." }));
            }

            // requires Database update
            ViewModel_Test testBuilder = new ViewModel_Test
            {
                TestId = test.Test_ID,
                Name = test.Name,
                Description = test.Description,
                Gender = test.Gender,
                HigherResult = test.HigherResult,
                HigherScore = test.HigherScore,
                LowerResult = test.LowerResult,
                LowerScore = test.LowerScore,
                CategoryId = test.TC_id,
                Unit = test.Unit,
                Reverse = test.Reverse,
                Public = test.Public,
                IsSplittable = test.IsSplittable,
                UsedAsSplit = test.UsedAsSplit,
                RowVersion = test.RowVersion
            };

            ViewData["CategoryList"] = new SelectList(_dbContext.Test_Category.ToList(), "TC_id", "Category", testBuilder.CategoryId);

            return View(testBuilder);
        }

        [Authorize("Admin")]
        [Authorize("Permission_All")]
        [HttpPost]
        public async Task<IActionResult> Edit(ViewModel_Test testBuilder)
        {
            if (!ModelState.IsValid)
            {
                return View(testBuilder);
            }
            else
            {
                var originalTest = await _dbContext.Test.Where(w => w.Test_ID == testBuilder.TestId).FirstOrDefaultAsync();

                if (originalTest == null)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. The test was deleted by another user.");
                    return View(testBuilder);
                }

                try
                {
                    var ToUpdateRelevantResults = originalTest.LowerScore != testBuilder.LowerScore
                                       || originalTest.LowerResult != testBuilder.LowerResult
                                       || originalTest.HigherResult != testBuilder.HigherResult
                                       || originalTest.HigherScore != testBuilder.HigherScore
                                       || originalTest.Reverse != testBuilder.Reverse
                                       || originalTest.TC_id != testBuilder.CategoryId;

                    
                    var entry = _dbContext.Entry(originalTest);

                    entry.Property("RowVersion").OriginalValue = testBuilder.RowVersion;

                    //require db update
                    if (await TryUpdateModelAsync<Test>(originalTest, "", e => e.Name, e => e.Gender, e => e.Description, e => e.Unit, e => e.LowerResult, e => e.LowerScore, e => e.HigherResult, e => e.HigherScore, e => e.Reverse, e => e.Public, e => e.TC_id, e => e.IsVerified ,e => e.IsSplittable, e => e.UsedAsSplit))
                    {
                        try
                        {
                            //require db update
                            originalTest.Name = testBuilder.Name;
                            originalTest.Gender = testBuilder.Gender;
                            originalTest.Description = testBuilder.Description;
                            originalTest.Unit = testBuilder.Unit;
                            originalTest.LowerResult = testBuilder.LowerResult;
                            originalTest.HigherResult = testBuilder.HigherResult;
                            originalTest.LowerScore = testBuilder.LowerScore;
                            originalTest.HigherScore = testBuilder.HigherScore;
                            originalTest.Reverse = testBuilder.Reverse;
                            originalTest.Public = testBuilder.Public;
                            originalTest.TC_id = testBuilder.CategoryId;
                            originalTest.IsVerified = testBuilder.IsVerified;
                            originalTest.IsSplittable = testBuilder.IsSplittable;
                            originalTest.UsedAsSplit = testBuilder.UsedAsSplit;

                            _dbContext.Test.Update(originalTest);
                            await _dbContext.SaveChangesAsync();

                            // only run if relevant result records need to be updated or re-calculated
                            if (ToUpdateRelevantResults)
                            {
                                try
                                {
                                    using (var transaction_update_relevant_result_test = _dbContext.Database.BeginTransaction())
                                    {
                                        var relevant_test_results = _dbContext.Test_Result.Where(w => w.Test_ID == originalTest.Test_ID).ToList();

                                        foreach (var item in relevant_test_results)
                                        {
                                            item.Point = (originalTest.LowerResult + originalTest.HigherResult + originalTest.LowerScore + originalTest.HigherScore == 0) ? 0
                                                                    : (originalTest.Reverse ? new TestFormula(originalTest.HigherResult, originalTest.LowerResult, originalTest.LowerScore, originalTest.HigherScore, item.Result).GetFinalScore()
                                                                    : new TestFormula(originalTest.LowerResult, originalTest.HigherResult, originalTest.LowerScore, originalTest.HigherScore, item.Result).GetFinalScore());


                                            // a question for this that do we need to make it to be forced to update, as it is supposed to be.
                                            // current solution is to force update point of each test result
                                            _dbContext.Test_Result.Update(item);
                                            await _dbContext.SaveChangesAsync();
                                        }

                                        transaction_update_relevant_result_test.Commit();
                                    }
                                }
                                catch (Exception e)
                                {
                                    //Log the error (uncomment ex variable name and write a log.)
                                    ModelState.AddModelError("", "Test successfully updated, but relevant result update failed. Please contact administrator.");

                                    var logFileName = DateTime.Now;

                                    var LoggerFileName = ("SystemLog/UpdateTestException/" + logFileName.ToShortDateString().Replace('/', '-') + " - " + logFileName.ToShortTimeString().Replace('/', '-') + ".txt");

                                    Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(_configuration).WriteTo.File(new CompactJsonFormatter(), LoggerFileName).CreateLogger();

                                    Log.Information("UserID: " + _claim.Uid + ".IP: " + _claim.IP + ",Action: Update All Relevant Result After Edit Test, Filded in Test result, Combine result and Event result," + "{@fileReaderWithResultError}:", "Exception Message: " + e.Message + "; Exception Source: " + e.Source + "; Exception Stack Trace: " + e.StackTrace);

                                    ViewData["CategoryList"] = new SelectList(_dbContext.Test_Category.ToList(), "TC_id", "Category", testBuilder.CategoryId);

                                    return View(testBuilder);
                                }
                                try
                                {
                                    using (var transaction_update_relevant_result_combine = _dbContext.Database.BeginTransaction())
                                    {
                                        var relevant_combines = _dbContext.Combine_Builder.Where(w => w.Test_ID == originalTest.Test_ID).Include(inc => inc.Combine).Select(s => s.Combine.C_ID).ToList();

                                        var relevant_combine_results = _dbContext.Combine_Result.Where(w => relevant_combines.Contains(w.C_ID)).ToList();

                                        foreach (var item in relevant_combine_results)
                                        {
                                            var combine_cate = _dbContext.Combine_Categories_Weight.Where(w => w.C_ID == item.C_ID).ToList();

                                            var relevant_test_id = _dbContext.Combine_Builder.Where(w => w.C_ID == item.C_ID).Select(s => s.Test_ID).ToList();

                                            var test_Result = _dbContext.Test_Result.Include(inc => inc.Test).Include(iinc => iinc.AppUser).Where(w => relevant_test_id.Contains(w.Test_ID) && w.Id == item.Id && w.E_ID == item.E_ID)
                                                               .GroupBy(y => y.Test_ID).Select(b => b.Max(z =>
                                                                        new Test_Result
                                                                        {
                                                                            Test_ID = z.Test_ID,
                                                                            Id = z.Id,
                                                                            Point = z.Point,
                                                                            Result = z.Result
                                                                        })).ToList();

                                            float point = 0.00f;

                                            foreach (var test_result_item in test_Result)
                                            {
                                                var TC_id = _dbContext.Test.Find(test_result_item.Test_ID).TC_id;

                                                float test_percentage_of_cate = (100f / (float)_dbContext.Combine_Builder.Include(inc => inc.Test).Where(w => w.C_ID == item.C_ID && w.IsWeighted && w.Test.TC_id == TC_id).Select(s => s.Test).ToList().Count) / 100f;
                                                float cate_percentage_of_combine = (float)combine_cate.Where(x => x.TC_ID == TC_id).First().Weight;

                                                point += ((float)test_result_item.Point * test_percentage_of_cate * cate_percentage_of_combine);
                                            }

                                            // a question for this that do we need to make it to be forced to update, as it is supposed to be.
                                            // current solution is to force update point of each test result

                                            item.Point = (int)Math.Round(point);

                                            _dbContext.Combine_Result.Update(item);
                                            await _dbContext.SaveChangesAsync();

                                        }

                                        transaction_update_relevant_result_combine.Commit();
                                    }
                                }
                                catch (Exception e)
                                {
                                    //Log the error (uncomment ex variable name and write a log.)
                                    ModelState.AddModelError("", "Test and relevant Test records successfully updated, but relevant combine and event result update failed. Please contact administrator.");

                                    var logFileName = DateTime.Now;

                                    var LoggerFileName = ("SystemLog/UpdateTestException/" + logFileName.ToShortDateString().Replace('/', '-') + " - " + logFileName.ToShortTimeString().Replace('/', '-') + ".txt");

                                    Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(_configuration).WriteTo.File(new CompactJsonFormatter(), LoggerFileName).CreateLogger();

                                    Log.Information("UserID: " + _claim.Uid + ".IP: " + _claim.IP + ",Action: Update All Relevant Result After Edit Test, Filded in Combine result and Event result," + "{@fileReaderWithResultError}:", "Exception Message: " + e.Message + "; Exception Source: " + e.Source + "; Exception Stack Trace: " + e.StackTrace);

                                    ViewData["CategoryList"] = new SelectList(_dbContext.Test_Category.ToList(), "TC_id", "Category", testBuilder.CategoryId);

                                    return View(testBuilder);
                                }
                                try
                                {
                                    using (var transaction_update_relevant_result_event = _dbContext.Database.BeginTransaction())
                                    {
                                        var relevant_combines = _dbContext.Combine_Builder.Where(w => w.Test_ID == originalTest.Test_ID).Include(inc => inc.Combine).Select(s => s.Combine.C_ID).ToList();
                                        var relevant_event = _dbContext.Event_Builder.Where(w => relevant_combines.Contains(w.C_ID)).Select(s => s.Event.E_ID).ToList();
                                        var relevant_event_results = _dbContext.Event_Result.Where(w => relevant_event.Contains(w.E_ID)).ToList();

                                        foreach (var item in relevant_event_results)
                                        {
                                            var combine_id = await _dbContext.Event_Builder.Where(w => w.E_ID == item.E_ID).Select(s => s.C_ID).FirstOrDefaultAsync();

                                            item.Final_Point = await _dbContext.Combine_Result.Where(w => w.C_ID == combine_id && w.Id == item.Id && w.E_ID == item.E_ID).Select(s => s.Point).FirstOrDefaultAsync();

                                            // a question for this that do we need to make it to be forced to update, as it is supposed to be.
                                            // current solution is to force update point of each test result

                                            _dbContext.Event_Result.Update(item);
                                            await _dbContext.SaveChangesAsync();
                                        }

                                        transaction_update_relevant_result_event.Commit();
                                    }
                                }
                                catch (Exception e)
                                {
                                    //Log the error (uncomment ex variable name and write a log.)
                                    ModelState.AddModelError("", "Test, relevant Test and relevant combine result records successfully updated, but relevant event result update failed. Please contact administrator.");

                                    var logFileName = DateTime.Now;

                                    var LoggerFileName = ("SystemLog/UpdateTestException/" + logFileName.ToShortDateString().Replace('/', '-') + " - " + logFileName.ToShortTimeString().Replace('/', '-') + ".txt");

                                    Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(_configuration).WriteTo.File(new CompactJsonFormatter(), LoggerFileName).CreateLogger();

                                    Log.Information("UserID: " + _claim.Uid + ".IP: " + _claim.IP + ",Action: Update All Relevant Result After Edit Test, Filded in Event result," + "{@fileReaderWithResultError}:", "Exception Message: " + e.Message + "; Exception Source: " + e.Source + "; Exception Stack Trace: " + e.StackTrace);

                                    ViewData["CategoryList"] = new SelectList(_dbContext.Test_Category.ToList(), "TC_id", "Category", testBuilder.CategoryId);

                                    return View(testBuilder);
                                }
                            }

                            return View("Index");
                        }
                        catch (DbUpdateConcurrencyException ex)
                        {
                            var exceptionEntry = ex.Entries.Single();
                            var databaseEntry = exceptionEntry.GetDatabaseValues();
                            if (databaseEntry == null)
                            {
                                ModelState.AddModelError(string.Empty,
                                    "Unable to save changes. The department was deleted by another user.");
                            }
                            else
                            {
                                var databaseValues = (Test)databaseEntry.ToObject();

                                ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                                        + "was modified by another user after you got the original value. The "
                                        + "edit operation was canceled. If you still want to edit this record, click "
                                        + "the Save button again.");

                                testBuilder.RowVersion = (byte[])databaseValues.RowVersion;
                                ModelState.Remove("RowVersion");
                            }

                            ViewData["CategoryList"] = new SelectList(_dbContext.Test_Category.ToList(), "TC_id", "Category", testBuilder.CategoryId);

                            return View(testBuilder);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unable to save changes. " +
                                    "Try again, and if the problem persists, " +
                                    "see your system administrator.");

                        ViewData["CategoryList"] = new SelectList(_dbContext.Test_Category.ToList(), "TC_id", "Category", testBuilder.CategoryId);

                        return View(testBuilder);
                    }
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                                "Try again, and if the problem persists, " +
                                "see your system administrator.");

                    ViewData["CategoryList"] = new SelectList(_dbContext.Test_Category.ToList(), "TC_id", "Category", testBuilder.CategoryId);

                    return View(testBuilder);
                }
            }
        }
    }
}
