using Application.Game.Commands;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

namespace WebUI.Controllers
{
    [ApiController]
    [Route("turtleChallenge")]
    public class TurtleChallengeController : Controller
    {
        private static IWebHostEnvironment _environment;
        private readonly IMediator _mediator;


        public TurtleChallengeController(
            IMediator mediator,
            IWebHostEnvironment environment)
        {
            _mediator = mediator;
            _environment = environment;
        }

        [HttpGet]
        public string GetText()
        {
            return "olá";
        }

        [HttpPost("uploadSettings")]
        [ProducesResponseType(typeof(Settings), StatusCodes.Status200OK)]
        public async Task<ActionResult> UploadSettingsAsync(
            [Required] IFormFile file)
        {
            if (file.Length > 0)
            {
                using (FileStream filestream = System.IO.File.Create(_environment.ContentRootPath + "\\temp\\settings.json"))
                {
                    await file.CopyToAsync(filestream);
                    filestream.Flush();
                }
            }

            return Ok();
        }

        /// <summary>
        /// [M]ovement/[R]otation, provide a JSON file with content: [m,m,r,m,r,...] 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("uploadMovements")]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
        public async Task<ActionResult> UploadMovementsAsync(
            [Required] IFormFile file)
        {
            if (file.Length > 0)
            {
                using (FileStream filestream = System.IO.File.Create(_environment.ContentRootPath + "\\temp\\movements.json"))
                {
                    await file.CopyToAsync(filestream);
                    filestream.Flush();
                }
            }

            return Ok();
        }

        [HttpPost("runGame")]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
        public async Task<ActionResult> RunGame()
        {
            string basePath = _environment.ContentRootPath + "\\temp\\";

            var result = await _mediator.Send(new RunGameCommand(
                                                        basePath + "settings.json",
                                                        basePath + "movements.json"));

            return Ok(result.GameInfo);
        }
    }
}
