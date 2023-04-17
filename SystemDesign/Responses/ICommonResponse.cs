using System.Text.Json.Nodes;
using System.Text.Json;
using SystemDesign.UserManager.Services;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace SystemDesign.Models {
  public class CommonResponse<T> {
    public bool IsSuccess { get; set; } = true;
    public bool IsError { get; set; } = false;
    public string? Message { get; set; }
    public T? Data { get;set; }
    public List<string> Errors { get; set; } = new List<string>();
    public CommonResponse(T? data) {
      this.Data = data;
    }
    public CommonResponse() { }
  }

  public class CommonJsonResponse : CommonResponse<JsonNode> {
    private static JsonSerializerOptions GetJsonOption() { 
      return new JsonSerializerOptions() {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
      };
    }
    public CommonJsonResponse(Object? data) :base(JsonSerializer.SerializeToNode(data,GetJsonOption())){
    }
    public CommonJsonResponse() { }
  }
}
