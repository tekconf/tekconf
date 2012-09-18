namespace TekConf.UI.Web.Models
{
  public class ApiDocumentation
  {
    public string Resource { get; set; }
    public string HttpMethod { get; set; }
    public string Description { get; set; }
    public string Uri { get; set; }
    public string ExampleUri { get; set; }
    public string ExampleRequestPayload { get; set; }
    public string ExampleResponsePayload { get; set; }
    public string Group { get; set; }
  }
}