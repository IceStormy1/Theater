﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Theater.Abstractions.Piece;
using Theater.Contracts.Theater;
using Theater.Entities.Theater;

namespace Theater.Controllers.Admin
{
    [Route("api/admin/piece")]
    public sealed class PieceAdminController : BaseAdminController<PieceParameters, PieceEntity>
    {
        public PieceAdminController(
            IMapper mapper, 
            IPieceService pieceService) : base(pieceService, mapper)
        {
        }
    }
}