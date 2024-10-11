using MarkdownNoteTakingAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Markdig;

namespace MarkdownNoteTakingAPI.Controllers
{
    [ApiController]
    [Route("Markdown")]
    public class MarkdownController : ControllerBase
    {
        protected string _folder;
        protected readonly IConfiguration _configuration;
        public MarkdownController(IConfiguration configuration)
        {
            _configuration = configuration;
            _folder = _configuration.GetValue<string>("MarkdownDirectory");
        }
        [HttpPost]
        [Route("MarkdownFile")]
        public async Task<IActionResult> UploadMarkdownFile(IFormFile file)
        {
            FileInfo fileInfo = new FileInfo(file.FileName);
            if (fileInfo.Extension != ".md")
                return BadRequest("Only accepts .md file");
            if(!Directory.Exists(_folder))
                Directory.CreateDirectory(_folder);
            var path = Path.Combine(_folder, fileInfo.Name);

            // saves a file in folder, defined by path
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new { success = $"{fileInfo.Name} saved sucessfully" });
        }
        [HttpPost]
        [Route("MarkdownFile/{filename}")]
        public IActionResult WriteMarkdownFile([FromRoute] string filename, [FromBody] string text)
        {
            try
            {
                var path = Path.Combine(_folder, filename);

                if (!System.IO.File.Exists(path))
                    return BadRequest(new { error = "File does exist!" });
                string decoded = text.Replace(@"\n", "\n");
                System.IO.File.WriteAllText(path, decoded);

                return Ok(new { success = "!" });

            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            
        }
        [HttpGet]
        [Route("MarkdownFile")]
        public IActionResult GetMarkdownFiles()
        {
            try
            {
                List<string> data = new List<string>();

                string[] files = Directory.GetFiles(_folder, "*.md");
                foreach (string file in files)
                {
                    var fileName = file.Split('\\').Last();
                    data.Add(fileName);
                }
                return Ok(new { SavedMarkdownFiles = data });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [HttpGet]
        [Route("MarkdownFile/{filename}")]
        public async Task<IActionResult> GetMarkdownFile([FromRoute] string filename)
        {
            try
            {
                var path = Path.Combine(_folder, filename); 
                var markdown = System.IO.File.ReadAllText(path); //reads .md file
                var html = Markdown.ToHtml(markdown); //converts raw .md text to HTML

                return Ok(new { MarkdownFile = html });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [HttpPost]
        [Route("CheckGrammar/{filename}")]
        public async Task<IActionResult> CheckGrammar([FromRoute] string filename)
        {
            var path = Path.Combine(_folder, filename);
            FileInfo fInfo = new FileInfo(path);
            if (!fInfo.Exists)
                return BadRequest($"File {filename} not found at {_folder}");
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
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
