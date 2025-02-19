global using System.Text;
global using System.Text.Json;
global using System.Text.Json.Serialization;
global using System.Diagnostics;
global using System.ComponentModel;
global using System.ComponentModel.DataAnnotations;
global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Cryptography.X509Certificates;
global using System.Security.Claims;

global using Microsoft.AspNetCore.Authentication.Cookies;
global using Microsoft.AspNetCore.Authentication;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Authorization;

global using NLog;
global using NLog.Web;

global using TaskManagementApp.Frontends.Web.Services;
global using TaskManagementApp.Frontends.Web.Services.IServices;
global using TaskManagementApp.Frontends.Web.AppProperties;
global using TaskManagementApp.Frontends.Web.Configurations;
global using TaskManagementApp.Frontends.Web.Models;
global using TaskManagementApp.Frontends.Web.Models.Dtos;
global using TaskManagementApp.Frontends.Web.Models.Enums;
