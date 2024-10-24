using Microsoft.AspNetCore.Mvc;
using WizardTest.App;

namespace WizardTest.Host.Web;

[ApiController]
[Route("api/service-entities")]
public class ServiceEntityController : ControllerBase
{
    private readonly IServiceEntityStore _store;

    public ServiceEntityController(IServiceEntityStore store) => _store = store;

    [HttpPost]
    public async Task<ActionResult> CreateAsync([FromBody] ServiceEntityApiModel model)
    {
        var result = await _store.InsertAsync(model);
        return CreatedAtRoute(
            nameof(ReadOneAsync),
            new {
                id = result.Id
            },
            ServiceEntityApiModel.FromInterface(result));
    }

    [HttpGet("{id}", Name = nameof(ReadOneAsync))]
    public async Task<ActionResult<ServiceEntityApiModel>> ReadOneAsync(string id)
    {
        var result = await _store.FindAsync(id);
        return result == null ? NotFound() : Ok(ServiceEntityApiModel.FromInterface(result));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(string id)
    {
        await _store.DeleteAsync(id);
        return Ok();
    }
}
