<?xml version="1.0" encoding="utf-8"?>
<Project DefaultTargets="Build" ToolsVersion="4.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <PropertyGroup>
    <Configuration Condition=" '$(Configuration)' == '' ">Debug</Configuration>
    <Platform Condition=" '$(Platform)' == '' ">x86</Platform>
    <ProjectGuid>{2DCE2EF0-32B8-4F2B-B6B7-838A5ACA337B}</ProjectGuid>
    <OutputType>Exe</OutputType>
    <RootNamespace>NOC</RootNamespace>
    <AssemblyName>NOC</AssemblyName>
    <TargetFrameworkVersion>v4.5</TargetFrameworkVersion>
  </PropertyGroup>
  <PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Debug|x86' ">
    <DebugSymbols>true</DebugSymbols>
    <DebugType>full</DebugType>
    <Optimize>false</Optimize>
    <OutputPath>bin\Debug</OutputPath>
    <DefineConstants>DEBUG;</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
    <Externalconsole>true</Externalconsole>
    <PlatformTarget>x86</PlatformTarget>
  </PropertyGroup>
  <PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Release|x86' ">
    <DebugType>full</DebugType>
    <Optimize>true</Optimize>
    <OutputPath>bin\Release</OutputPath>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
    <Externalconsole>true</Externalconsole>
    <PlatformTarget>x86</PlatformTarget>
  </PropertyGroup>
  <ItemGroup>
    <Reference Include="System" />
    <Reference Include="MySql.Data">
      <HintPath>bin\Debug\MySql.Data.dll</HintPath>
    </Reference>
    <Reference Include="MySql.Data.Entity">
      <HintPath>bin\Debug\MySql.Data.Entity.dll</HintPath>
    </Reference>
    <Reference Include="MySql.Data.Entity.EF6">
      <HintPath>bin\Debug\MySql.Data.Entity.EF6.dll</HintPath>
    </Reference>
    <Reference Include="MySql.Fabric.Plugin">
      <HintPath>bin\Debug\MySql.Fabric.Plugin.dll</HintPath>
    </Reference>
    <Reference Include="MySql.Web">
      <HintPath>bin\Debug\MySql.Web.dll</HintPath>
    </Reference>
    <Reference Include="System.Data" />
  </ItemGroup>
  <ItemGroup>
    <Compile Include="Program.cs" />
    <Compile Include="Properties\AssemblyInfo.cs" />
    <Compile Include="meclary.cs" />
    <Compile Include="mequery.cs" />
    <Compile Include="MySQLQueries.cs" />
    <Compile Include="LoadMySQLData.cs" />
    <Compile Include="MySQLGetData.cs" />
  </ItemGroup>
  <Import Project="$(MSBuildBinPath)\Microsoft.CSharp.targets" />
</Project>