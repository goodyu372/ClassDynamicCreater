using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection.Emit;
using System.Reflection;
using System.Threading;
using System.IO;
using System.Xml.Serialization;
using Mini_Mes;
using System.CodeDom.Compiler;
using System.CodeDom;
using System.Linq.Expressions;

/// <summary>
/// 类帮助器，可以动态对类,类成员进行控制（添加，删除）,目前只支持属性控制。
/// 注意，属性以外的其它成员会被清空，功能还有待完善，使其不影响其它成员。
/// </summary>
public class ClassDynamicCreater
{
    public const string TagName = " by ClassDynamicCreater";
    public const string FileOutPutsDirectory = @".\ClassCreated\";

    /// <summary>
    /// 生成.cs文件
    /// </summary>
    /// <param name="className"></param>
    /// <param name="lcpi"></param>
    public static void BuildClass(string className, List<CustPropertyInfo> lcpi)
    {
        //准备一个代码编译器单元
        CodeCompileUnit unit = new CodeCompileUnit();

        //设置命名空间（这个是指要生成的类的空间）
        CodeNamespace myNamespace = new CodeNamespace();  //"MyNameSpace"

        //导入必要的命名空间引用
        myNamespace.Imports.Add(new CodeNamespaceImport("System"));

        //Code:代码体
        CodeTypeDeclaration myClass = new CodeTypeDeclaration(className);

        //指定为类
        myClass.IsClass = true;

        //设置类的访问类型
        myClass.TypeAttributes = TypeAttributes.Public; 

        //把这个类放在这个命名空间下
        myNamespace.Types.Add(myClass);

        //把该命名空间加入到编译器单元的命名空间集合中
        unit.Namespaces.Add(myNamespace);

        foreach (var item in lcpi)
        {
            //添加字段
            CodeMemberField field = new CodeMemberField(item.PropertyType, item.fieldName);

            //设置访问类型
            field.Attributes = MemberAttributes.Private;

            ///添加到myClass类中
            myClass.Members.Add(field);

            //添加属性
            CodeMemberProperty property = new CodeMemberProperty();

            //设置访问类型
            property.Attributes = MemberAttributes.Public | MemberAttributes.Final;

            //对象名称
            property.Name = item.PropertyName;

            //有get
            property.HasGet = true;

            //有set
            property.HasSet = true;

            //设置property的类型            
            property.Type = new CodeTypeReference(item.PropertyType);

            //添加注释
            property.Comments.Add(new CodeCommentStatement($@"this is {item.PropertyType} {item.PropertyName}"));

            //get
            property.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), field.Name)));

            //set
            property.SetStatements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), field.Name), new CodePropertySetValueReferenceExpression()));

            ///添加到Customerclass类中
            myClass.Members.Add(property);
        }

        #region 添加方法
        ////添加方法
        //CodeMemberMethod method = new CodeMemberMethod();
        ////方法名
        //method.Name = "Function";

        ////访问类型
        //method.Attributes = MemberAttributes.Public | MemberAttributes.Final;

        ////添加一个参数
        //method.Parameters.Add(new CodeParameterDeclarationExpression(typeof(int), "number"));

        ////设置返回值类型：int/不设置则为void
        //method.ReturnType = new CodeTypeReference(typeof(int));

        ////设置返回值
        //method.Statements.Add(new CodeSnippetStatement(" return number+1; "));

        /////将方法添加到myClass类中
        //myClass.Members.Add(method);
        #endregion

        //添加构造方法
        CodeConstructor constructor = new CodeConstructor();
        constructor.Attributes = MemberAttributes.Public;
        #region test
        //CodeExpression codeExpression = new CodeExpression();
        //codeExpression.UserData.Add("TEST", 1);
        //constructor.ChainedConstructorArgs.Add(codeExpression);
        #endregion

        ///将构造方法添加到myClass类中
        myClass.Members.Add(constructor);

        //添加序列化特性
        myClass.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(SerializableAttribute))));   //SerializeField

        //生成C#脚本("VisualBasic"：VB脚本)
        CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");

        CodeGeneratorOptions options = new CodeGeneratorOptions();
        //代码风格:大括号的样式{}
        options.BracingStyle = "C";

        //是否在字段、属性、方法之间添加空白行
        options.BlankLinesBetweenMembers = true;

        if (!Directory.Exists(FileOutPutsDirectory))
        {
            Directory.CreateDirectory(FileOutPutsDirectory);
        }
        //输出文件路径
        string outputFile = FileOutPutsDirectory + className + ".cs";

        //保存
        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(outputFile))
        {
            //为指定的代码文档对象模型(CodeDOM) 编译单元生成代码并将其发送到指定的文本编写器，使用指定的选项。(官方解释)
            //将自定义代码编译器(代码内容)、和代码格式写入到sw中
            provider.GenerateCodeFromCompileUnit(unit, sw, options);
        }
    }


    //这里要求必须具有无参构造函数
    public static object CreateGeneric(Type generic, Type innerType)
    {
        Type specificType = generic.MakeGenericType(new System.Type[] { innerType });
        return Activator.CreateInstance(specificType);
    }

    #region 公有方法
    /// <summary>
    /// 根据类的类型型创建类实例。
    /// </summary>
    /// <param name="t">将要创建的类型。</param>
    /// <returns>返回创建的类实例。</returns>
    public static object CreateInstance(Type t)
    {
        return Activator.CreateInstance(t);
    }

    /// <summary>
    /// 根据类的名称，属性列表创建型实例。
    /// </summary>
    /// <param name="className">将要创建的类的名称。</param>
    /// <param name="lcpi">将要创建的类的属性列表。</param>
    /// <returns>返回创建的类实例</returns>
    public static object CreateInstance(string className, List<CustPropertyInfo> lcpi)
    {
        Type t = BuildType(className);
        t = AddProperty(t, lcpi);

        //生成.cs类
        BuildClass(className, lcpi);

        return Activator.CreateInstance(t);
    }

    /// <summary>
    /// 根据属性列表创建类的实例，默认类名为DefaultClass，由于生成的类不是强类型，所以类名可以忽略。
    /// </summary>
    /// <param name="lcpi">将要创建的类的属性列表</param>
    /// <returns>返回创建的类的实例。</returns>
    public static object CreateInstance(List<CustPropertyInfo> lcpi)
    {
        return CreateInstance("MyDefaultClass", lcpi);
    }

    /// <summary>
    /// 根据类的实例设置类的属性。
    /// </summary>
    /// <param name="classInstance">将要设置的类的实例。</param>
    /// <param name="propertyName">将要设置属性名。</param>
    /// <param name="propertSetValue">将要设置属性值。</param>
    public static void SetPropertyValue(object classInstance, string propertyName, object propertSetValue)
    {
        classInstance.GetType().InvokeMember(propertyName, BindingFlags.SetProperty,
                                      null, classInstance, new object[] { Convert.ChangeType(propertSetValue, propertSetValue.GetType()) });
    }

    /// <summary>
    /// 根据类的实例获取类的属性。
    /// </summary>
    /// <param name="classInstance">将要获取的类的实例</param>
    /// <param name="propertyName">将要设置的属性名。</param>
    /// <returns>返回获取的类的属性。</returns>
    public static object GetPropertyValue(object classInstance, string propertyName)
    {
        return classInstance.GetType().InvokeMember(propertyName, BindingFlags.GetProperty,
                                                      null, classInstance, new object[] { });
    }

    /// <summary>
    /// 创建一个没有成员的类型的实例，类名为"DefaultClass"。
    /// </summary>
    /// <returns>返回创建的类型的实例。</returns>
    public static Type BuildType()
    {
        return BuildType("DefaultClass");
    }

    /// <summary>
    /// 根据类名创建一个没有成员的类型的实例。
    /// </summary>
    /// <param name="className">将要创建的类型的实例的类名。</param>
    /// <returns>返回创建的类型的实例。</returns>
    public static Type BuildType(string className)
    {
        //AppDomainSetup setup = new AppDomainSetup();
        //setup.ApplicationBase = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NewAppDomain");
        //setup.CachePath = setup.ApplicationBase;
        //setup.ShadowCopyDirectories = setup.ApplicationBase;
        //string applicationName = "NewAppDomain";
        //setup.ApplicationName = applicationName;
        //AppDomain domain = AppDomain.CreateDomain(applicationName, null, setup);

        AppDomain myDomain = Thread.GetDomain();
        AssemblyName myAsmName = new AssemblyName();
        myAsmName.Name = $@"{className}";

        //如果存在该文件就先删除
        string strDLLFileName = myAsmName.Name + ".dll";
        if (File.Exists(strDLLFileName))
        {
            File.Delete(strDLLFileName);
        }
        if (File.Exists(FileOutPutsDirectory + strDLLFileName))
        {
            File.Delete(FileOutPutsDirectory + strDLLFileName);
        }
        if (!Directory.Exists(FileOutPutsDirectory))
        {
            Directory.CreateDirectory(FileOutPutsDirectory);
        }

        //创建一个永久程序集，设置为AssemblyBuilderAccess.RunAndSave。
        AssemblyBuilder myAsmBuilder = myDomain.DefineDynamicAssembly(myAsmName,
                                                        AssemblyBuilderAccess.RunAndSave);

        //创建一个永久单模程序块。
        ModuleBuilder myModBuilder =
            myAsmBuilder.DefineDynamicModule(myAsmName.Name, strDLLFileName);
        //创建TypeBuilder。
        TypeBuilder myTypeBuilder = myModBuilder.DefineType(className,
                                                        TypeAttributes.Public);

        //创建类型。
        Type retval = myTypeBuilder.CreateType(); 

        //保存程序集，以便可以被Ildasm.exe解析，或被测试程序引用。
        myAsmBuilder.Save(strDLLFileName);

        //dll剪切到输出文件夹
        File.Move(strDLLFileName, FileOutPutsDirectory + strDLLFileName);

        return retval;
    }

    /// <summary>
    /// 添加属性到类型的实例，注意：该操作会将其它成员清除掉，其功能有待完善。
    /// </summary>
    /// <param name="classType">指定类型的实例。</param>
    /// <param name="lcpi">表示属性的一个列表。</param>
    /// <returns>返回处理过的类型的实例。</returns>
    public static Type AddProperty(Type classType, List<CustPropertyInfo> lcpi)
    {
        //合并先前的属性，以便一起在下一步进行处理。
        MergeProperty(classType, lcpi);
        //把属性加入到Type。
        return AddPropertyToType(classType, lcpi);
    }

    /// <summary>
    /// 添加属性到类型的实例，注意：该操作会将其它成员清除掉，其功能有待完善。
    /// </summary>
    /// <param name="classType">指定类型的实例。</param>
    /// <param name="cpi">表示一个属性。</param>
    /// <returns>返回处理过的类型的实例。</returns>
    public static Type AddProperty(Type classType, CustPropertyInfo cpi)
    {
        List<CustPropertyInfo> lcpi = new List<CustPropertyInfo>();
        lcpi.Add(cpi);
        //合并先前的属性，以便一起在下一步进行处理。
        MergeProperty(classType, lcpi);
        //把属性加入到Type。
        return AddPropertyToType(classType, lcpi);
    }

    /// <summary>
    /// 从类型的实例中移除属性，注意：该操作会将其它成员清除掉，其功能有待完善。
    /// </summary>
    /// <param name="classType">指定类型的实例。</param>
    /// <param name="cpi">要移除的属性。</param>
    /// <returns>返回处理过的类型的实例。</returns>
    public static Type DeleteProperty(Type classType, string propertyName)
    {
        List<string> ls = new List<string>();
        ls.Add(propertyName);

        //合并先前的属性，以便一起在下一步进行处理。
        List<CustPropertyInfo> lcpi = SeparateProperty(classType, ls);
        //把属性加入到Type。
        return AddPropertyToType(classType, lcpi);
    }

    /// <summary>
    /// 从类型的实例中移除属性，注意：该操作会将其它成员清除掉，其功能有待完善。
    /// </summary>
    /// <param name="classType">指定类型的实例。</param>
    /// <param name="lcpi">要移除的属性列表。</param>
    /// <returns>返回处理过的类型的实例。</returns>
    public static Type DeleteProperty(Type classType, List<string> ls)
    {
        //合并先前的属性，以便一起在下一步进行处理。
        List<CustPropertyInfo> lcpi = SeparateProperty(classType, ls);
        //把属性加入到Type。
        return AddPropertyToType(classType, lcpi);
    }
    #endregion

    #region 私有方法
    /// <summary>
    /// 把类型的实例t和lcpi参数里的属性进行合并。
    /// </summary>
    /// <param name="t">实例t</param>
    /// <param name="lcpi">里面包含属性列表的信息。</param>
    private static void MergeProperty(Type t, List<CustPropertyInfo> lcpi)
    {
        CustPropertyInfo cpi;
        foreach (PropertyInfo pi in t.GetProperties())
        {
            cpi = new CustPropertyInfo(pi.PropertyType.FullName, pi.Name);
            lcpi.Add(cpi);
        }
    }

    /// <summary>
    /// 从类型的实例t的属性移除属性列表lcpi，返回的新属性列表在lcpi中。
    /// </summary>
    /// <param name="t">类型的实例t。</param>
    /// <param name="lcpi">要移除的属性列表。</param>
    private static List<CustPropertyInfo> SeparateProperty(Type t, List<string> ls)
    {
        List<CustPropertyInfo> ret = new List<CustPropertyInfo>();
        CustPropertyInfo cpi;
        foreach (PropertyInfo pi in t.GetProperties())
        {
            foreach (string s in ls)
            {
                if (pi.Name != s)
                {
                    cpi = new CustPropertyInfo(pi.PropertyType.FullName, pi.Name);
                    ret.Add(cpi);
                }
            }
        }

        return ret;
    }

    /// <summary>
    /// 把lcpi参数里的属性加入到myTypeBuilder中。注意：该操作会将其它成员清除掉，其功能有待完善。
    /// </summary>
    /// <param name="myTypeBuilder">类型构造器的实例。</param>
    /// <param name="lcpi">里面包含属性列表的信息。</param>
    private static void AddPropertyToTypeBuilder(TypeBuilder myTypeBuilder, List<CustPropertyInfo> lcpi)
    {
        FieldBuilder customerNameBldr;
        PropertyBuilder custNamePropBldr;
        MethodBuilder custNameGetPropMthdBldr;
        MethodBuilder custNameSetPropMthdBldr;
        MethodAttributes getSetAttr;
        ILGenerator custNameGetIL;
        ILGenerator custNameSetIL;

        // 属性Set和Get方法要一个专门的属性。这里设置为Public。
        getSetAttr =
            MethodAttributes.Public | MethodAttributes.SpecialName |
                MethodAttributes.HideBySig;

        //添加可序列化特性
        CustomAttributeBuilder SerializableAttributeBuilder = 
            new CustomAttributeBuilder(typeof(SerializableAttribute).GetConstructor(new Type[] { }), new object[] { });
        myTypeBuilder.SetCustomAttribute(SerializableAttributeBuilder);

        // 添加属性到myTypeBuilder。
        foreach (CustPropertyInfo cpi in lcpi)
        {
            Type t = cpi.PropertyType;
            if (t == null)
            {
                throw new Exception("Type不允许为null！请检查！");
            }

            //定义字段。
            string fieldName = cpi.fieldName;
            customerNameBldr = myTypeBuilder.DefineField(fieldName,
                                                             t,
                                                             FieldAttributes.Private);

            //定义属性。
            //最后一个参数为null，因为属性没有参数。
            custNamePropBldr = myTypeBuilder.DefineProperty(cpi.PropertyName,
                                                             PropertyAttributes.HasDefault,
                                                             t,
                                                             null);

            //定义Get方法。
            custNameGetPropMthdBldr =
                myTypeBuilder.DefineMethod("get_" + cpi.PropertyName,  //获取属性在IL中的Get方法名
                                           getSetAttr,
                                           t,
                                           Type.EmptyTypes);

            custNameGetIL = custNameGetPropMthdBldr.GetILGenerator();

            custNameGetIL.Emit(OpCodes.Ldarg_0);
            custNameGetIL.Emit(OpCodes.Ldfld, customerNameBldr);
            custNameGetIL.Emit(OpCodes.Ret);

            //定义Set方法。
            custNameSetPropMthdBldr =
                myTypeBuilder.DefineMethod("set_" + cpi.PropertyName,  //获取属性在IL中的Set方法名
                                           getSetAttr,
                                           null,
                                           new Type[] { t });

            custNameSetIL = custNameSetPropMthdBldr.GetILGenerator();

            custNameSetIL.Emit(OpCodes.Ldarg_0);
            custNameSetIL.Emit(OpCodes.Ldarg_1);
            custNameSetIL.Emit(OpCodes.Stfld, customerNameBldr);
            custNameSetIL.Emit(OpCodes.Ret);

            //把创建的两个方法(Get,Set)加入到PropertyBuilder中。
            custNamePropBldr.SetGetMethod(custNameGetPropMthdBldr);
            custNamePropBldr.SetSetMethod(custNameSetPropMthdBldr);
        }
    }

    /// <summary>
    /// 把属性加入到类型的实例。
    /// </summary>
    /// <param name="classType">类型的实例。</param>
    /// <param name="lcpi">要加入的属性列表。</param>
    /// <returns>返回处理过的类型的实例。</returns>
    public static Type AddPropertyToType(Type classType, List<CustPropertyInfo> lcpi)
    {
        AppDomain myDomain = Thread.GetDomain();
        AssemblyName myAsmName = new AssemblyName();
        myAsmName.Name = $@"{classType.FullName}";

        //如果存在该文件就先删除
        string strDLLFileName = myAsmName.Name + ".dll";
        if (File.Exists(strDLLFileName))
        {
            File.Delete(strDLLFileName);
        }
        if (File.Exists(FileOutPutsDirectory + strDLLFileName))
        {
            File.Delete(FileOutPutsDirectory + strDLLFileName);
        }

        //创建一个永久程序集，设置为AssemblyBuilderAccess.RunAndSave。
        AssemblyBuilder myAsmBuilder = myDomain.DefineDynamicAssembly(myAsmName,
                                                        AssemblyBuilderAccess.RunAndSave);

        //创建一个永久单模程序块。
        ModuleBuilder myModBuilder =
            myAsmBuilder.DefineDynamicModule(myAsmName.Name, strDLLFileName);
        //创建TypeBuilder。
        TypeBuilder myTypeBuilder = myModBuilder.DefineType(classType.FullName,
                                                        TypeAttributes.Public);

        //把lcpi中定义的属性加入到TypeBuilder。将清空其它的成员。其功能有待扩展，使其不影响其它成员。
        AddPropertyToTypeBuilder(myTypeBuilder, lcpi);

        //创建类型。
        Type retval = myTypeBuilder.CreateType();

        //保存程序集，以便可以被Ildasm.exe解析，或被测试程序引用。
        myAsmBuilder.Save(strDLLFileName);

        //dll剪切到输出文件夹
        File.Move(strDLLFileName, FileOutPutsDirectory + strDLLFileName);

        return retval;
    }
    #endregion
}

