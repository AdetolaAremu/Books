using Microsoft.AspNetCore.Mvc;

namespace bookreview.Helpers
{
 public static class ResponseHelper
 {
  public static IActionResult SuccessResponseHelper(string message, object data = null, int statusCode=200)
  {
    var response = new {
      status = "success",
      message,
      data
    };

    return new ObjectResult(response) { StatusCode = statusCode };
  }

  public static IActionResult ErrorResponseHelper(string message, object error = null, int statusCode=400)
  {
    var response = new {
      status = "fail",
      message,
      error
    };

    return new ObjectResult(response) { StatusCode = statusCode };
  }
 } 
}