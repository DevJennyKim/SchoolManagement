# School Management System
This is a small C# project for managing students, teachers, and section in a school. The project is built using .NET and MSSQL Server.

## Prerequisites
Make sure you have the following installed:
1. [Visual Studio](https://visualstudio.microsoft.com/) (Community Edition or higher)
   - Workload: `.NET desktop development`
   - Workload: `ASP.NET and web development` (if using ASP.NET)
2. [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (Local or Express edition)
3. .NET SDK (if not bundled with Visual Studio)
   - Version: `.NET 6.0` or your project's version.


## Database Setup
To run this project, you need to configure the database connection. Follow the steps below:

### Step 1: Restore the Database
1. Open **SQL Server Management Studio (SSMS)**.
2. Right-click on **Databases** in the Object Explorer and select **Restore Database...**.
3. Choose **Device**, click the **...** button, and locate the `Database/schooldb.bak` file included in this repository.
4. Follow the prompts to restore the database.

### Step 2: Configure the Connection String
The database connection string is located in the `Database.config.example` file in the main project folder. 

1. Rename `Database.config.example` to `Database.config`.
2. Open `Database.config` and update the connection string to match your SQL Server instance.

--------------------------------------------------------------------

## Getting Started

### Step 1: Clone the Repository
```bash
git clone https://github.com/your-username/your-repo-name.git
cd your-repo-name
```

### Step 2:Install Dependencies
Before running the project, you need to make sure all required dependencies are installed. This is typically done via the NuGet package manager.
Open the solution file (SchoolManagement.sln) in Visual Studio.
Visual Studio should automatically restore any required NuGet packages. If it doesn't, you can manually restore them by navigating to Tools > NuGet Package Manager > Restore NuGet Packages.

### Step 3: Configure the Database
Follow the steps in the Database Setup section to restore and configure your database connection.

### Step 4: Build the Project
In Visual Studio, press Ctrl + Shift + B or go to Build > Build Solution to compile the project.
Step 5: Run the Project
Press F5 or go to Debug > Start Debugging to run the application.

The application should now be running, and you can access it via the default web browser or the Visual Studio development server.





