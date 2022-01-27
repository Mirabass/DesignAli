# Design Ali
Complex system for product management

![image](https://user-images.githubusercontent.com/37962262/151333740-9e664616-5c79-4c2f-bf58-03401fc71aa9.png)

This self-developed product has two deeper goals.
The first is to publicly present my skills. If there is a company that is interested in me, this project can serve as my portfolio of experience.

The second goal is the practical use of the product.
In the first place, it is to serve as a product manager for a small business of member of my family. The goal is to allow employees who are assigned roles to log on. They will be able to work with materials and products, enter suppliers and customers, create delivery notes and invoices. Everything will be recorded and over time statistics will be generated.
Secondly, the system can grow into an e-shop.

In the solution of this application you will find several projects. I no longer develop some (WPF), but I kept them as my portfolio.
Currently developed projects are DAERP:
DAERP.Web - ASP.NET 5 base project
DAERP.DAL - library including access layer data
DAERP.BL - library including Business logic.
The following is used in these projects:
ASP NET 5 MVC,
Microsoft Identity, Dependency injection,
Entity framework Core,
Automapper,
Bootstrap 4,
Font awesome

Languages: C#, HTML, JS, Jquery, Ajax




History - Old state:

In its current state, this Solution is almost completely written according to the TimCoRetailManager course from the IAmTimCorey channel. Not copied. I went through the course and wrote the code myself (can be traced in commits). I tried not only to blindly describe everything that is written in this Solution, but also to understand it. The course was used mainly to create authorizations with roles, but the product will continue to be adapted to the needs of Design Ali.

Application structure:
BackEnd:
1. DAData - Ready MSSQL database (Net Framework 4.8, SQL server 2016).
2. DADataManager.Library - Library .Net Standard 2.1, Using Dapper and Procedures allows communication with SQL database. Provides DataAccess.
3. DAApi - ASP .NET 5 MVC WebAPI with authentication and authorization using JWT token. Contains Controllers that are connected to DADataMaganer.Library.
FrontEnd:
4. DADesktopUI.Library - .NET Library 5. Arranges communication with API.
5. DADesktopUI - .NET 5 WPF MVVM using Caliburn Micro.
