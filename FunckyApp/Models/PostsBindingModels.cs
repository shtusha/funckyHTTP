using System;
using System.ComponentModel.DataAnnotations;

namespace FunckyApp.Models
{
    public class PostBindingModel
    {
        [Required]
        public string Message { get; set; }

        [Required]
        [Range(1, 5)]
        public int InflationRate { get; set; }
    }
}