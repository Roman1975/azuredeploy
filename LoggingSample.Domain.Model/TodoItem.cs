using System;
using System.ComponentModel.DataAnnotations;

namespace LoggingSample.Domain.Model
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/complex-data-model
    /// </summary>
    public class TodoItem
    {
        [Key]
        public int Id { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsDelayed { get; set; }
        
        [Display(Name = "Title")]
        [StringLength(50, MinimumLength=5, ErrorMessage = "Title cannot be longer than 50 characters.")]
        public string Title { get; set; }
        
        [Display(Name = "Notes")]
        [StringLength(256, ErrorMessage = "Notes cannot be longer than 256 characters.")]
        public string Notes { get; set; }
        
        public DateTime DateStart { get; set; }
        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateFinish { get; set; }

    }
}
