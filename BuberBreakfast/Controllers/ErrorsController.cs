using BuberBreakfast.Contracts.Breakfast;
using BuberBreakfast.Models;
using BuberBreakfast.Services.Breakfasts;
using Microsoft.AspNetCore.Mvc; 

namespace BuberBreakfast.Controllers;

public class ErrorsController: ControllerBase {

    [Route("/error")]
    public IActionResult Error(){
        
        return Problem();
    }
}