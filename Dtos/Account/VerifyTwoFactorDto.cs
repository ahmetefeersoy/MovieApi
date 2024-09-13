using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Account;

public class VerifyTwoFactorDto
{
    [Required]
    public string Username { get; set; }
    
    [Required]
    public string Code { get; set; }
}