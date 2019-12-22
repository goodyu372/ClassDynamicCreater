namespace Mini_Mes
{
    partial class FormClassDynamicCreater
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormClassDynamicCreater));
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn_PropertyName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_PropertyType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.选择Type = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn_ClassName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_ClassType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_ListOrSingle = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_Set = new DevExpress.XtraGrid.Columns.GridColumn();
            this.查看 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.bt按类生成程序集 = new DevExpress.XtraEditors.SimpleButton();
            this.bt保存 = new DevExpress.XtraEditors.SimpleButton();
            this.bt打开输出目录 = new DevExpress.XtraEditors.SimpleButton();
            this.bt总览 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.选择Type)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.查看)).BeginInit();
            this.SuspendLayout();
            // 
            // gridView2
            // 
            this.gridView2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn_PropertyName,
            this.gridColumn_PropertyType});
            this.gridView2.DetailHeight = 389;
            this.gridView2.FixedLineWidth = 3;
            this.gridView2.GridControl = this.gridControl1;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            this.gridView2.InvalidRowException += new DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventHandler(this.gridView2_InvalidRowException);
            this.gridView2.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(this.gridView2_ValidateRow);
            // 
            // gridColumn_PropertyName
            // 
            this.gridColumn_PropertyName.Caption = "PropertyName";
            this.gridColumn_PropertyName.FieldName = "PropertyName";
            this.gridColumn_PropertyName.MinWidth = 26;
            this.gridColumn_PropertyName.Name = "gridColumn_PropertyName";
            this.gridColumn_PropertyName.Visible = true;
            this.gridColumn_PropertyName.VisibleIndex = 0;
            this.gridColumn_PropertyName.Width = 97;
            // 
            // gridColumn_PropertyType
            // 
            this.gridColumn_PropertyType.Caption = "PropertyType";
            this.gridColumn_PropertyType.ColumnEdit = this.选择Type;
            this.gridColumn_PropertyType.FieldName = "PropertyType";
            this.gridColumn_PropertyType.MinWidth = 26;
            this.gridColumn_PropertyType.Name = "gridColumn_PropertyType";
            this.gridColumn_PropertyType.Visible = true;
            this.gridColumn_PropertyType.VisibleIndex = 1;
            this.gridColumn_PropertyType.Width = 97;
            // 
            // 选择Type
            // 
            this.选择Type.AutoHeight = false;
            this.选择Type.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.选择Type.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("FullName", "FullName")});
            this.选择Type.Name = "选择Type";
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gridControl1.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gridControl1.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gridControl1.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gridControl1.EmbeddedNavigator.Buttons.First.Enabled = false;
            this.gridControl1.EmbeddedNavigator.Buttons.First.Visible = false;
            this.gridControl1.EmbeddedNavigator.Buttons.Last.Visible = false;
            this.gridControl1.EmbeddedNavigator.Buttons.NextPage.Enabled = false;
            this.gridControl1.EmbeddedNavigator.Buttons.NextPage.Visible = false;
            this.gridControl1.EmbeddedNavigator.Buttons.PrevPage.Enabled = false;
            this.gridControl1.EmbeddedNavigator.Buttons.PrevPage.Visible = false;
            gridLevelNode1.LevelTemplate = this.gridView2;
            gridLevelNode1.RelationName = "PropertyInfos";
            this.gridControl1.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gridControl1.Location = new System.Drawing.Point(0, 222);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.选择Type,
            this.查看});
            this.gridControl1.Size = new System.Drawing.Size(1415, 671);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.UseEmbeddedNavigator = true;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1,
            this.gridView2});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn_ClassName,
            this.gridColumn_ClassType,
            this.gridColumn_ListOrSingle,
            this.gridColumn_Set});
            this.gridView1.DetailHeight = 389;
            this.gridView1.FixedLineWidth = 3;
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
            this.gridView1.OptionsView.ShowFooter = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.InvalidRowException += new DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventHandler(this.gridView1_InvalidRowException);
            this.gridView1.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(this.gridView1_ValidateRow);
            // 
            // gridColumn_ClassName
            // 
            this.gridColumn_ClassName.Caption = "ClassName";
            this.gridColumn_ClassName.FieldName = "ClassName";
            this.gridColumn_ClassName.MinWidth = 26;
            this.gridColumn_ClassName.Name = "gridColumn_ClassName";
            this.gridColumn_ClassName.Visible = true;
            this.gridColumn_ClassName.VisibleIndex = 0;
            this.gridColumn_ClassName.Width = 97;
            // 
            // gridColumn_ClassType
            // 
            this.gridColumn_ClassType.Caption = "ClassType";
            this.gridColumn_ClassType.FieldName = "ClassType";
            this.gridColumn_ClassType.MinWidth = 26;
            this.gridColumn_ClassType.Name = "gridColumn_ClassType";
            this.gridColumn_ClassType.OptionsColumn.AllowEdit = false;
            this.gridColumn_ClassType.Visible = true;
            this.gridColumn_ClassType.VisibleIndex = 1;
            this.gridColumn_ClassType.Width = 97;
            // 
            // gridColumn_ListOrSingle
            // 
            this.gridColumn_ListOrSingle.Caption = "ListOrSingle";
            this.gridColumn_ListOrSingle.FieldName = "ListOrSingle";
            this.gridColumn_ListOrSingle.MinWidth = 28;
            this.gridColumn_ListOrSingle.Name = "gridColumn_ListOrSingle";
            this.gridColumn_ListOrSingle.ToolTip = "生成设置为列表（true）或单个对象（false）";
            this.gridColumn_ListOrSingle.UnboundType = DevExpress.Data.UnboundColumnType.Boolean;
            this.gridColumn_ListOrSingle.Visible = true;
            this.gridColumn_ListOrSingle.VisibleIndex = 2;
            this.gridColumn_ListOrSingle.Width = 106;
            // 
            // gridColumn_Set
            // 
            this.gridColumn_Set.Caption = "查看生成";
            this.gridColumn_Set.ColumnEdit = this.查看;
            this.gridColumn_Set.MinWidth = 28;
            this.gridColumn_Set.Name = "gridColumn_Set";
            this.gridColumn_Set.Visible = true;
            this.gridColumn_Set.VisibleIndex = 3;
            this.gridColumn_Set.Width = 106;
            // 
            // 查看
            // 
            this.查看.AutoHeight = false;
            this.查看.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Search)});
            this.查看.ButtonsStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.查看.Name = "查看";
            this.查看.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.查看.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.查看_ButtonClick);
            // 
            // bt按类生成程序集
            // 
            this.bt按类生成程序集.Appearance.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt按类生成程序集.Appearance.Options.UseFont = true;
            this.bt按类生成程序集.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bt按类生成程序集.Location = new System.Drawing.Point(0, 0);
            this.bt按类生成程序集.Name = "bt按类生成程序集";
            this.bt按类生成程序集.Size = new System.Drawing.Size(1415, 222);
            this.bt按类生成程序集.TabIndex = 1;
            this.bt按类生成程序集.Text = "按类生成程序集、更新";
            this.bt按类生成程序集.Click += new System.EventHandler(this.bt按类生成程序集_Click);
            // 
            // bt保存
            // 
            this.bt保存.Appearance.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt保存.Appearance.Options.UseFont = true;
            this.bt保存.Dock = System.Windows.Forms.DockStyle.Right;
            this.bt保存.Location = new System.Drawing.Point(1175, 0);
            this.bt保存.Name = "bt保存";
            this.bt保存.Size = new System.Drawing.Size(240, 222);
            this.bt保存.TabIndex = 2;
            this.bt保存.Text = "保存";
            this.bt保存.Click += new System.EventHandler(this.bt保存_Click);
            // 
            // bt打开输出目录
            // 
            this.bt打开输出目录.Appearance.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt打开输出目录.Appearance.Options.UseFont = true;
            this.bt打开输出目录.Dock = System.Windows.Forms.DockStyle.Left;
            this.bt打开输出目录.Location = new System.Drawing.Point(0, 0);
            this.bt打开输出目录.Name = "bt打开输出目录";
            this.bt打开输出目录.Size = new System.Drawing.Size(240, 222);
            this.bt打开输出目录.TabIndex = 3;
            this.bt打开输出目录.Text = "打开输出目录";
            this.bt打开输出目录.Click += new System.EventHandler(this.bt打开输出目录_Click);
            // 
            // bt总览
            // 
            this.bt总览.Appearance.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt总览.Appearance.Options.UseFont = true;
            this.bt总览.Dock = System.Windows.Forms.DockStyle.Right;
            this.bt总览.Location = new System.Drawing.Point(935, 0);
            this.bt总览.Name = "bt总览";
            this.bt总览.Size = new System.Drawing.Size(240, 222);
            this.bt总览.TabIndex = 4;
            this.bt总览.Text = "数据总览";
            this.bt总览.Click += new System.EventHandler(this.bt总览_Click);
            // 
            // FormClassDynamicCreater
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1415, 893);
            this.Controls.Add(this.bt总览);
            this.Controls.Add(this.bt打开输出目录);
            this.Controls.Add(this.bt保存);
            this.Controls.Add(this.bt按类生成程序集);
            this.Controls.Add(this.gridControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormClassDynamicCreater";
            this.Text = "自定义类生成器";
            this.Load += new System.EventHandler(this.FormClassDynamicCreater_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.选择Type)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.查看)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.XtraEditors.SimpleButton bt按类生成程序集;
        private DevExpress.XtraEditors.SimpleButton bt保存;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_ClassName;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_PropertyName;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_PropertyType;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_ClassType;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit 选择Type;
        private DevExpress.XtraEditors.SimpleButton bt打开输出目录;
        private DevExpress.XtraEditors.SimpleButton bt总览;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_ListOrSingle;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_Set;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit 查看;
    }
}