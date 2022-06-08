using Microsoft.AspNetCore.Hosting;
using System.Data;
using System.Net.Http.Headers;


namespace NewListWizard.Services
{
    public class ListService
    {
      
        private readonly IHttpContextAccessor http;
        IWebHostEnvironment hostEnvironment;
        private readonly NewListWizardContext context;


        public ListService(IHttpContextAccessor http, IWebHostEnvironment hostEnvironment, NewListWizardContext context)
        {
            this.hostEnvironment = hostEnvironment;
            this.http = http;
            this.context = context;
        }

        public async Task<DisplayContent> FileUploadAsync(IFormFile upload)
        {
            var createdList = http.HttpContext.Session.GetObject<WizardList>("newListInfo");
            createdList.CreatedDate = DateTime.Now;
            var currentUserEmail = http.HttpContext.Session.GetString("CurrentUserEmail");

            createdList.User = context.UserInfos.Where(u=>u.Email == currentUserEmail).First() ;
            IFormFile uploadedFile = upload;

            var postedFileName = ContentDispositionHeaderValue
                    .Parse(uploadedFile.ContentDisposition)
                    .FileName.Trim('"');

            FileInfo fileInfo = new FileInfo(postedFileName);

            if (fileInfo.Extension == ".csv")
            {
                List<CsvContent> content = new List<CsvContent>();
                int missingFields = 0;
                int presentFields = 0;
                var finalPath = Path.Combine(hostEnvironment.WebRootPath, "Files", postedFileName);

                using (var fs = new FileStream(finalPath, FileMode.Create))
                {


                    // Create a File into the folder
                    uploadedFile.CopyTo(fs);


                    var temp = await context.WizardLists.AddAsync(createdList);
                    var hold = await context.SaveChangesAsync();


                }
              //  http.HttpContext.Session.SetObject<WizardList>("ListInfo",createdList);
                http.HttpContext.Session.SetInt32("ListId", createdList.ListId);

                //Read the contents of CSV file.
                string csvData = File.ReadAllText(finalPath);

                //Execute a loop over the rows.
                foreach (string row in csvData.Split('\n'))
                {
                    if (!string.IsNullOrEmpty(row))
                    {
                        if (row.Split(',')[0] != string.Empty && row.Split(',')[1] != string.Empty && row.Split(',')[2] != string.Empty && row.Split(',')[3] != string.Empty && row.Split(',')[4] != string.Empty)
                        {
                            presentFields++;

                            content.Add(new CsvContent
                            {
                                FirstName = row.Split(',')[0],
                                LastName = row.Split(',')[1],
                                CompanyName = row.Split(',')[2],
                                Title = row.Split(',')[3],
                                Email = row.Split(',')[4],
                                ListId = createdList.ListId
                            });
                        }
                        else
                        {
                            missingFields++;
                        }
                    }
                }
                await context.CsvContents.AddRangeAsync(content);
                await context.SaveChangesAsync();

                DisplayContent newUpload = new DisplayContent()
                {
                    //File = upload,
                    csvContents = content,
                    MissingField = missingFields,
                    ImportedField = presentFields
                };
              //  http.HttpContext.Session.SetObject<List<CsvContent>>("csvContent", content);
                 http.HttpContext.Session.SetInt32("missing", newUpload.MissingField);
                 http.HttpContext.Session.SetInt32("imported", newUpload.ImportedField);
                http.HttpContext.Session.SetObject("newListInfo", new WizardList());
                return newUpload;
            }

            return null;
        }


        public async Task<string>DeleteAsync(string id)
        {
            var func = id.Split(new char[] { ',' });
            foreach (var listId in func)
            {
               var list = await context.WizardLists.Where(x => x.ListId == int.Parse(listId)).FirstAsync();
                list.IsDeleted = (byte)isDeleted.isDeletedSetToTrue;
            }
            await context.SaveChangesAsync();
            return "Deleted";
        }

        public async Task<string> OnSubmitAsync()
        {
            var contentsInCsv = http.HttpContext.Session.GetObject<List<CsvContent>>("csvContent");
            var wizardList = http.HttpContext.Session.GetObject<WizardList>("newListInfo");
            var temp = await context.WizardLists.AddAsync(wizardList);
            var hold = await context.SaveChangesAsync();
            foreach (var row in contentsInCsv)
            {
                row.ListId = wizardList.ListId;
            }
           
            await context.CsvContents.AddRangeAsync(contentsInCsv);
            await context.SaveChangesAsync();
            return "success";
        }
    }
}
