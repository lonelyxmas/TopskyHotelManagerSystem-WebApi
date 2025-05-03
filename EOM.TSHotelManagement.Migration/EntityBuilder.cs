using EOM.TSHotelManagement.Common.Core;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EOM.TSHotelManagement.Migration
{
    public class EntityBuilder
    {
        private readonly Type[] entityTypes =
        {
            typeof(ApplicationVersion),
            typeof(Administrator),
            typeof(AdministratorType),
            typeof(AppointmentNotice),
            typeof(AppointmentNoticeType),
            typeof(Asset),
            typeof(Customer),
            typeof(CustoType),
            typeof(CardCode),
            typeof(Department),
            typeof(Employee),
            typeof(EmployeeCheck),
            typeof(EmployeeHistory),
            typeof(EmployeePhoto),
            typeof(EmployeeRewardPunishment),
            typeof(EnergyManagement),
            typeof(Education),
            typeof(Menu),
            typeof(Nation),
            typeof(NavBar),
            typeof(OperationLog),
            typeof(Position),
            typeof(PromotionContent),
            typeof(PassportType),
            typeof(Room),
            typeof(RoomType),
            typeof(Reser),
            typeof(RewardPunishmentType),
            typeof(Role),
            typeof(RolePermission),
            typeof(SellThing),
            typeof(Spend),
            typeof(SupervisionStatistics),
            typeof(SystemInformation),
            typeof(UserRole),
            typeof(VipLevelRule),
        };

        private readonly List<object> entityDatas = new()
        {
            new AdministratorType
            {
                TypeId = "Admin",
                TypeName = "超级管理员",
                IsDelete = 0,
                DataInsUsr = "System",
                DataInsDate = DateTime.Now,
            },
            new Administrator
            {
                Number = "1263785187301658678",
                Account = "admin",
                Password = "clUKFMeIUWp6YflZweR0Cw==·#c0fbb?;*$>#;^b%$?>#%%<?%%%aeae^",
                Name = "Administrator",
                Type = "Admin",
                IsSuperAdmin = 1,
                IsDelete = 0,
                DataInsUsr = "System",
                DataInsDate = DateTime.Now,
            },
            new Menu // 1
            {
                Key = "home",
                Title = "首页",
                Path = "/home",
                Parent = null,
                Icon = "HomeOutlined",
                IsDelete = 0,
                DataInsUsr = "System",
                DataInsDate = DateTime.Now,
            },
            new Menu // 2
            {
                Key = "basic",
                Title = "基础信息管理",
                Path = "/",
                Parent = null,         
                Icon = "AppstoreOutlined",
                IsDelete = 0,
                DataInsUsr = "System",
                DataInsDate = DateTime.Now,
            },
            new Menu // 3
            {
                Key = "position",
                Title = "职位管理",
                Path = "/position",
                Parent = 2,            
                Icon = "PartitionOutlined",
                IsDelete = 0,
                DataInsUsr = "System",
                DataInsDate = DateTime.Now,
            },
            new Menu // 4
            {
                Key = "nation",
                Title = "民族管理",
                Path = "/nation",
                Parent = 2,            
                Icon = "FlagOutlined",
                IsDelete = 0,
                DataInsUsr = "System",
                DataInsDate = DateTime.Now,
            },
            new Menu // 5
            {
                Key = "qualification",
                Title = "学历管理",
                Path = "/qualification",
                Parent = 2,           
                Icon = "ReadOutlined",
                IsDelete = 0,
                DataInsUsr = "System",
                DataInsDate = DateTime.Now,
            },
            new Menu // 6
            {
                Key = "department",
                Title = "部门管理",
                Path = "/department",
                Parent = 2,            
                Icon = "ApartmentOutlined",
                IsDelete = 0,
                DataInsUsr = "System",
                DataInsDate = DateTime.Now,
            },
            new Menu // 7
            {
                Key = "noticetype",
                Title = "公告类型管理",
                Path = "/noticetype",
                Parent = 2,
                Icon = "ApartmentOutlined",
                IsDelete = 0,
                DataInsUsr = "System",
                DataInsDate = DateTime.Now,
            },
            new Menu // 8
            {
                Key = "passport",
                Title = "证件类型管理",
                Path = "/passport",
                Parent = 2,
                Icon = "IdcardOutlined",
                IsDelete = 0,
                DataInsUsr = "System",
                DataInsDate = DateTime.Now,
            },
            new Menu // 9
            {
                Key = "finance",
                Title = "财务信息管理",
                Path = "/",
                Parent = null,         
                Icon = "WalletOutlined",
                IsDelete = 0,
                DataInsUsr = "System",
                DataInsDate = DateTime.Now,
            },
            new Menu // 10
            {
                Key = "internalfinance",
                Title = "内部资产管理",
                Path = "/internalfinance",
                Parent = 9,            
                Icon = "DollarOutlined",
                IsDelete = 0,
                DataInsUsr = "System",
                DataInsDate = DateTime.Now,
            },
            new Menu // 11
            {
                Key = "hydroelectricity",
                Title = "水电信息管理",
                Path = "/",
                Parent = null,         
                Icon = "FireOutlined",
                IsDelete = 0,
                DataInsUsr = "System",
                DataInsDate = DateTime.Now,
            },
            new Menu // 12
            {
                Key = "hydroelectricinformation",
                Title = "水电信息管理",
                Path = "/hydroelectricinformation",
                Parent = 11,
                Icon = "FireOutlined",
                IsDelete = 0,
                DataInsUsr = "System",
                DataInsDate = DateTime.Now,
            },
            new Menu // 13
            {
                Key = "supervisionmanagement",
                Title = "监管统计管理",
                Path = "/",
                Parent = null,         
                Icon = "AuditOutlined",
                IsDelete = 0,
                DataInsUsr = "System",
                DataInsDate = DateTime.Now,
            },
            new Menu // 14
            {
                Key = "supervisioninfo",
                Title = "监管情况",
                Path = "/supervisioninfo",
                Parent = 13,           
                Icon = "AuditOutlined",
                IsDelete = 0,
                DataInsUsr = "System",
                DataInsDate = DateTime.Now,
            },
            new Menu // 15
            {
                Key = "roominformation",
                Title = "客房信息管理",
                Path = "/",
                Parent = null,         
                Icon = "HolderOutlined",
                IsDelete = 0,
                DataInsUsr = "System",
                DataInsDate = DateTime.Now,
            },
            new Menu // 16
            {
                Key = "resermanagement",
                Title = "预约管理",
                Path = "/resermanagement",
                Parent = 15,
                Icon = "BellOutlined",
                IsDelete = 0,
                DataInsUsr = "System",
                DataInsDate = DateTime.Now,
            },
            new Menu // 17
            {
                Key = "roommap",
                Title = "房态图一览",
                Path = "/roommap",
                Parent = 15,           
                Icon = "TableOutlined",
                IsDelete = 0,
                DataInsUsr = "System",
                DataInsDate = DateTime.Now,
            },
            new Menu // 18
            {
                Key = "roommanagement",
                Title = "客房管理",
                Path = "/roommanagement",
                Parent = 15,           
                Icon = "HomeOutlined",
                IsDelete = 0,
                DataInsUsr = "System",
                DataInsDate = DateTime.Now,
            },
            new Menu // 19
            {
                Key = "roomconfig",
                Title = "客房配置",
                Path = "/roomconfig",
                Parent = 15,           
                Icon = "ToolOutlined",
                IsDelete = 0,
                DataInsUsr = "System",
                DataInsDate = DateTime.Now,
            },
            new Menu // 20
            {
                Key = "customermanagement",
                Title = "酒店客户管理",
                Path = "/",
                Parent = null,         
                Icon = "DesktopOutlined",
                IsDelete = 0,
                DataInsUsr = "System",
                DataInsDate = DateTime.Now,
            },
            new Menu // 21
            {
                Key = "viplevel",
                Title = "会员等级规则",
                Path = "/viplevel",
                Parent = 20,           
                Icon = "CrownOutlined",
                IsDelete = 0,
                DataInsUsr = "System",
                DataInsDate = DateTime.Now,
            },
            new Menu // 22
            {
                Key = "customer",
                Title = "客户信息管理",
                Path = "/customer",
                Parent = 20,           
                Icon = "ContactsOutlined",
                IsDelete = 0,
                DataInsUsr = "System",
                DataInsDate = DateTime.Now,
            },
            new Menu // 23
            {
                Key = "customerspend",
                Title = "客户消费账单",
                Path = "/customerspend",
                Parent = 20,           
                Icon = "PayCircleOutlined",
                IsDelete = 0,
                DataInsUsr = "System",
                DataInsDate = DateTime.Now,
            },
            new Menu // 24
            {
                Key = "customertype",
                Title = "客户类型管理",
                Path = "/customertype",
                Parent = 20,
                Icon = "TagOutlined",
                IsDelete = 0,
                DataInsUsr = "System",
                DataInsDate = DateTime.Now,
            },
            new Menu // 25
            {
                Key = "humanresourcemanagement",
                Title = "酒店人事管理",
                Path = "/",
                Parent = null,         
                Icon = "TeamOutlined",
                IsDelete = 0,
                DataInsUsr = "System",
                DataInsDate = DateTime.Now,
            },
            new Menu // 26
            {
                Key = "staffmanagement",
                Title = "员工管理",
                Path = "/staffmanagement",
                Parent = 25,           
                Icon = "TeamOutlined",
                IsDelete = 0,
                DataInsUsr = "System",
                DataInsDate = DateTime.Now,
            },
            new Menu // 27
            {
                Key = "materialmanagement",
                Title = "酒店物资管理",
                Path = "/",
                Parent = null,         
                Icon = "ProjectOutlined",
                IsDelete = 0,
                DataInsUsr = "System",
                DataInsDate = DateTime.Now,
            },
            new Menu // 28
            {
                Key = "goodsmanagement",
                Title = "商品管理",
                Path = "/goodsmanagement",
                Parent = 27,           
                Icon = "ShopOutlined",
                IsDelete = 0,
                DataInsUsr = "System",
                DataInsDate = DateTime.Now,
            },
            new Menu // 29
            {
                Key = "operationmanagement",
                Title = "行为操作管理",
                Path = "/",
                Parent = null,         
                Icon = "CoffeeOutlined",
                IsDelete = 0,
                DataInsUsr = "System",
                DataInsDate = DateTime.Now,
            },
            new Menu // 30
            {
                Key = "operationlog",
                Title = "操作日志",
                Path = "/operationlog",
                Parent = 29,           
                Icon = "SolutionOutlined",
                IsDelete = 0,
                DataInsUsr = "System",
                DataInsDate = DateTime.Now,
            },
            new Menu // 31
            {
                Key = "systemmanagement",
                Title = "系统管理",
                Path = "/",
                Parent = null,         
                Icon = "ToolOutlined",
                IsDelete = 0,
                DataInsUsr = "System",
                DataInsDate = DateTime.Now,
            },
            new Menu // 32
            {
                Key = "administratormanagement",
                Title = "管理员管理",
                Path = "/administratormanagement",
                Parent = 31,           
                Icon = "KeyOutlined",
                IsDelete = 0,
                DataInsUsr = "System",
                DataInsDate = DateTime.Now,
            },
            new Menu // 33
            {
                Key = "systemmodule",
                Title = "系统模块",
                Path = "/systemmodule",
                Parent = 31,           
                Icon = "DatabaseOutlined",
                IsDelete = 0,
                DataInsUsr = "System",
                DataInsDate = DateTime.Now,
            },
            new Menu // 34
            {
                Key = "menumanagement",
                Title = "菜单管理",
                Path = "/menumanagement",
                Parent = 31,
                Icon = "MenuOutlined",
                IsDelete = 0,
                DataInsUsr = "System",
                DataInsDate = DateTime.Now,
            },
            new Menu // 35
            {
                Key = "rolemanagement",
                Title = "角色管理",
                Path = "/rolemanagement",
                Parent = 31,          
                Icon = "SmileOutlined",
                IsDelete = 0,
                DataInsUsr = "System",
                DataInsDate = DateTime.Now,
            },
            new Menu // 36
            {
                Key = "admintypemanagement",
                Title = "管理员类型管理",
                Path = "/admintypemanagement",
                Parent = 31,          
                Icon = "TagOutlined",
                IsDelete = 0,
                DataInsUsr = "System",
                DataInsDate = DateTime.Now,
            },
            new NavBar
            {
                NavigationBarName = "客房管理",
                NavigationBarOrder = 1,
                NavigationBarImage = null,
                NavigationBarEvent = "RoomManager_Event",
                IsDelete = 0,
                MarginLeft = 0,
                DataInsUsr = "System",
                DataInsDate = DateTime.Now
            },
            new NavBar
            {
                NavigationBarName = "客户管理",
                NavigationBarOrder = 2,
                NavigationBarImage = null,
                NavigationBarEvent = "CustomerManager_Event",
                IsDelete = 0,
                MarginLeft = 120,
                DataInsUsr = "System",
                DataInsDate = DateTime.Now
            },
            new NavBar
            {
                NavigationBarName = "商品消费",
                NavigationBarOrder = 3,
                NavigationBarImage = null,
                NavigationBarEvent = "SellManager_Event",
                IsDelete = 0,
                MarginLeft = 120,
                DataInsUsr = "System",
                DataInsDate = DateTime.Now
            },
            new SystemInformation
            {
                UrlNumber = 1,
                UrlAddress = "https://gitee.com/java-and-net/TopskyHotelManagerSystem/releases",
            },
            new PromotionContent
            {
                PromotionContentMessage = "欢迎使用酒店管理系统！",
                IsDelete = 0,
                DataInsUsr = "System",
                DataInsDate = DateTime.Now,
            },
            new PromotionContent
            {
                PromotionContentMessage = "本酒店即日起与闪修平台联合推出“多修多折”活动，详情请咨询前台！",
                IsDelete = 0,
                DataInsUsr = "System",
                DataInsDate = DateTime.Now,
            },
            new PromotionContent
            {
                PromotionContentMessage = "本酒店即日起与神之食餐厅联合推出“吃多折多”活动，详情请咨询前台！",
                IsDelete = 0,
                DataInsUsr = "System",
                DataInsDate = DateTime.Now,
            },
            new PromotionContent
            {
                PromotionContentMessage = "本酒店即日起与Second网吧联合推出“免费体验酒店式网吧”活动，详情请咨询前台！",
                IsDelete = 0,
                DataInsUsr = "System",
                DataInsDate = DateTime.Now,
            }

        };

        public Type[] EntityTypes => entityTypes;

        public List<object> GetEntityDatas() => entityDatas;
    }
}
