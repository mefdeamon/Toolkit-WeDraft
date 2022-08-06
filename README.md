# Toolkit-WeDraft
Build our own WPF Control Style Library

构建属于我们自己的WPF控件样式库

## 概要（Summary）

**First Draft **是面向 Windows Presentation Foundation (WPF)的一套自定义样式库。由 **Meiliyong** 自主设计开发完成，最初基于.NET Core 3.1开发，如需其他框架版本请下载源码重新编译，源码地址：[https://github.com/mefdeamon/Toolkit-WeDraft](https://github.com/mefdeamon/Toolkit-WeDraft)。

## 快速使用（Quick Start）

1. App.xaml中引入资源

   ```xaml
           <ResourceDictionary>
               <ResourceDictionary.MergedDictionaries>
                   <ResourceDictionary Source="pack://application:,,,/FirstDraft;component/Themes/Ui.xaml"/>
               </ResourceDictionary.MergedDictionaries>
               <data:ServiceLocator x:Key="Locator"/>
           </ResourceDictionary>
   ```

   示例（WPF应用程序名FirstDraft.ApplyDemo）：

   ```xaml
   <Application x:Class="FirstDraft.ApplyDemo.App"
                xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
                xmlns:local="clr-namespace:FirstDraft.ApplyDemo"
                xmlns:data="clr-namespace:FirstDraft.ApplyDemo.Data"
                StartupUri="MainWindow.xaml">
       <Application.Resources>
           <ResourceDictionary>
               <ResourceDictionary.MergedDictionaries>
                   <ResourceDictionary Source="pack://application:,,,/FirstDraft;component/Themes/Ui.xaml"/>
               </ResourceDictionary.MergedDictionaries>
           </ResourceDictionary>
       </Application.Resources>
   </Application>
   ```

2. 使用自定义控件时命名空间引入

   ```xaml
                xmlns:fdcontrols="clr-namespace:FirstDraft.Controls;assembly=FirstDraft"
   ```

   示例

   ```xaml
   <UserControl x:Class="FirstDraft.ApplyDemo.Views.ApplyIconButtonView"
                xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
                xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006" 
                xmlns:d="http://schemas.microsoft.com/expression/blend/2008" 
                xmlns:local="clr-namespace:FirstDraft.ApplyDemo.Views"
                xmlns:fdcontrols="clr-namespace:FirstDraft.Controls;assembly=FirstDraft"
                mc:Ignorable="d" 
                d:DesignHeight="450" d:DesignWidth="800">
   	<WrapPanel  Margin="5">
       	<fdcontrols:IconButton Margin="5" IconData="{Binding home_line, Source={StaticResource IconSet}}"/>
           <fdcontrols:IconButton Margin="5" IconData="{Binding home_fill, Source={StaticResource IconSet}}" />
           <fdcontrols:IconButton Margin="5" IconData="{Binding record_on, Source={StaticResource IconSet}}" Content="录制"/>
           <fdcontrols:IconButton Margin="5" IconData="{Binding settings_line, Source={StaticResource IconSet}}" Content="设置" />
    	</WrapPanel>
   </UserControl>
   ```

3. 样式资源使用

   1. 图标资源

      ```xaml
      <fdcontrols:IconButton Margin="5" IconData="{Binding home_line, Source={StaticResource IconSet}}"/>
      ```

   2. 样式资源

      ```xaml
      <fdcontrols:IconButton Style="{StaticResource VerticalTextIconButton}" IconData="{Binding user_fill, Source={StaticResource IconSet}}" Content="用户"/>
      ```

## 详细示例（Demo)

- 示例的应用程序可以直接下载[MUI release](https://github.com/firstfloorsoftware/mui/releases)。

- 完整的源码地址：[https://github.com/mefdeamon/Toolkit-WeDraft](https://github.com/mefdeamon/Toolkit-WeDraft)。
- 开发过程视频：[Bilibili视频](https://www.bilibili.com/video/BV1AA411J7vZ)。