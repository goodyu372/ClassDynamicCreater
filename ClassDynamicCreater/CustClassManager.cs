using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using Faster.Core;

namespace Mini_Mes
{
    [Serializable]
    public class CustClassManager
    {
        private static CustClassManager instance;
        public static CustClassManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CustClassManager();
                }
                return instance;
            }
        }
        public CustClassManager()
        {

        }

        //*	匹配前面的子表达式零次或多次。要匹配* 字符，请使用 \*。
        //+	匹配前面的子表达式一次或多次。要匹配 + 字符，请使用 \+。
        //.	匹配除换行符 \n 之外的任何单字符。要匹配. ，请使用 \. 。

        [NonSerialized]
        public static readonly Regex regex字母开头 = new Regex("^[a-zA-Z]+.*$");

        public BindingList<CustClassInfo> AllClasses { get; set; } = new BindingList<CustClassInfo>();  //所有自定义类的汇总

        [field: NonSerialized]
        public BindingList<Type> AllAvailableTypes =>
            new BindingList<Type>
            (
                new List<Type>() { typeof(string), typeof(int), typeof(double),typeof(DateTime),typeof(List<string>) }
            .Union(AllClasses?.Where(p => p.ClassType != null).Select(p => p.ClassType)
            .Union(AllClasses?.Where(p => p.ClassType != null).Select(p=> typeof(List<>).MakeGenericType(p.ClassType)))
                ).ToList()
                );

        [field: NonSerialized]
        public Dictionary<string, CustClassInfo> AllClassesDict => AllClasses?.ToDictionary(p => p.ClassName);

        public bool Save() 
        {
            CommonSerializer.SaveObjAsBinaryFile(this, $@".\CustClassManager.xml", out bool bSaveOK, out Exception ex);
            return bSaveOK;
        }

        public bool Load() 
        {
            instance = CommonSerializer.LoadObjFormBinaryFile<CustClassManager>($@".\CustClassManager.xml", out bool bLoadOK, out Exception ex);
            return bLoadOK;
        }
    }

    /// <summary>
    /// 用户自定义类
    /// </summary>
    [Serializable]
    public class CustClassInfo 
    {
        /// <summary>
        /// 用户自定义类名称，应唯一不能重复
        /// </summary>
        public string ClassName { get; set; } = "Default";

        public Type ClassType { get; private set; }

        /// <summary>
        /// 生成设置为列表（true）或单个对象（false） 
        /// </summary>
        public bool ListOrSingle { get; set; }

        public BindingList<CustPropertyInfo> PropertyInfos { get; set; } = new BindingList<CustPropertyInfo>() { new CustPropertyInfo() };

        [field: NonSerialized]
        [XmlIgnore]
        public Dictionary<string, CustPropertyInfo> PropertyInfosDict => PropertyInfos?.ToDictionary(p => p.PropertyName);  //属性名称应唯一不能重复

        public object Instance { get; private set; }

        public object DataSourceTag { get; set; }

        public void GetTypeFormInstance(object obj) 
        {
            this.Instance = obj;
            this.ClassType = obj.GetType();
        }
    }
}
