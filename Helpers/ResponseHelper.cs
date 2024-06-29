using Microsoft.AspNetCore.Mvc;

namespace bookreview.Helpers
{
 public static class ResponseHelper
 {
  public class SuccessResponseModel<T>
{
    public string status { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
}

public static IActionResult SuccessResponseHelper<T>(string message, T data, int statusCode = 200)
{
    var successResponse = new SuccessResponseModel<T>
    {
        status = "success",
        Message = message,
        Data = data
    };
    
    return new ObjectResult(successResponse) { StatusCode = statusCode };
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