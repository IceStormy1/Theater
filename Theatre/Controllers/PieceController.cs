using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using AutoMapper;
using Theater.Abstractions;
using Theater.Abstractions.Piece;
using Theater.Contracts;
using Theater.Contracts.Filters;
using Theater.Entities.Theater;
using Theater.Controllers.BaseControllers;
using Swashbuckle.AspNetCore.Annotations;
using Theater.Abstractions.Filters;
using Theater.Contracts.Theater.Piece;

namespace Theater.Controllers;

[ApiController]
[SwaggerTag("Пользовательские методы для работы с пьесами")]
public sealed class PieceController : CrudServiceBaseController<PieceParameters>
{
    private readonly IPieceService _pieceService;
    private readonly IIndexReader<PieceModel, PieceEntity, PieceFilterSettings> _pieceIndexReader;

    public PieceController(
        IPieceService service, 
        IMapper mapper,
        IIndexReader<PieceModel, PieceEntity, PieceFilterSettings> pieceIndexReader) : base(service, mapper)
    {
        _pieceService = service;
        _pieceIndexReader = pieceIndexReader;
    }

    /// <summary>
    /// Получить полную информацию о пьесе по идентификатору
    /// </summary>
    /// <param name="pieceId">Идентификатор пьесы</param>
    /// <response code="200">В случае успешной регистрации</response>
    [HttpGet("{pieceId:guid}")]
    [ProducesResponseType(typeof(PieceModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPieceById([FromRoute] Guid pieceId)
    {
        var piecesResult = await _pieceService.GetPieceById(pieceId);

        return RenderResult(piecesResult);
    }

    /// <summary>
    /// Получить краткую информацию об актуальных пьесах
    /// </summary>
    /// <remarks>
    /// Доступна сортировка по полям:
    /// * name
    /// * genre
    /// </remarks>
    /// <response code="200">В случае успешного запроса</response>
    [HttpGet]
    [ProducesResponseType(typeof(Page<PieceShortInformationModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPiecesShortInformation([FromQuery] PieceFilterParameters filterParameters)
    {
        var pieceFilterSettings = Mapper.Map<PieceFilterSettings>(filterParameters);

        var piecesShortInformation = await _pieceIndexReader.QueryItems(pieceFilterSettings);

        var result = Mapper.Map<Page<PieceShortInformationModel>>(piecesShortInformation);

        //TODO: добавить в QueryItems обогащение модели
        await _pieceService.EnrichPieceShortInformation(result);

        return Ok(result);
    }
}