using MarkdownNoteTakingAPI.Data;
using Microsoft.AspNetCore.Mvc;

namespace MarkdownNoteTakingAPI.Controllers
{
    [ApiController]
    [Route("Markdown")]
    public class MarkdownController : ControllerBase
    {
        protected readonly IConfiguration _configuration;
        public MarkdownController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPost]
        public async Task<IActionResult> UploadMarkdownFile(IFormFile file)
        {
            FileInfo fileInfo = new FileInfo(file.FileName);

            if (fileInfo.Extension != ".md")
            {
                return BadRequest("Only accepts .md file");
            }
            var path = Path.Combine(_configuration.GetValue<string>("MarkdownDirectory"), fileInfo.Name);

            // saves a file in folder, defined by path
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new { success = $"{fileInfo.Name} saved sucessfully" });
        }
        [HttpGet]
        [Route("Files")]
        public IActionResult GetMarkdownFiles()
        {
            try
            {
                List<string> data = new List<string>();

                string[] files = Directory.GetFiles(_configuration.GetValue<string>("MarkdownDirectory"), "*.md");
                foreach (string file in files)
                {
                    var fileName = file.Split('\\').Last();
                    data.Add(fileName);
                }
                return Ok(new { SavedMarkdownFiles = data });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("CheckGrammar/{filename}")]
        public async Task<IActionResult> CheckGrammar([FromRoute] string filename)
        {
            var folder = _configuration.GetValue<string>("MarkdownDirectory");
            var path = Path.Combine(folder, filename);
            FileInfo fInfo = new FileInfo(path);
            if (!fInfo.Exists)
                return BadRequest($"File {filename} not found at {folder}");
            if (!filename.EndsWith(".md"))
                return BadRequest("Not a markdown file");
            //checking if files exists, if it's a .md file
            try
            {
                string fileContent = System.IO.File.ReadAllText(path);

                for (int i = 0; i < fileContent.Length; i++)
                {
                    if (fileContent[i] == '#' || fileContent[i] == '-' || fileContent[i] == '[' ||
                        fileContent[i] == ']' || fileContent[i] == '(' || fileContent[i] == ')' ||
                        fileContent[i] == '`' || fileContent[i] == '*')
                    {
                        fileContent = fileContent.Replace(fileContent[i], ' ');
                    }
                }

                var client = new HttpClient();
                string key = _configuration["SaplingAPIKey"];
                //using Sapling API for grammar check

                var request = new CheckGrammar(key, fileContent, "test_session");

                var response = await client.PostAsJsonAsync("https://api.sapling.ai/api/v1/edits", request);

                var responseData = await response.Content.ReadFromJsonAsync<GrammarCheckDTO>();

                return Ok(responseData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
