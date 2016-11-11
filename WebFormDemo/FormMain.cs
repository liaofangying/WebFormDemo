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
    public partial class FormMain : Form
    {
        DBDataContext db = new DBDataContext();
        public FormMain()
        {
            InitializeComponent();
            Closed += FormMain_Closed;
            Closed += 退出ToolStripMenuItem_Click;
            Load += FormMain_Load;
        }//主窗体

        void FormMain_Load(object sender, EventArgs e)
        {
            BoundType();
            BoundItems();
        }

        private void BoundItems()
        {
            dgvItems.DataSource = db.TimeTable
                .Where(item => item.Title.Contains(txtTitle.Text))
                .Where(item => cboType.SelectedIndex == 0? true: item.TypeId == Convert.ToInt32(cboType.SelectedValue))
                .Where(item => rbAll.Checked? true: item.IsFinished == Convert.ToBoolean(rbIsFinished.Checked))
                .Select(item => new
                {
                    编号 = item.Id,
                    标题 = item.Title,
                    分类 = item.Type.Name,
                    开始时间 = item.BeginTime,
                    结束时间 = item.EndTime,
                    状态 = Convert.ToBoolean(item.IsFinished)?"已完成":"未完成"
                }).ToList();
        }//对应数据库的字段
        private void BoundType()
        {
            var types = db.Type.ToList();
            types.Insert(0, new Type { Id = 0, Name = "全部" });
            cboType.DataSource = types;
            cboType.DisplayMember = "Name";
            cboType.ValueMember = "Id";
        }//下拉选择类型
        void FormMain_Closed(object sender, EventArgs e)
        {
            Application.Exit();
        }//点击右上角‘X’关闭窗口

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }//点击【退出】关闭窗口

        private void btnSearch_Click(object sender, EventArgs e)
        {
            BoundItems();
        }

        private void 新建项目ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormEdit frm = new FormEdit {TimeTable = null};//新增操作
            frm.ShowDialog();
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvItems.SelectedRows.Count > 0)
            {
                var id = Convert.ToInt32(dgvItems.SelectedRows[0].Cells[0].Value);
                var item = db.TimeTable.FirstOrDefault(t => t.Id == id);
                db.TimeTable.DeleteOnSubmit(item);
                db.SubmitChanges();
                BoundItems();
            }
        }//删除记录

        private void 修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvItems.SelectedRows.Count > 0)
            {
                var id = Convert.ToInt32(dgvItems.SelectedRows[0].Cells[0].Value);
                var item = db.TimeTable.FirstOrDefault(t => t.Id == id);
                FormEdit frm = new FormEdit { TimeTable = item};
                frm.ShowDialog();
            }
            
        }//修改记录
    }
}
