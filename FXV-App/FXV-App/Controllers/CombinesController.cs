using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FXV.Data;
using FXV.JwtManager;
using FXV.Models;
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
    public class CombinesController : CustomizeController
    {
        private List<string> _UnWeightedTests = new List<string>() { "5M Sprint Male", "10M Sprint Male", "20M Sprint Male", "40M Sprint Male",
                "5M Sprint Female", "10M Sprint Female", "20M Sprint Female", "40M Sprint Female","Max Touch Male","Max Touch Female" };

        public CombinesController(IConfiguration configuration, IHttpContextAccessor accessor, UserManager<AppUser> userManager, ApplicationDbContext dbContext, IHostingEnvironment hostingEnvironment)
            : base(configuration, accessor, userManager, dbContext, hostingEnvironment)
        {
        }

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
        public async Task<IActionResult> RemoveCombine(int c_id)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var list_combine_result = _dbContext.Combine_Result.Where(x => x.C_ID == c_id).ToList();

                    var list_event = _dbContext.Event_Builder.Where(x => x.C_ID == c_id).ToList();
                    if (list_combine_result.Count > 0)
                    {
                        return Content(JsonConvert.SerializeObject(new { Success = false, Reason = "Failed! This Combine has related to one or more Combines Result, please remove all relevant combines Result firstly." }), "application/json");
                    }
                    else if (list_event.Count > 0)
                    {
                        return Content(JsonConvert.SerializeObject(new { Success = false, Reason = "Failed! This Combine has related to one or more Event, please remove all relevent event firstly." }), "application/json");
                    }
                    else
                    {
                        _dbContext.Combine.Remove(await _dbContext.Combine.FindAsync(c_id));
                        _dbContext.SaveChanges();
                        transaction.Commit();

                        return Content(JsonConvert.SerializeObject(new { Success = true, Reason = "Success." }), "application/json");
                    }
                }
                catch (Exception ex)
                {
                    return Content(JsonConvert.SerializeObject(new { Success = false, Reason = "Failed to remove combine, please try again. If keeps happen, contact our administrator." }), "application/json");
                }

            }
        }

        [Authorize("Admin")]
        [Authorize("Permission_All")]
        [HttpGet]
        public async Task<IActionResult> Edit(int c_id)
        {
            if (_dbContext.Combine.Find(c_id) == null)
            {
                return Content(JsonConvert.SerializeObject(new { Success = false, Reason = "Combine has been deleted." }));
            }

            List<CombineCateWeightEdit> list = new List<CombineCateWeightEdit>();

            var combine_weight_cate = _dbContext.Combine_Categories_Weight.Where(w => w.C_ID == c_id).Include(inc => inc.Test_Category).ToList();

            foreach (var x in combine_weight_cate)
            {
                var tests = _dbContext.Combine_Builder.Include(inc => inc.Test).Where(w => w.Test.TC_id == x.TC_ID && w.C_ID == c_id).Select(s => s.Test).ToList();

                list.Add(new CombineCateWeightEdit { Category = x.Test_Category.Category, Weight = x.Weight * 100, Tests = tests });
            }

            ViewBag.Cate = list;

            var editable = await _dbContext.Combine_Result.Where(w => w.C_ID == c_id).CountAsync() == 0 && await _dbContext.Event_Result.Where(w => _dbContext.Event_Builder.Where(ww => ww.C_ID == c_id).Select(s => s.C_ID).Contains(w.E_ID)).CountAsync() == 0;

            var combine = _dbContext.Combine.Where(w => w.C_ID == c_id).FirstOrDefault();

            var combinebuilder = new CombineBuilder { C_ID = combine.C_ID, Description = combine.Description, Gender = combine.Gender, Name = combine.Name, Img_Path = '.' + combine.Img_Path, Editable = editable, RowVersion = combine.RowVersion };

            if (!editable)
            {
                ModelState.AddModelError("", "This combine only allowed to edit Combine name, Combine description and Combine Image, due to some Combine result / Event result related.");
            }

            return View(combinebuilder);
        }


        [Authorize("Admin")]
        [Authorize("Permission_All")]
        [HttpPost]
        public async Task<IActionResult> Edit(CombineBuilder combineBuilder, string cate_weight_pair)
        {
            var ReturnResult = false;

            var c_id = combineBuilder.C_ID;

            var originalCombine = await _dbContext.Combine.FindAsync(combineBuilder.C_ID);

            //check if this combine is editable, in case of some user change them manually on html.
            combineBuilder.Editable = await _dbContext.Combine_Result.Where(w => w.C_ID == c_id).CountAsync() == 0 && await _dbContext.Event_Result.Where(w => _dbContext.Event_Builder.Where(ww => ww.C_ID == c_id).Select(s => s.C_ID).Contains(w.E_ID)).CountAsync() == 0;

            if (!ModelState.IsValid)
            {
                ReturnResult = false;
            }
            else
            {
                if (originalCombine == null)
                {
                    ModelState.AddModelError("", "Combine has been deleted.");

                    ReturnResult = false;
                }
                else if ((originalCombine != null))
                {
                    try
                    {
                        var New_Img_Path = originalCombine.Img_Path;

                        if (combineBuilder.Image != null)
                        {
                            var date = Request;
                            var files = Request.Form.Files;
                            long size = files.Sum(f => f.Length);
                            string contentRootPath = _hostingEnvironment.ContentRootPath;
                            IFormFile img = combineBuilder.Image;


                            if (img.Length > 0)
                            {
                                string fileExt = img.FileName;

                                while (fileExt.Contains('.'))
                                {
                                    fileExt = fileExt.Substring(fileExt.IndexOf('.') + 1);
                                }

                                long fileSize = img.Length;
                                var newFileName = System.Guid.NewGuid().ToString() + "." + fileExt;
                                string webRootPath = _hostingEnvironment.WebRootPath;
                                var filePath = webRootPath + "/sources/CombineImg/" + newFileName;
                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    await img.CopyToAsync(stream);
                                }
                                New_Img_Path = "/sources/CombineImg/" + newFileName;
                            }

                        }

                        var entry = _dbContext.Entry(originalCombine);

                        entry.Property("RowVersion").OriginalValue = combineBuilder.RowVersion;

                        if (await TryUpdateModelAsync<Combine>(originalCombine, "", e => e.Name, e => e.Description, e => e.Img_Path, e => e.Gender))
                        {
                            try
                            {
                                using (var transaction = _dbContext.Database.BeginTransaction())
                                {
                                    originalCombine.Img_Path = New_Img_Path;
                                    originalCombine.Name = combineBuilder.Name;
                                    originalCombine.Description = combineBuilder.Description;
                                    originalCombine.Img_Path = New_Img_Path;

                                    if (combineBuilder.Editable)
                                    {
                                        originalCombine.Gender = combineBuilder.Gender;
                                    }

                                    _dbContext.Combine.Update(originalCombine);
                                    await _dbContext.SaveChangesAsync();

                                    if (combineBuilder.Editable)
                                    {
                                        // for new tests
                                        //
                                        var currentTests = await _dbContext.Combine_Builder.Where(w => w.C_ID == c_id).ToListAsync();

                                        // find the new tests which are not stored in db for this combine, before the parameter currentTests been overrided or updated by next query operation.
                                        var newTestsToInsert = combineBuilder.Tests.Where(w => !currentTests.Select(s => s.Test_ID).Contains(w.Test_ID) && w.Test_ID != 0).Select(s => new Combine_Builder { C_ID = c_id, Attempt = 0, HasSplit = false, IsWeighted = !_UnWeightedTests.Contains(s.Name), Test_ID = s.Test_ID }).ToList();

                                        if (newTestsToInsert.Count > 0)
                                        {
                                            await _dbContext.Combine_Builder.AddRangeAsync(newTestsToInsert);
                                            await _dbContext.SaveChangesAsync();
                                        }

                                        // remove the tests from current tests query result that the new tests list containes. the tests remaimed in the list, are the which the new list does not contain.
                                        currentTests.RemoveAll(x => combineBuilder.Tests.Select(s => s.Test_ID).Contains(x.Test_ID));
                                        var currentTestsToRemove = currentTests;

                                        if (currentTestsToRemove.Count > 0)
                                        {
                                            _dbContext.Combine_Builder.RemoveRange(currentTestsToRemove);
                                            await _dbContext.SaveChangesAsync();
                                        }

                                        //for categories
                                        cate_weight_pair = '[' + cate_weight_pair + ']';

                                        Category_Weight[] arr = JsonConvert.DeserializeObject<Category_Weight[]>(cate_weight_pair);

                                        var current_Cate_Weight = await _dbContext.Combine_Categories_Weight.Where(w => w.C_ID == c_id).Include(inc => inc.Test_Category).ToListAsync();

                                        // find the new category weight key value pairs which are not stored in db for this combine, before the parameter currentTests been overrided or updated by next query operation.
                                        var newCateWeightPairToInset = arr.Where(w => !current_Cate_Weight.Select(s => s.Test_Category.Category).Contains(w.Category))
                                                                        .Select(s => new Combine_Categories_Weight
                                                                        {
                                                                            TC_ID = (_dbContext.Test_Category.Where(y => y.Category == s.Category).FirstOrDefault()).TC_id,
                                                                            C_ID = c_id,
                                                                            Weight = double.Parse(s.Weight.ToString()) / 100
                                                                        }).ToList();

                                        if (newCateWeightPairToInset.Count > 0)
                                        {
                                            await _dbContext.Combine_Categories_Weight.AddRangeAsync(newCateWeightPairToInset);
                                            await _dbContext.SaveChangesAsync();
                                        }

                                        // remove the category weight key value pairs from current category weight key value pairs query result that the new category weight key value pairs list containes. the category weight key value pairs remaimed in the list, are the which the new list does not contain.
                                        current_Cate_Weight.RemoveAll(x => arr.Select(s => s.Category).Contains(x.Test_Category.Category));

                                        var current_Cate_WeightToRemove = current_Cate_Weight;

                                        if (current_Cate_WeightToRemove.Count > 0)
                                        {
                                            _dbContext.Combine_Categories_Weight.RemoveRange(current_Cate_WeightToRemove);
                                            await _dbContext.SaveChangesAsync();
                                        }
                                    }
                                    transaction.Commit();

                                    ReturnResult = true;
                                }
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
                                    var databaseValues = (Combine)databaseEntry.ToObject();

                                    ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                                            + "was modified by another user after you got the original value. The "
                                            + "edit operation was canceled. If you still want to edit this record, click "
                                            + "the Save button again.");

                                    combineBuilder.RowVersion = (byte[])databaseValues.RowVersion;
                                    ModelState.Remove("RowVersion");
                                }

                                ReturnResult = false;
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Unable to save changes. " +
                                        "Try again, and if the problem persists, " +
                                        "see your system administrator.");

                            ReturnResult = false;
                        }
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", "Unable to save changes. " +
                                    "Try again, and if the problem persists, " +
                                    "see your system administrator.");
                        ReturnResult = false;
                    }
                }
            }
            if (ReturnResult)
            {
                return RedirectToAction("Combines");
            }
            else
            {
                combineBuilder.Gender = originalCombine.Gender;

                if (combineBuilder.Editable)
                {
                    List<CombineCateWeightEdit> list = new List<CombineCateWeightEdit>();

                    var combine_weight_cate = _dbContext.Combine_Categories_Weight.Where(w => w.C_ID == c_id).Include(inc => inc.Test_Category).ToList();

                    foreach (var x in combine_weight_cate)
                    {
                        var tests = _dbContext.Combine_Builder.Include(inc => inc.Test).Where(w => w.Test.TC_id == x.TC_ID && w.C_ID == c_id).Select(s => s.Test).ToList();

                        list.Add(new CombineCateWeightEdit { Category = x.Test_Category.Category, Weight = x.Weight * 100, Tests = tests });
                    }

                    ViewBag.Cate = list;
                }
                else
                {
                    List<CombineCateWeightEdit> list = new List<CombineCateWeightEdit>();

                    cate_weight_pair = '[' + cate_weight_pair + ']';

                    Category_Weight[] arr = JsonConvert.DeserializeObject<Category_Weight[]>(cate_weight_pair);

                    foreach (var x in arr)
                    {
                        var tests = await _dbContext.Test.Where(w => combineBuilder.Tests.Select(s => s.Test_ID).Contains(w.Test_ID))
                                                        .Include(inc => inc.Test_Category)
                                                        .Where(w => w.Test_Category.Category == x.Category).ToListAsync();

                        list.Add(new CombineCateWeightEdit
                        {
                            Category = x.Category,
                            Weight = x.Weight,
                            Tests = tests
                        });
                    }

                    ViewBag.Cate = list;
                }

                return View(combineBuilder);
            }
        }

        [Authorize("Admin")]
        [Authorize("Permission_All")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize("Admin")]
        [Authorize("Permission_All")]
        [HttpPost]
        public async Task<IActionResult> Create(CombineBuilder combineBuilder, string cate_weight_pair)
        {
            var success = false;

            if (!ModelState.IsValid)
            {
                success = false;
            }
            else if (ModelState.IsValid)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var Img_Path = "";
                        if (combineBuilder.Image != null)
                        {
                            var date = Request;
                            var files = Request.Form.Files;
                            long size = files.Sum(f => f.Length);
                            string contentRootPath = _hostingEnvironment.ContentRootPath;
                            IFormFile img = combineBuilder.Image;


                            if (img.Length > 0)
                            {
                                string fileExt = img.FileName;

                                while (fileExt.Contains('.'))
                                {
                                    fileExt = fileExt.Substring(fileExt.IndexOf('.') + 1);
                                }

                                long fileSize = img.Length;
                                var newFileName = System.Guid.NewGuid().ToString() + "." + fileExt;
                                string webRootPath = _hostingEnvironment.WebRootPath;
                                var filePath = webRootPath + "/sources/combineImg/" + newFileName;
                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    await img.CopyToAsync(stream);
                                }
                                Img_Path = "/sources/combineImg/" + newFileName;
                            }
                        }
                        else if (combineBuilder.Image == null)
                        {
                            Img_Path = _configuration["CombineImgNullString:Value"];
                        }
                        var combine = new Combine
                        {
                            Name = combineBuilder.Name,
                            Description = combineBuilder.Description,
                            Img_Path = Img_Path,
                            Gender = combineBuilder.Gender
                        };

                        _dbContext.Combine.Update(combine);
                        _dbContext.SaveChanges();

                        foreach (var x in combineBuilder.Tests)
                        {
                            if (x != null && x.Test_ID != 0)
                            {
                                var combine_Builder = new Combine_Builder
                                {
                                    C_ID = combine.C_ID,
                                    Test_ID = x.Test_ID,
                                    IsWeighted = !_UnWeightedTests.Contains(x.Name)
                                };

                                _dbContext.Combine_Builder.Update(combine_Builder);
                                _dbContext.SaveChanges();
                            }
                        }

                        cate_weight_pair = '[' + cate_weight_pair + ']';

                        Category_Weight[] arr = JsonConvert.DeserializeObject<Category_Weight[]>(cate_weight_pair);

                        foreach (var x in arr)
                        {
                            Combine_Categories_Weight combine_Categories_Weight = new Combine_Categories_Weight
                            {
                                TC_ID = _dbContext.Test_Category.Where(y => y.Category == x.Category).First().TC_id,
                                C_ID = combine.C_ID,
                                Weight = double.Parse(x.Weight.ToString()) / 100
                            };

                            _dbContext.Combine_Categories_Weight.Update(combine_Categories_Weight);
                            _dbContext.SaveChanges();
                        }

                        transaction.Commit();

                        success = true;
                    }
                    catch (Exception e)
                    {
                        cate_weight_pair = '[' + cate_weight_pair + ']';

                        Category_Weight[] arr = JsonConvert.DeserializeObject<Category_Weight[]>(cate_weight_pair);

                        List<CombineCateWeightEdit> list = new List<CombineCateWeightEdit>();

                        foreach (var x in arr)
                        {
                            var tests = await _dbContext.Test.Include(inc => inc.Test_Category).Where(w => w.Test_Category.Category == x.Category && combineBuilder.Tests.Select(s => s.Test_ID).Contains(w.Test_ID)).ToListAsync();

                            list.Add(new CombineCateWeightEdit { Category = x.Category, Weight = x.Weight, Tests = tests });
                        }

                        ViewBag.Cate = list;

                        success = false;
                    }
                }
            }

            if (success)
            {
                return RedirectToAction("Combines", "Combines");
            }
            else
            {
                cate_weight_pair = '[' + cate_weight_pair + ']';

                Category_Weight[] arr = JsonConvert.DeserializeObject<Category_Weight[]>(cate_weight_pair);

                List<CombineCateWeightEdit> list = new List<CombineCateWeightEdit>();

                foreach (var x in arr)
                {
                    var tests = await _dbContext.Test.Include(inc => inc.Test_Category).Where(w => w.Test_Category.Category == x.Category && combineBuilder.Tests.Select(s => s.Test_ID).Contains(w.Test_ID)).ToListAsync();

                    list.Add(new CombineCateWeightEdit { Category = x.Category, Weight = x.Weight * 100, Tests = tests });
                }

                ViewBag.Cate = list;

                ModelState.AddModelError("", "Unable to save changes. " +
                                           "Try again, and if the problem persists, " +
                                           "see your system administrator.");

                return View(combineBuilder);
            }
        }
    }
}
