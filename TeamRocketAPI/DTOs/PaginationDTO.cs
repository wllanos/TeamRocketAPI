namespace TeamRocketAPI.DTOs
{
    public class PaginationDTO
    {
        public int Page { get; set; } = 1;
        private int recordsPerPage = 15;
        private readonly int maximunRecordsPerPage = 100;

        public int RecordsPerPage
        {
            get
            {
                return recordsPerPage;
            }
            set
            {
                recordsPerPage = (value > maximunRecordsPerPage) ? maximunRecordsPerPage : value;
            }
        }


    }
}