#region 辅助类

[Serializable]
public enum SystemDataType
{
    String = 0,
    Int = 1,
    Double = 2,
    DateTime,
    ListString
}

/// <summary>
/// 用户自定义的属性信息类型。
/// </summary>
[Serializable]
public class CustPropertyInfo
{
    private string propertyName;
    private Type propertyType;

    /// <summary>
    /// 空构造。
    /// </summary>
    public CustPropertyInfo()
    {
        this.propertyType = typeof(string);
        this.propertyName = "mark备注";
    }

    public CustPropertyInfo(SystemDataType systemType, string propertyName)
    {
        Type type = null;
        switch (systemType)
        {
            case SystemDataType.String:
                type = typeof(String);
                break;
            case SystemDataType.Int:
                type = typeof(int);
                break;
            case SystemDataType.Double:
                type = typeof(Double);
                break;
            case SystemDataType.DateTime:
                type = typeof(DateTime);
                break;
            case SystemDataType.ListString:
                type = typeof(List<string>);
                break;
            default:
                break;
        }
        this.propertyType = type;
        this.propertyName = propertyName.ToUpper();
    }

    /// <summary>
    /// 根据属性类型名称，属性名称构造实例。
    /// </summary>
    /// <param name="propertyTypeName">属性类型名称。</param>
    /// <param name="propertyName">属性名称。</param>
    public CustPropertyInfo(string propertyTypeName, string propertyName)
    {
        this.propertyType = Type.GetType(propertyTypeName);
        this.propertyName = propertyName.ToUpper();
    }

