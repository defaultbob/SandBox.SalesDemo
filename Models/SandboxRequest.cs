using System;
using System.ComponentModel.DataAnnotations;

public class SandboxRequest
{
    [Required]
    public string SessionId { get; set; }

    [Required]
    public string Name { get; set; }

    public string Description { get; set; }

    [Required]
    public string SourceDomain { get; set; }

    public string TargetDomain { get; set; }

    public string Type { get { return "Demo"; } }
    
    public string Status { get; set; }
}
