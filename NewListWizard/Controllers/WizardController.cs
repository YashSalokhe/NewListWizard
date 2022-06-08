using Microsoft.AspNetCore.Mvc;


namespace NewListWizard.Controllers
{
    public class WizardController : Controller
    {
        private readonly NewListWizardContext wizardContext;
        private readonly ListService listService;
        public WizardController(NewListWizardContext wizardContext, ListService listService)
        {
            this.wizardContext = wizardContext;
            this.listService = listService;
        }
        public async Task<IActionResult> Index()
        {
            var contents = await wizardContext.WizardLists.Where(l => l.IsDeleted == (byte)isDeleted.isDeletedSetToFalse).ToListAsync();
            return View(contents);
        }

        public IActionResult CreateNewListPartial()
        {
            var currentList = HttpContext.Session.GetObject<WizardList>("newListInfo");
            if(currentList != null)
            {
                return PartialView(currentList);
            }
            return PartialView();
        }

        [HttpPost]
        public IActionResult CreateNewListPartial(WizardList wizardList)
            {
            if (ModelState.IsValid)
            {
                HttpContext.Session.SetObject("newListInfo", wizardList);
                return Ok();
            }
            return View();
        }

        public PartialViewResult FileUploadPartial()
        {
            Upload upload = new Upload();
            return PartialView(upload);
        }


        [HttpPost]
        public IActionResult FileUploadPartial(IFormFile uploadedFile)
        {
            if (ModelState.IsValid)
            {
                var result = listService.FileUploadAsync(uploadedFile).Result;
                return new ObjectResult(new { missing = result.MissingField , imported = result.ImportedField/* , contents = result.csvContents*/}) ;
            }
            return View(uploadedFile);
        }

        public PartialViewResult Display()
        {

            var result = HttpContext.Session.GetInt32("ListId");
            var res = wizardContext.CsvContents.Where(x => x.ListId == result).ToList();
            //var Content = HttpContext.Session.GetObject<List<CsvContent>>("csvContent");
            int missingField = (int)HttpContext.Session.GetInt32("missing");
            int importedField = (int)HttpContext.Session.GetInt32("imported");
            DisplayContent disp = new DisplayContent()
            {
                csvContents = res,
                MissingField=missingField,
                ImportedField = importedField
            };
            return PartialView("DisplayContentPartial",disp);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(IFormCollection formCollectioin)
        {
            string ids = formCollectioin["listId"];
            if(ids != null)
            {
                var str = await listService.DeleteAsync(ids);
                if (str == "Deleted")
                {
                    return RedirectToAction("Index");
                }
            }
            
            return RedirectToAction("Index");
        }

        //[HttpPost]
        //public async Task<IActionResult> SubmitAsync(IFormCollection formCollectioin)
        //{
        //    var str = await listService.OnSubmitAsync();
        //    return RedirectToAction("Index");
        //}
    }
}
