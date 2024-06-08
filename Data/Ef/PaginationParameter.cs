namespace Data;

public class PaginationParameter
    {
        private int _page=1;
        private int _pageSize=20;
        private int _skip=0;
        private int _take=20;

        private void _populateSkipandTake()
        {
            if(_page > 0 && _pageSize > 0)
            {
                this._skip = _page  * _pageSize - _pageSize;
                this._take = _pageSize;
            }
        }

        private void _populatePageandPageSize()
        {
            if(_take > 0)
            {
                this._page = (_skip + _take) / _take;
                this._pageSize = _take;
            }
        }

        public int Page
        {
            get
            {
                return _page;
            }
            set
            {
                this._page = value;
                this._populateSkipandTake();
            }
        }

        public int PageSize
        {
            get{
                return _pageSize;
            }
            set
            {
                this._pageSize = value;
                this._populateSkipandTake();
            }
        }

        public int Skip
        {
            get
            {
                return _skip;
            }
            set
            {
                this._skip = value;
                this._populatePageandPageSize();
            }
        }

        public int Take
        {
            get
            {
                return _take;
            }
            set
            {
                this._take = value;
                this._populatePageandPageSize();
            }
        }
    }
