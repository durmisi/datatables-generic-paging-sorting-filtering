using System.Collections.Generic;

namespace DataTablesGenerics.Grid
{
    public class GridResult<TViewModel>
    {
        private readonly IEnumerable<TViewModel> _data;
        private readonly int _recordsTotal;
        private readonly int _recordsFiltered;
        private readonly int _totalPages;

        public GridResult(IEnumerable<TViewModel> data, int recordsTotal, int recordsFiltered, int totalPages)
        {
            _data = data;
            _recordsTotal = recordsTotal;
            _recordsFiltered = recordsFiltered;
            _totalPages = totalPages;
        }

        public IEnumerable<TViewModel> Data { get { return _data; } }

        public int RecordsTotal { get { return _recordsTotal; } }

        public int RecordsFiltered { get { return _recordsFiltered; } }

        public int TotalPages { get { return _totalPages; } }
    }
}