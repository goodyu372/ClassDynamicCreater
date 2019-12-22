using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Diagnostics;
using Faster.Core;
using System.IO;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraVerticalGrid;

namespace Mini_Mes
{
    public partial class FormClassDynamicCreater : DevExpress.XtraEditors.XtraForm
    {
        public FormClassDynamicCreater()
        {
            InitializeComponent();
        }

        private void FormClassDynamicCreater_Load(object sender, EventArgs e)
        {
            CustClassManager.Instance.Load();

            this.gridControl1.DataSource = CustClassManager.Instance.AllClasses;
            this.选择Type.DataSource = CustClassManager.Instance.AllAvailableTypes;
        }

        private void bt按类生成程序集_Click(object sender, EventArgs e)
        {
            if (CustClassManager.Instance.AllClasses.Count > 0)
            {
                foreach (var item in CustClassManager.Instance.AllClasses)
                {
                    var var = ClassDynamicCreater.CreateInstance(item.ClassName, item.PropertyInfos.ToList());
                    item.GetTypeFormInstance(var);
                }

                //重新绑定数据源
                this.选择Type.DataSource = CustClassManager.Instance.AllAvailableTypes;

                MessageBox.Show("生成完成！");
            }
            else
            {
                MessageBox.Show("没有任何需要生成的类！");
            }
        }

        private void gridView1_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            //如果e.Valid等于false就会触发InvalidRowException事件
            CustClassInfo item = (CustClassInfo)e.Row;
            if (!item.ClassName.StartsWith("class_"))
            {
                item.ClassName = "class_" + item.ClassName;  //类名前面加个前缀
            }
            //item.ClassName = item.ClassName.ToUpper();

            //校验类名是否为字母开头
            if (!CustClassManager.regex字母开头.IsMatch(item.ClassName))
            {
                e.Valid = false;
                return;
            }
            
            if (CustClassManager.Instance.AllClasses.ToList().Where(p=>p.ClassName.Equals(item.ClassName)).Count() > 1)
            {
                e.Valid = false;
            }
        }

        private void gridView1_InvalidRowException(object sender, DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventArgs e)
        {
            e.ErrorText = $@"不允许存在重复类名称！只能为字母开头！请检查 (按ESC键可取消编辑并退出)";
        }

        private void gridView2_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            int i = this.gridView1.FocusedRowHandle;
            if (i >= 0)
            {
                //如果e.Valid等于false就会触发InvalidRowException事件
                CustClassInfo citem = CustClassManager.Instance.AllClasses[i];
                CustPropertyInfo item = (CustPropertyInfo)e.Row;
                if (!item.PropertyName.StartsWith("property_"))
                {
                    item.PropertyName = "property_" + item.PropertyName;  //属性名前面加个前缀
                }
                //item.PropertyName = item.PropertyName.ToUpper();

                //校验属性名是否为字母开头
                if (!CustClassManager.regex字母开头.IsMatch(item.PropertyName))
                {
                    e.Valid = false;
                    return;
                }
                
                if (citem.PropertyInfos.ToList().Where(p => p.PropertyName.Equals(item.PropertyName)).Count() > 1)
                {
                    e.Valid = false; 
                }
            }
        }

        private void gridView2_InvalidRowException(object sender, DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventArgs e)
        {
            e.ErrorText = $@"不允许存在重复属性名称！只能为字母开头！请检查 (按ESC键可取消编辑并退出)";
        }

        private void bt保存_Click(object sender, EventArgs e)
        {
            bool b = CustClassManager.Instance.Save();
            MessageBox.Show("保存结果"+b.ToString());
        }

        private void bt打开输出目录_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                if (!Directory.Exists(ClassDynamicCreater.FileOutPutsDirectory))
                {
                    Directory.CreateDirectory(ClassDynamicCreater.FileOutPutsDirectory);
                }
                Process.Start("explorer.exe", $@"{ClassDynamicCreater.FileOutPutsDirectory}");
            });
        }

        private void bt总览_Click(object sender, EventArgs e)
        {
            FormInfosView formTest = new FormInfosView();
            formTest.Show();
        }

        private void 查看_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int i = this.gridView1.FocusedRowHandle;
            if (i >= 0) 
            {
                CustClassInfo item = CustClassManager.Instance.AllClasses[i];
                XtraForm form = new XtraForm();
                if (item.ListOrSingle)
                {
                    GridView gv = new GridView();
                    gv.OptionsView.ShowGroupPanel = false;
                    gv.OptionsView.NewItemRowPosition = NewItemRowPosition.Top;
                    gv.OptionsView.ShowFooter = true;
                    GridControl gc = new GridControl();
                    gc.Dock = DockStyle.Fill;
                    gc.MainView = gv;
                    gc.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { gv });
                    gv.GridControl = gc;

                    foreach (var itemP in item.ClassType.GetProperties())
                    {
                        Type typeP = itemP.PropertyType; 
                        if (typeP.IsGenericType)   //判断是否有属性为List<>     //注意这里最多嵌套两层，三层或以上的情况懒得再递归了，一般最多两层
                        {
                            Type listSubType = typeP.GetGenericArguments()[0];  //如List<string>获取到string type


                            GridView gvSub = new GridView();  
                            gvSub.OptionsView.ShowGroupPanel = false;
                            gvSub.OptionsView.NewItemRowPosition = NewItemRowPosition.Top;
                            gvSub.OptionsView.ShowFooter = true;
                            gvSub.GridControl = gc;

                            GridLevelNode gridLevelNode = new GridLevelNode();
                            gridLevelNode.LevelTemplate = gvSub;
                            gridLevelNode.RelationName = $@"{itemP.Name}";
                            gc.LevelTree.Nodes.AddRange(new GridLevelNode[] {gridLevelNode});
                        }
                    }

                    object genericList = ClassDynamicCreater.CreateGeneric(typeof(BindingList<>), item.ClassType);
                    if (item.DataSourceTag == null || item.DataSourceTag.GetType() != genericList.GetType())
                    {
                        item.DataSourceTag = genericList;
                    }
                    //IBindingList ibl = item.DataSourceTag as IBindingList;
                    gc.DataSource = item.DataSourceTag;

                    form.Controls.Add(gc);
                }
                else
                {
                    PropertyGridControl pc = new PropertyGridControl();
                    pc.Dock = DockStyle.Fill;
                    pc.SelectedObject = item.Instance;
                    form.Controls.Add(pc);
                }
                form.ShowDialog();
            }
        }
    }

    public class FormClassDynamicCreaterView : LazyAbstractView<FormClassDynamicCreater>
    {
        public override string InsertPath => @"自定义类生成器";

        public override Image BarImage => Properties.Resources.配置;
    }
}