    /// <summary>
    /// 根据属性类型，属性名称构造实例。
    /// </summary>
    /// <param name="propertyType"></param>
    /// <param name="propertyName"></param>
    public CustPropertyInfo(Type propertyType, string propertyName)
    {
        this.propertyType = propertyType;
        this.propertyName = propertyName.ToUpper();
    }

    /// <summary>
    /// 根据当前泛型类型定义的类型参数组成的类型数组的元素，属性名称构造实例。如(List<>,type)=>List<type>
    /// </summary>
    /// <param name="generic"></param>
    /// <param name="innerType"></param>
    /// <param name="propertyName"></param>
    public CustPropertyInfo(Type generic, Type innerType, string propertyName)
    {
        Type specificType = generic.MakeGenericType(new System.Type[] { innerType });
        this.propertyType = specificType;
        this.propertyName = propertyName.ToUpper();
    }

    /// <summary>
    /// 获取或设置属性类型。
    /// </summary>
    public Type PropertyType
    {
        get { return propertyType; }
        set { propertyType = value; }
    }

    /// <summary>
    /// 获取字段名，会把属性名称的第一个字符小写、其他大写
    /// </summary>
    public string fieldName => PropertyName.Substring(0, 1).ToLower() + PropertyName.Substring(1).ToUpper();

