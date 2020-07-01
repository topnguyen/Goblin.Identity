using System.Threading;
using System.Threading.Tasks;
using Elect.Web.Swagger.Attributes;
using Goblin.Identity.Contract.Service;
using Goblin.Identity.Share;
using Goblin.Identity.Share.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Goblin.Identity.Controllers
{
    public class SampleController : BaseController
    {
        private readonly ISampleService _sampleService;

        public SampleController(ISampleService sampleService)
        {
            _sampleService = sampleService;
        }

        /// <summary>
        ///     Create Sample
        /// </summary>
        /// <remarks>
        ///     <b>SampleData</b>: Cannot be null or empty <br />
        /// </remarks>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ApiDocGroup("Sample")]
        [HttpPost]
        [Route(GoblinIdentityEndpoints.CreateSample)]
        [SwaggerResponse(StatusCodes.Status201Created, "Sample Saved", typeof(GoblinIdentitySampleModel))]
        public async Task<IActionResult> Upload([FromBody] GoblinIdentityCreateSampleModel model, CancellationToken cancellationToken = default)
        {
            var sampleModel = await _sampleService.CreateAsync(model, cancellationToken);
            
            return Created(Url.Action("Get", new {sampleModel.Id}), sampleModel);
        }
        
        /// <summary>
        ///     Get Sample
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ApiDocGroup("Resource")]
        [HttpGet]
        [Route(GoblinIdentityEndpoints.GetSample)]
        [SwaggerResponse(StatusCodes.Status200OK, "Sample Information", typeof(GoblinIdentitySampleModel))]
        public async Task<IActionResult> Get([FromRoute] long id, CancellationToken cancellationToken = default)
        {
            var sampleModel = await _sampleService.GetAsync(id, cancellationToken);

            return Ok(sampleModel);
        }
        
        /// <summary>
        ///     Delete Sample
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ApiDocGroup("Resource")]
        [HttpDelete]
        [Route(GoblinIdentityEndpoints.DeleteSample)]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Sample Deleted")]
        public async Task<IActionResult> Delete([FromRoute] long id, CancellationToken cancellationToken = default)
        {
            await _sampleService.DeleteAsync(id, cancellationToken);
            
            return NoContent();
        }
    }
}