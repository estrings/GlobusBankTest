using System.ComponentModel.DataAnnotations;

namespace Customer.API.Common.ResourceParameters
{
    public class ResourceParameters
    {
        const int maxPageSize = 30;
        [Required]
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 20;
        [Required]
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
    }
}
