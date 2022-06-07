using Microsoft.AspNetCore.Mvc;


namespace NewListWizard.Controllers
{
    public class WizardController : Controller
    {
        private readonly NewListWizardContext wizardContext;
        private readonly FileService fileService;
        public WizardController(NewListWizardContext wizardContext, FileService fileService)
        {
            this.wizardContext = wizardContext;
            this.fileService = fileService;
        }
        public async Task<IActionResult> Index()
        {
            var contents = await wizardContext.WizardLists.Where(l => l.IsDeleted == (byte)isDeleted.isDeletedSetToFalse).ToListAsync();
            return View(contents);
        }

        public IActionResult CreateNewListPartial()
        {
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
               
                var result = fileService.FileUploadAsync(uploadedFile).Result;
                return new ObjectResult(new { missing = result.MissingField , imported = result.ImportedField/* , contents = result.csvContents*/}) ;
            }
            return View(uploadedFile);
        }

        public PartialViewResult Display()
        {

            var result = HttpContext.Session.GetInt32("ListId");
            var res = wizardContext.CsvContents.Where(x=>x.ListId == result).ToList();
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

      
    }
}
