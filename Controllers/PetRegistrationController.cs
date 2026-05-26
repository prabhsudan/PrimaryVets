using Microsoft.AspNetCore.Mvc;
using PrimaryVets.Models;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Whisper.net;

namespace PrimaryVets.Controllers
{
    public class PetRegistrationController : Controller
    {
        private readonly WhisperFactory _whisperFactory;
        private readonly IWebHostEnvironment _env;
        public PetRegistrationController(WhisperFactory whisperFactory, IWebHostEnvironment env )
        {
            _whisperFactory = whisperFactory;
            _env = env;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(PetRegisterViewModel model, string selectedTests)
        {
            model.SelectedTests = selectedTests;
            TempData["PetData"] = JsonSerializer.Serialize(model);

            return RedirectToAction("Create2");
        }

        [HttpGet]
        public IActionResult Create2()
        {
            if (TempData["PetData"] == null)
                return RedirectToAction("Create");

            TempData.Keep("PetData");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create2(PetRegisterViewModel page2Model, List<IFormFile> Image)
        {
            if (TempData["PetData"] is string json)
            {
                var model = JsonSerializer.Deserialize<PetRegisterViewModel>(json);

                if (model != null)
                {
                    model.Remarks = page2Model.Remarks;
                    if (Image != null && Image.Count > 0)
                    {
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);

                        foreach (var file in Image)
                        {
                            var filePath = Path.Combine(path, file.FileName);

                            using var stream = new FileStream(filePath, FileMode.Create);
                            await file.CopyToAsync(stream);
                        }
                    }

                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Create");
        }

        [HttpPost]
        [Route("PetRegistration/ProcessAudioFile")]
        public async Task<IActionResult> ProcessAudioFile([FromForm] IFormFile audioFile)
        {
            if (audioFile == null || audioFile.Length == 0)
                return BadRequest("No audio received");

            string uploadDir = Path.Combine(_env.WebRootPath, "uploads", "audio");
            Directory.CreateDirectory(uploadDir);

            string webmPath = Path.Combine(uploadDir, $"{Guid.NewGuid()}.webm");
            string wavPath = Path.ChangeExtension(webmPath, ".wav");

            try
            {
                await using (var fs = new FileStream(webmPath, FileMode.Create))
                {
                    await audioFile.CopyToAsync(fs);
                }
                //var ffmpegPath = @"C:\Users\hp\Downloads\ffmpeg-2026-05-18-git-b4d11dffbf-full_build\bin\ffmpeg.exe";

                var ffmpegPath = Path.Combine(AppContext.BaseDirectory,"ffmpeg","bin","ffmpeg.exe");

                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = ffmpegPath,
                        Arguments = $"-y -i \"{webmPath}\" -ar 16000 -ac 1 \"{wavPath}\"",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.Start();

               
                await process.WaitForExitAsync();

                if (!System.IO.File.Exists(wavPath))
                {
                    return StatusCode(500, new
                    {
                        message = "FFmpeg conversion failed",
                    });
                }
                using var audioStream = new FileStream(wavPath, FileMode.Open, FileAccess.Read);

                var whisper = _whisperFactory.CreateBuilder()
                    .WithLanguage("auto")
                    .Build();

                var textBuilder = new StringBuilder();

                await foreach (var segment in whisper.ProcessAsync(audioStream))
                {
                    textBuilder.Append(segment.Text);
                }

                string text = textBuilder.ToString();

                if (string.IsNullOrWhiteSpace(text))
                {
                    return Ok(new { message = "No speech detected", rawText = "" });
                }

                bool isNotesOnly = Request.Form.TryGetValue("isNotesOnly", out var notesFlag) && notesFlag.ToString()
                    .Equals("true", StringComparison.OrdinalIgnoreCase);
                if (isNotesOnly)
                {
                    return Ok(new
                    {
                        text = text
                    });
                }
                var result = ParseVoice(text);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = ex.Message
                });
            }
        }
        private PetRegisterViewModel ParseVoice(string text)
        {
            var model = new PetRegisterViewModel();

            text = text.ToLower();

            model.Name = Extract(text,
                @"(?<!pet\s)name\s+is\s+(.+?)(pet name|species|gender|age|$)"
            );

            model.PetName = Extract(text,
                @"pet\s+name\s+is\s+(.+?)(species|gender|age|$)"
            );

            model.Species = ExtractFlexible(text,
                @"(species\s*(is)?\s*)?(dog|cat|bird|rabbit|other)");

            model.Gender = ExtractFlexible(text,
                @"(gender\s*(is)?\s*)?(male|female)");

            model.Breed = ExtractFlexible(text,
                @"breed\s*(is)?\s*(.+?)(mobile|weight|age|$)");

            model.Mobile = ExtractFlexible(text, @"(\d{10})");

            model.Treatment = ExtractFlexible(text,
                @"(treatment\s*(is)?\s*)?(vaccination|surgery|checkup|emergency)");

            var age = Regex.Match(text, @"age\s*(?:is)?\s*(\d+)", RegexOptions.IgnoreCase);
            if (age.Success)
                model.Age = int.Parse(age.Groups[1].Value);

            var weight = Regex.Match(text, @"weight\s*(?:is)?\s*(\d+)", RegexOptions.IgnoreCase);
            if (weight.Success)
                model.Weight = int.Parse(weight.Groups[1].Value);

            var temp = Regex.Match(text, @"temperature\s*(?:is)?\s*(\d+)", RegexOptions.IgnoreCase);
            if (temp.Success)
                model.BodyTemperature = int.Parse(temp.Groups[1].Value);

            return model;
        }

        private string ExtractFlexible(string text, string pattern)
        {
            var match = Regex.Match(text, pattern);
            if (!match.Success) return "";
            var value = match.Groups[match.Groups.Count - 1].Value;

            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.Trim());
        }
        private string Extract(string text, string pattern)
        {
            var match = Regex.Match(text, pattern);
            return match.Success ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(match.Groups[1].Value.Trim()) : "";
        }
    }
}