    /// <summary>
    /// 获取或设置属性名称，勿输入全中文或全数字，最好以字符开头。
    /// </summary>
    public string PropertyName
    {
        get { return propertyName; }
        set { propertyName = value; }
    }
}

#endregion


public class ClassDynamicCreaterDemo
{
    public static void Main2()
    {
        #region 演示一：动态生成类。
        //生成一个类t。
        Type t = ClassDynamicCreater.BuildType("MyClass");
        #endregion
        
        #region 演示二：动态添加属性到类。
        //先定义两个属性。
        List<CustPropertyInfo> lcpi = new List<CustPropertyInfo>();
        CustPropertyInfo cpi;

        cpi = new CustPropertyInfo("System.String", "S1");
        lcpi.Add(cpi);
        cpi = new CustPropertyInfo("System.String", "S2");
        lcpi.Add(cpi);

        //再加入上面定义的两个属性到我们生成的类t。
        t = ClassDynamicCreater.AddProperty(t, lcpi);

        //把它显示出来。
        DispProperty(t);

        //再定义三个属性。
        lcpi.Clear();
        cpi = new CustPropertyInfo(SystemDataType.Int, "I1");  //"System.Int32"
        lcpi.Add(cpi);
        cpi = new CustPropertyInfo("System.Int32", "I2");
        lcpi.Add(cpi);
        cpi = new CustPropertyInfo("System.Object", "O1");
        lcpi.Add(cpi);

        //再加入上面定义的三个属性到我们生成的类t。
        t = ClassDynamicCreater.AddProperty(t, lcpi);

        //再把它显示出来，看看有没有增加到5个属性。
        DispProperty(t);
        #endregion

        #region 演示三：动态从类里删除属性。
        //把'S1'属性从类t中删除。
        t = ClassDynamicCreater.DeleteProperty(t, "S1");
        //显示出来，可以看到'S1'不见了。
        DispProperty(t);

        #endregion

        #region 演示四：动态获取和设置属性值。
        //先生成一个类t的实例o。
        object o = ClassDynamicCreater.CreateInstance(t);

        //给S2,I2,O1属性赋值。
        ClassDynamicCreater.SetPropertyValue(o, "S2", "abcd");
        ClassDynamicCreater.SetPropertyValue(o, "I2", 1234);
        ClassDynamicCreater.SetPropertyValue(o, "O1", 7.5);

        //再把S2,I2,01的属性值显示出来。
        Console.WriteLine("S2 = {0}", ClassDynamicCreater.GetPropertyValue(o, "S2"));
        Console.WriteLine("I2 = {0}", ClassDynamicCreater.GetPropertyValue(o, "I2"));
        Console.WriteLine("O1 = {0}", ClassDynamicCreater.GetPropertyValue(o, "O1"));
        #endregion

        #region 演示五：类嵌套设置属性值

        Type t2 = ClassDynamicCreater.BuildType("MyClass2"); 
        List<CustPropertyInfo> lcpi2 = new List<CustPropertyInfo>();
        CustPropertyInfo cpi2;

        //cpi2 = new CustPropertyInfo(t.FullName, "mc");  //t2类中加入一个t属性
        cpi2 = new CustPropertyInfo(t, "MC");  //t2类中加入一个t属性
        lcpi2.Add(cpi2);
        TypeInfo tInfo = t.GetTypeInfo();

        //t2类中加入一个t类的列表属性
        //cpi2 = new CustPropertyInfo(typeof(List<>), "MCL");  
        cpi2 = new CustPropertyInfo(typeof(List<>),t, "MCL");
        lcpi2.Add(cpi2);
        
        t2 = ClassDynamicCreater.AddProperty(t2, lcpi2);

        DispProperty(t2);

        //先生成一个类t2的实例o。
        object o2 = ClassDynamicCreater.CreateInstance(t2);

        //给MC属性赋值。
        ClassDynamicCreater.SetPropertyValue(o2, "MC", o);   //o是类t的一个实例
        //给MCL属性赋值。
        dynamic Dynamic = ClassDynamicCreater.CreateGeneric(typeof(List<>), t);
        //object o = ClassDynamicCreater.CreateInstance(t);
        dynamic o1 = o;
        Dynamic.Add(o1);
        ClassDynamicCreater.SetPropertyValue(o2, "MCL", Dynamic);

        //再把mc属性值显示出来。
        Console.WriteLine("MC = {0}", ClassDynamicCreater.GetPropertyValue(o2, "MC"));

        #endregion

        object genericList = ClassDynamicCreater.CreateGeneric(typeof(List<>), typeof(StudentTest));
        
        dynamic dynamic = genericList;
        dynamic.Add(new StudentTest());

        Console.Read();
    }

    public static void DispProperty(Type t)
    {
        Console.WriteLine("ClassName '{0}'", t.Name);
        foreach (PropertyInfo pInfo in t.GetProperties())
        {
            Console.WriteLine("Has Property '{0}'", pInfo.ToString());
        }
        Console.WriteLine("");
    }
}

public class StudentTest 
{
    public string Name { get; set; } = "name1";
}