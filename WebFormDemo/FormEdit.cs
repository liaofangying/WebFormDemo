using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebFormDemo
{
    public partial class FormEdit : Form
    {
        DBDataContext db = new DBDataContext();
        public TimeTable TimeTable { get; set; }
        public FormEdit()
        {
            InitializeComponent();
            Load += FormEdit_Load;
        }//主窗体

        private void FormEdit_Load(object sender, EventArgs e)
        {
            BoundType();
            BoundTimeTable();
        }

        private void BoundTimeTable()
        {
            if (TimeTable == null) return;
            try
            {
                txtTitle.Text = TimeTable.Title;
                txtAddress.Text = TimeTable.Address;
                cboType.SelectedValue = TimeTable.TypeId;
                dtBeginTime.Value = Convert.ToDateTime(TimeTable.BeginTime);
                dtEndTime.Value = Convert.ToDateTime(TimeTable.EndTime);
                txtContent.Rtf = TimeTable.MainContent;
                cbStatus.Checked = Convert.ToBoolean(TimeTable.IsFinished);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }//填充

        private void BoundType()
        {
            cboType.DataSource = db.Type.ToList();
            cboType.DisplayMember = "Name";
            cboType.ValueMember = "Id";
        }//下拉选择类型

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (TimeTable == null)
            {
                db.TimeTable.InsertOnSubmit(new TimeTable
                {
                    Title = txtTitle.Text,
                    Address = txtAddress.Text,
                    TypeId = Convert.ToInt32(cboType.SelectedValue),
                    BeginTime = dtBeginTime.Value,
                    EndTime = dtEndTime.Value,
                    MainContent = txtContent.Rtf,
                    IsFinished = cbStatus.Checked
                });
            }//新建项目
            else
            {
                var item = db.TimeTable.FirstOrDefault(t => t.Id == TimeTable.Id);
                item.Title = txtTitle.Text;
                item.Address = txtAddress.Text;
                item.TypeId = Convert.ToInt32(cboType.SelectedValue);
                item.BeginTime = dtBeginTime.Value;
                item.EndTime = dtEndTime.Value;
                item.MainContent = txtContent.Rtf;
                item.IsFinished = cbStatus.Checked;
            }//修改
            db.SubmitChanges();//提交至数据库
        }//点击【保存】

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();//点击【取消】关闭FormEdit窗口
        }
    }
}
