using BuberBreakfast.Contracts.Breakfast;
using BuberBreakfast.Models;
using BuberBreakfast.ServiceErrors;
using BuberBreakfast.Services.Breakfasts;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace BuberBreakfast.Controllers;

[ApiController]
[Route("[controller]")]
public class BreakfastsController : ApiController
{

    private readonly IBreakfastService _breakfastService;

    public BreakfastsController(IBreakfastService breakfastService)
    {
        _breakfastService = breakfastService;
    }

    [HttpPost()]
    public IActionResult CreateBreakfast(CreateBreakfastRequest requests)
    {

        var breakfastToBreakfastResult = Breakfast.Create(
            requests.Name,
            requests.Description,
            requests.StartDateTime,
            requests.EndDateTime,
            requests.Savory,
            requests.Sweet
        );

        if (breakfastToBreakfastResult.IsError){
            return Problem(breakfastToBreakfastResult.Errors);
        }

        var breakfast = breakfastToBreakfastResult.Value;

        return _breakfastService.CreateBreakfast(breakfast).Match(
            _ => MakeCreatedActionResult(breakfast),
            errors => Problem(errors)
        ); ;
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetBreakfast(Guid id)
    {

        ErrorOr<Breakfast> getBreakfastResut = _breakfastService.GetBreakfast(id);

        return getBreakfastResut.Match(
            breakfast => Ok(MapBreakfastResponse(breakfast)),
            errors => Problem(errors)
        );
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpsertBreakfast(Guid id, UpsertBreakfastRequest requests)
    {

        var breakfastToBreakfastResult = Breakfast.Create(
            requests.Name,
            requests.Description,
            requests.StartDateTime,
            requests.EndDateTime,
            requests.Savory,
            requests.Sweet,
            id
        );

        if (breakfastToBreakfastResult.IsError){
            return Problem(breakfastToBreakfastResult.Errors);
        }

        var breakfast = breakfastToBreakfastResult.Value;

        return _breakfastService.UpsertBreakfast(breakfast).Match(
            upsertedBreakfast => upsertedBreakfast.isNewlyCreated ?  MakeCreatedActionResult(breakfast) : NoContent(),
            errors => Problem(errors)
        );;
    }

    [HttpDelete("{id:Guid}")]
    public IActionResult DeleteBreakfast(Guid id)
    {
        return _breakfastService.DeleteBreakfast(id).Match(
            _ => NoContent(),
            errors => Problem(errors)
        );
    }

    private static CreateBreakfastResponse MapBreakfastResponse(Breakfast breakfast)
    {
        return new CreateBreakfastResponse(
                    breakfast.Id,
                    breakfast.Name,
                    breakfast.Description,
                    breakfast.StartDateTime,
                    breakfast.EndDateTime,
                    breakfast.LastModifiedDateTime,
                    breakfast.Savory,
                    breakfast.Sweet
                );
    }

    private CreatedAtActionResult MakeCreatedActionResult(Breakfast breakfast)
    {
        return CreatedAtAction(
                    actionName: nameof(GetBreakfast),
                    routeValues: new { id = breakfast.Id },
                    value: MapBreakfastResponse(breakfast)
                );
    }
}