using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Mes
{
    /// <summary>
    /// 治具类
    /// </summary>
    public class FixtureItem
    {
        /// <summary>
        /// 治具上RFID的SN码
        /// </summary>
        public string SN;

        /// <summary>
        /// 治具有12个孔位，记录12个物料信息
        /// </summary>
        public List<CCDMaterialItem> CCDMaterialItems { get; private set; } = new List<CCDMaterialItem>(12);

        /// <summary>
        /// 根据物料获取孔号iHoleIndex（0至11），若该治具不携带该物料则返回-1.
        /// </summary>
        public int this[CCDMaterialItem materialItem]
        {
            get
            {
                return CCDMaterialItems?.IndexOf(materialItem) ?? -1;
            }
        }
    }

    /// <summary>
    /// 摄像头组装物料信息
    /// </summary>
    public class CCDMaterialItem
    {
        public FixtureItem fixture { get; }
    }

    /// <summary>
    /// 设备信息设置
    /// </summary>
    public class MachineSettingInfo
    {
        public Guid Guid = Guid.NewGuid();

    }

    /// <summary>
    /// 批次信息设置
    /// </summary>
    public class BatchSettingInfo
    {
        public Guid Guid = Guid.NewGuid();

    }
}
