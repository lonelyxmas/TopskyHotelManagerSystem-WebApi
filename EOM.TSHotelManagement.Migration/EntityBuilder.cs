using EOM.TSHotelManagement.Domain;

namespace EOM.TSHotelManagement.Migration
{
    public class EntityBuilder
    {
        public EntityBuilder(string? initialAdminEncryptedPassword = null)
        {
            if (string.IsNullOrWhiteSpace(initialAdminEncryptedPassword))
            {
                return;
            }

            var admin = entityDatas
                .OfType<Administrator>()
                .FirstOrDefault(a => string.Equals(a.Account, "admin", StringComparison.OrdinalIgnoreCase));

            if (admin != null)
            {
                admin.Password = initialAdminEncryptedPassword;
            }
        }

        private readonly Type[] entityTypes =
        {
            typeof(Administrator),
            typeof(AdministratorType),
            typeof(AppointmentNotice),
            typeof(AppointmentNoticeType),
            typeof(Asset),
            typeof(Customer),
            typeof(CustomerAccount),
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
            typeof(RequestLog),
            typeof(News),
            typeof(Permission)
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
                Password = "admin",
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
                Key = "menumanagement",
                Title = "菜单管理",
                Path = "/menumanagement",
                Parent = 31,
                Icon = "MenuOutlined",
                IsDelete = 0,
                DataInsUsr = "System",
                DataInsDate = DateTime.Now,
            },
            new Menu // 34
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
            new Menu // 35
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
            new Menu // 36
            {
                Key = "dashboard",
                Title = "仪表盘",
                Path = "/dashboard",
                Parent = 1,
                Icon = "DashboardOutlined",
                IsDelete = 0,
                DataInsUsr = "System",
                DataInsDate = DateTime.Now,
            },
            new Menu // 37
            {
                Key = "promotioncontent",
                Title = "宣传联动内容",
                Path = "/promotioncontent",
                Parent = 2,
                Icon = "DashboardOutlined",
                IsDelete = 0,
                DataInsUsr = "System",
                DataInsDate = DateTime.Now,
            },
            new Menu // 38
            {
                Key = "requestlog",
                Title = "请求日志",
                Path = "/requestlog",
                Parent = 29,
                Icon = "SolutionOutlined",
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
            new Department
            {
              DepartmentNumber = "D-000001",
              DepartmentName = "酒店部",
              DepartmentDescription = null,
              DepartmentCreationDate = DateOnly.FromDateTime(DateTime.Now),
              DepartmentLeader = "WK010",
              IsDelete = 0,
              DataInsUsr = "System",
              DataInsDate = DateTime.Now
            },
            new Position
            {
              PositionNumber = "P-000001",
              PositionName = "初级职员",
              IsDelete = 0,
              DataInsUsr = "System",
              DataInsDate = DateTime.Now
            },
            new Education
            {
              EducationNumber = "E-000001",
              EducationName = "本科",
              IsDelete = 0,
              DataInsUsr = "System",
              DataInsDate = DateTime.Now
            },
            new Nation
            {
              NationNumber = "N-000001",
              NationName = "汉族",
              IsDelete = 0,
              DataInsUsr = "System",
              DataInsDate = DateTime.Now
            },
            new PassportType
            {
              PassportId = 666,
              PassportName = "中国居民身份证",
              IsDelete = 0,
              DataInsUsr = "System",
              DataInsDate = DateTime.Now
            },
            new Employee
            {
              EmployeeId = "WK010",
              EmployeeName = "阿杰",
              DateOfBirth = DateOnly.FromDateTime(new DateTime(1999,7,20,0,0,0)),
              Password="oi6+T4604MqlB/SWAvrJBQ==·?bdc^^<ddb0c;0e#a?e0?d#d<$%d%^dd",
              Department = "D-000001",
              Position = "P-000001",
              EducationLevel = "E-000001",
              Address = "广东珠海",
              Ethnicity = "N-000001",
              PoliticalAffiliation = "TheMasses",
              EmailAddress = "demo@oscode.top",
              Gender = 1,
              HireDate = DateOnly.FromDateTime(new DateTime(2025,05,06,0,0,0)),
              IdCardNumber = "F9lr+YW3UpD9NSUiA3zMDD1jblLysPnRe1cwEAJslbE=·;*f?;de*$#0#;#<cac<%^*f>>d0%*#d<",
              PhoneNumber = "JyXOhUwOtSipMgKBXWhX1A==·c;0<*b^d^edb;c>daf^ff#e*?d$$acbe",
              IdCardType = 666,
              IsEnable = 1,
              IsInitialize = 1,
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
                PromotionContentNumber = "PC-000001",
                PromotionContentMessage = "欢迎使用酒店管理系统！",
                IsDelete = 0,
                DataInsUsr = "System",
                DataInsDate = DateTime.Now,
            },
            new PromotionContent
            {
                PromotionContentNumber = "PC-000002",
                PromotionContentMessage = "本酒店即日起与闪修平台联合推出“多修多折”活动，详情请咨询前台！",
                IsDelete = 0,
                DataInsUsr = "System",
                DataInsDate = DateTime.Now,
            },
            new PromotionContent
            {
                PromotionContentNumber = "PC-000003",
                PromotionContentMessage = "本酒店即日起与神之食餐厅联合推出“吃多折多”活动，详情请咨询前台！",
                IsDelete = 0,
                DataInsUsr = "System",
                DataInsDate = DateTime.Now,
            },
            new PromotionContent
            {
                PromotionContentNumber = "PC-000004",
                PromotionContentMessage = "本酒店即日起与Second网吧联合推出“免费体验酒店式网吧”活动，详情请咨询前台！",
                IsDelete = 0,
                DataInsUsr = "System",
                DataInsDate = DateTime.Now,
            }

            ,
            // ===== Permission seeds for button-level authorization (MenuKey-scoped) =====
            // Basic (基础信息管理)
            new Permission { PermissionNumber = "position.view", PermissionName = "职位-查看", Module = "basic", Description = "职位管理-查看", MenuKey = "position", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "position.create", PermissionName = "职位-新增", Module = "basic", Description = "职位管理-新增", MenuKey = "position", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "position.update", PermissionName = "职位-编辑", Module = "basic", Description = "职位管理-编辑", MenuKey = "position", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "position.delete", PermissionName = "职位-删除", Module = "basic", Description = "职位管理-删除", MenuKey = "position", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },

            new Permission { PermissionNumber = "nation.view", PermissionName = "民族-查看", Module = "basic", Description = "民族管理-查看", MenuKey = "nation", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "nation.create", PermissionName = "民族-新增", Module = "basic", Description = "民族管理-新增", MenuKey = "nation", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "nation.update", PermissionName = "民族-编辑", Module = "basic", Description = "民族管理-编辑", MenuKey = "nation", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "nation.delete", PermissionName = "民族-删除", Module = "basic", Description = "民族管理-删除", MenuKey = "nation", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },

            new Permission { PermissionNumber = "qualification.view", PermissionName = "学历-查看", Module = "basic", Description = "学历管理-查看", MenuKey = "qualification", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "qualification.create", PermissionName = "学历-新增", Module = "basic", Description = "学历管理-新增", MenuKey = "qualification", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "qualification.update", PermissionName = "学历-编辑", Module = "basic", Description = "学历管理-编辑", MenuKey = "qualification", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "qualification.delete", PermissionName = "学历-删除", Module = "basic", Description = "学历管理-删除", MenuKey = "qualification", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },

            new Permission { PermissionNumber = "department.view", PermissionName = "部门-查看", Module = "basic", Description = "部门管理-查看", MenuKey = "department", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "department.create", PermissionName = "部门-新增", Module = "basic", Description = "部门管理-新增", MenuKey = "department", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "department.update", PermissionName = "部门-编辑", Module = "basic", Description = "部门管理-编辑", MenuKey = "department", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "department.delete", PermissionName = "部门-删除", Module = "basic", Description = "部门管理-删除", MenuKey = "department", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },

            new Permission { PermissionNumber = "noticetype.view", PermissionName = "公告类型-查看", Module = "basic", Description = "公告类型管理-查看", MenuKey = "noticetype", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "noticetype.create", PermissionName = "公告类型-新增", Module = "basic", Description = "公告类型管理-新增", MenuKey = "noticetype", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "noticetype.update", PermissionName = "公告类型-编辑", Module = "basic", Description = "公告类型管理-编辑", MenuKey = "noticetype", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "noticetype.delete", PermissionName = "公告类型-删除", Module = "basic", Description = "公告类型管理-删除", MenuKey = "noticetype", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },

            new Permission { PermissionNumber = "passport.view", PermissionName = "证件-查看", Module = "basic", Description = "证件类型管理-查看", MenuKey = "passport", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "passport.create", PermissionName = "证件-新增", Module = "basic", Description = "证件类型管理-新增", MenuKey = "passport", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "passport.update", PermissionName = "证件-编辑", Module = "basic", Description = "证件类型管理-编辑", MenuKey = "passport", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "passport.delete", PermissionName = "证件-删除", Module = "basic", Description = "证件类型管理-删除", MenuKey = "passport", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },

            new Permission { PermissionNumber = "promotioncontent.view", PermissionName = "宣传联动-查看", Module = "basic", Description = "宣传联动内容-查看", MenuKey = "promotioncontent", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "promotioncontent.create", PermissionName = "宣传联动-新增", Module = "basic", Description = "宣传联动内容-新增", MenuKey = "promotioncontent", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "promotioncontent.update", PermissionName = "宣传联动-编辑", Module = "basic", Description = "宣传联动内容-编辑", MenuKey = "promotioncontent", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "promotioncontent.delete", PermissionName = "宣传联动-删除", Module = "basic", Description = "宣传联动内容-删除", MenuKey = "promotioncontent", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },

            // Finance (财务信息管理)
            new Permission { PermissionNumber = "internalfinance.view", PermissionName = "内部资产-查看", Module = "finance", Description = "内部资产管理-查看", MenuKey = "internalfinance", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "internalfinance.create", PermissionName = "内部资产-新增", Module = "finance", Description = "内部资产管理-新增", MenuKey = "internalfinance", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "internalfinance.update", PermissionName = "内部资产-编辑", Module = "finance", Description = "内部资产管理-编辑", MenuKey = "internalfinance", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "internalfinance.delete", PermissionName = "内部资产-删除", Module = "finance", Description = "内部资产管理-删除", MenuKey = "internalfinance", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },

            // Nav Bar (导航栏管理)
            new Permission { PermissionNumber = "navbar.view", PermissionName = "导航栏-查看", Module = "client", Description = "导航栏管理-查看", MenuKey = "navbar", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "navbar.create", PermissionName = "导航栏-新增", Module = "client", Description = "导航栏管理-新增", MenuKey = "navbar", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "navbar.update", PermissionName = "导航栏-编辑", Module = "client", Description = "导航栏管理-编辑", MenuKey = "navbar", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "navbar.delete", PermissionName = "导航栏-删除", Module = "client", Description = "导航栏管理-删除", MenuKey = "navbar", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },

            // Hydroelectricity (水电信息管理)
            new Permission { PermissionNumber = "hydroelectricinformation.view", PermissionName = "水电信息-查看", Module = "hydroelectricity", Description = "水电信息管理-查看", MenuKey = "hydroelectricinformation", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "hydroelectricinformation.create", PermissionName = "水电信息-新增", Module = "hydroelectricity", Description = "水电信息管理-新增", MenuKey = "hydroelectricinformation", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "hydroelectricinformation.update", PermissionName = "水电信息-编辑", Module = "hydroelectricity", Description = "水电信息管理-编辑", MenuKey = "hydroelectricinformation", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "hydroelectricinformation.delete", PermissionName = "水电信息-删除", Module = "hydroelectricity", Description = "水电信息管理-删除", MenuKey = "hydroelectricinformation", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },

            // Supervision (监管统计管理)
            new Permission { PermissionNumber = "supervisioninfo.view", PermissionName = "监管情况-查看", Module = "supervision", Description = "监管情况-查看", MenuKey = "supervisioninfo", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "supervisioninfo.create", PermissionName = "监管情况-新增", Module = "supervision", Description = "监管情况-新增", MenuKey = "supervisioninfo", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "supervisioninfo.update", PermissionName = "监管情况-编辑", Module = "supervision", Description = "监管情况-编辑", MenuKey = "supervisioninfo", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "supervisioninfo.delete", PermissionName = "监管情况-删除", Module = "supervision", Description = "监管情况-删除", MenuKey = "supervisioninfo", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },

            // Room information (客房信息管理)
            new Permission { PermissionNumber = "resermanagement.view", PermissionName = "预约-查看", Module = "room", Description = "预约管理-查看", MenuKey = "resermanagement", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "resermanagement.create", PermissionName = "预约-新增", Module = "room", Description = "预约管理-新增", MenuKey = "resermanagement", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "resermanagement.update", PermissionName = "预约-编辑", Module = "room", Description = "预约管理-编辑", MenuKey = "resermanagement", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "resermanagement.delete", PermissionName = "预约-删除", Module = "room", Description = "预约管理-删除", MenuKey = "resermanagement", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },

            new Permission { PermissionNumber = "roommap.view", PermissionName = "房态图-查看", Module = "room", Description = "房态图一览-查看", MenuKey = "roommap", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },

            new Permission { PermissionNumber = "roommanagement.view", PermissionName = "客房-查看", Module = "room", Description = "客房管理-查看", MenuKey = "roommanagement", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "roommanagement.create", PermissionName = "客房-新增", Module = "room", Description = "客房管理-新增", MenuKey = "roommanagement", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "roommanagement.update", PermissionName = "客房-编辑", Module = "room", Description = "客房管理-编辑", MenuKey = "roommanagement", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "roommanagement.delete", PermissionName = "客房-删除", Module = "room", Description = "客房管理-删除", MenuKey = "roommanagement", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },

            new Permission { PermissionNumber = "roomconfig.view", PermissionName = "客房配置-查看", Module = "room", Description = "客房配置-查看", MenuKey = "roomconfig", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "roomconfig.create", PermissionName = "客房配置-新增", Module = "room", Description = "客房配置-新增", MenuKey = "roomconfig", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "roomconfig.update", PermissionName = "客房配置-编辑", Module = "room", Description = "客房配置-编辑", MenuKey = "roomconfig", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "roomconfig.delete", PermissionName = "客房配置-删除", Module = "room", Description = "客房配置-删除", MenuKey = "roomconfig", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },

            // Customer management (客户管理)
            new Permission { PermissionNumber = "viplevel.view", PermissionName = "会员等级-查看", Module = "customer", Description = "会员等级规则-查看", MenuKey = "viplevel", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "viplevel.create", PermissionName = "会员等级-新增", Module = "customer", Description = "会员等级规则-新增", MenuKey = "viplevel", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "viplevel.update", PermissionName = "会员等级-编辑", Module = "customer", Description = "会员等级规则-编辑", MenuKey = "viplevel", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "viplevel.delete", PermissionName = "会员等级-删除", Module = "customer", Description = "会员等级规则-删除", MenuKey = "viplevel", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },

            new Permission { PermissionNumber = "customer.view", PermissionName = "客户-查看", Module = "customer", Description = "客户信息管理-查看", MenuKey = "customer", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "customer.create", PermissionName = "客户-新增", Module = "customer", Description = "客户信息管理-新增", MenuKey = "customer", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "customer.update", PermissionName = "客户-编辑", Module = "customer", Description = "客户信息管理-编辑", MenuKey = "customer", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "customer.delete", PermissionName = "客户-删除", Module = "customer", Description = "客户信息管理-删除", MenuKey = "customer", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },

            new Permission { PermissionNumber = "customerspend.view", PermissionName = "消费记录-查看", Module = "customer", Description = "客户消费账单-查看", MenuKey = "customerspend", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "customerspend.create", PermissionName = "消费记录-新增", Module = "customer", Description = "客户消费账单-新增", MenuKey = "customerspend", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "customerspend.update", PermissionName = "消费记录-编辑", Module = "customer", Description = "客户消费账单-编辑", MenuKey = "customerspend", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "customerspend.delete", PermissionName = "消费记录-删除", Module = "customer", Description = "客户消费账单-删除", MenuKey = "customerspend", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },

            new Permission { PermissionNumber = "customertype.view", PermissionName = "客户类型-查看", Module = "customer", Description = "客户类型管理-查看", MenuKey = "customertype", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "customertype.create", PermissionName = "客户类型-新增", Module = "customer", Description = "客户类型管理-新增", MenuKey = "customertype", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "customertype.update", PermissionName = "客户类型-编辑", Module = "customer", Description = "客户类型管理-编辑", MenuKey = "customertype", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "customertype.delete", PermissionName = "客户类型-删除", Module = "customer", Description = "客户类型管理-删除", MenuKey = "customertype", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },

            // Human resource (酒店人事管理)
            new Permission { PermissionNumber = "staffmanagement.view", PermissionName = "员工-查看", Module = "humanresource", Description = "员工管理-查看", MenuKey = "staffmanagement", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "staffmanagement.create", PermissionName = "员工-新增", Module = "humanresource", Description = "员工管理-新增", MenuKey = "staffmanagement", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "staffmanagement.update", PermissionName = "员工-编辑", Module = "humanresource", Description = "员工管理-编辑", MenuKey = "staffmanagement", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "staffmanagement.delete", PermissionName = "员工-删除", Module = "humanresource", Description = "员工管理-删除", MenuKey = "staffmanagement", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "staffmanagement.reset", PermissionName = "员工-重置密码", Module = "humanresource", Description = "员工管理-重置密码", MenuKey = "staffmanagement", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "staffmanagement.status", PermissionName = "员工-状态", Module = "humanresource", Description = "员工管理-状态", MenuKey = "staffmanagement", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },

            // Material management (酒店物资管理)
            new Permission { PermissionNumber = "goodsmanagement.view", PermissionName = "商品-查看", Module = "material", Description = "商品管理-查看", MenuKey = "goodsmanagement", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "goodsmanagement.create", PermissionName = "商品-新增", Module = "material", Description = "商品管理-新增", MenuKey = "goodsmanagement", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "goodsmanagement.update", PermissionName = "商品-编辑", Module = "material", Description = "商品管理-编辑", MenuKey = "goodsmanagement", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "goodsmanagement.delete", PermissionName = "商品-删除", Module = "material", Description = "商品管理-删除", MenuKey = "goodsmanagement", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },

            // Operation management (行为操作管理)
            new Permission { PermissionNumber = "operationlog.view", PermissionName = "操作日志-查看", Module = "operation", Description = "操作日志-查看", MenuKey = "operationlog", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "operationlog.delete", PermissionName = "操作日志-删除", Module = "operation", Description = "操作日志-删除", MenuKey = "operationlog", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },

            new Permission { PermissionNumber = "requestlog.view", PermissionName = "请求日志-查看", Module = "operation", Description = "请求日志-查看", MenuKey = "requestlog", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "requestlog.delete", PermissionName = "请求日志-删除", Module = "operation", Description = "请求日志-删除", MenuKey = "requestlog", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            
            // System management (系统管理)
            new Permission { PermissionNumber = "administratormanagement.view", PermissionName = "管理员-查看", Module = "system", Description = "管理员管理-查看", MenuKey = "administratormanagement", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "administratormanagement.create", PermissionName = "管理员-新增", Module = "system", Description = "管理员管理-新增", MenuKey = "administratormanagement", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "administratormanagement.update", PermissionName = "管理员-编辑", Module = "system", Description = "管理员管理-编辑", MenuKey = "administratormanagement", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "administratormanagement.delete", PermissionName = "管理员-删除", Module = "system", Description = "管理员管理-删除", MenuKey = "administratormanagement", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },

            new Permission { PermissionNumber = "menumanagement.view", PermissionName = "菜单-查看", Module = "system", Description = "菜单管理-查看", MenuKey = "menumanagement", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "menumanagement.create", PermissionName = "菜单-新增", Module = "system", Description = "菜单管理-新增", MenuKey = "menumanagement", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "menumanagement.update", PermissionName = "菜单-编辑", Module = "system", Description = "菜单管理-编辑", MenuKey = "menumanagement", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "menumanagement.delete", PermissionName = "菜单-删除", Module = "system", Description = "菜单管理-删除", MenuKey = "menumanagement", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },

            new Permission { PermissionNumber = "rolemanagement.view", PermissionName = "角色-查看", Module = "system", Description = "角色管理-查看", MenuKey = "rolemanagement", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "rolemanagement.create", PermissionName = "角色-新增", Module = "system", Description = "角色管理-新增", MenuKey = "rolemanagement", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "rolemanagement.update", PermissionName = "角色-编辑", Module = "system", Description = "角色管理-编辑", MenuKey = "rolemanagement", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "rolemanagement.delete", PermissionName = "角色-删除", Module = "system", Description = "角色管理-删除", MenuKey = "rolemanagement", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "rolemanagement.grant", PermissionName = "角色-授予权限", Module = "system", Description = "角色管理-授予权限", MenuKey = "rolemanagement", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },

            new Permission { PermissionNumber = "admintypemanagement.view", PermissionName = "管理员类型-查看", Module = "system", Description = "管理员类型管理-查看", MenuKey = "admintypemanagement", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "admintypemanagement.create", PermissionName = "管理员类型-新增", Module = "system", Description = "管理员类型管理-新增", MenuKey = "admintypemanagement", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "admintypemanagement.update", PermissionName = "管理员类型-编辑", Module = "system", Description = "管理员类型管理-编辑", MenuKey = "admintypemanagement", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "admintypemanagement.delete", PermissionName = "管理员类型-删除", Module = "system", Description = "管理员类型管理-删除", MenuKey = "admintypemanagement", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },

            // System management v2 (match API RequirePermission)
            new Permission { PermissionNumber = "system:role:list", PermissionName = "角色-列表/读取", Module = "system", Description = "角色管理-查询/读取", MenuKey = "rolemanagement", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "system:role:create", PermissionName = "角色-新增(API)", Module = "system", Description = "角色管理-新增（接口）", MenuKey = "rolemanagement", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "system:role:update", PermissionName = "角色-编辑(API)", Module = "system", Description = "角色管理-编辑（接口）", MenuKey = "rolemanagement", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "system:role:delete", PermissionName = "角色-删除(API)", Module = "system", Description = "角色管理-删除（接口）", MenuKey = "rolemanagement", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "system:role:grant", PermissionName = "角色-授予权限(API)", Module = "system", Description = "角色管理-授予权限/关联管理员（接口）", MenuKey = "rolemanagement", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },

            new Permission { PermissionNumber = "system:admin:list", PermissionName = "管理员-列表/读取", Module = "system", Description = "管理员管理-查询/读取", MenuKey = "administratormanagement", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "system:admin:create", PermissionName = "管理员-新增(API)", Module = "system", Description = "管理员管理-新增（接口）", MenuKey = "administratormanagement", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "system:admin:update", PermissionName = "管理员-编辑(API)", Module = "system", Description = "管理员管理-编辑（接口）", MenuKey = "administratormanagement", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "system:admin:delete", PermissionName = "管理员-删除(API)", Module = "system", Description = "管理员管理-删除（接口）", MenuKey = "administratormanagement", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },

            new Permission { PermissionNumber = "system:admintype:list", PermissionName = "管理员类型-列表/读取", Module = "system", Description = "管理员类型管理-查询/读取", MenuKey = "admintypemanagement", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "system:admintype:create", PermissionName = "管理员类型-新增(API)", Module = "system", Description = "管理员类型管理-新增（接口）", MenuKey = "admintypemanagement", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "system:admintype:update", PermissionName = "管理员类型-编辑(API)", Module = "system", Description = "管理员类型管理-编辑（接口）", MenuKey = "admintypemanagement", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "system:admintype:delete", PermissionName = "管理员类型-删除(API)", Module = "system", Description = "管理员类型管理-删除（接口）", MenuKey = "admintypemanagement", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },

            new Permission { PermissionNumber = "system:user:assign", PermissionName = "用户-分配角色/权限", Module = "system", Description = "管理员-分配角色/直接权限（接口）", MenuKey = "administratormanagement", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "system:user:assign.view", PermissionName = "用户-读取角色/权限", Module = "system", Description = "管理员-读取角色/直接权限（接口）", MenuKey = "administratormanagement", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            // Misc pages
            new Permission { PermissionNumber = "dashboard.view", PermissionName = "仪表盘-查看", Module = "home", Description = "仪表盘-查看", MenuKey = "dashboard", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now },
            new Permission { PermissionNumber = "home.view", PermissionName = "首页-查看", Module = "home", Description = "首页-查看", MenuKey = "home", ParentNumber = null, IsDelete = 0, DataInsUsr = "System", DataInsDate = DateTime.Now }
        };

        public Type[] EntityTypes => entityTypes;

        public List<object> GetEntityDatas() => entityDatas;
    }
}
