global using System.ComponentModel.DataAnnotations;
global using System.Text.Json;

global using Microsoft.EntityFrameworkCore;
global using Microsoft.AspNetCore.Http.HttpResults;
global using Microsoft.AspNetCore.Mvc;

global using Carter;
global using AutoMapper;
global using Asp.Versioning;
global using Asp.Versioning.Builder;

global using TaskManagementApp.Services.TasksApi.Models;
global using TaskManagementApp.Services.TasksApi.Models.Dtos;
global using TaskManagementApp.Services.TasksApi.Data;
global using TaskManagementApp.Services.TasksApi.Repositories;
global using TaskManagementApp.Services.TasksApi.Repositories.IRepositories;

global using TaskManagementApp.SharedLibraries.BaseSharedLibraries.Repositories;
global using TaskManagementApp.SharedLibraries.BaseSharedLibraries.Repositories.IRepositories;
global using TaskManagementApp.SharedLibraries.BaseSharedLibraries.SharedDtos;
