using Grit.Net.Common.TypeEx;
using System.Collections.Generic;

namespace Grit.Net.Common.Models.Page
{
    public class PageTable<T>
    {
        public PageTable()
        {
            this._iDisplayStart = 0;
            this._iDisplayLength = 10;
            this._sEcho = 1;
        }

        private int _iDisplayStart;

        /// <summary>
        ///  开始索引
        /// </summary>
        public int iDisplayStart
        {
            get
            {
                return _iDisplayStart;
            }
            set
            {
                _iDisplayStart = value;
            }
        }
        private int _iDisplayLength;
        /// <summary>
        /// 页面大小
        /// </summary>
        public int iDisplayLength
        {
            get
            {
                return _iDisplayLength;
            }
            set
            {
                _iDisplayLength = value;
            }
        }
        private int _iColumns;
        /// <summary>
        /// 显示的列数
        /// </summary>
        public int iColumns
        {
            get
            {
                return _iColumns;
            }
            set
            {
                _iColumns = value;
            }
        }
        private string _sSearch;

        /// <summary>
        /// 全局搜索字段
        /// </summary>
        public string sSearch
        {
            get
            {
                return _sSearch;
            }
            set
            {
                _sSearch = value;
            }
        }
        public int _sEcho;
        /// <summary>
        /// 请求次数
        /// </summary>
        public int sEcho
        {
            get
            {
                return _sEcho;
            }
            set
            {
                _sEcho = value;
            }
        }
        private string[] _mDataProp;
        /// <summary>
        /// 字段集合
        /// </summary>
        public string[] mDataProp
        {
            get
            {
                return _mDataProp;
            }
            set
            {
                _mDataProp = value;
            }
        }
        private string[] _iSortCol;
        /// <summary>
        /// 排序列序号
        /// </summary>
        public string[] iSortCol
        {
            get
            {
                return _iSortCol;
            }
            set
            {
                _iSortCol = value;
            }
        }
        private string[] _sSortDir;
        /// <summary>
        /// 排序列 正序或倒序
        /// </summary>
        public string[] sSortDir
        {
            get
            {
                return _sSortDir;
            }
            set
            {
                _sSortDir = value;
            }
        }
        private bool[] _bSortable;
        /// <summary>
        ///排序列 是否在客户端被标志位可排序
        /// </summary>
        public bool[] bSortable
        {
            get
            {
                return _bSortable;
            }
            set
            {
                _bSortable = value;
            }
        }
        private int _iSortingCols;
        /// <summary>
        /// 排序的列数 
        /// </summary>
        public int iSortingCols
        {
            get
            {
                return _iSortingCols;
            }
            set
            {
                _iSortingCols = value;
            }
        }
        private string _sortStr;

        public string sortStr
        {
            get
            {
                return _sortStr;
            }
            set
            {
                _sortStr = value;
            }
        }

        private int _iTotalRecords;
        /// <summary>
        /// 数据表中数据总和
        /// </summary>
        public int iTotalRecords
        {
            get
            {
                return _iTotalRecords;
            }
            set
            {
                _iTotalRecords = value;
            }
        }
        public int _iTotalDisplayRecords;
        /// <summary>
        /// 显示的数据总和
        /// </summary>
        public int iTotalDisplayRecords
        {
            get
            {
                return _iTotalDisplayRecords;
            }
            set
            {
                _iTotalDisplayRecords = value;
            }
        }
        private Mapping<string, object> _specCols;
        //排序列的特殊处理
        public Mapping<string, object> specCols
        {
            get
            {
                return _specCols;
            }
            set
            {
                _specCols = value;
            }
        }

        /// <summary>
        /// 数据
        /// </summary>
        private IList<T> _aaData;
        public IList<T> aaData
        {
            get
            {
                if (_aaData == null)
                    _aaData = new List<T>();
                return _aaData;
            }
            set
            {
                _aaData = value;
            }
        }
    
    }
}
