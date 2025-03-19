# 目的:SSO工具統一入口網站
該專案系統登入使用
有使用者/角色/使用者&角色(一對多)
無權限管理(另外架設Permissions微服務)

## 與Azure AD做結合
id_token 範例內容（解碼後的 JWT）： id_token 是一個 JSON Web Token，包含有關使用者的詳細身份資料。例如，解碼後的 id_token 可能如下所示：
{
  "aud": "client-id",
  "iss": "https://login.microsoftonline.com/{tenant-id}/v2.0",
  "iat": 1616178400,
  "exp": 1616182000,
  "sub": "12345",
  "name": "John Doe",
  "preferred_username": "john.doe@domain.com",
  "email": "john.doe@domain.com",
  "oid": "user-object-id",
  "upn": "john.doe@domain.com",
  "scp": "openid profile email"
}
將email取代為Users中的userid

## 工具使用
VSCode/MSSQL

## 安裝步驟
step 1.建立 ASP.NET Core API 專案
dotnet new webapi -n SSO
cd SSO

step2.安裝必要的 NuGet 套件
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.IdentityModel.Tokens
dotnet add package System.IdentityModel.Tokens.Jwt

## 資料庫設定
appsettings.json檔案
 "ConnectionStrings": {
    "DefaultConnection": "Server=127.0.0.1;Database=SSO;User Id=sa;Password=1111;TrustServerCertificate=True;"
  },

Table schema於schema資料夾中的table.sql

## 忘記密碼
目前直接回傳之後可改用email傳遞


## 前端
建立reactjs
npm create vite@latest frontend -- --template react-ts
cd frontend
npm install

安裝MUI
npm install @mui/material @emotion/react @emotion/styled @mui/icons-material axios

安裝react-router
 npm install react-router-dom@latest @types/react-router-dom

 安裝 jwt-decode
 npm install jwt-decode