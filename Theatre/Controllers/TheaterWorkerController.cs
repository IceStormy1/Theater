using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Theater.Abstractions.TheaterWorker;
using Theater.Contracts;
using Theater.Contracts.Theater;
using Theater.Entities.Theater;
using AutoMapper;

namespace Theater.Controllers
{
    [ApiController]
    [Route("api/worker")]
    public sealed class TheaterWorkerController : BaseController<TheaterWorkerParameters, TheaterWorkerEntity>
    {
        private readonly ITheaterWorkerService _theaterWorkerService;

        public TheaterWorkerController(
            ITheaterWorkerService theaterWorkerService,
            IMapper mapper) : base(theaterWorkerService, mapper)
        {
            _theaterWorkerService = theaterWorkerService;
        }

        /// <summary>
        /// Получить количество работников театра по каждому из типов должности 
        /// </summary>
        /// <response code="200">В случае успешного запроса</response>
        [HttpPost("total")]
        [ProducesResponseType(typeof(TotalWorkersModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTotalWorkers()
        {
            var totalWorkers = await _theaterWorkerService.GetTotalWorkers();

            return Ok(totalWorkers);
        }

        /// <summary>
        /// Получить краткую информацию о работниках театра по типу должности 
        /// </summary>
        /// <response code="200">В случае успешного запроса</response>
        [HttpPost("positionType/{positionType:int}")]
        [ProducesResponseType(typeof(DocumentCollection<TheaterWorkerShortInformationModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetShortInformationWorkersByPositionType([FromRoute] int positionType)
        {
            var workersShortInformation = 
                await _theaterWorkerService.GetShortInformationWorkersByPositionType(positionType);

            return Ok(new DocumentCollection<TheaterWorkerShortInformationModel>(workersShortInformation));
        }

        /// <summary>
        /// Получить полную информацию о работнике театра по его идентификатору
        /// </summary>
        /// <response code="200">В случае успешного запроса</response>
        /// <response code="404">В случае успешного запроса</response>
        [HttpGet("{theaterWorkerId:guid}")]
        [ProducesResponseType(typeof(TheaterWorkerModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTheaterWorkerById([FromRoute] Guid theaterWorkerId)
        {
            var theaterWorker = await Service.GetById(theaterWorkerId);

            return RenderResult(theaterWorker);
        }
    }
}