use SSO

-- 🔹 Users Table
CREATE TABLE [dbo].[Users] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [UserId] NVARCHAR(255) NOT NULL UNIQUE,  -- ✅ 唯一用戶識別
    [SystemName] VARCHAR(50) NOT NULL,  -- ✅ 用來區分不同系統
    [Username] NVARCHAR(255) NOT NULL UNIQUE, 
    [PasswordHash] NVARCHAR(255) NOT NULL,
    [CreatedAt] DATETIME DEFAULT GETDATE(),
    [CreatedBy] INT NULL,
    [UpdatedAt] DATETIME DEFAULT GETDATE(),
    [ModifiedBy] INT NULL,
    [IsDeleted] BIT DEFAULT 0,  -- ✅ 軟刪除
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([UserId] ASC),
);


-- 🔹 Roles Table
CREATE TABLE [dbo].[Roles] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [SystemName] VARCHAR(50) NULL,
    [RoleName] NVARCHAR(255) NOT NULL,
    [Description] NVARCHAR(MAX) NULL,
    [CreatedAt] DATETIME DEFAULT GETDATE(),
    [CreatedBy] INT NULL,
    [UpdatedAt] DATETIME DEFAULT GETDATE(),
    [ModifiedBy] INT NULL,
    [IsDeleted] BIT DEFAULT 0,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

-- 🔹 UserRoles Table (多對多關係)
CREATE TABLE [dbo].[UserRoles] (
    [Username] NVARCHAR(255) NOT NULL,
    [RoleId] INT NOT NULL,
    [CreatedAt] DATETIME DEFAULT GETDATE(),
    [CreatedBy] INT NULL,
    [UpdatedAt] DATETIME DEFAULT GETDATE(),
    [ModifiedBy] INT NULL,
    [IsDeleted] BIT DEFAULT 0, 
    PRIMARY KEY ([Username], [RoleId]),
);

-- 🔹 RolePermissions Table (角色權限 - RBAC)
CREATE TABLE [dbo].[RolePermissions] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [RoleId] INT NOT NULL,
    [PermissionKey] VARCHAR(100) NOT NULL,  -- 例: "View_Report"
    [PermissionType] VARCHAR(10) NOT NULL CHECK (PermissionType IN ('Page', 'Menu', 'Field', 'API')), 
    [CanAccess] BIT NOT NULL,  -- 1: 允許, 0: 禁止
    [CreatedAt] DATETIME DEFAULT GETDATE(),
    [CreatedBy] INT NULL,
    [UpdatedAt] DATETIME DEFAULT GETDATE(),
    [ModifiedBy] INT NULL,
    PRIMARY KEY ([Id]),
);

-- 🔹 AttributePermissions Table (屬性權限 - ABAC)
CREATE TABLE [dbo].[AttributePermissions] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [SubjectType] VARCHAR(10) NOT NULL CHECK (SubjectType IN ('User', 'Role')), 
    [SubjectId] INT NOT NULL,  -- RoleId 或 UserId
    [PermissionKey] VARCHAR(100) NOT NULL,  -- 例: "Edit_Report"
    [PermissionType] VARCHAR(10) NOT NULL CHECK (PermissionType IN ('Page', 'Menu', 'Field', 'API')), 
    [ResourceType] VARCHAR(100) NULL,  -- 例: "Report", "Customer"
    [ResourceId] NVARCHAR(255) NULL, 
    [EnvironmentKey] VARCHAR(100) NULL,  
    [EnvironmentValue] NVARCHAR(255) NULL,  
    [CanAccess] BIT NOT NULL,  
    [CreatedAt] DATETIME DEFAULT GETDATE(),
    [CreatedBy] INT NULL,
    [UpdatedAt] DATETIME DEFAULT GETDATE(),
    [ModifiedBy] INT NULL,
    PRIMARY KEY ([Id])
);
