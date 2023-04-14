using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Theater.Abstractions.PieceDates;
using Theater.Contracts.Theater;
using Theater.Entities.Theater;

namespace Theater.Controllers.Admin
{
    [Route("api/admin/piece/date")]
    public class PieceDateAdminController : BaseAdminController<PieceDateParameters, PieceDateEntity>
    {
        public PieceDateAdminController(
            IPieceDateService service, 
            IMapper mapper) : base(service, mapper)
        {
        }
    }
}
