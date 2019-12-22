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
using Faster.Core;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraVerticalGrid;
using DevExpress.XtraGrid;
using DevExpress.XtraBars.Docking2010.Views.WindowsUI;
using DevExpress.XtraBars.Docking2010;
using System.IO;
using System.Diagnostics;

namespace Mini_Mes
{
    public partial class FormInfosView : DevExpress.XtraEditors.XtraForm
    {
        public FormInfosView()
        {
            InitializeComponent();
        }

        private void FormTest_Load(object sender, EventArgs e)
        {
            foreach (var item in CustClassManager.Instance.AllClasses)
            {
                var document = this.windowsUIView1.AddDocument(CreateSettingControl(item)) as Document;
                document.Caption = item.ClassName;
                var tile = this.windowsUIView1.Tiles[document];
                
                TileItemElement element = new TileItemElement() 
                { 
                    Text = item.ClassName, 
                    TextAlignment = TileItemContentAlignment.MiddleCenter,
                };
                tile.Elements.Add(element);

                //this.windowsUIView1.TileProperties.ItemSize = TileItemSize.Large;
            }
            //this.windowsUIView1.ActivateContainer(this.pageGroup1);
        }

        public const string ExportDirectory = @".\DataExported\";

        public Control CreateSettingControl(CustClassInfo item)
        {
            XtraUserControl xtraUserControl = new XtraUserControl();
            xtraUserControl.AutoScroll = true;
            xtraUserControl.Dock = DockStyle.Fill;
            xtraUserControl.Text = item.ClassName;

            SimpleButton button = new SimpleButton() { Text = "导出报表" };

            button.Appearance.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            button.Appearance.Options.UseFont = true;
            button.Dock = System.Windows.Forms.DockStyle.Bottom;
            button.Location = new System.Drawing.Point(0, 0);
            button.Size = new System.Drawing.Size(240, 50);
            xtraUserControl.Controls.Add(button);

            ProgressBarControl progressBar = new ProgressBarControl();
            progressBar.Dock = DockStyle.Bottom;
            progressBar.Properties.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            progressBar.Properties.EndColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            progressBar.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            progressBar.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            progressBar.Properties.StartColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            progressBar.Size = new System.Drawing.Size(240, 30);
            xtraUserControl.Controls.Add(progressBar);

            //XtraForm form = new XtraForm();

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

                if (item.ClassType == null) //如果还嵌套List
                {
                    GridView gvSub = new GridView();  //最多嵌套两层
                    gvSub.OptionsView.ShowGroupPanel = false;
                    gvSub.OptionsView.NewItemRowPosition = NewItemRowPosition.Top;
                    gvSub.OptionsView.ShowFooter = true;
                    gvSub.GridControl = gc;
                }

                object genericList = ClassDynamicCreater.CreateGeneric(typeof(BindingList<>), item.ClassType);
                if (item.DataSourceTag == null || item.DataSourceTag.GetType() != genericList.GetType())
                {
                    item.DataSourceTag = genericList; 
                }
                //IBindingList ibl = item.DataSourceTag as IBindingList;
                gc.DataSource = item.DataSourceTag;

                //form.Controls.Add(gc);
                xtraUserControl.Controls.Add(gc);

                button.Click += (o, e) =>
                {
                    if (!Directory.Exists(ExportDirectory))
                    {
                        Directory.CreateDirectory(ExportDirectory);
                    }
                    progressBar.Position = 0;
                    gv.ExportToXlsx($@"{ExportDirectory}{item.ClassName}.xlsx");
                    progressBar.Position = 100;

                    Task.Run(() =>
                    {
                        Process.Start("explorer.exe", ExportDirectory);
                    });
                };
            }
            else
            {
                PropertyGridControl pc = new PropertyGridControl();
                pc.Dock = DockStyle.Fill;
                pc.SelectedObject = item.Instance;
                //form.Controls.Add(pc);
                xtraUserControl.Controls.Add(pc);

                button.Click += (o, e) =>
                {
                    if (!Directory.Exists(ExportDirectory))
                    {
                        Directory.CreateDirectory(ExportDirectory);
                    }
                    progressBar.Position = 0;
                    pc.ExportToXlsx($@"{ExportDirectory}{item.ClassName}.xlsx");
                    progressBar.Position = 100;

                    Task.Run(() =>
                    {
                        Process.Start("explorer.exe", ExportDirectory);
                    });
                };
            }
            return xtraUserControl;
        }
    }
}