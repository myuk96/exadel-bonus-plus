using AutoMapper;
using ExadelBonusPlus.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace ExadelBonusPlus.WebApi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class VendorController : ControllerBase
    {
        private IVendorService _vendorService;
        public VendorController(IVendorService vendorService, IMapper mapper)
        {
            _vendorService = vendorService;
        }
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "All Vendors", Type = typeof(ResultDto<List<VendorDto>>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResultDto<IEnumerable<VendorDto>>>> GetVendors(CancellationToken cancellationToken)
        {
            var vendors = await _vendorService.GetAllVendorsAsync(cancellationToken);
            return Ok(vendors);
        }
        [HttpGet("{id:Guid}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Vendor by ID", Type = typeof(ResultDto<VendorDto>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResultDto<VendorDto>>> GetVendor(Guid id, CancellationToken cancellationToken)
        {
            var vendor = await _vendorService.GetVendorByIdAsync(id, cancellationToken);

            return Ok(vendor);
        }
        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Vendor added ", Type = typeof(ResultDto<VendorDto>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResultDto<AddVendorDto>>> AddVendor([FromBody]AddVendorDto model, CancellationToken cancellationToken)
        {
           return Ok(await _vendorService.AddVendorAsync(model, cancellationToken));
        }

        [HttpPut]
        [Route("{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Vendor Updated ", Type = typeof(ResultDto<VendorDto>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResultDto<VendorDto>>> UpdateVendor([FromRoute][Required] Guid id, [FromBody][Required] VendorDto vendor,CancellationToken cancellationToken)
        {
            return Ok(await _vendorService.UpdateVendorAsync(id, vendor, cancellationToken));
        }
        [HttpDelete]
        [Route("{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Vendor Deleted ", Type = typeof(ResultDto<VendorDto>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResultDto<VendorDto>>> DeleteVendor([FromRoute]Guid id, CancellationToken cancellationToken)
        {
            return Ok(await _vendorService.DeleteVendorAsync(id, cancellationToken));
        }

        [HttpGet]
        [Route("{name}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Vendors Retrieved ", Type = typeof(ResultDto<IEnumerable<VendorDto>>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResultDto<IEnumerable<VendorDto>>>> GetVendorByName([FromRoute]string name, CancellationToken cancellationToken)
        {
            return Ok(await _vendorService.SearchVendorByNameAsync(name, cancellationToken));
        }
    }
}
