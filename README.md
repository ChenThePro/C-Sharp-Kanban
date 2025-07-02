# 🧱 Kanban Board Management System

## 📖 Overview

A full-stack Kanban Board Management System developed in C# using the N-tier architecture, MVVM design pattern, and WPF for GUI. The application supports user registration, login, board and task management, and is backed by a persistent SQLite database. Logging, input validation, and error handling ensure system robustness. Development spanned multiple milestones, culminating in a fully functional and testable project.

---

## 🧩 Features

### 👤 User Management

- Register with email and secure password
- Login and logout functionality
- Unique email enforcement
- Password validation (6–20 chars, 1 uppercase, 1 lowercase, 1 digit)

### 🗂️ Board Management

- Create, delete, join, and leave boards
- Each board has three columns: Backlog, In Progress, Done
- Board ownership transfer
- Prevent duplicate board names per user
- Task limits per column

### ✅ Task Management

- Add task with title, description, due date, assignee
- Tasks can be moved between columns by assignee only (Backlog → In Progress → Done)
- Tasks editable by assignee (excluding creation time)
- View all tasks assigned to user across boards
- Unassigned tasks can be reassigned

### 🖥️ Graphical User Interface (WPF)

- Clean, intuitive UI built with MVVM pattern
- Registration and Login window
- User dashboard: list of boards, create/delete boards
- Board view: see columns, tasks, and members
- Minimal clutter and usability-focused design

### 💾 Persistence

- SQLite database (`kanban.db`)
- Persists all users, boards, columns, and tasks
- Restores state on application start

### 🧪 Testing

- Unit tests (NUnit) for core business logic (Board class)
- Acceptance tests simulate user flows via the service layer
- Manual GUI tests validate usability and error feedback

### 🪵 Logging

- Uses `log4net` for structured logging
- Logs system events (e.g. registration, task creation) and errors
- Logs are categorized by severity and exclude verbose stack traces

### 🔌 Service Layer (JSON Interface)

- Public service layer returns/accepts JSON for integration
- Methods support both primitive and JSON string arguments

---

## 📂 Project Structure

```
Solution/
├── Backend/             # Core logic (ServiceLayer, BusinessLayer, DataAccessLayer)
├── BackendTests/        # Console project for black-box acceptance tests
├── BackendUnitTests/    # NUnit tests for unit-level backend testing
├── Frontend/            # WPF MVVM application (GUI)
├── kanban.db            # SQLite database with persistent data
└── documents/           # Design documents (UML diagrams, notes)
```

---

## 🧪 Testing Strategy

- **Acceptance Testing (BackendTests):**
  - Uses ServiceLayer interfaces
  - Covers success and failure cases for all user actions

- **Unit Testing (BackendUnitTests):**
  - Targets Board class functionality (e.g. task limits, transitions)
  - Uses 3A pattern and follows naming conventions

- **Manual Testing:**
  - GUI interactions verified (clicks, errors, navigation)
  - Usability checks: button text, layout, field sizes

---

## 🧑‍💻 Technologies Used

- **Language:** C# (.NET 6)
- **GUI Framework:** WPF
- **Architecture:** N-Tier + MVVM
- **Database:** SQLite
- **Testing:** NUnit
- **Logging:** log4net
- **Version Control:** Git + GitHub (GitHub Classroom)

---

## 📝 Sample Data

On startup, the system loads the following default data:

- **User:** `mail@mail.com` / `Password1`
- **Boards:**
  - `board1` — has 3 tasks (1 per column)
  - `board2` — empty

---

## 🚀 How to Run

1. Ensure you have [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) installed
2. Open solution in Visual Studio or via CLI
3. Build the solution
4. Run the `Frontend` project

The application loads from `kanban.db` located at the root directory.

---

## 📂 Database Location

The database must be located at:

```
<ProjectRoot>/bin/Debug/net6.0/kanban.db
```

Use a relative path to access it in your code.

---

## 🧑‍🤝‍🧑 Contributors

- [Your Name Here]  
  Lead developer: implemented core logic, GUI, testing, and database integration across all milestones

---

## 📄 License

This project is intended for educational use only as part of a university course assignment.
