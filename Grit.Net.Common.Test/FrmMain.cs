using Grit.Net.Common.Log;
using Grit.Net.DataAccess;
using Grit.Net.DataAccess.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tingche.Easy.Park.Support.Web.Model;

namespace Grit.Net.Common.Test
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            Logging.Create(AppDomain.CurrentDomain.BaseDirectory);
            DataHelper.Create();
            bool result = DataHelper.IsConnection();

            GeneralDAL<City> dal = new GeneralDAL<City>();
            var list = dal.GetList();
        }
    }

}
