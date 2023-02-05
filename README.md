# Solidworks Extension to implement automation of reading a 3D model, creating drawing and exporting the BOM with picture to an excel sheet and create parts and Assembly.

### _Developer_
---
Nikhil Chudasma, DhanushKumar Komalapura and Prashanth H N Rao

## _Objective_
Automation is a process that permits applications that are written in languages such as Visual C# .NET to programmatically control other applications. In this project we are automating process in Solidworks CAD tool and Excel. It permits to perform below actions:
1.	Creation of individual parts and their assembly in Solidworks CAD 
2.	Creation of drawing with various views and BOM table from an assembly in Solidworks CAD
3.	Export of the Assembly BOM to Excel file along with picture of each individual part. 

Thus the actions that one can perform manually through the CAD and excel user interface can be performed programmatically by using Automation. We have demonstrated the possibility of above listed features. But there is room for implementing similar automation for many such time consuming processes that can be automated programmatically.

## _Development_
* IDE: Visual Studio 2022
* Language : C#
* SDK : .NET 4.3 

## _Dependencies_
The project utilizes the following packages to develop the extension. The external library facilitates code reduction and ease of implementation and scaling of the application.
* Xarial.XCad.SolidWorks.0.7.5
* Xarial.XCad.SolidWorks.Interops.0.3.0
* Xarial.XCad.Toolkit.0.7.5
* EPPlus.4.5.2
* EPPlus.Interfaces.6.1.1
* EPPlus.System.Drawing.6.1.1
* Microsoft.IO.RecyclableMemoryStream.1.4.1
* ObjectListView
* Microsoft.Office.Interop.Excel

SolidWorks API internal references.
* SolidWorks.Interop.sldworks
* SolidWorks.Interop.swconst
* SolidWorks.Interop.swpublished
* SolidWorks.Interop.swdocumentmgr

## _Procedure to add the plugin_   
1. Unzip the SwExtension.zip file in 'C:' drive
2. Run as Administrator Register.bat file
3. A command prompt will show Types registered successfully
4. Press any key to exit the command prompt and complete registeration
5. In SolidWorks, the plugin visible under Tools.
6. To add as Tab, right click on Tab and select SwApp-Project
7. Paste the SwApp folder in 'D:' drive which has all templates.

## _Procedure to remove the plugin_ 
1. Go to the SwExtension folder created previously.
2. Run as Administrator Unregister.bat file

## _Procedure to use the extension_  
To create the parts and assembly of Bush
1. Click on Create Bush UI button in the SwApp-Project Tab
2. Enter the required dimensions and Click Model Parts
3. After the Dialog box appears saying Parts Generated. Proceed with Assembly
4. Click on Build assembly and the assembly file will be generated and saved in D:\SwApp

To create drawing and export to excel
1. Click on Assembly Properties button to select the required parts to be exported to excel and click OK button
2. Click on Generate Drawing button to get the drawing file 
3. Click on Export BOM button to export the BOM to excel file
4. All files will be in the same directory where the assembly is generated


