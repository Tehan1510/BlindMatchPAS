# Blind-Match PAS (Project Allocation System) 🎓

A secure, unbiased ASP.NET Core MVC web application designed to facilitate the matching of academic research projects between students and university supervisors. 

The core philosophy of this platform is **"Blind Review"**. Supervisors evaluate project proposals purely on academic merit, research area, and technical viability. Student identities remain strictly hidden until the supervisor officially accepts the project.

## 🚀 Features

* **Role-Based Smart Routing:** Custom login logic that automatically routes users to their specific dashboards (Student, Supervisor, Admin) based on their official `@nsbm.ac.lk` email domain.
* **Foolproof Registration:** A custom split-input email form ensures users register with perfectly formatted university credentials, eliminating routing errors.
* **Supervisor Blind-Match Dashboard:** A sleek, dynamic interface allowing supervisors to view incoming proposals without bias.
* **Dynamic Expertise Filtering:** Supervisors can instantly filter proposals by domains such as AI, Software Engineering, Java, and C#.
* **3-Step Match Flow:** 1. Browse proposals on the Dashboard.
  2. Read the full abstract in the Details view (Identity Hidden).
  3. Accept via the Confirmation warning screen (Identity Revealed).
* **Settings & Preferences:** Dedicated profile management for supervisors to set maximum student quotas and update research preferences.

## 🛠️ Tech Stack

* **Framework:** ASP.NET Core MVC
* **Language:** C#
* **Frontend:** HTML5, CSS3, Razor Views (`.cshtml`)
* **Styling & UI:** Bootstrap 5, Bootstrap Icons
* **Architecture:** Model-View-Controller (MVC)

## 📂 Project Structure

* **`/Controllers`**: Contains the C# routing and business logic (`HomeController`, `AccountController`, `SupervisorController`).
* **`/Models`**: Contains the data structures (e.g., `ProjectViewModel`).
* **`/Views`**: Contains the Razor HTML templates, organized by controller.
* **`/wwwroot`**: Houses static web assets like the custom `site.css` and Bootstrap libraries.

## ⚙️ How to Run Locally

1. Ensure you have the [.NET SDK](https://dotnet.microsoft.com/download) installed on your machine.
2. Clone or download this repository.
3. Open a terminal/command prompt and navigate to the root directory of the project.
4. Run the following commands:
   ```bash
   dotnet restore
   dotnet build
   dotnet run
